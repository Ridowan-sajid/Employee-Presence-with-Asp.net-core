using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emp.Model.Model.Dto
{
    public class LoginDTO
    {
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
