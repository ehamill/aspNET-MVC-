using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace aspNETfirstProject.ViewModels
{
    public class IndexSiteViewModel
    {
        public IList<Site> Sites { get; set; }
        
        public int? CustomerID { get; set; }
        public List<SelectListItem> Customers { get; set; } 

        public string SiteNumber { get; set; } 

        public int? CountryID { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public int? StateID { get; set; }
        public List<SelectListItem> States { get; set; }
        

    }

}