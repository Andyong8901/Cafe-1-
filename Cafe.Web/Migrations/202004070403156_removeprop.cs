namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeprop : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderCarts", "ProductTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderCarts", "ProductTotal", c => c.Double(nullable: false));
        }
    }
}
