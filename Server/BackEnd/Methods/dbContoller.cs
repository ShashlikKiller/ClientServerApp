using ClientServerApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientServerApp.BackEnd.Methods
{
    internal class DBController
    {

        public static async Task<List<Group>> GetGroups(dbContext db)
        {
            return await Task.Run(() => db.Group.ToList());
        }

        public static async Task<List<LearningStatus>> GetStatuses(dbContext db)
        {
            return await Task.Run(() => db.LearningStatus.ToList());
        }

        public static async Task<List<Student>> GetStudents(dbContext db)
        {
            return await Task.Run(() => db.Student.ToList());
        }
        public static async Task AddStudent(Student student, dbContext db)
        {
            if (student != null)
            {
                db.Student.Add(student);
                db.SaveChanges();
            }
        }

        public static async Task DeleteStudent(int index, dbContext db)
        {
            Student student = db.Student.FirstOrDefault(s => s.id == index);
            if (student != null)
            {
                db.Student.Remove(student);
                db.SaveChanges();
            }
        }
    }
}
