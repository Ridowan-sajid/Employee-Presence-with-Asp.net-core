using Emp.DAL.Repository.IRepository;
using Emp.Model.Model.Dto;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;


namespace Employee_Presence.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _context;

        public EmployeeController(IRepository<Employee> context)
        {
            _context=context;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> CreateEmployee([FromForm]EmployeeDTO data)
        {
            try
            {

                var employee = _context.CreateEmployee(data);
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


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> LoginEmployee([FromForm] LoginDTO data)
        {
            try
            {
                var employee = _context.LoginEmployee(data);
                if (employee == null)
                {
                    return BadRequest("Enter correct name and password");
                }
                return Ok(employee);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        


        [HttpGet,Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployee()
        {
            try
            {

                var employee = _context.GetEmployee().ToList();
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

        [HttpPut("{id:int}"),Authorize]
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

        [HttpGet("GetEmployeeThird"),Authorize]
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

        [HttpGet("HighestSalary"),Authorize]
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


        [HttpGet("GetMonthlyAttendance"),Authorize]
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

        [HttpGet("GetHierarchy/{EmployeeId:int}"),Authorize]
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
