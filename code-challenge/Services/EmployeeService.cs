using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository, ICompensationRepository compensationRepository)
        {
            _employeeRepository = employeeRepository;
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public bool Update(Employee employee)
        {
            return _employeeRepository.Update(employee);
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        /// <summary>
        /// Returns the total count of all direct and indirect reports for an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetNumberOfReports(string id)
        {
            List<Employee> results = GetFullReports(id);

            return results == null ? -1 : results.Count;
        }

        /// <summary>
        /// Returns a total sum of all reports for an employee's "tree" of reports
        /// </summary>
        /// <param name="id">Employee identifier</param>
        /// <returns></returns>
        public List<Employee> GetFullReports(string id)
        {
            //This sounds to me like a breadth-first/depth-first tree search algorithm test.
            if (String.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            //Grab the root
            Employee root = GetById(id);

            List<Employee> employees = new List<Employee>();
            List<Employee> searched = new List<Employee>();
            Queue<Employee> Q = new Queue<Employee>();

            try
            {
                //Start with root employee on the queue. Don't add it to the final list here because it will be added once traversed in the loop below
                Q.Enqueue(root);
                searched.Add(root);

                //Keep going as long as there is another employee to "visit"
                while (Q.Count > 0)
                {
                    Employee checking = Q.Dequeue();

                    //Don't want to add the root employee as a direct report
                    if (checking.EmployeeId != root.EmployeeId)
                    {
                        employees.Add(checking);
                    }

                    //Loop through the current stop on the chain and add direct reports, queuing them all to be checked as well
                    if (checking.DirectReports != null)
                    {
                        foreach (Employee e in checking.DirectReports)
                        {
                            //If its not already in the searched list, add it in and queue it to be searched for direct reports
                            if (!searched.Contains(e))
                            {
                                Q.Enqueue(e);
                                searched.Add(e);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                /*
                 * Log exception along with details about current queued employee, previously searched lists, and error messages. 
                 * This would allow potential for "replaying" the traverse when debugging prod issues
                 */
            }

            //Return all visited employees to a list
            return employees;
        }

        public Compensation GetEmployeeCompensation(string id)
        {
            Compensation comp = null;

            try
            {         
                comp = _compensationRepository.GetByID(_employeeRepository.GetById(id).CompensationID);
            }
            catch(Exception ex)
            {
                /*
                 * Log exception along with details helpful for debugging
                 */
            }

            return comp;
        }

        public Compensation CreateEmployeeCompensation(Compensation comp)
        {           
            try
            {
                _compensationRepository.Create(comp);
            }
            catch (Exception ex)
            {
                /*
                 * Log exception along with details helpful for debugging
                 */
            }

            return comp;
        }

        public bool SetEmployeeCompensation(string employeeIdentifier, string compensationIdentifier)
        {
            Employee target = GetById(employeeIdentifier);

            target.CompensationID = compensationIdentifier;

            return Update(target);           
        }

        /*
         * 
         * I added these utility methods as an initial solution before I starting using the iterative queue/list approach
         * 
            public int GetDirectReportsCount(string id)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    return 0;
                }

                try
                {
                    Employee target = GetById(id);

                    return target.DirectReports.Count();
                }
                catch(Exception ex)
                {
                    //Log exception ex to a repository
                    return 0;               
                }
            }

            public List<Employee> GetDirectReports(string id)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    return null;
                }

                try
                {
                    Employee target = GetById(id);

                    return target.DirectReports;
                }
                catch (Exception ex)
                {
                    //Log exception ex to a repository
                    return null;
                }
            }
        *
        *
        */


    }
}
