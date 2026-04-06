using EventManagementSystem.DAL;
using EventManagementSystem.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace EventManagementSystem.Auth
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private string email = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            email = Regex.Replace(Request.QueryString["email"] ?? string.Empty, @"\s+", string.Empty).Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Invalid reset request.");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                ShowError("Invalid reset request.");
                return;
            }
        }

        // 🔐 RESET PASSWORD
        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string otp = txtOtp.Text.Trim();
            string password = txtPassword.Text;

            // 🔥 EXTRA SECURITY CHECK
            if (!IsStrongPassword(password))
            {
                lblMsg.Text = "Weak password. Use uppercase, lowercase, number, and symbol.";
                lblMsg.CssClass = "error";
                return;
            }

            string hashedPassword = PasswordHasher.Hash(password);

            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ResetPasswordWithOtp", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Otp", otp);

                con.Open();
                int rows = 0;
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    rows = Convert.ToInt32(result);
                }

                if (rows > 0)
                {
                    Response.Redirect("~/Auth/Login.aspx?reset=1", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    ShowError("Invalid or expired OTP.");
                }
            }
        }

        // 🔒 PASSWORD STRENGTH CHECK
        bool IsStrongPassword(string password)
        {
            if (password.Length < 8) return false;

            bool hasUpper = false, hasLower = false, hasDigit = false, hasSymbol = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if ("@#$%^&+=".IndexOf(c) >= 0) hasSymbol = true;
            }

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }

        // ❌ ERROR HANDLER
        void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.CssClass = "error";
            pnlReset.Visible = false;
        }
    }
}