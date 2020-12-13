using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private IEnvironmentRepository repository;
        public AdministratorController(IEnvironmentRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Employees.ToList());
        }

        public async Task<ViewResult> UserPage(string id)
        {
            ViewBag.Title = "Detaljer för användaren";
            ViewBag.ID = id;

            ViewBag.Departments = repository.Departments;

            var userDetails = await repository.GetEmployee(id);

            return View(userDetails);
        }

        public IActionResult UpdateUser(string UserID, Department department, string role, string name)
        {
            repository.UpdateUser(UserID, name, department.DepartmentId, role);

            return View("Index", repository.Employees.ToList());
        }
    }
}
