using System;
using EventManagementSystem.BAL;
using EventManagementSystem.DAL;
using EventManagementSystem.Entity;

namespace EventManagementSystem.User
{
    public partial class Booking : System.Web.UI.Page
    {
        private readonly BookingBAL bookingBal = new BookingBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "error";

            int eventId;
            if (!int.TryParse(Request.QueryString["EventID"], out eventId))
            {
                lblMsg.Text = "Invalid event.";
                return;
            }

            int seats;
            if (!int.TryParse(txtSeats.Text, out seats) || seats <= 0)
            {
                lblMsg.Text = "Enter valid seats.";
                return;
            }

            try
            {
                bookingBal.BookEvent(new BookingEntity
                {
                    EventID = eventId,
                    SeatsBooked = seats,
                    UserEmail = Convert.ToString(Session["UserEmail"])
                });

                Response.Redirect("~/User/MyBookings.aspx?booked=1&status=pending", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            catch
            {
                lblMsg.Text = "Error while booking.";
            }
        }

    }
}