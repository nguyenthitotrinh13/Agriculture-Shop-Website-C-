using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CuaHangNongSan.Models
{
    public class User
    {
        public string id { get; set; }

        [Required(ErrorMessage = "Username không được để trống.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username không được chứa khoảng trắng.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Password không được để trống.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password phải có ít nhất 8 ký tự, bao gồm chữ cái, số và ký tự đặc biệt.")]
        public string pass { get; set; }    

        [Required(ErrorMessage = "Tên không được để trống.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string address { get; set; }

        [Required(ErrorMessage = "Địa chỉ thành phố không được để trống.")]
        public string addressdetail { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^(0|\+84)(\d{9,10})$", ErrorMessage = "Số điện thoại phải là số điện thoại của Việt Nam.")]
        public string phoneNumber { get; set; }

        public string role { get; set; }
    }
}
