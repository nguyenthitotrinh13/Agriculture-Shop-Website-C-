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
    
    public partial class tbl_Warehouse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Warehouse()
        {
            this.tbl_Receipt = new HashSet<tbl_Receipt>();
            this.tbl_Shipment = new HashSet<tbl_Shipment>();
        }
    
        public int warehouse_id { get; set; }
        public string name { get; set; }
        public string address_details { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Receipt> tbl_Receipt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Shipment> tbl_Shipment { get; set; }
    }
}