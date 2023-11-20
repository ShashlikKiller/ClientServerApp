namespace ClientServerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "age", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
