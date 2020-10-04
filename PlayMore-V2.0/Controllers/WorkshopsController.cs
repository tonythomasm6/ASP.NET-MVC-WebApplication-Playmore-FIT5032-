using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlayMore_V2._0.Models;

namespace PlayMore_V2._0.Controllers
{
    [Authorize(Roles = "admin")]
    public class WorkshopsController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Workshops
        public ActionResult Index()
        {
            var workshops1 = db.Workshops1.Include(w => w.Game).Include(w => w.Coach);
            return View(workshops1.ToList());
        }

        // GET: Workshops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops1.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // GET: Workshops/Create
        public ActionResult Create()
        {
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName");
            ViewBag.CoachCoachId = new SelectList(db.Coaches1, "CoachId", "CoachName");
            return View();
        }

        // POST: Workshops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkshopId,WorkshopDate,WorkshopLocation,GameGameId,CoachCoachId")] Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                db.Workshops1.Add(workshop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            ViewBag.CoachCoachId = new SelectList(db.Coaches1, "CoachId", "CoachName", workshop.CoachCoachId);
            return View(workshop);
        }

        // GET: Workshops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops1.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            ViewBag.CoachCoachId = new SelectList(db.Coaches1, "CoachId", "CoachName", workshop.CoachCoachId);
            return View(workshop);
        }

        // POST: Workshops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkshopId,WorkshopDate,WorkshopLocation,GameGameId,CoachCoachId")] Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workshop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            ViewBag.CoachCoachId = new SelectList(db.Coaches1, "CoachId", "CoachName", workshop.CoachCoachId);
            return View(workshop);
        }

        // GET: Workshops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops1.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // POST: Workshops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Workshop workshop = db.Workshops1.Find(id);
            db.Workshops1.Remove(workshop);
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
