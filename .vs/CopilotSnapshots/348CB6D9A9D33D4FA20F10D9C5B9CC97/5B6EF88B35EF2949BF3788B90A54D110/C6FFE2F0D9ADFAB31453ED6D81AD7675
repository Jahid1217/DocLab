using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Doctor_Details
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Day is required.")]
        public string Day { get; set; }
        [Required(ErrorMessage = "Time is required.")]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Time must be in HH:mm format.")]
        public string TimeStart { get; set; }
        [Required(ErrorMessage = "End time is required.")]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "End time must be in HH:mm format.")]
        public string TimeEnd { get; set; }
        [Required(ErrorMessage = "Consulting fees are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Consulting fees must be a positive number.")]
        public double ConsultingFees { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public virtual User_Info User_Info { get; set; }
    }
    public enum DayType
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
}