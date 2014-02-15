using ProductSearch.Services;
using ProductSearch.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Search(string q, int page = 1)
        {
            var affiliate = new AmazonAffiliate();
            var products = affiliate.Find(q, page);

            var result = new SearchResult
            {
                Products = products,
                TotalPages = affiliate.TotalPages,
                CurrentPage = page
            };

            return View("SearchResult", result);
        }
    }
}