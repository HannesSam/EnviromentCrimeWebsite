using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class ErrandStatus
    {
        [Key]
        public String StatusId { get; set; }
        public String StatusName { get; set; }

    }
}
