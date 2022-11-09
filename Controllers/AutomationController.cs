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
            ViewBag.Managers = GetManagerList();
            ViewBag.DisableManagerDDL = true;
            ViewBag.DisableEmployeeDDL = true;
            ViewBag.Employees = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "" } };

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string SelectedAutomationID, string SelectedAutomationTypeID, string Initiate, string SelectedManagerID)
        {
            // RecurrencePatternData(pattern, weeklydays);
            if (SelectedAutomationID != "")
            {
                AutomationRepository repository = new AutomationRepository();
                List<SelectListItem> updatedListItem = new List<SelectListItem>();

                foreach (SelectListItem item in GetSelectListItems())
                {
                    item.Selected = item.Value == SelectedAutomationID ? true : false;

                    updatedListItem.Add(item);
                }

                ViewBag.Managers = GetManagerList();
                ViewBag.ProcessNames = updatedListItem;
                ViewBag.AutomationTypesVB = GetAutomationTypes();
                ViewBag.Employees = new List<SelectListItem>() { new SelectListItem() { Text = "", Value = "" } };
                ViewBag.DisableEmployeeDDL = true;

                if (Initiate == null)
                {
                    ViewBag.DisableManagerDDL = false;
                    Automation automation = repository.GetAutomationDetailsByID(Convert.ToInt32(SelectedAutomationID));

                    if (SelectedManagerID != "")
                    {
                        ViewBag.DisableEmployeeDDL = false;
                        List<SelectListItem> mgrList = new List<SelectListItem>();

                        foreach(SelectListItem item in GetManagerList())
                        {
                            item.Selected = item.Value == SelectedManagerID ? true : false;
                            mgrList.Add(item);
                        }

                        ViewBag.Managers = mgrList;
                        ViewBag.Employees = GetEmployeeList(Convert.ToInt32(SelectedManagerID));
                    }

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

            foreach (AutomationType autType in repository.GetAutomationTypes())
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

        private List<SelectListItem> GetManagerList()
        {
            List<SelectListItem> managerList = new List<SelectListItem>();
            AutomationRepository repo = new AutomationRepository();
            foreach(Managers manager in repo.GetManagers())
            {
                SelectListItem listItem = new SelectListItem();

                listItem.Text = $"{manager.ManagerName} ({manager.ManagerEmailID})";
                listItem.Value = manager.ManagerID.ToString();

                managerList.Add(listItem);
            }

            return managerList;
        }

        private List<SelectListItem> GetEmployeeList(int managerID)
        {
            AutomationRepository repo = new AutomationRepository();
            List<SelectListItem> listItem = new List<SelectListItem>();

            foreach (EmployeeManagerViewModel emvm in repo.GetEmployees(managerID)) {
                SelectListItem item = new SelectListItem();
                item.Text = $"{emvm.EmployeeName} ({emvm.EmployeeEmailID})";
                item.Value = emvm.EmployeeID.ToString();

                listItem.Add(item);
            }

            return listItem;
        }
        private void RecurrencePatternData(string pattern, string weeklydays)
        {
            switch (pattern)
            {
                case "daily": 
                    break;
                case "monthly":
                    break;
                case "yearly":
                    break;
                default: String selectedDays = weeklydays;
                    break;
            }
        }
    }
}