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
    
    public partial class tbl_District
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_District()
        {
            this.tbl_Area = new HashSet<tbl_Area>();
            this.tbl_Ward = new HashSet<tbl_Ward>();
        }
    
        public string district_id { get; set; }
        public string name { get; set; }
        public string province_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Area> tbl_Area { get; set; }
        public virtual tbl_Province tbl_Province { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Ward> tbl_Ward { get; set; }
    }
}
