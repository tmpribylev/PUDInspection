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
    [Authorize(Roles = "validator")]
    public class ValidatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public ValidatorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ValidationList");
        }

        public async Task<IActionResult> ValidationList()
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);

            var all_user = await _context.Users.Include(i => i.Validations).ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            if (user.Validations == null)
            {
                user.Validations = new List<Validation>();
            }

            List<ValidatorListViewModel> model = new List<ValidatorListViewModel>();
            foreach (var validation in user.Validations)
            {
                if (!validation.Closed)
                {
                    ValidatorListViewModel viewModel = new ValidatorListViewModel()
                    {
                        ValidationId = validation.Id,
                        ValidationName = validation.Name,
                        StartDate = validation.StartDate,
                        EndDate = validation.EndDate,
                        IterationNumber = validation.IterationNumber,
                        CurrentIteration = validation.CurrentIteration,
                        Hunt = validation.Hunt,
                        Opened = validation.Opened
                    };

                    model.Add(viewModel);
                }
            }

            TempData["userId"] = user.Id;
            return View(model);
        }


        // GET: Validator/Details/5
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

        public async Task<IActionResult> ValidatePUD()
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);

            var all_user = await _context.Users.Include(i => i.PUDAllocations).ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            bool uncheckPUDExist = false;

            if (user.PUDAllocations != null)
            {
                foreach (var alloc in user.PUDAllocations)
                {
                    uncheckPUDExist = true;
                    break;
                }
            }

            if (user.PUDAllocations == null || user.PUDAllocations.Count == 0 || !uncheckPUDExist)
            {
                user.PUDAllocations = new List<PUDAllocation>();
            }
            else
            {
                var all_allocation = await _context.PUDAllocations.Include(i => i.PUD).Include(i => i.Validation).ToListAsync();
                var all_valid = await _context.Validations.Include(i => i.CriteriaList).ToListAsync();
                var all_pud = await _context.PUDs.Include(i => i.EduProgram).Include(i => i.Department).Include(i => i.InspectionPUDResults).ToListAsync();
                var all_pudResults = await _context.InspectionPUDResults.Include(i => i.CheckResults).ToListAsync();
                var all_critResults = await _context.CheckResults.ToListAsync();
                var all_crit = await _context.CheckVsCriterias.Include(i => i.Criteria).ToListAsync();
            }

            ValidatePUDViewModel viewModel = new ValidatePUDViewModel(); ;

            foreach (var allocation in user.PUDAllocations)
            {
                if (!allocation.Checked && allocation.Validation != null && !allocation.Validation.Closed && allocation.Validation.CurrentIteration == allocation.Iteration)
                {
                    viewModel = new ValidatePUDViewModel()
                    {
                        UserId = user.Id,
                        ValidationID = allocation.Validation.Id,
                        CurrentIteration = allocation.Validation.CurrentIteration,
                        PUDId = allocation.PUD.Id,
                        Link = allocation.PUD.Link,
                        EduProgram = allocation.PUD.EduProgram.Name,
                        Department = allocation.PUD.Department.Name,
                        EducationStage = allocation.PUD.EducationStage,
                        Language = allocation.PUD.Language,
                        CourseName = allocation.PUD.CourseName,
                        Details = allocation.PUD.Details,
                        Criterias = new List<ValidatePUDCriteriaViewModel>(),
                        AllocationId = allocation.Id,
                        InspectionResults = new List<EvaluateInspectionPUDResultsViewModel>()
                    };

                    foreach (var criteria in allocation.Validation.CriteriaList)
                    {
                        ValidatePUDCriteriaViewModel criteriaViewModel = new ValidatePUDCriteriaViewModel()
                        {
                            CheckVsCriteriaId = criteria.Id,
                            Criteria = criteria.Criteria
                        };
                        viewModel.Criterias.Add(criteriaViewModel);
                    }

                    // =====================================================================================================================

                    foreach (var result in allocation.PUD.InspectionPUDResults)
                    {
                        EvaluateInspectionPUDResultsViewModel eval = new EvaluateInspectionPUDResultsViewModel()
                        {
                            CheckResultEvaluations = new List<int>(),
                            InspectionPUDResultId = result.Id,
                            CriteriaNames = new List<string>()
                        };

                        foreach (var item in result.CheckResults)
                        {
                            eval.CheckResultEvaluations.Add(item.Evaluation);
                            eval.CriteriaNames.Add(item.InspectionCriteria.Criteria.Name);
                        }

                        viewModel.InspectionResults.Add(eval);
                    }

                    break;
                }
            }


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePUD(ValidatePUDViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == model.UserId);

            var validation = await _context.Validations.FirstOrDefaultAsync(m => m.Id == model.ValidationID);
            var pud = await _context.PUDs.FirstOrDefaultAsync(m => m.Id == model.PUDId);
            var allocation = await _context.PUDAllocations.FirstOrDefaultAsync(m => m.Id == model.AllocationId);
            var all_inspectionResults = await _context.InspectionPUDResults.Include(i => i.CheckResultEvaluations).ToListAsync();
            var all_crit = await _context.CheckVsCriterias.ToListAsync();

            if (model.Criterias == null)
            {
                model.Criterias = new List<ValidatePUDCriteriaViewModel>();
            }
            if (model.InspectionResults == null)
            {
                model.InspectionResults = new List<EvaluateInspectionPUDResultsViewModel>();
            }

            ValidationPUDResult validationPUDResult = new ValidationPUDResult()
            {
                Iteration = model.CurrentIteration,
                PUD = pud,
                User = user,
                Validation = validation,
                CheckResults = new List<CheckResult>(),
                InspectionPUDResults = new List<InspectionPUDResult>()
            };

            foreach (var modelCriteria in model.Criterias)
            {
                var criteria = all_crit.Find(i => i.Id == modelCriteria.CheckVsCriteriaId);
                CheckResult result = new CheckResult()
                {
                    Evaluation = modelCriteria.CheckResult,
                    InspectionCriteria = criteria
                };

                validationPUDResult.CheckResults.Add(result);
            }

            foreach (var result in model.InspectionResults)
            {
                CheckResultEvaluation eval = new CheckResultEvaluation()
                {
                    Evaluation = result.Evaluation,
                    Validation = validation,
                    Validator = user
                };

                var inspPUDRes = all_inspectionResults.Find(i => i.Id == result.InspectionPUDResultId);
                inspPUDRes.CheckResultEvaluations.Add(eval);
                _context.Update(inspPUDRes);
            }

            allocation.Checked = true;
            _context.Update(allocation);

            _context.Add(validationPUDResult);

            await _context.SaveChangesAsync();
            return RedirectToAction("ValidatePUD");
        }
    }
}
