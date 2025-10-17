using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Social_Platform.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter a valid email address")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}