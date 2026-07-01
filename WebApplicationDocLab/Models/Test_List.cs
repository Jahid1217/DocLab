using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Test_List
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        public int TestId { get; set; }
        public int PrescriptionId { get; set; }
        public DateTime Created_at { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual Test Test { get; set; }
        public Test_List()
        {
            Created_at = DateTime.Now;
        }
    }
}