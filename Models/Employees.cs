using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    [Table("tblEmployees")]
    public class Employees
    {
        [Key]
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string EmailID { get; set; }
        public int ManagerID { get; set; }
    }

    public class EmployeeManagerViewModel: Managers
    {
        [Key]
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailID { get; set; }
    }

    public class Managers
    {
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmailID { get; set; }
    }
}