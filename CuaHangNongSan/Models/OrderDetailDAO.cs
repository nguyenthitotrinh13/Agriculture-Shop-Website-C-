using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CuaHangNongSan.Models
{
    public class OrderDetailDAO
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void AddOrderDetail(OrderDetail orderDetail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO tbl_OrderDetail (order_id, product_id, quantity, price_per_unit)
                             VALUES (@OrderId, @ProductId, @Quantity, @PricePerUnit)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderDetail.OrderId);
                command.Parameters.AddWithValue("@ProductId", orderDetail.ProductId);
                command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                command.Parameters.AddWithValue("@PricePerUnit", orderDetail.PricePerUnit);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}