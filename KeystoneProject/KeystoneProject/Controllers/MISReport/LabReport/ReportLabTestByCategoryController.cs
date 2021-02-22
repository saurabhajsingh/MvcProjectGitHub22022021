using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.MISReport.LabReport;
using KeystoneProject.Buisness_Logic.MISReport.LabReport;

namespace KeystoneProject.Controllers.MISReport.LabReport
{
    public class ReportLabTestByCategoryController : Controller
    {
        //
        // GET: /ReportLabTestByCategory/
        public ActionResult ReportLabTestByCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportLabTestByCategory(ReportLabTestByCategory obj)
        {
            return View();
        }

        public JsonResult GetReportLabTestByCategoryDetails(DateTime FromDate, DateTime ToDate, string PatientType, int CategoryID)
        {

            var Cat="";
            var type = "";
            DataSet dsPatientBillNo = new DataSet();
            List<ReportLabTestByCategory> searchlist = new List<ReportLabTestByCategory>();

            BL_ReportLabTestByCategory objBL = new BL_ReportLabTestByCategory();
            if (PatientType == "All")
            {
                type = "%";
            }
            else
            {
                type = (PatientType).ToString();
            }
            if (CategoryID == 0)
            {
                Cat = "%";
            }
            else
            {
                Cat = Convert.ToInt32(CategoryID).ToString();
            }
            dsPatientBillNo = objBL.GetReportLabTestByCategory(FromDate, ToDate, type, Cat);
            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new ReportLabTestByCategory
                    {
                        ParentCategoryName = dr["ParentCategoryName"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        TestDate = (dr["TestDate"]).ToString(),
                        CategoryName = dr["CategoryName"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        PatientType = dr["PatientType"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Count = dr["Count"].ToString()


                    });
                }



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllCategory(string DisReason)
        {
            BL_ReportLabTestByCategory obj = new BL_ReportLabTestByCategory();
            List<ReportLabTestByCategory> searchlist = new List<ReportLabTestByCategory>();
            DataSet ds = obj.GetAllCategoryData();
                
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        searchlist.Add(new ReportLabTestByCategory
                        {
                            CategoryID = dr["CategoryID"].ToString(),
                            CategoryName = dr["CategoryName"].ToString()
                        });
                    }
                
               
                return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
        }
	}
}