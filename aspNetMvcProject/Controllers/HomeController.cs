using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search (String SearchInput)
        {      
            string[] strings = SearchInput.Split(' ');
            var Context = new ApplicationDbContext();
            var result = Context.Items.AsQueryable();

            if (!String.IsNullOrEmpty(SearchInput))
            {
                foreach (var splitString in strings)
                {
                    result = result.Where(x => x.Title.Contains(splitString) || x.Description.Contains(splitString));
                }
            }
            return PartialView("_SearchResultsPartial", result);           
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
    }
}