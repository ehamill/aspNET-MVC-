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
    public class ItemsRepository : IItemsRepository
    {
        private ApplicationDbContext context;
        
        public ItemsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Item> GetItem(int id)
        {
            Item item = await context.Items.FindAsync(id);
            return item;
        }

        public async Task<IList<Item>> GetItems(ItemType itemType)
        {
            IList<Item> items = await context.Items
                .Where(c => c.ItemType == itemType)
                .Where(c => c.Approved == true)
                .OrderByDescending(c => c.Updated_at)
                .ToListAsync();
            return items;
        }

        public void AddItem(Item item)
        {
            context.Items.Add(item);
             context.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            context.Items.Add(item);
             context.SaveChanges();
        }

        public void DeleteItem(Item item)
        {
            context.Items.Remove(item);
            context.SaveChanges();
        }

        public async Task<JsonResult> AddComment(Comment c)
        {
            try
            {
                context.Comments.Add(c);
                await context.SaveChangesAsync();
                return new JsonResult { Data = "Ok"  };
            }
            catch (Exception ex)
            {
                return new JsonResult{ Data = "Error: " + ex.InnerException.Message };
            }
            
        }
        
        public string getTitle(ItemType itemType) {
           
            switch (itemType) {
                case (ItemType.device):
                    return "Troubleshooting Devices: Thin Clients, Registers,Money Gram, Kiosks, etc.";
                case (ItemType.phone):
                    return "Troubleshooting Phones: Cisco, Avaya, Magix, Nortel";
                case (ItemType.ap)://"Switch Configurations"
                    return "Troubleshooting APs: Wireless Access Points";
                case (ItemType.switch2):
                    return "Switch Configurations";
                default:
                    return "Error";
            }
        }

        public async Task<IList<Comment>> GetComments(int ItemID) {
            return await context.Comments.Where(i => i.ItemID == ItemID).ToListAsync();
        }





    }
}