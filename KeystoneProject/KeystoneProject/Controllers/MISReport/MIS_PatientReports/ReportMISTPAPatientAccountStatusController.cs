using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using KeystoneProject.Models.MISReport.MIS_PatientReports;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class ReportMISTPAPatientAccountStatusController : Controller
    {
        BL_ReportMISTPAPatientAccountStatus obj = new BL_ReportMISTPAPatientAccountStatus();
        //
        // GET: /ReportMISTPAPatientAccountStatus/
        public ActionResult ReportMISTPAPatientAccountStatus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportMISTPAPatientAccountStatus(ReportMISTPAPatientAccountStatus obj)
        {
            return View();
        }

        public JsonResult GetReportMISPatientAccountStatus(DateTime FromDate, DateTime ToDate, string PatientType)
        {   


            DataSet dsTPA = new DataSet();
            List<ReportMISTPAPatientAccountStatus> searchlist = new List<ReportMISTPAPatientAccountStatus>();

           BL_ReportMISTPAPatientAccountStatus obj = new BL_ReportMISTPAPatientAccountStatus();
            
            //var dr = "";
            //var ser = "";
            //var PatNo = "";

           

            dsTPA = obj.ReportMISTPAPatientAccountStatus(FromDate, ToDate, PatientType);
           
            DataView dv = new DataView();
            var PaidAmt = "";
            var BillAmt = "";
            var DisAmt = "";
            var BalAmt = "";
            //dv = new DataView(dsTPA.Tables[0], " TPA/CASH = " + " TPA " + " ", "", DataViewRowState.CurrentRows);
            //foreach (DataRow dr in dv.ToTable().Rows)
            //{
                       
            //    //if (dsTPA.Tables[0].Rows[0]["TPA/CASH"]=="TPA")
            //    //{
                    
            //    //}
               
            //}

            

            if (dsTPA.Tables[0].Rows.Count > 0)
            {
                PaidAmt = Convert.ToString(dsTPA.Tables[0].Compute("Sum(PaidAmount)", String.Empty).ToString());
                BillAmt = Convert.ToString(dsTPA.Tables[0].Compute("Sum(BillAmount)", String.Empty).ToString());
                DisAmt = Convert.ToString(dsTPA.Tables[0].Compute("Sum(Discount)", String.Empty).ToString());
                  BalAmt = Convert.ToString(dsTPA.Tables[0].Compute("Sum(BalanceAmount)", String.Empty).ToString());
                    if (PatientType == "OPD")
                    {
                        foreach (DataRow dr1 in dsTPA.Tables[0].Rows)
                        {
                            searchlist.Add(new ReportMISTPAPatientAccountStatus
                            {

                                PatientReg = dr1["RegNo"].ToString(),
                                PatientName = dr1["PatientName"].ToString(),
                                PatientType = dr1["PatientType"].ToString(),
                                OPDIPDID = dr1["OPD/IPDID"].ToString(),
                                OrganizationName = dr1["OrganizationName"].ToString(),
                                //AddmissionDate = dr1["AddmissionDate"].ToString(),
                                //DischargeDate = dr1["Dischargedate"].ToString(),
                                TPA = dr1["TPA/CASH"].ToString(),
                                BillAmount = dr1["BillAmount"].ToString(),
                                //TDSAmt = dr1["TDSAmount"].ToString(),
                                //TPAOtherDeduction = dr1["TPAOtherDeduction"].ToString(),
                                DiscountAmt = dr1["Discount"].ToString(),
                                PaidAmt = dr1["PaidAmount"].ToString(),
                                BalanceAmt = dr1["BalanceAmount"].ToString(),
                                PaidAmt1 = PaidAmt,
                                BillAmt1 = BillAmt,
                                DisAmt1 = DisAmt,
                                BalAmt1 = BalAmt,




                            });
                        }
                    
                }
                else
                {
                    foreach (DataRow dr1 in dsTPA.Tables[0].Rows)
                    {
                        searchlist.Add(new ReportMISTPAPatientAccountStatus
                        {

                            PatientReg = dr1["RegNo"].ToString(),
                            PatientName = dr1["PatientName"].ToString(),
                            PatientType = dr1["PatientType"].ToString(),
                            OPDIPDID = dr1["OPD/IPDID"].ToString(),
                            OrganizationName = dr1["OrganizationName"].ToString(),
                            AddmissionDate = dr1["AddmissionDate"].ToString(),
                            DischargeDate = dr1["Dischargedate"].ToString(),
                            TPA = dr1["TPA/CASH"].ToString(),
                            BillAmount = dr1["BillAmount"].ToString(),
                            TDSAmt = dr1["TDSAmount"].ToString(),
                            TPAOtherDeduction = dr1["TPAOtherDeduction"].ToString(),
                            DiscountAmt = dr1["Discount"].ToString(),
                            PaidAmt = dr1["PaidAmount"].ToString(),
                            BalanceAmt = dr1["BalanceAmount"].ToString(),
                            PaidAmt1 = PaidAmt,
                            BillAmt1 = BillAmt,
                            DisAmt1 = DisAmt,
                            BalAmt1 = BalAmt,




                        });
                    }
                }
                

            

            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        

        }

        public JsonResult GetReportMISTPAPatientAccountStatus(DateTime FromDate, DateTime ToDate, string Patreg, string pattype, string TPAType)
        {
           


            DataSet ds = new DataSet();
            List<ReportMISTPAPatientAccountStatus> searchlist = new List<ReportMISTPAPatientAccountStatus>();

            ds = obj.ReportMISTPAPatientAccountStatusINDetail(FromDate, ToDate, Patreg, pattype, TPAType);
            // var PaidAmt = Convert.ToString(dsPatientBillNo.Tables[0].Compute("Sum(PaidAmount)", String.Empty));
            //var PaidAmt = "";
            //var SerAmt = "";
            //var SerAmt1 = "";




            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    searchlist.Add(new ReportMISTPAPatientAccountStatus
                    {

                        PatientName = dr1["PatientName"].ToString(),
                        PatientType = dr1["PatientType"].ToString(),

                        BillType = dr1["BillType"].ToString(),
                        Particular = dr1["Particular"].ToString(),

                        TPA = dr1["TPA_Name"].ToString(),
                        BillAmount = dr1["BillsAmount"].ToString(),

                        PaidAmt = dr1["PaidAmount"].ToString(),





                    });
                }




                
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public ActionResult ReportMISTPAPatientAccountStatusDetail(DateTime Date, DateTime DateTo, string PatientRegNO, string PatientType, string Type)
        //{
        //    obj.ReportMISTPAPatientAccountStatus(Date, DateTo,PatientRegNO,PatientType, Type);
        //    return View();
        //}
        public ActionResult RptReportMISTPAWisePatientAccountStatus(DateTime Date, DateTime DateTo, string RegNo, string PType, string Type, string TPAType, string a)
        {
            
            Session["a"] = a;
            if (a == "TPA")
            {
                obj.ReportMISTPAPatientAccountStatus(Date, DateTo, Type);
            }
            else
            {
                obj.ReportMISTPAPatientAccountStatusINDetail(Date, DateTo, RegNo, Type, TPAType);
            }
            
            
            //Session["Date"] = Date;
            return View();
        }
	}
}