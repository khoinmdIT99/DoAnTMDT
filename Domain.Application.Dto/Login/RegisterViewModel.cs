using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Login
{
    public class RegisterViewModel
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string FullName { get; set; }
        public string UserName { get; set; }
        [MaxLength(255)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword must match")]
        public string RetypePassword { get; set; }

    }

}
