using System.ComponentModel.DataAnnotations;

namespace Employee_Presence.Model.Dto
{
    public class EmployeeUpdateDTO
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
