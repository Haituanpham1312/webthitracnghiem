//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTracNghiem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CauHois
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CauHois()
        {
            this.DeThi_ChiTiets = new HashSet<DeThi_ChiTiets>();
        }
    
        public long Id { get; set; }
        public string Cauhoi { get; set; }
        public string dap_an_a { get; set; }
        public string dap_an_b { get; set; }
        public string dap_an_c { get; set; }
        public string dap_an_d { get; set; }
        public Nullable<int> IdDapAn { get; set; }
        public string ghi_chu { get; set; }
        public Nullable<byte> DaPheDuyet { get; set; }
        public Nullable<int> NguoiPheDuyet { get; set; }
        public Nullable<int> IdBai { get; set; }
        public Nullable<int> IdMucDo { get; set; }
        public Nullable<int> NguoiTao { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<int> NguoiSua { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public Nullable<byte> DaXoa { get; set; }
        public Nullable<System.DateTime> NgayXoa { get; set; }
        public Nullable<int> NguoiXoa { get; set; }
    
        public virtual Bai Bai { get; set; }
        public virtual DapAn DapAn { get; set; }
        public virtual MucDoKho MucDoKho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeThi_ChiTiets> DeThi_ChiTiets { get; set; }
    }
}