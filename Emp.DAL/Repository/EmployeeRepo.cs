using AutoMapper;
using Emp.DAL.Repository.IRepository;
using Employee_Presence.Data;
using Employee_Presence.Model;
using Employee_Presence.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Emp.DAL.Repository
{
    public class EmployeeRepo<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepo(ApplicationDbContext db)
        {
            _db=db;
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

        public Employee UpdateEmployee(int id, EmployeeUpdateDTO updateDTO)
        {
            List<Employee> empList = _db.Employees.Where(e => e.EmployeeCode == updateDTO.EmployeeCode && e.EmployeeId != id).ToList();
            if (updateDTO != null && empList.Count == 0)
            {
                Employee emp = _db.Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeId == id);
                if(emp != null) 
                {
                    emp.EmployeeName = updateDTO.EmployeeName;
                    emp.EmployeeCode = updateDTO.EmployeeCode;

                    var cfg = new MapperConfiguration(c =>
                    {
                        c.CreateMap<EmployeeDTO, Employee>();
                    });
                    var mapper = new Mapper(cfg);
                    var mapped = mapper.Map<Employee>(emp);
                    _db.Employees.Update(mapped);
                    _db.SaveChanges();
                    return mapped;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
