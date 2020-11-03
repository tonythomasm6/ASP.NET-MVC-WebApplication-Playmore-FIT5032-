using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PlayMore_V5._0.Models;
using PlayMore_V5._0.Util;

namespace PlayMore_V5._0.Controllers
{
    public class BookingsController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {

            var userID = User.Identity.GetUserId();
            var book = db.Bookings.Where(b => b.BookedBy_Userid == userID).ToList();
            //var bookings = db.Bookings.Include(b => b.Workshop);

            //Selecting only bookings of future
            var futureBook = new List<Booking>();
            foreach(Booking b in book)
            {
                if(DateTime.Compare(b.Workshop.WorkshopDate, DateTime.Now) > 0)
                {
                    futureBook.Add(b);
                }
            }

            if (User.IsInRole("admin"))
            {
                var b = db.Bookings.Include(k=>k.Workshop.Game);
                book = db.Bookings.ToList();
                foreach (Booking b2 in book)
                {
                    if (DateTime.Compare(b2.Workshop.WorkshopDate, DateTime.Now) > 0)
                    {
                        futureBook.Add(b2);
                    }
                }
            }

            return View(futureBook);
        }
        [Authorize(Roles = "user")]
        public ActionResult UserWSList()
        {
            var userID = User.Identity.GetUserId();
            var book = db.Bookings.Where(b => b.BookedBy_Userid == userID).ToList();
            //var workshops1 = db.Workshops.Include(w => w.Game).Include(w => w.Coach);

            var workshops1 = db.Workshops.Include(w => w.Coach).Include(w => w.Game).ToList();
            var ActiveWorkshops = new List<Workshop>();
            foreach (Workshop w in workshops1)
            {
                if (DateTime.Compare(w.WorkshopDate, DateTime.Now) > 0)
                {
                    ActiveWorkshops.Add(w);
                }
            }



            return View(ActiveWorkshops);
        }

        [Authorize(Roles = "user")]
        public ActionResult UserConfirmBook(int? id)
        {
            var ws = db.Workshops.Find(id);

            Booking b = new Booking();
            b.BookedBy_Userid = User.Identity.GetUserId();

            b.WorkshopWorkshopId = ws.WorkshopId;


            ViewBag.WSLocation = ws.WorkshopLocation;
            ViewBag.WSDate = ws.WorkshopDate;



            return View(b);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult UserConfirmBook([Bind(Include = "BookFName,BookLName,BookAge,WorkshopWorkshopId")] Booking booking)
        {
            booking.BookedBy_Userid = User.Identity.GetUserId();

            //Check for existing booking if any
            var selectedWS = db.Workshops.Where(w=>w.WorkshopId== booking.WorkshopWorkshopId).ToList();

            var allBooks = db.Bookings.ToList();


            foreach(Booking bookDB in allBooks) {

                if(bookDB.BookedBy_Userid == booking.BookedBy_Userid) { 
                    if(booking.WorkshopWorkshopId == bookDB.WorkshopWorkshopId && booking.BookFName == bookDB.BookFName 
                            && booking.BookLName == bookDB.BookLName && booking.BookAge == bookDB.BookAge)
                    {
                        ViewBag.errorMsg = "Already booked for the same workshop !!!";
                        return View();
                    }
                    else if(selectedWS[0].WorkshopDate == bookDB.Workshop.WorkshopDate && booking.BookFName == bookDB.BookFName
                            && booking.BookLName == bookDB.BookLName && booking.BookAge == bookDB.BookAge)
                    {
                        ViewBag.errorMsg = "Already booked for a workshop same day !!!";
                        return View();
                    }
                }
            }

            //End of checking


            //Check for number of bookings present for the particular user
            var bookingsForUser = db.Bookings.Where(b => b.BookedBy_Userid == booking.BookedBy_Userid).ToList();
            var futureBook = new List<Booking>();
            foreach (Booking b in bookingsForUser)
            {
                if (DateTime.Compare(b.Workshop.WorkshopDate, DateTime.Now) > 0)
                {
                    futureBook.Add(b);
                }
            }

            if (futureBook.Count >= AppConstants.MaximumBookings)
            {
                ViewBag.errorMsg = "User can only book a maximum of "+AppConstants.MaximumBookings+" bookings";
                return View();
            }



            //If no booking present then saving in DB

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();


                //On success save, sending confirmation email to user
                String toEmail = User.Identity.Name ;
                String subject = "Playmore booking confirmation";

                //Making content (Mail body) for booking confirmation
                String content = "";
                content += "<p>Booking confirmed.</p><br / > <p>Booking details are as follows:</p>";

                content += "<table border='1'><tr><th>Workshop Date:</th><td>" + selectedWS[0].WorkshopDate + "</td></tr>"
                            + "<tr><th>First Name :</th><td>" + booking.BookFName + "</td></tr>"
                            + "<tr><th>Last Name :</th><td>" + booking.BookLName + "</td></tr>"
                            + "<tr><th>Game :</th><td>" + selectedWS[0].Game.GameName + "</td></tr>"
                            + "<tr><th>WorkShop Location :</th><td>" + selectedWS[0].WorkshopLocation + "</td></tr>"
                            + "<tr><th>WorkShop Fees :</th><td>" + selectedWS[0].WorkShopFees + "</td></tr>";

                content += "</table>";

                EmailSender es = new EmailSender();
                es.Send(toEmail, subject, content, null);


                return RedirectToAction("CreateFeedback");
                //return RedirectToAction("Index");
            }


            return View();
        }

