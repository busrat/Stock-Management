//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockControl.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Satıcı
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Satıcı()
        {
            this.GirenUrun = new HashSet<GirenUrun>();
        }
    
        public int Id { get; set; }
        public Nullable<int> DepoId { get; set; }
        [Required(ErrorMessage = "Satıcı Adı alanı gereklidir.")]
        [Display(Name = "Satıcı Adı")]
        public string SaticiAdi { get; set; }
        public string Adres { get; set; }
        public string PostaKodu { get; set; }
        public string Kent { get; set; }
        [Required(ErrorMessage = "Cari Kod alanı gereklidir.")]
        [Display(Name = "Cari Kod")]
        public string CariKod { get; set; }

        public System.DateTime OlusTarih { get; set; }
        public Nullable<System.DateTime> DegisTarih { get; set; }
        public bool Status { get; set; }
    
        public virtual Depo Depo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GirenUrun> GirenUrun { get; set; }
    }
}
