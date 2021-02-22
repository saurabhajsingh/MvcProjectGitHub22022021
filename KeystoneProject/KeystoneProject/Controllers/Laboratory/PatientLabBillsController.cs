using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Laboratory;
using KeystoneProject.Buisness_Logic.Laboratory;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;

namespace KeystoneProject.Controllers.Laboratory
{
    public class PatientLabBillsController : Controller
    {
        //
        // GET: /PatientLabBills/
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
        }

        List<PatientLabBills> PatientLabBillsList = new List<PatientLabBills>();
        BL_PatientLabBills _PatientLabBills = new BL_PatientLabBills();
        PatientLabBills PatientLabBill = new PatientLabBills();
        public ActionResult PatientLabBills()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientLabBills(FormCollection collection,PatientLabBills opt)
        {
            
            PatientLabBills location = new PatientLabBills();
           
            Connect();

            opt.Discount_Service = collection["Discount_Service"].ToString();
            opt.DiscountServiceType = collection["DiscountServiceType"].ToString();
            opt.ServiceType = collection["ServiceType"].ToString();
            opt.ReportingDate = collection["ReportDate"].ToString();
            //opt.ProfileID=collection[""].ToString();
            opt.LabName = collection["out1"].ToString();
           opt.OutSourceID =collection["outid1"].ToString();
           if (collection["CompanyName"] == null || collection["CompanyName"] == "")
            {
                opt.OrganizationName = "";
              
            }
            else
            {
                opt.OrganizationName = collection["CompanyName"].ToString();
            }
           if (collection["CompanyID"] == null || collection["CompanyID"]=="")
           {
               opt.OrganizationID = 0;
           }
           else
           {
               opt.OrganizationID = Convert.ToInt32(collection["CompanyID"].ToString());
              
           }
          
            if (Request.Form["samplecollection1"] != null )
            {
               
                opt.samplecollection = Request.Form["samplecollection1"];

            }
            else
            {
                opt.samplecollection = Request.Form["samplecollection1"];
              

            }
            if (Request.Form["CollectionCentre"] == null || Request.Form["CollectionCentre"] == "")
            {
                opt.CollectionCentreName = "";
                
            }
            else
            {
                opt.CollectionCentreName = Request.Form["CollectionCentre"];
            }
           

            if (Request.Form["TesttableID"] != null)
            {
                opt.TestID = Request.Form["TesttableID"];
            }
            else
            {
                opt.TestID = "";
            }

            if (Request.Form["DateOFBirth"] != null)
            {
               
                opt.DateOfBirth = Request.Form["DateOFBirth"];
             
            }
            else
            {
                opt.DateOfBirth = System.DateTime.Now.ToString("dd-mm-yyyy");
            }
            TimeSpan tim = DateTime.Now - Convert.ToDateTime( opt.DateOfBirth);
            opt.TotalDay = tim.TotalDays.ToString();

            if (Request.Form["OldBillNo"] != null)
            {
                opt.BillNo = Request.Form["OldBillNo"];

            }
            else
            {
                opt.BillNo = "0";

            }
            if (opt.SampleCollectionBoyID=="" || opt.SampleCollectionBoyID==null)
            {
                opt.SampleCollectionBoyID = "0";
            }
            else
            {
                opt.SampleCollectionBoyID = Request.Form["SampleCollectionID"];
            }
          
            opt.PrefixName = Request.Form["PFPatientName"];
            if (Request.Form["PatientRegNo"] != null)
            {
                opt.PatientRegNo = Request.Form["PatientRegNo"];
            }
            else
            {
                opt.PatientRegNo = "";

            }
            if (Request.Form["PatientName"] != null)
            {
                opt.PatientName = Request.Form["PatientName"];
            }
            else
            {
                opt.PatientName = "";
            }
            if (Request.Form["GuardianName"] != null)
            {
                opt.GuardianName = Request.Form["GuardianName"];

            }
            else
            {
                opt.GuardianName = "";

            }

            if ((Request.Form["OPDIPDNo"] != null) || (Request.Form["OPDIPDNo"] != ""))
            {
                opt.OPDIPDID = Request.Form["OPDIPDNo"];

            }
            else
            {
                opt.OPDIPDID = "";

            }

            if (Request.Form["PatientType"] != null)
            {
                opt.PatientType = Request.Form["PatientType"];
            }
            else
            {
                opt.PatientType = "OPD";
            }

            if (Request.Form["Year"] != null)
            {
                opt.Age = Request.Form["Year"];
            }
            else
            {
                opt.Age = "";
            }


            if (Request.Form["BillDate"] != null)
            {
                opt.BillDate = Request.Form["BillDate"] +" "+ System.DateTime.Now.ToString("hh:mm:ss");
            }
            else
            {
                opt.BillDate = System.DateTime.Now.ToString()+" "+ System.DateTime.Now.ToString("hh:mm:ss");
            }
          
            if (Request.Form["Gender"] != null)
            {
                opt.Gender = Request.Form["Gender"];
            }
            else
            {
                opt.Gender = "";
            }

            if (Request.Form["MobileNo"] != null)
            {
                opt.MobileNo = Request.Form["MobileNo"];
            }
            else
            {
                opt.MobileNo = "";
            }

            if (Request.Form["Address"] != null)
            {
                opt.Address = Request.Form["Address"];
            }
            else
            {
                opt.Address = "";
            }

            if (Request.Form["Remark"]!=null)
            {
                opt.Remarks = Request.Form["Remark"];
            }
            else
            {
                opt.Remarks = "";
            }

            if (Request.Form["RoomID"] != "")
            {
                opt.RoomID = Request.Form["RoomID"];
            }
            else
            {
                opt.RoomID = "";

            }

            if (Request.Form["WardID"] != "")
            {
                opt.WardID = Request.Form["WardID"];
            }
            else
            {
                opt.WardID = "";
            }

            if (Request.Form["BedID"] != "")
            {
                opt.BedID = Request.Form["BedID"];
            }
            else
            {
                opt.BedID = "";
            }

          

            if (Request.Form["rate1"] != null)
            {
                opt.Rate = Request.Form["rate1"];
            }
            else
            {
                opt.Rate = "0.00";
            }

            if (Request.Form["Qty"] != null)
            {
                opt.Qty = Request.Form["Qty"];
            }
            else
            {
                opt.Qty = "1";
            }

            if (Request.Form["RefferedDoctorID"] != "")
            {
                opt.ReferredByDoctorID = Request.Form["RefferedDoctorID"];
            }
            else
            {
                opt.ReferredByDoctorID = "";
            }

            if (Request.Form["ConsultantDoctorID1"] != null)
            {
                opt.ConsultantDrID = Request.Form["ConsultantDoctorID1"];
            }
            else
            {
                opt.ConsultantDrID = "";
            }


            if (Request.Form["GrossTotal"] != null)
            {
                opt.GrossAmount = Convert.ToDecimal(Request.Form["GrossTotal"]);
            }
            else
            {
                opt.GrossAmount = 0;
            }

            if (Request.Form["TaxAmt"] != null)
            {
                opt.TaxAmount = Convert.ToDecimal(Request.Form["TaxAmt"]);
            }
            else
            {
                opt.TaxAmount = Convert.ToDecimal(0.00);

            }
            if (Request.Form["priBalanceDeposit"] == "Pre.Balance")
            {
                if (Request.Form["PreBalanceAmount"] != null)
                {
                    opt.PreBalanceAmount = Convert.ToDecimal(Request.Form["PreBalanceAmount"]);

                }
                else
                {
                    opt.PreBalanceAmount = Convert.ToDecimal(0.00);

                }
            }
            else
            {
                if (Request.Form["PreBalanceAmount"] != null)
                {
                    opt.Deposit = Convert.ToDecimal(Request.Form["PreBalanceAmount"]);

                }
                else
                {
                    opt.Deposit = Convert.ToDecimal(0.00);

                }
            }
            if (Request.Form["PreBalanceAmount"] != null)
            {
                opt.PreBalanceAmount = Convert.ToDecimal(Request.Form["PreBalanceAmount"]);

            }
            else
            {
                opt.PreBalanceAmount = Convert.ToDecimal(0.00);

            }
            if (Request.Form["TotalAmt"] != null)
            {
                opt.TotalAmount = Convert.ToDecimal(Request.Form["TotalAmt"]);

            }
            else
            {
                opt.TotalAmount = Convert.ToDecimal(0.00);

            }
            if (Request.Form["Discount"] != null)
            {
                opt.Discount = Request.Form["Discount"];
            }
            else
            {
                opt.Discount = "";

            }

          
            if (Request.Form["DiscReason"] != null)
            {
                opt.DiscountReason = Request.Form["DiscReason"];
            }
            else
            {
                opt.DiscountReason = "";

            }

            if(Request.Form["DiscountAmount"]!=null)
            {
                opt.DiscountAmount=Request.Form["DiscountAmount"];
            }
            else
            {
                opt.DiscountAmount = "";
            }

            if (Request.Form["AmtPaid"] != null && Request.Form["AmtPaid"].ToString() != "")
            {
             string a=   Request.Form["AmtPaid"].ToString();
                opt.PaidAmount = Convert.ToDecimal(Request.Form["AmtPaid"]);
            }
            else
            {
                opt.PaidAmount = Convert.ToDecimal(0.00);

            }
            if (Request.Form["BalAmt"] != null)
            {
                opt.BalanceAmount = Convert.ToDecimal(Request.Form["BalAmt"]);
            }
            else
            {
                opt.BalanceAmount = Convert.ToDecimal(0.00);

            }
            if (Request.Form["PaymentType"] != null)
            {
                if (opt.PaymentType == "Cash")
                {
                    opt.PaymentType = Request.Form["PaymentType"];
                }
                else
                {
                    opt.PaymentType = Request.Form["PaymentType"];
                    opt.Number = Request.Form["Number"].ToString();
                    opt.Name = Request.Form["Name"].ToString();
                   opt.Remark = Request.Form["Remarks"].ToString();
                   opt.PaymentDate = Convert.ToDateTime(Request.Form["paymentDate"]).ToString();
                }

            }
            else
            {
                opt.PaymentType = "";

            }
            if (Request.Form["TestNamerow"] != null)
            {
                opt.TestName = Request.Form["TestNamerow"];
            }
            else
            {
                opt.TestName = "";

            }

            if (Request.Form["PatientType1"] != null)
            {
                opt.PatientType = Request.Form["PatientType1"];
            }
            else
            {
                opt.PatientType = "";

            }

            if(Request.Form["ServiceTax"]!=null)
            {
                opt.ServiceTax = Convert.ToDecimal(Request.Form["ServiceTax"]);
            }
            else
            {
                opt.ServiceTax = Convert.ToDecimal(0.00);
            }

            if (Request.Form["mode"] != null)
            {
                opt.Mode = Request.Form["mode"];
            }
            else
            {
                opt.Mode = "";

            }
            KeystoneProject.Buisness_Logic.Patient.BL_PatientOPDBill objFin = new KeystoneProject.Buisness_Logic.Patient.BL_PatientOPDBill();
            if (Request.Form["FinancialYearID"] != null)
            {
                opt.FinancialYearID = Convert.ToInt32(objFin.GetAllFinancialYear().Tables[1].Rows[0]["FinancialYearID"]);

            }

            BL_PatientLabBills objTest = new BL_PatientLabBills();


            if (opt.BillNo == "--New--")
            {
                PatientLabBills obj2 = objTest.Save(opt);
                return RedirectToAction("RptPatientLabBills", "PatientReport");

                if(obj2.mybool)
                {
                 
                    ModelState.Clear();
                    ViewData["flag"] = "Done";
                    //Session["LabBillNo"] = obj2.BillNo;
                    //Session["LabBillType"] = obj2.BillType;
                   
                 
                }
            }
            else
            {
              
                if (objTest.Edit(opt))
                {
                      return RedirectToAction("RptPatientLabBills", "PatientReport");
                }
            }

            return RedirectToAction("PatientLabBills");
        }

