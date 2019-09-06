using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class ProfileModel
    {
        [Required]
        public string Email { get; set; }

        public string JobTitle { get; set; }

        public string ImageUrl { get; set; }

    }
}