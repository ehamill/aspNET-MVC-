namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sitetypekey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SiteTypes", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.SiteTypes", new[] { "Customer_ID" });
            RenameColumn(table: "dbo.SiteTypes", name: "Customer_ID", newName: "CustomerID");
            AlterColumn("dbo.SiteTypes", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.SiteTypes", "CustomerID");
            AddForeignKey("dbo.SiteTypes", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteTypes", "CustomerID", "dbo.Customers");
            DropIndex("dbo.SiteTypes", new[] { "CustomerID" });
            AlterColumn("dbo.SiteTypes", "CustomerID", c => c.Int());
            RenameColumn(table: "dbo.SiteTypes", name: "CustomerID", newName: "Customer_ID");
            CreateIndex("dbo.SiteTypes", "Customer_ID");
            AddForeignKey("dbo.SiteTypes", "Customer_ID", "dbo.Customers", "ID");
        }
    }
}
