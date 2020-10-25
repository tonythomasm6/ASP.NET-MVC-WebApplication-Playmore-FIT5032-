using Microsoft.AspNet.Identity.EntityFramework;
using PlayMore_V5._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace PlayMore_V5._0.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private Model1Container db = new Model1Container();


        public ActionResult Index()
        {

            /* using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
             {
                 Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                 PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                 document.Open();

                 Chunk chunk = new Chunk("This is from chunk. ");
                 document.Add(chunk);

                 Phrase phrase = new Phrase("This is from Phrase.");
                 document.Add(phrase);

                 Paragraph para = new Paragraph("This is from paragraph.");
                 document.Add(para);

                 string text =  "you are successfully created PDF file.";
                 Paragraph paragraph = new Paragraph();
                 paragraph.SpacingBefore = 10;
                 paragraph.SpacingAfter = 10;
                 paragraph.Alignment = Element.ALIGN_LEFT;
                 paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
                 paragraph.Add(text);
                 document.Add(paragraph);

                 document.Close();
                 byte[] bytes = memoryStream.ToArray();
                 memoryStream.Close();
                 Response.ClearHeaders();
                 Response.Clear();
                 Response.ContentType = "application/pdf";

                 string pdfName = "User";
                 Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                 Response.ContentType = "application/pdf";
                 Response.Buffer = true;
                 Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                 Response.BinaryWrite(bytes);
                 Response.End();
                 Response.Close();
             }
            */
            /*
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                Paragraph Text = new Paragraph("This is test file");
                pdfDoc.Add(Text);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        
            */
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserRatings()
        {
            var AllRatings = db.Ratings.ToList();
            var OneStarCount = 0;
            var TwoStarCount = 0;
            var ThreeStarCount = 0;
            var FourStarCount = 0;
            var FiveStarCount = 0;

            foreach (Rating r in AllRatings)
            {
                if(r.RatingGiven == 5)
                {
                    FiveStarCount += 1;

                }else if(r.RatingGiven == 4)
                {
                    FourStarCount += 1;
                }
                else if (r.RatingGiven == 3)
                {
                    ThreeStarCount += 1;
                }
                else if (r.RatingGiven == 2)
                {
                    TwoStarCount += 1;
                }
                else if (r.RatingGiven == 1)
                {
                    OneStarCount += 1;
                }
            }

            var points = new List<ChartPoint>();
            
            ChartPoint p = new ChartPoint("1 Star", 10, OneStarCount);
            points.Add(p);
            p = new ChartPoint("2 Star", 20, TwoStarCount);
            points.Add(p);
            p = new ChartPoint("3 Star", 30, ThreeStarCount);
            points.Add(p);
            p = new ChartPoint("4 Star", 40, FourStarCount);
            points.Add(p);
            p = new ChartPoint("5 Star", 50, FiveStarCount);
            points.Add(p);
            ViewBag.DataPoints = JsonConvert.SerializeObject(points);


            return View();
        }


        //Method to export pdf of statistics
        public void ExportPDF()
        {
            //Getting number of ratings per star.
            var AllRatings = db.Ratings.ToList();
            var OneStarCount = 0;
            var TwoStarCount = 0;
            var ThreeStarCount = 0;
            var FourStarCount = 0;
            var FiveStarCount = 0;

            foreach (Rating r in AllRatings)
            {
                if (r.RatingGiven == 5)
                {
                    FiveStarCount += 1;

                }
                else if (r.RatingGiven == 4)
                {
                    FourStarCount += 1;
                }
                else if (r.RatingGiven == 3)
                {
                    ThreeStarCount += 1;
                }
                else if (r.RatingGiven == 2)
                {
                    TwoStarCount += 1;
                }
                else if (r.RatingGiven == 1)
                {
                    OneStarCount += 1;
                }
            }

            //Counting number of bookings
            var NoOfBookings = db.Bookings.ToList().Count();

            //Counting number of registered users
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var NoOfUsers = userStore.Users.Count();
           
            //Method to export PDF
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                /*
                Chunk chunk = new Chunk("PlayMore");
                document.Add(chunk);

                
                Phrase phrase = new Phrase("User Bookings Statistics.");
                document.Add(phrase);
                */
                string text = "PlayMore";
                Paragraph paragraph = new Paragraph
                {
                    SpacingBefore = 10,
                    SpacingAfter = 10,
                    Alignment = Element.ALIGN_LEFT,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, BaseColor.BLUE)
                };
                paragraph.Add(text);
                document.Add(paragraph);




                Paragraph para = new Paragraph("Total number of registered users: "+ NoOfUsers);
                document.Add(para);
                para = new Paragraph("Total Number of bookings: "+NoOfBookings);
                document.Add(para);
                para = new Paragraph("Number of 5 Star ratings: "+ FiveStarCount);
                document.Add(para);
                para = new Paragraph("Number of 4 Star ratings: " + FourStarCount);
                document.Add(para);
                para = new Paragraph("Number of 3 Star ratings: " + ThreeStarCount);
                document.Add(para);
                para = new Paragraph("Number of 2 Star ratings: " + TwoStarCount);
                document.Add(para);
                para = new Paragraph("Number of 1 Star ratings: " + OneStarCount);
                document.Add(para);

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.ClearHeaders();
                Response.Clear();
                Response.ContentType = "application/pdf";

                string pdfName = "PlayMoreStats";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }

        }
    }
}