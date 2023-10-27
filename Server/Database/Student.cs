
namespace ClientServerApp.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public Nullable<int> group_id { get; set; }
        public Nullable<int> learningstatus_id { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual LearningStatus LearningStatus { get; set; }
    }
}
