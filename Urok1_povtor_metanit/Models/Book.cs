using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Urok1_povtor_metanit.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AuthorId { get; set; }

        public int Price { get; set; }

        public Author Author { get; set; }
    }
}