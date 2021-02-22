using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using KeystoneProject.Buisness_Logic.PatientReport;
using KeystoneProject.Models.PatientReport;
using System.Configuration;
namespace KeystoneProject.Controllers.PatientReport
{
    public class PatientReportController : Controller
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;
        private void Connect()
        {
            HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        }
        //
        // GET: /PatientReport/
        public ActionResult RptPatientIPDFinalBillsSummaryNew()
        {
            return View();
        }

        public ActionResult RptPatientIPDProvisionalBills()
        {
            return View();
        }

        public ActionResult RptAdmissionFormNew()
        { return View(); }

        public ActionResult RptPatientPrescriptionForOPD()
        {
            return View();

        }
        
        public ActionResult RptRefundAmount()
        {

            return View();
        }

        public ActionResult RptRefundAmount1(int PatientAccountRowID, string BillType, string RegNo, string OPDIPDNO)
        {
            Session["OtherAccountRowID"] = PatientAccountRowID;
            Session["BillTypePaymentAndDeposit"] = BillType;
            Session["RegNo1"] = RegNo;
            Session["OPDIPDNO1"] = OPDIPDNO;

            return View();
        }


        public ActionResult RptPatientLabBills()
        {
            return View();
        }
        public ActionResult RptPatientOPDBills()
        {
            return View();
        }
        public ActionResult RptPatientOtherBills()
        {
            return View();
        }
        
        public ActionResult Rpt_A4stickerprint()
        {
            return View();
        }

        #region PatientReport
        KeystoneProject.Buisness_Logic.PatientReport.BL_Report obj_ReportPatient = new Buisness_Logic.PatientReport.BL_Report();

        [HttpGet]
        public ActionResult OPDPatientReport()
        {
            Connect();
            KeystoneProject.Models.PatientReport.
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }

