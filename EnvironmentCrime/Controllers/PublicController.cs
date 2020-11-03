using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvironmentCrime.Infrastructure;
using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentCrime.Controllers
{
    public class PublicController : Controller
    {
        private IEnvironmentRepository repository;

        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public PublicController(IEnvironmentRepository repo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            repository = repo;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public ViewResult Index()
        {
            var errandSession = HttpContext.Session.GetJson<Errand>("NewErrand");
            if (errandSession == null)
            {
                return View();
            }
            else
            {
                return View(errandSession);
            }

        }

        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Faq()
        {
            return View();
        }
        public ViewResult Services()
        {
            return View();
        }
        public ViewResult Thanks()
        {
            var errandSession = HttpContext.Session.GetJson<Errand>("NewErrand");
            ViewBag.ErrandRefNumber = repository.SaveErrand(errandSession);
            HttpContext.Session.Remove("NewErrand");
            return View();
        }
        public ViewResult Validate(Errand errand)
        {
            HttpContext.Session.SetJson("NewErrand", errand);
            return View(errand);
        }
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {

                        if (await userManager.IsInRoleAsync(user, "Coordinator"))
                        {
                            return Redirect("/Coordinator/StartCoordinator");
                        }
                        if (await userManager.IsInRoleAsync(user, "Manager"))
                        {
                            return Redirect("/Manager/StartManager");
                        }
                        if (await userManager.IsInRoleAsync(user, "Investigator"))
                        {
                            return Redirect("/Investigator/StartInvestigator");
                        }
                    }
                }              
            }
            ModelState.AddModelError("", "Användarnamn eller lösenord stämmer inte");
            return View(loginModel);            
        }

        public ViewResult LogOut()
        {
            if (User != null)
            {
                 signInManager.SignOutAsync();
            }

            return View("Index");
        }
    }
}
