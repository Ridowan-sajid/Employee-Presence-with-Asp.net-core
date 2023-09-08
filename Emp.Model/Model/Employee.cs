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
        [StringLength(20)]
        public string EmployeeName { get; set; }
        [Required]
        
        public string EmployeeCode { get; set; }
        [Required]
        [Range(0, 1000000)]
        public double EmployeeSalary { get; set; }

        public int? SupervisorId { get; set; }
        public Employee? Supervisor { get; set; }
        public ICollection<EmployeeAttendance>? EmployeeAttendances { get; set; }

    }

}
    