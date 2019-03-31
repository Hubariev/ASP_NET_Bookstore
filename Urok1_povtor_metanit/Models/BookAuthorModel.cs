using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Urok1_povtor_metanit.Models
{
    public class BookAuthorModel
    {
        public int Price { get; set; }
        public List<Author> Authors { get; set; }
    }
}