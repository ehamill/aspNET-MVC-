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
        string getTitle(ItemType itemType);
        Task<IList<Item>> GetItems(ItemType itemType);
        Task AddItem(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(Item item);

    }
}