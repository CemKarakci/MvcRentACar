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
    public class ReservationsController : Controller
    {
        private RentCDataBaseEntities db = new RentCDataBaseEntities();

        // GET: Reservations
        [Authorize(Roles = "Admin,Manager,Sales")]
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            return View(reservations.ToList());
        }
        public ActionResult SortByLocation()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            var list = reservations.OrderBy(p => p.Location);
            return View(list.ToList());
        }
        public ActionResult SortByStartDate()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            var list = reservations.OrderBy(p => p.StartDate);
            return View(list.ToList());
        }
        public ActionResult SortByEndDate()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            var list = reservations.OrderBy(p => p.EndDate);
            return View(list.ToList());
        }
        public ActionResult SortByCustomerName()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            var list = reservations.OrderBy(p => p.Customers.Name);
            return View(list.ToList());
        }
        public ActionResult SortByPlate()
        {
            var reservations = db.Reservations.Include(r => r.Cars).Include(r => r.Coupons).Include(r => r.Customers).Include(r => r.ReservationStatuses);
            var list = reservations.OrderBy(p => p.Cars.Plate);
            return View(list.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }

        // GET: Reservations/Create

        public ActionResult CreateReservation()
        {
            var car = Session["Car"] as Cars;
            var price = car.PricePerDay;
            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate").Where(x => x.Text == car.Plate);
           

            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description");

            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name").Where(p => p.Text == "OPEN");


            if (Session["Customer"] != null)
            {
                var customer = Session["Customer"] as Customers;
                ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name").Where(p => p.Text == customer.Name);
            }

            else
            {
                var custom = Session["AlreadyCustomer"] as Customers;
                ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name").Where(p => p.Text == custom.Name);

            }

           

            var date = Session["Dates"] as Dates;
            ViewBag.StartDate = date.StartDate;
            ViewBag.EndDate = date.EndDate;
            var totaldays = date.EndDate.Day - date.StartDate.Day;

            var cost = price * totaldays;
            var totalCost = String.Format("{0:N2}", cost);
            ViewBag.TotalCost = totalCost;

            return View();
        }

        // POST: Reservations/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReservation([Bind(Include = "ReservationID,CarID,CostumerID,ReservStatsID,StartDate,EndDate,Location,CouponCode")] Reservations reservations)
        {
            var date = Session["Dates"] as Dates;
            ViewBag.StartDate = date.StartDate;
            ViewBag.EndDate = date.EndDate;
            var reservation1 = reservations;

            reservation1.ReservationID = reservations.ReservationID;
            reservation1.CarID = reservations.CarID;
            reservation1.ReservStatsID = reservations.ReservStatsID;
            reservation1.StartDate = date.StartDate;
            reservation1.EndDate = date.EndDate;
            reservation1.Location = reservations.Location;
            reservation1.CouponCode = reservation1.CouponCode;

            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation1);
                db.SaveChanges();
                return RedirectToAction("Complete");
                
            }

            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate", reservations.CarID);
            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description", reservations.CouponCode);
            ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name", reservations.CostumerID);
            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name", reservations.ReservStatsID);

            

            return View(reservations);
        }

        public ActionResult Complete()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate");
            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description");
            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name");
            ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name");

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,CarID,CostumerID,ReservStatsID,StartDate,EndDate,Location,CouponCode")] Reservations reservations)
        {
          
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate", reservations.CarID);
            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description", reservations.CouponCode);
            ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name", reservations.CostumerID);
            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name", reservations.ReservStatsID);

            return View(reservations);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate", reservations.CarID);
            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description", reservations.CouponCode);
            ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name", reservations.CostumerID);
            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name", reservations.ReservStatsID);
            return View(reservations);
        }

        // POST: Reservations/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,CarID,CostumerID,ReservStatsID,StartDate,EndDate,Location,CouponCode")] Reservations reservations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarID = new SelectList(db.Cars, "CarID", "Plate", reservations.CarID);
            ViewBag.CouponCode = new SelectList(db.Coupons, "CouponCode", "Description", reservations.CouponCode);
            ViewBag.CostumerID = new SelectList(db.Customers, "CostumerID", "Name", reservations.CostumerID);
            ViewBag.ReservStatsID = new SelectList(db.ReservationStatuses, "ReservStatsID", "Name", reservations.ReservStatsID);
            return View(reservations);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservations reservations = db.Reservations.Find(id);
            db.Reservations.Remove(reservations);
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
