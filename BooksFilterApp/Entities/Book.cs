using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public int Pages { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }
        public Guid PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

    }

}
