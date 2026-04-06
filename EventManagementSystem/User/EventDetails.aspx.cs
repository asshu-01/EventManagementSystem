using EventManagementSystem.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace EventManagementSystem.User
{
    public partial class EventDetails : System.Web.UI.Page
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
                LoadEventDetails();

                if (string.Equals(Request.QueryString["booked"], "1", StringComparison.Ordinal))
                {
                    lblMsg.CssClass = "success";
                    lblMsg.Text = "Booking confirmed.";
                }
            }
        }

        private void LoadEventDetails()
        {
            int eventId;
            if (!int.TryParse(Request.QueryString["EventID"], out eventId))
            {
                Response.Redirect("~/User/Home.aspx");
                return;
            }

            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetEventById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EventID", eventId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    lblMsg.Text = "Event not found.";
                    btnBookNow.Enabled = false;
                    return;
                }

                int maxSeats = Convert.ToInt32(reader["MaxSeats"]);
                int available = Convert.ToInt32(reader["AvailableSeats"]);
                DateTime eventDateValue = Convert.ToDateTime(reader["EventDate"]);
                string status = available == 0 ? "FULL" : (eventDateValue.Date < DateTime.Today ? "CLOSED" : "OPEN");
                string mode = reader["EventMode"].ToString();
                string meetingLink = reader["MeetingLink"] == DBNull.Value ? "" : reader["MeetingLink"].ToString();
                string eventType = reader["EventType"].ToString();
                string prizePool = reader["PrizePool"] == DBNull.Value ? "" : reader["PrizePool"].ToString();

                // BASIC INFO
                lblEventName.Text = reader["EventName"].ToString();
                lblEventDate.Text = eventDateValue.ToString("dd MMM yyyy");
                lblVenue.Text = reader["Venue"].ToString();
                lblPrice.Text = Convert.ToDecimal(reader["Price"]).ToString("0.00");
                lblDescription.Text = reader["Description"].ToString();

                lblAvailable.Text = available + " / " + maxSeats;
                lblStatus.Text = status;
                lblStatus.ForeColor = Color.Green;
                lblWarning.Text = string.Empty;
                if (!string.Equals(Request.QueryString["booked"], "1", StringComparison.Ordinal))
                    lblMsg.Text = string.Empty;

                // MODE
                lblMode.Text = mode;

                // MEETING LINK
                if (mode == "Online" && !string.IsNullOrEmpty(meetingLink))
                {
                    lnkMeeting.NavigateUrl = meetingLink;
                    meetingRow.Visible = true;
                }
                else
                {
                    meetingRow.Visible = false;
                }

                // TYPE + PRIZE
                lblType.Text = eventType;
                lblPrize.Text = string.IsNullOrEmpty(prizePool) ? "N/A" : "₹ " + prizePool;

                // WARNING
                if (available <= 5 && available > 0)
                {
                    lblWarning.Text = "⚠ Only few seats left!";
                    lblWarning.ForeColor = Color.Red;
                }

                // DISABLE BOOKING
                if (status == "FULL" || status == "CLOSED")
                {
                    lblStatus.ForeColor = status == "FULL" ? Color.Red : Color.DarkOrange;
                    btnBookNow.Enabled = false;
                    btnBookNow.Text = "Not Available";
                }
            }
        }

        protected void btnBookNow_Click(object sender, EventArgs e)
        {
            int eventId;
            if (!int.TryParse(Request.QueryString["EventID"], out eventId))
            {
                Response.Redirect("~/User/Home.aspx");
                return;
            }

            Response.Redirect("~/User/Booking.aspx?EventID=" + eventId);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Auth/Login.aspx");
        }
    }
}