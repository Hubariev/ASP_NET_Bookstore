using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Urok1_povtor_metanit.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public SelectList Authors { get; set; }
    }
}