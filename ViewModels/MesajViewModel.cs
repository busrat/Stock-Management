using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.ViewModels
{
    public class MesajViewModel
    {
        public bool Status { get; set; } //status trueysa başarılı falsesa başarısız
        public string Mesaj { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
    }
}