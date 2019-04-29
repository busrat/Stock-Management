using StockControl.Models.EntityFramework;
using StockControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockControl.Controllers
{
    public class StokDurumuController : Controller
    {
        // GET: StokDurumu
        public ActionResult Index()
        {
            StockControlEntities db = new StockControlEntities();
            var model = new UrunViewModel();
            model.material = db.Material.Where(x => x.Status).ToList();
            model.girenUrun = db.GirenUrun.Where(x => x.Status).ToList();
            model.cikanUrun = db.CikanUrun.Where(x => x.Status).ToList();
            return View(model);
        }
    }
}