using aspNETfirstProject.Models;
using aspNETfirstProject.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Controllers
{
    public class CommentsController : Controller
    {
        private ICommentsRepository _commentsRepository;

        public CommentsController(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowComments(int ItemID)
        {
            IList<Comment> comments = await _commentsRepository.GetComments(ItemID);
            return PartialView("_CommentsPartial", comments);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult Add_Comment([Bind(Include = "Description,ItemID")] Comment comment)
        {
            comment.UserID = User.Identity.GetUserId();
            JsonResult result = new JsonResult { Data = "Invalid Model." };
            if (ModelState.IsValid)
            {
                try
                {
                    result = _commentsRepository.AddComment(comment);
                }
                catch (Exception ex)
                {
                    result = new JsonResult { Data = "Error: " + ex.InnerException };
                }
            }
            return result;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteComment(int Id)
        {
            JsonResult result = new JsonResult { Data = "" };
            try
            {
                result  = _commentsRepository.DeleteComment(Id);
                result = new JsonResult { Data = "Ok. " };
            }
            catch (Exception ex)
            {
                result = new JsonResult { Data = "Error: " + ex.InnerException };
            }
            return result;
        }
    }
}