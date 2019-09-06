namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.States", "CountryID", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryID" });
            AlterColumn("dbo.States", "CountryID", c => c.Int());
            CreateIndex("dbo.States", "CountryID");
            AddForeignKey("dbo.States", "CountryID", "dbo.Countries", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "CountryID", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryID" });
            AlterColumn("dbo.States", "CountryID", c => c.Int(nullable: false));
            CreateIndex("dbo.States", "CountryID");
            AddForeignKey("dbo.States", "CountryID", "dbo.Countries", "ID", cascadeDelete: true);
        }
    }
}
