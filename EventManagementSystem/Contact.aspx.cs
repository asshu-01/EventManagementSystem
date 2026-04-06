using EventManagementSystem.DAL;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EventManagementSystem
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Convert.ToString(Session["Role"]);
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Admin/Dashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                if (Session["ContactSuccessMsg"] != null)
                {
                    lblMsg.Text = Session["ContactSuccessMsg"].ToString();
                    lblMsg.CssClass = "text-success"; // Fixed to use proper bootstrap class or inline styled
                    Session.Remove("ContactSuccessMsg");
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string name = txtName.Text.Trim();
            string email = Regex.Replace(txtEmail.Text ?? string.Empty, @"\s+", string.Empty).Trim().ToLowerInvariant();
            string message = txtMessage.Text.Trim();

            txtEmail.Text = email;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                lblMsg.Text = "Please fill all fields.";
                lblMsg.CssClass = "error";
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                lblMsg.Text = "Enter a valid email.";
                lblMsg.CssClass = "error";
                return;
            }

            if (!Regex.IsMatch(name, @"^[A-Za-z ]{3,100}$"))
            {
                lblMsg.Text = "Name must be 3-100 letters/spaces only.";
                lblMsg.CssClass = "error";
                return;
            }

            if (message.Length < 10 || message.Length > 1000)
            {
                lblMsg.Text = "Message must be between 10 and 1000 characters.";
                lblMsg.CssClass = "error";
                return;
            }

            bool savedToDb = false;
            bool mailSent = false;

            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_SaveContactMessage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Message", message);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    savedToDb = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Contact.btnSend_Click DB save failed: " + ex);
            }

            if (IsEmailConfigured())
            {
                try
                {
                    SendEmail(name, email, message);
                    mailSent = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Contact.btnSend_Click email send failed: " + ex);
                }
            }

            if (savedToDb && mailSent)
            {
                Session["ContactSuccessMsg"] = "Message sent successfully!";
            }
            else if (savedToDb)
            {
                Session["ContactSuccessMsg"] = "Message saved successfully. Email notification is not configured.";
            }
            else if (mailSent)
            {
                Session["ContactSuccessMsg"] = "Message sent successfully, but database save failed.";
            }
            else
            {
                lblMsg.Text = "Unable to send your message. Please try again.";
                lblMsg.CssClass = "text-danger";
                return;
            }

            Response.Redirect(Request.RawUrl, false);
            Context.ApplicationInstance.CompleteRequest();
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
        void SendEmail(string name, string email, string message)
        {
            string fromEmail = ConfigurationManager.AppSettings["Email"];
            string fromPassword = ConfigurationManager.AppSettings["Password"];
            string contactToEmail = ConfigurationManager.AppSettings["ContactToEmail"];
            if (string.IsNullOrWhiteSpace(contactToEmail))
                contactToEmail = fromEmail;

            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add(contactToEmail);
                mail.ReplyToList.Add(new MailAddress(email));
                mail.Subject = "New Contact Message";
                mail.Body = "Name: " + name +
                            "\nEmail: " + email +
                            "\n\nMessage:\n" + message;
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