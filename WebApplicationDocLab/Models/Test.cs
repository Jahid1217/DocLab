using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Test
    {
        public Test()
        {
            Created_at = DateTime.Now;
        }
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Test Name is required.")]
        [StringLength(100, ErrorMessage = "Test Name cannot be longer than 100 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Test Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Test Name can only contain letters and spaces.")]
        public string TestName { get; set; }
        [Required(ErrorMessage = "Test Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Category")]
        public string Category { get; set; }
        public DateTime Created_at { get; set; }
    }
}