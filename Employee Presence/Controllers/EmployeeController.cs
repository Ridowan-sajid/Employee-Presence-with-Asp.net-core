using Emp.DAL.Repository.IRepository;
using Employee_Presence.Data;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;
using System.Net;

namespace Employee_Presence.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IRepository<Employee> _context;

        public EmployeeController(ApplicationDbContext db, IRepository<Employee> context)
        {
            _db = db;
            _context=context;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployee()
        {
            return _context.GetEmployee().ToList();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateEmployee(int id,[FromBody] EmployeeUpdateDTO updateDTO)
        {
            try
            {
               
                var employee = _context.UpdateEmployee(id,updateDTO);
                if (employee == null)
                {
                    return BadRequest();
                }
                return Ok(employee);
                
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
                var result= _context.GetEmployee3rd();

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
                var employees = _context.HighestSalary();


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
                var result=_context.MonthlyAttendance();
                
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
        public ActionResult<IEnumerable> GetHierarchy(int EmployeeId)
        {

            try
            {
                var hierarchy = _context.GetHierarchy(EmployeeId);

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
