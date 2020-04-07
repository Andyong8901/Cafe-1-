namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFK1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tables", "UserId", c => c.Int());
            CreateIndex("dbo.Tables", "UserId");
            AddForeignKey("dbo.Tables", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "UserId", "dbo.Users");
            DropIndex("dbo.Tables", new[] { "UserId" });
            AlterColumn("dbo.Tables", "UserId", c => c.Int(nullable: false));
        }
    }
}
