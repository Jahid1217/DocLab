using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Medicine
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Medicine Name cannot be longer than 100 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Medicine Name")]
        public string MedicineName { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Generic Name is required.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Generic Name")]
        public string GenericName { get; set; }

        
        [Required(ErrorMessage = "Medicine Category is required.")]
        [StringLength(50, ErrorMessage = "Medicine Category cannot be longer than 50 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Medicine Category")]
        public string MedicineCategory { get; set; }
        
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
        public DateTime Created_at { get; set; }
        public Medicine()
        {
            Created_at = DateTime.Now;
        }
    }
}