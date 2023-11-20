namespace ClientServerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalMigration6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UniversitySchools",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            AddForeignKey("dbo.Student", "UniversitySchool_id", "dbo.UniversitySchools", "id");
        }
        
        public override void Down()
        {
        }
    }
}
