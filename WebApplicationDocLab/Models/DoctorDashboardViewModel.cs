using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class DoctorDashboardViewModel
    {
        public List<BookingAppointment> TodayAppointments { get; set; }
        public List<BookingAppointment> UpcomingAppointments { get; set; }
        public List<Prescription> RecentPrescriptions { get; set; }
    }
}