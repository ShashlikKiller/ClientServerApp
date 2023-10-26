using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Entities
{
    public class Group
    {
        public Group()
        {
            this.Student = new HashSet<Student>();
        }

        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<Student> Student { get; set; }

        public Group(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
