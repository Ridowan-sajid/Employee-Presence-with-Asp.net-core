using Azure;
using Employee_Presence.Data;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace Employee_Presence.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployee()
        {
            return Ok(_db.Employees.ToList());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateEmployee(int id,[FromBody] EmployeeUpdateDTO updateDTO)
        {
            try
            {
                List<Employee> empList= _db.Employees.Where(e=>e.EmployeeCode==updateDTO.EmployeeCode && e.EmployeeId != id).ToList();
                //
                Console.WriteLine("ssf");
                if (updateDTO != null && empList.Count == 0)
                {
                    Employee emp = _db.Employees.AsNoTracking().FirstOrDefault(e=>e.EmployeeId==id);
                    if (emp == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        emp.EmployeeName = updateDTO.EmployeeName;
                        emp.EmployeeCode = updateDTO.EmployeeCode;
                        Employee model = new()
                        {
                            EmployeeCode = emp.EmployeeCode,
                            EmployeeId=emp.EmployeeId,
                            EmployeeName=emp.EmployeeName,
                            EmployeeSalary  =emp.EmployeeSalary,
                            SupervisorId=emp.SupervisorId,
                            
                        };


                        _db.Employees.Update(model);
                        _db.SaveChanges();
                        return Ok(emp);  
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("GetEmployeeThird")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<EmployeeDTO> GetEmployee3rd()
        {
            return Ok(_db.Employees
                .OrderBy(e => e.EmployeeSalary)
                .Skip(2)
                .Take(1));
        }

        [HttpGet("HighestSalary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> HighestSalary()
        {
            var employees = _db.Employees
                .OrderByDescending(e => e.EmployeeSalary)
                .Where(e => e.EmployeeAttendances.Any(a => a.IsPresent==1))
                .ToList();

            return Ok(employees);
        }


        [HttpGet("GetMonthlyAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> MonthlyAttendance()
        {
            var result = (from a in _db.Employees
                          join b in _db.EmployeeAttendances on a.EmployeeId equals b.EmployeeId
                          select new { a.EmployeeName,PayableSalary=a.EmployeeSalary, b.AttendanceDat, TotalPresent = _db.EmployeeAttendances.Count(e => e.IsPresent == 1 && e.EmployeeId==b.EmployeeId), TotalOffday = _db.EmployeeAttendances.Count(e => e.IsOffDay == 1 && e.EmployeeId == b.EmployeeId), TotalAbsesnt = _db.EmployeeAttendances.Count(e => e.IsAbsent == 1 && e.EmployeeId == b.EmployeeId) }).Distinct().ToList();

            return Ok(result);
        }

        [HttpGet("GetHierarchy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetHierarchy()
        {
            //var employees = _db.Employees.ToList(); // Retrieve employees from the database

            //foreach (var employee in employees)
            //{
            //    if (employee.Supervisor == null)
            //    {
            //        return BadRequest();
            //    }

            //    // Get the supervisor of the employee.
            //    var supervisor = employee.Supervisor;

            //    // Get the hierarchy of the supervisor.
            //    var hierarchy = GetHierarchy(supervisor);

            //    // Add the employee to the hierarchy.
            //    hierarchy.Add(employee);
            //}

            // Return the hierarchy.
            return Ok();
        }

    }
}
