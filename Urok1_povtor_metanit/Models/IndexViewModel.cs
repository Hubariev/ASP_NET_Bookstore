using System;
using System.Collections.Generic;

namespace Urok1_povtor_metanit.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}