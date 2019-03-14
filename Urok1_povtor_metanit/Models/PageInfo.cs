using System;

namespace Urok1_povtor_metanit.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; }// numer obecnej strony

        public int PageSize { get; set; }// ilość objektów na stronie

        public int TotalItems { get; set; } // Suma objektów

        public int TotalPages // suma stron
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}