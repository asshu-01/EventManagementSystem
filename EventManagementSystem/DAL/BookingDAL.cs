using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EventManagementSystem.Entity;

namespace EventManagementSystem.DAL
{
    public class BookingDAL
    {
        // 🔥 GET BOOKINGS BY USER
        public List<BookingEntity> GetBookings(string email)
        {
            List<BookingEntity> list = new List<BookingEntity>();

            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetUserBookings", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new BookingEntity
                    {
                        BookingID = Convert.ToInt32(reader["BookingID"]),
                        UserID = 0,
                        UserEmail = email,
                        EventID = Convert.ToInt32(reader["EventID"]),
                        SeatsBooked = Convert.ToInt32(reader["SeatsBooked"]),
                        BookingDate = Convert.ToDateTime(reader["BookingDate"])
                    });
                }
            }

            return list;
        }

        // 🔥 BOOK EVENT
        public void BookEvent(BookingEntity booking)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_BookEvent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserEmail", booking.UserEmail ?? string.Empty);
                cmd.Parameters.AddWithValue("@EventID", booking.EventID);
                cmd.Parameters.AddWithValue("@Seats", booking.SeatsBooked);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // 🔥 CANCEL BOOKING
        public void CancelBooking(int bookingId)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdminCancelBooking", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}