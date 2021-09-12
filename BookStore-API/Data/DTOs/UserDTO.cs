using System.ComponentModel.DataAnnotations;

namespace BookStore_API.Data.DTOs
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = "Password limited to {2} to {1}" ,MinimumLength = 5)]
        public string Password { get; set; }
    }
}
