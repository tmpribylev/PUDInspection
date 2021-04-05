using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PUDInspection.Data;
using PUDInspection.Models;
using PUDInspection.ViewModels;

namespace PUDInspection.Controllers
{
    [Authorize(Roles = "admin,moderator")]
    public class ValidationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public ValidationsController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Validations
        public async Task<IActionResult> Index(int? id)
        {
            //return View(await _context.Validations.ToListAsync());

            if (id == null)
            {
                return NotFound();
            }

            var all_valid = await _context.Validations.Include(i => i.Inspection).ToListAsync();
            var validations = from valid in all_valid
                              where valid.Inspection != null && valid.Inspection.Id == id
                              select valid;

            if (validations == null)
            {
                return NotFound();
            }
            TempData["InspID"] = id;
            return View(validations);
        }

        // GET: Validations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validation == null)
            {
                return NotFound();
            }

            return View(validation);
        }

        // GET: Validations/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateValidationViewModel model = new CreateValidationViewModel()
            {
                InspId = (int)id
            };

            return View(model);
        }

        // POST: Validations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateValidationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var all_insp = await _context.Inspections.Include(i => i.PUDList).ToListAsync();
                var inspection = all_insp.Find(i => i.Id == model.InspId);

                Validation validation = new Validation()
                {
                    Name = model.ValidationName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IterationNumber = model.IterationNumber,
                    Inspection = inspection,
                    PUDList = inspection.PUDList
                };

                _context.Add(validation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = model.InspId });
            }
            return View(model);
        }

        // GET: Validations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_valid = await _context.Validations.Include(i => i.Inspection).Include(i => i.CriteriaList).Include(i => i.UserList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == id);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            if (validation == null)
            {
                return NotFound();
            }

            EditValidationViewModel model = new EditValidationViewModel()
            {
                InspectionId = validation.Inspection.Id,
                ValidationId = validation.Id,
                ValidationName = validation.Name,
                StartDate = validation.StartDate,
                EndDate = validation.EndDate,
                IterationNumber = validation.IterationNumber,
                Criterias = new List<Criteria>(),
                Users = new List<string>()
            };

            foreach (CheckVsCriteria cvc in validation.CriteriaList)
            {
                model.Criterias.Add(cvc.Criteria);
            }

            foreach (ApplicationUser user in validation.UserList)
            {
                model.Users.Add(user.RealName);
            }

            return View(model);
        }

        // POST: Validations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditValidationViewModel model)
        {
            var all_valid = await _context.Validations.Include(i => i.Inspection).ToListAsync();
            var validation = all_valid.Find(i => i.Id == model.ValidationId);

            if (validation == null)
            {
                ViewBag.ErrorMessage = $"Перепроверки с ID = {model.ValidationId} не найдены";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    validation.Name = model.ValidationName;
                    validation.StartDate = model.StartDate;
                    validation.EndDate = model.EndDate;
                    validation.IterationNumber = model.IterationNumber;

                    try
                    {
                        _context.Update(validation);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ValidationExists(validation.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction("Index", new { id = model.InspectionId });
                }

                return View(model);
            }
        }

        public async Task<IActionResult> EditUsersInValidation(int validId)
        {
            ViewBag.validId = validId;

            var all_valid = await _context.Validations.Include(i => i.UserList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == validId);

            if (validation == null)
            {
                ViewBag.ErrorMessage = $"Перепроверки с ID = {validId} не найдены";
                return View("NotFound");
            }

            if (validation.UserList == null)
            {
                validation.UserList = new List<ApplicationUser>();
            }

            var model = new List<UsersValidationViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, "validator"))
                {
                    var usersInspectionViewModel = new UsersValidationViewModel
                    {
                        UserId = user.Id,
                        UserName = user.RealName
                    };

                    if (validation.UserList.Contains(user))
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
        public async Task<IActionResult> EditUsersInValidation(List<UsersValidationViewModel> model, int validId)
        {
            var all_valid = await _context.Validations.Include(i => i.UserList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == validId);

            if (validation == null)
            {
                ViewBag.ErrorMessage = $"Перепроверки с ID = {validId} не найдены";
                return View("NotFound");
            }

            if (validation.UserList == null)
            {
                validation.UserList = new List<ApplicationUser>();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                if (model[i].IsSelected && !(validation.UserList.Contains(user)))
                {
                    validation.UserList.Add(user);
                }
                else if (!model[i].IsSelected && validation.UserList.Contains(user))
                {
                    validation.UserList.Remove(user);
                }
                else
                {
                    continue;
                }
            }

            try
            {
                _context.Update(validation);
                await _context.SaveChangesAsync();
                //var registry = new Registry();
                //registry.Schedule(() => AllocatePUDsPerInspectors(inspId)).ToRunNow();
                //JobManager.Initialize(registry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidationExists(validation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Edit", new { id = validId });
        }

        public async Task<IActionResult> EditCriteriasInValidation(int validId, int inspId)
        {
            ViewBag.validId = validId;

            var all_valid = await _context.Validations.Include(i => i.CriteriaList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == validId);

            var all_insp = await _context.Inspections.Include(i => i.CriteriaList).ToListAsync();
            var inspection = all_insp.Find(i => i.Id == inspId);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            var all_criteria = await _context.Criterias.ToListAsync();

            if (validation == null)
            {
                ViewBag.ErrorMessage = $"Перепроверки с ID = {validId} не найдены";
                return View("NotFound");
            }

            if (validation.CriteriaList == null)
            {
                validation.CriteriaList = new List<CheckVsCriteria>();
            }

            List<Criteria> validCriteria = new List<Criteria>();

            foreach (var cvc in validation.CriteriaList)
            {
                validCriteria.Add(cvc.Criteria);
            }

            List<Criteria> inspCriteria = new List<Criteria>();

            foreach (var cvc in inspection.CriteriaList)
            {
                inspCriteria.Add(cvc.Criteria);
            }

            var model = new List<CriteriasValidationViewModel>();

            foreach (Criteria criteria in inspCriteria)
            {
                var criteriasValidationViewModel = new CriteriasValidationViewModel
                {
                    CriteriaId = criteria.Id,
                    Name = criteria.Name,
                    Description = criteria.Description
                };

                if (validCriteria.Contains(criteria))
                {
                    criteriasValidationViewModel.IsSelected = true;
                }
                else
                {
                    criteriasValidationViewModel.IsSelected = false;
                }

                model.Add(criteriasValidationViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCriteriasInValidation(List<CriteriasValidationViewModel> model, int validId)
        {
            var all_valid = await _context.Validations.Include(i => i.CriteriaList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == validId);

            var all_check_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).Include(i => i.Check).ToListAsync();

            var all_criteria = await _context.Criterias.ToListAsync();

            if (validation == null)
            {
                ViewBag.ErrorMessage = $"Перепроверки с ID = {validId} не найдены";
                return View("NotFound");
            }

            if (validation.CriteriaList == null)
            {
                validation.CriteriaList = new List<CheckVsCriteria>();
            }

            List<Criteria> validCriteria = new List<Criteria>();

            foreach (var cvc in validation.CriteriaList)
            {
                validCriteria.Add(cvc.Criteria);
            }

            for (int i = 0; i < model.Count; i++)
            {
                var criteria = all_criteria.Find(item => item.Id == model[i].CriteriaId);

                if (model[i].IsSelected && !(validCriteria.Contains(criteria)))
                {
                    CheckVsCriteria checkVsCriteria = new CheckVsCriteria
                    {
                        Criteria = criteria,
                        Check = validation
                    };
                    validation.CriteriaList.Add(checkVsCriteria);
                }
                else if (!model[i].IsSelected && validCriteria.Contains(criteria))
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
                _context.Update(validation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidationExists(validation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Edit", new { id = validId });
        }

        // GET: Validations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validation == null)
            {
                return NotFound();
            }

            return View(validation);
        }

        // POST: Validations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var validation = await _context.Validations.FindAsync(id);
            _context.Validations.Remove(validation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValidationExists(int id)
        {
            return _context.Validations.Any(e => e.Id == id);
        }

        public ActionResult GoToInspection(int? id)
        {
            return RedirectToAction("Details", "Inspection", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Open(int id, int inspId)
        {
            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (validation == null)
            {
                return NotFound();
            }

            var registry = new Registry();
            registry.Schedule(() => NextIteration(id)).ToRunNow();
            JobManager.Initialize(registry);

            return RedirectToAction("Index", new { id = inspId });
        }

        [HttpPost]
        public async Task<IActionResult> Iteration(int id, int inspId)
        {
            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (validation == null)
            {
                return NotFound();
            }

            validation.Opened = false;
            _context.Update(validation);

            await _context.SaveChangesAsync();

            var registry = new Registry();
            registry.Schedule(() => NextIteration(id)).ToRunNow();
            JobManager.Initialize(registry);

            return RedirectToAction("Index", new { id = inspId });
        }

        [HttpPost]
        public async Task<IActionResult> Close(int id, int inspId)
        {
            var all_valid = await _context.Validations.Include(i => i.PUDAllocationList).ToListAsync();
            var validation = all_valid.Find(i => i.Id == id);

            if (validation == null)
            {
                return NotFound();
            }

            foreach (var allocation in validation.PUDAllocationList)
            {
                _context.Remove(allocation);
            }

            validation.Closed = true;
            _context.Update(validation);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = inspId });
        }

        private async void NextIteration(int validId)
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

            var valid = await context.Validations
                .FirstOrDefaultAsync(m => m.Id == validId);

            try
            {
                valid.CurrentIteration++;
                context.Update(valid);
                await context.SaveChangesAsync();

                await AllocatePUDsPerValidators(validId);

                valid.Opened = true;
                context.Update(valid);
            }
            catch
            {
                if (valid.CurrentIteration == 0)
                {
                    valid.Opened = false;
                }
                else
                {
                    valid.Opened = true;
                }

                valid.CurrentIteration--;
                context.Update(valid);
            }

            await context.SaveChangesAsync();
        }

        private async Task<string> AllocatePUDsPerValidators(int validId)
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

            var validations = await context.Validations.Include(i => i.PUDList)
                                                      .ToListAsync();
            var valid = validations.Find(i => i.Id == validId);

            if (valid.PUDList.Count != 0)
            {
                Validation validation = null;
                // Выбираем нужную проверку
                var all_valid = await context.Validations.Include(i => i.UserList)
                                                         .Include(i => i.PUDAllocationList)
                                                         .Include(i => i.CriteriaList)
                                                         .ToListAsync();
                var all_users = await context.Users.Include(i => i.InspectionPUDResults).ToListAsync();
                var all_pudResults = await context.InspectionPUDResults.Include(i => i.CheckResults).Include(i => i.Inspection).ToListAsync();
                var all_results = await context.CheckResults.Include(i => i.InspectionCriteria).ToListAsync();
                var all_crit = await context.CheckVsCriterias.Include(i => i.Check).ToListAsync();

                validation = all_valid.Find(i => i.Id == validId);
                validation.PUDList = valid.PUDList;

                // Создаем список для сохранения проверенных ПУД
                List<PUD> checkedPUDs = new List<PUD>();

                // Создаем список для еще нераспределенных ПУД
                List<PUD> unallocatePUDs = new List<PUD>();
                unallocatePUDs.AddRange(validation.PUDList);

                // Создаем список для нового распределения ПУД
                List<PUDAllocation> newAllocations = new List<PUDAllocation>();

                // Считаем, сколько ПУД на человека должно прийтись
                double pudsPerInspector = (double)validation.PUDList.Count / (double)validation.UserList.Count;

                // Число обработанных пользователей
                int userCount = 0;

                Random rand = new Random();

                foreach (var user in validation.UserList)
                {
                    if (user.InspectionPUDResults == null)
                    {
                        user.InspectionPUDResults = new List<InspectionPUDResult>();
                    }
                    // Создаем список тех ПУД, которые были проверены данным проверяющим в прошлую итерацию 
                    var previousIterationsChechedPUDs = from result in user.InspectionPUDResults
                                                        where result.Inspection.Id == validation.Id
                                                         && result.Iteration < validation.CurrentIteration
                                                        select result.PUD;

                    if (previousIterationsChechedPUDs == null)
                    {
                        previousIterationsChechedPUDs = new List<PUD>();
                    }

                    // Создаем список проверенных в данную итерацию проверяющим ПУДов 
                    var userCheckedPUDs = from result in user.InspectionPUDResults
                                          where result.Inspection.Id == validation.Id
                                               && result.Iteration == validation.CurrentIteration
                                          select result.PUD;

                    if (userCheckedPUDs == null)
                    {
                        userCheckedPUDs = new List<PUD>();
                    }

                    // Считаем реальное число проверенных ПУД
                    int countCheckedPUD = userCheckedPUDs.Count();

                    // Проверенные ПУД сохраняем в список проверенных, удаляем из списка нераспределенных и добавляем их в распределение как уже проверенные пользователями
                    foreach (PUD pud in userCheckedPUDs)
                    {
                        if (!checkedPUDs.Contains(pud))
                        {
                            checkedPUDs.Add(pud);
                            unallocatePUDs.Remove(pud);
                            newAllocations.Add(new PUDAllocation { Validation = validation, User = user, Checked = true, Iteration = validation.CurrentIteration, PUD = pud });
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
                            newAllocations.Add(new PUDAllocation { Validation = validation, User = user, Checked = false, Iteration = validation.CurrentIteration, PUD = unallocatePUDs[itemIndex] });
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
                foreach (PUDAllocation allocation in validation.PUDAllocationList)
                {
                    if (allocation.Iteration == validation.CurrentIteration)
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

            return "";
        }
    }
}
