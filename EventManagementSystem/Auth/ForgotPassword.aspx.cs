using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using EventManagementSystem.DAL;

namespace EventManagementSystem.Auth
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private const int OtpCooldownSeconds = 60;

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMsg.Text = "Enter a valid email.";
                lblMsg.CssClass = "error";
                return;
            }

            string email = Regex.Replace(txtEmail.Text ?? string.Empty, @"\s+", string.Empty).Trim().ToLowerInvariant();
            txtEmail.Text = email;

            if (string.IsNullOrEmpty(email))
            {
                lblMsg.Text = "Enter email.";
                lblMsg.CssClass = "error";
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                lblMsg.Text = "Enter a valid email.";
                lblMsg.CssClass = "error";
                return;
            }

            string cooldownKey = "ForgotOtpLastSent_" + email;
            object lastSentObj = Session[cooldownKey];
            if (lastSentObj is DateTime)
            {
                DateTime lastSentUtc = ((DateTime)lastSentObj).ToUniversalTime();
                double secondsPassed = (DateTime.UtcNow - lastSentUtc).TotalSeconds;
                if (secondsPassed < OtpCooldownSeconds)
                {
                    int waitSeconds = (int)Math.Ceiling(OtpCooldownSeconds - secondsPassed);
                    lblMsg.Text = "Please wait " + waitSeconds + " seconds before requesting OTP again.";
                    lblMsg.CssClass = "error";
                    return;
                }
            }

            using (SqlConnection con = DBHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand existsCmd = new SqlCommand("sp_UserExists", con))
                {
                    existsCmd.CommandType = CommandType.StoredProcedure;
                    existsCmd.Parameters.AddWithValue("@Email", email);
                    int existingCount = Convert.ToInt32(existsCmd.ExecuteScalar());
                    if (existingCount == 0)
                    {
                        lblMsg.Text = "Email is not registered. Please register first.";
                        lblMsg.CssClass = "error";
                        return;
                    }
                }

                if (!IsEmailConfigured())
                {
                    lblMsg.Text = "Email service is not configured. Contact admin.";
                    lblMsg.CssClass = "error";
                    return;
                }

                string otp = new Random().Next(100000, 999999).ToString();
                DateTime expiry = DateTime.Now.AddMinutes(10);

                using (SqlCommand updateCmd = new SqlCommand("sp_SetResetOtp", con))
                {
                    updateCmd.CommandType = CommandType.StoredProcedure;
                    updateCmd.Parameters.AddWithValue("@Otp", otp);
                    updateCmd.Parameters.AddWithValue("@Expiry", expiry);
                    updateCmd.Parameters.AddWithValue("@Email", email);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        lblMsg.Text = "Unable to process reset request. Try again.";
                        lblMsg.CssClass = "error";
                        return;
                    }
                }

                try
                {
                    SendEmail(email, otp);
                }
                catch
                {
                    lblMsg.Text = "Failed to send OTP. Please verify email settings and try again.";
                    lblMsg.CssClass = "error";
                    return;
                }
            }

            Session[cooldownKey] = DateTime.UtcNow;
            lblMsg.Text = "OTP sent to your registered email. It is valid for 10 minutes.";
            lblMsg.CssClass = "success";
            Response.Redirect("~/Auth/ResetPassword.aspx?email=" + Server.UrlEncode(email), false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        bool IsEmailConfigured()
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

        // 🔥 EMAIL FUNCTION
        void SendEmail(string toEmail, string otp)
        {
            string fromEmail = ConfigurationManager.AppSettings["Email"];
            string fromPassword = ConfigurationManager.AppSettings["Password"];

            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add(toEmail);
                mail.Subject = "Password Reset OTP";
                mail.Body = "Your OTP for password reset is: " + otp + "\n\nThis OTP is valid for 10 minutes.";
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