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
        string getTitle(string itemType);
        Task<IList<Item>> GetItems(string itemType);
        Task<IList<Comment>> GetComments(int ItemId);
        Task<JsonResult> AddComment(Comment c);
        //Item GetItemByID(int itemID);
        //void InsertItem(Item item);
        //void DeleteItem(int itemID);
        //void UpdateItem(Item item);

    }
}