using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.ViewModels
{
    public class ItemsViewModel
    {
        public String Title { get; set; }
        public String ItemType { get; set; }
        public bool UserAuthorized { get; set; }
        public IList<Item> Items { get; set; }
        public string UserId { get; set; }
        public bool UserAdmin { get; set; }
    }
}