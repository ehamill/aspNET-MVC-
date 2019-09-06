using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace aspNETfirstProject.ViewModels
{
    public class IndexSiteViewModel
    {
        public List<Site> Sites { get; set; }

        //Rest of objects for the Filter...
        public Nullable<int> CustomerID { get; set; }
        public List<SelectListItem> Customers { get; set; } 

        public string SiteNumber { get; set; } //Site number can be 1004 or S800754555

        public Nullable<int> CountryID { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public int? StateID { get; set; }
        public List<SelectListItem> States { get; set; }
        

    }

}