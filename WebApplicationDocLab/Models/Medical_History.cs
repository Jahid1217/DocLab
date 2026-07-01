using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Medical_History
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Record_Type { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
        public Medical_History()
        {
            Created_at = DateTime.Now;
        }
    }
    public enum RecordType
    {
        Allergy,
        Medication,
        Surgery,
        FamilyHistory,
        SocialHistory,
        Immunization,
        Diabetes,
        Other
    }
}