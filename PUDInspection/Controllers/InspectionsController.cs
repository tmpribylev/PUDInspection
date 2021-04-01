using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUDInspection.Data;
using PUDInspection.Models;
using PUDInspection.ViewModels;
using FluentScheduler;
using Microsoft.Extensions.Configuration;

namespace PUDInspection.Controllers
{
    [Authorize(Roles = "admin,moderator")]
    public class InspectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectionsController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace).ToListAsync();
            var inspections = from insp in all_insp
                              where insp.InspectionSpace != null && insp.InspectionSpace.Id == id 
                              select insp;

            if (inspections == null)
            {
                return NotFound();
            }
            TempData["InspSpaceID"] = id;
            return View(inspections);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace)
                                                     .Include(i => i.CriteriaList)
                                                     .Include(i => i.UserList)
                                                     .Include(i => i.Validations)
                                                     .Include(i => i.PUDList)
                                                     .ToListAsync();
            var inspection = all_insp.Find(i => i.Id == id);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            if (inspection == null)
            {
                return NotFound();
            }

            if (inspection.CriteriaList == null)
            {
                inspection.CriteriaList = new List<CheckVsCriteria>();
            }

            DetailsInspectionViewModel model = new DetailsInspectionViewModel()
            {
                InspectionId = inspection.Id,
                SpaceId = inspection.InspectionSpace.Id,
                InspectionName = inspection.Name,
                StartDate = inspection.StartDate,
                EndDate = inspection.EndDate,
                IterationNumber = inspection.IterationNumber,
                CurrentIteration = inspection.CurrentIteration,
                Closed = inspection.Closed,
                Hunt = inspection.Hunt,
                PUDCount = inspection.PUDList is null ? 0 : inspection.PUDList.Count,
                Criterias = inspection.CriteriaList,
                Users = new List<string>(),
                Validations = new List<string>(),
                CurrentCheckPUDCount = 0
            };

            foreach (ApplicationUser user in inspection.UserList)
            {
                model.Users.Add(user.RealName);
            }

            foreach (var valudation in inspection.Validations)
            {
                model.Validations.Add(valudation.Name);
            }

            if (inspection.CriteriaList.Count != 0 && inspection.CriteriaList[0].CheckResults != null)
            {
                foreach (var result in inspection.CriteriaList[0].CheckResults)
                {
                    if (result.Iteration == model.CurrentIteration)
                    {
                        model.CurrentCheckPUDCount++;
                    }
                }
            }

            return View(model);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateInspectionViewModel model = new CreateInspectionViewModel()
            {
                SpaceId = (int)id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInspectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Inspection inspection = new Inspection()
                {
                    Name = model.InspectionName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IterationNumber = model.IterationNumber,
                    InspectionSpace = await _context.InspectionSpaces
                    .FirstOrDefaultAsync(m => m.Id == model.SpaceId)
                };

                _context.Add(inspection);
                await _context.SaveChangesAsync();

                if (model.FilePUD != null)
                {
                    inspection = await _context.Inspections
                    .FirstOrDefaultAsync(item => item.Name == model.InspectionName
                                                 && item.InspectionSpace.Id == model.SpaceId
                                                 && item.StartDate == model.StartDate
                                                 && item.EndDate == model.EndDate
                                                 && item.IterationNumber == model.IterationNumber);

                    FileReader fileReader = new FileReader(_context);
                    await fileReader.UploadPUDToDatabase(model.FilePUD, inspection);
                }

                return RedirectToAction("Index", "Inspections", new { id = model.SpaceId });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace).Include(i => i.CriteriaList).Include(i => i.UserList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == id);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            if (inspection == null)
            {
                return NotFound();
            }

            EditInspectionViewModel model = new EditInspectionViewModel()
            {
                InspectionId = inspection.Id,
                SpaceId = inspection.InspectionSpace.Id,
                InspectionName = inspection.Name,
                StartDate = inspection.StartDate,
                EndDate = inspection.EndDate,
                IterationNumber = inspection.IterationNumber,
                Criterias = new List<Criteria>(),
                Users = new List<string>()
            };

            foreach (CheckVsCriteria cvc in inspection.CriteriaList)
            {
                model.Criterias.Add(cvc.Criteria);
            }

            foreach (ApplicationUser user in inspection.UserList)
            {
                model.Users.Add(user.RealName);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInspectionViewModel model)
        {
            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == model.InspectionId);

            if (inspection == null)
            {
                ViewBag.ErrorMessage = $"Проверки с ID = {model.InspectionId} не найдены";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    inspection.Name = model.InspectionName;
                    inspection.StartDate = model.StartDate;
                    inspection.EndDate = model.EndDate;
                    inspection.IterationNumber = model.IterationNumber;

                    try
                    {
                        _context.Update(inspection);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InspectionExists(inspection.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (model.FilePUD != null)
                    {
                        inspection = await _context.Inspections
                        .FirstOrDefaultAsync(item => item.Name == model.InspectionName
                                                     && item.InspectionSpace.Id == model.SpaceId
                                                     && item.StartDate == model.StartDate
                                                     && item.EndDate == model.EndDate
                                                     && item.IterationNumber == model.IterationNumber);

                        FileReader fileReader = new FileReader(_context);
                        await fileReader.UploadPUDToDatabase(model.FilePUD, inspection);
                    }

                    return RedirectToAction("Index", "Inspections", new { id = model.SpaceId });
                }

                return View(model);
            }
        }

        public async Task<IActionResult> EditUsersInInspection(int inspId)
        {
            ViewBag.inspId = inspId;

            var all_insp = await _context.Inspections.Include(i => i.UserList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == inspId);

            if (inspection == null)
            {
                ViewBag.ErrorMessage = $"Проверки с ID = {inspId} не найдены";
                return View("NotFound");
            }

            if (inspection.UserList == null)
            {
                inspection.UserList = new List<ApplicationUser>();
            }

            var model = new List<UsersInspectionViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, "inspector"))
                {
                    var usersInspectionViewModel = new UsersInspectionViewModel
                    {
                        UserId = user.Id,
                        UserName = user.RealName
                    };

                    if (inspection.UserList.Contains(user))
                    {
                        usersInspectionViewModel.IsSelected = true;
                    }
                    else
                    {
                        usersInspectionViewModel.IsSelected = false;
                    }

                    model.Add(usersInspectionViewModel);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInInspection(List<UsersInspectionViewModel> model, int inspId)
        {
            var all_insp = await _context.Inspections.Include(i => i.UserList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == inspId);

            if (inspection == null)
            {
                ViewBag.ErrorMessage = $"Проверки с ID = {inspId} не найдены";
                return View("NotFound");
            }

            if (inspection.UserList == null)
            {
                inspection.UserList = new List<ApplicationUser>();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                if (model[i].IsSelected && !(inspection.UserList.Contains(user)))
                {
                    inspection.UserList.Add(user);
                }
                else if (!model[i].IsSelected && inspection.UserList.Contains(user))
                {
                    inspection.UserList.Remove(user);
                }
                else
                {
                    continue;
                }
            }

            try
            {
                _context.Update(inspection);
                await _context.SaveChangesAsync();
                var registry = new Registry();
                registry.Schedule(() => AllocatePUDsPerInspectors(inspId)).ToRunNow();
                JobManager.Initialize(registry);
                //await this.AllocatePUDsPerInspectors(inspId);
                // RedirectToAction("AllocatePUDsPerInspectors", new { inspId = inspId });
                //return RedirectToAction("AllocatePUDsPerInspectors", new { inspId = inspId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(inspection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Edit", new { id = inspId });
        }

        public async Task<IActionResult> EditCriteriasInInspection(int inspId)
        {
            ViewBag.inspId = inspId;

            var all_insp = await _context.Inspections.Include(i => i.CriteriaList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == inspId);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            var all_criteria = await _context.Criterias.ToListAsync();

            if (inspection == null)
            {
                ViewBag.ErrorMessage = $"Проверки с ID = {inspId} не найдены";
                return View("NotFound");
            }

            if (inspection.CriteriaList == null)
            {
                inspection.CriteriaList = new List<CheckVsCriteria>();
            }

            List<Criteria> inspCriteria = new List<Criteria>();

            foreach (var cvc in inspection.CriteriaList)
            {
                inspCriteria.Add(cvc.Criteria);
            }

            var model = new List<CriteriasInspectionViewModel>();

            foreach (Criteria criteria in all_criteria)
            {
                    var criteriasInspectionViewModel = new CriteriasInspectionViewModel
                    {
                        CriteriaId = criteria.Id,
                        Name = criteria.Name,
                        Description = criteria.Description
                    };

                    if (inspCriteria.Contains(criteria))
                    {
                        criteriasInspectionViewModel.IsSelected = true;
                    }
                    else
                    {
                        criteriasInspectionViewModel.IsSelected = false;
                    }

                    model.Add(criteriasInspectionViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCriteriasInInspection(List<CriteriasInspectionViewModel> model, int inspId)
        {
            var all_insp = await _context.Inspections.Include(i => i.CriteriaList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == inspId);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            var all_criteria = await _context.Criterias.ToListAsync();

            if (inspection == null)
            {
                ViewBag.ErrorMessage = $"Проверки с ID = {inspId} не найдены";
                return View("NotFound");
            }

            if (inspection.CriteriaList == null)
            {
                inspection.CriteriaList = new List<CheckVsCriteria>();
            }

            List<Criteria> inspCriteria = new List<Criteria>();

            foreach (var cvc in inspection.CriteriaList)
            {
                inspCriteria.Add(cvc.Criteria);
            }

            for (int i = 0; i < model.Count; i++)
            {
                var criteria = all_criteria.Find(item => item.Id == model[i].CriteriaId);

                if (model[i].IsSelected && !(inspCriteria.Contains(criteria)))
                {
                    CheckVsCriteria checkVsCriteria = new CheckVsCriteria
                    {
                        Criteria = criteria,
                        Check = inspection
                    };
                    inspection.CriteriaList.Add(checkVsCriteria);
                }
                else if (!model[i].IsSelected && inspCriteria.Contains(criteria))
                {
                    CheckVsCriteria checkVsCriteria = all_check_crit.Find(item => item.Criteria.Id == criteria.Id);
                    checkVsCriteria.Check = null;
                    _context.Update(checkVsCriteria);
                }
                else
                {
                    continue;
                }
            }

            try
            {
                _context.Update(inspection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(inspection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Edit", new { id = inspId });
        }

        // GET: Inspections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == id);

            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        // POST: Inspections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var all_insp = await _context.Inspections.Include(i => i.InspectionSpace).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == id);
            int idInspSpace = inspection.InspectionSpace.Id;
            _context.Inspections.Remove(inspection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = idInspSpace });
        }

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.Id == id);
        }

        public ActionResult GoToInspectionSpace(int? id)
        {
            return RedirectToAction("Details", "InspectionSpaces", new { id = id });
        }
        public ActionResult GoToValidations(int? id)
        {
            return RedirectToAction("Index", "Validations", new { id = id });
        }

        public async void AllocatePUDsPerInspectors(int inspId)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                            .UseSqlServer(connectionString)
                                                            .Options;

            ApplicationDbContext context = new ApplicationDbContext(contextOptions);
            context.Database.SetCommandTimeout(100000);

            var inspections = await context.Inspections.Include(i => i.PUDList)
                                                      .ToListAsync();
            var insp = inspections.Find(i => i.Id == inspId);

            if (insp.PUDList.Count != 0)
            {
                // Выбираем нужную проверку
                var all_insp = await context.Inspections.Include(i => i.UserList)
                                                         .Include(i => i.PUDAllocationList)
                                                         .Include(i => i.CriteriaList)
                                                         .ToListAsync();
                //var all_crit = await context.CheckVsCriterias.ToListAsync();
                //var all_puds = await context.PUDs.ToListAsync();
                //var all_users = await context.Users.ToListAsync();
                //var all_allocation = await context.PUDAllocations.ToListAsync();
                var all_users = await context.Users.Include(i => i.CheckResults).ToListAsync();
                var all_results = await context.CheckResults.Include(i => i.InspectionCriteria).ToListAsync();
                var all_crit = await context.CheckVsCriterias.Include(i => i.Check).ToListAsync();

                var inspection = all_insp.Find(i => i.Id == inspId);
                inspection.PUDList = insp.PUDList;

                // Создаем список для сохранения проверенных ПУД
                List<PUD> checkedPUDs = new List<PUD>();

                // Создаем список для еще нераспределенных ПУД
                List<PUD> unallocatePUDs = new List<PUD>();
                unallocatePUDs.AddRange(inspection.PUDList);

                // Создаем список для нового распределения ПУД
                List<PUDAllocation> newAllocations = new List<PUDAllocation>();

                // Считаем, сколько ПУД на человека должно прийтись
                double pudsPerInspector = (double)inspection.PUDList.Count / (double)inspection.UserList.Count;

                // Число обработанных пользователей
                int userCount = 0;

                Random rand = new Random();

                foreach (var user in inspection.UserList)
                {
                    if (user.CheckResults == null)
                    {
                        user.CheckResults = new List<CheckResult>();
                    }
                    // Создаем список тех ПУД, которые были проверены данным проверяющим в прошлую итерацию (при этом в списке должно быть столько дубликатов каждого ПУДа, сколько было критерив проверки)
                    var previousIterationsChechedPUDs = from result in user.CheckResults where result.InspectionCriteria.Check.Id == inspection.Id && result.Iteration < inspection.CurrentIteration select result.PUD;

                    if (previousIterationsChechedPUDs == null)
                    {
                        previousIterationsChechedPUDs = new List<PUD>();
                    }

                    // Создаем список проверенных в данную итерацию проверяющим ПУДов (при этом в списке должно быть столько дубликатов каждого ПУДа, сколько было критерив проверки)
                    var userCheckResultPUDs = from result in user.CheckResults where result.InspectionCriteria.Check.Id == inspection.Id && result.Iteration == inspection.CurrentIteration select result.PUD;

                    if (userCheckResultPUDs == null)
                    {
                        userCheckResultPUDs = new List<PUD>();
                    }

                    double userCheckResultPUDsCount = (double)userCheckResultPUDs.Count();
                    double criteriaListCount = (double)(inspection.CriteriaList != null ? inspection.CriteriaList.Count : 0);

                    // Считаем реальное число проверенных ПУД
                    int countCheckedPUD = criteriaListCount != 0 ? (int)Math.Round((userCheckResultPUDsCount / criteriaListCount) - 0.5) : 0;

                    // Проверенные ПУД сохраняем в список проверенных, удаляем из списка нераспределенных и добавляем их в распределение
                    foreach (PUD pud in userCheckResultPUDs)
                    {
                        if (!checkedPUDs.Contains(pud))
                        {
                            checkedPUDs.Add(pud);
                            unallocatePUDs.Remove(pud);
                            newAllocations.Add(new PUDAllocation { Inspection = inspection, User = user, Checked = true, Iteration = inspection.CurrentIteration, PUD = pud });
                        }
                    }

                    // распределяем ПУДы
                    for (int i = 0; i < (int)((userCount + 1) * pudsPerInspector - userCount * pudsPerInspector) - countCheckedPUD; i++)
                    {
                        // Выбираем случайный индекс нераспределенного ПУДы
                        int itemIndex = rand.Next(0, unallocatePUDs.Count);

                        // Проверяем, что данный ПУД не был проверен пользователем на прошлых итерациях
                        if (!previousIterationsChechedPUDs.Contains(unallocatePUDs[itemIndex]))
                        {
                            // Добавляем ПУД в распределение
                            newAllocations.Add(new PUDAllocation { Inspection = inspection, User = user, Checked = false, Iteration = inspection.CurrentIteration, PUD = unallocatePUDs[itemIndex] });
                        }
                        else
                        {
                            // Возвращаем цикл на шаг назад
                            i--;
                        }
                    }

                    userCount++;
                }

                // Удаляем старое распределение, сохраняем новое и обновляем контекст
                foreach (PUDAllocation allocation in inspection.PUDAllocationList)
                {
                    if (allocation.Iteration == inspection.CurrentIteration)
                    {
                        context.Remove(allocation);
                    }
                }
                foreach (PUDAllocation allocation in newAllocations)
                {
                    context.Add(allocation);
                }

                await context.SaveChangesAsync();
            }

            return;
        }
    }
}
