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

        /// <summary>
        /// Wyświetlenie wszytkich książek z bazy danych
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()

        {
            //IEnumerable<Book> books = await db.Books.ToListAsync();
            //ViewBag.Books = books;
            SelectList books = new SelectList(await db.Books.ToListAsync(), "Author", "Name");
            ViewBag.Books = books;
            return View(await db.Books.Include(p => p.Author).ToListAsync());
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            return View();
        }



        /// <summary>
        /// W  #region mamy metody sprawdzenia wieku, pobrania plików, info o użytkowniku i t.d
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        #region
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
        #endregion 




        [HttpPost]
        public RedirectResult Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;

            db.Purchases.Add(purchase);

            db.SaveChanges();

            return Redirect("/Home/Index");
        }


        /// <summary>
        /// Zamówienia
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Reads(string b)
        {
            IEnumerable<Purchase> purchases = await db.Purchases.ToListAsync();
            ViewBag.Purchases = purchases;
            ViewData["Head"] = b;
            return View();
        }


        /// <summary>
        /// Edytowanie książek z bazy danych
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region
        [HttpGet]
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                //Book book = await db.Books.Where(p => p.Id == id).FirstOrDefault();
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                SelectList authors = new SelectList(db.Authors, "Id", "Name", book.AuthorId);
                ViewBag.Authors = authors;

                return View(book);
            }
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            if (book == null)
            {
                return HttpNotFound();
            }
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        
        /// <summary>
        /// Tworzenie nowej książki
        /// </summary>
        /// <returns></returns>
        #region
        public ActionResult Create()
        {
            SelectList authors = new SelectList(db.Authors, "Id", "Name");

            ViewBag.Authors = authors;
            return View();
        }

        [HttpPost]
        public ActionResult Create(List<Book> books)
        {
            foreach(Book book in books)
            {
                if (book == null){}
                else
                {
                    db.Books.Add(book);
                }
            }          
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        /// <summary>
        /// Usunięcie książki
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region
        [HttpGet]
        public ActionResult DeleteBook(int? id)
        {
            Book book = db.Books.Find(id);
            Author vv  = db.Authors.Find(book.AuthorId);
            ViewData["Author"] = vv.Name;
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult DeleteBook(Book book)
        {
            if (book == null)
            {
                return HttpNotFound();
            }
            Book book1 = db.Books.Find(book.Id);
            db.Books.Remove(book1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult InfoAboutAuthor()
        {
            return PartialView(db.Authors);
        }

        [HttpGet]
        public ActionResult AuthorHisBooks(int? id)
        {
            var allbooks = db.Books.Where(p => p.AuthorId == id).ToList();
            if(allbooks.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allbooks);
        }

        [HttpPost]
        public ActionResult BookSearch(string name)
        {
            //Author author = db.Authors.Where(p => p.Name == name) as Author;
            //var allbooks = db.Books.Where(a => a.AuthorId == author.Id).ToList();
            var allbooks = db.Authors.Include(p => p.Books).ToList();
            if (allbooks.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allbooks);
        }

        [HttpPost]
        public ActionResult GetAuthor(Author author)
        {
            return View();
        }

        public ActionResult GetAuthor()
        {
            return View();
        }
    }
}
