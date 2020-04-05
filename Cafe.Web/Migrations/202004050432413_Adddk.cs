namespace Cafe.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adddk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tables");
            AddColumn("dbo.Tables", "TableStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Tables", "TableId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tables", "TableId");
            DropColumn("dbo.Tables", "TableStutus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tables", "TableStutus", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Tables");
            AlterColumn("dbo.Tables", "TableId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Tables", "TableStatus");
            AddPrimaryKey("dbo.Tables", "TableId");
        }
    }
}
