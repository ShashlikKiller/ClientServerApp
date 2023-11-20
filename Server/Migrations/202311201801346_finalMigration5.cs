namespace ClientServerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalMigration5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Student", "UniversitySchool_id", "dbo.UniversitySchools");
        }
        
        public override void Down()
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
    }
}
