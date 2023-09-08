using Azure;
using Employee_Presence.Data;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EmployeeDTO> GetEmployee3rd()
        {
            try
            {
                var result =_db.Employees
                .OrderByDescending(e => e.EmployeeSalary)
                .Skip(2)
                .Take(1);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpGet("HighestSalary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<EmployeeDTO>> HighestSalary()
        {

            try
            {
                var employees = _db.Employees
                .Where(e => e.EmployeeAttendances.Any(a => a.IsAbsent == 0)).Distinct()
                .OrderByDescending(e => e.EmployeeSalary)
                .ToList();

                if (employees == null)
                {
                    return BadRequest();
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }


        [HttpGet("GetMonthlyAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<EmployeeDTO>> MonthlyAttendance()
        {
           try {
                var result = (from a in _db.Employees
                              join b in _db.EmployeeAttendances on a.EmployeeId equals b.EmployeeId
                              select new { a.EmployeeName, PayableSalary = a.EmployeeSalary, MonthName = b.AttendanceDat.ToString("MMMM"), TotalPresent = _db.EmployeeAttendances.Count(e => e.IsPresent == 1 && e.EmployeeId == b.EmployeeId), TotalOffday = _db.EmployeeAttendances.Count(e => e.IsOffDay == 1 && e.EmployeeId == b.EmployeeId), TotalAbsesnt = _db.EmployeeAttendances.Count(e => e.IsAbsent == 1 && e.EmployeeId == b.EmployeeId) }).Distinct().ToList();
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message); 
            }    
            
        }

        [HttpGet("GetHierarchy/{EmployeeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetHierarchy(int EmployeeId)
        {

            try
            {

                List<string> hierarchy = new List<string>();
                List<int> ids = new List<int>();

                var employees = _db.Employees.Where(e => e.EmployeeId == EmployeeId).FirstOrDefault();
                hierarchy.Add(employees.EmployeeName);
                ids.Add(employees.EmployeeId);
                var sup = employees.SupervisorId;

                while (sup != null)
                {
                  
                    if (sup == employees.EmployeeId) break;
                    var emp2 = _db.Employees.Where(e => e.EmployeeId == sup).FirstOrDefault();
                    hierarchy.Add(emp2.EmployeeName);
                    ids.Add(emp2.EmployeeId);
                    sup = (int)emp2.SupervisorId;
                }
                                
                if (hierarchy == null)
                {
                    return BadRequest();
                }
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
