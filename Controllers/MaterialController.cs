using StockControl.Models.EntityFramework;
using StockControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockControl.Controllers
{
    [Authorize(Roles = "A")]
    public class MaterialController : Controller
    {
        private StockControlEntities db = new StockControlEntities();

        public ActionResult Index()
        { 
            var model = db.Material.Where(x=>x.Status).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult New()
        {
            return View("MaterialForm", new Material());
        }
        [ValidateAntiForgeryToken]
        public ActionResult Save(Material material)
        {
           
            if (!ModelState.IsValid)
            {
                return View("MaterialForm");
            }
            MesajViewModel model = new MesajViewModel();

            if (db.Material.Where(x=>x.Status).Where(x => x.MaterialName.ToLower() == material.MaterialName.ToLower()).Any()) {
                model.Mesaj = material.MaterialName + " zaten mevcut";
                return View("_Mesaj", model);
            }
            if (ModelState.IsValid)
            {
                material.UserId=(int)Session["Kullanici"];
                material.OlusTarih = DateTime.Now;
                material.Status = true;
                db.Material.Add(material);
                model.Mesaj = material.MaterialName + " başarıyla eklendi";
            }
           //else
           // {
           //     var guncellenecekMalzeme = db.Material.Find(material.Id);
           //     guncellenecekMalzeme.status = false;
           //     guncellenecekMalzeme.degistarih = DateTime.Now;
           //     if (guncellenecekMalzeme == null)
           //     {
           //         return HttpNotFound();
           //     }
           //     //guncellenecekMalzeme.materialname = material.materialname;
           //     material.userId = (int)Session["Kullanici"];
           //     material.olustarih = guncellenecekMalzeme.olustarih;
           //     //material.degistarih = DateTime.Now;
           //     material.status = true;
           //     db.Material.Add(material);
           //     model.Mesaj = material.materialname + " başarıyla güncellendi";
           // }   
            db.SaveChanges();
            model.Status = true; 
            return View("_Mesaj", model); 
        }
        public ActionResult Update(int id)
        {
            var model = db.Material.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Material material)
        {
            var guncellenecekMalzeme = db.Material.Find(material.Id);
            guncellenecekMalzeme.Status = false;
            guncellenecekMalzeme.DegisTarih = DateTime.Now;
            if (guncellenecekMalzeme == null)
            {
                return HttpNotFound();
            }
            //guncellenecekMalzeme.materialname = material.materialname;
            material.UserId = (int)Session["Kullanici"];
            material.OlusTarih = guncellenecekMalzeme.OlusTarih;
            //material.degistarih = DateTime.Now;
            material.Status = true;
            db.Material.Add(material);
            db.SaveChanges();
                return RedirectToAction("Index");
            }

        public ActionResult Delete(int id)
        {
            var silinecekMalzeme = db.Material.Find(id);
            if (silinecekMalzeme == null)
                return HttpNotFound();
            silinecekMalzeme.UserId = (int)Session["Kullanici"];
            silinecekMalzeme.Status = false;
            silinecekMalzeme.OlusTarih = silinecekMalzeme.OlusTarih;
            silinecekMalzeme.DegisTarih = DateTime.Now;
            //db.Material.Remove(silinecekMalzeme);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}