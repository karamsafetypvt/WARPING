using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class DashboardModel
    {
        public int? ID { get; set; } 
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int AllRequest { get; set; } 
        public int Completed { get; set; } 
        public int Closed { get; set; } 
        public string? CreatedBy { get; set; } 
    }
}
