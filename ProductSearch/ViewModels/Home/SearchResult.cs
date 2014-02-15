using ProductSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductSearch.ViewModels.Home
{
    public class SearchResult
    {
        public ICollection<Product> Products { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}