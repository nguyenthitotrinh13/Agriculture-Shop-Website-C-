//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CuaHangNongSan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Area
    {
        public string area_id { get; set; }
        public string provinve_id { get; set; }
        public string district_id { get; set; }
        public string ward_id { get; set; }
    
        public virtual tbl_District tbl_District { get; set; }
        public virtual tbl_Province tbl_Province { get; set; }
        public virtual tbl_Ward tbl_Ward { get; set; }
    }
}
