﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlayMore_V5._0.Models;

namespace PlayMore_V5._0.Controllers
{
    [Authorize(Roles = "admin")]
    public class CoachesController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Coaches
        public ActionResult Index()
        {
            var coaches = db.Coaches.Include(c => c.Game);
            return View(coaches.ToList());
        }

        // GET: Coaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // GET: Coaches/Create
        public ActionResult Create()
        {
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName");
            return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoachId,CoachFName,CoachLName,CoachEmail,GameGameId")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                db.Coaches.Add(coach);
                db.SaveChanges();

                return RedirectToAction("CreateFeedback");

                //return RedirectToAction("Index");
            }

            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", coach.GameGameId);
            return View(coach);
        }

        public ActionResult CreateFeedback()
        {
            return View();
        }

        // GET: Coaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", coach.GameGameId);
            return View(coach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoachId,CoachFName,CoachLName,CoachEmail,GameGameId")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coach).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.feedBackMsg = "Coach Details edited successfully !!";
                //return RedirectToAction("Index");
                ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", coach.GameGameId);
                return View(coach);
            }
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", coach.GameGameId);
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coach coach = db.Coaches.Find(id);
            db.Coaches.Remove(coach);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("DeleteFeedback");
        }

        public ActionResult DeleteFeedback()
        {
            return View();
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
