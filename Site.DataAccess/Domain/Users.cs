using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Site.DataAccess.Domain
{
	public class Users
	{
	}
    public class Login_VM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class Signup_VM
    {
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$",
        ErrorMessage = "Full Name should contain only letters.")]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format. Use 10 digits.")]
        public string PhoneNo { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must have at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class AddStaff_VM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email address format.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format. Use 10 digits.")]
        public string PhoneNo { get; set; }
        public IFormFile UploadImage { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must have at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class AddStaff
    {
        
        public string FullName { get; set; }
    
        public string Email { get; set; }
   
        public string PhoneNo { get; set; }
        public string UploadImage { get; set; }
      
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
    }
    public class AddMember_VM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }

    }



    public class Save_PortalUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }

        public int Role { get; set; }
    }

    public class Portal_User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }

        public string UploadImage { get; set; }

    }
}
