using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class RezervationController : Controller
    {
        DB_AracKiralamaEntities db = new DB_AracKiralamaEntities();
        // GET: Rezervation
        public ActionResult Index()
        {

            if (Session["session_giris"] != null)
            {
                string tc = Session["session_tc"].ToString();
                return View(db.Tbl_Rezervasyonlar.Where(x=>x.TcKimlik==tc).ToList());
            }
           else
            {
                return RedirectToAction("Giris", "Customer");
            }
        }

        public ActionResult Details(int? id)
        {
            Tbl_Rezervasyonlar rezervasyon_bilgileri = db.Tbl_Rezervasyonlar.Find(id);
            int arac_id = Convert.ToInt16(rezervasyon_bilgileri.AracId);
            ViewData["a_tarihi"] = @Convert.ToString(string.Format("{0:yyyy/MM/dd}", rezervasyon_bilgileri.AlmaTarihi));
            ViewData["t_tarihi"] = @Convert.ToString(string.Format("{0:yyyy/MM/dd}", rezervasyon_bilgileri.TeslimTarihi));
            ViewData["i_tarihi"] = @Convert.ToString(string.Format("{0:yyyy/MM/dd}", rezervasyon_bilgileri.IptalTarihi));
            ViewData["ücret"] = rezervasyon_bilgileri.Ucret;
            ViewData["iptal_mi"] = rezervasyon_bilgileri.IptalMi;
            ViewData["sonuc"] = TempData["sonuc"];

            Tbl_Araclar arac_bilgileri = db.Tbl_Araclar.Find(arac_id);


            return View(arac_bilgileri);
        }
        
        //rezervasyon iptali
        public ActionResult Iptal(int? id)
        {
            Tbl_Rezervasyonlar rezervasyon_bilgileri = db.Tbl_Rezervasyonlar.Find(id);
            int rezId = rezervasyon_bilgileri.RezervasyonId;
            DateTime alma = Convert.ToDateTime(rezervasyon_bilgileri.AlmaTarihi);
            TimeSpan fark = alma - DateTime.Today;
            double days = fark.TotalDays;
            if(days>2)
            {
                rezervasyon_bilgileri.IptalMi = "Evet";
                db.Entry(rezervasyon_bilgileri).State = EntityState.Modified;
                db.SaveChanges();
                TempData["sonuc"] = "Tebrikler rezervasyon iptal edildi...";
               
            }

            else
            {
                TempData["sonuc"] = "İşlem başarısız çünkü rezervasyon çok yakın(2 günden az)...";
            }

            return RedirectToAction("Details", new { id = rezId });

        }

    }
}