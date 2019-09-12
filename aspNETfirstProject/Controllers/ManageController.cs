using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using aspNETfirstProject.Models;
using System.IO;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace aspNETfirstProject.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var model = new ProfileModel
            {
                Email = user.Email,
                JobTitle = user.JobTitle,
                ImageUrl = user.ImageUrl
                //Role = UserManager.GetRoles.
            };
            var UserID = User.Identity.GetUserId();
            
            ViewBag.NumPendingPosts = db.Items.Where(column => column.Approved == false).Count();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProfileModel model)
        {
            // Get User
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            //Get uploaded image, if there is one.
            var ImageFile = Request.Files["ImageUrl"];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                //Save image is custom fn; Saves image to folder, returns URL to image
                user.ImageUrl = SaveImage(ImageFile);
            }

            //User can change their email. If nobody has the email (user found is null)
            // OR email found, but it is the user's, then ok.
            //var checkUserId = await UserManager.FindByEmailAsync(model.Email).Id;
            var checkUserId = await UserManager.FindByEmailAsync(model.Email);
            if (checkUserId == null || checkUserId.Id == user.Id)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                //ModelState.AddModelError("Email", "That Email is already in use.");//ok keep going
            }
            else
            {
                ModelState.AddModelError("Email", "That Email is already in use.");
            }
            //var user = UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.JobTitle = model.JobTitle;
            if (ModelState.IsValid) //Can't do this, else it will require password 
            {

                var updateResult = await UserManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Save image for User's Gravatar
        public string SaveImage(HttpPostedFileBase ImageFile)
        {

            var imageTypes = new string[] { "image/gif", "image/jpeg", "image/jpg", "image/pjpeg", "image/png" };
            if (!imageTypes.Contains(ImageFile.ContentType))
            {
                ModelState.AddModelError("ImageUrl", "Please choose either a GIF, JPG or PNG image.");
            }

            // Prepend file name with date so no duplicates..Ex "20173663439MyPic.jpg"
            var LongDate = String.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
            // Append date to fileName
            var FileName = LongDate + Path.GetFileName(ImageFile.FileName);
            // Append File Name to folder
            var path = Path.Combine(Server.MapPath("~/Images/Gravatars"), FileName);
            //Save image in folder..note path is to your computer..c:users/eric/phpprojects....
            ImageFile.SaveAs(path);
            //Save URL for image in model/db
            return FileName;
        }

        // GET: Pending Posts
        [Authorize(Roles = "admin")]
        public ActionResult PendingPosts()
        {
            var Items = db.Items.Where(c => c.Approved == false).ToList();
            return View(Items);
        }

        // POST: Approve Pending Posts by Ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ApprovePost(int itemId)
        {
            Item item = db.Items.Find(itemId);
            item.Approved = true;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return "approved";
        }

        public string DeletePost(int itemId)
        {
            Item item = db.Items.Find(itemId);
            db.Items.Remove(item);
            db.SaveChanges();
            return "Deleted";
        }



        //
        // GET: CreateRoles Page
        public ActionResult CreateRole()
        {
            return View();
        }




        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}