using AutoMapper;
using Emp.DAL.Repository.IRepository;
using Emp.Model.Model.Dto;
using Employee_Presence.Data;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Emp.DAL.Repository
{
    public class EmployeeRepo: IRepository<Employee>
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public EmployeeRepo(ApplicationDbContext db,IConfiguration configuration)
        {
            _db=db;
            _configuration=configuration;
        }
        public IEnumerable<EmployeeDTO> GetEmployee()
        {

            IEnumerable<Employee> res = _db.Employees.ToList();
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Employee, EmployeeDTO>();
            });
            var mapper = new Mapper(cfg);
            var mapped = mapper.Map<List<EmployeeDTO>>(res);
            return mapped;
        }

        public EmployeeDTO GetEmployee3rd()
        {
            var result = _db.Employees
                            .OrderByDescending(e => e.EmployeeSalary)
                            .Skip(2)
                            .Take(1).SingleOrDefault();
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Employee, EmployeeDTO>();
            });
            var mapper = new Mapper(cfg);
            var mapped = mapper.Map<EmployeeDTO>(result);
            return mapped;

        }



        public IEnumerable<EmployeeDTO> HighestSalary()
        {
            var result = _db.Employees
                    .Where(e => e.EmployeeAttendances.Any(a => a.IsAbsent == 0)).Distinct()
                    .OrderByDescending(e => e.EmployeeSalary)
                    .ToList();
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Employee, EmployeeDTO>();
            });
            var mapper = new Mapper(cfg);
            var mapped = mapper.Map<List<EmployeeDTO>>(result);
            return mapped;
        }

        public IEnumerable<object> MonthlyAttendance()
        {
            var result = (from a in _db.Employees
                          join b in _db.EmployeeAttendances on a.EmployeeId equals b.EmployeeId
                          select new { a.EmployeeName, PayableSalary = a.EmployeeSalary, MonthName = b.AttendanceDat.ToString("MMMM"), TotalPresent = _db.EmployeeAttendances.Count(e => e.IsPresent == 1 && e.EmployeeId == b.EmployeeId), TotalOffday = _db.EmployeeAttendances.Count(e => e.IsOffDay == 1 && e.EmployeeId == b.EmployeeId), TotalAbsesnt = _db.EmployeeAttendances.Count(e => e.IsAbsent == 1 && e.EmployeeId == b.EmployeeId) }).Distinct().ToList();

            return result;
        }

        public IEnumerable GetHierarchy(int EmployeeId)
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
            return hierarchy;
        }

        public EmployeeDTO UpdateEmployee(int id, EmployeeUpdateDTO updateDTO)
        {
            List<Employee> empList = _db.Employees.Where(e => e.EmployeeCode == updateDTO.EmployeeCode && e.EmployeeId != id).ToList();
            if (updateDTO != null && empList.Count == 0)
            {
                Employee emp = _db.Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeId == id);
                if(emp != null) 
                {
                    emp.EmployeeName = updateDTO.EmployeeName;
                    emp.EmployeeCode = updateDTO.EmployeeCode;

                    _db.Employees.Update(emp);
                    _db.SaveChanges();

                    var cfg = new MapperConfiguration(c =>
                    {
                        c.CreateMap<Employee, EmployeeDTO>();
                    });
                    var mapper = new Mapper(cfg);
                    var mapped = mapper.Map<EmployeeDTO>(emp);
                    return mapped;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public bool CreateEmployee(EmployeeDTO data)
        {
            
            if (data != null)
            {
                data.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<EmployeeDTO, Employee>();
                });
                var mapper = new Mapper(cfg);
                var mapped = mapper.Map<Employee>(data);
                
                _db.Employees.Add(mapped);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public string LoginEmployee(LoginDTO data)
        {
            if (data != null)
            {
                Employee employee = _db.Employees.Where(e => e.EmployeeName.Equals(data.EmployeeName)).FirstOrDefault();
                if (employee!=null)
                {
                    if(BCrypt.Net.BCrypt.Verify(data.Password, employee.Password))
                    {
                        string token=CreateToken(employee);
                        return token;
                    }
                    return null;
                }
            }
            return null;
        }

        private string CreateToken(Employee emp)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,emp.EmployeeName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

       
    }




}
