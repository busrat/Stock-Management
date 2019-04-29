using MongoDB.Driver;
using OfficeOpenXml;
using StockControl.Models.EntityFramework;
using StockControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockControl.Controllers
{
    [Authorize(Roles = "A")]
    public class IslemPanosuController : Controller
    {
        // GET: IslemPanosu
        public ActionResult Index()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.material = db.Material.ToList();
            model.girenUrun = db.GirenUrun.ToList();
            model.cikanUrun = db.CikanUrun.ToList();
            return View(model);
        }
        public void IslemPanosu()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.material = db.Material.ToList();
            model.girenUrun = db.GirenUrun.ToList();
            model.cikanUrun = db.CikanUrun.ToList();
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Malzeme Grubu İşlemleri");
            Sheet.Cells["A1"].Value = "MAL GRUBU";
            Sheet.Cells["B1"].Value = "KULLANICI ADI";
            Sheet.Cells["C1"].Value = "DEPARTMAN";
            Sheet.Cells["D1"].Value = "OLUŞTURULMA TARİHİ";
            Sheet.Cells["E1"].Value = "DEĞİŞTİRİLME TARİHİ";
            int row = 2;
            foreach (var item in model.material)
            {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.MaterialName;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.User.Username;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.User.Departman;
                    Sheet.Cells[string.Format("D{0}", row)].Value = "" + item.OlusTarih.ToString("dd.MM.yyyy HH:mm:ss");
                    Sheet.Cells[string.Format("E{0}", row)].Value = "" + item.DegisTarih?.ToString("dd.MM.yyyy HH:mm:ss") ?? "";
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();

            ExcelWorksheet Sheet2 = Ep.Workbook.Worksheets.Add("Ürün Girişi İşlemleri");
            Sheet2.Cells["A1"].Value = "İŞLEM KODU";
            Sheet2.Cells["B1"].Value = "MAL GRUBU";
            Sheet2.Cells["C1"].Value = "GİRİŞ TARİHİ";
            Sheet2.Cells["D1"].Value = "ADET";
            Sheet2.Cells["E1"].Value = "TUTAR";
            Sheet2.Cells["F1"].Value = "SATICI ADI";
            Sheet2.Cells["G1"].Value = "DEPO YERİ";
            Sheet2.Cells["H1"].Value = "KULLANICI ADI";
            Sheet2.Cells["I1"].Value = "DEPARTMAN";
            Sheet2.Cells["J1"].Value = "OLUŞTURULMA TARİHİ";
            Sheet2.Cells["K1"].Value = "DEĞİŞTİRİLME TARİHİ";
            int row2 = 2;
            foreach (var item2 in model.girenUrun)
            {   
                    Sheet2.Cells[string.Format("A{0}", row2)].Value = item2.IslemKodu;
                    Sheet2.Cells[string.Format("B{0}", row2)].Value = item2.Material.MaterialName;
                    Sheet2.Cells[string.Format("C{0}", row2)].Value = item2.GirisTarihi.Date.ToString("dd.MM.yyyy");
                    Sheet2.Cells[string.Format("D{0}", row2)].Value = item2.Adet;
                    if(item2.ParaBirimi==1) { Sheet2.Cells[string.Format("E{0}", row2)].Value = "" + item2.Tutar.ToString("0.00") + " TL"; }
                    else if(item2.ParaBirimi==2) { Sheet2.Cells[string.Format("E{0}", row2)].Value = "" + item2.Tutar.ToString("0.00") + " USD"; }
                    else { Sheet2.Cells[string.Format("E{0}", row2)].Value = "" + item2.Tutar.ToString("0.00") + " EUR"; }
                    Sheet2.Cells[string.Format("F{0}", row2)].Value = item2.Satıcı.SaticiAdi;
                    Sheet2.Cells[string.Format("G{0}", row2)].Value = item2.Depo.Yer;
                    Sheet2.Cells[string.Format("H{0}", row2)].Value = item2.User.Username;
                    Sheet2.Cells[string.Format("I{0}", row2)].Value = item2.User.Departman;
                    Sheet2.Cells[string.Format("J{0}", row2)].Value = "" + item2.OlusTarih.ToString("dd.MM.yyyy HH:mm:ss");
                    Sheet2.Cells[string.Format("K{0}", row)].Value = "" + item2.DegisTarih?.ToString("dd.MM.yyyy HH:mm:ss") ?? "";
                row2++;
            }
            Sheet2.Cells["A:AZ"].AutoFitColumns();

            ExcelWorksheet Sheet3 = Ep.Workbook.Worksheets.Add("Ürün Çıkışı İşlemleri");
            Sheet3.Cells["A1"].Value = "İŞLEM KODU";
            Sheet3.Cells["B1"].Value = "MAL GRUBU";
            Sheet3.Cells["C1"].Value = "ÇIKIŞ TARİHİ";
            Sheet3.Cells["D1"].Value = "ADET";
            Sheet3.Cells["E1"].Value = "SEVK YERİ";
            Sheet3.Cells["F1"].Value = "ÜRÜN ÇIKIŞ YERİ";
            Sheet3.Cells["G1"].Value = "KULLANICI ADI";
            Sheet3.Cells["H1"].Value = "DEPARTMAN";
            Sheet3.Cells["I1"].Value = "OLUŞTURULMA TARİHİ";
            Sheet3.Cells["J1"].Value = "DEĞİŞTİRİLME TARİHİ";
            int row3 = 2;
            foreach (var item3 in model.cikanUrun)
            {    
                    Sheet3.Cells[string.Format("A{0}", row3)].Value = item3.IslemKodu;
                    Sheet3.Cells[string.Format("B{0}", row3)].Value = item3.Material.MaterialName;
                    Sheet3.Cells[string.Format("C{0}", row3)].Value = item3.CikisTarihi.Date.ToString("dd.MM.yyyy");
                    Sheet3.Cells[string.Format("D{0}", row3)].Value = item3.Adet;
                    Sheet3.Cells[string.Format("E{0}", row3)].Value = item3.SevkYeri;
                    Sheet3.Cells[string.Format("F{0}", row3)].Value = item3.Depo.Yer;
                    Sheet3.Cells[string.Format("G{0}", row3)].Value = item3.User.Username;
                    Sheet3.Cells[string.Format("H{0}", row3)].Value = item3.User.Departman;
                    Sheet3.Cells[string.Format("I{0}", row3)].Value = "" + item3.OlusTarih.ToString("dd.MM.yyyy HH:mm:ss");
                    Sheet3.Cells[string.Format("J{0}", row)].Value = "" + item3.DegisTarih?.ToString("dd.MM.yyyy HH:mm:ss") ?? "";
                row3++;
            }   
            Sheet3.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
    }
}