using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Common.DataModel.DTO.Dashboard.UserDTO
{
    public class RegisteredUserDto
    {
        [HiddenInput(DisplayValue = false)]
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DisplayName("نام کاربری ")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DisplayName("ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("زمان ایجاد")]
        [DataType(DataType.DateTime)]
        public System.DateTime CreatedDate { get; set; }

        [DisplayName("آخرین ورود")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> LastLoginDate { get; set; }
    }
}