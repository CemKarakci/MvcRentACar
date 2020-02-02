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
    [Authorize(Roles="Admin,Manager")]
    public class ReservationStatusesController : Controller
    {
        private RentCDataBaseEntities db = new RentCDataBaseEntities();

        // GET: ReservationStatuses
        public ActionResult Index()
        {
            return View(db.ReservationStatuses.ToList());
        }

        // GET: ReservationStatuses/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservationStatuses reservationStatuses = db.ReservationStatuses.Find(id);
            if (reservationStatuses == null)
            {
                return HttpNotFound();
            }
            return View(reservationStatuses);
        }

        // GET: ReservationStatuses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationStatuses/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservStatsID,Name,Description")] ReservationStatuses reservationStatuses)
        {
            if (ModelState.IsValid)
            {
                db.ReservationStatuses.Add(reservationStatuses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reservationStatuses);
        }

        // GET: ReservationStatuses/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservationStatuses reservationStatuses = db.ReservationStatuses.Find(id);
            if (reservationStatuses == null)
            {
                return HttpNotFound();
            }
            return View(reservationStatuses);
        }

        // POST: ReservationStatuses/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservStatsID,Name,Description")] ReservationStatuses reservationStatuses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservationStatuses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservationStatuses);
        }

        // GET: ReservationStatuses/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservationStatuses reservationStatuses = db.ReservationStatuses.Find(id);
            if (reservationStatuses == null)
            {
                return HttpNotFound();
            }
            return View(reservationStatuses);
        }

        // POST: ReservationStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            ReservationStatuses reservationStatuses = db.ReservationStatuses.Find(id);
            db.ReservationStatuses.Remove(reservationStatuses);
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
