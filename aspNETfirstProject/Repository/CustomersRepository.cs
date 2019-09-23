using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public class CustomersRepository : ICustomersRepository
    {
        private ApplicationDbContext context;
        
        public CustomersRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<List<SelectListItem>> GetAllCustomersAsSelectListItem()
        {
            //IList<Customer> customers = await new List<SelectListItem>();
            //List<SelectListItem> customers = new List<SelectListItem>();
            List<SelectListItem> customers =await context.Customers.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToListAsync();
            return customers;
        }

        //public async Task<Site> GetSite(int id)
        //{
        //    Site site = await context.Sites.FindAsync(id);
        //    return site;
        //}

        //public void AddSite(Site site)
        //{
        //    context.Sites.Add(site);
        //     context.SaveChanges();
        //}

        //public void UpdateSite(Site site)
        //{
        //    context.Sites.Add(site);
        //     context.SaveChanges();
        //}

        //public void DeleteSite(Site site)
        //{
        //    context.Sites.Remove(site);
        //    context.SaveChanges();
        //}

    }
}