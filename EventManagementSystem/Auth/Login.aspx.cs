using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using EventManagementSystem.BAL;

namespace EventManagementSystem.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["UserEmail"] != null)
            {
                string role = Convert.ToString(Session["Role"]);
                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    Response.Redirect("~/Admin/Dashboard.aspx");
                else
                    Response.Redirect("~/User/Home.aspx");

                return;
            }

            if (!IsPostBack && string.Equals(Request.QueryString["registered"], "1", StringComparison.Ordinal))
            {
                lblMsg.CssClass = "success";
                lblMsg.Text = "Registration successful. Please login.";
            }

            if (!IsPostBack && string.Equals(Request.QueryString["reset"], "1", StringComparison.Ordinal))
            {
                lblMsg.CssClass = "success";
                lblMsg.Text = "Password updated successfully. Please login.";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Text = "Login";
            btnLogin.Enabled = true;
            lblMsg.Text = string.Empty;

            if (!Page.IsValid)
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please enter valid login details.";
                return;
            }

            string email = Regex.Replace(txtEmail.Text ?? string.Empty, @"\s+", string.Empty).Trim();
            txtEmail.Text = email;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please enter valid email and password.";
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Enter a valid email address.";
                return;
            }

            UserBAL bal = new UserBAL();
            DataTable dt = bal.Login(email, txtPassword.Text);

            if (dt == null || dt.Rows.Count == 0)
            {
                if (!bal.UserExists(email))
                {
                    lblMsg.CssClass = "error";
                    lblMsg.Text = "This user does not exist. Please register first.";
                    return;
                }

                lblMsg.CssClass = "error";
                lblMsg.Text = "Invalid password.";
                return;
            }

            DataRow row = dt.Rows[0];
            string role = Convert.ToString(row["Role"]);
            if (string.IsNullOrWhiteSpace(role))
            {
                role = "User";
            }

            Session["UserID"] = Convert.ToInt32(row["UserID"]);
            Session["UserName"] = Convert.ToString(row["Name"]);
            Session["UserEmail"] = Convert.ToString(row["Email"]);
            Session["UserRole"] = role;
            Session["Role"] = role;

            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Admin/Dashboard.aspx");
                return;
            }

            Response.Redirect("~/User/Home.aspx");
        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            btnAdminLogin.Text = "Admin Login";
            btnAdminLogin.Enabled = true;
            lblMsg.Text = string.Empty;

            if (!Page.IsValid)
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please enter valid admin login details.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAdminId.Text) || string.IsNullOrWhiteSpace(txtSpecialKey.Text))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please enter Admin ID and Special Key.";
                return;
            }

            string enteredAdminId = txtAdminId.Text.Trim();
            string enteredSpecialKey = txtSpecialKey.Text;

            string adminId = ConfigurationManager.AppSettings["AdminId"];
            string specialKey = ConfigurationManager.AppSettings["AdminSpecialKey"];

            if (string.IsNullOrWhiteSpace(adminId) || string.IsNullOrWhiteSpace(specialKey))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Admin login is not configured.";
                return;
            }

            if (!string.Equals(enteredAdminId, adminId, StringComparison.Ordinal) ||
                !string.Equals(enteredSpecialKey, specialKey, StringComparison.Ordinal))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Invalid Admin ID or Special Key.";
                return;
            }

            Session["UserID"] = -1;
            Session["UserName"] = "Admin";
            Session["UserEmail"] = "Admin";
            Session["UserRole"] = "Admin";
            Session["Role"] = "Admin";

            Response.Redirect("~/Admin/Dashboard.aspx");
        }
    }
}