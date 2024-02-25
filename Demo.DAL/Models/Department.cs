using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    // Model
    public class Department : ModelBase
    {
        // Model
        public string Code { get; set; }

        
       
        public string Name { get; set; }

        
        public DateTime DateOfCreation { get; set; }

        //[InverseProperty(nameof(Employee.Department))]
        public /*virtual*/ ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
