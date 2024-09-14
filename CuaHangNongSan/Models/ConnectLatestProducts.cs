using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using CuaHangNongSan.Models;

namespace CuaHangNongSan.Models
{
    public class ConnectLatestProducts
    {
        public string conStr = "Data Source=LAPTOP-FD3P69GF;Initial Catalog=MuaBanNongSan;Integrated Security=True";

        public List<LatestProduct> getData()
        {
            List<LatestProduct> listProducts = new List<LatestProduct>();
            SqlConnection con = new SqlConnection(conStr);
            
            string sql = "select /*image,*/ product_name, price from tbl_Products";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                LatestProduct sp = new LatestProduct();
                //sp.Masp = Convert.ToInt32(rdr.GetValue(0).ToString());
                //sp.Hinh = rdr.GetValue(0).ToString();
                sp.Tensp = rdr.GetValue(0).ToString();
                sp.Gia = Convert.ToInt32(rdr.GetValue(1).ToString());
                //sp.Mota = rdr.GetValue(4).ToString();
                //sp.Maloai = Convert.ToInt32(rdr.GetValue(5).ToString());

                listProducts.Add(sp);

            }
            return (listProducts);
        }
    }
}