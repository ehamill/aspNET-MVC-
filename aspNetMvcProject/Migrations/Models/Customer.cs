using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
        public class Customer
        {
            public int ID { get; set; }

            [Required, StringLength(40)]
            public string Name { get; set; }

            public string BillingAddress { get; set; }

            public ICollection<Site> Sites { get; set; }
            public ICollection<SiteType> SiteTypes { get; set; }
    }
}