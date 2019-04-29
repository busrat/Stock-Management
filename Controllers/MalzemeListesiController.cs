using StockControl.Models.EntityFramework;
using StockControl.Models;
using StockControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Configuration;
using MongoDB.Driver;

namespace StockControl.Controllers
{
    public class MalzemeListesiController : Controller
    {

        public ActionResult Index()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.girenUrun = db.GirenUrun.Where(x => x.Status).ToList();
            model.cikanUrun = db.CikanUrun.Where(x => x.Status).ToList();
            return View(model);
        }
        public void MalzemeListesi()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.girenUrun = db.GirenUrun.Where(x => x.Status).ToList();
            model.cikanUrun = db.CikanUrun.Where(x => x.Status).ToList();
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "İŞLEM KODU";
            Sheet.Cells["B1"].Value = "MAL GRUBU";
            Sheet.Cells["C1"].Value = "GİRİŞ TARİHİ";
            Sheet.Cells["D1"].Value = "ÇIKIŞ TARİHİ";
            Sheet.Cells["E1"].Value = "ADET";
            Sheet.Cells["F1"].Value = "TUTAR";
            Sheet.Cells["G1"].Value = "SATICI ADI";
            Sheet.Cells["H1"].Value = "DEPO YERİ";
            Sheet.Cells["I1"].Value = "SEVK YERİ";
            Sheet.Cells["J1"].Value = "KULLANICI ADI";
            Sheet.Cells["K1"].Value = "DEPARTMAN";
            int row = 2;
            foreach (var item in model.girenUrun)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.IslemKodu;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Material.MaterialName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.GirisTarihi.Date.ToString("dd.MM.yyyy");
                Sheet.Cells[string.Format("D{0}", row)].Value = null;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Adet;
                if (item.ParaBirimi == 1) { Sheet.Cells[string.Format("F{0}", row)].Value = "" + item.Tutar.ToString("0.00") + " TL"; }
                else if (item.ParaBirimi == 2) { Sheet.Cells[string.Format("F{0}", row)].Value = "" + item.Tutar.ToString("0.00") + " USD"; }
                else { Sheet.Cells[string.Format("F{0}", row)].Value = "" + item.Tutar.ToString("0.00") + " EUR"; }
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Satıcı.SaticiAdi;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.Depo.Yer;
                Sheet.Cells[string.Format("I{0}", row)].Value = null;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.User.Username;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.User.Departman;
                row++;
            }
            foreach (var item2 in model.cikanUrun)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item2.IslemKodu;
                Sheet.Cells[string.Format("B{0}", row)].Value = item2.Material.MaterialName;
                Sheet.Cells[string.Format("C{0}", row)].Value = null;
                Sheet.Cells[string.Format("D{0}", row)].Value = item2.CikisTarihi.Date.ToString("dd.MM.yyyy");
                Sheet.Cells[string.Format("E{0}", row)].Value = item2.Adet;
                Sheet.Cells[string.Format("F{0}", row)].Value = null;
                Sheet.Cells[string.Format("G{0}", row)].Value = null;
                Sheet.Cells[string.Format("H{0}", row)].Value = item2.Depo.Yer;
                Sheet.Cells[string.Format("I{0}", row)].Value = item2.SevkYeri;
                Sheet.Cells[string.Format("J{0}", row)].Value = item2.User.Username;
                Sheet.Cells[string.Format("K{0}", row)].Value = item2.User.Departman;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
        public void StokDurumu()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.girenUrun = db.GirenUrun.Where(x => x.Status).ToList();
            model.cikanUrun = db.CikanUrun.Where(x => x.Status).ToList();
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "MAL GRUBU";
            Sheet.Cells["B1"].Value = "DEPO YERİ";
            Sheet.Cells["C1"].Value = "ADET";
            int row = 2;
            var girenUrunToplam = model.girenUrun
                     .GroupBy(x => new { x.Depo.Yer, x.Material.MaterialName })
                     .Select(g => new
                     {
                         g.Key.MaterialName,
                         g.Key.Yer,
                         total = g.Sum(t => t.Adet)
                     });

            var cikanUrunToplam = model.cikanUrun
                .GroupBy(x => new { x.Depo.Yer, x.Material.MaterialName })
                .Select(g => new
                {
                    g.Key.MaterialName,
                    g.Key.Yer,
                    total = g.Sum(t => t.Adet)
                });

            foreach (var inProduct in girenUrunToplam)
            {
                var outProduct = cikanUrunToplam.Where(p => p.MaterialName == inProduct.MaterialName && p.Yer == inProduct.Yer).FirstOrDefault();
                if (outProduct == null)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = inProduct.MaterialName;
                    Sheet.Cells[string.Format("B{0}", row)].Value = inProduct.Yer;
                    Sheet.Cells[string.Format("C{0}", row)].Value = inProduct.total;
                }
                row++;
            }
                foreach (var inProduct2 in girenUrunToplam)
                {
                    var outProduct2 = cikanUrunToplam.Where(p => p.MaterialName == inProduct2.MaterialName && p.Yer == inProduct2.Yer).FirstOrDefault();
                    if (outProduct2 != null)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = inProduct2.MaterialName;
                        Sheet.Cells[string.Format("B{0}", row)].Value = inProduct2.Yer;
                        Sheet.Cells[string.Format("C{0}", row)].Value = (inProduct2.total - outProduct2.total);
                    }
                    row++;
                }
                Sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ReportS.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();
        }
    }
}
   