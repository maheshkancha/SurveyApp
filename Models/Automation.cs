using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
	[Table("tblAutomation")]
	public class Automation: AutomationViewModel
	{
		[Key]
		public int AutomationID { get; set; }
		public int? AutomationType { get; set; }
		public int? Frequency { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}

	public class AutomationViewModel
	{
        public string ProcessName { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

	[Table("tblAutomationTypes")]
	public class AutomationType
	{
		[Key]
		public int AutomationTypeID { get; set; }

		[Column("AutomationType")]
		public string AutomationTypeDescription { get; set; }
	}
}
