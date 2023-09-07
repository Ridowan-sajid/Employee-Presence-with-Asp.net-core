using Employee_Presence.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee_Presence.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
 //           modelBuilder.Entity<Employee>().HasData(
 //                                new Employee()
 //                                {
 //                                    EmployeeId = 502030,
 //                                    EmployeeName = "Mehedi Hasan",
 //                                    EmployeeCode = "EMP320",
 //                                    EmployeeSalary = 50000,
 //                                    SupervisorId = 502036,
 //                                },
 //                new Employee()
 //                {
 //                    EmployeeId = 502031,
 //                    EmployeeName = "Ashikur Rahman",
 //                    EmployeeCode = "EMP321",
 //                    EmployeeSalary = 45000,
 //                    SupervisorId = 502036,
 //                },
 //new Employee()
 //{
 //    EmployeeId = 502032,
 //    EmployeeName = "Rakibul Islam",
 //    EmployeeCode = "EMP322",
 //    EmployeeSalary = 52000,
 //    SupervisorId = 502030,
 //},
 //           new Employee()
 //           {
 //               EmployeeId = 502033,
 //               EmployeeName = "Hasan Abdullah",
 //               EmployeeCode = "EMP323",
 //               EmployeeSalary = 46000,
 //               SupervisorId = 502031,
 //           }, new Employee()
 //           {
 //               EmployeeId = 502034,
 //               EmployeeName = "Akib Khan",
 //               EmployeeCode = "EMP324",
 //               EmployeeSalary = 66000,
 //               SupervisorId = 502032,
 //           }, new Employee()
 //           {
 //               EmployeeId = 502035,
 //               EmployeeName = "Rasel Shikder",
 //               EmployeeCode = "EMP325",
 //               EmployeeSalary = 53500,
 //               SupervisorId = 502033,
 //           }, new Employee()
 //           {
 //               EmployeeId = 502036,
 //               EmployeeName = "Selim Reja",
 //               EmployeeCode = "EMP326",
 //               EmployeeSalary = 59000,
 //               SupervisorId = 502035,

 //           }
 //           );

 //           modelBuilder.Entity<EmployeeAttendance>().HasData(
 //               new EmployeeAttendance()
 //           {
 //               Id = 1,
 //               AttendanceDat = new DateTime(2023,06,24),
 //               IsAbsent = 0,
 //               IsPresent = 1,
 //               IsOffDay = 0,
 //               EmployeeId = 502030,
 //           }, new EmployeeAttendance()
 //           {
 //               Id = 2,
 //               AttendanceDat = new DateTime(2023,06,24),
 //               IsAbsent = 1,
 //               IsPresent = 0,
 //               IsOffDay = 0,
 //               EmployeeId = 502030,

 //           }, new EmployeeAttendance()
 //           {
 //               Id = 3,
 //               AttendanceDat = new DateTime(2023,06,2),
 //               IsAbsent = 0,
 //               IsPresent = 1,
 //               IsOffDay = 0,
 //               EmployeeId = 502031,
 //           });

        //}

    }
}
