using MvcRentC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRentC.Controllers
{
   
    [Authorize(Roles="Admin,Manager")]
    public class ReportsController : Controller
    {
        private RentCDataBaseEntities db = new RentCDataBaseEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GoldMembers()
        {
            var customers = db.Customers.ToList();
            var reservations = db.Reservations.ToList();

            var newList = reservations.GroupBy(x => x.CostumerID).Where(y => y.Count() > 3).Select(y => y.Key);

            var goldMembers = customers.FindAll(x => newList.Contains(x.CostumerID));
            return View(goldMembers);
        }
        public ActionResult SilverMembers()
        {
            var customers = db.Customers.ToList();
            var reservations = db.Reservations.ToList();
        
            var newList = reservations.GroupBy(x => x.CostumerID).
               Where(y => y.Count() > 1 && y.Count() < 4).Select(y => y.Key);

            var silverMembers = customers.FindAll(x => newList.Contains(x.CostumerID));
            return View(silverMembers);
        }
        public ActionResult MostRented()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MostRented(Dates dates)
        {
            if (ModelState.IsValid)
            {
                var storedDate = new Dates
                {
                    StartDate = dates.StartDate,
                    EndDate = dates.EndDate
                };
                this.Session["DatesRent"] = storedDate;
            }
            return RedirectToAction("ListCars", "Reports");
        }

       
        public ActionResult ListCars()
        {
            var dates = Session["DatesRent"] as Dates;
            var cars = db.Cars.ToList();
            var reservations = db.Reservations.ToList();
            
            var rezList = reservations.Where(p => p.StartDate >= dates.StartDate
            && p.EndDate <= dates.EndDate);

            var newList = rezList.GroupBy(x => x.CarID).OrderByDescending(y => y.Count()).Select(y => y.Key).Take(1).ToList();


            var mostRented = cars.FindAll(x => newList.Contains(x.CarID));

            return View(mostRented);
        }

        public ActionResult LeastRented()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeastRented(Dates date)
        {
            if (ModelState.IsValid)
            {
                var storedDate = new Dates
                {
                    StartDate = date.StartDate,
                    EndDate = date.EndDate
                };
                this.Session["DatesLeast"] = storedDate;
            }
            return RedirectToAction("LeastCars", "Reports");
        }

        public ActionResult LeastCars()
        {
            var date = Session["DatesLeast"] as Dates;
            var cars = db.Cars.ToList();
            var reservations = db.Reservations.ToList();
            var reserved = reservations.Select(p => p.CarID).ToList();

            var carList = cars.RemoveAll(x => reserved.Contains(x.CarID));


            if (cars.Count > 0)
            {
                return View(cars);
            }
            else
            {
                var cars1 = db.Cars.ToList();
                var reservations1 = db.Reservations.ToList();

                var rezList = reservations1.Where(p => p.StartDate > date.StartDate
                && p.EndDate < date.EndDate);

                var newList = rezList.GroupBy(x => x.CarID).OrderBy(y => y.Count()).Select(y => y.Key).Take(1).ToList();


                var leastRented = cars1.FindAll(x => newList.Contains(x.CarID));

                return View(leastRented);
            }

        }
    }
}