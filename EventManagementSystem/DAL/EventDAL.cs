using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EventManagementSystem.Entity;

namespace EventManagementSystem.DAL
{
    public class EventDAL
    {
        public List<EventEntity> GetEvents()
        {
            List<EventEntity> list = new List<EventEntity>();

            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetEvents", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EventEntity ev = new EventEntity
                    {
                        EventID = Convert.ToInt32(reader["EventID"]),
                        EventName = reader["EventName"].ToString(),
                        EventDate = Convert.ToDateTime(reader["EventDate"]),
                        Venue = reader["Venue"].ToString(),
                        Price = reader["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Price"]),
                        MaxSeats = reader["MaxSeats"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxSeats"]),
                        AvailableSeats = reader["AvailableSeats"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AvailableSeats"]),
                        Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString(),
                        EventMode = reader["EventMode"] == DBNull.Value ? string.Empty : reader["EventMode"].ToString(),
                        MeetingLink = reader["MeetingLink"] == DBNull.Value ? string.Empty : reader["MeetingLink"].ToString(),
                        EventType = reader["EventType"] == DBNull.Value ? string.Empty : reader["EventType"].ToString(),
                        PrizePool = reader["PrizePool"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["PrizePool"]),
                        ImagePath = reader["ImagePath"] == DBNull.Value ? string.Empty : reader["ImagePath"].ToString()
                    };

                    list.Add(ev);
                }
            }

            return list;
        }
    }
}