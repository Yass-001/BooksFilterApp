﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.Filter
{
    public class Filter
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? MoreThanPages { get; set; }
        public int? LessThanPages { get; set; }
        public DateTime? PublishedBefore { get; set; }
        public DateTime? PublishedAfter { get; set; }
    }

}
