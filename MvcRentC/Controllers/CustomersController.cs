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
    public class CustomersController : Controller
    {
        private RentCDataBaseEntities db = new RentCDataBaseEntities();

        // GET: Customers
        [Authorize(Roles="Admin,Manager,Sales")]
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult SortById()
        {
            var customers = db.Customers.ToList();
            var list = customers.OrderBy(p => p.CostumerID);
            return View(list.ToList());
        }
        public ActionResult SortByName()
        {
            var customers = db.Customers.ToList();
            var list = customers.OrderBy(p => p.Name);
            return View(list.ToList());
        }
        public ActionResult SortByBirthDay()
        {
            var customers = db.Customers.ToList();
            var list = customers.OrderBy(p => p.BirthDate);
            return View(list.ToList());
        }
        public ActionResult SortByLocation()
        {
            var customers = db.Customers.ToList();
            var list = customers.OrderBy(p => p.Location);
            return View(list.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Customers/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CostumerID,Name,BirthDate,Location")] Customers customers)
        {

            if (ModelState.IsValid)
            {
                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        public ActionResult CreateClient()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClient([Bind(Include = "CostumerID,Name,BirthDate,Location")] Customers customers)
        {
            
           
                var storedCustomer = new Customers
                {
                    CostumerID = customers.CostumerID,
                    Name = customers.Name,
                    BirthDate = customers.BirthDate,
                    Location = customers.Location
                };
                Session["Customer"] = storedCustomer;
                if (ModelState.IsValid)
                {
                    db.Customers.Add(customers);
                    db.SaveChanges();
                    return RedirectToAction("CreateReservation", "Reservations");
                }
            

            return View(customers);
        }



        [HttpPost]
        public ActionResult Submit(Customers customers)
        {
           
                var customer = db.Customers.Where(p => p.Name == customers.Name).FirstOrDefault();
                Session["AlreadyCustomer"] = customer;


                return RedirectToAction("CreateReservation", "Reservations");
            
            
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CostumerID,Name,BirthDate,Location")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
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
