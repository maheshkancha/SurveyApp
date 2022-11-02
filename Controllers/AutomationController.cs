using Microsoft.Ajax.Utilities;
using SurveyApp.Models;
using SurveyApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyApp.Controllers
{
    public class AutomationController : Controller
    {
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            ViewBag.ProcessNames = GetSelectListItems();
            ViewBag.AutomationTypesVB = GetAutomationTypes();

            return View();
        }

        private List<SelectListItem> GetSelectListItems()
        {
            AutomationRepository repository = new AutomationRepository();
            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (Automation aut in repository.GetAutomationDetails())
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = aut.ProcessName,
                    Value = aut.AutomationID.ToString()
                };

                itemList.Add(item);
            }

            return itemList;
        }

        private List<SelectListItem> GetAutomationTypes()
        {
            AutomationRepository repository = new AutomationRepository();
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach(AutomationType autType in repository.GetAutomationTypes())
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = autType.AutomationTypeDescription,
                    Value = autType.AutomationTypeID.ToString()
                };

                selectListItems.Add(item);
            }

            return selectListItems;
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string SelectedAutomationID, string SelectedAutomationTypeID, string Initiate)
        {
            if (SelectedAutomationID != "")
            {
                AutomationRepository repository = new AutomationRepository();
                List<SelectListItem> updatedListItem = new List<SelectListItem>();

                foreach (SelectListItem item in GetSelectListItems())
                {
                    item.Selected = item.Value == SelectedAutomationID ? true : false;

                    updatedListItem.Add(item);
                }

                ViewBag.ProcessNames = updatedListItem;
                ViewBag.AutomationTypesVB = GetAutomationTypes();

                if (Initiate == null)
                {
                    Automation automation = repository.GetAutomationDetailsByID(Convert.ToInt32(SelectedAutomationID));

                    return View(automation);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Automation automationData = new Automation();
                        automationData.AutomationID = Convert.ToInt32(SelectedAutomationID);
                        automationData.AutomationType = Convert.ToInt32(SelectedAutomationTypeID);

                        UpdateModel<Automation>(automationData);
                        repository.UpdateAutomationDetails(automationData);

                        return RedirectToAction("Create", "Automation");
                    }
                }
            } else
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post()
        {
            if (ModelState.IsValid)
            {
                AutomationViewModel automationVM = new AutomationViewModel();
                UpdateModel<AutomationViewModel>(automationVM);

                Automation automation = new Automation()
                {
                    ProcessName = automationVM.ProcessName,
                    Description = automationVM.Description,
                    Subject = automationVM.Subject,
                    Body = automationVM.Body,
                    AutomationType = null,
                    Frequency = null,
                    StartDate = null,
                    EndDate = null
                };

                AutomationRepository automationRepository = new AutomationRepository();
                automationRepository.SaveAutomationDetails(automation);

                return RedirectToAction("Index", "Automation");
            }

            return View();
        }
    
        private DateTime FormattedDate(DateTime inputDate)
        {
            int date = inputDate.Day;
            int month = inputDate.Month;
            int year = inputDate.Year;

            string formattedDate = $"{year}-{month}-{date}";

            return DateTime.Parse(formattedDate);
        }
    }
}