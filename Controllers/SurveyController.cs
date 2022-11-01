using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using SurveyApp.Repository;

namespace SurveyApp.Controllers
{
    public class SurveyController : Controller
    {
        Questionnaire surveyQuestions = new Questionnaire();

        // GET: Survey
        public ActionResult Index()
        {
            List<SurveyViewModel> surveys = GetSurveys();

            return View(surveys);
        }

        public ActionResult Details(int id)
        {
            SurveyViewModel survey = GetSurveys().FirstOrDefault(s => s.SurveyID == id);

            return View(survey);
        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            List<SelectListItem> list = GetChoiceTypes();

            ViewBag.ChoiceType = list;

            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post(CreateSurveyViewModel createSurveyViewModel, string AddQuestion, string CreateQuestion)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(AddQuestion))
                {
                    ViewBag.Category = createSurveyViewModel.Category;
                    ViewBag.Description = createSurveyViewModel.Description;
                    ViewBag.ChoiceType = GetChoiceTypes();

                    GenerateQuestionnaire(createSurveyViewModel.Question, createSurveyViewModel.ChoiceType, createSurveyViewModel.Choices);

                    return View("Create");
                } else if (!string.IsNullOrEmpty(CreateQuestion))
                {
                    Survey survey = new Survey();

                    survey.StatusID = 1;
                    survey.Category = createSurveyViewModel.Category;
                    survey.Description = createSurveyViewModel.Description;
                    survey.Questionnaire = JsonConvert.SerializeObject(surveyQuestions.SurveyQuestions);
                    survey.CreatedOn = DateTime.Now;
                    survey.UpdatedOn = DateTime.Now;

                    SurveyRepository repository = new SurveyRepository();
                    repository.CreateSurvey(survey);

                    return RedirectToAction("Index");
                }
            }

            return View("Create");
        }
        private List<SurveyViewModel> GetSurveys()
        {
            SurveyDBContext surveyDBContext = new SurveyDBContext();
            List<Survey> surveyList = surveyDBContext.Surveys.ToList();

            List<SurveyViewModel> surveyViewModelList = new List<SurveyViewModel>();

            foreach (var survey in surveyList)
            {
                SurveyViewModel surveyVM = new SurveyViewModel();
                surveyVM.SurveyID = survey.SurveyID;
                surveyVM.Category = survey.Category;
                surveyVM.Description = survey.Description;
                surveyVM.SurveyQnA = JsonConvert.DeserializeObject<Questionnaire>(survey.Questionnaire);
                surveyVM.Status = GetSurveyStatus(survey.StatusID);
                surveyVM.CreatedOn = survey.CreatedOn.ToShortDateString();
                surveyVM.UpdatedOn = survey.UpdatedOn.ToShortDateString();

                surveyViewModelList.Add(surveyVM);
            }

            return surveyViewModelList;
        }

        private string GetSurveyStatus(int statusID)
        {
            switch (statusID)
            {
                case 1: return "New";
                case 2: return "In Progress";
                case 3: return "Completed";
                default: return null;
            }
        }
    
        private List<SelectListItem> GetChoiceTypes()
        {
            SurveyDBContext surveyDBContext = new SurveyDBContext();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (ChoiceTypes ct in surveyDBContext.ChoiceTypes)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = ct.ChoiceTypeText,
                    Value = ct.ChoiceTypeValue,
                    Selected = ct.ChoiceTypeID == 1 ? true : false
                };

                list.Add(item);
            }

            return list;
        }

        private Questionnaire GenerateQuestionnaire(string question, string choiceType, string choices)
        {
            QuestionChoices qc = new QuestionChoices();

            List<string> choiceList = choices.Split(';').ToList();
            qc.question = question;
            qc.choiceType = choiceType;
            qc.choices = choiceList;

            surveyQuestions.SurveyQuestions = new List<QuestionChoices>();
            surveyQuestions.SurveyQuestions.Add(qc);

            return surveyQuestions;
        }
    }
}