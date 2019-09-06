using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.ViewModels
{
    public class CreateSiteViewModel
    {

        [Required(ErrorMessage = "Please select a Customer")]
        public Nullable<int> CustomerID { get; set; }  //Going to store the Selected Item's id (in dropDowns' selected item's value is string ..usually a number though
        public List<SelectListItem> Customers { get; set; } //Used to populate the drop Down.

        [Required, DisplayName("Site Number")]
        public string SiteNumber { get; set; } //Site number can be 1004 or S800754555

        [Required]
        public int? SiteTypeID { get; set; }  //Going to store the Selected Item's id
        public List<SelectListItem> SiteTypes { get; set; } //Used to populate the drop Down.

        [Required]
        public Nullable<int> CountryID { get; set; }  //Going to store the Selected Item's id
        public List<SelectListItem> Countries { get; set; } //Used to populate the drop Down.

        [Required]
        public int? StateID { get; set; }
        public List<SelectListItem> States { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public int? Zip { get; set; }
    }

}