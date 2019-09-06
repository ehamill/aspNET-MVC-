using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
//using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace aspNETfirstProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser 
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Add Custom User Properties here:
        public string JobTitle { get; set; }
        
        [Display(Name = "Upload Gravatar")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public virtual ICollection<Item> Items { get; set; } //Need using System.Collections.Generic;
                                                             //Join User Roles
                                                             // public virtual ICollection<AspNetUserRoles> RoleKeys { get; set; }
                                                             //public virtual RoleK
                                                             /*
                                                             public string City { get; set; }
                                                             public string State { get; set; }

                                                             // Use a sensible display name for views:
                                                             [Display(Name = "Postal Code")]
                                                             public string PostalCode { get; set; }

                                                             // Concatenate the address info for display in tables and such:
                                                             public string DisplayAddress
                                                             {
                                                                 get
                                                                 {
                                                                     string dspAddress = string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;
                                                                     string dspCity = string.IsNullOrWhiteSpace(this.City) ? "" : this.City;
                                                                     string dspState = string.IsNullOrWhiteSpace(this.State) ? "" : this.State;
                                                                     string dspPostalCode = string.IsNullOrWhiteSpace(this.PostalCode) ? "" : this.PostalCode;

                                                                     return string.Format("{0} {1} {2} {3}", dspAddress, dspCity, dspState, dspPostalCode);
                                                                 }
                                                             } */
        //public class IdentityManager
        //{
        //    public bool RoleExists(string name)
        //    {
        //        var rm = new RoleManager<IdentityRole>(
        //            new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //        return rm.RoleExists(name);
        //    }


        //    public bool CreateRole(string name)
        //    {
        //        var rm = new RoleManager<IdentityRole>(
        //            new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //        var idResult = rm.Create(new IdentityRole(name));
        //        return idResult.Succeeded;
        //    }
        //}

        }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<aspNETfirstProject.Models.Item> Items { get; set; }
        public System.Data.Entity.DbSet<aspNETfirstProject.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<aspNETfirstProject.Models.Site> Sites { get; set; }
        public System.Data.Entity.DbSet<aspNETfirstProject.Models.Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<aspNETfirstProject.Models.SiteType> SiteTypes { get; set; }
        public System.Data.Entity.DbSet<aspNETfirstProject.Models.Country> Countries { get; set; }
        public System.Data.Entity.DbSet<aspNETfirstProject.Models.State> States { get; set; }
    }
}