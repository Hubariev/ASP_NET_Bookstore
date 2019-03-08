using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Urok1_povtor_metanit.Models
{
    public class BookDbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext db)
        {
            db.Books.Add(new Book { Name = "Tom and Jerry", Author = "Disney", Price = 220 });
            db.Books.Add(new Book { Name = "Booby where r you", Author = "Douglas", Price = 180 });
            db.Books.Add(new Book { Name = "Merry", Author = "Anton", Price = 150 });

            base.Seed(db);
        }
    }
}