namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderCarts", "UserId", "dbo.Users");
            DropIndex("dbo.OrderCarts", new[] { "UserId" });
            AddColumn("dbo.OrderCarts", "TableId", c => c.Int());
            CreateIndex("dbo.OrderCarts", "TableId");
            AddForeignKey("dbo.OrderCarts", "TableId", "dbo.Tables", "TableId");
            DropColumn("dbo.OrderCarts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderCarts", "UserId", c => c.Int());
            DropForeignKey("dbo.OrderCarts", "TableId", "dbo.Tables");
            DropIndex("dbo.OrderCarts", new[] { "TableId" });
            DropColumn("dbo.OrderCarts", "TableId");
            CreateIndex("dbo.OrderCarts", "UserId");
            AddForeignKey("dbo.OrderCarts", "UserId", "dbo.Users", "UserId");
        }
    }
}
