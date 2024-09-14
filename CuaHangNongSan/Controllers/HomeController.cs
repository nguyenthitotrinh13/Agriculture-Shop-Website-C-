using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using CuaHangNongSan.Models;
using CuaHangNongSan.Attributes;
using CuaHangNongSan.Connection;
using PagedList;

namespace CuaHangNongSan.Controllers
{
    public class HomeController : Controller
    {
        DataConnection db = new DataConnection();
        ////string conStr = "Data Source=LAPTOP-FD3P69GF;Initial Catalog=MuaBanNongSan;Integrated Security=True";
        //private object _dbContext;

        //// GET: Home
        //public ActionResult Index()
        //{
        //    List<LatestProduct> listSanPham = new List<LatestProduct>();

        //    using (SqlConnection con = db.sqlstring())
        //    {

        //        //con.ConnectionString = conStr;
        //        con.Open();
        //        string sql = "select  product_id, image, product_name, price from tbl_Products ";

        //        DataTable dt = new DataTable();
        //        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        //        da.Fill(dt);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            var sp = new LatestProduct
        //            {
        //                Id = (int)row["product_id"],
        //                Hinh = row["image"].ToString(),
        //                Tensp = row["product_name"].ToString(),
        //                Gia = (int)row["price"]
        //            };

        //            listSanPham.Add(sp);
        //        }
        //    }
        //    return View(listSanPham);
        //}
        public ActionResult Index(int? page)
        {
            int pageSize = 10; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là trang 1
            List<LatestProduct> listSanPham = new List<LatestProduct>();

            using (SqlConnection con = db.sqlstring())
            {
                con.Open();

                // Calculate startIndex based on page number and pageSize


                // SQL query with pagination
                //string sql = $"SELECT product_id, image, product_name, price FROM tbl_Products ORDER BY product_id OFFSET {startIndex} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                string sql = "select  product_id, image, product_name, price from tbl_Products ";

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var sp = new LatestProduct
                    {
                        Id = (int)row["product_id"],
                        Hinh = row["image"].ToString(),
                        Tensp = row["product_name"].ToString(),
                        Gia = (int)row["price"]
                    };

                    listSanPham.Add(sp);
                }
            }
            IPagedList<LatestProduct> pagedList = listSanPham.ToPagedList(pageNumber, pageSize);
            // Convert listSanPham to IPagedList
            //IPagedList<LatestProduct> pagedList = listSanPham.ToPagedList(page, pageSize);

            return View(pagedList);
        }
        public ActionResult Search(string keyword)
        {
            List<LatestProduct> searchResult = new List<LatestProduct>();

            using (SqlConnection con = db.sqlstring())
            {
                //con.ConnectionString = conStr;
                con.Open();
                string sql = "SELECT image, product_name, price FROM tbl_Products WHERE product_name LIKE @Keyword";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                SqlDataReader rdr = cmd.ExecuteReader();
                if (string.IsNullOrEmpty(keyword))
                {
                    return RedirectToAction("Index");
                }
                while (rdr.Read())
                {
                    LatestProduct sp = new LatestProduct();
                    sp.Hinh = rdr["image"].ToString();
                    sp.Tensp = rdr["product_name"].ToString();
                    sp.Gia = (int)rdr["price"];
                    searchResult.Add(sp);
                }
               
            }
            return View("Search", searchResult);
        }
        private LatestProduct GetProductById(int id)
        {
            LatestProduct product = new LatestProduct();
            using (SqlConnection con = db.sqlstring())
            {
                string sql = "SELECT product_id , product_name, description, price, image FROM tbl_Products WHERE product_id = @Id";
                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new LatestProduct
                            {
                                Id = Convert.ToInt32(reader["product_id"]),
                                Tensp = Convert.ToString(reader["product_name"]),
                                Mota = Convert.ToString(reader["description"]),
                                Gia = Convert.ToInt32(reader["price"]),
                                Hinh = Convert.ToString(reader["image"])
                            };

                        }
                    }
                }
            }
            return product;
        }
        public ActionResult ProductDetail(int id)
        {
            System.Diagnostics.Debug.WriteLine("ID from URL: " + id);
            LatestProduct product = GetProductById(id);
            return View(product);
        }

        [HttpGet]
        public ActionResult AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            CartItem existingItem = cart.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                CartItem newItem = new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    Image = imageUrl
                };
                cart.Add(newItem);
            }

            Session["Cart"] = cart;

            return Redirect(Request.UrlReferrer.ToString());
        }
      


        public ActionResult About()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            return View();
        }
        //public ActionResult Blog()
        //{
        //    return View();
        //}
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}