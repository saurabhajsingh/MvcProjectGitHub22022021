using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using KeystoneProject.Models.Master;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class MISHospitalExpencesReportController : Controller
    {
        BL_MISHospitalExpencesReport bl = new BL_MISHospitalExpencesReport();
        MISHospitalExpencesReport AddServiceMod = new MISHospitalExpencesReport();
        //
        // GET: /MISHospitalExpencesReport/
        public ActionResult MISHospitalExpencesReport()
        {
            BL_MISHospitalExpencesReport bl = new BL_MISHospitalExpencesReport();
            AddServiceMod.dsServicesCharges = bl.GetHospitalExpences(Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(DateTime.Now), "%");


            return View(AddServiceMod);
        }
        public JsonResult GetServiceGrpRecord(string prefix)
        {
            BL_MISHospitalExpencesReport bl = new BL_MISHospitalExpencesReport();

            AddServiceMod.dsServicesCharges = bl.GetServiceGroupID(prefix);
            List<MISHospitalExpencesReport> SearchList = new List<MISHospitalExpencesReport>();
            foreach (DataRow dr in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            {
                SearchList.Add(new MISHospitalExpencesReport
                {
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    ServiceGroupID = dr["ServiceGroupID"].ToString()
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }


        public JsonResult GetHospitalExpences(string DateFrom, string DateTo, string GroupID)
        {
            BL_MISHospitalExpencesReport bl = new BL_MISHospitalExpencesReport();
            if(GroupID=="")
            {
                GroupID = "%";
            }
            List<MISHospitalExpencesReport> SearchList = new List<MISHospitalExpencesReport>();
            AddServiceMod.dsServicesCharges = bl.GetHospitalExpences( Convert.ToDateTime( DateFrom),  Convert.ToDateTime(DateTo), GroupID);
            var modeladd = new List<dynamic>(AddServiceMod.dsServicesCharges.Tables[0].Rows.Count);

            int a = 0;
            foreach (DataRow row in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            {
                var obj = (IDictionary<string, object>)new System.Dynamic.ExpandoObject();

                a = 0;
                  //  string RowsName = AddServiceMod.dsServicesCharges.Tables[0].Rows[row][col].ToString();
                foreach (DataColumn col in AddServiceMod.dsServicesCharges.Tables[0].Columns)
                {
                      //  string ColumnName = AddServiceMod.dsServicesCharges.Tables[0].Columns[col].ToString();

                 //   SearchList.Add(new  string[][], Array1[2][3]);
                    string per = "";
                    string TotalRs = "";
                    string Rowname = row[col].ToString();
                    if (a > 3)
                    {
                        if (row[col].ToString()=="")
                        {
                            row[col] = 0;
                        }
                        per = "(%)";
                        string aa = Convert.ToDecimal(row["TotalAmount"]).ToString();
                        string aa1 = Convert.ToDecimal(row[col]).ToString();
                        decimal TotalPer = Convert.ToDecimal(row["TotalAmount"]) / 100 * Convert.ToDecimal(row[col]);

                        Rowname = "("+row[col].ToString()+"%)" + " " + "(Rs " +Math.Round( TotalPer,2).ToString() + ")";
                    }

                    obj.Add(col.ColumnName, Rowname.ToString());
                    a++;
                }
               
                    modeladd.Add(obj);
            }

            return Json(new { modeladd = modeladd }, JsonRequestBehavior.AllowGet);
        }



	}
}