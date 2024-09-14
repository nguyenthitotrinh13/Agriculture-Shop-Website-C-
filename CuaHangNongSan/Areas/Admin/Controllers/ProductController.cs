using CuaHangNongSan.Attributes;
using CuaHangNongSan.Connection;
using CuaHangNongSan.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangNongSan.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private DataConnection db = new DataConnection();

        public ActionResult Create()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection con = db.sqlstring())
            {
                con.Open();
                string sql = "SELECT category_id, category_name FROM tbl_Category";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = (int)reader["category_id"],
                        CategoryName = reader["category_name"].ToString()
                    });
                }
            }

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
       
        public ActionResult Create(LatestProduct product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và lưu ảnh nếu có
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(ImageFile.FileName);
                        string physicalPath = Path.Combine(Server.MapPath("~/images/"), fileName);

                        // Lưu file vào thư mục trên máy chủ
                        ImageFile.SaveAs(physicalPath);

                        // Lưu tên file vào thuộc tính Hinh của sản phẩm
                        product.Hinh = fileName;
                    }
                    // Thực hiện lưu sản phẩm vào cơ sở dữ liệu
                    using (SqlConnection con = db.sqlstring())
                    {
                        con.Open();
                        string sql = "INSERT INTO tbl_Products (product_name, category_id, description, price, image) VALUES (@Name, @CategoryId, @Description, @Price, @Image)";
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@Name", product.Tensp);
                        cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                        cmd.Parameters.AddWithValue("@Description", product.Mota);
                        cmd.Parameters.AddWithValue("@Price", product.Gia);
                        cmd.Parameters.AddWithValue("@Image", product.Hinh);

                        cmd.ExecuteNonQuery();
                    }

                    return RedirectToAction("listProduct", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi khi tạo sản phẩm: " + ex.Message;
                }
            }

            // Load lại danh sách Category nếu có lỗi hoặc không hợp lệ
            List<Category> categories = new List<Category>();
            using (SqlConnection con = db.sqlstring())
            {
                con.Open();
                string sql = "SELECT category_id, category_name FROM tbl_Category";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = (int)reader["category_id"],
                        CategoryName = reader["category_name"].ToString()
                    });
                }
            }

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }
        public ActionResult Delete(int id)
        {
            // Tìm sản phẩm cần xóa từ ID
            LatestProduct productToDelete = null;

            using (SqlConnection con = db.sqlstring())
            {
                con.Open();
                string sql = "SELECT product_id, image, product_name, price FROM tbl_Products WHERE product_id = @ProductId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ProductId", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    productToDelete = new LatestProduct
                    {
                        Id = (int)reader["product_id"],
                        Tensp = reader["product_name"].ToString(),
                        Gia = (int)reader["price"],
                        Hinh = reader["image"].ToString()
                    };
                }
            }

            if (productToDelete == null)
            {
                return HttpNotFound(); // Hoặc thực hiện xử lý khi không tìm thấy sản phẩm
            }

            return View(productToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Xóa sản phẩm từ database
                using (SqlConnection con = db.sqlstring())
                {
                    con.Open();
                    string sql = "DELETE FROM tbl_Products WHERE product_id = @ProductId";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.ExecuteNonQuery();
                }

                // Set thông báo thành công
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công.";

                // Không redirect, mà là quay lại view listProduct
                return RedirectToAction("listProduct", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi xóa sản phẩm: " + ex.Message;
                // Xử lý lỗi và hiển thị thông báo lỗi nếu cần
                return RedirectToAction("listProduct", "Home");
            }
        }
        public ActionResult Edit(int id)
        {
            // Lấy thông tin sản phẩm từ database bằng id
            LatestProduct product = null;

            try
            {
                using (SqlConnection con = db.sqlstring())
                {
                    con.Open();
                    string sql = "SELECT product_id, image, product_name, price FROM tbl_Products WHERE product_id = @ProductId";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        product = new LatestProduct
                        {
                            Id = (int)reader["product_id"],
                            Tensp = reader["product_name"].ToString(),
                            Gia = (int)reader["price"],
                            Hinh = reader["image"].ToString(),
                        };
                    }
                }

                if (product == null)
                {
                    return HttpNotFound(); // Xử lý khi không tìm thấy sản phẩm
                }

                return View(product); // Trả về view để chỉnh sửa sản phẩm
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi lấy thông tin sản phẩm: " + ex.Message;
                return View(); // Trả về view rỗng nếu có lỗi xảy ra
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LatestProduct product, HttpPostedFileBase Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = db.sqlstring())
                    {
                        con.Open();
                        string sql = "UPDATE tbl_Products SET product_name = @ProductName, price = @Price";

                        if (Hinh != null && Hinh.ContentLength > 0)
                        {
                            // Xử lý lưu hình ảnh mới vào thư mục và cập nhật tên hình vào cơ sở dữ liệu
                            string fileName = Path.GetFileName(Hinh.FileName);
                            string path = Path.Combine(Server.MapPath("~/images/"), fileName);
                            Hinh.SaveAs(path);
                            product.Hinh = fileName;

                            sql += ", image = @Image";
                        }

                        sql += " WHERE product_id = @ProductId";

                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@ProductName", product.Tensp);
                        cmd.Parameters.AddWithValue("@Price", product.Gia);
                        cmd.Parameters.AddWithValue("@ProductId", product.Id);

                        if (Hinh != null && Hinh.ContentLength > 0)
                        {
                            cmd.Parameters.AddWithValue("@Image", product.Hinh);
                        }

                        cmd.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
                    return RedirectToAction("listProduct", "Home", new { area = "Admin" });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi khi sửa sản phẩm: " + ex.Message;
                }
            }

            return View(product);
        }





    }
}