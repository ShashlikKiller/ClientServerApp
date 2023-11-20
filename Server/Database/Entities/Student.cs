using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApp.Database
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int group_id { get; set; }
        public int learningstatus_id { get; set; }
        public int age { get; set; }
        public int? universityschool_id { get; set; }

        public virtual Group Group { get; set; }
        public virtual LearningStatus LearningStatus { get; set; }
        public virtual UniversitySchool UniversitySchool { get; set; }

        public Student(int id, string name, string surname, int group_id, int learningstatus_id, int age = 0, int? universityschool_id = null)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.group_id = group_id;
            this.learningstatus_id = learningstatus_id;
            this.age = age;
            this.universityschool_id = universityschool_id;
        }
        public Student()
        { }
    }
}
