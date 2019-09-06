using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
        public class Site
        {
            public int ID { get; set; }
            [Required]
            public int? CustomerID { get; set; }  //Int must be nullable, else int set to 0 and is never null, thus model state ALWAYS valid.
            public virtual Customer Customer { get; set; } // FK = Foriegn Key

            [Required,DisplayName("Site Number")]
            public string SiteNumber { get; set; } //Site number can be 1004 or S800754555

            public int SiteTypeID { get; set; }
            public virtual SiteType SiteType { get; set; } // FK
            [Required]
            public Nullable<int> CountryID { get; set; }
            public virtual Country Country { get; set; } // FK

            [Required]
            public int StateID { get; set; }
            public virtual State State { get; set; } // FK

            [Required]
            public string City { get; set; }

            [Required]
            public string Address { get; set; }

            [Required]
            [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
            public int Zip { get; set; }
        }
}