using System.Collections.Generic;
using EventManagementSystem.DAL;
using EventManagementSystem.Entity;

namespace EventManagementSystem.BAL
{
    public class EventBAL
    {
        private EventDAL dal = new EventDAL();

        public List<EventEntity> GetEvents()
        {
            return dal.GetEvents();
        }
    }
}