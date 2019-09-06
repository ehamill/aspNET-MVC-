namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherkeysdf2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "Country_ID", "dbo.Countries");
            DropForeignKey("dbo.Sites", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Sites", "SiteType_ID", "dbo.SiteTypes");
            DropForeignKey("dbo.Sites", "State_ID", "dbo.States");
            DropIndex("dbo.Sites", new[] { "Country_ID" });
            DropIndex("dbo.Sites", new[] { "Customer_ID" });
            DropIndex("dbo.Sites", new[] { "SiteType_ID" });
            DropIndex("dbo.Sites", new[] { "State_ID" });
            RenameColumn(table: "dbo.Sites", name: "Country_ID", newName: "CountryID");
            RenameColumn(table: "dbo.Sites", name: "Customer_ID", newName: "CustomerID");
            RenameColumn(table: "dbo.Sites", name: "SiteType_ID", newName: "SiteTypeID");
            RenameColumn(table: "dbo.Sites", name: "State_ID", newName: "StateID");
            AlterColumn("dbo.Sites", "SiteNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Sites", "CountryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Sites", "SiteTypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Sites", "StateID", c => c.Int(nullable: false));
            CreateIndex("dbo.Sites", "CustomerID");
            CreateIndex("dbo.Sites", "SiteTypeID");
            CreateIndex("dbo.Sites", "CountryID");
            CreateIndex("dbo.Sites", "StateID");
            AddForeignKey("dbo.Sites", "CountryID", "dbo.Countries", "ID", cascadeDelete: false);
            AddForeignKey("dbo.Sites", "CustomerID", "dbo.Customers", "ID", cascadeDelete: false);
            AddForeignKey("dbo.Sites", "SiteTypeID", "dbo.SiteTypes", "ID", cascadeDelete: false);
            AddForeignKey("dbo.Sites", "StateID", "dbo.States", "ID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "StateID", "dbo.States");
            DropForeignKey("dbo.Sites", "SiteTypeID", "dbo.SiteTypes");
            DropForeignKey("dbo.Sites", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Sites", "CountryID", "dbo.Countries");
            DropIndex("dbo.Sites", new[] { "StateID" });
            DropIndex("dbo.Sites", new[] { "CountryID" });
            DropIndex("dbo.Sites", new[] { "SiteTypeID" });
            DropIndex("dbo.Sites", new[] { "CustomerID" });
            AlterColumn("dbo.Sites", "StateID", c => c.Int());
            AlterColumn("dbo.Sites", "SiteTypeID", c => c.Int());
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int());
            AlterColumn("dbo.Sites", "CountryID", c => c.Int());
            AlterColumn("dbo.Sites", "SiteNumber", c => c.String());
            RenameColumn(table: "dbo.Sites", name: "StateID", newName: "State_ID");
            RenameColumn(table: "dbo.Sites", name: "SiteTypeID", newName: "SiteType_ID");
            RenameColumn(table: "dbo.Sites", name: "CustomerID", newName: "Customer_ID");
            RenameColumn(table: "dbo.Sites", name: "CountryID", newName: "Country_ID");
            CreateIndex("dbo.Sites", "State_ID");
            CreateIndex("dbo.Sites", "SiteType_ID");
            CreateIndex("dbo.Sites", "Customer_ID");
            CreateIndex("dbo.Sites", "Country_ID");
            AddForeignKey("dbo.Sites", "State_ID", "dbo.States", "ID");
            AddForeignKey("dbo.Sites", "SiteType_ID", "dbo.SiteTypes", "ID");
            AddForeignKey("dbo.Sites", "Customer_ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Sites", "Country_ID", "dbo.Countries", "ID");
        }
    }
}
