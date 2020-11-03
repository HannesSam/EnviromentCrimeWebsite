using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class Employee
    {
        [Key]
        public String EmployeeId { get; set; }
        public String EmployeeName { get; set; }
        public String RoleTitle { get; set; }
        public String DepartmentId { get; set; }
    }
}
