using ClientServerApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApp.BackEnd.Methods
{
    internal class DBController
    {
        //TODO: вынести все в один универсальный метод
        // fgdfgdgdfgdfg
        public static async Task<List<Group>> GetGroups(dbEntities db) // TODO: сделать так же с остальными из базы данных
        {
            return await Task.Run(() => db.Groups.ToList());
        }

        public static async Task<List<LearningStatus>> GetStatuses(dbEntities db)
        {
            return await Task.Run(() => db.LearningStatuses.ToList());
        }

        public static async Task<List<Student>> GetStudents(dbEntities db)
        {
            return await Task.Run(() => db.Students.ToList());
        }
        public void AddStudent(Student student, dbEntities db)
        {
            db.Students.Add(student);
            db.SaveChanges();

        }
        public void DeleteStudent(int index, dbEntities db)
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
