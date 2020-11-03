using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvironmentCrime.Infrastructure;
using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        private IEnvironmentRepository repository;
        public CoordinatorController(IEnvironmentRepository repo)
        {
            repository = repo;
        }
        public ViewResult CrimeCoordinator(int id)
        {
            ViewBag.Title = "Detaljer för ärendet";
            ViewBag.ID = id;
            ViewBag.DepartmentList = repository.Departments;
            return View();
        }
        public ViewResult ReportCrime()
        {
            ViewBag.Title = "Rapportera brott";

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
        public ViewResult StartCoordinator()
        {
            ViewBag.Title = "Ärenden";
            return View(repository);
        }
        public ViewResult Thanks()
        {
            var errandSession = HttpContext.Session.GetJson<Errand>("NewErrand");
            ViewBag.ErrandRefNumber = repository.SaveErrand(errandSession);
            HttpContext.Session.Remove("NewErrand");
            ViewBag.Title = "Tack för din anmälan";
            return View();
        }
        public ViewResult Validate(Errand errand)
        {
            HttpContext.Session.SetJson("NewErrand", errand);
            ViewBag.Title = "Din anmälan av miljöbrott";
            return View(errand);
        }

        public ViewResult AssingDepartment(Department department, int errandID)
        {
            repository.AssignDepartment(department, errandID);
            return View("StartCoordinator", repository);
        }

        public ViewResult Search(ErrandStatus errandStatus, Department department)
        {

            //Kanske kolla upp hur man skickar till en anann vy bara
            return StartCoordinator();
        }
    }
}
