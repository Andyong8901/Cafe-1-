namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderCarts", "UserId", "dbo.Users");
            DropIndex("dbo.OrderCarts", new[] { "UserId" });
            AlterColumn("dbo.OrderCarts", "UserId", c => c.Int());
            CreateIndex("dbo.OrderCarts", "UserId");
            AddForeignKey("dbo.OrderCarts", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCarts", "UserId", "dbo.Users");
            DropIndex("dbo.OrderCarts", new[] { "UserId" });
            AlterColumn("dbo.OrderCarts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderCarts", "UserId");
            AddForeignKey("dbo.OrderCarts", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
