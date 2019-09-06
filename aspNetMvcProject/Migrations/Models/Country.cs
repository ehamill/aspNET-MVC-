using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class Country
    {
        public int ID { get; set; }

        [Required, StringLength(40)]
        public string Name { get; set; }

        [Required, StringLength(5)]
        public string Abbreviation { get; set; }

        public ICollection<State> States { get; set; }
        public ICollection<Site> Sites { get; set; }

    }
}