namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherkeysdd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "CountryID", "dbo.Countries");
            DropForeignKey("dbo.Sites", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Sites", new[] { "CustomerID" });
            DropIndex("dbo.Sites", new[] { "CountryID" });
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int());
            AlterColumn("dbo.Sites", "CountryID", c => c.Int());
            CreateIndex("dbo.Sites", "CustomerID");
            CreateIndex("dbo.Sites", "CountryID");
            AddForeignKey("dbo.Sites", "CountryID", "dbo.Countries", "ID");
            AddForeignKey("dbo.Sites", "CustomerID", "dbo.Customers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Sites", "CountryID", "dbo.Countries");
            DropIndex("dbo.Sites", new[] { "CountryID" });
            DropIndex("dbo.Sites", new[] { "CustomerID" });
            AlterColumn("dbo.Sites", "CountryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Sites", "CountryID");
            CreateIndex("dbo.Sites", "CustomerID");
            AddForeignKey("dbo.Sites", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Sites", "CountryID", "dbo.Countries", "ID", cascadeDelete: true);
        }
    }
}
