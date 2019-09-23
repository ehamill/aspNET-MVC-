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

        public IEnumerable<Site> GetSitesAsIEnumerable()
        {
            return context.Sites.ToList();
        }


        public async Task<Site> GetSite(int id)
        {
            Site site = await context.Sites.FindAsync(id);
            return site;
        }

        public void AddSite(Site site)
        {
            context.Sites.Add(site);
             context.SaveChanges();
        }

        public void UpdateSite(Site site)
        {
            context.Sites.Add(site);
             context.SaveChanges();
        }

        public void DeleteSite(Site site)
        {
            context.Sites.Remove(site);
            context.SaveChanges();
        }

       

    }
}