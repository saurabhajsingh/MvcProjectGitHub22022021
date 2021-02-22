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
using KeystoneProject.Models.Keystone;

namespace KeystoneProject.Controllers.Patient
{
    public class OPDBillController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;
        private SqlConnection con;

        List<OPDBill> searchList = new List<OPDBill>();

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

      //OPDBill obj1 = new OPDBill();

        KeystoneProject.Models.Patient.OPDBill obj1 = new OPDBill();
        [HttpGet]
      
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        //
        // GET: /OPDBill/
        public ActionResult OPDBill()
        {
            return View();
        }

        public JsonResult OldBillReport( string BillNo)
        {
            HospitlLocationID();
            
                Session["HospitalIDReport"] = HospitalID;
                Session["LocationIDReport"] = LocationID;
                Session["BillNoReport"] = BillNo.ToString();
                Session["rptReport"] = "";
            
            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult OPDBill(OPDBill obj, FormCollection fc)       
        {
            HospitlLocationID();
            // FormCollection fc = new FormCollection();
            //   OPDBill obj = new OPDBill();
            string Mode = "Add";
            try
            {
                //string hh = TempData["Reg"].ToString();

                rpt = Request.Form["rpt"];
                obj.ChargesType = Request.Form["RadioCharge"];
                obj.PaymentType = Request.Form["payment_type"];

                obj.BillDate = fc["bill_date"] + " " + fc["bill_time"];
              
                //obj.BillDate = Convert.ToDateTime(fc["bill_date"]).ToString("dd-MMM-yyyy");// +" " + fc["bill_time"];
                // obj.Gender = Request.Form["gen"].ToString();
                obj.PreBalance = Convert.ToDecimal(fc["PreBalanceandDiposit"]);
                if (fc["OldBillNo"] == "" || fc["OldBillNo"] == null)
                {
                    obj.BillNoId = 0;
                }
                else
                {
                    obj.BillNoId =Convert.ToInt32(fc["OldBillNo"]);
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

                //if (fc["doctorID"] == null)
                //{
                //    obj.DoctorID = 0;
                //}
                //else
                //{
                //    obj.DoctorID = Convert.ToInt32(fc["doctorID"]);
                //}





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
               // obj.BillDate = Convert.ToDateTime(fc["bill_date"]).ToString("dd-MMM-yyyy");// +" " + fc["bill_time"];
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
                        OPDBill OPDbill = _OPDBill.Save1(obj);
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
                                //ds.Tables[0].Rows[0]["PreBalance"] = 0;
                                return RedirectToAction("RptPatientOPDBills", "PatientReport");
                            }



                            TempData["Msg"] = "Record Saved Sussefully";
                            return RedirectToAction("OPDBill", "OPDBill");
                        }
                        else
                        {
                            TempData["Msg"] = OPDbill.MessageError;
                            return RedirectToAction("OPDBill", "OPDBill");
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
                return RedirectToAction("OPDBill", "OPDBill");
            }
            return View();
        }

       
        //public ActionResult Delete(int PatientRegNO, int BillNo,string txtinput)
        //{
        //    string del = "";
        //    BL_PatientOPDBill obj = new BL_PatientOPDBill();
        //    PatientOPDBill patientopdbill = new PatientOPDBill();
        //    if (obj.DeletePatientBills(PatientRegNO, BillNo, txtinput))
        //    {
        //        del = "Record Deleted Successfully";
        //    }
        //        return Json(del);
        //}      

        string rpt = "";
        [HttpPost]
        public ActionResult Delete(int PatientRegNO, int BillNo, string txtinput)
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
            cmd.Parameters.AddWithValue("@CreationID", CreationID);
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
                cmdIUForAuthorization.Parameters.AddWithValue("@UserID", CreationID);
                cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", txtinput);
                cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", BillNo);
                cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "IPDBill");
                cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", CreationID);
                cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                con.Open();
                cmdIUForAuthorization.ExecuteNonQuery();

            }
            return Json(del, JsonRequestBehavior.AllowGet);

            // return RedirectToAction("IPDBill", "IPDBill");
        }
        BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
        public void AuthoriseSave(int BillNo)
        {

            BL_PatientOPDBill BL_obj = new BL_PatientOPDBill();
            DataSet dsPatientOldBills = new DataSet();


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
                cmdBillModify.Parameters.AddWithValue("@CreationID", CreationID);
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
                cmdBillModifyDetail.Parameters.AddWithValue("@CreationID", CreationID);
                cmdBillModifyDetail.Parameters.AddWithValue("@Mode", "Add");
             
                int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();
              
            }
        }

        public static Nullable<DateTime> DateBirth
        {
            get;
            set;
        }

        public JsonResult GetPatientForOPDNoBills(string IPDNo)
        {
          
            BL_PatientOPDBill db = new BL_PatientOPDBill();
            return new JsonResult { Data = db.GetPatientForIPDNoBills(IPDNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult OldBillNo_IndexChange(int BillNO, OPDBill obj)
        {
            List<OPDBill> searchlist = new List<OPDBill>();
            HospitlLocationID();
            string Mode = "Add";
            if (BillNO > 0)
            {
                if (Mode == "Add")
                {
                    dsPatientBills.Reset();
                    dsPatientBills = _OPDBill.GetPatientBills(BillNO, HospitalID, LocationID);

                    for (int i = 0; i < dsPatientBills.Tables[0].Rows.Count; i++)
                    {
                      if (dsPatientBills.Tables.Count > 0)
                        {
                            obj.ForAuthorization = "0";
                            if (dsPatientBills.Tables[0].Rows.Count > 0)
                            {

                                   DataTable dt1 = new DataTable();

                                
                                string ForAuthorization = dsPatientBills.Tables[0].Rows[0]["ForAuthorization"].ToString();

                                if (ForAuthorization == "1")
                                {
                                    Connect();
                                    DataSet ds = new DataSet();
                                    DataTable dt = new DataTable();


                                    SqlCommand cmd1 = new SqlCommand("GetPatientBillsForAuthorization", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                                    cmd1.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                                    cmd1.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNO)));
                                    //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                                    SqlDataAdapter sd1 = new SqlDataAdapter();
                                    sd1.SelectCommand = cmd1;

                                    dsPatientBills.Reset();
                                    con.Open();
                                    sd1.Fill(dsPatientBills);

                                    //sd1.Fill(dt);

                                    // obj.ForAuthorization = ForAuthorization.ToString();
                                    }
                                Connect();
                                SqlCommand cmd2 = new SqlCommand();
                                if (ForAuthorization == "0" || ForAuthorization == "" || ForAuthorization == null)
                                {
                                    cmd2 = new SqlCommand("GetPatientBillsDetails", con);
                                }
                                else
                                {
                                    if (ForAuthorization == "1")
                                    {
                                        cmd2 = new SqlCommand("GetPatientBillsDetailsAurthorise", con);
                                    }

                                }

                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                                cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                                cmd2.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNO)));
                                //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                                SqlDataAdapter sd2 = new SqlDataAdapter();
                                sd2.SelectCommand = cmd2;
                                DataSet ds1 = new DataSet();


                                sd2.Fill(ds1);

                                //sd2.Fill(dt1);
                            }
                          
                            obj.PaymentType = dsPatientBills.Tables[0].Rows[0]["PaymentType"].ToString();
                            switch (obj.PaymentType)
                            {
                                case "Cheque":
                                    obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                    obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                    obj.Date = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                                    obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                    break;

                                case "Credit Card":
                                    obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                    obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                    obj.Date = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                                    obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                    break;

                                case "Debit Card":
                                    obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                    obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                    obj.Date = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                                    obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                    break;


                                case "E-Money":
                                    obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                    obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                    obj.Date = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
                                    obj.Remarks = dsPatientBills.Tables[0].Rows[0]["Remarks"].ToString();
                                    break;

                                case "EFT":
                                    obj.Number = dsPatientBills.Tables[0].Rows[0]["Number"].ToString();
                                    obj.Name = dsPatientBills.Tables[0].Rows[0]["Name"].ToString();
                                    obj.Date = Convert.ToDateTime(dsPatientBills.Tables[0].Rows[0]["Date"]).ToString("yyyy-MM-dd");
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

                        DataRow dr = dsPatientBills.Tables[0].Rows[i];

                        string DateTimestr = Convert.ToString(dr["BillDate"]);
                        DateTime DateTime = Convert.ToDateTime(DateTimestr);

                        if (dsPatientBills.Tables[0].Rows.Count > 0)
                        {
                            obj.ForAuthorization = dsPatientBills.Tables[0].Rows[0]["ForAuthorization"].ToString();
                            obj.BillNoPrint = Convert.ToString(dr["PrintBillNo"]);
                            obj.BillDate = DateTime.ToString("yyyy-MM-dd");
                            obj.Time = DateTime.ToString("hh:mm");
                            obj.grosstotal = Convert.ToDecimal(dr["GrossAmount"]);
                            obj.TaxPercent = Convert.ToDecimal(dr["TaxPercent"]);
                            obj.SerTaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                            obj.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                            obj.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                            obj.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            obj.DiscountReason = dr["DiscountReason"].ToString();

                           
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
                                    obj.PreBalance = Convert.ToDecimal(dr["PreBalanceAmount"]);

                                    if (Convert.ToDecimal(PreBalance) > 0)
                                        obj.PreBalance = Convert.ToDecimal(dr["PreBalanceAmount"]);
                                }
                                else
                                {
                                    obj.PreBalance = Convert.ToDecimal("0.00");
                                }
                            }
                        }
                        obj.NetPayableAmount = Convert.ToDecimal(dr["NetPayableAmount"]);
                        obj.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                        obj.BalanceAmount = Convert.ToDecimal(dr["BalanceAmount"]);
                        obj.BillNo = BillNO;

                    }
                }

            }
           
            searchlist.Add(obj);
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBill(int BillNo,string ForAuthorization)
        {
            HospitlLocationID();
            dsPatientBillsDetails.Reset();

           bool AurthoChk= _OPDBill.ForAuthorizationGetMasterSetting();
            //ForAuthorization
            if (AurthoChk)
            {
                dsPatientBills = _OPDBill.GetPatientBills(BillNo, HospitalID, LocationID);
                
                if (dsPatientBills.Tables.Count > 0)
                {
                    if (dsPatientBills.Tables[0].Rows.Count > 0)
                    {
                        ForAuthorization = dsPatientBills.Tables[0].Rows[0]["ForAuthorization"].ToString();
                    }
                }
                if (ForAuthorization == "")
                {
                    ForAuthorization = "0";
                }

                if (ForAuthorization == "1")
                {
                    dsPatientBillsDetails = _OPDBill.GetPatientBillsDetailsAurthorise(BillNo, HospitalID, LocationID);
                }
                else
                {
                    dsPatientBillsDetails = _OPDBill.GetPatientBillsDetails(BillNo, HospitalID, LocationID);
                }
            }
            else
            { 
            //end
            BL_PatientLabBills BL_Lab = new BL_PatientLabBills();
            
                dsPatientBillsDetails = _OPDBill.GetPatientBillsDetails(BillNo, HospitalID, LocationID);
            }
           
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
                    DoctorCharges = dr["DoctorCharges"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetOldBill(int PatientOPDNo, int PatientRegNO, string BillType)
        {

            OPDBill obj = new Models.Patient.OPDBill();
            HospitlLocationID();
            DataSet dsPatientBillNo = new DataSet();
            List<OPDBill> searchlist = new List<OPDBill>();
            dsPatientBillNo.Reset();
            int PatientOPDIPDNO = PatientOPDNo;
            int LastBillNo = 0;
            dsPatientBillNo = _OPDBill.GetPatientOLDBillsNO(PatientOPDNo, PatientRegNO, BillType, HospitalID, LocationID);

            if (dsPatientBillNo.Tables[0].Rows.Count > 0)
            {
               
                foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                {
                    searchlist.Add(new OPDBill
                    {
                        BillNoDate = dr["BillNo&Date"].ToString(),
                        BillNo = Convert.ToInt32(dr["BillNo"]),
                        BillNoPrint=dr["P_BillNo"].ToString()
                        
                        
                    });
                }

                LastBillNo   = Convert.ToInt32(dsPatientBillNo.Tables[0].Rows[dsPatientBillNo.Tables[0].Rows.Count-1]["BillNo"]);
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
        public JsonResult Add(int ServiceID, string DName, string SName, decimal Rate, decimal Quantity, string Unit, string Charges,
            string DoctorID, string UpDoctorID, decimal TAmt, decimal NetPayableAmount, decimal BalanceAmt, decimal GTotal, decimal TotalAmt1)
        {
            HospitlLocationID();
            GetEmptyDataSetPatientBillsDetails();
            DataSet dsServiceName = _OPDBill.Services();
            DataView dv = new DataView(dsServiceName.Tables[0], " ServiceID = " + ServiceID + " ", "", DataViewRowState.CurrentRows);
            DataView dv1 = new DataView(dsPatientBillsDetails.Tables[0], " ServiceID = " + ServiceID + " ", "", DataViewRowState.CurrentRows);
            DataRow dr = dsPatientBillsDetails.Tables[0].NewRow();
            dr["HospitalID"] = HospitalID;
            dr["LocationID"] = LocationID;
            dr["ServiceID"] = ServiceID;
            if (DName == "")
            {
                dr["ServiceName"] = SName;
            }
            else
            {
                dr["ServiceName"] = SName + " " + DName;
            }
            Decimal Price = Convert.ToDecimal(Rate);
            Decimal Qty = Convert.ToDecimal(Quantity);
            if (Unit == "")
            {
                dr["UnitID"] = 1;
            }
            else
            {
                dr["UnitID"] = Unit;
            }
            if (Charges == "EmergencyCharges")
            {
                dr["ChargesType"] = "Emergency";
            }
            else if (Charges == "TPACharges")
            {
                dr["ChargesType"] = "TPA";
            }
            else
            {
                dr["ChargesType"] = "General";
            }
            dr["Rate"] = Rate;
            dr["Quantity"] = Quantity;
            DataSet dsDoctorData = new DataSet();
            dsDoctorData.Reset();
          //  dsDoctorData = _OPDBill.GetServicesDetailsforOPDBills(ServiceID);
            dr["ReffCommission"] = 0.00;
            dr["Commisson"] = 0.00;
            dr["ServiceType"] = "OPD";
            dr["TotalAmount"] = Math.Round(Price * Quantity, 2);
            dr["HideInBilling"] = false;
            dr["DoctorID"] = DoctorID;
            //if (DName == "")
            //{
            //    dr["DoctorID"] = UpDoctorID;
            //}
            //else
            //{
            //    dr["DoctorID"] = DoctorID;
            //}
            dr["CreationID"] = iUserID;
            dr["Mode"] = "Add";
            dsPatientBillsDetails.Tables[0].Rows.Add(dr);
            List<OPDBill> searchList = new List<OPDBill>();
            foreach (DataRow dr1 in dsPatientBillsDetails.Tables[0].Rows)
            {
                searchList.Add(new OPDBill
                {
                    HospitalID = Convert.ToInt32(dr1["HospitalID"]),
                    LocationID = Convert.ToInt32(dr1["LocationID"]),
                    ServiceID = Convert.ToInt32(dr1["ServiceID"]).ToString(),
                    servicename = dr1["ServiceName"].ToString(),
                    UnitID = Convert.ToInt32(dr1["UnitID"]),
                    ChargesType = dr1["ChargesType"].ToString(),
                    Rate = dr1["Rate"].ToString(),
                    Quantity = dr1["Quantity"].ToString(),
                    Commisson = Convert.ToDecimal(dr1["Commisson"]),
                    ReffCommission = Convert.ToDecimal(dr1["ReffCommission"]),
                    ServiceType = dr1["ServiceType"].ToString(),
                    TotalAmount1 = dr1["TotalAmount"].ToString(),
                    HideInBilling = dr1["HideInBilling"].ToString(),
                    Doctor = Convert.ToInt32(dr1["DoctorID"]),
                    CreationID = Convert.ToInt32(dr1["CreationID"]),
                    Mode = dr1["Mode"].ToString(),
                    TotalAmount = (TAmt + Convert.ToDecimal(dr1["TotalAmount"])),
                    NetPayableAmount = (NetPayableAmount + Convert.ToDecimal(dr1["TotalAmount"])),
                    BalanceAmount = (BalanceAmt + Convert.ToDecimal(dr1["TotalAmount"])),
                    grosstotal = (GTotal + Convert.ToDecimal(dr1["TotalAmount"]))
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetServiceRecord(string prefix, string id, string chk, string HSNCode ,string charges)
            {
            List<OPDBill> searchList = new List<OPDBill>();
            if (chk == "False" || chk == null)
            {
                if (id == "0" || id == null)
                {
                    id = "";

                }
                if (prefix == "0" || prefix == null)
                {
                    prefix = "";

                }
                DataSet ds = _OPDBill.GetServiceGrp(prefix, id, HSNCode);
                DataSet dsGen = _OPDBill.GetServiceGrpGen(prefix, id, HSNCode);
                DataSet dsTPA = _OPDBill.GetServiceGrpTPA(prefix, id, HSNCode);
                if (charges == "EmergencyCharges")
                {


                    if (HSNCode != null)
                    {
                        DataView dv = new DataView(ds.Tables[0], "HSNCode like '" + HSNCode + "%'", "", DataViewRowState.CurrentRows);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            searchList.Add(new OPDBill
                            {
                                servicename = dv.ToTable().Rows[0]["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceID"]).ToString(),
                                GeneralCharges = dv.ToTable().Rows[0]["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dv.ToTable().Rows[0]["EmergencyCharges"]).ToString()


                            });
                        }
                    }
                    else
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {


                            searchList.Add(new OPDBill
                            {
                                servicename = dr["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                            });
                        }
                    }
                }
                else if (charges == "GeneralCharges")
                {
                    if (HSNCode != null)
                    {
                        DataView dv = new DataView(dsGen.Tables[0], "HSNCode like '" + HSNCode + "%'", "", DataViewRowState.CurrentRows);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            searchList.Add(new OPDBill
                            {
                                servicename = dv.ToTable().Rows[0]["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceID"]).ToString(),
                                GeneralCharges = dv.ToTable().Rows[0]["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dv.ToTable().Rows[0]["EmergencyCharges"]).ToString()


                            });
                        }
                    }
                    else
                    {

                        foreach (DataRow dr in dsGen.Tables[0].Rows)
                        {


                            searchList.Add(new OPDBill
                            {
                                servicename = dr["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                            });
                        }
                    }

                }
                else
                {
                    if (HSNCode != null)
                    {
                        DataView dv = new DataView(dsTPA.Tables[0], "HSNCode like '" + HSNCode + "%'", "", DataViewRowState.CurrentRows);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            searchList.Add(new OPDBill
                            {
                                servicename = dv.ToTable().Rows[0]["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dv.ToTable().Rows[0]["ServiceID"]).ToString(),
                                GeneralCharges = dv.ToTable().Rows[0]["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dv.ToTable().Rows[0]["EmergencyCharges"]).ToString()


                            });
                        }
                    }
                    else
                    {

                        foreach (DataRow dr in dsTPA.Tables[0].Rows)
                        {


                            searchList.Add(new OPDBill
                            {
                                servicename = dr["servicename"].ToString(),
                                ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                            });
                        }
                    }

                }
            }
            else
            {

                DataSet ds = new DataSet();
                ds = _OPDBill.GetTest(prefix);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        searchList.Add(new OPDBill
                        {
                            servicename = dr["servicename"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                        });
                    }
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #region Bind_Service_TPA
        public JsonResult GetServiceNameTPA(string prefix, string id, string chk,  int OrganizationID)
        {

            string Unit = "";
            string UnitName = "";

       
           // BL_PatientOPDBill objtest = new BL_PatientOPDBill();
          
            OPDBill add = new OPDBill();
          
            List<OPDBill> searchList = new List<OPDBill>();
           
          
            string GeneralCharges = "";
            string EmergencyCharges = "";

            string ServiceGroupName = "";
            DataSet ds = new DataSet();
           
            if (chk == "Service")
            {

                ds = _OPDBill.GetServiceDetailByNameTPA(prefix, "%", OrganizationID);
               
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["servicename"].ToString() != "")
                    {
                        searchList.Add(new OPDBill
                        {
                            ServiceGroupName = dr["ServiceGroupName"].ToString(),
                            servicename = dr["serviceName"].ToString(),
                            ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"]),
                            ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            EmergencyCharges = dr["EmergencyCharges"].ToString(),
                            ServiceHSNCode = dr["TPAHSNCode"].ToString(),
                            Unit = Unit,
                            Quantity ="1",
                        });
                    }
                }
            }

            else if(chk=="Test")
            {


                ds = _OPDBill.GetPatientLabBillsForTestMaster(OrganizationID);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {


                        searchList.Add(new OPDBill
                        {

                            servicename = dr["TestName"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            ServiceID = Convert.ToInt32(dr["TestID"]).ToString(),

                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                        });
                    }
                }
            }
            else
            {

                ds = _OPDBill.GetPatientLabBillsForTestMaster(OrganizationID);
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {


                        searchList.Add(new OPDBill
                        {

                            servicename = dr["TestName"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            ServiceID = Convert.ToInt32(dr["TestID"]).ToString(),

                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                        });
                    }
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        #endregion

        //public JsonResult GetServicenameTPA(string prefix, int OrganizationID)
        //{
        //    DataSet ds = new DataSet();
        //    BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
        //    List<OPDBill> searchList = new List<OPDBill>();
        //    ds = obj.GetServiceDetailByNameTPA(prefix,"", OrganizationID);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        searchList.Add(new OPDBill
        //        {
        //            servicename = dr["servicename"].ToString(),
        //            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
        //            ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
        //            GeneralCharges = dr["GeneralCharges"].ToString(),
        //            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
        //            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

        //        });
        //    }
        //    return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}


        #region Bind_Service_And_Test
        public JsonResult GetServiceName(string prefix, string id, string chk, string HSNCode, string charges,string RegNo, string OPDNo, string BillDate)
        {
        
          //  BL_PatientOPDBill objtest = new BL_PatientOPDBill();
          
            OPDBill add = new OPDBill();
           // BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
           
          
            string GeneralCharges = "";
            string EmergencyCharges = "";
            string Unit = "";
            string UnitName = "";
            string ServiceGroupName = "";
            DataSet ds = new DataSet();
            if (chk == "Service" || chk == "Package")
            {
                if (HSNCode != null)
                {
                    ds = _OPDBill.GetServiceDetailByCode(HSNCode);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ServiceGroupName = ds.Tables[0].Rows[0]["ServiceGroupName"].ToString();
                    }
                }
                else
                {
                    ds = _OPDBill.GetServiceDetailByName(prefix, id, chk);
                }

               


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                  
                    searchList.Add(new OPDBill
                    {
                        ServiceHSNCode = dr["HSNCode"].ToString(),
                        ServiceGroupName = dr["ServiceGroupName"].ToString(),
                        servicename = dr["servicename"].ToString(),
                        ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                        ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges =EmergencyCharges,
                        Unit =UnitName,
                        UnitID = Convert.ToInt16(dr["UnitID"]),
                        BillCharges = dr["BillCharges"].ToString(),
                        Quantity = "1",
                     
                        
                    });

                    Decimal Discount = DiscountPreVilege(dr, Convert.ToInt32(RegNo), chk);
                    if (Discount > 0)
                    {
                        if (Convert.ToBoolean(TempData["PrivalageCard"]) == true)
                        {
                            Decimal DWithRate = Convert.ToDecimal(searchList[0].GeneralCharges);
                            DWithRate = DWithRate - (DWithRate * Discount / 100);
                            searchList[0].servicename = searchList[0].servicename + " ( " + searchList[0].GeneralCharges + ",  " + Math.Round(Discount, 2) + "% )";
                            searchList[0].GeneralCharges = DWithRate.ToString();
                        }
                    }


                }
                if (id == "%")
                {
                    TESTBind(prefix);
                }

            }

            else
            {
                //  DataSet ds = new DataSet();

                ds = _OPDBill.GetTest(id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {


                        searchList.Add(new OPDBill
                        {

                            servicename = dr["servicename"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                            ServiceHSNCode=dr["HSNCode"].ToString(),
                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                        });
                    }
                }
            }
            //-----PrivalageCard Service-------------------------------------------------------------------------------------
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (searchList[0].GeneralCharges == "")
                {
                    searchList[0].GeneralCharges = "0";
                }

               
            }




            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        #endregion
        public void TESTBind(string prefix)
        {
          
            DataSet ds = new DataSet();
            BL_PatientOPDBill objtest = new BL_PatientOPDBill();
            ds = objtest.GetTest("%");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dvServiceGroup = new DataView(ds.Tables[0], "ServiceName like '" + prefix + "%'", "", DataViewRowState.CurrentRows);
                // ds.Tables[0].DefaultView.RowFilter = "ServiceName like 'Karnataka'";
                foreach (DataRow dr in dvServiceGroup.ToTable().Rows)
                {


                    searchList.Add(new OPDBill
                    {
                        ServiceGroupName = "",
                        servicename = dr["servicename"].ToString() + "   ( Lab )",
                        ServiceType = "Lab",
                        ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString(),
                        DoctorID = 0,

                    });
                }

            }
        }
        public decimal DiscountPreVilege(DataRow dr, int RegNo, string ServiceType)
        {
            HospitlLocationID();

            TempData["PrivalageCard"] = false;
            decimal DWithRate = 0;
            decimal Discount = 0;
            bool PrivalageCard = false;
            string PrivName = "";
            DataSet dsPatientPrivilege = new System.Data.DataSet();
            BL_PatientOPDBill objPrivalage = new BL_PatientOPDBill();
            dsPatientPrivilege = objPrivalage.GetPatientPrivilegeDiscount(HospitalID, LocationID, Convert.ToInt32(RegNo));
            //    string TPA = obj["TPA"];
            if (dsPatientPrivilege.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsPatientPrivilege.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date)
                {
                    TempData["PrivalageCard"] = true;
                    PrivalageCard = true;
                    //  ucPatientOPDBills1.chkPrivilegeCard.Checked = true;
                    PrivName = dsPatientPrivilege.Tables[0].Rows[0]["CardName"].ToString();

                }
            }

            // DWithRate = Convert.ToDecimal(Rate);

            if (ServiceType == "Test")
            {
                DataView dvServiceGroup = new DataView(dsPatientPrivilege.Tables[3], " TestID = " + dr["ServiceID"] + "", "", DataViewRowState.CurrentRows);
                if (dvServiceGroup.Count != 0)
                {
                    Discount = Convert.ToDecimal(dvServiceGroup[0]["DiscountPer"].ToString());
                }
                DataView dvTest = new DataView(dsPatientPrivilege.Tables[4], " TestID = " + dr["ServiceID"] + "", "", DataViewRowState.CurrentRows);
                if (dvTest.Count != 0)
                {
                    Discount = Convert.ToDecimal(dvTest[1]["DiscountPer"].ToString());
                }
            }
            else
            {
                DataView dvServiceGroup = new DataView(dsPatientPrivilege.Tables[2], " ServiceID = " + dr["ServiceID"] + "", "", DataViewRowState.CurrentRows);
                if (dvServiceGroup.Count != 0)
                {
                    Discount = Convert.ToDecimal(dvServiceGroup[0]["DiscountPer"].ToString());
                }
                
            }
           
            if (Discount > 0)
            {
                if (Convert.ToBoolean(TempData["PrivalageCard"]) == true)
                {
                    string ser = dr["ServiceID"].ToString();
                    DWithRate = Convert.ToDecimal(dr["GeneralCharges"].ToString());
                    DWithRate = DWithRate - (DWithRate * Discount / 100);
                    dr["servicename"] = dr["servicename"] + "(" + dr["GeneralCharges"] + " of " + Math.Round(Discount, 2) + "% )";
                    dr["GeneralCharges"] = DWithRate.ToString();
                }
            }

            return DWithRate;

        }
        //public JsonResult GetServicename(string prefix, string id, string chk, string HSNCode, string charges, string drID)
        //{

        //    HospitlLocationID();
        //    List<OPDBill> searchList = new List<OPDBill>();
        //    string Rate, Unit, Quantity;
        //    try
        //    {
        //        if (prefix != "")
        //        {
        //            if (prefix == "2")
        //            {
        //                DataSet dsRate = new DataSet();
        //                dsRate = _OPDBill.GetPatientBillsForDoctorDetail(HospitalID, LocationID, Convert.ToInt32(drID));
        //                Rate = dsRate.Tables[0].Rows[0]["CounsultancyFees"].ToString();
        //                Unit = "1";
        //                Quantity = "1";
        //                foreach (DataRow dr in dsRate.Tables[0].Rows)
        //                {
        //                    searchList.Add(new OPDBill
        //                    {
        //                        Rate = dr["CounsultancyFees"].ToString(),
        //                        UnitID = Convert.ToInt32(Unit),
        //                        Quantity = Quantity
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                int ServiceID = Convert.ToInt32(prefix);
        //                int organization = 1;
        //                DataSet ds = new DataSet();
        //                DataSet dsTPA = new DataSet();
        //                string Unit1 = "";
        //                string Rate1 = "";
        //                Quantity = "1.00";
        //                ds = _OPDBill.GetServicesDetailsforOPDBills(ServiceID);
        //                dsTPA = _OPDBill.GetServicesDetailsforOPDBillsBasedOnTPA(ServiceID, organization);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    Unit1 = ds.Tables[0].Rows[0]["UnitID"].ToString();
        //                    Quantity = "1.00";
        //                    RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
        //                    HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
        //                    if (charges == "EmergencyCharges")
        //                    {
        //                        Rate1 = ds.Tables[0].Rows[0]["EmergencyCharges"].ToString();
        //                    }
        //                    else if (charges == "TPACharges")
        //                    {
        //                        if (dsTPA.Tables[0].Rows.Count>0)
        //                        {
        //                            Rate1 = dsTPA.Tables[0].Rows[0]["GeneralCharges"].ToString();

        //                        }
        //                        else
        //                        {
        //                            Rate1 = "0.00";
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Rate1 = ds.Tables[0].Rows[0]["GeneralCharges"].ToString();
        //                    }
        //                }
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    searchList.Add(new OPDBill
        //                    {
        //                        Rate = Rate1.ToString(),
        //                        UnitID = Convert.ToInt32(Unit1),
        //                        Quantity = Quantity
        //                    });
        //                }
        //            }
        //        }
        //        return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public JsonResult GetOPDPatient(string prefix, bool outside)
        {
            DataSet ds = _OPDBill.GetOPDPatient(prefix);
            List<PatientOPDBill> searchList = new List<PatientOPDBill>();
           // if (outside == false)
          //  {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchList.Add(new PatientOPDBill
                    {
                        patientregNo =(dr["PatientRegNO"].ToString()),
                        patientname = dr["PatientName"].ToString(),
                        address =   (dr["Address"].ToString()),
                        contactno = dr["MobileNo"].ToString(),
                        PrintRegNO = dr["PrintRegNO"].ToString()
                    });
                }
            //}
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceDetailsPackage()
        {
            return new JsonResult { Data = _OPDBill.GetServiceDetailsPackage(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDoctorConsRecord(string prefix)
        {
            DataSet ds = _OPDBill.GetConsDoctor(prefix);
            List<OPDBill> searchList = new List<OPDBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new OPDBill
                {
                    DoctorPrintName = dr["DoctorPrintName"].ToString(),
                    DoctorID = Convert.ToInt16(dr["DoctorID"])
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetServicechk(string prefix)
        {
            string _Check = "";
            try
            {
                string DependaincyName = _OPDBill.GetServicechk(prefix);
                if (DependaincyName == "False")
                {
                    _Check = "False";
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Check, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceGrp(string prefix,string type)
        {
            BL_PatientOPDBill db = new BL_PatientOPDBill();


            return new JsonResult { Data = db.GetServiceGrp(prefix,type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public JsonResult GetServiceGrp(string prefix)
        //{
         // DataSet ds = _OPDBill.GetServiceGrpBind(prefix);
        //    List<OPDBill> searchList = new List<OPDBill>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        searchList.Add(new OPDBill
        //        {
        //            ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
        //            ServiceGroupName = dr["ServiceGroupName"].ToString(),
        //            ServiceType = "",
        //        });
        //    }
        //    return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public JsonResult GetDiscountReason(string prefix)
        {
            DataSet ds = _OPDBill.GetDiscountReason(prefix);
            List<OPDBill> searchList = new List<OPDBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new OPDBill
                {
                    DiscountReason = dr["DiscountReason"].ToString(),
                    DiscountReasonID = dr["DiscountReasonID"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public DataSet GetTest(string GetServiceGrp)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select TestID as ServiceID ,upper(TestName) as ServiceName ,GeneralCharges,EmergencyCharges from TestMaster   where TestName like ''+@GetServiceGrp+'%'  and  RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "  order by  ServiceName asc", con);
            cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public JsonResult GetDiscountReasonID(string DisReason)
        {
            if (DisReason == "")
            {
                return new JsonResult();
            }
            else
            {
                List<OPDBill> searchlist = new List<OPDBill>();
                DataSet ds = _OPDBill.GetDiscountReason(DisReason);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        searchlist.Add(new OPDBill
                        {
                            DiscountReason = dr["DiscountReason"].ToString(),
                            DiscountReasonID = dr["DiscountReasonID"].ToString()
                        });
                    }
                }
                else
                {
                    DataSet ds1 = _OPDBill.AddDiscountReason(DisReason);
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        searchlist.Add(new OPDBill
                        {
                            DiscountReason = dr["DiscountReason"].ToString(),
                            DiscountReasonID = dr["DiscountReasonID"].ToString()
                        });
                    }
                }
                return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = _OPDBill.GetAllFinancialYear();
            List<OPDBill> searchList = new List<OPDBill>();

            if (ds.Tables[0].Rows.Count != 0)
            {

               DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                dvTest.ToTable().Rows[0]["FinancialYear"].ToString();

                foreach (DataRow dr in dvTest.ToTable().Rows)
                {
                    searchList.Add(new OPDBill
                    {
                        FinancialYear = dr["FinancialYear"].ToString(),
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"])
                    });
                }
            }
                return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
          
        }
        public JsonResult GetFinancialYearID(string prefix)
        {
            DataSet ds = _OPDBill.GetFinancialYearID(prefix);
            List<OPDBill> searchlist = new List<OPDBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new OPDBill
                {
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"])
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult PatientRegNO_Leave(int RegistrationNO, int PatientOPDNO, string GTotal, string STax, string DisPercent, string DiscountAmount, string BalanceAmount, string PaidAmount, string TotalAmount)
            {
            if (PatientOPDNO > 0)
            {
                RegistrationNO = 0;
            }
            HospitlLocationID();
            string DepartName = "";
            string TPA = "";
            string TpaName1 = "";
            string PrivName = "";
            bool PrivalageCard = false;
            //  bool PrivalageCard = false;
            string RefferDoctorID;
            List<OPDBill> searchList = new List<OPDBill>();


            TempData["Reg"] = RegistrationNO;
            dsPatientOPD = _OPDBill.GetPatientForOPDBills(RegistrationNO, PatientOPDNO, HospitalID, LocationID);
             if (dsPatientOPD.Tables[0].Rows.Count > 0)
            {
                dsPatientPrivilege = _OPDBill.GetPatientPrivilegeDiscount(HospitalID, LocationID, RegistrationNO);
                //  string TPA = obj["TPA"];
                if (dsPatientPrivilege.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsPatientPrivilege.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date)// && (TPA. ucPatientOPDBills1.rbTPA.Checked == false))
                    {
                        PrivalageCard = true;

                        PrivName = dsPatientPrivilege.Tables[0].Rows[0]["CardName"].ToString();

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
                    // TempData["Msg"] = "Registration Number not Found";
                    //  return Json("PatientOPDBill");
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
                TempData["DoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                TempData["RDoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["RDoctorPrintName"].ToString();


                //   _OPDBill.GetConsDoctor(dsPatientOPD.Tables[0].Rows[0]["DoctorID"].ToString()).Tables[0].Rows[0]["DoctorPrintName"].ToString();

                {
                    //Gender = dsPatientOPD.Tables[0].Rows[0]["Gender"].ToString();

                    foreach (DataRow dr in dsPatientOPD.Tables[0].Rows)
                    {
                        searchList.Add(new OPDBill
                        {
                            PrintRegNO = dr["PrintRegNO"].ToString(),
                            PrivalageCard = PrivalageCard,
                            PrivalageCardName = PrivName,
                            PatientRegNO = Convert.ToInt32(dr["PatientRegNO"]),
                            RegDate = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("yyyy-MM-dd"),
                            PatientOPDNo = Convert.ToInt32(dr["PatientOPDNo"]),
                            PrintOPDNo = Convert.ToInt32(dr["PrintOPDNo"]),
                            PatientName = dr["PatientName"].ToString(),
                            GuardianName = dr["GuardianName"].ToString(),
                            Address = dr["Address"].ToString(),
                            TPA_ID = Convert.ToInt32(TPAid),
                            TPA_Name = TPAname,
                            //   TPA_ID = Convert.ToInt32(dr["TPA_ID"]),
                            MobileNo = dr["MobileNo"].ToString(),
                            DoctorPrintName = dr["DoctorPrintName"].ToString(),
                            DoctorID = Convert.ToInt32(dr["DoctorID"]),

                            Gender = dr["Gender"].ToString(),
                            BloodPressure = dr["BloodPressure"].ToString(),

                            DateOfBirth = Convert.ToDateTime(dr["DateofBirth"]).ToString("yyyy-MM-dd"),
                            TotalDay = TDays,
                            TotalMonth = TMonth,
                            Year = TYear,
                            DepartmentName = DepartName,
                            RDoctorPrintName = dr["RDoctorPrintName"].ToString(),
                            RDoctorID = RefferDoctorID,
                            BloodGroup = dr["BloodGroup"].ToString(),
                            PreBalance = Convert.ToDecimal(PreBalance),
                            TaxAmount = Convert.ToDecimal(SerTaxAmount),
                            NetPayableAmount = Convert.ToDecimal(NetAmtPayable),
                            BalanceAmount = Convert.ToDecimal(BalAmount),
                            //PatientRegistrationDate = Convert.ToDateTime(dr["PatientRegistrationDate"]),
                            MessageError = "",
                            // TotalAmount = Convert.ToDecimal(TAmount)
                        });
                    }


                }
            }
            else
            {
                searchList.Add(new OPDBill
                {
                    MessageError = "Registration Number not Found",
                });
                //TempData["Msg"] = "Registration Number not Found";
                //RedirectToAction("OPDBill", "OPDBill");
            }


        
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public void BindTestCharges(int DoctorID)
        {
            dsTestName = _OPDBill.GetPatientLabBillsForTestMaster(HospitalID, LocationID, DoctorID);
            int i = 0;
            foreach (DataRow dr in dsTestName.Tables[0].Rows)
            {
                dr["IndexNo"] = i;
                i++;
            }
        }

        public void GetEmptyDataSetPatientBillsDetails()
        {
            dsPatientBillsDetails.Reset();
            dsPatientBillsDetails.Tables.Add(new DataTable("PatientBillsDetails"));
            dsPatientBillsDetails.Tables[0].Columns.Add("HospitalID");
            dsPatientBillsDetails.Tables[0].Columns.Add("LocationID");
            dsPatientBillsDetails.Tables[0].Columns.Add("BillNo");
            dsPatientBillsDetails.Tables[0].Columns.Add("ServiceID");
            dsPatientBillsDetails.Tables[0].Columns.Add("ServiceName");
            dsPatientBillsDetails.Tables[0].Columns.Add("UnitID");
            dsPatientBillsDetails.Tables[0].Columns.Add("ChargesType");
            dsPatientBillsDetails.Tables[0].Columns.Add("Rate");
            dsPatientBillsDetails.Tables[0].Columns.Add("Quantity");
            dsPatientBillsDetails.Tables[0].Columns.Add("Discount");
            dsPatientBillsDetails.Tables[0].Columns.Add("Commisson");
            dsPatientBillsDetails.Tables[0].Columns.Add("ReffCommission");
            dsPatientBillsDetails.Tables[0].Columns.Add("ServiceType");
            dsPatientBillsDetails.Tables[0].Columns.Add("TotalAmount");
            dsPatientBillsDetails.Tables[0].Columns.Add("HideInBilling");
            dsPatientBillsDetails.Tables[0].Columns.Add("DoctorID");
            //dsPatientBillsDetails.Tables[0].Columns.Add("IsPaid");
            dsPatientBillsDetails.Tables[0].Columns.Add("CreationID");
            dsPatientBillsDetails.Tables[0].Columns.Add("Mode");
        }

        public ActionResult NXtRegNO()
        {
            string regNO = "";
            BL_PatientOPDBill Nxtreg = new BL_PatientOPDBill();
            regNO = Nxtreg.GetNextPatientRegNO().Tables[0].Rows[0]["PatientRegNo"].ToString();
            return Json(regNO);
        }

        #region DipartmentBind
        public JsonResult GetDepartment(string prefix)
        {
            BL_PatientOPDBill BL_obj = new BL_PatientOPDBill();
            DataSet ds = BL_obj.Department(prefix);
            List<OPDBill> searchList = new List<OPDBill>();

            //Department(prefix);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new OPDBill
                {
                    DepartmentID = Convert.ToInt32(dr["DepartmentID"]),
                     DepartmentName = dr["DepartmentName"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        #endregion


        #region RefferedDoctor
        public JsonResult GetReferred_DrBind(string prefix)
        {
            BL_PatientOPDBill BL_obj = new BL_PatientOPDBill();

            DataSet ds = BL_obj.GetReferred_Dr(prefix, "%");
            List<OPDBill> searchList = new List<OPDBill>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new OPDBill
                {
                    RDoctorPrintName = dr["DoctorPrintName"].ToString(),
                    ReferredByDoctorID = Convert.ToInt32(dr["DoctorID"].ToString()),
                    //  DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        public JsonResult BindConsultant_dr(string prefix)
        {


            BL_PatientOPDBill db = new BL_PatientOPDBill();

            return new JsonResult { Data = db.Consultant(prefix, "%"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult chkAurthorise(string prefix)

        {
            HospitlLocationID();
            KeystoneProject.Buisness_Logic.Hospital.BL_Users user = new Buisness_Logic.Hospital.BL_Users();
            DataSet ds = new DataSet();
           // List<Users> Search = new List<Users>();
            ds = user.GetUsers(CreationID);

            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj = new Buisness_Logic.Master.BL_MasterSetting();
            DataSet dsMasterSetting = new DataSet();
            List<string> serch = new List<string>();
            dsMasterSetting = obj.GetMasterSetting();
           
                serch.Add(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                serch.Add(ds.Tables[0].Rows[0]["AuthorizationRights"].ToString());
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult BindCaculteAge(string date)
        {
            string aa = "";
            BL_PatientOPDBill db = new BL_PatientOPDBill();
            if (date != "NaN-NaN-NaN")
            {
                 aa = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            }
            return new JsonResult { Data = aa, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
      

    }
}