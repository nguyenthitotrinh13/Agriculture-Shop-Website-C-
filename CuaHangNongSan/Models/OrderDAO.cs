using CuaHangNongSan.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace CuaHangNongSan.Models
{
    public class OrderDAO
    { 
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    DataConnection db = new DataConnection();

    public static int AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO tbl_Order (user_id, total_amount, status, created_at, updated_at, ShipName, ShipMobile, ShipAddress, ShipEmail)
                             VALUES (@UserId, @TotalAmount, @Status, @CreatedAt, @UpdatedAt, @ShipName, @ShipMobile, @ShipAddress, @ShipEmail);
                             SELECT CAST(scope_identity() AS int)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", order.UserId);
                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", order.UpdatedAt);
                command.Parameters.AddWithValue("@ShipName", order.ShipName);
                command.Parameters.AddWithValue("@ShipMobile", order.ShipMobile);
                command.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                command.Parameters.AddWithValue("@ShipEmail", order.ShipEmail);

                connection.Open();
                int orderId = (int)command.ExecuteScalar();
                return orderId;
            }
        }
    }
}