namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOcProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderCarts", "ProductTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderCarts", "ProductTotal");
        }
    }
}
