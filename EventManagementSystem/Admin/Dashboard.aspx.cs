using System;
using System.Data;
using System.Data.SqlClient;
using EventManagementSystem;
using EventManagementSystem.DAL;

namespace EventManagementSystem.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 Not logged in
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            // 🔒 Not admin
            if (Session["UserRole"] == null ||
                !Session["UserRole"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/User/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // ✅ Show user info
                lblUserName.Text = Session["UserName"].ToString();
                lblRole.Text = "Role: " + Session["UserRole"].ToString();

                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdminStats", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblUsers.Text = reader["TotalUsers"].ToString();
                        lblEvents.Text = reader["TotalEvents"].ToString();
                        lblBookings.Text = reader["TotalBookings"].ToString();
                        lblTickets.Text = reader["TotalTickets"].ToString();
                    }
                }
            }
        }
    }
}