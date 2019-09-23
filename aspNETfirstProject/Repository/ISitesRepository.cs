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
        IEnumerable<Site> GetSitesAsIEnumerable();
        Task<Site> GetSite(int id);
        void AddSite(Site site);
        void UpdateSite(Site site);
        void DeleteSite(Site site);
    }
}