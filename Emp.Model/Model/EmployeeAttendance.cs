using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Employee_Presence.Model
{
    public class EmployeeAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime AttendanceDat { get; set; }
        [Required]
        [Range(0, 1)]
        public int IsPresent { get; set; }
        [Required]
        [Range(0, 1)]
        public int IsAbsent { get; set; }
        [Required]
        [Range(0, 1)]
        public int IsOffDay { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

    }
}
