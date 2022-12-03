using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Text;
using System.Net.Mime;
using Outlook = Microsoft.Office.Interop.Outlook;
using netCoreApi.ApiModels;

namespace netCoreApi.Service
{
    public class Email
    {
        public bool SendEmail(Users user)
        {
            bool success = false;
            var fromEmail = new MailAddress("panos.vlahos@hotmail.com", "Title");
            var toEmail = new MailAddress(user.Email);

            const string subject = "Register";
            const string body = "Register successful";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("panos.vlahos100@gmail.com", "uexlundnmgovnrkg")
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body
            })
                try
                {
                    smtp.Send(message);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }
            return success;
        }
        public bool SendInvitationEmail()
        {
            bool success = false;
            try
            {
                string _from = "panos.vlahos100@gmail.com";
                string _subj = "TEST EMAIL " + DateTime.Now.ToString("G");
                string _body = "Hi Mark,<br/><br/>THIS IS A TEST EMAIL<br/><br/>WITH A Calendar Invite and File Attachment<br/><br/>Regards<br/>TESTER";
                _body = CheckWellFormedHtml(_body);

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(_from);
                msg.To.Add("panos.vlahos@hotmail.com");
                //msg.To.Add("email2");
                msg.Subject = _subj;

                AlternateView avBody = AlternateView.CreateAlternateViewFromString(_body, Encoding.UTF8, MediaTypeNames.Text.Html);
                msg.AlternateViews.Add(avBody);


                // Generate Calendar Invite ---------------------------------------------------
                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");
                str.AppendLine("PRODID:-//Schedule a Meeting");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");
                str.AppendLine("BEGIN:VEVENT");
                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+330)));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+660)));
                str.AppendLine("LOCATION: " + "abcd");
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
                str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");


                // Attach Calendar Invite ------------------------------------------------------
                byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
                MemoryStream stream = new MemoryStream(byteArray);

                Attachment attach = new Attachment(stream, "invite.ics");
                attach.TransferEncoding = TransferEncoding.QuotedPrintable;
                msg.Attachments.Add(attach);

                ContentType contype = new ContentType("text/calendar");
                contype.CharSet = "UTF-8";
                contype.Parameters.Add("method", "REQUEST");
                contype.Parameters.Add("name", "invite.ics");

                AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
                avCal.TransferEncoding = TransferEncoding.QuotedPrintable;
                msg.AlternateViews.Add(avCal);


                // File Attachment --------------------------------------------------------------
                string filePath = @"C:\TESTFile.txt";
                string fileName = Path.GetFileName(filePath);
                byte[] bytes = File.ReadAllBytes(filePath);
                msg.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName));


                //Now sending a mail with attachment ICS file. ----------------------------------
                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Host = "smtp.gmail.com"; //-------this has to given the Mailserver IP
                smtpclient.EnableSsl = true;
                smtpclient.Port = 587;
                smtpclient.Credentials = new System.Net.NetworkCredential(_from, "uexlundnmgovnrkg");
                smtpclient.Send(msg);
                Console.WriteLine("Email Sent");
                Console.ReadLine();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
       
        public static string CheckWellFormedHtml(string txt)
        {
            if (txt == null)
                return "";
            else
            {
                StringBuilder htmlTop = new StringBuilder();
                StringBuilder htmlBottom = new StringBuilder();

                if (!txt.ToUpper().Contains("<!DOCTYPE"))
                {
                    // If the txt already contains a doc type then we assume that the rest of the html is valid

                    htmlTop.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" ");
                    htmlTop.Append("\"http://www.w3.org/TR/html4/loose.dtd\">");
                    // Html at the top of the email
                    if (!txt.ToUpper().Contains("<HTML>") & !txt.ToUpper().Contains("< HTML>") & !txt.ToUpper().Contains("<HTML >") & !txt.ToUpper().Contains("< HTML >"))
                        htmlTop.Append("<html>");
                    if (!txt.ToUpper().Contains("<HEAD>") & !txt.ToUpper().Contains("< HEAD>") & !txt.ToUpper().Contains("<HEAD >") & !txt.ToUpper().Contains("< HEAD >"))
                        htmlTop.Append("<head>");
                    if (!txt.ToUpper().Contains("<TITLE>") & !txt.ToUpper().Contains("< TITLE>") & !txt.ToUpper().Contains("<TITLE >") & !txt.ToUpper().Contains("< TITLE >"))
                        htmlTop.Append("<title>Untitled Document</title>");
                    if (!txt.ToUpper().Contains("</HEAD>") & !txt.ToUpper().Contains("</ HEAD>") & !txt.ToUpper().Contains("</HEAD >") & !txt.ToUpper().Contains("</ HEAD >"))
                        htmlTop.Append("</head>");
                    if (!txt.ToUpper().Contains("<BODY>") & !txt.ToUpper().Contains("< BODY>") & !txt.ToUpper().Contains("<BODY >") & !txt.ToUpper().Contains("< BODY >"))
                        htmlTop.Append("<body>");

                    // Html at the bottom of the email
                    if (!txt.ToUpper().Contains("</BODY>") & !txt.ToUpper().Contains("</ BODY>") & !txt.ToUpper().Contains("</BODY >") & !txt.ToUpper().Contains("</ BODY >"))
                        htmlBottom.Append("</body>");
                    if (!txt.ToUpper().Contains("</HTML>") & !txt.ToUpper().Contains("</ HTML>") & !txt.ToUpper().Contains("</HTML >") & !txt.ToUpper().Contains("</ HTML >"))
                        htmlBottom.Append("</html>");

                    txt = htmlTop.ToString() + txt + htmlBottom.ToString();
                }

                return txt;
            }
        }

        public bool SendConfirmEmail(string token, string email,string urlHost,int? urlPort)
        {
            bool success = false;
            var fromEmail = new MailAddress("panos.vlahos@hotmail.com", "Title");
            var toEmail = new MailAddress(email);
            string baseUri ="";
            if (urlPort != null)
                baseUri = $"https://{urlHost}:{urlPort}";
            else
                baseUri = $"https://{urlHost}";

            const string subject = "Register";
            string body = $"{baseUri}/email/ConfirmEmail?token={token}";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("panos.vlahos100@gmail.com", "uexlundnmgovnrkg")
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body
            })
                try
                {
                    smtp.Send(message);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }
            //var x = SendInvitationEmail();
            return success;
        }
        //private static string MeetingRequestString(string from, List<string> toUsers, string subject, string desc, string location, DateTime startTime, DateTime endTime, int? eventID = null, bool isCancel = false)
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine("BEGIN:VCALENDAR");
        //    str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine(string.Format("METHOD:{0}", (isCancel ? "CANCEL" : "REQUEST")));
        //    str.AppendLine("BEGIN:VEVENT");

        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
        //    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.Now));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime.ToUniversalTime()));
        //    str.AppendLine(string.Format("LOCATION: {0}", location));
        //    str.AppendLine(string.Format("UID:{0}", (eventID.HasValue ? "blablabla" + eventID : Guid.NewGuid().ToString())));
        //    str.AppendLine(string.Format("DESCRIPTION:{0}", desc.Replace("\n", "<br>")));
        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", desc.Replace("\n", "<br>")));
        //    str.AppendLine(string.Format("SUMMARY:{0}", subject));

        //    str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", from, from));
        //    str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", string.Join(",", toUsers), string.Join(",", toUsers)));

        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");

        //    return str.ToString();
        //}
    }
}
