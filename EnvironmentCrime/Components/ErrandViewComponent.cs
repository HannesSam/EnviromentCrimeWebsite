using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Components
{
    public class ErrandViewComponent:ViewComponent
    {
        private IEnvironmentRepository repository;
        public ErrandViewComponent(IEnvironmentRepository repo)
        {
            repository = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync (int id)
        {
            var errandDetail = await repository.GetErrand(id);

            return View("Errand", errandDetail);
        }
    }
}
