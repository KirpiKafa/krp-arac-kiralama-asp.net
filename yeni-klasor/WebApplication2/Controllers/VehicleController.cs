using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class VehicleController : Controller
    {
        DB_AracKiralamaEntities db = new DB_AracKiralamaEntities();

        // GET: Vehicle
        public ActionResult Index()
        {
            return View(db.Tbl_Araclar.ToList());
        }

        public ActionResult Details(int? id)
        {
            Tbl_Araclar arac_bilgileri = db.Tbl_Araclar.Find(id);
            return View(arac_bilgileri);

        }

        [HttpGet]

        public ActionResult Rezervation(int? id)
        {
            Tbl_Araclar arac_bilgileri = db.Tbl_Araclar.Find(id);
            ViewData["Marka"] = arac_bilgileri.Marka;
            ViewData["Model"] = arac_bilgileri.Model;
            ViewData["Fiyat"] = arac_bilgileri.Fiyat;
            return View();
      }

        [HttpPost]
        public ActionResult Rezervation([Bind(Include = "RezervasyonId,AracId,TcKimlik,AdSoyad,AlmaTarihi,TeslimTarihi,Ucret")]Tbl_Rezervasyonlar rezervasyon)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Rezervasyonlar.Add(rezervasyon);
                db.SaveChanges();
            }
            ViewBag.Message = "Tebrikler, rezervasyon işleminiz başarı ile gerçekleştirildi.";
            return View();
        }

    }
}
