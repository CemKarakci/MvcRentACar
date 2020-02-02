using MvcRentC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRentC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Dates dates)
        {
            if (ModelState.IsValid)
            {
                var storedDate = new Dates
                {
                    StartDate = dates.StartDate,
                    EndDate = dates.EndDate
                };
                this.Session["Dates"] = storedDate;
            }



            return RedirectToAction("SelectCars", "Cars");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}