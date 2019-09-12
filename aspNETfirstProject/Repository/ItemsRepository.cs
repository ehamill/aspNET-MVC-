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

        public async Task<IList<Item>> GetItems(string itemType)
        {
            ItemType item = getType(itemType);

            IList<Item> items = await context.Items
                .Where(c => c.ItemType == item)
                .Where(c => c.Approved == true)
                .OrderByDescending(c => c.Updated_at)
                .ToListAsync();
            return items;
        }

        public async void  AddItem(Item item)
        {
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }

        public async void UpdateItem(Item item)
        {
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }

        public async void DeleteItem(Item item)
        {
            context.Items.Remove(item);
            await context.SaveChangesAsync();
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
        
        public ItemType getType(string itemType) {
            switch (itemType)
            {
                case ("Devices"):
                    return ItemType.device;
                case ("Phones"):
                    return ItemType.phone;
                case ("APs"):
                    return ItemType.ap;
                case ("Switches"):
                    return ItemType.switch2;
                default:
                    return ItemType.device; ;
            }
        }

        public string getTitle(string itemType) {
            switch (itemType) {
                case ("Devices"):
                    return "Troubleshooting Devices: Thin Clients, Registers,Money Gram, Kiosks, etc.";
                case ("Phones"):
                    return "Troubleshooting Phones: Cisco, Avaya, Magix, Nortel";
                case ("APs")://"Switch Configurations"
                    return "Troubleshooting APs: Wireless Access Points";
                case ("Switches"):
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