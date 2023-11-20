using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Entities
{
    public class UniversitySchool
    {
        public UniversitySchool()
        {
            this.Student = new HashSet<Student>();
        }

        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<Student> Student { get; set; }

        public UniversitySchool(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
