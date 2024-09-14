using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CuaHangNongSan.Models
{
    public class AccountController : Controller
    {
        // GET: Account
        string conStr = "Data Source=LAPTOP-FD3P69GF;Initial Catalog=MuaBanNongSan;Integrated Security=True";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM user)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT role FROM tbl_User WHERE username=@Username AND password=@Password";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", user.userName);
                    cmd.Parameters.AddWithValue("@Password", user.pass);
                    con.Open();
                    var role = cmd.ExecuteScalar()?.ToString();
                    if (role != null)
                    {
                        Session["Username"] = user.userName;
                        Session["Role"] = role;
                        if (role == "admin")
                        {
                            //Session["Username"] = username;
                            //Session["Role"] = role;
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

 
        [HttpPost]
        public ActionResult Register(User user)
        {
            

            if (!ModelState.IsValid)
            {
                // If ModelState is not valid, return the view with validation errors
                return View(user);
            }

           
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Kiểm tra xem username đã tồn tại trong cơ sở dữ liệu hay chưa
                string checkUserQuery = "SELECT COUNT(*) FROM tbl_User WHERE username = @Username";
                SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, con);
                checkUserCmd.Parameters.AddWithValue("@Username", user.userName);

                int userCount = (int)checkUserCmd.ExecuteScalar();
                if (userCount > 0)
                {
                    // Nếu username đã tồn tại, thêm lỗi vào ModelState và trả về view
                    ModelState.AddModelError("username", "Username đã tồn tại.");
                    return View();
                }

                // Thêm user mới vào cơ sở dữ liệu
                string query = "INSERT INTO tbl_User (user_id, username, password, fullname, email,address_detail, address, phone_number, created_at, updated_at) VALUES (@UserId, @Username, @Password, @Fullname, @Email,@AddressDetail, @Address, @PhoneNumber, @CreatedAt, @UpdatedAt)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", Guid.NewGuid().ToString().Substring(0, 20));
                cmd.Parameters.AddWithValue("@Username", user.userName);
                cmd.Parameters.AddWithValue("@Password", user.pass);
                cmd.Parameters.AddWithValue("@Fullname", user.name);
                cmd.Parameters.AddWithValue("@Email", user.email);
                cmd.Parameters.AddWithValue("@AddressDetail", user.addressdetail);
                cmd.Parameters.AddWithValue("@Address", user.address);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                cmd.ExecuteNonQuery();
                con.Close();

                return RedirectToAction("Login", "Account");
            }
        
    }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Hàm kiểm tra định dạng số điện thoại (cơ bản cho số điện thoại Việt Nam)
        private bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^(0\d{9,10})$");
        }

        // Hàm kiểm tra mật khẩu (yêu cầu ít nhất một chữ cái in hoa, một chữ cái thường, một số và một ký tự đặc biệt, có độ dài từ 8 đến 20 ký tự)
        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$");
        }

        // Hàm kiểm tra username (ví dụ đơn giản là không để trống)
        private bool IsValidUsername(string username)
        {
            return !string.IsNullOrEmpty(username);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            // Xóa session
            Session.Clear();
            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login", "Account");
        }
    }
}