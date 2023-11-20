using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApp.Database
{
    public partial class dbContext : DbContext
    {
        public dbContext()
            : base("name=dbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<LearningStatus>().ToTable("LearningStatus");
            modelBuilder.Entity<UniversitySchool>().ToTable("UniversitySchools");

            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<LearningStatus> LearningStatus { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<UniversitySchool> UniversitySchool { get; set; }

    }
}
