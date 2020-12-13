using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private IEnvironmentRepository repository;
        private IHttpContextAccessor contextAcc;
        public ManagerController(IEnvironmentRepository repo,  IHttpContextAccessor contextAcc)
        {
            this.contextAcc = contextAcc;
            repository = repo;
        }
        public ViewResult CrimeManager(int id)
        {
            ViewBag.ID = id;
            ViewBag.Title = "Detaljer för ärendet";

            //Get the departmentId of the current user to display only the employees in that department
            String userName = contextAcc.HttpContext.User.Identity.Name;
            var user = repository.Employees.Single(e => e.EmployeeId == userName);
            ViewBag.Employees = repository.Employees.Where(e => e.DepartmentId == user.DepartmentId);
            return View();
        }
        public ViewResult StartManager()
        {
            ViewBag.Title = "Ärenden";
            return View(repository);
        }
        public ViewResult AssingEmployee(Employee employee, int errandID, bool noAction, string reason)
        {
            if (noAction)
            {
                repository.CloseCase(errandID, reason);
            }
            else
            {
                repository.AssignEmployee(employee.EmployeeId, errandID);
            }
            
            return View("StartManager", repository);
        }

        public ViewResult Filter(string errandStatus, string investigator)
        {
            if (errandStatus == "Välj alla")
            {
                ViewBag.errandStatus = null;
            }
            else
            {
                ViewBag.errandStatus = errandStatus;
            }
            if (investigator == "Välj alla")
            {
                ViewBag.investigator = null;
            }
            else
            {
                ViewBag.investigator = investigator;
            }
            ViewBag.searchString = null;
            ViewBag.Title = "Ärenden";
            return View("StartManager", repository);
        }

        public ViewResult Search(string casenumber)
        {
            ViewBag.errandStatus = null;
            ViewBag.investigator = null;
            ViewBag.searchString = casenumber;
            ViewBag.Title = "Ärenden";
            return View("StartManager", repository);
        }
    }
}