        public JsonResult OldBillReport(string BillNo,string BillType)
        {
            HospitlLocationID();

            Session["LabBillNo"] = BillNo.ToString();
            Session["LabBillType"] = BillType.ToString();


            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Fill(string prefix)
        {
            BL_PatientLabBills objPatient = new BL_PatientLabBills();
            List<PatientLabBills> searchList = new List<PatientLabBills>();
            DataSet ds = objPatient.Fill(prefix);
            
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    searchList.Add(new PatientLabBills

                        {
                            PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                            PatientLabName = dr["PatientName"].ToString(),
                        });
                   //PatientLabBills cat=new PatientLabBills();
                   //cat.PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString();
                   //     cat.PatientLabName = dr["PatientName"].ToString();
                   // searchList.Add(cat);
                }


                return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult AppointDetails(string appID)
        {
            BL_PatientLabBills objPatient = new BL_PatientLabBills();
            List<PatientLabBills> searchList = new List<PatientLabBills>();
            List<PatientLabBills> GetsearchList = new List<PatientLabBills>();
            DataSet ds1=objPatient.GetPatientLabAppointment(appID);
            DataSet ds = objPatient.GetPatientLabAppointmentDetails(appID);
            foreach(DataRow dr in ds1.Tables[0].Rows)
            {
                searchList.Add(new PatientLabBills
                {
                    LocationId = Convert.ToInt32(dr["HospitalID"]),
                    HospitalId = Convert.ToInt32(dr["LocationID"]),
                    PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                    PrefixName = dr["PatientPrefixName"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    GuardianName = dr["GuardianName"].ToString(),
                    Years = dr["Age"].ToString(),
                    AgeType = dr["AgeType"].ToString(),
                    DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                    Gender = dr["Gender"].ToString(),
                    Address = dr["Address"].ToString(),
                    ConsultantDr = dr["ConsultantDr"].ToString(),
                    ConsultantDrID = dr["ConsultantDrID"].ToString(),
                    RefferedDr = dr["RefferredDr"].ToString(),
                    ReferredByDoctorID = dr["RefferredDrID"].ToString(),
                    MobileNo = dr["MobileNo"].ToString(),
                    SampleCollectionBy = dr["SampleCollectedBy"].ToString(),
                    AppointmentDate = Convert.ToDateTime(dr["PatientLabAppointmentDate"]).ToString("yyyy-MM-dd"),
                    CreationID = dr["CreationID"].ToString(),
                    Mode = dr["Mode"].ToString(),

                });

            }
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                GetsearchList.Add(new PatientLabBills
                {
                    HospitalId = Convert.ToInt32(dr["HospitalID"]),
                    LocationId = Convert.ToInt32(dr["LocationID"]),
                    PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                    PatientLabAppointmentDetailID = dr["PatientLabAppointmentDetailID"].ToString(),
                    TestID = dr["TestID"].ToString(),
                    TestName = dr["TestName"].ToString(),
                    Rate = dr["Rate"].ToString(),
                    Qty = dr["Quantity"].ToString(),
                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                    ServiceType = dr["TestType"].ToString(),
                    OrganizationName = dr["TPAWiseCollect"].ToString(),
                    CreationID = dr["CreationID"].ToString(),
                    Mode = dr["Mode"].ToString(),
                });
            }

            return Json(new { searchList = searchList, GetsearchList = GetsearchList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPatientName(string prefix)
            {
            BL_PatientLabBills objPatient = new BL_PatientLabBills();
            List<PatientLabBills> searchList = new List<PatientLabBills>();
            DataSet ds1 = objPatient.GetPatient(prefix);
         
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                   PatientLabBills Patient = new PatientLabBills();

                    Patient.PatientRegNo = dr["PatientRegNo"].ToString();

                    Patient.PatientName = dr["PatientName"].ToString();

                    searchList.Add(Patient);
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetPatientLabOPDIPDNo(int OPDIPDID)
            {
            BL_PatientLabBills objPatient = new BL_PatientLabBills();
            List<PatientLabBills> searchList = new List<PatientLabBills>();
            DataSet ds1 = objPatient.GetPatientLabOPDIPDNo(OPDIPDID);

            {
                
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    PatientLabBills Patient = new PatientLabBills();
                   
                    Patient.OPDIPDID = dr["OPDIPDNo"].ToString();
                    Patient.PatientRegNo = dr["PatientRegNo"].ToString();
                    Patient.PrintRegNO = dr["PrintRegNO"].ToString();
                    Patient.PatientName = dr["PatientName"].ToString();
                    Patient.PatientType = dr["PatientType"].ToString();

                    searchList.Add(Patient);
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindPatientPrefix(string prefix)
        {
            BL_PatientLabBills objPatient = new BL_PatientLabBills();
            return new JsonResult { Data = objPatient.BindPrefixPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetConsDoctor(string prefix)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetConsDoctor(prefix);
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills cat1 = new PatientLabBills();

                cat1.DoctorID = Convert.ToInt32(dr["DoctorID"].ToString());

                cat1.DoctorPrintName = dr["DoctorPrintName"].ToString();

                searchlist.Add(cat1);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetRefferedDoctor(string prefix)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetRefferedDoctor(prefix);
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills catl = new PatientLabBills();

                catl.DoctorID = Convert.ToInt32(dr["DoctorID"].ToString());

                catl.DoctorPrintName = dr["DoctorPrintName"].ToString();

                searchlist.Add(catl);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetOutSourceLabTestRate(int OutSourceLabID, int TestID)
                        
                                {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetOutSourceLabTestRate(OutSourceLabID);

            DataView dvTest = new DataView(ds.Tables[0], "TestID = " + TestID + " ", "", DataViewRowState.CurrentRows);

            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                PatientLabBills catl = new PatientLabBills();

                catl.OutSourceLabID = Convert.ToInt32(dr["OutSourceLabID"].ToString());

                catl.LabName = dr["LabName"].ToString();
                catl.TestID = dr["TestID"].ToString();
                catl.TestName = dr["TestName"].ToString();
                catl.Rate = dr["Rate"].ToString();

                searchlist.Add(catl);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetOutsource(string prefix)
       {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetOutsource(prefix);
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills catl = new PatientLabBills();

                catl.OutSourceLabID = Convert.ToInt32(dr["OutSourceLabID"].ToString());

                catl.LabName = dr["LabName"].ToString();

                searchlist.Add(catl);
            }

           
          
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetUser(string prefix)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetUser(prefix);

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills cat = new PatientLabBills();

                cat.UserID = dr["UserID"].ToString();

                cat.FullName = dr["FullName"].ToString();
             

                searchlist.Add(cat);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }     
        public ActionResult CompanyNameBind(string prefix)
        {
            BL_PatientLabBills CompanyName = new BL_PatientLabBills();
            List<PatientLabBills> searchListCompanyName = new List<PatientLabBills>();
            DataSet ds = CompanyName.CompanyNameBind(prefix);
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills name = new PatientLabBills();
                name.OrganizationID = Convert.ToInt32(dr["OrganizationID"].ToString());
                name.OrganizationName = dr["OrganizationName"].ToString();

                searchListCompanyName.Add(name);
            }
            return new JsonResult { Data = searchListCompanyName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult CollectionCentre(string prefix)
        {
            BL_PatientLabBills CollectionCentre = new BL_PatientLabBills();
            List<PatientLabBills> searchListCollectionCentre = new List<PatientLabBills>();
            DataSet ds = CollectionCentre.CollectionCentreBind(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills name = new PatientLabBills();
                name.CollectionCentreID = Convert.ToInt32(dr["CollectionID"].ToString());
                name.CollectionCentreName = dr["CollectionName"].ToString();

                searchListCollectionCentre.Add(name);
            }
            return new JsonResult { Data = searchListCollectionCentre, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetDiscountReason(string prefix)
        {
            BL_PatientLabBills obDiscount = new BL_PatientLabBills();
            List<PatientLabBills> searchListDiscount = new List<PatientLabBills>();
            DataSet ds = obDiscount.GetDiscountReason("%");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills discount = new PatientLabBills();
                discount.DiscountReasonID = Convert.ToInt32(dr["DiscountReasonID"].ToString());
                discount.DiscountReason = dr["DiscountReason"].ToString();

                searchListDiscount.Add(discount);
            }
            return new JsonResult { Data = searchListDiscount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult NXtRegNO()
        {

            PatientLabBills catl = new PatientLabBills();

            string regNO = "";
            BL_PatientLabBills Nxtreg = new BL_PatientLabBills();
            regNO = Nxtreg.GetNextPatientRegNO(catl).Tables[0].Rows[0]["PatientRegNo"].ToString();

            return Json(regNO);

        }
        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = _PatientLabBills.GetAllFinancialYear();
            
            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                PatientLabBillsList.Add(new PatientLabBills
                {
                    FinancialYear = dr["FinancialYear"].ToString(),
                    FinancialYearID =Convert.ToInt32(dr["FinancialYearID"])
                });
            }
            return new JsonResult { Data = PatientLabBillsList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public ActionResult FillData(int PatientRegNO)
        //{
        //    PatientLabBills catl = new PatientLabBills();
        //    try
        //    {
               
        //        DataSet dsPatientOPD = new DataSet();
        //        DataSet dsPatientOPDDetails = new DataSet();

        //        int IPDNo = 0;
        //        BL_PatientLabBills BL_obj = new BL_PatientLabBills();
        //        DataSet dsPatient = new DataSet();
        //        dsPatient.Reset();
             
        //        dsPatient = BL_obj.GetPatientForLabBills(HospitalID, LocationID, PatientRegNO);

        //        if (dsPatient.Tables[0].Rows.Count > 0)
        //        {
        //            if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
        //            {
        //                IPDNo = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["PatientIPDNo"].ToString());
        //            }

        //        }
        //        //if (obj_PatientLABBills_R.CheckFinalBill(HospitalID, LocationID, PatientRegNO, IPDNo) == false)
        //        //{
        //        if (dsPatient.Tables.Count > 0)
        //        {
        //            if (dsPatient.Tables[0].Rows.Count > 0)
        //            {
        //                catl.PatientRegNo = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();

        //                catl.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
        //                catl.GuardianName = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
        //                catl.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
        //                catl.AgeType = dsPatient.Tables[0].Rows[0]["AgeType"].ToString();
        //                catl.Age = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["Age"]).ToString();
        //                catl.Gender = dsPatient.Tables[0].Rows[0]["Gender"].ToString();
        //                catl.DateOfBirth = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["DateOfBirth"]).ToString("dd/MMM/yyyy");
        //                catl.MobileNo = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
        //                catl.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
        //                //  EnablForOutPatient();
        //                if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
        //                {
        //                    #region IPD Detail Of Patient

        //                    catl.PatientIPDNO = dsPatient.Tables[0].Rows[0]["PatientIPDNO"].ToString();
        //                    catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
        //                    catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();
        //                    catl.WardID = dsPatient.Tables[0].Rows[0]["WardID"].ToString();
        //                    //  WardID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["WardID"].ToString());
        //                    catl.RoomID = dsPatient.Tables[0].Rows[0]["RoomID"].ToString();
        //                    catl.BedID = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
        //                    catl.PatientType = dsPatient.Tables[0].Rows[0]["AddmissionType"].ToString();
        //                    catl.LabType = "IPDLab";
        //                    //  BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString()));
        //                    #endregion
        //                }
        //                else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
        //                {
        //                    #region OPD Detail Of Patient
        //                    catl.PatientOPDNO = dsPatient.Tables[0].Rows[0]["PatientOPDNO"].ToString();

        //                    catl.DoctorID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString());
        //                    catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();
        //                    catl.LabType = "OPDLab";
        //                    //BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString()));
        //                    #endregion
        //                }
        //                else
        //                {

        //                }


        //                if (dsPatient.Tables[4].Rows.Count > 0)
        //                {
        //                    if (dsPatient.Tables[4].Rows[0]["ServiceCharge"].ToString() != null && dsPatient.Tables[4].Rows[0]["ServiceCharge"].ToString().Trim() != "")
        //                    {
        //                        //   catl. = dsPatient.Tables[4].Rows[0]["ServiceCharge"].ToString();
        //                    }
        //                    else
        //                    {
        //                        // ucPatientLabBills1.txtServiceTax.Text = "0.00";
        //                    }
        //                }

        //                //   ucPatientLabBills1.atxtTestName.Focus();
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //  ExceptionManager.Publish(ex);
        //        // ShowMessage(ex.Message, MessageTypes.ErrorMessage);
        //    }
        //    return View(catl);
        //}
        public ActionResult Package(string prefix)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
        
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
           
           
            DataSet ds1 = objTest.GetPackage(prefix);
            foreach(DataRow dr in ds1.Tables[0].Rows)
            {
                PatientLabBills cat = new PatientLabBills();
                cat.ProfileID = Convert.ToInt32(dr["TestID"].ToString());

                cat.ProfileName = dr["TestName"].ToString();
                cat.Rate = dr["Rate"].ToString();
                searchlist.Add(cat);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindCharges(int DrID, int testID, string select, int OutSoreceID, int tpaID)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            List<PatientLabBills> GetOutSourceLab = new List<PatientLabBills>();
              List<PatientLabBills> GetTPA = new List<PatientLabBills>();
            DataSet ds = objTest.GetPatientLabBillsForTestMaster(DrID);
            DataSet ds1 = objTest.GetOutSourceLabTestRate(OutSoreceID);
            DataView dv = new DataView();
            if (DrID >= 0)
            {
                if (select == "Test")
                {
                    dv = new DataView(ds.Tables[0], " TestID = " + testID + " ", "", DataViewRowState.CurrentRows);

                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        searchlist.Add(new PatientLabBills
                        {
                           
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                       GeneralCharges = dr["GeneralCharges"].ToString(),
                       EmergencyCharges = dr["EmergencyCharges"].ToString(),


                        });
                       
                       
                    }
                    DataView dvTest = new DataView(ds1.Tables[0], "TestID = " + testID + " ", "", DataViewRowState.CurrentRows);

                    foreach (DataRow dr in dvTest.ToTable().Rows)
                    {
                        GetOutSourceLab.Add(new PatientLabBills
                        {

                            OutSourceLabID = Convert.ToInt32(dr["OutSourceLabID"].ToString()),

                       LabName = dr["LabName"].ToString(),
                       TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                      Rate = dr["Rate"].ToString(),


                        });
                       
                    }
                   
                  
                }
                else
                {
                    if (tpaID > 0)
                    {
                       DataView dv1 = new DataView(ds.Tables[3], " ProfileID = " + testID + " ", "", DataViewRowState.CurrentRows);
                       foreach (DataRow dr in dv1.ToTable().Rows)
                       {
                           GetTPA.Add(new PatientLabBills
                           {
                               ProfileID=Convert.ToInt32(dr["ProfileID"].ToString()),
                               ProfileName=dr["Name"].ToString(),
                               TestID = dr["TestID"].ToString(),
                               TestName = dr["TestName"].ToString(),
                               GeneralCharges = dr["GeneralCharges"].ToString(),
                               EmergencyCharges = dr["EmergencyCharges"].ToString(),


                           });
                       }
                    }
                   


                        dv = new DataView(ds.Tables[2], " ProfileID = " + testID + " ", "", DataViewRowState.CurrentRows);
                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            searchlist.Add(new PatientLabBills
                            {

                                TestID = dr["TestID"].ToString(),
                                TestName = dr["TestName"].ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                EmergencyCharges = dr["EmergencyCharges"].ToString(),


                            });


                        }



                        DataView dvTest = new DataView(ds1.Tables[0], "TestID = " + testID + " ", "", DataViewRowState.CurrentRows);

                        foreach (DataRow dr in dvTest.ToTable().Rows)
                        {
                            GetOutSourceLab.Add(new PatientLabBills
                            {

                                OutSourceLabID = Convert.ToInt32(dr["OutSourceLabID"].ToString()),

                                LabName = dr["LabName"].ToString(),
                                TestID = dr["TestID"].ToString(),
                                TestName = dr["TestName"].ToString(),
                                Rate = dr["Rate"].ToString(),


                            });

                        
                    }
                }
            }

            return Json(new { searchlist = searchlist, GetOutSourceLab = GetOutSourceLab, GetTPA = GetTPA }, JsonRequestBehavior.AllowGet);
           // return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindCharges1(int tpaID, int testID, string select, int DrID)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetPatientLabBillsForTestMaster(DrID);

            if (select == "Test")
            {

                DataView dv1 = new DataView(ds.Tables[1], " TestID = " + testID + " ", "", DataViewRowState.CurrentRows);
                foreach (DataRow dr in dv1.ToTable().Rows)
                {
                    searchlist.Add(new PatientLabBills
                    {
                       
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = dr["EmergencyCharges"].ToString(),


                    });
                }
            }
            else
            {
                DataView dv1 = new DataView(ds.Tables[3], " ProfileID = " + testID + " ", "", DataViewRowState.CurrentRows);
                foreach (DataRow dr in dv1.ToTable().Rows)
                {
                    searchlist.Add(new PatientLabBills
                    {
                        ProfileID = Convert.ToInt32(dr["ProfileID"].ToString()),
                        ProfileName = dr["Name"].ToString(),
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = dr["EmergencyCharges"].ToString(),


                    });
                }
            }


            return Json(new { searchlist = searchlist }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestName(string prefix)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetTest(prefix);

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills cat = new PatientLabBills();

                cat.TestID = dr["TestID"].ToString();

                cat.TestName = dr["TestName"].ToString();
                cat.GeneralCharges = dr["GeneralCharges"].ToString();
                cat.EmergencyCharges = dr["EmergencyCharges"].ToString();

                searchlist.Add(cat);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult sample(string id)
        {
            BL_PatientLabBills objTest = new BL_PatientLabBills();
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            DataSet ds = objTest.GetUser1(id);

         
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills cat = new PatientLabBills();

                cat.UserID = dr["UserID"].ToString();

                cat.FullName = dr["FullName"].ToString();


                searchlist.Add(cat);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult OLDBILLBind(int RegNo)
        {
            PatientLabBills obj = new PatientLabBills();
            BL_PatientLabBills BL_obj = new BL_PatientLabBills();
           // HospitlLocationID();
            DataSet ds = BL_obj.GetPatientOLDLabBillsNO(Convert.ToInt32(RegNo), "OPDLabBills", HospitalID, LocationID);
            List<PatientLabBills> fillList = new List<PatientLabBills>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabBills objOLDBill = new PatientLabBills();
                objOLDBill.OLDBill = dr["BillNo&Date"].ToString();
                objOLDBill.BillNo = dr["BillNo"].ToString();
                objOLDBill.BillType = dr["BillType"].ToString();
                fillList.Add(objOLDBill);
            }

            return Json(fillList);
        }
        public ActionResult FillRegDetails(string RegNo,int OPDIPDNo)

        {
            List<PatientLabBills> fillList = new List<PatientLabBills>();
            PatientLabBills catl = new PatientLabBills();

            DataSet dsPatientOPD = new DataSet();
            DataSet dsPatientOPDDetails = new DataSet();
            DataSet dsPrivilegeCard = new DataSet();
            if(OPDIPDNo>0)
            {
                RegNo = "0";
            }
            int IPDNo = 0;
            string OrganizationName = "";
            BL_PatientLabBills BL_obj = new BL_PatientLabBills();
            DataSet dsPatient = new DataSet();
            dsPatient.Reset();
            dsPrivilegeCard.Reset();
            if (RegNo!="")
                
            dsPatient = BL_obj.GetPatientForLabBills(HospitalID, LocationID, Convert.ToInt32(RegNo),OPDIPDNo);
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        IPDNo = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["PatientIPDNo"].ToString());
                       
                    }

                }
            }

            dsPrivilegeCard = BL_obj.GetPatientPrivilegeDiscount(HospitalID, LocationID, Convert.ToInt32(RegNo));
            if (dsPrivilegeCard.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsPrivilegeCard.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date)
                {
                    //ucPatientLabBills1.chkPrivilegeCard.Checked = true;
                    catl.PrivilegeCard = dsPrivilegeCard.Tables[0].Rows[0]["CardName"].ToString();
                }
            }
            //if (obj_PatientLABBills_R.CheckFinalBill(HospitalID, LocationID, PatientRegNO, IPDNo) == false)
            //{
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    catl.PatientRegNo = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    // catl.PatientIPDNO = dsPatient.Tables[0].Rows[0]["PatientIPDNO"].ToString();
                    catl.PrefixName = dsPatient.Tables[0].Rows[0]["PFPatientName"].ToString();
                    catl.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    catl.GuardianName = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
                    catl.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
                    catl.AgeType = dsPatient.Tables[0].Rows[0]["AgeType"].ToString();
                    //catl.OrganizationName = dsPatient.Tables[0].Rows[0]["OrganizationName"].ToString();
                    catl.Age = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["Age"]).ToString();
                    catl.Gender = dsPatient.Tables[0].Rows[0]["Gender"].ToString();
                    catl.DateOfBirth = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["DateOfBirth"]).ToString("yyyy-MM-dd");
                    catl.MobileNo = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
                    catl.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
                    //  EnablForOutPatient();
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        #region IPD Detail Of Patient

                        //  PrintOPDIPDNo
                        catl.PrintOPDIPDNo = dsPatient.Tables[0].Rows[0]["PrintOPDIPDNo"].ToString();

                        catl.PreBalanceAmount = Convert.ToDecimal(dsPatient.Tables[3].Rows[0]["PreBalance"].ToString());

                      
                        //catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                        catl.PatientIPDNO = dsPatient.Tables[0].Rows[0]["PatientIPDNO"].ToString();

                        // catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();
                        if (dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString() != "" || dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["DoctorPrintName1"].ToString();

                            //catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            //DataSet ds = BL_obj.GetRefferedDoctor("%");
                            //DataView dv = new DataView(ds.Tables[0], "DoctorID=" + catl.ReferredByDoctorID + "", "", DataViewRowState.CurrentRows);
                            //catl.ReferredByDoctorID = dv.ToTable().Rows[0]["DoctorPrintName"].ToString();
                        }

                        if (dsPatient.Tables[0].Rows[0]["WardID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["WardID"].ToString() != "0")
                        {
                            catl.WardID = dsPatient.Tables[0].Rows[0]["WardID"].ToString();

                            DataSet ds = BL_obj.GetWardName("%");
                            DataView dv = new DataView(ds.Tables[0], "WardID=" + catl.WardID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.WardID = dv.ToTable().Rows[0]["WardName"].ToString();
                            }
                        }

                        if (dsPatient.Tables[0].Rows[0]["RoomID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["RoomID"].ToString() != "0")
                        {
                            catl.RoomID = dsPatient.Tables[0].Rows[0]["RoomID"].ToString();
                            DataTable ds = BL_obj.GetRoomNo();
                            DataView dv = new DataView(ds, "RoomID=" + catl.RoomID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.RoomID = dv.ToTable().Rows[0]["RoomName"].ToString();
                            }
                        }

                        if (dsPatient.Tables[0].Rows[0]["BedID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["BedID"].ToString() != "0")
                        {
                            catl.BedID = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
                            DataTable ds = BL_obj.GetBedNo();
                            DataView dv = new DataView(ds, "BedID=" + catl.BedID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.BedID = dv.ToTable().Rows[0]["BedNo"].ToString();
                            }
                        }



                        //catl.BedID = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
                        //  catl.PatientType = dsPatient.Tables[0].Rows[0]["AddmissionType"].ToString();
                        catl.LabType = "IPDLab";


                        catl.FinancialYearID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["FinancialYearID"].ToString());
                        catl.FinancialYear = dsPatient.Tables[0].Rows[0]["FinancialYear"].ToString();

                        catl.OrganizationName = dsPatient.Tables[0].Rows[0]["OrganizationName"].ToString();




                        //  BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString()));
                        #endregion
                    }
                    else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
                    {
                        catl.PrintOPDIPDNo = dsPatient.Tables[0].Rows[0]["PrintOPDIPDNo"].ToString();
                        //catl.OPDLabBills = "OPDLabBills";
                        #region OPD Detail Of Patient

                        if (dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["DoctorID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["DoctorPrintName1"].ToString();

                            //catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            //DataSet ds = BL_obj.GetRefferedDoctor("%");
                            //DataView dv = new DataView(ds.Tables[0], "DoctorID=" + catl.ReferredByDoctorID + "", "", DataViewRowState.CurrentRows);
                            //catl.ReferredByDoctorID = dv.ToTable().Rows[0]["DoctorPrintName"].ToString();
                        }




                        catl.PatientOPDNO = dsPatient.Tables[0].Rows[0]["PatientOPDNO"].ToString();

                        // catl.DoctorID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString());

                        catl.LabType = "OPDLab";

                        catl.FinancialYearID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["FinancialYearID"].ToString());
                        catl.FinancialYear = dsPatient.Tables[0].Rows[0]["FinancialYear"].ToString();
                        if (dsPatient.Tables[3].Rows[0]["PreBalance"].ToString() != "" && dsPatient.Tables[3].Rows[0]["PreBalance"].ToString()!="0")
                        {
                            catl.PreBalanceAmount = Convert.ToDecimal(dsPatient.Tables[3].Rows[0]["PreBalance"]);
                            
                           
                        }
                       
                        else
                        {
                            catl.PreBalanceAmount = 0;
                        }



                        //BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString()));
                        #endregion
                    }

                }
            }

            //  PatientLabBills obj_OldBill = new PatientLabBills();



            fillList.Add(catl);
            return Json(fillList, JsonRequestBehavior.AllowGet);

        }  
        public ActionResult GetPatientForLabBillsOPDIPD(string PatientType,int OPDIPDNo)
        {
            List<PatientLabBills> fillList = new List<PatientLabBills>();
            PatientLabBills catl = new PatientLabBills();

            DataSet dsPatientOPD = new DataSet();
            DataSet dsPatientOPDDetails = new DataSet();
            DataSet dsPrivilegeCard = new DataSet();
           
            int IPDNo = 0;
            BL_PatientLabBills BL_obj = new BL_PatientLabBills();
            DataSet dsPatient = new DataSet();
            dsPatient.Reset();
            dsPrivilegeCard.Reset();
         
                dsPatient = BL_obj.GetPatientForLabBillsOPDIPD(PatientType, OPDIPDNo);
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        IPDNo = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["PatientIPDNo"].ToString());
                    }

                }
            }

            dsPrivilegeCard = BL_obj.GetPatientPrivilegeDiscount(HospitalID, LocationID, Convert.ToInt32(dsPatient.Tables[0].Rows[0]["PatientRegNO"]));
            if (dsPrivilegeCard.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsPrivilegeCard.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date)
                {
                    //ucPatientLabBills1.chkPrivilegeCard.Checked = true;
                    catl.PrivilegeCard = dsPrivilegeCard.Tables[0].Rows[0]["CardName"].ToString();
                }
            }
            //if (obj_PatientLABBills_R.CheckFinalBill(HospitalID, LocationID, PatientRegNO, IPDNo) == false)
            //{
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    catl.PatientRegNo = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    catl.PrintRegNO = dsPatient.Tables[0].Rows[0]["PrintRegNO"].ToString();
                    catl.PrefixName = dsPatient.Tables[0].Rows[0]["PFPatientName"].ToString();
                    catl.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    catl.GuardianName = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
                    catl.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
                    catl.AgeType = dsPatient.Tables[0].Rows[0]["AgeType"].ToString();
                    catl.Age = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["Age"]).ToString();
                    catl.Gender = dsPatient.Tables[0].Rows[0]["Gender"].ToString();
                    catl.DateOfBirth = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["DateOfBirth"]).ToString("yyyy-MM-dd");
                    catl.MobileNo = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
                    catl.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
                    //  EnablForOutPatient();
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        #region IPD Detail Of Patient

                        //  PrintOPDIPDNo
                        catl.PrintOPDIPDNo = dsPatient.Tables[0].Rows[0]["PrintOPDIPDNo"].ToString();

                        catl.PreBalanceAmount = Convert.ToDecimal(dsPatient.Tables[3].Rows[0]["PreBalance"].ToString());


                        //catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                        catl.PatientIPDNO = dsPatient.Tables[0].Rows[0]["PatientIPDNO"].ToString();

                        // catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();
                        if (dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString() != "" || dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["DoctorPrintName1"].ToString();

                            //catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            //DataSet ds = BL_obj.GetRefferedDoctor("%");
                            //DataView dv = new DataView(ds.Tables[0], "DoctorID=" + catl.ReferredByDoctorID + "", "", DataViewRowState.CurrentRows);
                            //catl.ReferredByDoctorID = dv.ToTable().Rows[0]["DoctorPrintName"].ToString();
                        }

                        if (dsPatient.Tables[0].Rows[0]["WardID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["WardID"].ToString() != "0")
                        {
                            catl.WardID = dsPatient.Tables[0].Rows[0]["WardID"].ToString();

                            DataSet ds = BL_obj.GetWardName("%");
                            DataView dv = new DataView(ds.Tables[0], "WardID=" + catl.WardID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.WardID = dv.ToTable().Rows[0]["WardName"].ToString();
                            }
                        }

                        if (dsPatient.Tables[0].Rows[0]["RoomID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["RoomID"].ToString() != "0")
                        {
                            catl.RoomID = dsPatient.Tables[0].Rows[0]["RoomID"].ToString();
                            DataTable ds = BL_obj.GetRoomNo();
                            DataView dv = new DataView(ds, "RoomID=" + catl.RoomID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.RoomID = dv.ToTable().Rows[0]["RoomName"].ToString();
                            }
                        }

                        if (dsPatient.Tables[0].Rows[0]["BedID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["BedID"].ToString() != "0")
                        {
                            catl.BedID = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
                            DataTable ds = BL_obj.GetBedNo();
                            DataView dv = new DataView(ds, "BedID=" + catl.BedID + "", "", DataViewRowState.CurrentRows);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                catl.BedID = dv.ToTable().Rows[0]["BedNo"].ToString();
                            }
                        }



                        //catl.BedID = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
                        //  catl.PatientType = dsPatient.Tables[0].Rows[0]["AddmissionType"].ToString();
                        catl.LabType = "IPDLab";


                        catl.FinancialYearID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["FinancialYearID"].ToString());
                        catl.FinancialYear = dsPatient.Tables[0].Rows[0]["FinancialYear"].ToString();




                        //  BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString()));
                        #endregion
                    }
                    else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
                    {
                        catl.PrintOPDIPDNo = dsPatient.Tables[0].Rows[0]["PrintOPDIPDNo"].ToString();
                        #region OPD Detail Of Patient

                        if (dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["DoctorID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["DoctorPrintName1"].ToString();

                            //catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                            //DataSet ds = BL_obj.GetRefferedDoctor("%");
                            //DataView dv = new DataView(ds.Tables[0], "DoctorID=" + catl.ReferredByDoctorID + "", "", DataViewRowState.CurrentRows);
                            //catl.ReferredByDoctorID = dv.ToTable().Rows[0]["DoctorPrintName"].ToString();
                        }




                        catl.PatientOPDNO = dsPatient.Tables[0].Rows[0]["PatientOPDNO"].ToString();

                        // catl.DoctorID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString());

                        catl.LabType = "OPDLab";

                        catl.FinancialYearID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["FinancialYearID"].ToString());
                        catl.FinancialYear = dsPatient.Tables[0].Rows[0]["FinancialYear"].ToString();
                        if (dsPatient.Tables[3].Rows[0]["PreBalance"].ToString() != "" && dsPatient.Tables[3].Rows[0]["PreBalance"].ToString() != "0")
                        {
                            catl.PreBalanceAmount = Convert.ToDecimal(dsPatient.Tables[3].Rows[0]["PreBalance"]);


                        }

                        else
                        {
                            catl.PreBalanceAmount = 0;
                        }



                        //BindTestCharges(Convert.ToInt32(dsPatient.Tables[0].Rows[0]["DoctorID"].ToString()));
                        #endregion
                    }

                }
            }

            //  PatientLabBills obj_OldBill = new PatientLabBills();



            fillList.Add(catl);
            return Json(fillList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeletePatientBills(string PatientRegNO, string BillNo,string txtinput)
        {
            string del = "";

            HospitlLocationID();
            Connect();
            AuthoriseSave(BillNo);
            if (con.State.ToString() == "Close")
            {
                con.Open();
            }
            BL_PatientOPDBill obj = new BL_PatientOPDBill();
            bool ChkAuth = obj.ForAuthorizationGetMasterSetting();
            SqlCommand cmd;
            if (ChkAuth)
            {
                cmd = new SqlCommand("DeletePatientBillsAuthorise", con);
            }
            else
            {
                cmd = new SqlCommand("DeletePatientBills", con);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@BillNo", BillNo);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            int a = cmd.ExecuteNonQuery();
            con.Close();
            //  BL_PatientOPDBill obj = new BL_PatientOPDBill();
            if (a > 0)
            {
                del = "Delete";
                ViewData["Delete"] = "Delete";
            }
            if (ChkAuth)
            {
                con.Close();
                SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                cmdIUForAuthorization.Parameters.AddWithValue("@UserID", UserID);
                cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", txtinput);
                cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", BillNo);
                cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "LabBill");
                cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", UserID);
                cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                con.Open();
                cmdIUForAuthorization.ExecuteNonQuery();

            }
            return Json(del, JsonRequestBehavior.AllowGet);
        }

        BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
        public void AuthoriseSave(string BillNo)
        {

            BL_PatientOPDBill BL_obj = new BL_PatientOPDBill();
            DataSet dsPatientOldBills = new DataSet();
            try
            {

            dsPatientOldBills = BL_obj.GetPatientBills(Convert.ToInt32(BillNo));
            foreach (DataRow dr in dsPatientOldBills.Tables[0].Rows)
            {
                SqlCommand cmdBillModify = new SqlCommand("IUPatientBillsModify", con);
                cmdBillModify.CommandType = CommandType.StoredProcedure;
                cmdBillModify.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdBillModify.Parameters.AddWithValue("@LocationID", LocationID);
                cmdBillModify.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                cmdBillModify.Parameters.AddWithValue("@PrintBillNo", dr["PrintBillNo"]);
                cmdBillModify.Parameters.AddWithValue("@FinancialYearID", dr["FinancialYearID"]);
                cmdBillModify.Parameters.AddWithValue("@SancationAmount", dr["SancationAmount"]);
                cmdBillModify.Parameters.AddWithValue("@NonMedicalExp", dr["NonMedicalExp"]);
                cmdBillModify.Parameters.AddWithValue("@BillDate", dr["BillDate"]);
                cmdBillModify.Parameters.AddWithValue("@PatientRegNO", dr["PatientRegNO"]);
                cmdBillModify.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                cmdBillModify.Parameters.AddWithValue("@BillType", dr["BillType"]);
                cmdBillModify.Parameters.AddWithValue("@GrossAmount", dr["GrossAmount"]);
                cmdBillModify.Parameters.AddWithValue("@TaxPercent", dr["TaxPercent"]);
                cmdBillModify.Parameters.AddWithValue("@ReffCommission", "0.00");
                cmdBillModify.Parameters.AddWithValue("@Commisson", "0.00");
                cmdBillModify.Parameters.AddWithValue("@TotalAmount", dr["TotalAmount"]);
                cmdBillModify.Parameters.AddWithValue("@DiscountPercent", "");
                cmdBillModify.Parameters.AddWithValue("@DiscountAmount", dr["DiscountAmount"]);
                cmdBillModify.Parameters.AddWithValue("@NetPayableAmount", dr["NetPayableAmount"]);
                cmdBillModify.Parameters.AddWithValue("@BalanceAmount", dr["BalanceAmount"]);
                cmdBillModify.Parameters.AddWithValue("@PreBalanceAmount", dr["PreBalanceAmount"]);
                cmdBillModify.Parameters.AddWithValue("@IsPaid", dr["IsPaid"]);
                cmdBillModify.Parameters.AddWithValue("@PaidAmount", dr["PaidAmount"]);
                cmdBillModify.Parameters.AddWithValue("@TaxAmount", dr["TaxAmount"]);
                cmdBillModify.Parameters.AddWithValue("@DipositAmount", "0.00");
                cmdBillModify.Parameters.AddWithValue("@ForAuthorization", 1);
                // cmdBillModify.Parameters.AddWithValue(" @DipositAmount", "0.00");
                cmdBillModify.Parameters.AddWithValue("@IsBillMade", "false");
                cmdBillModify.Parameters.AddWithValue("@CreationID", UserID);
                cmdBillModify.Parameters.AddWithValue("@Mode", "Add");

                con.Open();
                int IUPatientBillsModify = cmdBillModify.ExecuteNonQuery();

            }
            // BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
            DataSet dsPatientBllDetails = new DataSet();
            dsPatientBllDetails = BillModifyDetails.GetPatientLabOLdBillsDetails(HospitalID, LocationID, Convert.ToInt32(BillNo));
            SqlCommand updateauth = new SqlCommand("update PatientBillsDetailsModify set RowStatus = 2 where BillNo = '" + BillNo + "' and RowStatus = 0", con);
            updateauth.ExecuteNonQuery();
            foreach (DataRow dr1 in dsPatientBllDetails.Tables[0].Rows)
            {

                SqlCommand cmdBillModifyDetail = new SqlCommand("IUPatientBillsDetailsModify", con);
                cmdBillModifyDetail.CommandType = CommandType.StoredProcedure;
                cmdBillModifyDetail.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdBillModifyDetail.Parameters.AddWithValue("@LocationID", LocationID);
                cmdBillModifyDetail.Parameters.AddWithValue("@BillModifyID", 0);
                cmdBillModifyDetail.Parameters.AddWithValue("@ServiceID", dr1["ServiceID"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@ServiceName", dr1["ServiceName"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@BillNo", dr1["BillNo"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@UnitID ", dr1["UnitID"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@ChargesType", dr1["ChargesType"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@Rate", dr1["Rate"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@Quantity", dr1["Quantity"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@Discount", dr1["Discount"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@ReffCommission", "0.00");
                cmdBillModifyDetail.Parameters.AddWithValue("@Commisson", "0.00");
                cmdBillModifyDetail.Parameters.AddWithValue("@ServiceType", dr1["ServiceType"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@TotalAmount", dr1["TotalAmount"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@HideInBilling", dr1["HideInBilling"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@DoctorID", dr1["DoctorID"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@ForAuthorization", 1);
                cmdBillModifyDetail.Parameters.AddWithValue("@DiscountServiceType", dr1["DiscountServiceType"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@Discount_Service", dr1["Discount_Service"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@DoctorCharges", dr1["DoctorCharges"]);
                cmdBillModifyDetail.Parameters.AddWithValue("@CreationID", UserID);
                cmdBillModifyDetail.Parameters.AddWithValue("@Mode", "Add");

                int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();


            }

            }
            catch (Exception exe)
            {

                throw;
            }
        }
        public ActionResult FillOldBillDetails(int BillNo, PatientLabBills obj)
        {
            List<PatientLabBills> searchlist = new List<PatientLabBills>();
            string Mode;
            if (BillNo > 0)
            {
                Mode = "Edit";
            }
            BL_PatientLabBills BL_obj = new BL_PatientLabBills();
            DataSet dsPatientLABBills = new DataSet();
            DataSet dsPatientLABBillsDetails = new DataSet();
            List<PatientLabBills> fillList = new List<PatientLabBills>();
            DataSet ds1 = new DataSet();

            dsPatientLABBills.Reset();
            dsPatientLABBillsDetails.Reset();
            dsPatientLABBills = BL_obj.GetPatientOldLabBills(HospitalID, LocationID, BillNo);

            for (int i = 0; i < dsPatientLABBills.Tables[0].Rows.Count; i++)
            {
                if (dsPatientLABBills.Tables.Count > 0)
                {
                    obj.ForAuthorization = "0";
                    if (dsPatientLABBills.Tables[0].Rows.Count > 0)
                    {

                        DataTable dt1 = new DataTable();

                        string ForAuthorization = dsPatientLABBills.Tables[0].Rows[0]["ForAuthorization"].ToString();

                        if (ForAuthorization == "1")
                        {
                            Connect();
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();


                            SqlCommand cmd1 = new SqlCommand("GetPatientBillsForAuthorization", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                            cmd1.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                            cmd1.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNo)));
                            //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                            SqlDataAdapter sd1 = new SqlDataAdapter();
                            sd1.SelectCommand = cmd1;

                            dsPatientLABBills.Reset();
                            con.Open();
                            sd1.Fill(dsPatientLABBills);                            
                        }
                        Connect();
                        SqlCommand cmd2 = new SqlCommand();
                        if (ForAuthorization == "0" || ForAuthorization == "" || ForAuthorization == null)
                        {
                            cmd2 = new SqlCommand("GetPatientLabOLdBillsDetails", con);
                        }
                        else
                        {
                            if (ForAuthorization == "1")
                            {
                                cmd2 = new SqlCommand("GetPatientLabOLdBillsDetailsAurthorise", con);
                            }

                        }

                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd2.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNo)));
                        //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                        SqlDataAdapter sd2 = new SqlDataAdapter();
                        sd2.SelectCommand = cmd2;
                       

                        sd2.Fill(ds1);                      
                    }

                    obj.PaymentType = dsPatientLABBills.Tables[0].Rows[0]["PaymentType"].ToString();
                    switch (obj.PaymentType)
                    {
                        case "Cheque":
                            obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();
                            obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                            obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                            obj.Remarks = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                            break;

                        case "Credit Card":
                            obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();
                            obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                            obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                            obj.Remarks = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                            break;

                        case "Debit Card":
                            obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();
                            obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                            obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                            obj.Remarks = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                            break;


                        case "E-Money":
                            obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();
                            obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                            obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                            obj.Remarks = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                            break;

                        case "EFT":
                            obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();
                            obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                            obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                            obj.Remarks = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                            break;

                        default:
                            obj.Number = "Cash";
                            obj.Name = "Cash";
                            obj.PaymentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                            obj.Remarks = "Cash";
                            break;
                    }
                }

                DataRow dr1 = dsPatientLABBills.Tables[0].Rows[i];

                string DateTimestr = Convert.ToString(dr1["BillDate"]);
                DateTime DateTime = Convert.ToDateTime(DateTimestr);

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {

                    obj.TestID = dr["ServiceID"].ToString();
                    obj.samplecollection = dr["TestStatus"].ToString();
                    if (dr["TestStatus"].ToString() == "BILLING")
                    {
                        obj.samplecollection = "0";

                    }
                    else
                    {
                        if (dr["TestStatus"].ToString() == "SAMPLECOLLECTION")
                        {
                            obj.samplecollection = "1";

                        }
                    }
                    obj.TestName = dr["ServiceName"].ToString();
                    obj.SampleCollectionBoyID = dr["SampleCollectionID"].ToString();
                    obj.OrganizationName = dr["TPAWiseName"].ToString();
                    obj.CollectionCentreName = dr["CollectionCentre"].ToString();
                    obj.Rate = dr["Rate"].ToString();
                    obj.Qty = dr["Quantity"].ToString();
                    obj.Discount_Service = dr["Discount_Service"].ToString();
                    obj.DiscountServiceType = dr["DiscountServiceType"].ToString();
                    obj.ServiceTotal = Convert.ToDecimal(dr["TotalAmount"].ToString());
                    if (dr["ReportingDate"].ToString() != "")
                    {
                        obj.ReportingDate = Convert.ToDateTime(dr["ReportingDate"]).ToString("dd-MM-yyyy");
                    }
                    obj.OutSourceID = dr["OutSourceID"].ToString();
                    obj.LabName = dr["OutSourceLabName"].ToString();
                    obj.ServiceType = dr["ServiceType"].ToString();

                    if (dsPatientLABBills.Tables[0].Rows.Count > 0)
                    {
                        obj.ForAuthorization = dsPatientLABBills.Tables[0].Rows[0]["ForAuthorization"].ToString();
                        obj.BillDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["BillDate"]).ToString("yyyy-MM-dd");
                        obj.BillDatetime = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["BillDate"]).ToString("hh:mm");
                        obj.GrossAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["GrossAmount"]);
                        obj.TaxPercent = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["TaxPercent"].ToString());
                        obj.TaxAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["TaxAmount"].ToString());
                        obj.TotalAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["TotalAmount"].ToString());


                        obj.Discount = dsPatientLABBills.Tables[0].Rows[0]["DiscountAmount"].ToString();

                        obj.Name = dsPatientLABBills.Tables[0].Rows[0]["Name"].ToString();
                        //obj.PaymentType = dsPatientLABBills.Tables[0].Rows[0]["PaymentType"].ToString();
                        //obj.PaymentDate = Convert.ToDateTime(dsPatientLABBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");


                        //obj.Remark = dsPatientLABBills.Tables[0].Rows[0]["Remarks"].ToString();
                        //obj.Number = dsPatientLABBills.Tables[0].Rows[0]["Number"].ToString();


                        if (dsPatientLABBills.Tables[0].Rows[0]["PreBalanceAmount"].ToString() == "")
                        {

                        }
                        else
                        {
                            obj.PreBalanceAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["PreBalanceAmount"].ToString());
                        }
                        obj.NetPayableAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["NetPayableAmount"].ToString());
                        obj.PaidAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["PaidAmount"].ToString());
                        obj.BalanceAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["BalanceAmount"].ToString());
                        obj.DiscountReason = dsPatientLABBills.Tables[0].Rows[0]["DiscountReason"].ToString();
                        obj.TaxAmount = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["TaxAmount"].ToString());
                        obj.TaxPercent = Convert.ToDecimal(dsPatientLABBills.Tables[0].Rows[0]["TaxPercent"].ToString());
                    }
                }
              
            }


            searchlist.Add(obj);
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ShowPatient()
        {

            BL_PatientLabBills BL_obj = new BL_PatientLabBills();

            return new JsonResult { Data = BL_obj.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        public JsonResult GetAllPatient(string prefix)
        {
            BL_PatientLabBills BL_obj = new BL_PatientLabBills();

            DataSet ds = BL_obj.GetAllPatient(prefix);
            List<PatientOPDBill> searchList = new List<PatientOPDBill>();
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientOPDBill
                {
                    patientregNo = (dr["PatientRegNO"].ToString()),
                    patientname = dr["PatientName"].ToString(),
                    address = (dr["Address"].ToString()),
                    contactno = dr["MobileNo"].ToString(),
                    PrintRegNO = dr["PrintRegNO"].ToString()
                });
            }
            //}
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}