using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class BookingAppointment
    {
        public BookingAppointment()
        {
            Created_at = DateTime.Now;
        }
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Booking_Date { get; set; }  // Only date part

        // Add these new properties
        public TimeSpan ActualTime { get; set; }  // Will store the calculated time
        public int SerialNumber { get; set; }     // Will store the position in queue

        public string BookType { get; set; }
        public string Status { get; set; }
        public DateTime Created_at { get; set; }

        public virtual User_Info User_Info { get; set; }
     
        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed,
            NoShow
        }

    }

    public enum BookType
    {
        Online,
        Offline
    }
    
}