using aspNETfirstProject.Controllers;
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

        public async Task<IList<Site>>  GetFilteredSites(SiteSearchViewModel SearchModel)
        {
            var sites = from s in context.Sites
                        select s;
            if (SearchModel.CustomerID.HasValue)
                sites = sites.Where(x => x.CustomerID == SearchModel.CustomerID);
            if (SearchModel.SiteNumber != null)
                sites = sites.Where(x => x.SiteNumber.Contains(SearchModel.SiteNumber));
            if (SearchModel.CountryID.HasValue)
                sites = sites.Where(x => x.CountryID == SearchModel.CountryID);
            if (SearchModel.StateID.HasValue)
                sites = sites.Where(x => x.StateID == SearchModel.StateID);
            return await sites.ToListAsync();
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

        public async Task AddSiteType(SiteType siteType)
        {
            context.SiteTypes.Add(siteType);
            await context.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetAllSiteTypesAsSelectListItem(int CustomerID)
        {
            List<SelectListItem> siteTypes = await context.SiteTypes.Where(c => c.Customer.ID == CustomerID)
                .OrderBy(r => r.Name)
                .Select(rr => new SelectListItem
                {
                    Value = rr.ID.ToString(),
                    Text = rr.Name
                }).ToListAsync();
            
            return siteTypes;
        }
    }
}