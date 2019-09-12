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

namespace aspNETfirstProject.Controllers
{
    public class SitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // private States 
        
        //private StateDBContext test = new StateDBContext();
        
        // GET: Sites
        public ActionResult Index()
        {
            // List<Site> Sites = db.Sites.Join

            var blogs = db.Sites.SqlQuery("SELECT * FROM dbo.Sites");
            System.Diagnostics.Debug.WriteLine(blogs);

            /*
            var result = 
            var result = db.Database.SqlQuery.List<Site>("SELECT SUM(d.PurchaseValue) AS 'Total', div.Name, l.Name " +
                                                  "FROM Device AS d " +
                                                  "RIGHT JOIN Location AS l " +
                                                  "ON d.LOCATION_ID = l.ID " +
                                                  "RIGHT JOIN Division AS div " +
                                                  "ON d.DIVISION_ID = div.ID " +
                                                  "GROUP BY div.Name, l.Name " +
                                                  "ORDER BY l.Name");
            var result = db.Database.SqlQuery<List<Site>>(...);

            SELECT a.userID, b.usersFirstName, b.usersLastName FROM databaseA.dbo.TableA a inner join database B.dbo.TableB b  ON a.userID = b.userID
            
            SELECT * FROM DefaultConnection.dbo.Sites;

            var query = @"SELECT * FROM dbData as entry
              INNER JOIN Users
              ON entry.UserId = Users.Id
              ORDER BY Users.Username";
            

            //List<Site> Sites = db.Sites.ToList();
            //Sites = Sites.Join.
            List<JunkStates> States = test.States.ToList();
            */

                List < Site > Sites = db.Sites.ToList();
            List<SelectListItem> customers = new List<SelectListItem>();
            customers = db.Customers.OrderBy(r => r.Name)
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
            //int USCountryID = db.Countries.FirstOrDefault(n => n.Abbreviation == "US").ID;
            int USCountryID = 0;
            states = db.States.Where(c => c.CountryID == 1)
                .OrderBy(r => r.Name)
                .Select(rr => new SelectListItem
                    {
                        Value = rr.ID.ToString(),
                        Text = rr.Name,
                    }).ToList();

            List<Site> AllSites = db.Sites.ToList();

            var model = new IndexSiteViewModel
            {
                Sites = AllSites,
                Customers = customers,
                Countries = countries,
                States = states,
            };
            return View(model);
            
        }

        // POST: Sites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SiteSearchViewModel SearchModel)
        {
            var Context = new ApplicationDbContext();
            var result = Context.Sites.AsQueryable();

            if (SearchModel.CustomerID.HasValue)
                result = result.Where(x => x.CustomerID == SearchModel.CustomerID);
            if (SearchModel.SiteNumber != null)
                result = result.Where(x => x.SiteNumber.Contains(SearchModel.SiteNumber) );
            if (SearchModel.CountryID.HasValue)
                result = result.Where(x => x.CountryID == SearchModel.CountryID);
            if (SearchModel.StateID.HasValue)
                result = result.Where(x => x.StateID == SearchModel.StateID);
            
            List<SelectListItem> customers = new List<SelectListItem>();
            customers = db.Customers.OrderBy(r => r.Name)
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
            int USCountryID = db.Countries.FirstOrDefault(n => n.Abbreviation == "US").ID;
            states = db.States.Where(c => c.CountryID == 1)
                .OrderBy(r => r.Name)
                .Select(rr => new SelectListItem
                {
                    Value = rr.ID.ToString(),
                    Text = rr.Name,
                }).ToList();

        //            var allSites = result.ToList();
            
            var model = new IndexSiteViewModel
            {
                Sites = result.ToList(),
                Customers = customers,
                Countries = countries,
                States = states,
            };
            return View(model);

        }

        // GET: Sites/Details/5
        public ActionResult Details(int? id)
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

        // GET: Sites/Create
        public ActionResult Create()
        {
        //Get list of customers for drop down. Should really move this to a repository
        List<SelectListItem> customers = new List<SelectListItem>();
        customers = db.Customers.OrderBy(r => r.Name)
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

        var model = new CreateSiteViewModel
        {
            Customers = customers,
            Countries = countries,
        };
        return View(model);

//// Get Customers for dropdown using ViewBag
        //var customers = db.Customers.OrderBy(r => r.Name)
        //        .Select(rr => new SelectListItem
        //        {
    //            Value = rr.ID.ToString(),
        //            Text = rr.Name
        //        }).ToList();
        //var countries = db.Countries.OrderBy(r => r.Name)
        //        .Select(rr => new SelectListItem
        //        {
        //            Value = rr.ID.ToString(),
        //            Text = rr.Name
        //        }).ToList();
        //ViewBag.Customers = customers;
        //ViewBag.SiteTypes = new SiteType(); //Don't load site types because depends on Customer DropDown
        //ViewBag.Countries = countries;
        //ViewBag.States = new State(); //Don't load site types because depends on Customer DropDown
        //ViewBag.Action = "Create";
        //return View();
    }

        // POST: Sites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Site site)
        {
            
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> customers = new List<SelectListItem>();
            customers = db.Customers.OrderBy(r => r.Name)
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

            var model = new CreateSiteViewModel
            {
                Customers = customers,
                Countries = countries,
            };

            return View(model);
        }

        /// <summary>
        ///          CREATE SITE AJAX STUFF
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer([Bind(Include = "Name, BillingAddress")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
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
        public String CheckSiteNumber(int? CustomerID, String SiteNumber)
        {
            //Have to use int? because regular int is never null 
            if (CustomerID == null || SiteNumber == null)
            {
                return "<b style='color:red'>Must Select Customer And Enter Site Number!</b>"; //Should never get here
            }

            //Check if SiteNumber already exists for this customer
            var check = db.Sites
                .Where(c => c.SiteNumber == SiteNumber)  // AND
                .Where(c => c.Customer.ID == CustomerID)
                .FirstOrDefault();

            if (check != null)
            {
                return "<b style='color:red'> Site Number Already Taken!</b>";
            }
            else
            {
                return "<span style='color:green'>Site Number Looks good!</span>";
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
