using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class MISDoctorFeesController : Controller
    {

        int HospitalID;
        int LocationID;
        int UserID;

        public void HospitalLocation()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        //
        // GET: /MISDoctorFees/

        [HttpGet]
        public ActionResult MISDoctorFees()
        {
            return View();
        }

        MISDoctorFees objUserWiseModel = new MISDoctorFees();
        BL_MISDoctorFees BL_MISDoctorFees = new BL_MISDoctorFees();

        [HttpPost]
        public ActionResult MISDoctorFees(DateTime DateFrom, DateTime DateTo, string DoctorType, string DoctorID)
        {

            if (DoctorType =="Consultant")
            {
                DoctorType = "CON";
            }
            else
            {
                DoctorType = "REF";
            }
          
           

            return new JsonResult { Data = BL_MISDoctorFees.ReportMISDoctorFees(DateFrom, DateTo, DoctorType, DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
     

        public JsonResult BindDoctor(string prefix, string Type)
        {
            List<MISDoctorFees> searchList = new List<MISDoctorFees>();
            MISDoctorFees doctor = new MISDoctorFees();

            if (Type == "Consultant")
            {
                DataSet ds = BL_MISDoctorFees.DoctorType();

                DataView dv = new DataView(ds.Tables[0], "Doctorname like '" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);

                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    searchList.Add(new MISDoctorFees
                    {

                        DoctorID = dr["DoctorID"].ToString(),
                        DoctorName = dr["Doctorname"].ToString()

                    });
                }

            }

            else
            {
                if (Type == "Referred")
                {

                    DataSet ds = BL_MISDoctorFees.DoctorType();
                    DataView dv = new DataView(ds.Tables[1], "Doctorname like'" + prefix + "%" + "'", "", DataViewRowState.CurrentRows);

                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        searchList.Add(new MISDoctorFees
                        {

                            DoctorID = dr["DoctorID"].ToString(),
                            DoctorName = dr["Doctorname"].ToString()


                        });
                    }
                }


            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult RptreportMISDoctorCommission(string Type)
        {
            Session["CommistionType"] = Type;
            return View();
        }
        public JsonResult OPDCommissionPopup1(DateTime DateFrom, DateTime DateTo, string DoctorID, string RegNO, string DoctorType)
        {
            if (DoctorType == "Consultant")
            {
                DoctorType = "CON";
            }
            else
            {
                DoctorType = "REF";
            }
            if (DoctorID == "" || DoctorID == null)
            {
                DoctorID = "%";
            }

            return new JsonResult { Data = BL_MISDoctorFees.OPDCommissionPopup1(DateFrom, DateTo, DoctorID, RegNO, DoctorType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        
	}
}