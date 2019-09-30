using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public interface IGeoRepository
    {
        Task<List<SelectListItem>> GetAllCountriesAsSelectListItem();
        Task<List<SelectListItem>> GetAllStatesAsSelectListItem();
        Task<bool> ValidateCountry(Country country);
        Task<bool> ValidateStateUnique(State state);
        Task AddCountry(Country country);
        Task AddState(State state);
    }
}