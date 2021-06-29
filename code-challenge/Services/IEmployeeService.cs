using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
        bool Update(Employee employee);
        int GetNumberOfReports(string id);
        List<Employee> GetFullReports(string id);
        Compensation GetEmployeeCompensation(string id);
        Compensation CreateEmployeeCompensation(Compensation comp);
        bool SetEmployeeCompensation(string employeeIdentifier, string compensationIdentifier);
        //int GetDirectReportsCount(string id);
        //List<Employee> GetDirectReports(string id);
    }
}
