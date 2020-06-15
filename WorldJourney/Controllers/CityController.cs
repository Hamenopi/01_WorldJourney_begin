using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WorldJourney.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using WorldJourney.Filters;

namespace WorldJourney.Controllers
{
    public class CityController : Controller
    {

        private IData _data;
        private IHostingEnvironment _environment;

        public CityController(IData data, IHostingEnvironment environment)
        {
            _data = data;
            _environment = environment;
            _data.CityInitializeData();
        }

        [Route("WorldJourney")]
        [ServiceFilter(typeof(LogActionFilterAttribute))]
        public IActionResult Index()
        {
            ViewData["Page"] = "Search city";
            return View();
        }

        [Route("CityDetails/{id?}")]
        public IActionResult Details(int? id)
        {
            ViewData["Page"] = "Selected city";
            City city = _data.GetCityById(id);

            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Title = city.CityName;
            return View(city);
        }

        public IActionResult GetImage(int? cityId)
        {
            ViewData["Message"] = "Display Image";
            City requestedCity = _data.GetCityById(cityId);
            if (requestedCity != null)
            {
                var webRootPath = _environment.WebRootPath;
                var folderPath = "\\images\\";
                var fullPath = webRootPath + folderPath + requestedCity.ImageName;

                FileStream fileOnDisk = new FileStream(fullPath, FileMode.Open);
                byte[] fileBytes;
                using (var br = new BinaryReader(fileOnDisk))
                {
                    fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                }
                return File(fileBytes, requestedCity.ImageMimeType);
            }
            else
            {
                return NotFound();
            }
        }
    }


}
