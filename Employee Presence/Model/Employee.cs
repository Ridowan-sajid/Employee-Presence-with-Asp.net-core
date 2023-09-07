using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Presence.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public double EmployeeSalary { get; set; }

        public int? SupervisorId { get; set; }
        public Employee? Supervisor { get; set; }
        public ICollection<EmployeeAttendance>? EmployeeAttendances { get; set; }

    }

}
    