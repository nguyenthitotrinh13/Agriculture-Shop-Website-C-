using CuaHangNongSan.Connection;
using CuaHangNongSan.Models;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CuaHangNongSan.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private DataConnection db = new DataConnection();

        public ActionResult DeleteUser(string id)
        {
            User delUser = null;

            using (SqlConnection con = db.sqlstring())
            {
                con.Open();
                string sql = "SELECT user_id, username, password, fullname, address_detail, email, phone_number, role FROM tbl_User WHERE user_id = @UserId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    delUser = new User
                    {
                        id = reader["user_id"].ToString(),
                        userName = reader["username"].ToString(),
                        pass = reader["password"].ToString(),
                        name = reader["fullname"].ToString(),
                        address = reader["address_detail"].ToString(),
                        email = reader["email"].ToString(),
                        phoneNumber = reader["phone_number"].ToString(),
                        role = reader["role"].ToString(),
                    };
                }
            }

            if (delUser == null)
            {
                return HttpNotFound();
            }

            return View(delUser);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(string id)
        {
            try
            {
                using (SqlConnection con = db.sqlstring())
                {
                    con.Open();
                    string sql = "DELETE FROM tbl_User WHERE user_id = @UserId";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserId", id);
                    cmd.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
                return RedirectToAction("user", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                return RedirectToAction("user", "Home");
            }
        }

        public ActionResult EditUser(string id)
        {
            User user = null;

            try
            {
                using (SqlConnection con = db.sqlstring())
                {
                    con.Open();
                    string sql = "SELECT user_id, username, password, fullname, address_detail, email, phone_number, role FROM tbl_User WHERE user_id = @UserId";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserId", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            id = reader["user_id"].ToString(),
                            userName = reader["username"].ToString(),
                            pass = reader["password"].ToString(),
                            name = reader["fullname"].ToString(),
                            address = reader["address_detail"].ToString(),
                            email = reader["email"].ToString(),
                            phoneNumber = reader["phone_number"].ToString(),
                            role = reader["role"].ToString(),
                        };
                    }
                }

                if (user == null)
                {
                    return HttpNotFound(); 
                }

                return View(user); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi lấy thông tin người dùng: " + ex.Message;
                return View(); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = db.sqlstring())
                    {
                        con.Open();
                        string sql = "UPDATE tbl_User SET username = @Username, password = @Password,fullname = @FullName, address_detail = @AddressDetail, email =@Email, phone_number = @PhoneNumber,role = @Role WHERE user_id = @UserId ";

                        SqlCommand cmd = new SqlCommand(sql, con);

                      
                        cmd.Parameters.AddWithValue("@Username", user.userName);
                        cmd.Parameters.AddWithValue("@Password", user.pass);
                        cmd.Parameters.AddWithValue("@FullName", user.name);
                        cmd.Parameters.AddWithValue("@AddressDetail", user.address);
                        cmd.Parameters.AddWithValue("@Email", user.email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
                        cmd.Parameters.AddWithValue("@Role", user.role);
                        cmd.Parameters.AddWithValue("@UserId", user.id);

                        cmd.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Sửa user thành công!";
                    return RedirectToAction("user", "Home", new { area = "Admin" });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi khi sửa user: " + ex.Message;
                }
            }

            return View(user);
        }

    }
}
