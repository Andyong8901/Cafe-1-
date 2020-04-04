namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoriesId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        FoodImg = c.Binary(),
                        FoodName = c.String(),
                        UnitPrice = c.Double(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.CategoriesId);
            
            CreateTable(
                "dbo.OrderCarts",
                c => new
                    {
                        OrdercartId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        CategoriesId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrdercartId)
                .ForeignKey("dbo.Categories", t => t.CategoriesId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CategoriesId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Roles = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.String(nullable: false, maxLength: 128),
                        TableNo = c.String(),
                        TableStutus = c.Int(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCarts", "UserId", "dbo.Users");
            DropForeignKey("dbo.OrderCarts", "CategoriesId", "dbo.Categories");
            DropIndex("dbo.OrderCarts", new[] { "UserId" });
            DropIndex("dbo.OrderCarts", new[] { "CategoriesId" });
            DropTable("dbo.Tables");
            DropTable("dbo.Users");
            DropTable("dbo.OrderCarts");
            DropTable("dbo.Categories");
        }
    }
}
