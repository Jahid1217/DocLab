// New view model to hold appointment info with patient and doctor names
using System;

namespace WebApplicationDocLab.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string BookType { get; set; }
        public DateTime Booking_Date { get; set; }
        public TimeSpan ActualTime { get; set; }
        // Add other properties as needed
    }
}
