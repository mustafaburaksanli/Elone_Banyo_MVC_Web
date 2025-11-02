namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SubtitleID", c => c.Int());
            CreateIndex("dbo.Products", "SubtitleID");
            AddForeignKey("dbo.Products", "SubtitleID", "dbo.CategorySubtitles", "SubtitleID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SubtitleID", "dbo.CategorySubtitles");
            DropIndex("dbo.Products", new[] { "SubtitleID" });
            DropColumn("dbo.Products", "SubtitleID");
        }
    }
}
