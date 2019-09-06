namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteNumber = c.String(),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Zip = c.Int(nullable: false),
                        Country_ID = c.Int(),
                        Customer_ID = c.Int(),
                        SiteType_ID = c.Int(),
                        State_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Countries", t => t.Country_ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .ForeignKey("dbo.SiteTypes", t => t.SiteType_ID)
                .ForeignKey("dbo.States", t => t.State_ID)
                .Index(t => t.Country_ID)
                .Index(t => t.Customer_ID)
                .Index(t => t.SiteType_ID)
                .Index(t => t.State_ID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        Abbreviation = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        BillingAddress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SiteTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Customer_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .Index(t => t.Customer_ID);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        Abbreviation = c.String(nullable: false, maxLength: 2),
                        Country_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Countries", t => t.Country_ID)
                .Index(t => t.Country_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "State_ID", "dbo.States");
            DropForeignKey("dbo.States", "Country_ID", "dbo.Countries");
            DropForeignKey("dbo.Sites", "SiteType_ID", "dbo.SiteTypes");
            DropForeignKey("dbo.SiteTypes", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Sites", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Sites", "Country_ID", "dbo.Countries");
            DropIndex("dbo.States", new[] { "Country_ID" });
            DropIndex("dbo.SiteTypes", new[] { "Customer_ID" });
            DropIndex("dbo.Sites", new[] { "State_ID" });
            DropIndex("dbo.Sites", new[] { "SiteType_ID" });
            DropIndex("dbo.Sites", new[] { "Customer_ID" });
            DropIndex("dbo.Sites", new[] { "Country_ID" });
            DropTable("dbo.States");
            DropTable("dbo.SiteTypes");
            DropTable("dbo.Customers");
            DropTable("dbo.Countries");
            DropTable("dbo.Sites");
        }
    }
}
