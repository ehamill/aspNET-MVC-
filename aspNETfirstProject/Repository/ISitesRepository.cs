﻿using aspNETfirstProject.Controllers;
using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public interface ISitesRepository
    {
        Task<IList<Site>> GetAllSites();
        Task<IList<Site>> GetFilteredSites(SiteSearchViewModel SearchModel);
        Task<Site> GetSite(int id);
        Task AddSite(Site site);
        Task UpdateSite(Site site);
        Task DeleteSite(Site site);
        Task<bool> ValidateSiteNumberUniqueForCustomer(string SiteNumber, int CustomerID);
        Task<bool> ValidateSiteTypeUniqueForCustomer(SiteType siteType);
        Task AddSiteType(SiteType siteType);
        Task<List<SelectListItem>> GetAllSiteTypesAsSelectListItem(int CustomerID);
    }
}