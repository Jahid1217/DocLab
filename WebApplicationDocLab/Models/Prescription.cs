using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Prescription
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Patient ID is required.")]

        public int PatientId { get; set; }
        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Prescription date is required.")]
        [DataType(DataType.Date)]
        public DateTime PrescriptionDate { get; set; }
        public string Notes { get; set; }
        public virtual User_Info User_Info { get; set; }
        public virtual Doctor_Details Doctor_Details { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}