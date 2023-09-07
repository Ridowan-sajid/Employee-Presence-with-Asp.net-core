using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Employee_Presence.Model.Dto
{
    public class EmployeeDTO
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
