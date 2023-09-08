using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Employee_Presence.Model.Dto
{
    public class EmployeeAttendanceDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime AttendanceDat { get; set; }
        public int IsPresent { get; set; }
        public int IsAbsent { get; set; }
        public int IsOffDay { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
