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
    
    public partial class DeThi_ChiTiets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeThi_ChiTiets()
        {
            this.KetQuas = new HashSet<KetQua>();
        }
    
        public long ID { get; set; }
        public Nullable<int> IdDeThi { get; set; }
        public Nullable<long> IdCauHoi { get; set; }
    
        public virtual CauHois CauHois { get; set; }
        public virtual DeThi DeThi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KetQua> KetQuas { get; set; }
    }
}
