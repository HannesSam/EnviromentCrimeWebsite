using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class Sample
    {
        [Key]
        public int SampleId { get; set; }
        public string SampleName { get; set; }
        public int ErrandId { get; set; }
    }
}
