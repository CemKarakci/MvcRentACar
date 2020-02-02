using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcRentC.Models;

namespace MvcRentC.Controllers
{
        [HandleError]
        public class CarsController : Controller
        {
            private RentCDataBaseEntities db = new RentCDataBaseEntities();

            // GET: Cars
            public ActionResult SelectCars()
            {
                var dates = Session["Dates"] as Dates;

                var cars = db.Cars.ToList();
                var reservations = db.Reservations.ToList();
                var statuses = db.ReservationStatuses.ToList();
                var canceledReservations = statuses.FindAll(x => x.Name == "CANCELED").ToList();
                var result = reservations.Where(p => !canceledReservations.Any(x => x.ReservStatsID == p.ReservStatsID)).ToList();
                var carsRented = from b in result
                                 where
                                         ((dates.StartDate >= b.StartDate) && (dates.StartDate <= b.EndDate)) ||
                                         ((dates.EndDate >= b.StartDate) && (dates.EndDate <= b.EndDate)) ||
                                         ((dates.StartDate <= b.StartDate) && (dates.EndDate >= b.StartDate) && (dates.EndDate <= b.EndDate)) ||
                                         ((dates.StartDate >= b.StartDate) && (dates.StartDate <= b.EndDate) && (dates.EndDate >= b.EndDate)) ||
                                         ((dates.StartDate <= b.StartDate) && (dates.EndDate >= b.EndDate))
                                 select b;

                var availableCars = cars.Where(r => !carsRented.Any(b => b.CarID == r.CarID)).ToList();

                return View(availableCars);
            }
            [Authorize(Roles="Admin,Manager,Sales")]
            public ActionResult Index()
            {

                var cars = db.Cars.ToList();
                return View(cars);
            }

            public ActionResult SelectDetails(int? id)
            {
               Cars cars = db.Cars.Find(id);

            if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                if (cars == null)
                {
                    return HttpNotFound();
                }
                return View(cars);
            }



            public ActionResult Select(int? id)
            {

                Cars cars = db.Cars.Find(id);
                Session["Car"] = cars;

                return RedirectToAction("CreateClient", "Customers");

            }

        public ActionResult SortByPlate()
        {
            var cars = db.Cars.ToList();
            var list = cars.OrderBy(p => p.Plate);

            return View(list.ToList());
        }
        public ActionResult SortById()
        {
            var cars = db.Cars.ToList();
            var list = cars.OrderBy(p => p.CarID);

            return View(list.ToList());
        }
        public ActionResult SortByManufacturer()
        {
            var cars = db.Cars.ToList();
            var list = cars.OrderBy(p => p.Manufacturer);
            return View(list.ToList());
        }

        public ActionResult SortByModel()
        {
            var cars = db.Cars.ToList();
            var list = cars.OrderBy(p => p.Model);
            return View(list.ToList());
        }


        // GET: Cars/Details/5
        public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cars cars = db.Cars.Find(id);
                if (cars == null)
                {
                    return HttpNotFound();
                }
                return View(cars);
            }

            // GET: Cars/Create
            [Authorize(Roles = "Admin, Manager")]

            public ActionResult Create()
            {
                return View();
            }

            // POST: Cars/Create
            // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
            // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "CarID,Plate,Manufacturer,Model,PricePerDay")] Cars cars)
            {
                if (ModelState.IsValid)
                {
                    db.Cars.Add(cars);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(cars);
            }

            // GET: Cars/Edit/5
            [Authorize(Roles = "Admin, Manager")]
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cars cars = db.Cars.Find(id);
                if (cars == null)
                {
                    return HttpNotFound();
                }
                return View(cars);
            }

            // POST: Cars/Edit/5
            // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
            // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "CarID,Plate,Manufacturer,Model,PricePerDay")] Cars cars)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cars).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(cars);
            }

            // GET: Cars/Delete/5
            [Authorize(Roles = "Admin , Manager")]
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cars cars = db.Cars.Find(id);
                if (cars == null)
                {
                    return HttpNotFound();
                }
                return View(cars);
            }

            // POST: Cars/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                Cars cars = db.Cars.Find(id);
                db.Cars.Remove(cars);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
}
