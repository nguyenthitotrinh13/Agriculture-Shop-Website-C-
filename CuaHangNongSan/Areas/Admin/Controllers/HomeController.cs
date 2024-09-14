using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangNongSan.Models;
using CuaHangNongSan.Connection;
using PagedList;
using PagedList.Mvc;

namespace CuaHangNongSan.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DataConnection db = new DataConnection();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult user()
        {
            List<User> listuser = new List<User>();

            using (SqlConnection con = db.sqlstring())
            {

                con.Open();
                string sql = "select  user_id, username, password, fullname,address_detail,email, phone_number, role   from tbl_User ";

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var us = new User
                    {
                        id = row["user_id"].ToString(),
                        userName = row["username"].ToString(),
                        pass = row["password"].ToString(),
                        name = row["fullname"].ToString(),
                         address = row["address_detail"].ToString(),
                          email = row["email"].ToString(),
                           phoneNumber = row["phone_number"].ToString(),
                           role = row["role"].ToString()
                        
    };

                    listuser.Add(us);
                }
            }
            return View(listuser);
        }
        public ActionResult listProduct(int? page)
        {
            int pageSize = 7; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là trang 1

            List<LatestProduct> listSanPham = new List<LatestProduct>();

            using (SqlConnection con = db.sqlstring())
            {
                con.Open();
                string sql = @"
            SELECT 
                p.product_id, 
                p.image, 
                p.product_name, 
                p.price, 
                c.category_name 
            FROM 
                tbl_Products p
            JOIN 
                tbl_Category c ON p.category_id = c.category_id";

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);

                // Chuyển DataTable thành List
                listSanPham = dt.AsEnumerable().Select(row => new LatestProduct
                {
                    Id = row.Field<int>("product_id"),
                    Loai = row.Field<string>("category_name"),
                    Hinh = row.Field<string>("image"),
                    Tensp = row.Field<string>("product_name"),
                    Gia = row.Field<int>("price")
                }).ToList();
            }

            // Sử dụng PagedList để phân trang
            IPagedList<LatestProduct> pagedList = listSanPham.ToPagedList(pageNumber, pageSize);

            // Trả về View với danh sách sản phẩm của trang hiện tại
            return View(pagedList);
        }
    }
}