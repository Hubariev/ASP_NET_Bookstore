using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Urok1_povtor_metanit.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int Year { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}

