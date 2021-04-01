using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PUDInspection.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PUDInspection.Models;

namespace PUDInspection.Controllers
{
    [Authorize(Roles = "creator,admin,moderator")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        SignInManager<ApplicationUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize(Roles = "creator,admin")]
        public async Task<ActionResult> ListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    RealName = model.RealName,
                    VKLink = model.VKLink,
                    Blocked = false
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (User.IsInRole("admin") || User.IsInRole("creator"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    if (User.IsInRole("moderator"))
                    {
                        return RedirectToAction("Index", "Inspection");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "creator, admin")]
        public async Task<ActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователи с ID = {id} не найдены";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                RealName = user.RealName,
                Email = user.Email,
                EmailComfirmed = user.EmailConfirmed,
                VKLink = user.VKLink,
                Roles = roles
            };

            return View(model);
        }

        [Authorize(Roles = "creator, admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователи с ID = {model.Id} не найдены";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    user.RealName = model.RealName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailComfirmed;
                    user.VKLink = model.VKLink;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [Authorize(Roles = "creator, admin")]
        public async Task<IActionResult> EditRolesInUser(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователи с ID = {userId} не найдены";
                return View("NotFound");
            }

            var model = new List<RolesUserViewModel>();
            var roles = await roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                var rolesUserViewModel = new RolesUserViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    rolesUserViewModel.IsSelected = true;
                }
                else
                {
                    rolesUserViewModel.IsSelected = false;
                }

                model.Add(rolesUserViewModel);
            }

            return View(model);
        }

        [Authorize(Roles = "creator, admin")]
        [HttpPost]
        public async Task<IActionResult> EditRolesInUser(List<RolesUserViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователи с ID = {userId} не найдены";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var role = await roleManager.FindByIdAsync(model[i].RoleId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditUser", new { Id = userId });
                }
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }

        [Authorize(Roles = "creator, admin")]
        [HttpPost]
        public async Task<IActionResult> BlockOrUnblockUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователи с ID = {id} не найдены";
                return View("NotFound");
            }
            else
            {
                user.Blocked = !user.Blocked;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("ListUsers");
        }

        [Authorize(Roles = "creator")]
        public ActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "creator")]
        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "creator")]
        public ActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }

        [Authorize(Roles = "creator")]
        public async Task<ActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Роли с ID = {id} не найдены";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in await userManager.GetUsersInRoleAsync(role.Name))
            {
                model.Users.Add(user.UserName);
            }

            return View(model);
        }

        [Authorize(Roles = "creator")]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Роли с ID = {model.Id} не найдены";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    role.Name = model.RoleName;

                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [Authorize(Roles = "creator")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Роли с ID = {roleId} не найдены";
                return View("NotFound");
            }

            var model = new List<UsersRoleViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                var userRoleViewModel = new UsersRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [Authorize(Roles = "creator")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UsersRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Роли с ID = {roleId} не найдены";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

    }
}
