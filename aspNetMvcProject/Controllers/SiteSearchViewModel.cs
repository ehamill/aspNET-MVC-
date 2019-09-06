using System;

namespace aspNETfirstProject.Controllers
{
    public class SiteSearchViewModel
    {
        public int? CustomerID { get; set; }

        public string SiteNumber { get; set; } //Site number can be 1004 or S800754555

        public Nullable<int> CountryID { get; set; }

        public int? StateID { get; set; }

    }
}