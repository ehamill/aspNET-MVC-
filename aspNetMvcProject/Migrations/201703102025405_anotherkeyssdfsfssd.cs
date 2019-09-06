namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherkeyssdfsfssd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Sites", new[] { "CustomerID" });
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Sites", "CustomerID");
            AddForeignKey("dbo.Sites", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Sites", new[] { "CustomerID" });
            AlterColumn("dbo.Sites", "CustomerID", c => c.Int());
            CreateIndex("dbo.Sites", "CustomerID");
            AddForeignKey("dbo.Sites", "CustomerID", "dbo.Customers", "ID");
        }
    }
}
