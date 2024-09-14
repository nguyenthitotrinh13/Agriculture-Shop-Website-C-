using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangNongSan.Models
{
    public class CartItem
    {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int UserId { get; set; }
            public int Username { get; set; }
            public string ShipName { get; set; }

            public string ShipMobile { get; set; }
            public string ShipAddress { get; set; }
            public string ShipEmail { get; set; }
                public string Image { get; set; }

    }
}