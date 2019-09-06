using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class State
    {
        public int ID { get; set; }

        [Required, StringLength(40)]
        public string Name { get; set; }

        [Required, StringLength(2)]
        public string Abbreviation { get; set; }

        public int? CountryID { get; set; } 
        public virtual Country Country { get; set; }

        public ICollection<Site> Sites { get; set; } 
        // A site can only have one state, thus Site gets virtual and fkID
        // A state can have many sites, thus State gets ICollection
    }
}