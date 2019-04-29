using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockControl.Models.EntityFramework;
using StockControl.ViewModels;

namespace StockControl.Controllers
{
    public class CikanUrunController : Controller
    {
        private StockControlEntities db = new StockControlEntities();

        // GET: CikanUrun
        public ActionResult Index()
        {
            var cikanUrun = db.CikanUrun.Where(c=>c.Status);
            return View(cikanUrun.ToList());
        }

        // GET: CikanUrun/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CikanUrun cikanUrun = db.CikanUrun.Find(id);
            if (cikanUrun == null)
            {
                return HttpNotFound();
            }
            return View(cikanUrun);
        }

        // GET: CikanUrun/Create
        public ActionResult Create()
        {
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer");
            ViewBag.MaterialId = new SelectList(db.Material.Where(x=>x.Status), "Id", "MaterialName");
            return View();
        }

        // POST: CikanUrun/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CikanUrun cikanUrun)
        {

            if (ModelState.IsValid)
            {
                cikanUrun.IslemKodu = 102;
                cikanUrun.UserId = (int)Session["Kullanici"];
                cikanUrun.OlusTarih = DateTime.Now;
                cikanUrun.Status = true;
                db.CikanUrun.Add(cikanUrun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", cikanUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x => x.Status), "Id", "MaterialName", cikanUrun.MaterialId);
            return View(cikanUrun);
        }

        // GET: CikanUrun/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CikanUrun cikanUrun = db.CikanUrun.Find(id);
            if (cikanUrun == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", cikanUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x=>x.Status), "Id", "MaterialName", cikanUrun.MaterialId);
            return View(cikanUrun);
        }

        // POST: CikanUrun/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CikanUrun cikanUrun)
        {
            var guncellenecekUrun = db.CikanUrun.Find(cikanUrun.Id);
            guncellenecekUrun.Status = false;
            if (ModelState.IsValid)
            {
                cikanUrun.IslemKodu = 102;
                cikanUrun.OlusTarih = guncellenecekUrun.OlusTarih;
                guncellenecekUrun.DegisTarih = DateTime.Now;
                cikanUrun.Status = true;
                db.CikanUrun.Add(cikanUrun);
                //db.Entry(cikanUrun).State = System.Data.Entity.EntityState.Modified;
                cikanUrun.UserId = (int)Session["Kullanici"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepoId = new SelectList(db.Depo, "Id", "Yer", cikanUrun.DepoId);
            ViewBag.MaterialId = new SelectList(db.Material.Where(x => x.Status), "Id", "MaterialName", cikanUrun.MaterialId);
            return View(cikanUrun);
        }

        // GET: CikanUrun/Delete/5
        public ActionResult Delete(CikanUrun cikanUrun)
        {
            
            var silinecekUrun = db.CikanUrun.Find(cikanUrun.Id);
            if (silinecekUrun == null)
            {
                return HttpNotFound();
            }
            silinecekUrun.Status = false;
            cikanUrun.OlusTarih = silinecekUrun.OlusTarih;
            silinecekUrun.DegisTarih=DateTime.Now;
            //db.CikanUrun.Remove(cikanUrun);
            cikanUrun.UserId = (int)Session["Kullanici"];
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

        public string GetStock(int MaterialId, int DepoId)
        {
            int did = DepoId;
            int mid = MaterialId;
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            var girenUrunToplam = db.GirenUrun
                .GroupBy(x => new { x.DepoId, x.MaterialId ,x.Status})
                .Select(g => new
                {
                    g.Key.MaterialId,
                    g.Key.DepoId,
                    g.Key.Status,          
                    total = g.Sum(t => t.Adet)
                }
                )
                .Where(x=>x.MaterialId==mid && x.DepoId== did && x.Status).ToList();

            var cikanUrunToplam = db.CikanUrun
                .GroupBy(x => new { x.DepoId, x.MaterialId, x.Status })
                .Select(g => new
                {
                    g.Key.MaterialId,
                    g.Key.DepoId,
                    g.Key.Status,
                    total = g.Sum(t => t.Adet)
                }
                )
                .Where(x => x.MaterialId == mid && x.DepoId == did && x.Status).ToList();
            var kalanMiktar = 0;
            foreach (var inProduct in girenUrunToplam)
            {
                var outProduct = cikanUrunToplam.Where(p => p.MaterialId == inProduct.MaterialId && p.DepoId == inProduct.DepoId).FirstOrDefault();
                
                if (outProduct==null)
                {
                    kalanMiktar = inProduct.total;
                }
                else {
                    kalanMiktar = inProduct.total - outProduct.total;
                }
            }
            return kalanMiktar.ToString();
        }

    }
}
