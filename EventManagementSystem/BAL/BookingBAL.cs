using System;
using EventManagementSystem.DAL;
using EventManagementSystem.Entity;

namespace EventManagementSystem.BAL
{
    public class BookingBAL
    {
        private BookingDAL dal = new BookingDAL();

        public void BookEvent(BookingEntity booking)
        {
            try
            {
                dal.BookEvent(booking);
            }
            catch (Exception ex)
            {
                throw new Exception("Booking failed.", ex);
            }
        }
    }
}