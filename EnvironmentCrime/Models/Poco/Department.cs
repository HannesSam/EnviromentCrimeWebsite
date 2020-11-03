using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class Department
    {
        [Key]
        public String DepartmentId { get; set; }
        public String DepartmentName { get; set; }

    }
}
