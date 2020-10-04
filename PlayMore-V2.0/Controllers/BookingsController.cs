using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PlayMore_V2._0.Models;

namespace PlayMore_V2._0.Controllers
{
    
    public class BookingsController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Bookings

        [Authorize(Roles ="user")]
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var book = db.Bookings.Where(b => b.BookedBy_Userid == userID).ToList();
            //var bookings = db.Bookings.Include(b => b.Workshop);
            return View(book);
        }

        [Authorize(Roles = "user")]
        public ActionResult UserWSList()
        {
            var userID = User.Identity.GetUserId();
            var book = db.Bookings.Where(b => b.BookedBy_Userid == userID).ToList();
            var workshops1 = db.Workshops1.Include(w => w.Game).Include(w => w.Coach);
            return View(workshops1.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult UserConfirmBook(int? id)
        {
            var ws = db.Workshops1.Find(id);

            Booking b = new Booking();
            b.BookedBy_Userid = User.Identity.GetUserId();
            b.WorkshopWorkshopId = ws.WorkshopId ;

            ViewBag.WSLocation = ws.WorkshopLocation;
            ViewBag.WSDate = ws.WorkshopDate;
           
            


            return View(b);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult UserConfirmBook([Bind(Include = "Name,WorkshopWorkshopId")] Booking booking)
        {
            booking.BookedBy_Userid = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View();
        }


        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize(Roles ="admin")]
        public ActionResult Create()
        {
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops1, "WorkshopId", "WorkshopLocation");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "BookId,BookedBy_Userid,Name,WorkshopWorkshopId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops1, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops1, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "BookId,BookedBy_Userid,Name,WorkshopWorkshopId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops1, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "user")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
