using System.Collections.Generic;
using System.Web.Mvc;
using aspNETfirstProject.Models;
using Microsoft.AspNet.Identity;
using aspNETfirstProject.Repository;
using aspNETfirstProject.ViewModels;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web;
using System.Linq;
using System.IO;

namespace aspNETfirstProject.Controllers
{
    public class ItemsController : Controller
    {

        private IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        // GET: Items
        public async Task<ActionResult> Index(string itemType)
        {
            var model = new ItemsViewModel
            {
                Title = _itemsRepository.getTitle(itemType),
                ItemType = itemType,
                UserAuthorized = User.Identity.IsAuthenticated,
                UserId = User.Identity.GetUserId(),
                UserAdmin = User.IsInRole("admin"),
                Items = await _itemsRepository.GetItems(itemType)
            };
            return View(model);
        }
        
        // GET: Items/Create
        public ActionResult Create(string itemType)
        {
            ViewBag.ItemType = itemType;
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at")] Item item)
        {
            //Get uploaded image, if there is one.
            var ImageFile = Request.Files["ImagePath"];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                item.ImagePath = SaveImage(ImageFile);
            }

            var DocumentFile = Request.Files["DocumentPath"];
            if (DocumentFile != null && DocumentFile.ContentLength > 0)
            {
                item.DocumentPath = SaveDocument(DocumentFile);
            }

            item.Approved = false;
            string currentUserId = User.Identity.GetUserId();
            if (User.IsInRole("admin"))
            {
                item.Approved = true;
            }
            item.Created_at = DateTime.Now;
            item.Updated_at = DateTime.Now;
            item.UserID = User.Identity.GetUserId();   // NOte need to include: using Microsoft.AspNet.Identity;
            if (ModelState.IsValid)
            {
                try
                {
                    _itemsRepository.AddItem(item);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error: " + ex.InnerException);
                }
                
                return RedirectToAction("Index", new { pageName = item.ItemType });
            }

            return View(item);
        }


        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await _itemsRepository.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at")] Item item)
        {
            //Need to save current ImagePath and Document path, Otherwise
            //If no image/doc uploaded the URL = null
            Item OldItem = await _itemsRepository.GetItem(item.ID);
            item.ImagePath = OldItem.ImagePath;
            item.DocumentPath = OldItem.DocumentPath;

            var ImageFile = Request.Files["ImagePath"];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                item.ImagePath = SaveImage(ImageFile);
            }

            var DocumentFile = Request.Files["DocumentPath"];
            if (DocumentFile != null && DocumentFile.ContentLength > 0)
            {
                item.DocumentPath = SaveDocument(DocumentFile);
            }

            //If user not admin, approved = false
            item.Approved = (User.IsInRole("admin")) ? true :  false;
            item.Updated_at = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _itemsRepository.UpdateItem(item);
                }
                catch (Exception ex) {
                    ModelState.AddModelError(string.Empty, "Error: " + ex.InnerException);
                }
                return RedirectToAction("Index", new { pageName = item.ItemType });
            }
            return View(item);
        }
        

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await _itemsRepository.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await _itemsRepository.GetItem(id);
            try
            {
                _itemsRepository.DeleteItem(item);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.InnerException);
            }
            return RedirectToAction("Index", new { pageName = item.ItemType });
        }

        public async Task<ActionResult> ShowComments(int ItemID)
        {
            IList<Comment> comments = await _itemsRepository.GetComments(ItemID);
            return PartialView("_CommentsPartial", comments);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> add_comment([Bind(Include = "Description,ItemID")] Comment comment, string ItemType)
        {
            comment.UserID = User.Identity.GetUserId();
            JsonResult result = new JsonResult { Data = "Invalid Model." };
            if (ModelState.IsValid)
            {
                result = await _itemsRepository.AddComment(comment);
            }
            return result;
        }


        public string SaveImage(HttpPostedFileBase ImageFile)
        {

            var imageTypes = new string[] { "image/gif", "image/jpeg", "image/jpg", "image/pjpeg", "image/png" };
            if (!imageTypes.Contains(ImageFile.ContentType))
            {
                ModelState.AddModelError("ImagePath", "Please choose either a GIF, JPG or PNG image.");
            }

            // Prepend file name with date so no duplicates..Ex "20173663439MyPic.jpg"
            var LongDate = String.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
            // Append date to fileName
            var FileName = LongDate + Path.GetFileName(ImageFile.FileName);
            // Append File Name to folder
            var path = Path.Combine(Server.MapPath("~/Images"), FileName);
            //Save image in folder..note path is to your computer..c:users/eric/phpprojects....
            ImageFile.SaveAs(path);
            //Save URL for image in model/db
            return FileName;
        }

        public string SaveDocument(HttpPostedFileBase DocumentFile)
        {
            var extension = Path.GetExtension(DocumentFile.FileName);
            var allowedExtensions = new[] { ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".docx" };
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("DocumentPath", "Document must be Word, Excel, or PDF.");
            }
            // Prepend file name with date so no duplicates..Ex "20173663439MyPic.jpg"
            var LongDate = String.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
            // Append date to fileName
            var FileName = LongDate + Path.GetFileName(DocumentFile.FileName);
            // Append File Name to folder
            var path = Path.Combine(Server.MapPath("~/Documents"), FileName);
            //Save doc in folder..note path is to your computer..c:users/eric/phpprojects....
            DocumentFile.SaveAs(path);
            //Save URL for image in model/db
            return FileName;
        }

        

    }// END CONtroller
}











