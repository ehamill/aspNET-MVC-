using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public interface ICustomersRepository
    {
        Task<List<SelectListItem>> GetAllCustomersAsSelectListItem();
        Task AddCustomer(Customer customer);
    }
}