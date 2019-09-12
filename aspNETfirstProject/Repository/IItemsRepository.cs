using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public interface IItemsRepository
    {
        Task<Item> GetItem(int id);
        string getTitle(string itemType);
        Task<IList<Item>> GetItems(string itemType);
        Task<IList<Comment>> GetComments(int ItemId);
        Task<JsonResult> AddComment(Comment c);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Item item);
    }
}