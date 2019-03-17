using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Urok1_povtor_metanit.Models
{
    public class Book
    {
        
        [ScaffoldColumn(false)]
        public int Id { get; set; }//id

        [Required(ErrorMessage = "Pole musi być wypełnione")]
        [Display(Name ="Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole musi być wypełnione")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Pole musi być wypełnione")]
        public virtual ICollection<Author> Authors { get; set; }

        public Book()
        {
            Authors = new List<Author>();
        }
    }
}
