using StockControl.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StockControl.Controllers
{
    public class SecurityController : Controller
    {
        StockControlEntities db = new StockControlEntities();
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["Kullanici"] != null) { return RedirectToAction("NotFound", "Error"); }
            else { return View(); }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            var kullaniciInDb = db.User.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (kullaniciInDb != null)
            {
                FormsAuthentication.SetAuthCookie(kullaniciInDb.Username, false);
                Session.Add("Kullanici", kullaniciInDb.Id);
                return RedirectToAction("Index","StokDurumu");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz kullanıcı adı veya şifre";
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("Kullanici");
            Session.Clear();
            return RedirectToAction("Login");

        }
    }
}