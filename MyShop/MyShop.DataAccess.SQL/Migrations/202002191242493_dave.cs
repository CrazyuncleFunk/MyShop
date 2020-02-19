namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dave : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "FirstName", c => c.String());
            DropColumn("dbo.Customers", "FristName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "FristName", c => c.String());
            DropColumn("dbo.Customers", "FirstName");
        }
    }
}
