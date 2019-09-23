using aspNETfirstProject.Models;
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
    public class CommentsRepository : ICommentsRepository
    {
        private ApplicationDbContext context;
        
        public CommentsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IList<Comment>> GetComments(int ItemID)
        {
            return await context.Comments.Where(i => i.ItemID == ItemID).ToListAsync();
        }

        public JsonResult AddComment(Comment comment)
        {
            try
            {
                comment.Created_at = DateTime.Now;
                context.Comments.Add(comment);
                context.SaveChanges();
                return new JsonResult { Data = "Ok"  };
            }
            catch (Exception ex)
            {
                return new JsonResult{ Data = "Error: " + ex.InnerException.Message };
            }
            
        }

        public JsonResult DeleteComment(int Id)
        {
            try
            {
                Comment comment = context.Comments.Find(Id);
                context.Comments.Remove(comment);
                context.SaveChanges();
                return new JsonResult { Data = "Ok" };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "Error: " + ex.InnerException.Message };
            }

        }


    }
}