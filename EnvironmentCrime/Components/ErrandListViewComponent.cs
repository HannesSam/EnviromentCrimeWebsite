﻿using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Components
{
    public class ErrandListViewComponent:ViewComponent
    {
        private IEnvironmentRepository repository;
        private IHttpContextAccessor contextAcc;
        public ErrandListViewComponent(IEnvironmentRepository repo, IHttpContextAccessor contextAcc)
        {
            this.contextAcc = contextAcc;
            repository = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            string userName = contextAcc.HttpContext.User.Identity.Name;
            var user = repository.Employees.Single(e => e.EmployeeId == userName);
            List<MyErrand> errandList;

            //Skickar här även med all information om sökning och filtrering
            switch (user.RoleTitle)
            {
                case "Coordinator":
                    errandList = repository.CoordinatorErrandList(ViewBag.searchString, ViewBag.errandStatus, ViewBag.department);
                    ViewBag.Controller = "Coordinator";
                    ViewBag.Action = "CrimeCoordinator";
                    break;
                case "Manager":
                    errandList = repository.ManagerErrandList(user.DepartmentId, ViewBag.searchString, ViewBag.errandStatus, ViewBag.investigator);
                    ViewBag.Controller = "Manager";
                    ViewBag.Action = "CrimeManager";
                    break;
                case "Investigator":
                    errandList = repository.InvestigatorErrandList(user.DepartmentId, user.EmployeeId, ViewBag.searchString, ViewBag.errandStatus);
                    ViewBag.Controller = "Investigator";
                    ViewBag.Action = "CrimeInvestigator";
                    break;
                default:
                    errandList = new List<MyErrand>();
                    break;
            }
            

            return View("ErrandList", errandList);
        }
    }
}
