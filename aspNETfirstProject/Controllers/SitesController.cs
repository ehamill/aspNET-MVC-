using System;
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

        //private ApplicationDbContext db = new ApplicationDbContext();

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
        [ValidateAntiForgeryToken]
        public String AddSiteType([Bind(Include = "CustomerID, Name")] SiteType sitetype)
        {
            
            bool SiteTypeUnique = await _sitesRepository.ValidateSiteTypeUniqueForCustomer(sitetype)

            if (SiteTypeUnique)
            {
                return Json(new { success = true, responseText = "Ok" });
            }
            else
            {
                return Json(new { success = false, responseText = "Site already exists for this Customer!" });
            }

            if (check != null)
            {
                return "Site Type already exists for this Customer.";
            }
            if (ModelState.IsValid)
            {
                db.SiteTypes.Add(sitetype);
                db.SaveChanges();
                Json(new { success = true, responseText = "SiteType added" });
            }
            else {
                Json(new { success = false, responseText = "SiteType missing fields!" });
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
        public async Task<JsonResult> ValidateSiteNumber(int CustomerID, String SiteNumber)
        {
            bool SiteUnique = await _sitesRepository.ValidateSiteNumberUniqueForCustomer(SiteNumber, CustomerID);
            
            if (SiteUnique)
            {
                return Json(new { success = true, responseText = "Ok" });
            }
            else
            {
                return Json(new { success = false, responseText = "Site already exists for this Customer!" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCountry([Bind(Include = "Name, Abbreviation")] Country country)
        {
           
            bool CountryUnique = await _geoRepository.ValidateCountry(country);

            if (CountryUnique == false)
            {
                return Json(new { success = false, responseText = "Country already exists!" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _geoRepository.AddCountry(country);
                    return Json(new { success = true, responseText = "Ok" });
                }
                catch (Exception ex)
                {
                    throw new HttpException("Unable to Add Country. " + ex);
                }
                
            }
            ModelState.AddModelError("Country", "Cannot add this Country.");
            return Json(new { success = false, responseText = "Country error!" });
        }

    public async Task<JsonResult> GetCountries()
    {
            return Json(await _geoRepository.GetAllCountriesAsSelectListItem());
    }
        
    [HttpPost]
    public async Task<JsonResult> VerifyNewStateName([Bind(Include = "CountryID, Name, Abbreviation")] State state)
    {
            bool StateUnique = await _geoRepository.ValidateStateUnique(state);

            if (StateUnique)
            {
                return Json(new { success = true, responseText = "Ok" });
            }
            else
            {
                return Json(new { success = false, responseText = "State already exists for this Country!" });
            }
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> AddState([Bind(Include = "CountryID, Name, Abbreviation")] State state )
    {

        if (ModelState.IsValid)
        {
            bool StateUnique = await _geoRepository.ValidateStateUnique(state);

            if (StateUnique == false)
            {
                return Json(new { success = false, responseText = "State already exists for this Country!" });
            }

            try
            {
                await _geoRepository.AddState(state);
                return Json(new { success = true, responseText = "Ok" });
            }
            catch (Exception ex)
            {
                throw new HttpException("Unable to Add Country. " + ex);
            }
                
        }
        return Json(new { success = false, responseText = "<b style='color:red'>Must enter country, name, and abbrev! </b>" });
            
    }

    public async Task<JsonResult>  GetStates(int CountryID)
    {
            return Json(await _geoRepository.GetAllStatesAsSelectListItem());
        
    }
       
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
