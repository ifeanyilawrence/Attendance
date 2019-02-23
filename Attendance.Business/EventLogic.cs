using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class EventLogic : BusinessBaseLogic<EVENT>
    {
        public List<EVENT> GetTodaysEvent()
        {
            List<EVENT> events = null;
            try
            {
                DateTime todayStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime todayEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                events = base.GetEntitiesBy(e => e.Date >= todayStartTime && e.Date <= todayEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return events;
        }
        public List<EVENT> GetEventsForStudent(STUDENT student)
        {
            List<EVENT> events = null;
            try
            {
                DateTime todayStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime todayEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                events = base.GetEntitiesBy(e => e.Date >= todayStartTime && e.Date <= todayEndTime);

                StudentLevelLogic studentLevelLogic = new StudentLevelLogic();

                STUDENT_LEVEL studentLevel = studentLevelLogic.GetEntitiesBy(s => s.Student_Id == student.Person_Id).LastOrDefault();

                if (studentLevel != null)
                {
                    events = events.Where(e => e.Programme_Id == studentLevel.Programme_Id && e.Department_Id == studentLevel.Department_Id && e.Level_Id == studentLevel.Level_Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return events;
        }

        public bool GetEventStatus(EVENT e)
        {
            try
            {
                return DateTime.Now >= e.Event_Start && DateTime.Now <= e.Event_End;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InLocation(EVENT currentEvent, decimal latitude, decimal longitude)
        {
            try
            {
                decimal distance = GetDistance(Convert.ToDouble(currentEvent.LOCATION.Latitude), Convert.ToDouble(currentEvent.LOCATION.Longitude), Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                distance = distance < 0 ? distance * -1 : distance;

                return distance <= 25.0M;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            try
            {
                // generally used geo measurement function
                var R = 6378.137; // Radius of earth in KM
                var dLat = (lat2 * Math.PI / 180) - (lat1 * Math.PI / 180);
                var dLon = (lon2 * Math.PI / 180) - (lon1 * Math.PI / 180);
                var a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2)) + (Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2));
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var d = R * c;
                return Convert.ToDecimal(d * 1000); // meters
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
