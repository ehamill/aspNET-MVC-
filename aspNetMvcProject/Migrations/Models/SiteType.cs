using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class SiteType
    {

        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int CustomerID { get; set; } //SiteType must have a customerID
        public virtual Customer Customer { get; set; } //Each siteType can only have one customer, but customers can have
                                                       //many site types
        public ICollection<Site> Sites { get; set; }
    }
}