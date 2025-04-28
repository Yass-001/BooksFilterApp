using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.Models
{
    public class BookFromCsv
    {
        public string? Title { get; set; } = string.Empty;
        public string? Pages { get; set; } = string.Empty;
        public string? ReleaseDate { get; set; } = string.Empty;
        public string? Genre { get; set; } = string.Empty;
        public string? Author { get; set; } = string.Empty;
        public string? Publisher { get; set; } = string.Empty;
    }

}
