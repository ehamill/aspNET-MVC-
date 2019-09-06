using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.ViewModels
{
    public class StatesView
    {
        //{ name = s.Name, counrtyID = s.CountryID, countryName = c.Name};
        public string name { get; set; }
        public int CountryID { get; set; }
        public string countryName { get; set; }

    }
}