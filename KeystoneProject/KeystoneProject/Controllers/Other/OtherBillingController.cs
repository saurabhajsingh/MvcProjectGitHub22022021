using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Laboratory;
namespace KeystoneProject.Controllers.Other
{
    public class OtherBillingController : Controller
    {
        // GET: OtherBilling


        int HospitalID;
        int LocationID;
        int CreationID;
        private SqlConnection con;


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        //
        // GET: /PatientOPDBill/
        DataSet dsPatientOPD = new DataSet();
        DataSet dsPatientPrivilege = new DataSet();
        DataSet dsTestName = new DataSet();
        DataSet dsServiceName = new DataSet();
        DataSet dsPatientBillsDetails = new DataSet();
        DataSet dsPatientBills = new DataSet();
        DataSet dsPatientOPDDetails = new DataSet();
        DataSet dsPatient = new DataSet();


        BL_PatientOPDBill _OPDBill = new BL_PatientOPDBill();

        //IBL_PatientOPDBills_R objPatientOPDBills_R = new BL_PatientOPDBill();

        string PreBalance = "0.00";
        string CurrentPreBalance = "0.00";
        string balance;
        string SerTaxAmount;
        double NetAmtPayable;
        double BalAmount;
        string TAmount;
        string RecommendedByDoctor = "";
        string HideInBilling;
        int iUserID;
        //  DateTime DOB;
        string TDays = "";
        string TMonth = "";
        string TYear = "";
        string TPAid;
        string TPAname;
        string rpt = "";

        //OPDBill obj1 = new OPDBill();

        KeystoneProject.Models.Patient.OPDBill obj1 = new OPDBill();
        [HttpGet]

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        [HttpGet]
        public ActionResult OtherBilling()
        {
            return View();
        }
        public static Nullable<DateTime> DateBirth
        {
            get;
            set;
        }

        public  ActionResult DateOfBirthCal(DateTime DateOfBirth)
        {
            DateTime dtToday = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtBeforeDate = new System.DateTime(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0);
            int Year = 0;
            int Month = 0;
            int Day = 0;

            dtBeforeDate = dtBeforeDate.AddYears(1);
            while (dtToday >= dtBeforeDate)
            {
                dtBeforeDate = dtBeforeDate.AddYears(1);

                Year++;
            }
            dtBeforeDate = new System.DateTime(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0);
            dtBeforeDate = dtBeforeDate.AddMonths(1);
            while (dtToday >= dtBeforeDate)
            {
                dtBeforeDate = dtBeforeDate.AddMonths(1);
                Month++;
            }

            dtBeforeDate = new System.DateTime(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0);

            dtBeforeDate = dtBeforeDate.AddMonths(Month);

            TimeSpan diffResult = dtToday.Date - dtBeforeDate.Date;

            int TotalDay = Convert.ToInt32(diffResult.TotalDays);
            int TotalMonth = Month % 12;

            obj1.Day = TotalDay.ToString();

            obj1.Month = TotalMonth.ToString();

            obj1.Years = Year.ToString();
            return Json(obj1);
        }

