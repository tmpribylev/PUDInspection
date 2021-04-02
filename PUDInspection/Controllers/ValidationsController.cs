using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                Validation validation = new Validation()
                {
                    Name = model.ValidationName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IterationNumber = model.IterationNumber,
                    Inspection = await _context.Inspections
                    .FirstOrDefaultAsync(m => m.Id == model.InspId)
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
    }
}