//[HttpPost]
//[Authorize]
//[ValidateAntiForgeryToken]
//public ActionResult add_comment2(string Description, int ItemID, string type)
//{
//    Comment comment = new Comment();
//    comment.Description = Description;
//    comment.ItemID = ItemID;
//    comment.Created_at = DateTime.Now;
//    comment.UserID = User.Identity.GetUserId();
//    if (ModelState.IsValid)
//    {
//        db.Comments.Add(comment);
//        db.SaveChanges();
//    }
//    return RedirectToAction("Index", new { pageName = type });

//}



//public ActionResult DeleteComment(int id)
//{
//    Comment comment = db.Comments.Find(id);
//    int itemID = comment.ItemID;
//    Item item = db.Items.Find(itemID);
//    db.Comments.Remove(comment);
//    db.SaveChanges();
//    return RedirectToAction("Index", new { pageName = item.ItemType });
//}

//// POST: Items/Edit/5
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[Authorize]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at")] Item item)
//{
//    //Need to save current ImagePath and Document path, Otherwise
//    //If no image/doc uploaded the URL = null
//    Item OldItem = db.Items.Find(item.ID);
//    item.ImagePath = OldItem.ImagePath;
//    item.DocumentPath = OldItem.DocumentPath;
//    db.Entry(OldItem).State = EntityState.Detached; //Need this, else get wierd "key" error..idk asp sucks sometimes

//    //Get uploaded image, if there is one.
//    var ImageFile = Request.Files["ImagePath"];
//    if (ImageFile != null && ImageFile.ContentLength > 0)
//    {
//        //SaveImage() is custom fn; Saves image to folder, returns URL to image
//        item.ImagePath = SaveImage(ImageFile);
//    }

//    var DocumentFile = Request.Files["DocumentPath"];
//    if (DocumentFile != null && DocumentFile.ContentLength > 0)
//    {
//        //SaveImage() is custom fn; Saves image to folder, returns URL to image
//        item.DocumentPath = SaveDocument(DocumentFile);
//    }

//    //If user not admin, approved = false
//    item.Approved = false;

//    // Get User
//    string currentUserId = User.Identity.GetUserId();
//    ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
//    //Not sure what following line does, but have to have it!!!
//    var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

//    if (usermanager.IsInRole(user.Id, "admin"))
//    {
//        item.Approved = true;
//    }

//    item.Updated_at = DateTime.Now;

//    if (ModelState.IsValid)
//    {
//        db.Entry(item).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index", new { pageName = item.ItemType });
//    }
//    return View(item);
//}



//protected override void Dispose(bool disposing)
//{
//    if (disposing)
//    {
//        db.Dispose();
//    }
//    base.Dispose(disposing);
//}

//    }
//}
