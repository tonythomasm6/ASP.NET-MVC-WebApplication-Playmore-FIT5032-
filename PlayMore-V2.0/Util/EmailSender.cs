using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mail;

namespace PlayMore_V5._0.Util
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents, HttpPostedFileBase uploadFile)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("tonymanthuruthil@gmail.com", subject);
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);


            if(uploadFile != null) { 
                    //Attachment a = new Attachment (uploadFile.InputStream, System.IO.Path.GetFileName(uploadFile.FileName));
                    Attachment a = new Attachment();
                    a.Filename = uploadFile.FileName;

                    byte[] bytes = new byte[uploadFile.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(uploadFile.InputStream))
                    {
                        bytes = theReader.ReadBytes(uploadFile.ContentLength);
                    }
                    string thePictureDataAsString = Convert.ToBase64String(bytes);

                    a.Content = thePictureDataAsString;

                    msg.AddAttachment(a);
            }
            //msg.Attachments.Add(new Attachment(uploadFile.InputStream, System.IO.Path.GetFileName(uploadFile.FileName));

            var response = client.SendEmailAsync(msg);
        }



        public void SendBroadcast(List<String> toEmailList, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("tonymanthuruthil@gmail.com", "Playmore mail");
            var toEmailListObj = new List<EmailAddress>();
            foreach (String email in toEmailList)
            {
                toEmailListObj.Add(new EmailAddress(email, ""));
            }
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmailListObj, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

        }
    }
}