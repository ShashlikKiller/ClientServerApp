
namespace ClientServerApp.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntitiesNew : DbContext
    {
        public dbEntitiesNew()
            : base("name=dbEntitiesNew")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LearningStatus> LearningStatuses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
    }
}
