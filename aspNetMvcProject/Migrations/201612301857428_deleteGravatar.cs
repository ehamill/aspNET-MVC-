namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteGravatar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ImagePath", c => c.String());
        }
    }
}
