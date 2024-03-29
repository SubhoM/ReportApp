﻿using System;
using System.Web.Mvc;
using JCR.Reports.Models;
using JCR.Reports.Services;
using Microsoft.Reporting.WebForms;
using Kendo.Mvc.UI;
using JCR.Reports.Common;
using JCR.Reports.ViewModels;
using System.Configuration;
using JCR.Reports.Models.Enums;
using JCR.Reports.Areas.Tracer.ViewModels;
using JCR.Reports.Areas.Tracer.Services;
using System.Linq;

namespace JCR.Reports.Areas.Tracer.Controllers
{
    public class DepartmentComparativeAnalysisController : Controller
    {
        ExceptionService exceptionService = new ExceptionService();
        /// <summary>
        /// Load Department Comparative Analysis report default parameters
        /// </summary>
        /// <returns>View</returns>
        [SessionExpireFilter]
        public ActionResult Index(int id, int? actionType)
        {
            try
            {


                HelperClasses.SetReportOrScheduleID(id, (int)ReportsListEnum.DepartmentComparativeAnalysis);
                ViewBag.ShowCMSRadio = true;

                SearchInputService reportservice = new SearchInputService();
                if (AppSession.ReportScheduleID > 0)
                {
                    //Load the saved parameters
                    var oSaveAndScheduleService = new SaveAndScheduleService();
                    var savedParameters = oSaveAndScheduleService.LoadUserSchedule(AppSession.ReportScheduleID);
                    TempData["SavedParameters"] = savedParameters; //This tempdata will be used by the Ajax call to avoid loading the saved parameters again from DB
                    TempData["ActionType"] = actionType;

                    //Show/Hide Save to my reports button
                    ViewBag.HideSaveReport = HelperClasses.HideSaveToMyReports(AppSession.RoleID, savedParameters.UserID, AppSession.UserID, actionType);
                    

                    return View(reportservice.GetSearchListsForSavedParameters(AppSession.ReportScheduleID, savedParameters, WebConstants.TRACER_REPORT_TITLE_DEPARTMENT_COMPARATIVE_ANALYSIS));
                }
                else
                    return View(reportservice.GetSearchLists(WebConstants.TRACER_REPORT_TITLE_DEPARTMENT_COMPARATIVE_ANALYSIS));
            }
            catch (Exception ex)
            {

                ExceptionLog exceptionLog = new ExceptionLog
                {
                    ExceptionText = "Reports: " + ex.Message,
                    PageName = "DepartmentComparativeAnalysis",
                    MethodName = "Index",
                    UserID = Convert.ToInt32(AppSession.UserID),
                    SiteId = Convert.ToInt32(AppSession.SelectedSiteId),
                    TransSQL = "",
                    HttpReferrer = null
                };
                exceptionService.LogException(exceptionLog);

                return RedirectToAction("Error", "Transfer");
            }
        }

        public ActionResult _DepartmentComparativeAnalysisDataExcel([DataSourceRequest]DataSourceRequest request, Search search)
        {
            var dcaService = new DepartmentComparativeAnalysis();
            DataSourceResult result = dcaService._departmentcomparativeanalysisDataExcel(request, search);
            JsonResult jr = new JsonResult();

            jr = Json(result, JsonRequestBehavior.AllowGet);
            jr.MaxJsonLength = Int32.MaxValue;
            jr.RecursionLimit = 100;
            return jr;
            //  return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DepartmentComparativeAnalysisOppExcel([DataSourceRequest]DataSourceRequest request, Search search)
        {
            var dcaService = new DepartmentComparativeAnalysis();
            DataSourceResult result = dcaService._departmentcomparativeanalysisOppExcel(request, search);
            JsonResult jr = new JsonResult();

            jr = Json(result, JsonRequestBehavior.AllowGet);
            jr.MaxJsonLength = Int32.MaxValue;
            jr.RecursionLimit = 100;
            return jr;

            //  return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LoadDepartmentComparativeAnalysisOppIE8()
        {
            return PartialView("DepartmentComparativeAnalysisOppIE8");
        }

        public ActionResult LoadDepartmentComparativeAnalysisDataIE8()
        {
            return PartialView("DepartmentComparativeAnalysisDataIE8");
        }
        public PartialViewResult _DepartmentComparativeAnalysis(Search search, Email emailInput)
        {

     
            ReportViewer reportViewer = new ReportViewer();
          
            try
            {
                var dcaService = new DepartmentComparativeAnalysis();

                if (emailInput.To != null)
                {
                    ViewBag.FromEmail = true;
                    ViewBag.FromEmailSuccess = WebConstants.Email_Success;
                }
                //CommonService pdfService = new CommonService();

                reportViewer = dcaService._departmentcomparativeanalysisRDLC(search, emailInput);
                Session["MyReportViewer"] = reportViewer;
                //   ViewBag.ReportViewer = reportViewer;
                // ViewBag.createPdfLocation = pdfService.GetRDLCPathtoOpen(search.ReportTitle, dcaService._departmentcomparativeanalysisRDLC(search, emailInput));


            }
            catch (Exception ex)
            {

                if (ex.Message.ToString() != "Email")
                {
                    if (ex.Message.ToString() == "No Data")
                    {
                        ModelState.AddModelError("Error", WebConstants.NO_DATA_FOUND_RDLC_VIEW);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Maximum limit of " + ConfigurationManager.AppSettings["ReportOutputLimit"].ToString() + " records reached. Refine your criteria to narrow the result.");
                    }
                }
                else
                {
                    ViewBag.FromEmail = true;
                    ModelState.AddModelError("Error", WebConstants.Email_Failed);
                }

            }
            return PartialView("_ReportViewer");
        }

    }
}