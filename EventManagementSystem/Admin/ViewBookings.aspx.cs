using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using EventManagementSystem.DAL;
using EventManagementSystem.Utilities;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EventManagementSystem.Admin
{
    public partial class ViewBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || !Session["Role"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadBookings();
        }

        private void LoadBookings()
        {
            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetAdminBookings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());
                    cmd.Parameters.AddWithValue("@Filter", ddlFilter.SelectedValue);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvBookings.DataSource = dt;
                        gvBookings.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            lblMsg.CssClass = "error";
                            lblMsg.Text = "No bookings found.";
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("ViewBookings.LoadBookings failed: " + ex);
                gvBookings.DataSource = null;
                gvBookings.DataBind();
                lblMsg.CssClass = "error";
                lblMsg.Text = "Unable to load bookings.";
            }
        }

        protected void gvBookings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBookings.PageIndex = e.NewPageIndex;
            LoadBookings();
        }

        protected void gvBookings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            string status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "BookingStatus"));
            bool isPending = string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase);
            bool isCancelled = string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase);

            LinkButton btnAccept = e.Row.FindControl("btnAccept") as LinkButton;
            LinkButton btnCancel = e.Row.FindControl("btnCancel") as LinkButton;

            if (btnAccept != null)
            {
                btnAccept.Visible = isPending;
            }

            if (btnCancel != null)
            {
                btnCancel.Visible = !isCancelled;
            }
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "AcceptBooking", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(e.CommandName, "CancelBooking", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            int bookingId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out bookingId))
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Invalid booking.";
                return;
            }

            bool isAccept = string.Equals(e.CommandName, "AcceptBooking", StringComparison.OrdinalIgnoreCase);

            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                {
                    con.Open();
                    string currentStatus = "Pending";
                    string userEmail = string.Empty;
                    string userName = "User";
                    string eventName = string.Empty;
                    DateTime eventDate = DateTime.Now;
                    string venue = string.Empty;
                    string eventMode = string.Empty;
                    string meetingLink = string.Empty;
                    int seatsBooked = 0;

                    using (SqlCommand getBooking = new SqlCommand("sp_GetBookingNotificationDetails", con))
                    {
                        getBooking.CommandType = CommandType.StoredProcedure;
                        getBooking.Parameters.AddWithValue("@BookingID", bookingId);

                        using (SqlDataReader reader = getBooking.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblMsg.CssClass = "error";
                                lblMsg.Text = "Booking not found.";
                                return;
                            }

                            seatsBooked = Convert.ToInt32(reader["SeatsBooked"]);
                            currentStatus = Convert.ToString(reader["BookingStatus"]);
                            userEmail = Convert.ToString(reader["Email"]);
                            userName = Convert.ToString(reader["UserName"]);
                            eventName = Convert.ToString(reader["EventName"]);
                            eventDate = Convert.ToDateTime(reader["EventDate"]);
                            venue = Convert.ToString(reader["Venue"]);
                            eventMode = Convert.ToString(reader["EventMode"]);
                            meetingLink = Convert.ToString(reader["MeetingLink"]);
                        }
                    }

                    if (isAccept)
                    {
                        if (string.Equals(currentStatus, "Cancelled", StringComparison.OrdinalIgnoreCase))
                        {
                            lblMsg.CssClass = "error";
                            lblMsg.Text = "Cancelled booking cannot be accepted.";
                            return;
                        }

                        using (SqlCommand acceptCmd = new SqlCommand("sp_AdminAcceptBooking", con))
                        {
                            acceptCmd.CommandType = CommandType.StoredProcedure;
                            acceptCmd.Parameters.AddWithValue("@BookingID", bookingId);
                            acceptCmd.ExecuteNonQuery();
                        }

                        try
                        {
                            NotificationHelper.SendBookingStatusEmail(userEmail, userName, eventName, eventDate,
                                venue, eventMode, meetingLink, seatsBooked, "Confirmed");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError("ViewBookings.SendBookingStatusEmail(Confirmed) failed: " + ex);
                        }

                        lblMsg.CssClass = "success";
                        lblMsg.Text = "Booking accepted successfully.";
                        LoadBookings();
                        return;
                    }

                    if (string.Equals(currentStatus, "Cancelled", StringComparison.OrdinalIgnoreCase))
                    {
                        lblMsg.CssClass = "error";
                        lblMsg.Text = "Booking already cancelled.";
                        return;
                    }

                    using (SqlCommand cancelCmd = new SqlCommand("sp_AdminCancelBooking", con))
                    {
                        cancelCmd.CommandType = CommandType.StoredProcedure;
                        cancelCmd.Parameters.AddWithValue("@BookingID", bookingId);
                        cancelCmd.ExecuteNonQuery();
                    }

                    try
                    {
                        NotificationHelper.SendBookingStatusEmail(userEmail, userName, eventName, eventDate,
                            venue, eventMode, meetingLink, seatsBooked, "Cancelled");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("ViewBookings.SendBookingStatusEmail(Cancelled) failed: " + ex);
                    }

                    lblMsg.CssClass = "success";
                    lblMsg.Text = "Booking cancelled successfully.";
                    LoadBookings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("ViewBookings.gvBookings_RowCommand outer failure: " + ex);
                lblMsg.CssClass = "error";
                lblMsg.Text = "Unable to cancel booking.";
            }
        }
    }
}