using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class Errand
    {
        public int ErrandID { get; set; }
        public string RefNumber { get; set; }
        public ICollection<Sample> Samples { get; set; }
        public ICollection<Picture> Pictures { get; set; }

        [Required(ErrorMessage = "Du måste fylla i plats för brottet")]
        public String Place { get; set; }

        [Required(ErrorMessage = "Du måste fylla i typ av brott")]
        public String TypeOfCrime { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateOfObservation { get; set; }
        public String Observation { get; set; }
        public String InvestigatorInfo { get; set; }
        public String InvestigatorAction { get; set; }
        [Required(ErrorMessage = "Du måste fylla i ditt namn")]
        public String InformerName { get; set; }

        [Required(ErrorMessage = "Du måste fylla i ditt telefonnummer"), RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Inte ett giltigt telefonnummer")]
        public String InformerPhone { get; set; }
        public String StatusId { get; set; }
        public String DepartmentId { get; set; }
        public String EmployeeId { get; set; }

    }
}