        public ActionResult GetAllPatientOPDNew(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.GetAllPatientOPDNew(HospitalID, LocationID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["OPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr"].ToString() + "/" + dr["DepartmentName"].ToString() + "/" + dr["RefDr"].ToString(),
                    Patient = dr["PatientName"].ToString(),
                });
            }

            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult NewIPDPatient()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }


        public ActionResult GetAllPatientIPDNew(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.GetAllPatientIPDNew(HospitalID, LocationID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr"].ToString() + "/" + dr["DepartmentName"].ToString() + "/" + dr["RefDr"].ToString(),
                    Patient = dr["Patient"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult PatientRevisit()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }

        public ActionResult GetAllPatientOPDRevisit(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.GetAllPatientOPDRevisit(HospitalID, LocationID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr"].ToString() + "/" + dr["DepartmentName"].ToString() + "/" + dr["RefDr"].ToString(),
                    Patient = dr["Patient"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult PatientReAddmission()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }
        public ActionResult GetAllPatientIPDReAdmission(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.GetAllPatientIPDReAdmission(HospitalID, LocationID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr / DepName / RefDr"].ToString(),
                    Patient = dr["Patient"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PatientOPDToIPD()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }

        public ActionResult GetAllPatientOPDToIPD(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.GetAllPatientOPDToIPD(HospitalID, LocationID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr / RefDr"].ToString(),
                    Patient = dr["Patient"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PatientAll()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }

        public ActionResult ReportPatientAll(string Department, string FromDate, string ToDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.ReportPatientAll(  Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsDr /"].ToString(),
                    Patient = dr["Patient"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion
        public ActionResult PatientIPDAdmit()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }
        public ActionResult ReportPatientIPDAdmit(string Department, string FromDate, string Consultant, string Referred)
        {
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.ReportPatientIPDAdmit(HospitalID, LocationID, Convert.ToDateTime(FromDate), Consultant, Referred, Department);
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["PatientRegNO"].ToString(),
                    OPDNo = dr["IPDNo"].ToString(),
                    Address = dr["Address"].ToString(),
                    DoctorName = dr["ConsultantDr  /  DepartmentName  /  Referred"].ToString(),
                    Patient = dr["PatientName"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PatientHistory()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);
            obj.Financiar = obj_ReportPatient.GetAllFinancialYear();
            return View(obj);
        }

        public ActionResult GetReportPatientHistory(string Financiar, string RegNo, string Patient, string PatientType)
        {
            if (RegNo == "")
            {
                RegNo = "%";
            }
            if (Financiar == "")
            {
                Financiar = "%";
            }
            if (Patient == "")
            {
                Patient = "%";
            }
            if (PatientType == "%")
            {
                PatientType = "All";
            }
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            Connect();
            ds = obj_ReportPatient.GetReportPatientHistory(HospitalID, LocationID, Financiar, RegNo, PatientType);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    RegNo = dr["RegNo."].ToString(),
                    OPDNo = dr["OPD/IPD No."].ToString(),
                    PatientType = dr["PatientType"].ToString(),
                    Address = dr["Address"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    Mobile = dr["MobileNo"].ToString(),
                    AddmissionDate = dr["AddmissionDate"].ToString(),
                    DischargeDate = dr["DischargeDate"].ToString(),
                    DoctorName = dr["ConsultantDoctor"].ToString(),
                    RefferdDoctor = dr["RefferdDoctor"].ToString(),
                    FinanceYear = dr["FinanceYear"].ToString(),
                    BillAmount = dr["BillAmount"].ToString(),
                    Discount = dr["Discount"].ToString(),
                    PaidAmount = dr["PaidAmount"].ToString(),
                    BalanceAmount = dr["BalanceAmount"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ExportPatientData()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.Financiar = obj_ReportPatient.GetAllFinancialYear();
            return View(obj);
        }


        public ActionResult VoucherReport()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);
            obj.Financiar = obj_ReportPatient.GetAllFinancialYear();
            return View(obj);
        }
        public ActionResult VoucherNameBind(string prefix)
        {
            List<Reports> add = new List<Reports>();
            Reports obj = new Reports();
            DataSet ds = new DataSet();

            ds = obj_ReportPatient.VoucherName();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    VoucherID = dr["VoucherTypeID"].ToString(),
                    VoucherName = dr["VoucherTypeName"].ToString(),

                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult RptMISVoucherWise(string FromDate, string ToDate, string Voucher)
        {
            if (Voucher == "")
            {
                Voucher = "%";
            }
            Connect();
            List<Reports> add = new List<Reports>();
            DataSet ds = new DataSet();
            ds = obj_ReportPatient.RptMISVoucherWise(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Voucher);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                add.Add(new Reports
                {
                    VoucherName = dr["VoucharName"].ToString(),
                    AcountName = dr["AccountName"].ToString(),
                    DrAmount = dr["DrAmount"].ToString(),
                    CrAmount = dr["CrAmount"].ToString(),
                    Date = dr["Date"].ToString(),
                    ChequeNo = dr["ChequeNo"].ToString(),
                    BankName = dr["BankName"].ToString(),
                    VouheEntryDetailID = dr["VoucharEntryDetailID"].ToString(),
                    Narration = dr["Narration"].ToString(),
                });
            }
            return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportMISMonthlyConsultancyCharges()
        {
            Connect();
            Reports obj = new Reports();
            DataSet dsConsultantDr = new DataSet();
            dsConsultantDr.Reset();
            obj.dscons = obj_ReportPatient.GetAllConsultantDr(HospitalID, LocationID);
            obj.dsDepartment = obj_ReportPatient.GetAllDepartment(HospitalID, LocationID);
            obj.dsReferd = obj_ReportPatient.GetAllReferredDr(HospitalID, LocationID);

            return View(obj);
        }

       }
    //public ActionResult GetReportMISMonthlyConsultancyCharges(string FromDate, string ToDate, string Consultant, string PatientType)
        //    {

        //        List<Reports> add = new List<Reports>();
        //        DataSet ds = new DataSet();
        //        ds = obj_ReportPatient.GetReportMISMonthlyConsultancyCharges(PatientType,Consultant, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            add.Add(new Reports
        //            {
        //                RegNo = dr["RegNo"].ToString(),

        //                PatientName = dr["PatientName"].ToString(),
        //                PatientType = dr["PatientType"].ToString(),
        //                DoctorName = dr["DoctorPrintName"].ToString(),
        //                Date = dr["BillDate"].ToString(),
        //                BillAmt = dr["BillAmount"].ToString(),
        //                consAmt = dr["ConsAmt"].ToString(),
        //                Discount = dr["Discount"].ToString(),
        //                PaidAmount = dr["PaidAmount"].ToString(),

        //            });
        //        }
        //        return new JsonResult { Data = add, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        // /*   }

   // }
}