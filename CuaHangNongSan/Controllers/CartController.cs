using CuaHangNongSan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangNongSan.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return View(cart);
        }
        public ActionResult RemoveFromCart(int productId)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                CartItem itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                }

                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string ShipName, string ShipMobile, string ShipAddress, string ShipEmail)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            // Kiểm tra giỏ hàng trống
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            // Kiểm tra đăng nhập
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để thanh toán!";
                return RedirectToAction("Login", "Account");
            }

            // Lấy UserId từ cơ sở dữ liệu dựa trên username trong session
            string username = Session["Username"] as string;
            int userId = GetUserIdByUsername(username);

            if (userId == -1)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index", "Cart");
            }

            // Tạo đơn hàng
            Order order = new Order
            {
                UserId = userId,
                TotalAmount = cart.Sum(item => item.Price * item.Quantity),
                Status = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ShipName = ShipName,
                ShipMobile = ShipMobile,
                ShipAddress = ShipAddress,
                ShipEmail = ShipEmail
            };

            // Thêm đơn hàng vào cơ sở dữ liệu
            int orderId = OrderDAO.AddOrder(order);

            if (orderId != -1)
            {
                // Lưu chi tiết đơn hàng
                foreach (var item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PricePerUnit = item.Price
                    };
                    OrderDetailDAO.AddOrderDetail(orderDetail);
                }

                // Xóa giỏ hàng sau khi thanh toán thành công
                Session["Cart"] = null;

                TempData["SuccessMessage"] = "Bạn đã đặt hàng thành công!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Xử lý lỗi khi thêm đơn hàng vào cơ sở dữ liệu
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý đơn hàng. Vui lòng thử lại!";
                return RedirectToAction("Index", "Cart");
            }
        }

        private int GetUserIdByUsername(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT user_id FROM tbl_User WHERE userName = @Username";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return -1; // Trường hợp không tìm thấy user_id
            }
        }
    }
}