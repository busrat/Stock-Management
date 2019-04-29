using StockControl.Models.EntityFramework;
using StockControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StockControl.Controllers
{
    [Authorize(Roles = "A")]
    public class YoneticiPaneliController : Controller
    {
        private StockControlEntities db = new StockControlEntities();
        // GET: YoneticiPaneli
        public ActionResult Index()
        {
            var model = new UrunViewModel();
            model.user = db.User.Where(g=>g.Status==true).ToList();
            model.satici = db.Satıcı.Where(g=>g.Status==true).ToList();
            return View(model);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıcı satici = db.Satıcı.Find(id);
            User user = db.User.Find(id);
            if (satici == null || user == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpGet]
        public ActionResult UserCreateNew()
        {
            return View("UserCreate", new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreate(User user)
        {
            MesajViewModel model = new MesajViewModel();
            if (db.User.Where(x => x.Username.ToLower() == user.Username.ToLower()).Any())
            {
                model.Mesaj = user.Username + " zaten mevcut";
                return View("_Mesaj", model);
            }
            if (ModelState.IsValid)
            {
                user.OlusTarih = DateTime.Now;
                user.Status = true;
                db.User.Add(user);
                model.Mesaj = user.Username + " başarıyla eklendi";
            }
            db.SaveChanges();
            model.Status = true;
            return View("_Mesaj", model);
        }

        public ActionResult UserEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit(User user)
        {
            var guncellenecekUser = db.User.Find(user.Id);
            guncellenecekUser.Status = false;
            if (ModelState.IsValid)
            {
                user.OlusTarih = guncellenecekUser.OlusTarih;
                guncellenecekUser.DegisTarih = DateTime.Now;
                user.Status = true;
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public ActionResult UserDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Status = false;
            user.OlusTarih = user.OlusTarih;
            user.DegisTarih = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SaticiCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaticiCreate(Satıcı satici)
        {

            if (ModelState.IsValid)
            {
                satici.OlusTarih = DateTime.Now;
                satici.Status = true;
                db.Satıcı.Add(satici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(satici);
        }

        public ActionResult SaticiEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıcı satici = db.Satıcı.Find(id);
            if (satici == null)
            {
                return HttpNotFound();
            }
            return View(satici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaticiEdit(Satıcı satici)
        {
            var guncellenecekSatici = db.Satıcı.Find(satici.Id);
            guncellenecekSatici.Status = false;
            if (ModelState.IsValid)
            {
                satici.OlusTarih = guncellenecekSatici.OlusTarih;
                guncellenecekSatici.DegisTarih = DateTime.Now;
                satici.Status = true;
                db.Satıcı.Add(satici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(satici);
        }
        public ActionResult SaticiDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satıcı satici = db.Satıcı.Find(id);
            if (satici == null)
            {
                return HttpNotFound();
            }
            satici.Status = false;
            satici.OlusTarih = satici.OlusTarih;
            satici.DegisTarih = DateTime.Now;
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