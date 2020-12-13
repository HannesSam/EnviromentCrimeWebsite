using EnvironmentCrime.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentCrime.Models
{
    public class EFEnviromentRepository : IEnvironmentRepository
    {
        private ApplicationDbContext context;

        public EFEnviromentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);
        public IQueryable<Department> Departments => context.Departments;
        public IQueryable<Employee> Employees => context.Employees;
        public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;
        public IQueryable<Picture> Pictures => context.Pictures;
        public IQueryable<Sample> Samples => context.Samples;
        public IQueryable<Sequence> Sequence => context.Sequences;

        public Task<Errand> GetErrand(int id)
        {
            return Task.Run(() =>
            {
                var errandDetail = Errands.Where(er => er.ErrandID == id).First();
                return errandDetail;
            });
        }

        public Task<Employee> GetEmployee(string id)
        {
            return Task.Run(() =>
            {
                var userDetails = Employees.Where(em => em.EmployeeId == id).First();
                return userDetails;
            });
        }

        //Save or update errand
        public string SaveErrand(Errand errand)
        {
            if (errand.ErrandID == 0)
            {
                //Create new ID
                var sequenceObject = Sequence.First();
                int sequenceValue = sequenceObject.CurrentValue;
                errand.RefNumber = "2020-45-" + sequenceValue;
                errand.StatusId = "S_D";

                //Add errand to database
                context.Errands.Add(errand);

                //Update Sequence count
                sequenceValue++;
                sequenceObject.CurrentValue = sequenceValue;
                context.Sequences.Update(sequenceObject);
            }
            else
            {
                Errand dbEntry = context.Errands.FirstOrDefault(er => er.ErrandID == errand.ErrandID);
                if (dbEntry != null)
                {
                    //code to update here
                }
            }
            context.SaveChanges();
            return errand.RefNumber;
        }

        //Add department to errand
        public void AssignDepartment(Department department, int errandID)
        {
            Errand dbEntry = context.Errands.FirstOrDefault(er => er.ErrandID == errandID);
            dbEntry.DepartmentId = department.DepartmentId;
            context.SaveChanges();
        }

        public void AssignEmployee(string employeeID, int errandID)
        {
            Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandID == errandID);
            dbEntry.EmployeeId = employeeID;
            context.SaveChanges();

        }

        public void CloseCase(int errandID, string reason)
        {
            Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandID == errandID);
            dbEntry.StatusId = "S_B";
            dbEntry.InvestigatorInfo = string.Format("{0} {1}",
            dbEntry.InvestigatorInfo, reason);
            context.SaveChanges();
        }

        public void UpdateUser(string userID, string name, string departmentID, string title)
        {
            Employee dbEntry = context.Employees.FirstOrDefault(em => em.EmployeeId == userID);
            if (name != null)
            {
                dbEntry.EmployeeName = name;
            }
            if (departmentID != null)
            {
                dbEntry.DepartmentId = departmentID;
            }
            if (title != null)
            {
                dbEntry.RoleTitle = title;
            }
            context.SaveChanges();
        }

        public void UpdateStatus(int errandID, string errandStatusID, string events, string information)
        {
            Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandID == errandID);
            dbEntry.StatusId = errandStatusID;
            if (events != null)
            {
                dbEntry.InvestigatorAction += events;
            }
            if (information != null)
            {
                dbEntry.InvestigatorInfo += information;
            }
            context.SaveChanges();
        }

        public void AddImageFile(string fileName, int errandId)
        {
            Picture picture = new Picture();
            picture.PictureName = fileName;
            picture.ErrandId = errandId;
            context.Pictures.Add(picture);
            context.SaveChanges();
        }
        public void AddSampleFile(string fileName, int errandId)
        {
            Sample sample = new Sample();
            sample.SampleName = fileName;
            sample.ErrandId = errandId;
            context.Samples.Add(sample);
            context.SaveChanges();
        }

        //Hämtar alla ärenden för handläggaren och ifall det har gjorts en sökning eller filtrering så hanteras denna här
        public List<MyErrand> CoordinatorErrandList(string searchString, string errandStatus, string department)
        {
            IEnumerable<MyErrand> errandList = GetErrandList();

            if (searchString != null)
            {
                errandList = errandList.Where(er => er.RefNumber == searchString);
            } //Om båda är satta så körs en filtrering på båda
            else if(errandStatus != null && department != null)
            {
                errandList = errandList.Where(er => er.StatusName == errandStatus && er.DepartmentName == department);
            }
            else if(errandStatus != null)
            {
                errandList = errandList.Where(er => er.StatusName == errandStatus);
            }

            return errandList.ToList();
        }

        public List<MyErrand> ManagerErrandList(string departmentId, string searchString, string errandStatus, string investigator)
        {
            IEnumerable<MyErrand> errandList = GetErrandList();
            errandList = errandList.Where(e => e.DepartmentId == departmentId);

            if (searchString != null)
            {
                errandList = errandList.Where(er => er.RefNumber == searchString);
            }
            else if (errandStatus != null && investigator != null)
            {
                errandList = errandList.Where(er => er.StatusName == errandStatus && er.EmployeeName == investigator);
            }
            else if (errandStatus != null)
            {
                errandList = errandList.Where(er => er.StatusName == errandStatus);
            }


            return errandList.ToList();
        }

        public List<MyErrand> InvestigatorErrandList(string departmentId, string userId, string searchString, string errandStatus)
        {
            IEnumerable<MyErrand> errandList = GetErrandList();
            errandList = errandList.Where(e => e.DepartmentId == departmentId && e.EmployeeId == userId);

            if (searchString != null)
            {
                errandList = errandList.Where(er => er.RefNumber == searchString);
            }
            else if (errandStatus != null)
            {
                errandList = errandList.Where(er => er.StatusName == errandStatus);
            }

            return errandList.ToList();
        }

        public IEnumerable<MyErrand> GetErrandList()
        {
            IEnumerable<MyErrand> errandList = from err in Errands
                                               join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                                               join dep in Departments on err.DepartmentId equals dep.DepartmentId
                                               into departmentErrand
                                               from deptE in departmentErrand.DefaultIfEmpty()
                                               join em in Employees on err.EmployeeId equals em.EmployeeId
                                               into employeeErrand
                                               from empE in employeeErrand.DefaultIfEmpty()
                                               orderby err.RefNumber descending
                                               select new MyErrand
                                               {
                                                   DateOfObservation = err.DateOfObservation,
                                                   ErrandId = err.ErrandID,
                                                   RefNumber = err.RefNumber,
                                                   TypeOfCrime = err.TypeOfCrime,
                                                   StatusName = stat.StatusName,
                                                   DepartmentId = err.DepartmentId,
                                                   DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : deptE.DepartmentName),
                                                   EmployeeId = err.EmployeeId,
                                                   EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                                               };
            return errandList;
        }


    }
}
