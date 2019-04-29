//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

public enum ParaBirimi
{
    TRY = 1,
    USD = 2,
    EUR = 3
}
namespace StockControl.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class GirenUrun
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Mal Grubu alanı gereklidir.")]
        [Display(Name = "Mal Grubu")]
        public int MaterialId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Depo Yeri alanı gereklidir.")]
        [Display(Name = "Depo Yeri")]
        public int DepoId { get; set; }
        [Required(ErrorMessage = "Satıcı Adı alanı gereklidir.")]
        [Display(Name = "Satıcı Adı")]
        public int SaticiId { get; set; }
        public int IslemKodu { get; set; }
        [Required(ErrorMessage = "Giriş Tarihi alanı gereklidir.")]
        [Display(Name = "Giriş Tarihi")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/M/yyyy}")]
        public System.DateTime GirisTarihi { get; set; }
        [Required(ErrorMessage = "Adet alanı gereklidir.")]
        public int Adet { get; set; }
        [Required(ErrorMessage = "Tutar alanı gereklidir.")]
        public decimal Tutar { get; set; }
        public byte ParaBirimi { get; set; }
        public System.DateTime OlusTarih { get; set; }
        public Nullable<System.DateTime> DegisTarih { get; set; }
        public bool Status { get; set; }

        public virtual Depo Depo { get; set; }
        public virtual Material Material { get; set; }
        public virtual Satıcı Satıcı { get; set; }
        public virtual User User { get; set; }
    }
}
