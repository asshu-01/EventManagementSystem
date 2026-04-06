using System;

namespace EventManagementSystem.Entity
{
    public class EventEntity
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }   // ✅ FIXED
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public int MaxSeats { get; set; }
        public int AvailableSeats { get; set; }

        public string EventMode { get; set; }     // Online / Offline
        public string MeetingLink { get; set; }   // For online
        public string EventType { get; set; }     // Workshop, Hackathon, etc.
        public decimal? PrizePool { get; set; }   // Nullable

        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}