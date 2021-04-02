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
    [Authorize(Roles = "inspector")]
    public class InspectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("InspectionList");
        }

        public async Task<IActionResult> InspectionList()
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);

            var all_user = await _context.Users.Include(i => i.Inspections).ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            if (user.Inspections == null)
            {
                user.Inspections = new List<Inspection>();
            }

            List<InspectorListViewModel> model = new List<InspectorListViewModel>();
            foreach (var inspection in user.Inspections) {
                InspectorListViewModel viewModel = new InspectorListViewModel()
                {
                    InspectionId = inspection.Id,
                    InspectionName = inspection.Name,
                    StartDate = inspection.StartDate,
                    EndDate = inspection.EndDate,
                    IterationNumber = inspection.IterationNumber,
                    CurrentIteration = inspection.CurrentIteration,
                    Hunt = inspection.Hunt
                };

                model.Add(viewModel);
            }

            TempData["userId"] = user.Id;
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        public async Task<IActionResult> InspectPUD()
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);

            var all_user = await _context.Users.Include(i => i.PUDAllocations).ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            if (user.PUDAllocations == null)
            {
                user.PUDAllocations = new List<PUDAllocation>();
            }
            else
            {
                var all_allocation = await _context.PUDAllocations.Include(i => i.PUD).Include(i => i.Inspection).ToListAsync();
                var all_insp = await _context.Inspections.Include(i => i.CriteriaList).ToListAsync();
                var all_pud = await _context.PUDs.Include(i => i.EduProgram).Include(i => i.Department).ToListAsync();
                var all_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).ToListAsync();
            }

            List<InspectPUDViewModel> model = new List<InspectPUDViewModel>();

            foreach (var allocation in user.PUDAllocations)
            {
                if (!allocation.Checked)
                {
                    InspectPUDViewModel viewModel = new InspectPUDViewModel()
                    {
                        UserId = user.Id,
                        InspectionId = allocation.Inspection.Id,
                        CurrentIteration = allocation.Inspection.CurrentIteration,
                        PUDId = allocation.PUD.Id,
                        Link = allocation.PUD.Link,
                        EduProgram = allocation.PUD.EduProgram.Name,
                        Department = allocation.PUD.Department.Name,
                        EducationStage = allocation.PUD.EducationStage,
                        Language = allocation.PUD.Language,
                        CourseName = allocation.PUD.CourseName,
                        Details = allocation.PUD.Details,
                        Criterias = new List<InspectPUDCriteriaViewModel>(),
                        AllocationId = allocation.Id
                    };

                    foreach (var criteria in allocation.Inspection.CriteriaList)
                    {
                        InspectPUDCriteriaViewModel criteriaViewModel = new InspectPUDCriteriaViewModel()
                        {
                            CheckVsCriteriaId = criteria.Id,
                            Criteria = criteria.Criteria
                        };
                        viewModel.Criterias.Add(criteriaViewModel);
                    }

                    model.Add(viewModel);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InspectPUD(List<InspectPUDViewModel> model)
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);
            var all_user = await _context.Users.ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            var inspection = await _context.Inspections.FirstOrDefaultAsync(m => m.Id == model[0].InspectionId);
            var pud = await _context.PUDs.FirstOrDefaultAsync(m => m.Id == model[0].PUDId);
            var allocation = await _context.PUDAllocations.FirstOrDefaultAsync(m => m.Id == model[0].AllocationId);
            var all_crit = await _context.CheckVsCriterias.ToListAsync();

            InspectionPUDResult inspectionResult = new InspectionPUDResult()
            {
                Iteration = model[0].CurrentIteration,
                PUD = pud,
                User = user,
                Inspection = inspection,
                CheckResults = new List<CheckResult>()
            };

            foreach (var modelCriteria in model[0].Criterias)
            {
                var criteria = all_crit.Find(i => i.Id == modelCriteria.CheckVsCriteriaId);
                CheckResult result = new CheckResult()
                {
                    Evaluation = modelCriteria.CheckResult,
                    InspectionCriteria = criteria
                };

                inspectionResult.CheckResults.Add(result);
            }

            allocation.Checked = true;
            _context.Update(allocation);

            _context.Add(inspectionResult);

            await _context.SaveChangesAsync();
            return RedirectToAction("InspectPUD");
        }
    }
}
