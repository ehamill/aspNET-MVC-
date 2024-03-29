﻿using aspNETfirstProject.Models;
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
            List<SelectListItem> customers =await context.Customers.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToListAsync();
            return customers;
        }

        public async Task AddCustomer(Customer customer)
        {
             context.Customers.Add(customer);
            await context.SaveChangesAsync();
        }
        
    }
}