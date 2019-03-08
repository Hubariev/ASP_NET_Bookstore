using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Urok1_povtor_metanit.Models;

namespace Urok1_povtor_metanit.Controllers
{
    public class HomeController : Controller
    {

        BookContext db = new BookContext();

        public async Task<ActionResult> Index()
        {
            //IEnumerable<Book> books = await db.Books.ToListAsync();
            //ViewBag.Books = books;
            return View(await db.Books.ToListAsync());
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            return View();
        }

        public ActionResult Check(int age)
        {
            if (age < 21)
            {
                return new HttpStatusCodeResult(503);
            }
            return View();
        }

        public FileResult GetFile()
        {
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/hqdefault.jpg");
            // Тип файла - content-type
            string file_type = "application/jpg";
            // Имя файла - необязательно
            string file_name = "hqdefault.jpg";
            return File(file_path, file_type, file_name);
        }
        public FileResult GetBytes()
        {
            string path = Server.MapPath("~/Files/hqdefault.jpg");
            byte[] mas = System.IO.File.ReadAllBytes(path);
            string file_type = "application/jpg";
            string file_name = "hqdefault.jpg";
            return File(mas, file_type, file_name);
        }

        public string InfoAboutUser()
        {
            string browser = HttpContext.Request.Browser.Browser;
            string user_agent = HttpContext.Request.UserAgent;
            string url = HttpContext.Request.RawUrl;
            string ip = HttpContext.Request.UserHostAddress;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            return "<p>Browser: " + browser + "</p><p>User-Agent: " + user_agent + "</p><p>Url запроса: " + url +
                "</p><p>Реферер: " + referrer + "</p><p>IP-адрес: " + ip + "</p>";
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это частичное представление.";
            return PartialView();
        }


        [HttpPost]
        public RedirectResult Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;

            db.Purchases.Add(purchase);

            db.SaveChanges();

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<ActionResult> Reads(string b)
        {
            IEnumerable<Purchase> purchases = await db.Purchases.ToListAsync();
            ViewBag.Purchases = purchases;
            ViewData["Head"] = b;
            return View();
        }

    }
}