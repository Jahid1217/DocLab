using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class User_Info
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters.")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(3, ErrorMessage = "First Name must be at least 3 characters long.")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters.")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Last Name is required.")]
        [MinLength(3, ErrorMessage = "Last Name must be at least 3 characters long.")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1900", "12/31/2100", ErrorMessage = "Date of Birth must be between 1/1/1900 and 12/31/2100.")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Address")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Address is required.")]
        [MinLength(4, ErrorMessage = "Address must be at least 4 characters long.")]
        public string Address { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public GenderType Gender { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Blood Group")]
        [Required(ErrorMessage = "Blood Group is required.")]
        public BloodGroupType BloodGroup { get; set; }

        [Display(Name = "NID No/ Passport No")]
        [StringLength(20, ErrorMessage = "NID/Passport number cannot be longer than 20 characters.")]
        [DataType(DataType.Text)]
        public string NID { get; set; }

        [Display(Name = "Registration No")]
        [StringLength(20, ErrorMessage = "Registration number cannot be longer than 20 characters.")]
        [DataType(DataType.Text)]
        public string RegistrationNo { get; set; }

        [Display(Name = "Degree")]
        [StringLength(100, ErrorMessage = "Degree cannot be longer than 50 characters.")]
        [DataType(DataType.Text)]
        public string Department { get; set; }

        [Display(Name = "Doctor Type")]
        public string DocType { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Confirm Password must be at least 6 characters long.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Image")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Image URL must be between 5 and 200 characters.")]
        public string Image { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "User Type")]
        public string UserType { get; set; }
        public string createdBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Doctor_Type DoctorType { get; set; }
    }

    public enum AppUserType
    {
        Doctor,
        Patient,
        Admin
    }

    public enum UserStatus
    {
        Inactive,
        Active
    }

    public enum GenderType
    {
        Male,
        Female,
        Other
    }

    public enum BloodGroupType
    {
        A_Positive,
        A_Negative,
        B_Positive,
        B_Negative,
        AB_Positive,
        AB_Negative,
        O_Positive,
        O_Negative
    }

}