namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "UserId");
        }
    }
}
