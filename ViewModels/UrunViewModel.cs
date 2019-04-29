using StockControl.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.ViewModels
{
    public class UrunViewModel
    {
        public IEnumerable<User> user { get; set; }
        public IEnumerable<Material> material { get; set; }
        public IEnumerable<GirenUrun> girenUrun { get; set; }
        public IEnumerable<CikanUrun> cikanUrun { get; set; }
        public IEnumerable<Depo> depo { get; set; }
        public IEnumerable<Satıcı> satici { get; set; }
    }
}