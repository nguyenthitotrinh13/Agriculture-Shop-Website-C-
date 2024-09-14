using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangNongSan.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username không được để trống.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username không được chứa khoảng trắng.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Password không được để trống.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password phải có ít nhất 8 ký tự, bao gồm chữ cái, số và ký tự đặc biệt.")]
        public string pass { get; set; }


    }
}