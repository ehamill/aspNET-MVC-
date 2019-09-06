namespace aspNETfirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.States", "Country_ID", "dbo.Countries");
            DropIndex("dbo.States", new[] { "Country_ID" });
            RenameColumn(table: "dbo.States", name: "Country_ID", newName: "CountryID");
            AlterColumn("dbo.States", "CountryID", c => c.Int(nullable: false));
            CreateIndex("dbo.States", "CountryID");
            AddForeignKey("dbo.States", "CountryID", "dbo.Countries", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "CountryID", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryID" });
            AlterColumn("dbo.States", "CountryID", c => c.Int());
            RenameColumn(table: "dbo.States", name: "CountryID", newName: "Country_ID");
            CreateIndex("dbo.States", "Country_ID");
            AddForeignKey("dbo.States", "Country_ID", "dbo.Countries", "ID");
        }
    }
}
