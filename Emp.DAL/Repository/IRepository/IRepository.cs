using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emp.DAL.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<EmployeeDTO> GetEmployee();
        Employee UpdateEmployee(int id, EmployeeUpdateDTO updateDTO);
        EmployeeDTO GetEmployee3rd();
        IEnumerable<EmployeeDTO> HighestSalary();
        IEnumerable<object> MonthlyAttendance();
        IEnumerable GetHierarchy(int EmployeeId);
    }
}
