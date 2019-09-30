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

        public async Task<bool> ValidateCountry(Country country)
        {
            var check = await context.Countries
            .Where(c => c.Name == country.Name)
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

        public async Task<bool> ValidateStateUnique(State state)
        {
            var check = await context.States
            .Where(c => c.CountryID == state.CountryID && (c.Name.ToLower() == state.Name.ToLower() || c.Abbreviation.ToLower() == state.Abbreviation.ToLower()))
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

        public async Task AddCountry(Country country)
        {
            context.Countries.Add(country);
            await context.SaveChangesAsync();
        }

        //    var States = db.States
        //    .Where(c =>c.CountryID == CountryID)
        //    .OrderBy(r => r.Name)
        //    .Select(rr => new SelectListItem
        //    {
        //        Value = rr.ID.ToString(),
        //        Text = rr.Name
        //    }).ToList();
        //return Json(new SelectList(States, "Value", "Text"));
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