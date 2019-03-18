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
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? author, int page = 1)
        {
            int pageSize = 3;// ilość skiążek na stronie
            
            //paginacja
            IEnumerable<Book> booksPerPages = await db.Books.Include(c => c.Authors).OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            //warunek dla filtracji
            if (author != null && author != 0)
            {
                Author author1 = db.Authors.Find(author);
                booksPerPages = booksPerPages.Where(l => l.Authors.Contains(author1));
            }

            List<Author> authors = db.Authors.ToList();

            authors.Insert(0, new Author { Name = "Wszyscy", Id = 0 });

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Books.Count() };

            //model dla filtracji i paginacji
            IndexViewModel ivm = new IndexViewModel
            {
                PageInfo = pageInfo, Books = booksPerPages, Authors = new SelectList(authors, "Id", "Name")
            };

            SelectList authors_spis = new SelectList(db.Authors, "Id", "Name");
            ViewBag.Authors = authors_spis;

            return View(ivm);
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
        #region
        [HttpGet]
        public async Task<ActionResult> Reads(string b)
        {
            IEnumerable<Purchase> purchases = await db.Purchases.ToListAsync();
            ViewBag.Purchases = purchases;
            ViewData["Head"] = b;
            return View();
        }
        #endregion

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
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Authors = db.Authors;

                return View(book);
            }
        }

        [HttpPost]
        public ActionResult EditBook(Book book, int[] selectedAuthors)
        {
            Book newbook = db.Books.Find(book.Id);

            newbook.Name = book.Name;
            newbook.Price = book.Price;

            if (selectedAuthors != null)
            {
                newbook.Authors.Clear();
                foreach (var b in db.Authors.Where(p => selectedAuthors.Contains(p.Id)).ToList())
                {
                    newbook.Authors.Add(b);
                    ////book.Authors.Clear();
                }
            }

            if (book == null)
            {
                return HttpNotFound();
            }

            db.Entry(newbook).State = EntityState.Modified;
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
            ViewBag.Authors = db.Authors;

            return View();
        }


        [HttpPost]
        [HandleError(ExceptionType = typeof(System.Exception), View = "ExceptionFound")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Authors")] Book book, int[] selectedAuthors)
        {
            if(selectedAuthors != null)
            {
                foreach(var t in db.Authors.Where(p => selectedAuthors.Contains(p.Id)))
                {
                    book.Authors.Add(t);
                }
            }

            if (ModelState.IsValid && book != null )
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Authors = db.Authors;
            return View(book);
        }
        #endregion

        /// <summary>
        /// Tworzenie nowego authora
        /// </summary>
        /// <returns></returns>
        #region
        public ActionResult Create_Author()
        {
            ViewBag.Books = db.Books;
            return View();
        }

        [HttpPost]
        public ActionResult Create_Author(Author author, int[] selectedBooks)
        {
            if (selectedBooks != null)
            {
                foreach (var c in db.Books.Where(co => selectedBooks.Contains(co.Id)))//!!!
                {
                    author.Books.Add(c);
                }
            }
            if (author == null)
            {
                return HttpNotFound();
            }
            db.Authors.Add(author);
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

        [Authorize(Roles ="admin")]
        [HttpGet]
        public ActionResult AuthorHisBooks(int? id)
        {
            Author author = db.Authors.Find(id);
            return PartialView(author);
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

        

        [HttpGet]
        public ActionResult BooksForAuthor(Book book)
        {

            var authors = db.Authors.Where(p => p.Books.Contains(book)).ToList();
            return PartialView(authors);
        }

        /// <summary>
        /// Informacja o wyszukiwarce
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        /// 
        public string BrowserInfo(string browser)
        {
            return browser;
        }

        /// <summary>
        /// Wyszukuje wszystkich autorów do książki
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult AuthorSearch(string name)
        {
            List<Book> books = db.Books.Where(p => p.Name == name).ToList();

            Book book = books[0];

            return PartialView(book);
        }
    }
}
