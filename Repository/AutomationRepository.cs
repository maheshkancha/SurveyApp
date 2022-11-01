using SurveyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Cryptography;

namespace SurveyApp.Repository
{
    public class AutomationRepository
    {
        public void SaveAutomationDetails(Automation automation)
        {
            SurveyDBContext dbContext = new SurveyDBContext();
            dbContext.Automations.Add(automation);

            dbContext.SaveChanges();
        }

        public void UpdateAutomationDetails(Automation automation)
        {
            using (SurveyDBContext dbContext = new SurveyDBContext())
            {
                Automation updatedAutomationDetails = dbContext.Automations.Where(aut => aut.AutomationID == automation.AutomationID).FirstOrDefault();
                updatedAutomationDetails.AutomationType = automation.AutomationType;
                updatedAutomationDetails.Frequency = automation.Frequency;
                updatedAutomationDetails.StartDate = automation.StartDate;
                updatedAutomationDetails.EndDate = automation.EndDate;
                updatedAutomationDetails.Subject = automation.Subject;
                updatedAutomationDetails.Description = automation.Description;
                updatedAutomationDetails.Body = automation.Body;

                dbContext.Automations.Add(updatedAutomationDetails);
                dbContext.Entry(updatedAutomationDetails).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public List<Automation> GetAutomationDetails()
        {
            return GetAutomationsList();
        }

        public Automation GetAutomationDetailsByID(int automationID)
        {
            return GetAutomationsList().FirstOrDefault(aut => aut.AutomationID == automationID);
        }

        private List<Automation> GetAutomationsList()
        {
            List<Automation> automations = new List<Automation>();
           

            using (SurveyDBContext context = new SurveyDBContext())
            {
                automations = context.Automations.ToList();
            }

            return automations;
        }

        public List<AutomationType> GetAutomationTypes()
        {
            List<AutomationType> automationType = new List<AutomationType>();

            using (SurveyDBContext context = new SurveyDBContext())
            {
                automationType = context.AutomationTypes.ToList();
            }

            return automationType;
        }
    }
}
