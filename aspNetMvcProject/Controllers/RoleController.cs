using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using aspNETfirstProject.Models;
using System.Threading.Tasks;

namespace aspNETfirstProject.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        /// Manage Users and Roles
        /// Page allows admin to show || add || delete user's roles
        /// 
        public ActionResult Index()
        {
            // prepopulat roles for the view dropdown
            var users = context.Users.OrderBy(r => r.UserName)
                .Select(rr => new SelectListItem
                {
                    Value = rr.UserName.ToString(),
                    Text = rr.UserName
                }).ToList();
            ViewBag.Users = users;
            
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name)
                .Select(rr => new SelectListItem {
                    Value = rr.Name.ToString(),
                    Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //var account = new AccountController();
            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ViewBag.RolesForThisUser = usermanager.AddToRole(user.Id, RoleName);
            //account.UserManager.AddToRole(user.Id, RoleName);
           // account.UserManager.AddToRoleAsync(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            ViewBag.RolesForThisUser = usermanager.GetRoles(user.Id);
            var users = context.Users.OrderBy(r => r.UserName)
                .Select(rr => new SelectListItem
                {
                    Value = rr.UserName.ToString(),
                    Text = rr.UserName
                }).ToList();
            ViewBag.Users = users;

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName) 
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {

                //Get User
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                //Get UserRoles...Need a userManager
                var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var UserRoles = usermanager.GetRoles(user.Id);
                if (UserRoles == null) {
                    ViewBag.RolesForThisUser = "No Roles Found!!";
                } else {
                    ViewBag.RolesForThisUser = usermanager.GetRoles(user.Id);
                }
                
                //Load stuff for the view...
                var list = context.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem {
                        Value = rr.Name.ToString(),
                        Text = rr.Name })
                    .ToList();
                ViewBag.Roles = list;

                var users = context.Users.OrderBy(r => r.UserName)
                .Select(rr => new SelectListItem
                {
                    Value = rr.UserName.ToString(),
                    Text = rr.UserName
                }).ToList();
                ViewBag.Users = users;
            }

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            if (usermanager.IsInRole(user.Id, RoleName))
            {
                usermanager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role <b>" + RoleName + "</b> removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "The user doesn't belong to selected role.";
            }

            // prepopulatEthe view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            var users = context.Users.OrderBy(r => r.UserName)
                .Select(rr => new SelectListItem
                {
                    Value = rr.UserName.ToString(),
                    Text = rr.UserName
                }).ToList();
            ViewBag.Users = users;
            var UserRoles = usermanager.GetRoles(user.Id);
            if (UserRoles == null)
            {
                ViewBag.RolesForThisUser = "No Roles Found!!";
            }
            else
            {
                ViewBag.RolesForThisUser = usermanager.GetRoles(user.Id);
            }
            return View("Index");
        }
        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}