using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using EventManagementSystem.DAL;
using EventManagementSystem.Security;
using System.Web.UI.WebControls;

namespace EventManagementSystem.Auth
{
    public partial class Register : System.Web.UI.Page
    {
        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value ?? string.Empty, @"^[A-Za-z ]{3,100}$");
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value ?? string.Empty, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        }

        protected void cvPhone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value ?? string.Empty, @"^\d{10}$");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && string.Equals(Request.QueryString["need"], "1", StringComparison.Ordinal))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "This user does not exist. Please register first.";

                string email = Request.QueryString["email"];
                if (!string.IsNullOrWhiteSpace(email))
                    txtEmail.Text = email;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            btnRegister.Text = "Register";
            btnRegister.Enabled = true;
            lblMsg.Text = string.Empty;

            if (!Page.IsValid)
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please correct the highlighted errors.";
                return;
            }

            string name = txtName.Text.Trim();
            string email = Regex.Replace(txtEmail.Text ?? string.Empty, @"\s+", string.Empty).Trim().ToLowerInvariant();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            txtEmail.Text = email;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone)
                || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Please fill in all required fields.";
                return;
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Enter a valid email address.";
                return;
            }

            if (!Regex.IsMatch(name, @"^[A-Za-z ]{3,100}$"))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Name must be at least 3 letters and can contain only letters and spaces.";
                return;
            }

            if (!password.Equals(confirmPassword, StringComparison.Ordinal))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Password and Confirm Password do not match.";
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Phone must be exactly 10 digits (no less, no more) and only numbers are allowed.";
                return;
            }

            if (!Regex.IsMatch(password, @"^(?=.*\d)(?=.*[@#]).{6,50}$"))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Password must contain at least one digit and one symbol (@ or #).";
                return;
            }

            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                {
                    con.Open();

                    using (SqlCommand checkCmd = new SqlCommand("sp_UserExists", con))
                    {
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = email;
                        int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (existingCount > 0)
                        {
                            lblMsg.CssClass = "error";
                            lblMsg.Text = "This email is already registered.";
                            return;
                        }
                    }

                    string hashed = PasswordHasher.Hash(txtPassword.Text);

                    using (SqlCommand insertCmd = new SqlCommand("sp_RegisterUser", con))
                    {
                        insertCmd.CommandType = CommandType.StoredProcedure;
                        insertCmd.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = name;
                        insertCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = email;
                        insertCmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 20).Value = phone;
                        insertCmd.Parameters.Add("@Password", SqlDbType.NVarChar, -1).Value = hashed;
                        insertCmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Auth/Login.aspx?registered=1", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            catch (SqlException ex)
            {
                lblMsg.CssClass = "error";
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    string sqlMessage = ex.Message ?? string.Empty;
                    if (sqlMessage.IndexOf("phone", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        lblMsg.Text = "This phone number is already registered.";
                        return;
                    }

                    if (sqlMessage.IndexOf("email", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        lblMsg.Text = "This email is already registered.";
                        return;
                    }

                    lblMsg.Text = "An account with the same email or phone already exists.";
                    return;
                }

                lblMsg.Text = "Registration failed. Please check your details and try again.";
            }
        }

    }
}