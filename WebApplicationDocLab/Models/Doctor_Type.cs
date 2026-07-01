using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Doctor_Type
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(50, ErrorMessage = "Type Name cannot be longer than 50 characters.")]
        [DataType(DataType.Text)]
        public string TypeName { get; set; }
    }
}