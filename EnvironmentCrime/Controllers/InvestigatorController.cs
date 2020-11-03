using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Investigator")]
    public class InvestigatorController : Controller
    {
        private IWebHostEnvironment environment;
        private IEnvironmentRepository repository;
        public InvestigatorController(IEnvironmentRepository repo, IWebHostEnvironment environment)
        {
            this.environment = environment;
            repository = repo;
        }
        public ViewResult CrimeInvestigator(int id)
        {
            ViewBag.Title = "Detaljer för ärendet";
            ViewBag.ID = id;
            //Passar bara in de två statusar som en investigator kan sätta
            ViewBag.ErrandStatus = repository.ErrandStatuses.Where(es => es.StatusId == "S_C" || es.StatusId == "S_D");
            return View();
        }
        public ViewResult StartInvestigator()
        {
            ViewBag.Title = "Ärenden";
            return View(repository);
        }

        public async Task<IActionResult> UpdateStatus(ErrandStatus errandStatus, int errandID, string events, string information, IFormFile loadSample, IFormFile loadImage)
        {
            repository.UpdateStatus(errandID, errandStatus.StatusId, events, information);

            var tempPath = Path.GetTempFileName();

            if (loadSample != null && loadSample.Length > 0)
            {
                using(var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await loadSample.CopyToAsync(stream);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + loadSample.FileName;

                var path = Path.Combine(environment.WebRootPath, "SampleFIles", uniqueFileName);

                System.IO.File.Move(tempPath, path);

                repository.AddSampleFile(uniqueFileName, errandID);
            }

            if (loadImage != null && loadImage.Length > 0)
            {
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await loadImage.CopyToAsync(stream);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + loadImage.FileName;

                var path = Path.Combine(environment.WebRootPath, "ImageFiles", uniqueFileName);

                System.IO.File.Move(tempPath, path);

                repository.AddImageFile(uniqueFileName, errandID);
            }

            return View("StartInvestigator", repository);
        }

    }
}
