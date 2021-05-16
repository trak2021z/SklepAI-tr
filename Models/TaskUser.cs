using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models
{
    public class TaskUser
    {
        public int TaskUserID { get; set; }
        public int UserID { get; set; }
        public List<Task> Tasks { get; set; }
        
    }
}
