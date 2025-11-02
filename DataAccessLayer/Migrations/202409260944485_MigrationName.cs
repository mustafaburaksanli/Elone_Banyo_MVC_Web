namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropIndex("dbo.Contacts", new[] { "UserID" });
            AlterColumn("dbo.Contacts", "UserID", c => c.Int());
            CreateIndex("dbo.Contacts", "UserID");
            AddForeignKey("dbo.Contacts", "UserID", "dbo.Users", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropIndex("dbo.Contacts", new[] { "UserID" });
            AlterColumn("dbo.Contacts", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Contacts", "UserID");
            AddForeignKey("dbo.Contacts", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
