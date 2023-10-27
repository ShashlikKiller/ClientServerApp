using ClientServerApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientServerApp.BackEnd.Methods
{
    internal class DBController
    {

        public static async Task<List<Group>> GetGroups(dbEntitiesNew db)
        {
            return await Task.Run(() => db.Groups.ToList());
        }

        public static async Task<List<LearningStatus>> GetStatuses(dbEntitiesNew db)
        {
            return await Task.Run(() => db.LearningStatuses.ToList());
        }

        public static async Task<List<Student>> GetStudents(dbEntitiesNew db)
        {
            return await Task.Run(() => db.Students.ToList());
        }
        public static async Task AddStudent(Student student, dbEntitiesNew db)
        {
            if (student != null)
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
        }

        public static async Task DeleteStudent(int index, dbEntitiesNew db)
        {
            Student student = db.Students.FirstOrDefault(s => s.id == index);
            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
        }
    }
}
