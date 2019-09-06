using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace aspNETfirstProject.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; } 
        
        [Required]
        public string Description { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Item")]
        [Required]
        public int ItemID { get; set; }
        public virtual Item Item { get; set; }
        
        public DateTime Created_at { get; set; }

        //public class CommentDBContext : DbContext
        //{
        //    public DbSet<Comment> Comments { get; set; }
        //}

    }
}