        public ActionResult CreateFeedback()
        {
            return View();
        }

        public ActionResult SetRating(int? id)
        {
            var rating = id;

            var UserName = User.Identity.Name;
            var NewRating = new Rating();
            NewRating.RatingGiven = (int)rating;
            NewRating.UserName = UserName;

            db.Ratings.Add(NewRating);
            db.SaveChanges();

            return RedirectToAction("Index");


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
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops, "WorkshopId", "WorkshopLocation");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "BookId,BookedBy_Userid,BookFName,BookLName,BookAge,WorkshopWorkshopId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
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
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "BookId,BookedBy_Userid,BookFName,BookLName,BookAge,WorkshopWorkshopId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
                ViewBag.editMsg = "Booking details edited successfully !!";
                return View(booking);


                //return RedirectToAction("Index");
            }
            ViewBag.WorkshopWorkshopId = new SelectList(db.Workshops, "WorkshopId", "WorkshopLocation", booking.WorkshopWorkshopId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize]
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

            return RedirectToAction("DeleteFeedback");
            //return RedirectToAction("Index");
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


        //Sending mail 
        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }


        //Method to send email with attachment : Charts
        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;
                    //String contents = model.Upload.FileName;

                   HttpPostedFileBase file = model.Upload;

                var bookings = db.Bookings.ToList();
                   
                   
                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents, file);

                    ViewBag.Result = "Email has been send successfully !!!!";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }



        public ActionResult Charts()
        {
            var booksObjectList = db.Bookings.ToList();
            
            var gameObjectList = db.Games.ToList();
            var gameBookingCount = new Dictionary<String, int>();

            var gameNameList = new List<String>();
            var gameCount = new List<int>();

            foreach (Game g in gameObjectList)
            {
                int count = 0;
                foreach (Booking b in booksObjectList)
                {
                    
                    if(b.Workshop.Game.GameName == g.GameName)
                    {
                        count += 1;
                    }

                }
                gameNameList.Add(g.GameName);
                gameCount.Add(count);

            }


            var points = new List<ChartPoint>();
            var xVal = 10;
            for (var i = 0; i < gameCount.Count; i++)
            {
                var c = new ChartPoint(gameNameList[i], xVal, gameCount[i]);
                xVal += 10;
                points.Add(c);
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(points);




            //End

            return View();
        }


    }

}