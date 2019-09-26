﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using aspNETfirstProject.Models;
using aspNETfirstProject.ViewModels;
using aspNETfirstProject.Repository;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace aspNETfirstProject.Controllers
{
    public class SitesController : Controller
    {
        private ISitesRepository _sitesRepository;
        private ICustomersRepository _customersRepository;
        private IGeoRepository _geoRepository;

        public SitesController(ISitesRepository sitesRepository, ICustomersRepository customersRepository, IGeoRepository geoRepository)
        {
            _sitesRepository = sitesRepository;
            _customersRepository = customersRepository;
            _geoRepository = geoRepository;
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sites
        public async Task<ActionResult> Index()
        {
            IList<Site> Sites = await _sitesRepository.GetAllSites();
            List<SelectListItem> customers = await _customersRepository.GetAllCustomersAsSelectListItem();
            List<SelectListItem> countries = await _geoRepository.GetAllCountriesAsSelectListItem();
            List<SelectListItem> states = await _geoRepository.GetAllStatesAsSelectListItem();

            
            var model = new IndexSiteViewModel
            {
                Sites = Sites,
                Customers = customers,
                Countries = countries,
                States = states,
            };
            return View(model);
            
        }

        // POST: Sites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>  Index(SiteSearchViewModel SearchModel)
        {
            var sites = from s in await _sitesRepository.GetSitesAsIEnumerable()
                           select s;

            if (SearchModel.CustomerID.HasValue)
                sites = sites.Where(x => x.CustomerID == SearchModel.CustomerID);
            if (SearchModel.SiteNumber != null)
                sites = sites.Where(x => x.SiteNumber.Contains(SearchModel.SiteNumber) );
            if (SearchModel.CountryID.HasValue)
                sites = sites.Where(x => x.CountryID == SearchModel.CountryID);
            if (SearchModel.StateID.HasValue)
                sites = sites.Where(x => x.StateID == SearchModel.StateID);

            List<SelectListItem> customers = await _customersRepository.GetAllCustomersAsSelectListItem();
            List<SelectListItem> countries = await _geoRepository.GetAllCountriesAsSelectListItem();
            List<SelectListItem> states = await _geoRepository.GetAllStatesAsSelectListItem();
            
            var model = new IndexSiteViewModel
            {
                Sites = sites.ToList(),
                Customers = customers,
                CustomerID = SearchModel.CountryID,
                SiteNumber = SearchModel.SiteNumber,
                Countries = countries,
                CountryID = SearchModel.CountryID,
                States = states,
                StateID = SearchModel.StateID

            };
            return View(model);

        }

        // GET: Sites/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Site site = await _sitesRepository.GetSite(id);  
            if (site == null)
            {
                return HttpNotFound("Site Not found. Invalid Site # " + id);
            }
            return View(site);
        }

        // GET: Sites/Create
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> customers = await _customersRepository.GetAllCustomersAsSelectListItem();
            List<SelectListItem> countries = await _geoRepository.GetAllCountriesAsSelectListItem();

            var model = new CreateSiteViewModel
            {
                Customers = customers,
                Countries = countries,
            };
            return View(model);
        }

        // POST: Sites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Site site)
        {
            site.SiteNumber = null;
            if (ModelState.IsValid)
            {
                try {
                    await _sitesRepository.AddSite(site);
                } catch (Exception ex) {
                    throw new HttpException("Unable to Create a Site. " + ex);
                }
                return RedirectToAction("Index");
            }
            List<SelectListItem> customers = await _customersRepository.GetAllCustomersAsSelectListItem();
            List<SelectListItem> countries = await _geoRepository.GetAllCountriesAsSelectListItem();

            var model = new CreateSiteViewModel
            {
                Customers = customers,
                Countries = countries,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCustomer([Bind(Include = "Name, BillingAddress")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _customersRepository.AddCustomer(customer);
                }
                catch (Exception ex)
                {
                    throw new HttpException("Unable to Add Customer. " + ex);
                }
                return RedirectToAction("Create");
            }
            ModelState.AddModelError("Customer", "Cannot add this customer.");
            return View("Create");
        }

        //Add new site type using ajax
        [HttpPost]
        public String AddSiteType([Bind(Include = "CustomerID, Name")] SiteType sitetype)
        {
            //Have to use int? because regular int is never null
            int? CustomerID = sitetype.CustomerID;
            if (CustomerID == null) {
                return "No CustomerID";
            }

            //Check if already exists, check if THIS customer already has this siteType name
            var check = db.SiteTypes
                .Where(c => c.Name == sitetype.Name)
                .Where(c => c.Customer.ID == sitetype.CustomerID)
                .FirstOrDefault();

            if (check != null)
            {
                return "Site Type already exists for this Customer.";
            }
            //What does bind do? it creates a new object of SiteType called sitetype and 
            // assigns the CustomerID and Name to the new object
            //Could also do this 
            //public String AddSiteType(string CustomerID, string Name) {do stuff..}
            //WHy CustomerID string? Ans: DropDown values are passed as string..convert to int later
            if (ModelState.IsValid)
            {
                db.SiteTypes.Add(sitetype);
                db.SaveChanges();
                //string Message = "Site Type " + sitetype + " added!";
                return "Site Type " + sitetype.Name + " added!";
            }
            else {
                return "Error. Cannot add Site Type";
            }
        }

    public JsonResult GetSiteTypes(int CustomerID)
    {
        var SiteTypes = db.SiteTypes.Where(c => c.Customer.ID == CustomerID)
            .OrderBy(r => r.Name)
            .Select(rr => new SelectListItem
            {
                Value = rr.ID.ToString(),
                Text = rr.Name
            }).ToList();
        return Json(new SelectList(SiteTypes, "Value", "Text"));
    }

    
        //Check if SiteNumber already exists for this Customer
        [HttpPost]
        public JsonResult CheckSiteNumber(int? CustomerID, String SiteNumber)
        {
            //Have to use int? because regular int is never null 
            if (CustomerID == null || SiteNumber == null)
            {
                return  Json(new { success = false, responseText = "Saved Note!" });

                //  "<b style='color:red'>Must Select Customer And Enter Site Number!</b>"; //Should never get here
            }

            //Check if SiteNumber already exists for this customer
            var check = db.Sites
                .Where(c => c.SiteNumber == SiteNumber)  // AND
                .Where(c => c.Customer.ID == CustomerID)
                .FirstOrDefault();

            if (check != null)
            {
                return Json(new { success = false, responseText = "Site already exists!" });
                //return "<b style='color:red'> Site Number Already Taken!</b>";
            }
            else
            {
                return Json(new { success = true, responseText = "Ok" });
                //return "<span style='color:green'>Site Number Looks good!</span>";
            }
        }

    [HttpPost]
    public String AddCountry([Bind(Include = "Name, Abbreviation")] Country country)
    {
        //Have to use int? because regular int is never null
        if (country.Name == null || country.Abbreviation == null)
        {
            return "<b style='color:red'>Must enter name and abbrev! </b>";
        }
            
        //Check if already exists...
        var check = db.Countries
            .Where(c => c.Name == country.Name)
            .FirstOrDefault();

        if (check != null)
        {
            return "<b style='color:red'>Country already exists!</b>";
        }
        if (ModelState.IsValid)
        {
            db.Countries.Add(country);
            db.SaveChanges();
            //string Message = "Site Type " + sitetype + " added!";
            return "Country " + country.Name + " added!";
        }
        else
        {
            return "<b style='color:red'>Error. Cannot add Site Type</b>";
        }
    }

    public JsonResult GetCountries()
    {
        var Countries = db.Countries
            .OrderBy(r => r.Name)
            .Select(rr => new SelectListItem
            {
                Value = rr.ID.ToString(),
                Text = rr.Name
            }).ToList();
        return Json(new SelectList(Countries, "Value", "Text"));
    }

    //Check if SiteNumber already exists for this Customer
    [HttpPost]
    public String CheckNewStateName(int? CountryID, String NewStateName)
    {
        //Have to use int? because regular int is never null 
        if (CountryID == null || NewStateName == null)
        {
            return "<b style='color:red'>Must Select COUNTRY And STATE NAME!</b>"; //Should never get here
        }

        //Check States Table to see if the COUNTRY already has this STATE
        var check = db.States
            .Where(c => c.CountryID == CountryID) //AND
            .Where(c => c.Name == NewStateName)
            .FirstOrDefault();

        if (check != null)
        {
            return "<b style='color:red'> STATE NAME Already Taken!</b>";
        }
        else
        {
            return "<span style='color:green'>State Name Looks Bueno!</span>";
        }
    }

    [HttpPost]
    public String AddState([Bind(Include = "CountryID, Name, Abbreviation")] State state )
    {
         //Have to use int? because regular int is never null (it will be 0)
        //int? NullableCountryID = state.CountryID;
      //      return "<b style='color:red'>Must enter country, name, and abbrev! </b>";
        if ( state.CountryID == null ||  state.Name == null || state.Abbreviation == null)
        {
            return "<b style='color:red'>Must enter country, name, and abbrev! </b>";
        }

        //Check if already exists...
        var check = db.States
            .Where(c => c.CountryID == state.CountryID) // AND ..
            .Where(c => c.Name == state.Name) 
            .FirstOrDefault();

        if (check != null)
        {
            return "<b style='color:red'>State already exists for this country!</b>";
        }
        if (ModelState.IsValid)
        {
            //int NewCountryID = CountryID.Value;
            //var NewState = new State
            //{
                
            //    CountryID = NewCountryID,
            //    Name = NewStateName,
            //    Abbreviation = Abbreviation,
            //};
            //    State NewState =
            db.States.Add(state);
            db.SaveChanges();
            //string Message = "Site Type " + sitetype + " added!";
            return "State " + state.Name + " added!";
        }
        else
        {
            return "<b style='color:red'>Error. Cannot add State</b>";
        }
    }

    public JsonResult GetStates(int CountryID)
    {
        var States = db.States
            .Where(c =>c.CountryID == CountryID)
            .OrderBy(r => r.Name)
            .Select(rr => new SelectListItem
            {
                Value = rr.ID.ToString(),
                Text = rr.Name
            }).ToList();
        return Json(new SelectList(States, "Value", "Text"));
    }

        /// END AJAX STUFF




        // GET: Sites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.SiteID = site.ID;

            List<SelectListItem> customers = new List<SelectListItem>();
            customers = db.Customers.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> siteTypes = new List<SelectListItem>();
            siteTypes = db.SiteTypes.Where(s => s.CustomerID == site.CustomerID).OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> countries = new List<SelectListItem>();
            countries = db.Countries.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> states = new List<SelectListItem>();
            states = db.States.Where(s=> s.CountryID == site.CountryID).OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();


            var model = new CreateSiteViewModel
            {
                CustomerID = site.CustomerID,
                Customers = customers,
                SiteTypes = siteTypes,
                SiteTypeID = site.SiteTypeID,
                SiteNumber = site.SiteNumber,
                CountryID = site.CountryID,
                Countries = countries,
                States = states,
                City = site.City,
                Address = site.Address,
                Zip = site.Zip,
            };
            return View(model);
        }

        // POST: Sites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID, SiteTypeID,SiteNumber,CountryID,StateID, City,Address, Zip")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteID = site.ID;

            List<SelectListItem> customers = new List<SelectListItem>();
            customers = db.Customers.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> siteTypes = new List<SelectListItem>();
            siteTypes = db.SiteTypes.Where(s => s.CustomerID == site.CustomerID).OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> countries = new List<SelectListItem>();
            countries = db.Countries.OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<SelectListItem> states = new List<SelectListItem>();
            states = db.States.Where(s => s.CountryID == site.CountryID).OrderBy(r => r.Name)
                    .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();


            var model = new CreateSiteViewModel
            {
                CustomerID = site.CustomerID,
                Customers = customers,
                SiteTypes = siteTypes,
                SiteTypeID = site.SiteTypeID,
                SiteNumber = site.SiteNumber,
                CountryID = site.CountryID,
                Countries = countries,
                States = states,
                City = site.City,
                Address = site.Address,
                Zip = site.Zip,
            };
            return View(model);
        }

        // GET: Sites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    
}
