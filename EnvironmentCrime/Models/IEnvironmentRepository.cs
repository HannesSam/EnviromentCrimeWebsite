using EnvironmentCrime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public interface IEnvironmentRepository
    {
        //Read
        IQueryable<Errand> Errands { get; }
        IQueryable<Department> Departments { get; }
        IQueryable<Employee> Employees { get; }
        IQueryable<ErrandStatus> ErrandStatuses { get; }
        IQueryable<Sequence> Sequence { get; }
        Task<Errand> GetErrand(int id);

        //Create and Update
        public String SaveErrand(Errand errand);
        public void AssignDepartment(Department department, int errandID);

        public void AssignEmployee(string employeeID, int errandID);
        public void CloseCase(int errandID, string reason);

        public void UpdateStatus(int errandID, string errandStatusID, string events, string information);
        public void AddSampleFile(string fileName, int errandId);
        public void AddImageFile(string fileName, int errandId);
        public List<MyErrand> CoordinatorErrandList();
        public List<MyErrand> ManagerErrandList(string department);
        public List<MyErrand> InvestigatorErrandList(string department, string userId);
    }
}
