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
    public class GeoRepository : IGeoRepository
    {
        private ApplicationDbContext context;
        
        public GeoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<SelectListItem>> GetAllCountriesAsSelectListItem()
        {
            List<SelectListItem> countries = await context.Countries.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToListAsync();
            return countries;
        }

        public async Task<List<SelectListItem>> GetAllStatesAsSelectListItem()
        {
            List<SelectListItem> states = await context.States.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToListAsync();
            return states;
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