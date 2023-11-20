namespace ClientServerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalMigration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "universityschool_id", c => c.Int(nullable: true));
            CreateIndex("dbo.Student", "UniversitySchool_id");
        }

        public override void Down()
        {
        }
    }
}
