using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Views.Sites
{
    public class SiteSearchViewModel
    {    
        public int? CustomerID { get; set; }
     
        public string SiteNumber { get; set; } //Site number can be 1004 or S800754555

        public Nullable<int> CountryID { get; set; }
     
        public int? StateID { get; set; }
        
    }
}