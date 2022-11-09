using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SurveyApp.Models
{
    public class SurveyDBContext: DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<ChoiceTypes> ChoiceTypes { get; set; }
        public DbSet<Automation> Automations { get; set; }
        public DbSet<AutomationType> AutomationTypes { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeeManagerViewModel> EmployeeManager  { get; set; }
    }
}