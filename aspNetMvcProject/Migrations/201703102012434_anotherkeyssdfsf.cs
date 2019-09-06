namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherkeyssdfsf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "CountryID", "dbo.Countries");
            DropIndex("dbo.Sites", new[] { "CountryID" });
            AlterColumn("dbo.Sites", "CountryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Sites", "CountryID");
            AddForeignKey("dbo.Sites", "CountryID", "dbo.Countries", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "CountryID", "dbo.Countries");
            DropIndex("dbo.Sites", new[] { "CountryID" });
            AlterColumn("dbo.Sites", "CountryID", c => c.Int());
            CreateIndex("dbo.Sites", "CountryID");
            AddForeignKey("dbo.Sites", "CountryID", "dbo.Countries", "ID");
        }
    }
}
