using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using PlayMore_V5._0.Models;
using PlayMore_V5._0.Util;

namespace PlayMore_V5._0.Controllers
{
    [Authorize(Roles = "admin")]
    public class WorkshopsController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Workshops
        public ActionResult Index()
        {

            var workshops = db.Workshops.Include(w => w.Coach).Include(w => w.Game).ToList();
            var ActiveWorkshops = new List<Workshop>();
            foreach(Workshop w in workshops)
            {
                if(DateTime.Compare(w.WorkshopDate, DateTime.Now ) > 0)
                {
                    ActiveWorkshops.Add(w);
                }
            }

            return View(ActiveWorkshops);
        }

        // GET: Workshops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // GET: Workshops/Create
        public ActionResult Create()
        {
            ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName");
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName");
            return View();
        }

        // POST: Workshops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkshopId,WorkshopDate,WorkshopLocation,WSLocLattitude,WSLocLongitude,WorkShopFees,CoachCoachId,GameGameId")] Workshop workshop)
        {


            //Setting workshop time as 10am
            var WorkDate = workshop.WorkshopDate;
            TimeSpan ts = new TimeSpan(10, 0, 0);
            WorkDate = WorkDate.Date + ts;
            workshop.WorkshopDate = WorkDate;

            //To check if selected coach is of the game
            var SelectedCoach = db.Coaches.Find(workshop.CoachCoachId);
            
            if( SelectedCoach.GameGameId != workshop.GameGameId)
            {
                ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName");
                ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName");
                ViewBag.errormsg = "Selected Coach doesnot train " + db.Games.Find(workshop.GameGameId).GameName+ ". Please change coach !";
                return View();
            }


            if (ModelState.IsValid)
            {
                db.Workshops.Add(workshop);
                db.SaveChanges();
                return RedirectToAction("CreateFeedback");
                // return RedirectToAction("Index");
            }

            ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName", workshop.CoachCoachId);
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            return View(workshop);
        }

        public ActionResult CreateFeedback()
        {
            return View();
        }


        // GET: Workshops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName", workshop.CoachCoachId);
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            return View(workshop);
        }

        // POST: Workshops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkshopId,WorkshopDate,WorkshopLocation,WSLocLattitude,WSLocLongitude,WorkShopFees,CoachCoachId,GameGameId")] Workshop workshop)
        {
            //Setting workshop time as 10am
            var WorkDate = workshop.WorkshopDate;
            TimeSpan ts = new TimeSpan(10, 0, 0);
            WorkDate = WorkDate.Date + ts;
            workshop.WorkshopDate = WorkDate;


            if (ModelState.IsValid)
            {
                db.Entry(workshop).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName", workshop.CoachCoachId);
                ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
                ViewBag.editmsg = "Workshop Details edited successfully !!!";
                return View(workshop);

               // return RedirectToAction("Index");
            }
            ViewBag.CoachCoachId = new SelectList(db.Coaches, "CoachId", "CoachFName", workshop.CoachCoachId);
            ViewBag.GameGameId = new SelectList(db.Games, "GameId", "GameName", workshop.GameGameId);
            return View(workshop);
        }

        // GET: Workshops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
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
            Workshop workshop = db.Workshops.Find(id);

            //Check if bookings are present for the workshop

            var bookings = db.Bookings.Where(b => b.WorkshopWorkshopId == id).ToList();

            if (bookings.Count() > 1)
            {
                ViewBag.deleteError = "Bookings present for the Selected Workshop. Delete them to delete the workshop";
                return View(workshop);
            }

            db.Workshops.Remove(workshop);
            db.SaveChanges();

            return RedirectToAction("DeleteFeedback");
            //return RedirectToAction("Index");
        }


        public ActionResult DeleteFeedback()
        {
            return View();
        }

        public ActionResult BoradcastEmail()
        {
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var list = new List<String>();
            foreach (var u in userStore.Users)
            {
                list.Add(u.Email);
            }
            ViewBag.listEmail = list;

            return View();
        }

        public ActionResult SendBoradcastEmail()
        {
            // var v = User.Identity.Name;
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var list = new List<String>();
            foreach (var u in userStore.Users)
            {
                list.Add(u.Email);
            }

            List<String> toEmailList = list;
            String subject ="Playmore: Upcoming workshops";

            //Making content
            var Workshops = db.Workshops.ToList();

            String content = "<table border='1'>";
            content = content + "<tr><th>Workshop Date</th><th> Workshop Location</th>" +
                "<th>Workshop Fees</th><th>Game</th>";
            foreach (Workshop w in Workshops)
            {
                content = content + "<tr><td>" + w.WorkshopDate + "</td><td>" + w.WorkshopLocation + "</td><td>" 
                    + w.WorkShopFees + "</td><td>" +
                    w.Game.GameName + "</td>" ;
            }
            content = content + "<table>";


            EmailSender es = new EmailSender();
            es.SendBroadcast(toEmailList, subject, content);


            return RedirectToAction("Index");
        }

        public ActionResult BoradcastEmailSuccess()
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
