using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspNETfirstProject.Models  // aspNETfirstProject is just the name of the project
{
    /*    In Laravel, this would be in the migration file, 
     *    then do a php artisan migrate for laravel to create the table
          
            $table->increments('id');
            $table->enum('type', ['device', 'phone','ap','switch','news']);
            $table->string('title',40);  //max length 40
            $table->text('description');
            $table->string('imagePath',255)->nullable();
            $table->string('documentPath',255)->nullable();
            $table->boolean('approved')->default(false);
            $table->integer('user_id');
            $table->timestamps();
 
     */
    public enum ItemType
    {
        device, phone, ap, switch2     //had to use switch2 becuase switch is a keyword
    }

    public class Item
    {
        public int ID { get; set; }  //All int's and datetime are required by default
        
        public ItemType ItemType { get; set; }

        [Required(ErrorMessage = "A Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A Description is required")]
        public string Description { get; set; }

        public string ImagePath { get; set; } //nullable
        public string DocumentPath { get; set; }

        [DefaultValue(false)]
        public bool Approved { get; set; }

        [ForeignKey("ApplicationUser")] //Need using System.ComponentModel.DataAnnotations.Schema;
        public string UserID { get; set; }   //Foriegn key to user's table ...ID's in ASP.net are hashed STRINGS, not INT!!

        public virtual ApplicationUser ApplicationUser { get; set; }
        /*ApplicationUser is used in the View. To get data from the AspNetUsers table
        //In the view you use:
        //@Html.DisplayNameFor(model => model.Approved)  <-- Regular Item data
            @Html.DisplayNameFor(model => model.ApplicationUser.UserName)  <-- JOINED data
            
            @Html.DisplayFor(modelItem => item.Approved)
            @Html.DisplayFor(modelItem => item.ApplicationUser.Email)
                */

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        /*
        public class ItemDBContext : DbContext
        {
            public DbSet<Item> Items { get; set; }
        }
        */
    }
    /*
    public class ItemDBContext : DbContext   //Need "using System.Data.Entity;" at top of file
    {
        public DbSet<Item> Items { get; set; }
    }
    */
}