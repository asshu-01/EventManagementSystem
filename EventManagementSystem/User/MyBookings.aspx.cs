using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using EventManagementSystem.DAL;

namespace EventManagementSystem.User
{
    public partial class MyBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSummary();
                LoadBookings();

                if (string.Equals(Request.QueryString["booked"], "1", StringComparison.Ordinal))
                {
                    lblMsg.CssClass = "success";
                    lblMsg.Text = "Booking placed successfully. Waiting for admin approval.";
                }
            }
        }

        // 🔥 SUMMARY
        void LoadSummary()
        {
            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetUserBookingSummary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Session["UserEmail"].ToString());

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int totalBookings = reader["TotalBookings"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalBookings"]);
                            decimal totalAmount = reader["TotalAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalAmount"]);

                            lblTotalBookings.Text = totalBookings.ToString();
                            lblTotalAmount.Text = "₹" + totalAmount.ToString("0.00");
                        }
                        else
                        {
                            lblTotalBookings.Text = "0";
                            lblTotalAmount.Text = "₹0.00";
                        }
                    }
                }
            }
            catch
            {
                lblTotalBookings.Text = "0";
                lblTotalAmount.Text = "₹0.00";
            }
        }

        // 🔥 LOAD BOOKINGS
        void LoadBookings()
        {
            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetUserBookings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Session["UserEmail"]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    FormatDates(dt);

                    gvBookings.DataSource = dt;
                    gvBookings.DataBind();
                }
            }
            catch
            {
                gvBookings.DataSource = null;
                gvBookings.DataBind();
                lblMsg.CssClass = "error";
                lblMsg.Text = "Unable to load bookings.";
            }
        }

        // 📄 PAGINATION
        protected void gvBookings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBookings.PageIndex = e.NewPageIndex;
            LoadBookings();
        }

        // ❌ CANCEL BOOKING (WITH TRANSACTION)
        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                string userEmail = Session["UserEmail"].ToString();

                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cancelCmd = new SqlCommand("sp_UserCancelBooking", con))
                {
                    cancelCmd.CommandType = CommandType.StoredProcedure;
                    cancelCmd.Parameters.AddWithValue("@BookingID", bookingId);
                    cancelCmd.Parameters.AddWithValue("@Email", userEmail);

                    con.Open();
                    try
                    {
                        cancelCmd.ExecuteNonQuery();

                        lblMsg.Text = "Booking cancelled successfully!";
                        lblMsg.CssClass = "success";
                    }
                    catch
                    {
                        lblMsg.Text = "Error cancelling booking.";
                        lblMsg.CssClass = "error";
                    }
                }

                LoadBookings();
                LoadSummary();
            }
        }

        // 📅 DATE FORMAT
        void FormatDates(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                DateTime d;
                if (DateTime.TryParse(row["EventDate"].ToString(), out d))
                    row["EventDate"] = d.ToString("dd MMM yyyy");
            }
        }

        // 🔓 LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Auth/Login.aspx");
        }

    }
}