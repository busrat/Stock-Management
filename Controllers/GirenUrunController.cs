using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockControl.Models.EntityFramework;

namespace StockControl.Controllers
{
    public class GirenUrunController : Controller
    {
        private StockControlEntities db = new StockControlEntities();

        // GET: GirenUrun
        public ActionResult Index()
        {
            var girenUrun = db.GirenUrun.Where(g => g.Status);
            return View(girenUrun.ToList());
        }
       
        // GET: GirenUrun/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirenUrun girenUrun = db.GirenUrun.Find(id);
            if (girenUrun == null)
            {
                return HttpNotFound();
            }
            return View(girenUrun);
        }

        // GET: GirenUrun/Create
        public ActionResult Create()
        {
           
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer");
            ViewBag.MaterialId = new SelectList(db.Material.Where(x=>x.Status), "Id", "MaterialName");
            ViewBag.SaticiId = new SelectList(db.Satıcı, "Id", "SaticiAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GirenUrun girenUrun)
        {

            if (ModelState.IsValid)
            {
                girenUrun.IslemKodu = 101;
                girenUrun.UserId = (int)Session["Kullanici"];
                girenUrun.OlusTarih = DateTime.Now;
                girenUrun.Status = true;
                db.GirenUrun.Add(girenUrun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", girenUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x=>x.Status), "Id", "MaterialName", girenUrun.MaterialId);
            ViewBag.SaticiId = new SelectList(db.Satıcı, "Id", "SaticiAdi", girenUrun.SaticiId);
            return View(girenUrun);
        }

        // GET: GirenUrun/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirenUrun girenUrun = db.GirenUrun.Find(id);
            if (girenUrun == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", girenUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x=>x.Status), "Id", "MaterialName", girenUrun.MaterialId);
            ViewBag.SaticiId = new SelectList(db.Satıcı, "Id", "SaticiAdi", girenUrun.SaticiId);
            return View(girenUrun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GirenUrun girenUrun)
        {
            var guncellenecekUrun = db.GirenUrun.Find(girenUrun.Id);
            guncellenecekUrun.Status = false;
            if (ModelState.IsValid)
            {
                girenUrun.IslemKodu = 101;
                girenUrun.OlusTarih = guncellenecekUrun.OlusTarih;
                guncellenecekUrun.DegisTarih = DateTime.Now;
                girenUrun.Status = true;
                //db.Entry(girenUrun).State = System.Data.Entity.EntityState.Modified;
                girenUrun.UserId = (int)Session["Kullanici"];
                db.GirenUrun.Add(girenUrun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", girenUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x => x.Status), "Id", "MaterialName", girenUrun.MaterialId);
            ViewBag.SaticiId = new SelectList(db.Satıcı, "Id", "SaticiAdi", girenUrun.SaticiId);
            return View(girenUrun);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirenUrun girenUrun = db.GirenUrun.Find(id);
            if (girenUrun == null)
            {
                return HttpNotFound();
            }
            girenUrun.Status = false;
            girenUrun.OlusTarih = girenUrun.OlusTarih;
            girenUrun.DegisTarih = DateTime.Now;
            //db.GirenUrun.Remove(girenUrun);
            girenUrun.UserId = (int)Session["Kullanici"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
