using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CustomerController : Controller
    {


        DB_AracKiralamaEntities db = new DB_AracKiralamaEntities();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }


        public static string GetMD5_2(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }


            return byte2String;
        }


        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(FormCollection bilgi)
        {
            string tc = bilgi["tc"].ToString();
            string sif = GetMD5_2(bilgi["sif"].ToString());
            var count = db.Tbl_Musteriler.Where(x => x.TcKimlik == tc && x.Sifre == sif).Count();

            if (count == 0)
            {
                ViewData["sonuc"] = "Hata! Kayıtlı Değilsiniz veya Bilgileriniz Yanlış.";

            }

            else
            {
                Session["session_giris"] = true;
                Session["session_tc"] = tc;
                //ViewData["sonuc"] = "Tebrikler Başarıyla Giriş Yaptınız...";
                return RedirectToAction("Profil", "Customer");


            }

            return View();
        }

        public ActionResult İndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kayit([Bind(Include = "TcKimlik,AdSoyad,DogumTarihi,Cinsiyet,Telefon,Sifre")] Tbl_Musteriler musteri)
        {
            db.Tbl_Musteriler.Add(musteri);
            int result = db.SaveChanges();
            if (result > 0)
            {
                ViewData["sonuc"] = "Tebrikler! Kaydınız gerçekleşti...";
            }
            else
            {
                ViewData["sonuc"] = "Üzgünüm! Kaydınız gerçekleşti...";
            }
            return View();

        }

        [HttpGet]
        public ActionResult Profil()
        {
            if (Session["session_giris"] != null)
            {
                string tc = Session["session_tc"].ToString();
                Tbl_Musteriler musteri = db.Tbl_Musteriler.Find(tc);
                if (musteri == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(musteri);

                }
            }
            else
            {
                return RedirectToAction("Giris");
            }

        }

        [HttpPost]
        public ActionResult Profil(FormCollection bilgi)
        {
            if(ModelState.IsValid)
            {
                string tc = Session["session_tc"].ToString();
                Tbl_Musteriler musteri = db.Tbl_Musteriler.Find(tc);

                musteri.AdSoyad = bilgi["AdSoyad"].ToString();
                musteri.DogumTarihi = Convert.ToDateTime(bilgi["DogumTarihi"]);
                musteri.Cinsiyet = bilgi["Cinsiyet"].ToString();
                musteri.Telefon = bilgi["Telefon"].ToString();

                db.Entry(musteri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profil");              
            }
            return View();
        }

        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Giris");
        }

        [HttpGet]
        public ActionResult SifreDegistir()
        {
            return View();

        }

        [HttpPost]
        public ActionResult SifreDegistir(FormCollection bilgi)
        {
            string tc = Session["session_tc"].ToString();
            string sif = GetMD5_2(bilgi["msif"].ToString());
            var count = db.Tbl_Musteriler.Where(x => x.TcKimlik == tc && x.Sifre == sif).Count();
            if (count > 0)
            {
                if (bilgi["ysif1"] == bilgi["ysif2"])
                {
                    string ysif = GetMD5_2(bilgi["ysif1"].ToString());
                    Tbl_Musteriler musteri = db.Tbl_Musteriler.Find(tc);
                    musteri.Sifre = ysif;
                    db.Entry(musteri).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["sonuc"] = "Tebrikler, şifreniz değişti...";
                }
                else
                {
                    ViewData["sonuc"] = "Yeni şifreler Uyuşmuyor";
                }
            }
            else
            {
                ViewData["sonuc"] = "Hata Mevcut Şifreyi Yanlış Yazdınız.";
            }

            return View();
        }

    }
}