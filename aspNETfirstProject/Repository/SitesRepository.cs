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
    public class SitesRepository : ISitesRepository
    {
        private ApplicationDbContext context;
        
        public SitesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<IList<Site>> GetAllSites()
        {
            IList<Site> sites = await context.Sites.ToListAsync();
            return sites;
        }

        public async Task<IEnumerable<Site>>  GetSitesAsIEnumerable()
        {
            return await context.Sites.ToListAsync();
        }


        public async Task<Site> GetSite(int id)
        {
            Site site = await context.Sites.FindAsync(id);
            return site;
        }

        public async Task AddSite(Site site)
        {
            context.Sites.Add(site);
             await context.SaveChangesAsync();
        }

        public async Task UpdateSite(Site site)
        {
            context.Entry(site).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteSite(Site site)
        {
            context.Sites.Remove(site);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateSiteNumberUniqueForCustomer(string SiteNumber, int CustomerID)
        {
            Site site = await context.Sites
                .Where(c => c.SiteNumber == SiteNumber) 
                .Where(c => c.Customer.ID == CustomerID)
                .FirstOrDefaultAsync();
            if (site == null)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public async Task<bool> ValidateSiteTypeUniqueForCustomer(SiteType siteType)
        {
            SiteType check = await context.SiteTypes
                .Where(c => c.Customer.ID == siteType.CustomerID && c.Name.ToLower() == siteType.Name.ToLower())
                .FirstOrDefaultAsync();
            if (check == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}