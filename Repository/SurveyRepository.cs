using SurveyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Repository
{
    public class SurveyRepository
    {
        public void CreateSurvey(Survey survey)
        {
            SurveyDBContext context = new SurveyDBContext();
            context.Surveys.Add(survey);
        }
    }
}