        public ActionResult OtherBilling(OPDBill obj, FormCollection fc)
        {
            HospitlLocationID();
            // FormCollection fc = new FormCollection();
            //   OPDBill obj = new OPDBill();
            string Mode = "Add";
            try
            {
                //string hh = TempData["Reg"].ToString();
              //  obj.PatientType
                rpt = Request.Form["rpt"];
                obj.ChargesType = Request.Form["RadioCharge"];
                obj.PaymentType = Request.Form["payment_type"];
                obj.BillDate = Convert.ToDateTime(fc["bill_date"]).ToString("dd-MMM-yyyy");// +" " + fc["bill_time"];
                                                                                           // obj.Gender = Request.Form["gen"].ToString();
                obj.PreBalance = Convert.ToDecimal(fc["PreBalanceandDiposit"]);
                if (fc["OldBillNo"] == "" || fc["OldBillNo"] == null)
                {
                    obj.BillNoId = 0;
                }
                else
                {
                    obj.BillNoId = Convert.ToInt32(fc["OldBillNo"]);
                }
                if (Request.Form["DateOfBirth"] != "")
                {
                    //string aaa = Request.Form["DateOfBirth"].ToString();
                    obj.DateOfBirth = Convert.ToDateTime(Request.Form["DateOfBirth"]).ToString("dd-MM-yyyy");
                    DateTime DAT = Convert.ToDateTime(DateBirth);
                    TimeSpan tim = DateTime.Now - DAT;
                    obj.TotalDay = tim.TotalDays.ToString(); //CalGetDaysAge();   //Math.Abs(tim.Days).ToString();
                }
                else
                {
                    obj.DateOfBirth = System.DateTime.Now.ToString("dd-mmm-yyyy");
                }





                switch (obj.PaymentType)
                {
                    case "Cheque":
                        obj.Number = Request.Form["Number"];     // "Cheques";// dr["Number"] = ucCheques1.txtChequeNO.Text;
                        obj.Name = Request.Form["Name"];
                        obj.Date = Request.Form["paymentDate"];
                        obj.Remarks = Request.Form["Remarks"];
                        break;

                    case "Credit Card":
                        obj.Number = Request.Form["Number"];
                        obj.Name = Request.Form["Name"];
                        obj.Date = Request.Form["paymentDate"];
                        obj.Remarks = Request.Form["Remarks"];
                        break;

                    case "Debit Card":
                        obj.Number = Request.Form["Number"];
                        obj.Name = Request.Form["Name"];
                        obj.Date = Request.Form["paymentDate"];
                        obj.Remarks = Request.Form["Remarks"];
                        break;

                    case "E-money":
                        obj.Number = Request.Form["Number"];
                        obj.Name = Request.Form["Name"];
                        obj.Date = Request.Form["paymentDate"];
                        obj.Remarks = "EMoney";
                        break;

                    case "EFT":
                        obj.Number = Request.Form["Number"];
                        obj.Name = Request.Form["Name"];
                        obj.Date = Request.Form["paymentDate"];
                        obj.Remarks = Request.Form["Remarks"];
                        break;

                    default:
                        obj.Number = "Cash";
                        obj.Name = "Cash";
                        obj.Date = System.DateTime.Now.ToString();
                        obj.Remarks = "Cash";
                        break;
                }
                double Qty = 0;
                double Rate = 0;
                obj.BillDate = Convert.ToDateTime(fc["bill_date"]).ToString("dd-MMM-yyyy");// +" " + fc["bill_time"];
                string svcid = fc["svc_hid"].ToString();
                string svcname = fc["svc_nm_name"].ToString();
                string unitid = fc["unithid"].ToString();
                string docid = fc["doc_hid"].ToString();
                string rate = fc["rate1table"].ToString();
                string ServiceType = fc["ServiceType"].ToString();
                string DoctorCharge = fc["DoctorCharges"].ToString();
                string qty = fc["qty"].ToString();
                string Mode1 = fc["Mode"].ToString();
                string Discount_Service = fc["Discount_Service"].ToString();
                string Discount_serviceType = fc["DiscountServiceType"].ToString();
                string Aurth = fc["Authorization"].ToString();
                // string tot = fc["total"].ToString();
                String[] service_ID = svcid.Split(',');
                String[] service_Name = svcname.Split(',');
                String[] service_unit_ID = unitid.Split(',');
                String[] service_doc_ID = docid.Split(',');
                String[] service_rate = rate.Split(',');
                String[] service_qty = qty.Split(',');
                String[] ServiceType1 = ServiceType.Split(',');
                string[] ServiceDisAmt = Discount_Service.Split(',');
                string[] ServiceDisType = Discount_serviceType.Split(',');
                String[] Mod = Mode1.Split(',');
                string[] Doctorcharges = DoctorCharge.Split(',');
                string[] ForAuthoriazation = Aurth.Split(',');
                //String[] total_a = tot.Split(',');
                ServiceName[] svc_array = new ServiceName[service_ID.Length];
                for (int i = 0; i < service_ID.Length; i++)
                {
                    svc_array[i] = new ServiceName();
                    svc_array[i].SvcID = service_ID[i].ToString();
                    svc_array[i].SvcName = service_Name[i].ToString();
                    svc_array[i].UnitID = service_unit_ID[i].ToString();
                    svc_array[i].DoctorID = service_doc_ID[i].ToString();
                    svc_array[i].Rate = service_rate[i].ToString();
                    svc_array[i].Quantity = service_qty[i].ToString();
                    svc_array[i].ServiceType = ServiceType1[i].ToString();
                    svc_array[i].Doctorcharges = Doctorcharges[i].ToString();
                    svc_array[i].Discount_Service = ServiceDisAmt[i].ToString();
                    svc_array[i].DiscountServiceType = ServiceDisType[i].ToString();
                    svc_array[i].Authorization = ForAuthoriazation[i].ToString();
                    double ServiceAmt = 0;

                    //       TotalPer = ((Amount * DiscountPercent) / 100);
                    svc_array[i].Mode = Mod[i].ToString();


                    if (svc_array[i].Rate != "")
                    {
                        Rate = Convert.ToDouble(svc_array[i].Rate);
                    }
                    if (svc_array[i].Quantity != "")
                    {
                        Qty = Convert.ToDouble(svc_array[i].Quantity);
                        double totalAmt = Rate * Qty;


                        if (svc_array[i].DiscountServiceType == "%")
                        {
                            ServiceAmt = ((totalAmt * Convert.ToDouble(svc_array[i].Discount_Service)) / 100);
                            svc_array[i].sevicedisAmt = ServiceAmt.ToString();
                        }
                        if (svc_array[i].DiscountServiceType == "Rs")
                        {
                            svc_array[i].sevicedisAmt = Convert.ToDouble(svc_array[i].Discount_Service).ToString();
                        }
                        totalAmt = totalAmt - Convert.ToDouble(svc_array[i].sevicedisAmt);
                        svc_array[i].Total = totalAmt.ToString();//total_a[i].ToString();

                    }
                }
                obj.Services = svc_array;
                if (Mode == "Add")
                {
                    HospitlLocationID();
                    // _OPDBill.Save1(obj);
                    Buisness_Logic.Other.BL_OtherBilling BL_OtherBill = new Buisness_Logic.Other.BL_OtherBilling();
                    BL_OtherBill.Save(obj);
                    OPDBill OPDbill = new OPDBill();
                    if (OPDbill.MessageError == "Done")
                    {
                        BL_PatientOPDBill db = new BL_PatientOPDBill();

                        OPDbill.BillNo = obj.BillNo;
                        Connect();
                        SqlCommand cmd = new SqlCommand("RptPatientOPDBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@BillNo", OPDbill.BillNo);
                        //Session["HospitalIDReport"] = "1";
                        //Session["LocationIDReport"] = "1";
                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["BillNoReport"] = OPDbill.BillNo.ToString();
                        Session["rptReport"] = rpt.ToString();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);
                        //con.Close();
                        //return ds;
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            //DialogResult ObjDialogResult = MessageBox.Show("With Balance Amount", "Keystone ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //if (ObjDialogResult == DialogResult.No)
                            //{
                            return RedirectToAction("RptPatientOPDBills", "PatientReport");
                        }



                        TempData["Msg"] = "Save Sussefully";
                        return RedirectToAction("OtherBilling", "OtherBilling");
                    }
                    else
                    {
                        TempData["Msg"] = OPDbill.MessageError;
                        return RedirectToAction("OtherBilling", "OtherBilling");
                    }
                    // Sms
                    //test.Controllers.Hospital.SMSSettingController objsms = new Hospital.SMSSettingController();
                    //DataSet dssms = new DataSet();
                    //dssms = objsms.GetMasterSetting(HospitalID, LocationID);

                    //string SmsChk = "";
                    //if (fc["sms"] == null)
                    //{
                    //    SmsChk = "false";
                    //}
                    //else
                    //{
                    //    SmsChk = "true";
                    //}

                    //if (SmsChk == "true" && Convert.ToBoolean(dssms.Tables[0].Rows[0]["OPDBillDrSMS"].ToString()) == true)
                    //{
                    //    objsms.SendSMS(Convert.ToInt32(obj.PatientRegNO), Convert.ToInt32(TempData["RefferDoctorID"]), TempData["DoctorPrintName"].ToString(), "OPD1", TempData["RDoctorPrintName"].ToString());
                    //}

                    //--------------------------

                    //con.Close();
                    //return ds;
                    //if (ds.Tables[0].Rows.Count != 0)
                    //{
                    //    //DialogResult ObjDialogResult = MessageBox.Show("With Balance Amount", "Keystone ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //    //if (ObjDialogResult == DialogResult.No)
                    //    //{
                    //    return RedirectToAction("RptPatientOPDBills");


                    //}
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("OtherBilling", "OtherBilling");
            }
            return View();
        }
        public JsonResult PatientRegNO_Leave(int RegistrationNO, string GTotal, string STax, string DisPercent, string DiscountAmount, decimal BalanceAmount, string PaidAmount, string TotalAmount)
        {
            Buisness_Logic.Other.BL_OtherBilling Bl_obj = new Buisness_Logic.Other.BL_OtherBilling();
            HospitlLocationID();
            string DepartName = "";
            string TPA = "";
            string TpaName1 = "";
            string RefferDoctorID;
            List<OPDBill> searchList = new List<OPDBill>();
            if (RegistrationNO != 0)
            {
                if (RegistrationNO != null)
                {
                    TempData["Reg"] = RegistrationNO;
                    dsPatientOPD = Bl_obj.GetPatientForOPDBills(RegistrationNO, HospitalID, LocationID);
                    dsPatientPrivilege = _OPDBill.GetPatientPrivilegeDiscount(HospitalID, LocationID, RegistrationNO);
                    //  string TPA = obj["TPA"];
                    if (dsPatientPrivilege.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToDateTime(dsPatientPrivilege.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date)// && (TPA. ucPatientOPDBills1.rbTPA.Checked == false))
                        {
                            //  ucPatientOPDBills1.chkPrivilegeCard.Checked = true;
                            //  ucPatientOPDBills1.chkPrivilegeCard.Text = dsPatientPrivilege.Tables[0].Rows[0]["CardName"].ToString();
                        }
                    }
                }
                #region Check Discharge IPD Patient
                Boolean FlagForDischarge;
                if (dsPatientOPD.Tables.Count > 5)
                {
                    if (dsPatientOPD.Tables[5].Rows.Count > 0)
                    {
                        FlagForDischarge = false;
                    }
                    else
                    {
                        FlagForDischarge = true;
                    }
                }
                else
                {
                    FlagForDischarge = false;
                }
                #endregion
                if (FlagForDischarge == false && dsPatientOPD.Tables[0].Rows.Count > 0)
                    if (dsPatientOPD.Tables[1].Rows[0]["PreBalance"].ToString() != null && dsPatientOPD.Tables[1].Rows[0]["PreBalance"].ToString() != "")
                    {


                        PreBalance = dsPatientOPD.Tables[1].Rows[0]["PreBalance"].ToString();
                        CurrentPreBalance = PreBalance;
                        if (Convert.ToDecimal(PreBalance) < 0)
                        {

                            // PreBalance = PreBalance.Replace("-", "");

                        }
                        else
                        {
                            PreBalance = dsPatientOPD.Tables[1].Rows[0]["PreBalance"].ToString();
                            double GrossAmount = Convert.ToDouble(GTotal);
                            double ServiceTax = Convert.ToDouble(STax);
                            double TaxAmount = 0.00;
                            if (ServiceTax > 0)
                            {
                                TaxAmount = GrossAmount * (ServiceTax / 100);
                            }
                            SerTaxAmount = TaxAmount.ToString();
                            double TAmt = GrossAmount + TaxAmount;
                            if (balance != "")
                            {
                                TAmt = TAmt + Convert.ToDouble(PreBalance);
                            }
                            TAmount = TAmt.ToString();
                            double Discount = Convert.ToDouble(DisPercent);
                            double DisAmount = Convert.ToDouble(DiscountAmount);
                            NetAmtPayable = Convert.ToDouble(TAmt) - Convert.ToDouble(DisAmount);
                            BalAmount = Convert.ToDouble(NetAmtPayable) - Convert.ToDouble(PaidAmount);
                        }
                    }
                    else
                    {
                    }
                else
                {
                    TempData["message"] = "Registration Number not Found";
                    return Json("PatientOPDBill");
                    //RedirectToAction("PatientOPDBill", "PatientOPDBill");                    
                }
                string DepartID = dsPatientOPD.Tables[0].Rows[0]["DepartmentID"].ToString();
                if (DepartID == "" || DepartID == "0")
                {
                    DepartName = "";
                }
                else
                {
                    DepartName = dsPatientOPD.Tables[0].Rows[0]["DepartmentName"].ToString();
                }
                DateTime DOB = Convert.ToDateTime(dsPatientOPD.Tables[0].Rows[0]["DateofBirth"]);
                DateBirth = DOB;
                DateTime dtToday = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime dtBeforeDate = new System.DateTime(DOB.Year, DOB.Month, DOB.Day, 0, 0, 0);
                int Year = 0;
                int Month = 0;
#pragma warning disable CS0219 // The variable 'Day' is assigned but its value is never used
                int Day = 0;
#pragma warning restore CS0219 // The variable 'Day' is assigned but its value is never used
                dtBeforeDate = dtBeforeDate.AddYears(1);
                while (dtToday >= dtBeforeDate)
                {
                    dtBeforeDate = dtBeforeDate.AddYears(1);
                    Year++;
                }
                dtBeforeDate = new System.DateTime(DOB.Year, DOB.Month, DOB.Day, 0, 0, 0);
                dtBeforeDate = dtBeforeDate.AddMonths(1);
                while (dtToday >= dtBeforeDate)
                {
                    dtBeforeDate = dtBeforeDate.AddMonths(1);
                    Month++;
                }
                dtBeforeDate = new System.DateTime(DOB.Year, DOB.Month, DOB.Day, 0, 0, 0);
                dtBeforeDate = dtBeforeDate.AddMonths(Month);
                TimeSpan diffResult = dtToday.Date - dtBeforeDate.Date;

                int TotalDay = Convert.ToInt32(diffResult.TotalDays);
                int TotalMonth = Month % 12;

                TDays = TotalDay.ToString();
                TMonth = TotalMonth.ToString();
                TYear = Year.ToString();
                TPA = dsPatientOPD.Tables[0].Rows[0]["TPA_ID"].ToString();
                if (TPA == "0" || TPA == "")
                {
                    TPA = "";
                }
                else
                {
                    DataSet DataSetTpa = new DataSet();
                    DataSetTpa = _OPDBill.TPA(Convert.ToInt32(TPA));
                    TPAid = DataSetTpa.Tables[0].Rows[0]["OrganizationID"].ToString();
                    TPAname = DataSetTpa.Tables[0].Rows[0]["Organizationname"].ToString();
                }
                RefferDoctorID = dsPatientOPD.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();

                TempData["RefferDoctorID"] = RefferDoctorID;
                TempData["DoctorID"] = dsPatientOPD.Tables[0].Rows[0]["DoctorID"].ToString();
                //TempData["DoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["DoctorPrintName"].ToString();
              //  TempData["RDoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["RDoctorPrintName"].ToString();


                //   _OPDBill.GetConsDoctor(dsPatientOPD.Tables[0].Rows[0]["DoctorID"].ToString()).Tables[0].Rows[0]["DoctorPrintName"].ToString();

                {
                    //Gender = dsPatientOPD.Tables[0].Rows[0]["Gender"].ToString();

                    foreach (DataRow dr in dsPatientOPD.Tables[0].Rows)
                    {
                        searchList.Add(new OPDBill
                        {

                            PatientRegNO = Convert.ToInt32(dr["PatientRegNO"]),
                            PatientOPDNo = Convert.ToInt32(dr["OPDIPDID"]),
                            PrintOPDNo = Convert.ToInt32(dr["PrintOPDIPDID"]),
                            PatientName = dr["PatientName"].ToString(),
                            GuardianName = dr["GuardianName"].ToString(),
                            Address = dr["Address"].ToString(),
                            TPA_ID = Convert.ToInt32(TPAid),
                            TPA_Name = TPAname,
                            //   TPA_ID = Convert.ToInt32(dr["TPA_ID"]),
                            MobileNo = dr["MobileNo"].ToString(),
                            //DoctorPrintName = dr["DoctorPrintName"].ToString(),
                            DoctorID = Convert.ToInt32(dr["DoctorID"]),

                            Gender = dr["Gender"].ToString(),
                            PatientType = dr["PatientType"].ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateofBirth"]).ToString("yyyy-MM-dd"),
                            TotalDay = TDays,
                            TotalMonth = TMonth,
                            Year = TYear,
                            DepartmentName = DepartName,
                           // RDoctorPrintName = dr["RDoctorPrintName"].ToString(),
                            RDoctorID = RefferDoctorID,
                            BloodGroup = dr["BloodGroup"].ToString(),
                            PreBalance = Convert.ToDecimal(PreBalance),
                            TaxAmount = Convert.ToDecimal(SerTaxAmount),
                            NetPayableAmount = Convert.ToDecimal(NetAmtPayable),
                            BalanceAmount = Convert.ToDecimal(BalAmount),
                            // TotalAmount = Convert.ToDecimal(TAmount)
                        });
                    }


                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult OldBillNo_IndexChange(int BillNO, OPDBill obj)
        {
            Buisness_Logic.Other.BL_OtherBilling Bl_obj = new Buisness_Logic.Other.BL_OtherBilling();
            HospitlLocationID();
            string Mode = "Add";
            if (BillNO > 0)
            {
                if (Mode == "Add")
                {
                    dsPatientBills.Reset();
                    dsPatientBills = Bl_obj.GetOtherBills(BillNO, HospitalID, LocationID);

                    if (dsPatientBills.Tables.Count > 0)
                       {
                        obj.ForAuthorization = "0";
                        //if (dsPatientBills.Tables[0].Rows.Count > 0)
                        //{
                        //    string ForAuthorization = dsPatientBills.Tables[0].Rows[0]["ForAuthorization"].ToString();
                        //    if (ForAuthorization == "1")
                        //    {
                        //        obj.ForAuthorization = ForAuthorization.ToString();
                        //    }
                      //  }
                        obj.PaymentType = dsPatientBills.Tables[0].Rows[0]["PaymentType"].ToString();
                        switch (obj.PaymentType)
                        {
                            case "Cheque":
                                obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                obj.Date = dsPatientBills.Tables[0].Rows[0]["Date"].ToString();
                                obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                break;

                            case "Credit Card":
                                obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                obj.Date = dsPatientBills.Tables[0].Rows[0]["Date"].ToString();
                                obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                break;

                            case "Debit Card":
                                obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                obj.Date = dsPatientBills.Tables[0].Rows[0]["Date"].ToString();
                                obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                break;


                            case "E-Money":
                                obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                obj.Date = dsPatientBills.Tables[0].Rows[0]["Date"].ToString();
                                obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                break;

                            case "EFT":
                                obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                obj.Date = dsPatientBills.Tables[0].Rows[0]["Date"].ToString();
                                obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                break;

                            default:
                                obj.Number = "Cash";
                                obj.Name = "Cash";
                                obj.Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                                obj.Remarks = "Cash";
                                break;
                        }

                    }
                    if (dsPatientBills.Tables[0].Rows.Count > 0)
                    {
                        obj.BillDate = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["BillDate"]).ToString("yyyy-MM-dd");
                        //time = dsPatientBills.Tables[0].Rows[0]["BillDate"].ToString();  
                        obj.grosstotal = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["GrossAmount"]);
                        obj.TaxPercent = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["TaxPercent"]);
                        obj.SerTaxAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["TaxAmount"]);
                        obj.TotalAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["TotalAmount"]);
                        obj.DiscountPercent = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["DiscountPercent"]);
                        obj.DiscountAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["DiscountAmount"]);
                        obj.DiscountReason = dsPatientBills.Tables[0].Rows[0]["DiscountReason"].ToString();
                        //if (disID != "0")
                        //{
                        //    DataSet ds = _OPDBill.GetDiscountReasonID(Convert.ToString(disID));
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        obj.DiscountReason = ds.Tables[0].Rows[0]["DiscountReason"].ToString();
                        //    }
                        //}
                        //else
                        //{
                        //    obj.DiscountReason = dsPatientBills.Tables[0].Rows[0]["DiscountReason"].ToString();
                        //}
                        if (dsPatientBills.Tables[0].Rows.Count > 0)
                        {
                            if (dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"].ToString() != null && dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"].ToString().Trim() != "")
                            {
                                obj.PreBalance = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"]);

                                if (Convert.ToDecimal(PreBalance) > 0)
                                    obj.PreBalance = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"]);
                            }
                            else
                            {
                                obj.PreBalance = Convert.ToDecimal("0.00");
                            }
                        }
                    }
                    obj.NetPayableAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["NetPayableAmount"]);
                    obj.PaidAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["PaidAmount"]);
                    obj.BalanceAmount = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["BalanceAmount"]);
                    obj.BillNo = BillNO;
                }

            }
            List<OPDBill> searchlist = new List<OPDBill>();
            searchlist.Add(obj);
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetOldBill(int PatientOPDNo, int PatientRegNO, string BillType)
        {
            Buisness_Logic.Other.BL_OtherBilling Bl_obj = new Buisness_Logic.Other.BL_OtherBilling();
            OPDBill obj = new Models.Patient.OPDBill();
            HospitlLocationID();
            DataSet dsPatientBillNo = new DataSet();
            List<OPDBill> searchlist = new List<OPDBill>();
            dsPatientBillNo.Reset();
            int PatientOPDIPDNO = PatientOPDNo;
            int LastBillNo = 0;
            dsPatientBillNo = Bl_obj.GetPatientOLDBillsNO(PatientOPDNo, PatientRegNO, BillType, HospitalID, LocationID);

            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new OPDBill
                    {
                        BillNoDate = dr["BillNo&Date"].ToString(),
                        BillNo = Convert.ToInt32(dr["BillNo"])

                    });
                }

                LastBillNo = Convert.ToInt32(dsPatientBillNo.Tables[0].Rows[dsPatientBillNo.Tables[0].Rows.Count - 1]["BillNo"]);
                dsPatientBills.Reset();
                dsPatientBills = _OPDBill.GetPatientBills(LastBillNo, HospitalID, LocationID);

                //if(dsPatientBills.Tables[0].Rows.Count>0)
                //{
                //    obj.BalencePre = Convert.ToDecimal(dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"]);
                //    searchlist.Add(obj);
                //}
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetBill(int BillNo)
        {
            Buisness_Logic.Other.BL_OtherBilling Bl_obj = new Buisness_Logic.Other.BL_OtherBilling();
            HospitlLocationID();
            dsPatientBillsDetails.Reset();

          //  bool AurthoChk = _OPDBill.ForAuthorizationGetMasterSetting();
            //ForAuthorization
          //  if (AurthoChk)
          //  {
                dsPatientBills = Bl_obj.GetOtherBills(BillNo, HospitalID, LocationID);

             //   if (dsPatientBills.Tables.Count > 0)
                //{
                //    if (dsPatientBills.Tables[0].Rows.Count > 0)
                //    {
                //        ForAuthorization = dsPatientBills.Tables[0].Rows[0]["ForAuthorization"].ToString();
                //    }
                //}
                //if (ForAuthorization == "")
                //{
                //    ForAuthorization = "0";
                //}

                //if (ForAuthorization == "1")
                //{
                //    dsPatientBillsDetails = _OPDBill.GetPatientBillsDetailsAurthorise(BillNo, HospitalID, LocationID);
                //}
                //else
                //{
                    dsPatientBillsDetails = Bl_obj.GetOtherBillsDetails(BillNo, HospitalID, LocationID);
            //    }
            //}
            //else
            //{
            //    //end
            //    BL_PatientLabBills BL_Lab = new BL_PatientLabBills();

            //    dsPatientBillsDetails = _OPDBill.GetPatientBillsDetails(BillNo, HospitalID, LocationID);
            //}

            List<OPDBill> searchlist = new List<OPDBill>();
            foreach (DataRow dr in dsPatientBillsDetails.Tables[0].Rows)
            {
                //DataSet ds = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, BillNo);

                //string ServiceID = dr["ServiceID"].ToString();
                //DataView dv1 = new DataView(ds.Tables[0], "TestID = " + ServiceID + "", "", DataViewRowState.CurrentRows);
                //if (dv1.ToTable().Rows.Count > 0)
                //{
                //    obj1.ServiceType = "OPDLabBills";
                //}
                //else
                //{
                //    obj1.ServiceType = "OPD";
                //  }
                searchlist.Add(new OPDBill
                {
                    DiscountServiceType = dr["DiscountServiceType"].ToString(),
                    Discount_Service = dr["Discount_Service"].ToString(),
                    servicename = dr["ServiceName"].ToString(),
                    ServiceID = dr["ServiceID"].ToString(),
                    ServiceType = dr["ServiceType"].ToString(),
                    UnitID = Convert.ToInt32(dr["UnitID"]),
                    Doctor = Convert.ToInt32(dr["DoctorID"]),
                    Rate = dr["Rate"].ToString(),
                    Quantity = dr["Quantity"].ToString(),
                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                  //  DoctorCharges = dr["DoctorCharges"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult Delete(int PatientRegNO, int BillNo)
        {
            string del = "";
            Buisness_Logic.Other.BL_OtherBilling Bl_obj = new Buisness_Logic.Other.BL_OtherBilling();
            if (Bl_obj.DeleteOtherBill(PatientRegNO, BillNo))
            {
                del = "Delete";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        }
    }
}