using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace EventManagementSystem.Utilities
{
    public static class NotificationHelper
    {
        public static bool IsEmailConfigured()
        {
            string fromEmail = ConfigurationManager.AppSettings["Email"];
            string fromPassword = ConfigurationManager.AppSettings["Password"];

            if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(fromPassword))
                return false;

            if (fromEmail.Equals("your_email@gmail.com", StringComparison.OrdinalIgnoreCase))
                return false;

            if (fromPassword.Equals("your_app_password", StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public static void SendBookingStatusEmail(string toEmail, string userName, string eventName, DateTime eventDate,
            string venue, string eventMode, string meetingLink, int seats, string bookingStatus)
        {
            if (!IsEmailConfigured() || string.IsNullOrWhiteSpace(toEmail))
                return;

            string statusText = (bookingStatus ?? string.Empty).Trim();
            string fromEmail = ConfigurationManager.AppSettings["Email"];
            string fromPassword = ConfigurationManager.AppSettings["Password"];

            string formalName = string.IsNullOrWhiteSpace(userName) ? "Participant" : userName.Trim();
            string subject = "Booking " + statusText + " - " + eventName;
            string modeLine = string.Equals(eventMode, "Online", StringComparison.OrdinalIgnoreCase)
                ? "Meeting Link: " + (string.IsNullOrWhiteSpace(meetingLink) ? "Will be shared soon" : meetingLink)
                : "Venue: " + venue;

            string body = "Dear " + formalName + ",\n\n"
                + "Your booking status has been updated by admin.\n\n"
                + "Booking Status: " + statusText + "\n"
                + "Event: " + eventName + "\n"
                + "Date: " + eventDate.ToString("dd MMM yyyy hh:mm tt") + "\n"
                + modeLine + "\n"
                + "Seats: " + seats + "\n\n"
                + "Thank you,\n"
                + "Event Management Team";

            SendEmail(toEmail, subject, body, fromEmail, fromPassword);
        }

        public static void SendEventUpdatedEmail(string toEmail, string userName, string eventName, DateTime eventDate,
            string venue, string eventMode, string meetingLink, decimal price)
        {
            if (!IsEmailConfigured() || string.IsNullOrWhiteSpace(toEmail))
                return;

            string fromEmail = ConfigurationManager.AppSettings["Email"];
            string fromPassword = ConfigurationManager.AppSettings["Password"];
            string formalName = string.IsNullOrWhiteSpace(userName) ? "Participant" : userName.Trim();

            string modeLine = string.Equals(eventMode, "Online", StringComparison.OrdinalIgnoreCase)
                ? "Meeting Link: " + (string.IsNullOrWhiteSpace(meetingLink) ? "Will be shared soon" : meetingLink)
                : "Venue: " + venue;

            string body = "Dear " + formalName + ",\n\n"
                + "This is to inform you that event details have been updated by admin.\n\n"
                + "Event: " + eventName + "\n"
                + "Date: " + eventDate.ToString("dd MMM yyyy hh:mm tt") + "\n"
                + modeLine + "\n"
                + "Price: ₹" + price.ToString("0.00") + "\n\n"
                + "Please check your dashboard for latest details.\n\n"
                + "Regards,\n"
                + "Event Management Team";

            SendEmail(toEmail, "Event Updated - " + eventName, body, fromEmail, fromPassword);
        }

        private static void SendEmail(string toEmail, string subject, string body, string fromEmail, string fromPassword)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new MailAddress(fromEmail);

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
