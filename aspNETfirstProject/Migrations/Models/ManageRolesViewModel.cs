using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class ManageRolesViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> Admins { get; set; }

    }
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Roles { get; set; }
    }
}