using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DataModel.Domain.Models
{
    public class UserSignin
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public Nullable<System.DateTime> LastLoginDate { get; set; }

        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "please enter {0}")]
        [Display(Name = "sum of ")]
        public string Captcha { get; set; }
    }
}