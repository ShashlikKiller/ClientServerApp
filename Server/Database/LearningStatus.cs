
namespace ClientServerApp.Database
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public partial class LearningStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LearningStatus()
        {
            this.Student = new HashSet<Student>();
        }
    
        public int id { get; set; }
        public string status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Student> Student { get; set; }
    }
}
