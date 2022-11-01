using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    [Table("tblSurvey")]
    public class Survey
    {
        public int SurveyID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Questionnaire { get; set; }
        public int StatusID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    [Table("tblChoiceTypes")]
    public class ChoiceTypes
    {
        [Key]
        public int ChoiceTypeID { get; set; }
        public string ChoiceTypeValue { get; set; }
        public string ChoiceTypeText { get; set; }

    }

    public class Questionnaire
    {
        public List<QuestionChoices> SurveyQuestions { get; set; }
    }

    public class QuestionChoices
    {
        public string question { get; set; }
        public string choiceType { get; set; }
        public List<string> choices { get; set; }
    }

    public class SurveyViewModel
    {
        public int SurveyID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Questionnaire SurveyQnA { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class CreateSurveyViewModel
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string ChoiceType { get; set; }
        public string Choices { get; set; }
    }
}
