using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreUI.Models
{
    public class RegistrationModel
    {
        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = "Password limited to {2} to {1}", MinimumLength = 5)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage ="Passwords dont match")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
