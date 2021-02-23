using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Buisness_Logic.Laboratory;

namespace KeystoneProject.Controllers.Patient
{
    public class IPDBillController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;

        //
        // GET: /PatientSearch/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        // GET: /IPDBill/
        [HttpGet]
        public ActionResult IPDBill()
        {
            return View();
        }
        [HttpPost]

        public ActionResult IPDBill(FormCollection fc, IPDBill ipdb)
        {

            try
            {

                //  GetBalanceAmountRegistration



                // string chkbtn = Request.Form["RadioChargee"].ToString();
                string chkbtn = fc["radio_chargee_hid"].ToString();
                string chkReport = fc["radioReports"].ToString();

                HospitlLocationID();
                //if (chkbtn != "3")
                //{
                //    ipdb.DoctorCharges = fc["Doctorcharges"].ToString();
                //}
                //else
                //{
                //    ipdb.DoctorCharges = "00";
                //}
                ipdb.HospitalID = HospitalID.ToString();
                ipdb.LocationID = LocationID.ToString();
                ipdb.PrintBillNo = "";
                ipdb.BillDate = fc["bill_date"] + " " + fc["bill_time"];
                string old_bill_no = fc["old_bill_no"].ToString();



                //  string DoctorChar = fc["Doctorcharges"].ToString();
                string ServiceType = "";


                ipdb.PatientRegNO = fc["patient_reg_no_txt"].ToString();
                //ipdb.OPDIPDID = fc["patient_ipd_no_name"].ToString();
                ipdb.OPDIPDID = fc["patient_ipd_no_id"].ToString();
                //ipdb.OPDIPDID = "146";



                //--Payment

                ipdb.PaymentType = Request.Form["payment_type"].ToString();

                ipdb.Number = Request.Form["Number"].ToString();
                ipdb.Name = Request.Form["Name"].ToString();
                ipdb.Remarks = Request.Form["Remarks"].ToString();
                ipdb.Date = Request.Form["paymentDate"].ToString();
                ipdb.DiscountReason = Request.Form["DiscountReasonID"].ToString();


                //-------------

                ipdb.BillType = "IPDBill";
                ipdb.GrossAmount = fc["gross_total_name"].ToString();
                ipdb.TaxPercent = fc["surcharge"].ToString();
                ipdb.TaxAmount = fc["tax_amount"].ToString();
                // ipdb.TaxPercent = "0.0";
                // ipdb.TaxAmount = "0.0";
                ipdb.ReffCommission = fc["surcharge"].ToString();
                ipdb.ReffCommission = "0.00";
                ipdb.Commisson = "0.0";
                ipdb.TotalAmount = fc["total_amount_name"].ToString();
                ipdb.DiscountPercent = fc["discount"].ToString();
                ipdb.DiscountAmount = fc["disc_amount_name"].ToString();
                ipdb.NetPayableAmount = fc["netpayable_name"].ToString();
                ipdb.BalanceAmount = fc["bal_amount_name"].ToString();
                if (ipdb.BalanceAmount == "")
                {
                    ipdb.BalanceAmount = "0.00";
                }
                ipdb.PreBalanceAmount = fc["pre_balance_name"].ToString();
                ipdb.IsPaid = "False";
                ipdb.PaidAmount = fc["amtpaid"].ToString();
                ipdb.DipositAmount = fc["Deposit"].ToString();
                ipdb.IsBillMade = "False";
                ipdb.Date = fc["bill_date"] + " " + fc["bill_time"];
                //ipdb.SancationAmount = fc["sanction_amount"].ToString();
                ipdb.SancationAmount = "0";
                ipdb.FinancialYearID = fc["fin_year_id_hid"].ToString();
                ipdb.CreationID = CreationID.ToString();
                ipdb.LastModificationID = CreationID.ToString();
                ipdb.CreationDate = fc["bill_date"] + " " + fc["bill_time"];

                if (fc["svc_hid"] != null)
                {
                    if (fc["ServiceType"] != null)
                    {
                        ServiceType = fc["ServiceType"].ToString();
                    }
                    if (fc["ServiceType"].ToString() != "")
                    {
                        ServiceType = fc["ServiceType"].ToString();
                    }

                    string svcid = fc["svc_hid"].ToString();
                    string svcname = fc["svc_nm_hid"].ToString();
                    string unitid = fc["UnitID1"].ToString();
                    string docid = fc["doc_hid"].ToString();
                    string rate = fc["svc_rate_hid"].ToString();
                    string qty = fc["svc_qty_hid"].ToString();
                    string tot = fc["tot_hid"].ToString();
                    //   string tblIPDBill = fc["tblIPDBill"].ToString();
                    string Discount_Service = fc["Discount_Service"].ToString();

                    string Discount_serviceType = fc["DiscountServiceType"].ToString();
                    string sevicedisAmt = fc["sevicedisAmt"].ToString();
                    string Authorization = fc["Authorization"].ToString();
                    string Mode = fc["Mode"].ToString();
                    String[] service_ID = svcid.Split(',');
                    String[] service_Name = svcname.Split(',');
                    String[] service_unit_ID = unitid.Split(',');
                    String[] service_doc_ID = docid.Split(',');
                    String[] service_rate = rate.Split(',');
                    String[] service_qty = qty.Split(',');
                    String[] total_a = tot.Split(',');
                    string[] ServiceDisAmt = Discount_Service.Split(',');
                    string[] ServiceDisType = Discount_serviceType.Split(',');
                    string[] sevicedis_Amt = sevicedisAmt.Split(',');
                    String[] Mode1 = Mode.Split(',');
                    String[] ServiceType1 = ServiceType.Split(',');
                    string[] tblIPDBill = ServiceType.Split(',');
                    string[] Authorization1 = Authorization.Split(',');
                    if (old_bill_no == "-2")
                    {
                        tblIPDBill = fc["tblIPDBill"].ToString().Split(',');
                    }
                    // string[] DoctorCharge = DoctorChar.Split(',');

                    ServiceName[] svc_array = new ServiceName[service_ID.Length];
                    for (int i = 0; i < service_ID.Length; i++)
                    {

                        svc_array[i] = new ServiceName();
                        if (service_ID[i].ToString() != "0")
                        {
                            if (service_rate[i].ToString() == "")
                            {
                                service_rate[i] = "0";
                            }
                            if (total_a[i].ToString() == "" || total_a[i].ToString() != "NaN")
                            {
                                total_a[i] = "0";
                            }
                            if (ServiceDisAmt[i].ToString() == "")
                            {
                                ServiceDisAmt[i] = "0";
                            }
                            svc_array[i].UnitID = service_unit_ID[i].ToString();
                            svc_array[i].SvcID = service_ID[i].ToString();
                            svc_array[i].SvcName = service_Name[i].ToString();
                            svc_array[i].UnitID = service_unit_ID[i].ToString();
                            svc_array[i].DoctorID = service_doc_ID[i].ToString();
                            svc_array[i].Rate = service_rate[i].ToString();
                            svc_array[i].Quantity = service_qty[i].ToString();
                            svc_array[i].Total = total_a[i].ToString();
                            svc_array[i].ServiceType = ServiceType1[i].ToString();
                            svc_array[i].Discount_Service = ServiceDisAmt[i].ToString();
                            svc_array[i].DiscountServiceType = ServiceDisType[i].ToString();
                            svc_array[i].sevicedisAmt = sevicedis_Amt[i].ToString();
                            svc_array[i].Authorization = Authorization1[i].ToString();
                            svc_array[i].Mode = Mode1[i].ToString();
                            if (old_bill_no == "-2")
                            {
                                svc_array[i].BillNoChangeHeder = tblIPDBill[i].ToString();
                            }
                        }
                        // svc_array[i].Dr_charges = DoctorCharge[i].ToString();
                    }
                    ipdb.Services = svc_array;
                }


                BL_Patient_IPDBill db = new BL_Patient_IPDBill();
                IPDBill bill2 = null;
                if (old_bill_no == "-1")
                {
                    if (chkbtn == "3")
                    {
                        if (db.CheckFinalBill(HospitalID, LocationID, Convert.ToInt32(ipdb.PatientRegNO), Convert.ToInt32(ipdb.OPDIPDID)))
                        {
                            ViewBag.message = "Please Select Final Bill";
                        }
                        else
                        {
                            ipdb.BillType = "IPDFinalBill";
                            bill2 = db.AddIPDBill(ipdb);
                            if (bill2.Message != null)
                            {
                                TempData["Msg"] = bill2.Message;
                                return RedirectToAction("IPDBill", "IPDBill");
                            }
                            connection();
                            HospitlLocationID();
                            SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummary", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd.Parameters.AddWithValue("@PatientRegNo", ipdb.PatientRegNO);
                            cmd.Parameters.AddWithValue("@PatientIPDNo", ipdb.OPDIPDID);
                            Session["HospitalIDReport"] = HospitalID;
                            Session["LocationIDReport"] = LocationID;
                            Session["PatientRegNoReport"] = ipdb.PatientRegNO.ToString();
                            Session["PatientIPDNoReport"] = ipdb.OPDIPDID.ToString();
                            Session["RadioReport"] = chkReport;
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            //  con.Open();
                            da.Fill(ds);

                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                string ServiceName = "";
                                foreach (DataRow dr in ds.Tables[1].Rows)
                                {
                                    ServiceName = dr["ServiceName"].ToString();
                                }
                                return RedirectToAction("RptPatientIPDFinalBillsSummaryNew", "PatientReport");
                                /*
                                                ReportDocument rd = new ReportDocument();
                                                rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                                rd.SetDataSource(ds);
                                                Response.Buffer = false;
                                                Response.ClearContent();
                                                Response.ClearHeaders();
                                                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                                stream.Seek(0, SeekOrigin.Begin);
                                                return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                                */

                            }
                        }
                    }
                    else if (chkbtn == "2")
                    {
                        ipdb.BillType = "IPDProvisionalBill";
                        bill2 = db.AddIPDBill(ipdb);
                        if (bill2.Message != null)
                        {
                            TempData["Msg"] = bill2.Message;
                            RedirectToAction("IPDBill", "IPDBill");
                        }
                        connection();
                        HospitlLocationID();
                        SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@BillNo", bill2.BillNo);
                        //cmd.Parameters.AddWithValue("@PatientIPDNo", ipdb.OPDIPDID);
                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["ProvisionalReportBillNo"] = bill2.BillNo.ToString();
                        Session["RadioReport"] = chkReport;
                        cmd.CommandTimeout = 500;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);

                        if (ds.Tables[1].Rows.Count != 0)
                        {
                            string ServiceName = "";
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                ServiceName = dr["ServiceName"].ToString();
                            }
                            return RedirectToAction("RptPatientIPDProvisionalBills", "PatientReport");
                            /*
                                            ReportDocument rd = new ReportDocument();
                                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                            rd.SetDataSource(ds);
                                            Response.Buffer = false;
                                            Response.ClearContent();
                                            Response.ClearHeaders();
                                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                            stream.Seek(0, SeekOrigin.Begin);
                                            return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                            */

                        }
                    }
                    else if (chkbtn == "1")
                    {


                        bill2 = db.AddIPDBill(ipdb);


                        if (bill2.Message != null)
                        {
                            TempData["Msg"] = bill2.Message;
                            return RedirectToAction("IPDBill", "IPDBill");
                        }
                        //--Sms
                        //test.Controllers.Hospital.SMSSettingController objsms = new test.Controllers.Hospital.SMSSettingController();
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
                        ////TempData["RefferDoctorID"] = RefferDoctorID;
                        ////TempData["DoctorID"] = dsPatientOPD.Tables[0].Rows[0]["DoctorID"].ToString();
                        ////TempData["DoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                        ////TempData["RDoctorPrintName"] = dsPatientOPD.Tables[0].Rows[0]["RDoctorPrintName"].ToString(); 


                        //if (SmsChk == "true" && Convert.ToBoolean(dssms.Tables[0].Rows[0]["OPDBillDrSMS"].ToString()) == true)
                        //{
                        //    objsms.SendSMS(Convert.ToInt32(ipdb.PatientRegNO), Convert.ToInt32(TempData["RefferDoctorID"]), TempData["DoctorPrintName"].ToString(), "IPD4", TempData["RDoctorPrintName"].ToString());
                        //}
                        //--------------SMS

                        connection();
                        HospitlLocationID();
                        SqlCommand cmd = new SqlCommand("RptPatientIPDBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@BillNo", bill2.BillNo);
                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["BillIDReport"] = bill2.BillNo.ToString();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            string ServiceName = "";
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                ServiceName = dr["ServiceName"].ToString() + " By " + dr["DoctorPrintName"]; ; ;
                            }
                            return RedirectToAction("RptPatientIPDBill");
                            /*
                                            ReportDocument rd = new ReportDocument();
                                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                            rd.SetDataSource(ds);
                                            Response.Buffer = false;
                                            Response.ClearContent();
                                            Response.ClearHeaders();
                                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                            stream.Seek(0, SeekOrigin.Begin);
                                            return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                            */

                        }
                    }

                }
                else
                {
                    ipdb.BillNo = Convert.ToInt32(old_bill_no);
                    //bill2 = db.UpdateIPDBill(ipdb);
                    if (chkbtn == "3")
                    {
                        ipdb.BillType = "IPDFinalBill";
                        //bill2 = db.AddIPDBill(ipdb);
                        bill2 = db.UpdateIPDBill(ipdb);
                        if (bill2.Message != null)
                        {
                            TempData["Msg"] = bill2.Message;
                            return RedirectToAction("IPDBill", "IPDBill");
                        }
                        if (bill2.Message != null)
                        {
                            TempData["Msg"] = bill2.Message;
                            return RedirectToAction("IPDBill", "IPDBill");
                        }
                        if (bill2.Message != null)
                        {
                            TempData["Msg"] = bill2.Message;
                            return RedirectToAction("IPDBill", "IPDBill");
                        }
                        connection();
                        HospitlLocationID();
                        SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@PatientRegNo", ipdb.PatientRegNO);
                        cmd.Parameters.AddWithValue("@PatientIPDNo", ipdb.OPDIPDID);

                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["PatientRegNoReport"] = ipdb.PatientRegNO.ToString();
                        Session["PatientIPDNoReport"] = ipdb.OPDIPDID.ToString();
                        Session["RadioReport"] = chkReport;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            string ServiceName = "";
                            //foreach (DataRow dr in ds.Tables[1].Rows)
                            //{
                            //    ServiceName = dr["ServiceName"].ToString();
                            //}
                            return RedirectToAction("RptPatientIPDFinalBillsSummaryNew", "PatientReport");
                            /*
                                            ReportDocument rd = new ReportDocument();
                                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                            rd.SetDataSource(ds);
                                            Response.Buffer = false;
                                            Response.ClearContent();
                                            Response.ClearHeaders();
                                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                            stream.Seek(0, SeekOrigin.Begin);
                                            return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                            */

                        }
                    }
                    else if (chkbtn == "2")
                    {
                        ipdb.BillType = "IPDProvisionalBill";
                        // bill2 = db.AddIPDBill(ipdb);
                        bill2 = db.AddIPDBill(ipdb);
                        connection();
                        HospitlLocationID();
                        SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@BillNo", bill2.BillNo);
                        //cmd.Parameters.AddWithValue("@PatientIPDNo", ipdb.OPDIPDID);

                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["ProvisionalReportBillNo"] = bill2.BillNo.ToString();
                        Session["RadioReport"] = chkReport;
                        cmd.CommandTimeout = 500;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);

                        if (ds.Tables[1].Rows.Count != 0)
                        {
                            string ServiceName = "";
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                ServiceName = dr["ServiceName"].ToString();
                            }
                            return RedirectToAction("RptPatientIPDProvisionalBills", "PatientReport");
                            /*
                                            ReportDocument rd = new ReportDocument();
                                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                            rd.SetDataSource(ds);
                                            Response.Buffer = false;
                                            Response.ClearContent();
                                            Response.ClearHeaders();
                                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                            stream.Seek(0, SeekOrigin.Begin);
                                            return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                            */

                        }
                    }
                    else if (chkbtn == "1")
                    {
                        if (old_bill_no == "-2")
                        {
                            db.UpdateAllIPDBill(ipdb);
                            if (bill2 == null)
                            {
                                return RedirectToAction("IPDBill", "IPDBill");
                            }
                        }
                        else
                        {
                            bill2 = db.UpdateIPDBill(ipdb);
                        }
                        //govind
                        //  bill2 = db.AddIPDBill(ipdb);

                        if (bill2.Message != null)
                        {
                           
                            TempData["Msg"] = bill2.Message;

                            return RedirectToAction("IPDBill", "IPDBill");
                        }
                        connection();
                        HospitlLocationID();
                        SqlCommand cmd = new SqlCommand("RptPatientIPDBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@BillNo", bill2.BillNo);

                        Session["HospitalIDReport"] = HospitalID;
                        Session["LocationIDReport"] = LocationID;
                        Session["BillIDReport"] = bill2.BillNo.ToString();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        //  con.Open();
                        da.Fill(ds);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            string ServiceName = "";
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                // ServiceName = dr["ServiceName"].ToString() ;
                                ServiceName = (dr["ServiceName"].ToString()) + " By " + dr["DoctorPrintName"];
                            }
                            return Redirect("RptPatientIPDBill");
                            /*
                                            ReportDocument rd = new ReportDocument();
                                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                            rd.SetDataSource(ds);
                                            Response.Buffer = false;
                                            Response.ClearContent();
                                            Response.ClearHeaders();
                                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                            stream.Seek(0, SeekOrigin.Begin);
                                            return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                            */

                        }
                    }

                }
                //GetRptPatientIPDBills(Convert.ToInt32(bill2.HospitalID), Convert.ToInt32(bill2.LocationID), bill2.BillNo, "IPD", "SaveView");

                //  try
                // {
                /*
                connection();

                SqlCommand cmd = new SqlCommand("RptPatientIPDBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", 1);
                cmd.Parameters.AddWithValue("@LocationID", 1);
                cmd.Parameters.AddWithValue("@BillNo", bill2.BillNo);

                Session["HospitalIDReport"] = "1";
                Session["LocationIDReport"] = "1";
                Session["BillIDReport"] = bill2.BillNo.ToString();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //  con.Open();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count != 0)
                {

               
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();

                        //if (ServiceName.Contains("VISIT CHARGES"))
                        //{
                        //    dr["ServiceName"] += " By " + dr["DoctorPrintName"];
                        //}
                    }
                    return RedirectToAction("RptPatientIPDBill");
                    */
                //   return RedirectToAction("RptPatientIPDBill",new { });

                //  CrystalReport1 rptPatientIPDBills = new CrystalReport1();


                /*
                                ReportDocument rd = new ReportDocument();
                                rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));
                                rd.SetDataSource(ds);
                                Response.Buffer = false;
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                stream.Seek(0, SeekOrigin.Begin);
                                return File(stream, "application/pdf", "RptPatientIPDBills.pdf");
                */

                //            }

                #region RptMoneyReceipt
                //decimal PaidAmt = Convert.ToDecimal(bill2.PaidAmount);

                //if (PaidAmt > 0)
                //{
                //DBHandle objMoneyReceipt = new DBHandle();
                //objMoneyReceipt.RptMoneyReceipt(Convert.ToInt32(bill2.HospitalID), Convert.ToInt32(bill2.LocationID), bill2.BillNo, "SaveView");
                /*  
              connection();

               SqlCommand cmd1 = new SqlCommand("RptMoneyReceipt", con);
               cmd1.CommandType = CommandType.StoredProcedure;
               cmd1.Parameters.AddWithValue("@HospitalID", 1);
               cmd1.Parameters.AddWithValue("@LocationID", 1);
               cmd1.Parameters.AddWithValue("@BillNo", bill2.BillNo);
               SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

               DataSet ds1 = new DataSet();
               da.Fill(ds1);

               if (ds1.Tables[1].Rows.Count != 0)
               {
                   */
                // RptPreBalanceAmount rptPreBalanceAmount = new RptPreBalanceAmount();

                // return RedirectToAction("RptPatientIPDBill");

                /*
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));

                rd.SetDataSource(ds1);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", "CustomerList.pdf");
                */

                //  }

                //}
                #endregion
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
            }
            return RedirectToAction("IPDBill", "IPDBill");
        }


        #region PatienBind
        public JsonResult GetPatientNameValue(string search)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();

            return new JsonResult { Data = db.GetPatientName(search), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


        #region PatientBindFillData
        public JsonResult GetPatientDetailByReg(string RegNo, string RB, string IPDNo)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            BL_PatientOPD BL_Reg = new BL_PatientOPD();
            string RegId = RegNo;

            if (IPDNo != "")
            {
                RegNo = "0";
            }
            //  BL_Patient_IPDBill db = new BL_Patient_IPDBill();

            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            SqlConnection con = new SqlConnection(constring);

            DataSet dsfinalRefundAmt = new DataSet();
            IPDBill objpatinet = new IPDBill();
            int PaidAmount = 0;
            try
            {
                TempData["pri"] = "pri";


                TempData["Reg"] = RegNo;


                if (RegNo != "")
                {
                    SqlCommand cmd = new SqlCommand("GetPatientForIPDBills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    HospitlLocationID();
                    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
                    cmd.Parameters.Add(new SqlParameter("@PatientOPIDNo", IPDNo));
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    //DataTable dt = new DataTable();

                    sd.Fill(ds);


                    TempData["RegDate"] = ds.Tables[0].Rows[0]["AddmissionDate"].ToString();
                    TempData["RefferDoctorID"] = ds.Tables[0].Rows[0]["ReferredByDoctorID"].ToString();
                    TempData["DoctorID"] = ds.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                    TempData["DoctorPrintName"] = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();
                    TempData["RDoctorPrintName"] = ds.Tables[0].Rows[0]["DoctorPrintName1"].ToString();
                    TempData["ApplyWardID"] = ds.Tables[0].Rows[0]["ApplyWardID"].ToString();

                    Session["PatientOPIDNO"] = ds.Tables[0].Rows[0]["PatientIPDNO"].ToString();

                    //  string B= TempData["ApplyWardID"].ToString();
                    if (RB == "2")
                    {
                        SqlCommand cmd1 = new SqlCommand("GetPatientIPDFinalForBalance", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        HospitlLocationID();
                        cmd1.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd1.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd1.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegId)));
                        cmd1.Parameters.Add(new SqlParameter("@PatientIPDNO", Convert.ToInt32(ds.Tables[0].Rows[0]["PatientIPDNO"].ToString())));
                        cmd1.CommandTimeout = 500;
                        SqlDataAdapter sd1 = new SqlDataAdapter(cmd1);
                        sd1.Fill(dsfinalRefundAmt);

                        PaidAmount = Convert.ToInt32(dsfinalRefundAmt.Tables[0].Rows[0]["PaidAmount"]);
                        objpatinet.PaidAmount = dsfinalRefundAmt.Tables[0].Rows[0]["PaidAmount"].ToString();

                    }
                }
                //  objpatinet.OPDIPDID = db.GetPatientDetailByReg(RegNo, RB);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
            }
            return new JsonResult { Data = db.GetPatientDetailByReg(RegNo, RB, PaidAmount, IPDNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        public string ApplyWord(string RegNo, string IPDNo)
        {
            HospitlLocationID();
            connection();
            SqlCommand cmd = new SqlCommand("GetPatientForIPDBills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            HospitlLocationID();
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
            cmd.Parameters.Add(new SqlParameter("@PatientOPIDNo", IPDNo));
            cmd.CommandTimeout = 500;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            sd.Fill(ds);

            return ds.Tables[0].Rows[0]["ApplyWardID"].ToString();
        }
        #region BindServiceGroup
        public JsonResult GetServiceGrp(string prefix, string Type)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = db.GetServiceGrp(prefix, Type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region Declare Varible Bind 
        string MessageBed = "";
        string BedDate = "";
        int PayedDays = 0;
        DataSet dsBedCharges = new DataSet();

        decimal oldBedCharges = 0;
        decimal CurrentBedCharges = 0;
        decimal newBedCharges = 0;
        int Days = 0;
        int BedchargeshCount = 0;
        DateTime EnterDate = new DateTime();
        DateTime ExitDate = new DateTime();
        DateTime oldEnterDate = new DateTime();
        DateTime lastPaydate = new DateTime();
        DateTime ToPaydate = new DateTime();
        DateTime FromPaydate = new DateTime();
        BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
        string ChargesType = "";
        OPDBill add = new OPDBill();

        #endregion
        List<OPDBill> searchList = new List<OPDBill>();
        public JsonResult GetServiceAutoBind(string BillDate, string RegDate, string WordID, string chk, string RegNo, string IPDNo, string OrganizationID, bool TPA)
        {  //GetAutoServiceBind
           // List<OPDBill> searchList = new List<OPDBill>();

            if (TPA == false)
            {
                DateTime EnterDate = new DateTime();
                DateTime ExitDate = new DateTime();
                BL_Patient_IPDBill db = new BL_Patient_IPDBill();
                string UnitName = "";
                BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
                DataSet dswordcharge = new DataSet();
                try
                {
                    //RegDate = TempData["RegDate"].ToString();

                    EnterDate = Convert.ToDateTime(RegDate);
                    ExitDate = Convert.ToDateTime(BillDate);
                    string GeneralCharges = "";
                    string EmergencyCharges = "";
                    string Unit = "";
                    DataSet ds = obj.GetWordChargesServiceAutoFill(IPDNo, RegNo);
                    Days = Convert.ToInt32((ExitDate - EnterDate).TotalDays);
                    DateTime a = Convert.ToDateTime(RegDate);//some datetime
                    DateTime now = Convert.ToDateTime(ExitDate);
                    TimeSpan ts = now - a;
                    string days = ts.TotalDays.ToString();
                    //if (Days > Convert.ToDecimal(days))
                    //{
                    //    Days += 1;
                    //}
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        dswordcharge = obj.GetServicesDetailsforIPDBills(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID));
                        if (chk != "TPA")
                        {
                            if (dswordcharge.Tables[0].Rows.Count > 0)
                            {
                                Unit = dswordcharge.Tables[0].Rows[0]["UnitID"].ToString();
                                UnitName = dswordcharge.Tables[0].Rows[0]["UnitName"].ToString();
                                //  RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
                                //  HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
                                EmergencyCharges = dswordcharge.Tables[0].Rows[0]["EmergencyCharges"].ToString();
                                GeneralCharges = dswordcharge.Tables[0].Rows[0]["GeneralCharges"].ToString();
                                string applywardID = ApplyWord(RegNo, IPDNo);
                                if (applywardID != "" && applywardID != "0")
                                {
                                    WordID = applywardID;
                                    dswordcharge = obj.GetServicesDetailsforIPDBills(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID));
                                    EmergencyCharges = dswordcharge.Tables[0].Rows[0]["EmergencyCharges"].ToString();
                                    GeneralCharges = dswordcharge.Tables[0].Rows[0]["GeneralCharges"].ToString();
                                }


                                if (dr["ServiceName"].ToString() == "BED CHARGES")
                                {
                                    BedCharge = true;

                                    BedCharges(ExitDate, EnterDate, Convert.ToInt32(IPDNo), Convert.ToInt32(RegNo), chk);
                                    if (BedDate == "")
                                    {
                                        BedCharge = false;
                                    }
                                    dr["BillCharges"] = true;
                                    dr["ServiceName"] = BedDate;// dr["ServiceName"] + "--" + EnterDate.ToString("dd-MM-yyyy") + " To " + ExitDate.ToString("dd-MM-yyyy");
                                }
                                else
                                {
                                    //if (BedDate == "")
                                    //{
                                    //    dr["ServiceName"] = "";
                                    //}
                                }
                            }
                            if (dr["ServiceName"].ToString() != "")
                            {
                                if (EmergencyCharges == "")
                                {
                                    EmergencyCharges = "0";
                                }
                                if (GeneralCharges == "")
                                {
                                    GeneralCharges = "0";
                                }
                                if (Unit == "")
                                {
                                    Unit = "0";
                                }
                                if (Days.ToString() == "0" || Convert.ToBoolean(dr["BillAutoOne"]) == true)
                                {
                                    Days = 1;
                                }
                                //  if(dr[""])
                                searchList.Add(new OPDBill
                                {
                                    servicename = dr["ServiceName"].ToString(),
                                    ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                    ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                                    GeneralCharges = GeneralCharges,
                                    EmergencyCharges = EmergencyCharges,
                                    UnitID = Convert.ToInt32(Unit),
                                    BillCharges = dr["BillCharges"].ToString(),
                                    ServiceType = "IPD",
                                    Quantity = Days.ToString()

                                    // ucPatientIPDBills1.atxtUnit.Text = ds.Tables[1].Rows[0]["UnitID"].ToString();
                                    //RecommendedByDoctor = ds.Tables[1].Rows[0]["RecommendedByDoctor"].ToString();
                                    //HideInBilling = ds.Tables[1].Rows[0]["HideInBilling"].ToString();

                                });
                                // BedDate = "";
                            }
                        }
                    }

                    if (searchList[0].GeneralCharges == "")
                    {
                        searchList[0].GeneralCharges = "0";
                    }


                    //Decimal Discount = DiscountPreVilege(Convert.ToInt32(searchList[0].ServiceID), Convert.ToDecimal(searchList[0].GeneralCharges), Convert.ToInt32(RegNo), chk);
                    //if (Discount > 0)
                    //{
                    //    if (Convert.ToBoolean(TempData["PrivalageCard"]) == true)
                    //    {
                    //        Decimal DWithRate = Convert.ToDecimal(searchList[0].GeneralCharges);
                    //        DWithRate = DWithRate - (DWithRate * Discount / 100);
                    //        searchList[0].servicename = searchList[0].servicename + " ( " + searchList[0].GeneralCharges + ",  " + Math.Round(Discount, 2) + "% )";
                    //        searchList[0].GeneralCharges = DWithRate.ToString();
                    //    }
                    //}

                    if (BedCharge == false)
                    {
                        searchList = null;
                    }

                }
                catch (Exception ex)
                {
                    TempData["Msg"] = ex.Message;
                }
            }
            else
            {
                // TPA Wise
                DataSet ds = obj.GetWordChargesServiceAutoFill(IPDNo, RegNo);
                int i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    GetServiceNameTPA(dr["ServiceID"].ToString(), "%", dr["ServiceGroupID"].ToString(), chk, "%", RegNo, IPDNo, BillDate, RegDate, WordID, Convert.ToInt16(OrganizationID));


                    if (Convert.ToBoolean(dr["BillAutoOne"]) == true)
                    {
                        searchList[i].Quantity = "1";

                    }
                    i++;
                    //searchList[0].BillCharges
                    // if()
                }
                if (BedCharge == false)
                {
                    searchList = null;
                }

            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        // Bed Charges
        public void BedChargesAuto(DateTime BillDate, DateTime RegDate, int IPDNo, int RegNo, bool TPAType, string ServiceID)
        {
            Boolean Flage = false;
            BedchargeshCount = -1;
            EnterDate = RegDate;

            //  BillDate = ExitDate;
            if (TPAType == true)
            {
                ChargesType = "GeneralCharges";
            }
            else
            {
                ChargesType = "EmergencyCharges";
            }

            PayedDays = 0;

            HospitlLocationID();
            if (TPAType == true)
            {
                dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(IPDNo));
            }
            else
            {
                dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(IPDNo));
            }
            oldEnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[0]["EnterDateTime"].ToString());
            lastPaydate = Convert.ToDateTime(dsBedCharges.Tables[1].Rows[0]["lastPaydate"].ToString());
            PayedDays = Convert.ToInt32((lastPaydate - oldEnterDate).TotalDays);
            for (int i = 0; i < dsBedCharges.Tables[0].Rows.Count; i++)
            {
                if (dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString() == null || dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString() == "")
                {
                    EnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["EnterDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                    if (BillDate == EnterDate.Date)
                    {
                        ExitDate = BillDate;
                    }
                    else
                    {
                        ExitDate = BillDate;
                    }
                }
                else
                {
                    EnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["EnterDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                    ExitDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                }
                Days = Convert.ToInt32((ExitDate - EnterDate).TotalDays - 1);
                if (i != 0)
                {
                    oldBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i - 1][ChargesType].ToString());
                }
                else
                {
                    oldBedCharges = 0;
                }
                CurrentBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i][ChargesType].ToString());
                if (dsBedCharges.Tables[0].Rows.Count > i + 1)
                {
                    newBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i + 1][ChargesType].ToString());
                }
                else
                {
                    newBedCharges = 0;
                }
                if (CurrentBedCharges >= oldBedCharges)
                {
                    Days = Days + 1;
                }
                if (CurrentBedCharges > newBedCharges)
                {
                    Days = Days + 1;
                }
                if (Days < 0)
                {
                    Days = 0;
                }
                if (Flage == false)
                {
                    PayedDays -= Days;
                    if (PayedDays < 0)
                    {
                        PayedDays = Convert.ToInt32(PayedDays.ToString().Substring(1, PayedDays.ToString().Trim().Length - 1));
                        Days = PayedDays;
                        Flage = true;
                    }
                }
                if (Flage)
                {
                    //for (int count = 0; Days > count; count++)
                    //{
                    //   ucPatientIPDBills1.txtRate.Text = dsBedCharges.Tables[0].Rows[i][ChargesType].ToString();
                    //  ucPatientIPDBills1.txtQuantity.Text = Days.ToString();

                    ToPaydate = lastPaydate;
                    FromPaydate = lastPaydate;

                    PayedDays = Convert.ToInt32((lastPaydate - oldEnterDate).TotalDays); // old Code 
                    FromPaydate = FromPaydate.AddDays(+BedchargeshCount + 1);
                    BedchargeshCount = Days + BedchargeshCount;
                    if (dsBedCharges.Tables[0].Rows[i]["ServiceName"].ToString() == "BED CHARGES")
                    {
                        BedDate = dsBedCharges.Tables[0].Rows[i]["WardName"].ToString() + " --" + FromPaydate.ToString().Substring(0, 10) + " To  " + ToPaydate.AddDays(BedchargeshCount).ToString().Substring(0, 10);//dsBedCharges.Tables[0].Rows[i]["WardName"].ToString(); 
                    }
                    else
                    {
                        BedDate = dsBedCharges.Tables[0].Rows[i]["ServiceName"].ToString() + " --" + FromPaydate.ToString().Substring(0, 10) + " To  " + ToPaydate.AddDays(BedchargeshCount).ToString().Substring(0, 10);//dsBedCharges.Tables[0].Rows[i]["WardName"].ToString(); 

                    }


                    // }
                }
                else
                {

                    if ((i + 1) == dsBedCharges.Tables[0].Rows.Count)
                    {
                        MessageBed = "Bed Charges is already paid.";
                        // MessageBox.Show("Bed Charges is already paid.");
                    }

                }

            }
        }
        bool Flage = false;
        bool BedCharge = false;
        public void BedCharges(DateTime BillDate, DateTime RegDate, int IPDNo, int RegNo, string Type)
        {
            BedchargeshCount = -1;
            EnterDate = RegDate;
            int Days = 0;
            //  BillDate = ExitDate;
            if (Type == "true")
            {
                ChargesType = "GeneralCharges";
            }
            else
            {
                ChargesType = "EmergencyCharges";
            }
            if (Type == "General")
            {
                ChargesType = "GeneralCharges";
            }
            if (Type == "Emergency")
            {
                ChargesType = "EmergencyCharges";
            }
            PayedDays = 0;

            HospitlLocationID();
            //if (TPAType == true)
            //{
            //    dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(IPDNo));
            //}
            //else
            //{
            dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(IPDNo));
            // }
            if (dsBedCharges.Tables[0].Rows.Count > 0)
                oldEnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[0]["EnterDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));

            lastPaydate = Convert.ToDateTime(dsBedCharges.Tables[1].Rows[0]["lastPaydate"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
            PayedDays = Convert.ToInt32((lastPaydate - oldEnterDate).TotalDays);

            for (int i = 0; i < dsBedCharges.Tables[0].Rows.Count; i++)
            {
                if (dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString() == null || dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString() == "")
                {
                    EnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["EnterDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                    if (BillDate == EnterDate.Date)
                    {
                        ExitDate = Convert.ToDateTime(BillDate).Date.Add(new TimeSpan(00, 00, 00));
                    }
                    else
                    {
                        ExitDate = Convert.ToDateTime(BillDate).Date.Add(new TimeSpan(00, 00, 00));
                    }
                }
                else
                {
                    EnterDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["EnterDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                    ExitDate = Convert.ToDateTime(dsBedCharges.Tables[0].Rows[i]["ExitDateTime"].ToString()).Date.Add(new TimeSpan(00, 00, 00));
                }
                Days = Convert.ToInt32((ExitDate - EnterDate).TotalDays - 1);
                if (i != 0)
                {
                    oldBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i - 1][ChargesType].ToString());
                }
                else
                {
                    oldBedCharges = 0;
                }
                //if (dsBedCharges.Tables[0].Rows[i][ChargesType].ToString() == "")
                //{
                //    dsBedCharges.Tables[0].Rows[i][ChargesType] = 0;
                //}
                CurrentBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i][ChargesType].ToString());
                if (dsBedCharges.Tables[0].Rows.Count > i + 1)
                {
                    newBedCharges = Convert.ToDecimal(dsBedCharges.Tables[0].Rows[i + 1][ChargesType].ToString());
                }
                else
                {
                    newBedCharges = 0;
                }
                if (CurrentBedCharges >= oldBedCharges)
                {
                    Days = Days + 1;
                }
                if (CurrentBedCharges > newBedCharges)
                {
                    Days = Days + 1;
                }
                if (Days < 0)
                {
                    Days = 0;
                }
                if (Flage == false)
                {
                    PayedDays -= Days;
                    if (PayedDays < 0)
                    {
                        PayedDays = Convert.ToInt32(PayedDays.ToString().Substring(1, PayedDays.ToString().Trim().Length - 1));
                        Days = PayedDays;
                        Flage = true;
                    }
                }
                if (Flage)
                {
                    //for (int count = 0; Days > count; count++)
                    //{
                    //   ucPatientIPDBills1.txtRate.Text = dsBedCharges.Tables[0].Rows[i][ChargesType].ToString();
                    //  ucPatientIPDBills1.txtQuantity.Text = Days.ToString();

                    ToPaydate = lastPaydate;
                    FromPaydate = lastPaydate;

                    PayedDays = Convert.ToInt32((lastPaydate - oldEnterDate).TotalDays); // old Code 
                    FromPaydate = FromPaydate.AddDays(+BedchargeshCount + 1);
                    BedchargeshCount = Days + BedchargeshCount;
                    BedDate = dsBedCharges.Tables[0].Rows[i]["WardName"].ToString() + " --" + FromPaydate.ToString().Substring(0, 10) + " To  " + ToPaydate.AddDays(BedchargeshCount).ToString().Substring(0, 10);//dsBedCharges.Tables[0].Rows[i]["WardName"].ToString(); 
                    // }
                }
                else
                {

                    if ((i + 1) == dsBedCharges.Tables[0].Rows.Count)
                    {
                        MessageBed = "Bed Charges is already paid.";
                        // MessageBox.Show("Bed Charges is already paid.");
                    }

                }

            }
        }
        #region Bind_Service_TPA

        public JsonResult GetServiceNameTPA(string prefix, string Code, string id, string chk, string Charges, string RegNo, string IPDNo, string BillDate, string RegDate, string WordID, int OrganizationID)
        {
            //if (prefix == null || prefix=="")
            //{
            //    prefix = "%";

            //}
            if (Code == null)
            {
                Code = "%";

            }
            string Unit = "";
            string UnitName = "";

            //string RegDate = TempData["RegDate"].ToString();
            BL_PatientOPDBill objtest = new BL_PatientOPDBill();
            //    int Days = 0;
            int BedchargeshCount = 0;
            DateTime EnterDate = new DateTime();
            DateTime ExitDate = new DateTime();
            DataSet dsBedCharges = new DataSet();
            OPDBill add = new OPDBill();
            BL_Patient_IPDBill obj = new BL_Patient_IPDBill();

            DataSet dsWordChargesService = new DataSet();
            // dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(ucPatientIPDBills1.txtPatientRegNO.Text), Convert.ToInt32(ucPatientIPDBills1.txtPatientIPDNo.Text));
            dsWordChargesService = obj.GetWordChargesService();
            // ExitDate = BillDate.ToString();
            EnterDate = Convert.ToDateTime(RegDate).Date.Add(new TimeSpan(00, 00, 00));
            ExitDate = Convert.ToDateTime(BillDate).Date.Add(new TimeSpan(00, 00, 00));
            string GeneralCharges = "";
            string EmergencyCharges = "";

            string ServiceGroupName = "";
            DataSet ds = new DataSet();
            if (BedCharge == false)
            {
                Days = Convert.ToInt32((ExitDate - EnterDate).TotalDays);
            }


            ds = obj.GetServiceDetailByNameTPA(prefix, id, Code, OrganizationID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                // ServiceGroupName = ds.Tables[0].Rows[0]["ServiceGroupName"].ToString();
            }

            DataSet dswordcharge = new DataSet();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dswordcharge = obj.GetServicesDetailsforIPDBillsTPA(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID), OrganizationID);

                if (dswordcharge.Tables[0].Rows.Count > 0)
                {
                    Unit = dswordcharge.Tables[0].Rows[0]["UnitID"].ToString();
                    UnitName = dswordcharge.Tables[0].Rows[0]["UnitName"].ToString();
                    ServiceGroupName = dr["ServiceGroupName"].ToString();
                    //  RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
                    //  HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
                    EmergencyCharges = dswordcharge.Tables[0].Rows[0]["EmergencyCharges"].ToString();
                    GeneralCharges = dswordcharge.Tables[0].Rows[0]["GeneralCharges"].ToString();
                    string applywardID = ApplyWord(RegNo, IPDNo);
                    if (applywardID != "" && applywardID != "0")
                    {
                        WordID = applywardID;
                        dswordcharge = obj.GetServicesDetailsforIPDBills(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID));
                        EmergencyCharges = dswordcharge.Tables[0].Rows[0]["EmergencyCharges"].ToString();
                        GeneralCharges = dswordcharge.Tables[0].Rows[0]["GeneralCharges"].ToString();
                    }
                    //    BedCharges(ExitDate, EnterDate, Convert.ToInt32(IPDNo), Convert.ToInt32(RegNo), "true");
                    if (dr["servicename"].ToString() == "BED CHARGES")
                    {
                        BedCharge = true;
                        Days = 0;
                        BedCharges(ExitDate, EnterDate, Convert.ToInt32(IPDNo), Convert.ToInt32(RegNo), "true");
                        string nam = BedDate;
                        string m = add.MessageError;
                        if (BedDate == "")
                        {
                            BedCharge = false;
                        }
                        //dr["BillCharges"] = true;
                        dr["servicename"] = BedDate;
                        //dr["servicename"] + "--" + EnterDate.ToString("dd-MM-yyyy") + " To " + ExitDate.ToString("dd-MM-yyyy");
                    }

                }
                //if (Days.ToString() == "0" || Convert.ToBoolean(dr["BillAutoOne"]) == true)
                //{
                //    Days = 1;
                //}
                if (dr["servicename"].ToString() != "")
                {
                    searchList.Add(new OPDBill
                    {
                        ServiceType = "IPD",
                        ServiceGroupName = ServiceGroupName,
                        servicename = dr["serviceName"].ToString(),
                        ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"]),
                        ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                        GeneralCharges = GeneralCharges,
                        EmergencyCharges = EmergencyCharges,
                        Unit = Unit,
                        BillCharges = "",
                        Quantity = Days.ToString(),
                        ServiceGroupCode = dr["HSNCode"].ToString(),
                        ServiceHSNCode = dr["TPAHSNCode"].ToString(),

                        // ucPatientIPDBills1.atxtUnit.Text = ds.Tables[1].Rows[0]["UnitID"].ToString();
                        //RecommendedByDoctor = ds.Tables[1].Rows[0]["RecommendedByDoctor"].ToString();
                        //HideInBilling = ds.Tables[1].Rows[0]["HideInBilling"].ToString();

                    });
                }
            }


            if (id == "%" && chk == "Service")

                //  DataSet ds = new DataSet();
                if (id == "%" || id == null)
                {
                    ds = obj.GetTestGroupTpaWise(OrganizationID);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        searchList.Add(new OPDBill
                        {
                            //ServiceGroupID = Convert.ToInt16(dr["CategoryID"]),
                            ServiceGroupName = Convert.ToString(dr["CategoryName"]),
                            ServiceType = "Lab",
                            ServiceGroupCode = dr["HSNCode"].ToString(),

                            servicename = dr["TestName"].ToString() + "   (LAB)",
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            ServiceID = Convert.ToInt32(dr["TestID"]).ToString(),
                            GeneralCharges = dr["GeneralCharges"].ToString(),
                            //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                            EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                        });
                    }
                }
                else
                {
                    ds = obj.GetPatientLabBillsForTestMaster(OrganizationID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            searchList.Add(new OPDBill
                            {
                                ServiceType = "Lab",
                                servicename = dr["TestName"].ToString() + "   (LAB)",
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dr["TestID"]).ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                            });
                        }
                    }
                }
            // }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


        public ActionResult GetAllPackageServiceWise(int ServiceID, int WardID)
        {
            BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
            List<OPDBill> searchList = new List<OPDBill>();
            if (ServiceID != -1)
            {
                string PatientOPIDNO = Session["PatientOPIDNO"].ToString();
                DataSet ds = obj.GetAllPackageServiceWise(ServiceID, WardID, PatientOPIDNO);
                if (ds.Tables[1].Rows.Count == 0)
                {
                    //if (ds.Tables[1].Rows[0][0].ToString() == ServiceID.ToString())
                    //{
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["UnitID"].ToString() == "")
                        {
                            dr["UnitID"] = 0;
                        }

                        //BedDate = dsBedCharges.Tables[0].Rows[i]["WardName"].ToString() + " --" + FromPaydate.ToString().Substring(0, 10) + " To  " + ToPaydate.AddDays(BedchargeshCount).ToString().Substring(0, 10);//dsBedCharges.Tables[0].Rows[i]["WardName"].ToString(); 

                        searchList.Add(new OPDBill
                        {
                            ServiceGroupName = dr["ServiceGroupName"].ToString(),
                            ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"]),
                            servicename = dr["ServiceOrTestName"].ToString(),
                            ServiceID = dr["ServiceOrTestID"].ToString(),
                            Quantity = dr["Quantity"].ToString(),
                            UnitID = Convert.ToInt32(dr["UnitID"]),
                            //  UnitID =   dr["UnitID"].ToString(),
                            //  RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
                            //  HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
                            EmergencyCharges = "0",
                            GeneralCharges = dr["GeneralCharges"].ToString(),
                        });
                        //}
                    }
                }
                else
                {
                    return new JsonResult { Data = "Already This Packege Add", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        #region Bind_Service_And_Test
        public JsonResult GetServiceName(string prefix, string Code, string id, string chk, string Charges, string RegNo, string IPDNo, string BillDate, string RegDate, string WordID, string chkprivalage)

        {
            MessageBed = "";
            //string RegDate = TempData["RegDate"].ToString();
            BL_PatientOPDBill objtest = new BL_PatientOPDBill();
            // int Days = 0;
            int BedchargeshCount = 0;
            DateTime EnterDate = new DateTime();
            DateTime ExitDate = new DateTime();
            DataSet dsBedCharges = new DataSet();
            OPDBill add = new OPDBill();
            BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
            //  List<OPDBill> searchList = new List<OPDBill>();
            DataSet dsWordChargesService = new DataSet();
            //   dsBedCharges = obj.GetPatientIPDBillsBedCharges(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(IPDNo));
            dsWordChargesService = obj.GetWordChargesService();
            // ExitDate = BillDate.ToString();
            EnterDate = Convert.ToDateTime(RegDate).Date.Add(new TimeSpan(00, 00, 00));
            ExitDate = Convert.ToDateTime(BillDate).Date.Add(new TimeSpan(00, 00, 00));
            string GeneralCharges = "";
            string EmergencyCharges = "";
            string Unit = "0";
            string UnitName = "";
            string ServiceGroupName = "";

            DataSet ds = new DataSet();
            Days = Convert.ToInt32((ExitDate - EnterDate).TotalDays);

            if (chk == "Service" || chk == "Package")
            {
                if (Code != null)
                {
                    ds = obj.GetServiceDetailByCode(Code);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ServiceGroupName = ds.Tables[0].Rows[0]["ServiceGroupName"].ToString();
                    }
                }
                else
                {
                    ds = obj.GetServiceDetailByName(prefix, id, chk);
                }

                DataSet dswordcharge = new DataSet();

                int i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dswordcharge = obj.GetServicesDetailsforIPDBills(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID));
                    if (dswordcharge.Tables[0].Rows.Count > 0)
                    {
                        dr["EmergencyCharges"] = dswordcharge.Tables[0].Rows[i]["EmergencyCharges"].ToString();
                        ServiceGroupName = dr["ServiceGroupName"].ToString();
                        dr["GeneralCharges"] = dswordcharge.Tables[0].Rows[i]["GeneralCharges"].ToString();
                        Unit = dswordcharge.Tables[0].Rows[0]["UnitID"].ToString();
                        if (Unit == "")
                        {
                            Unit = "0";
                        }
                        if (chk != "Package")
                        {
                            UnitName = dswordcharge.Tables[0].Rows[0]["UnitName"].ToString();
                            //  RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
                            //  HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
                            EmergencyCharges = dswordcharge.Tables[0].Rows[i]["EmergencyCharges"].ToString();
                            GeneralCharges = dswordcharge.Tables[0].Rows[i]["GeneralCharges"].ToString();
                        }
                        else
                        {
                            Unit = dswordcharge.Tables[i].Rows[0]["UnitID"].ToString();
                            UnitName = "";
                            //  RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
                            //  HideInBilling = ds.Tables[0].Rows[0]["HideInBilling"].ToString();
                            EmergencyCharges = dswordcharge.Tables[1].Rows[i]["EmergencyCharges"].ToString();
                            GeneralCharges = dswordcharge.Tables[1].Rows[i]["GeneralCharges"].ToString();
                            ServiceGroupName = dr["ServiceGroupName"].ToString();

                        }
                        string applywardID = ApplyWord(RegNo, IPDNo);
                        if (applywardID != "" && applywardID != "0")
                        {
                            WordID = applywardID;
                            dswordcharge = obj.GetServicesDetailsforIPDBills(Convert.ToInt32(dr["ServiceID"]), Convert.ToInt32(WordID));
                            EmergencyCharges = dswordcharge.Tables[0].Rows[0]["EmergencyCharges"].ToString();
                            GeneralCharges = dswordcharge.Tables[0].Rows[0]["GeneralCharges"].ToString();
                        }

                        if (dr["servicename"].ToString() == "BED CHARGES")
                        {


                            BedCharges(ExitDate, EnterDate, Convert.ToInt32(IPDNo), Convert.ToInt32(RegNo), Charges);
                            string nam = BedDate;
                            string m = add.MessageError;
                            if (BedDate == "")
                            {

                            }
                            dr["BillCharges"] = true;
                            dr["servicename"] = BedDate;  //dr["servicename"] + "--" + EnterDate.ToString("dd-MM-yyyy") + " To " + ExitDate.ToString("dd-MM-yyyy");
                        }
                    }
                    if (GeneralCharges == "")
                    {
                        GeneralCharges = "0";
                    }
                    if (EmergencyCharges == "")
                    {
                        EmergencyCharges = "0";
                    }
                    if (dr["HSNCode"].ToString() == "" || dr["HSNCode"] == null)
                    {
                        dr["HSNCode"] = "";
                    }

                    //---- PrivelageCard
                    if (dr["ServiceID"].ToString() == "1123")
                    {

                    }
                    if (Convert.ToBoolean(Session["PrivalageCard"]) == true)
                    {
                        Decimal Discount = DiscountPreVilege(dr, Convert.ToInt32(RegNo), chk);
                        GeneralCharges = Math.Round(Discount, 2, MidpointRounding.ToEven).ToString();
                        // Math.Round(2.3, 2,MidpointRounding.ToEven);
                    }

                    //End

                    searchList.Add(new OPDBill
                    {
                        ServiceGroupName = ServiceGroupName,
                        servicename = dr["servicename"].ToString(),
                        ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                        ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                        GeneralCharges = Math.Round(Convert.ToDecimal(GeneralCharges), 2, MidpointRounding.ToEven).ToString(),
                        EmergencyCharges = Math.Round(Convert.ToDecimal(EmergencyCharges), 2, MidpointRounding.ToEven).ToString(),
                        Unit = UnitName,
                        UnitID = Convert.ToInt16(Unit),
                        BillCharges = dr["BillCharges"].ToString(),
                        Quantity = Days.ToString(),
                        MessageError = MessageBed,
                        ServiceHSNCode = dr["HSNCode"].ToString(),
                        ServiceType = "IPD",
                        // ucPatientIPDBills1.atxtUnit.Text = ds.Tables[1].Rows[0]["UnitID"].ToString();
                        //RecommendedByDoctor = ds.Tables[1].Rows[0]["RecommendedByDoctor"].ToString();
                        //HideInBilling = ds.Tables[1].Rows[0]["HideInBilling"].ToString();
                    });

                    //i++;
                }
                if (id == "%")
                {
                    TESTBind(prefix);
                }
            }

            else
            {
                //  DataSet ds = new DataSet();
                string prefix1 = prefix;
                if (prefix != "%")
                {
                    prefix = "%";
                }
                ds = objtest.GetTest(prefix);
                if (id == "%")
                {
                    id = "0";
                }
                if (Convert.ToInt32(id) > 0)
                {
                    DataView dvServiceGroup = new DataView(ds.Tables[0], " CategoryID = " + id + "", "", DataViewRowState.CurrentRows);
                    if (dvServiceGroup.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow dr in dvServiceGroup.ToTable().Rows)
                        {


                            searchList.Add(new OPDBill
                            {
                                ServiceType = "Lab",
                                servicename = dr["servicename"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                                ServiceHSNCode = dr["HSNCode"].ToString(),
                                GeneralCharges = dr["GeneralCharges"].ToString(),
                                //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                                EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

                            });
                        }
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dvServiceGroup = new DataView(ds.Tables[0], "ServiceName like '" + prefix1 + "%'", "", DataViewRowState.CurrentRows);
                        // ds.Tables[0].DefaultView.RowFilter = "ServiceName like 'Karnataka'";
                        foreach (DataRow dr in dvServiceGroup.ToTable().Rows)
                        {


                            searchList.Add(new OPDBill
                            {
                                ServiceGroupName = dr["ServiceGroupName"].ToString(),
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
            }
            // PrivalageCard Service-------------------------------------------------------------------------------------
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (searchList.Count > 0)
                {
                    if (searchList[0].GeneralCharges == "")
                    {
                        searchList[0].GeneralCharges = "0";
                    }
                }
                //if (searchList.Count > 0)
                //{
                //    Decimal Discount = DiscountPreVilege(Convert.ToInt32(searchList[0].ServiceID), Convert.ToDecimal(searchList[0].GeneralCharges), Convert.ToInt32(RegNo), chk);
                //    if (Discount > 0)
                //    {
                //        if (Convert.ToBoolean(TempData["PrivalageCard"]) == true)
                //        {
                //            Decimal DWithRate = Convert.ToDecimal(searchList[0].GeneralCharges);
                //            DWithRate = DWithRate - (DWithRate * Discount / 100);
                //            searchList[0].servicename = searchList[0].servicename + " ( " + searchList[0].GeneralCharges + ",  " + Math.Round(Discount, 2) + "% )";
                //            searchList[0].GeneralCharges = DWithRate.ToString();
                //        }
                //    }
                //}
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
                        //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                        ServiceID = Convert.ToInt32(dr["ServiceID"]).ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        //ServiceGroupID = Convert.ToInt32(dr["ServiceGroupID"].ToString()),
                        EmergencyCharges = Convert.ToInt32(dr["EmergencyCharges"]).ToString()

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
                //DataView dvTest = new DataView(dsPatientPrivilege.Tables[2], " ServiceID = " + dr["ServiceID"] + "", "", DataViewRowState.CurrentRows);
                //if (dvTest.Count != 0)
                //{
                //    Discount = Convert.ToDecimal(dvTest[0]["DiscountPer"].ToString());
                //}
            }
            //ucPatientIPDBills1.atxtServiceName.Text = ucPatientIPDBills1.atxtServiceName.Text + " (" + ucPatientIPDBills1.txtRate.Text +" , "+ Discount.ToString() +" % ";

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

        public JsonResult GetPatientLabBillsForTestMaster(int ServiceID, int OrganizationID, int WordID)
        {
            BL_Patient_IPDBill obj = new BL_Patient_IPDBill();
            List<OPDBill> searchList = new List<OPDBill>();
            DataSet ds = new DataSet();

            ds = obj.GetPatientLabBillsForTestMaster(OrganizationID);
            DataView dv = new DataView(ds.Tables[0], " TestID = " + ServiceID + " and  WardID = " + WordID + " ", "", DataViewRowState.CurrentRows);

            if (dv.ToTable().Rows.Count > 0)
            {
                foreach (DataRow dr in dv.ToTable().Rows)
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
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDoctorName(string name)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = db.GetDoctorName(name), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBillDetailByEditToAllIPDNo(string PatientIPDNo, int BillNo)
        {

            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = db.FillDataOldBillEditToAll(PatientIPDNo, BillNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetPatientBillsForIPDNo(int BillNo, string gross_total, string pre_balance_id, string NonMedicalExp, string total_amount, string disc_perc_id, string disc_amount_id, string netpayable_id, string amtpaid_id, string bal_amount_id,int DiscountReasonID)
        {


            DataSet dt1 = (DataSet)Session["GetPatientBillsForIPDNosection"];

            foreach (DataRow dr in dt1.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["BillNo"]) == BillNo)
                {
                    dr["GrossAmount"] = gross_total;
                    dr["TotalAmount"] = total_amount;
                    dr["DiscountPercent"] = total_amount;
                    dr["DiscountAmount"] = disc_amount_id;
                    dr["NetPayableAmount"] = netpayable_id;
                    dr["DiscountReason"] = DiscountReasonID;
                    dr["BalanceAmount"] = bal_amount_id;
                //    dr["DipositAmount"]
                    dr["PreBalanceAmount"] = pre_balance_id;
                    dr["NonMedicalExp"] = NonMedicalExp;
                    dr["PaidAmount"] = amtpaid_id;
                }
            }
            Session["GetPatientBillsForIPDNosection"] = dt1;
            // DataSet ds=     (DataSet)Session["GetPatientBillsForIPDNosection1"];
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetBillDetailByBillNo(string BillNo, string chk)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = db.GetBillDetailByBillNo(BillNo, chk), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RptPatientIPDBill()
        {
            return View();
        }

        public JsonResult OldBillReport(string IPDNo, string BillType, string BillNo, string RegNo, string chkReport)
        {
            HospitlLocationID();
            if (chkReport == null)
            {
                chkReport = "1";
            }
            if (BillType.Trim() == "IPDFinalBill")
            {
                Session["HospitalIDReport"] = HospitalID;
                Session["LocationIDReport"] = LocationID;
                Session["PatientRegNoReport"] = RegNo.ToString();
                Session["PatientIPDNoReport"] = IPDNo.ToString();
                Session["RadioReport"] = chkReport;
            }
            if (BillType.Trim() == "IPDBill")
            {
                Session["HospitalIDReport"] = HospitalID;
                Session["LocationIDReport"] = LocationID;
                Session["BillIDReport"] = BillNo.ToString();
            }
            if (BillType == "Provisional")
            {
                Session["HospitalIDReport"] = HospitalID;
                Session["LocationIDReport"] = LocationID;
                Session["ProvisionalReportBillNo"] = BillNo.ToString();
                Session["RadioReport"] = chkReport;
            }
            return new JsonResult { Data = BillType, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetPatientForIPDNoBills(string IPDNo)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();

            return new JsonResult { Data = db.GetPatientForIPDNoBills(IPDNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult SearchPrintBillNo(int BillNo, string BillType)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();

            return new JsonResult { Data = db.SearchPrintBillNo(BillNo, BillType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult Delete(int PatientRegNO, int BillNo, string txtinput)
        {
            string del = "";

            HospitlLocationID();
            connection();

         
            BL_PatientOPDBill obj = new BL_PatientOPDBill();
            bool ChkAuth = obj.ForAuthorizationGetMasterSetting();
            SqlCommand cmd;
            if (ChkAuth)
            {
                AuthoriseSave(BillNo);
               
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
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
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
        public JsonResult GetBalanceAmountRegistration(string PatientRegNo)
        {
            BL_Patient_IPDBill db = new BL_Patient_IPDBill();
            return new JsonResult { Data = db.GetBalanceAmountRegistration(PatientRegNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // Authorise



       BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();

        public void AuthoriseSave(int BillNo)
        {
          
            BL_Patient_IPDBill BL_obj = new BL_Patient_IPDBill();
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
                    //   con.Close();
                    con.Open();
                    int IUPatientBillsModify = cmdBillModify.ExecuteNonQuery();

                }
                //con.Close();



                

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
                    // con.Close();
                    // con.Open();

                    int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();

                    // con.Close();
                }
            }


            










        //---------------












        //  Transfer OPDIPD

        public JsonResult IURefoundAmount(string Reg, string OPDIPDNOFrom, string OPDIPDNOTO, string RefundAmt, string OPDIPDNOTOpPrint)
        {
            BL_Deposit BL_obj1 = new BL_Deposit();
            Deposit objdepo = new Models.Patient.Deposit();
            objdepo.PatientRegNo = Reg;
            objdepo.BillDate = DateTime.Now;
            objdepo.PaymentType = "Transfer  " + OPDIPDNOFrom + " To " + OPDIPDNOTO;
            objdepo.PatinetType = "IPDBill";
            objdepo.OPDIPDNO = OPDIPDNOFrom.Split('-')[0];
            objdepo.PaidAmount = Convert.ToDecimal(RefundAmt.Replace("-", ""));
            objdepo.BillType = "RefundBills";
            int OtherAccountRowID = BL_obj1.IURefoundAmount(objdepo);


            //----

            BL_PrebalanceAmount BL_obj = new BL_PrebalanceAmount();
            PreBalanceAmount objPre = new Models.Patient.PreBalanceAmount();
            objPre.PatientRegNo = Reg;
            objPre.BillDate = DateTime.Now.ToString();
            objPre.PaymentType = "Transfer  " + OPDIPDNOFrom + " To " + OPDIPDNOTO; ;
            objPre.PatinetType = "IPDBill";
            objPre.OPDIPDNO = OPDIPDNOTO;
            objPre.OPDIPDID = OPDIPDNOTO.Split('-')[0];
            objPre.PaidAmount = RefundAmt.Replace("-", "");
            objPre.Name = "";
            objPre.Date = "";
            objPre.Remarks = "";
            objPre.Number = "";
            objPre.Mode = "Add";
            objPre.BillType = "IPD PreBalanceBills";
            //  objPre =  BL_obj.SavePrebalAmt(objPre);

            //-----------------------------------------------

            connection();
            HospitlLocationID();

            SqlCommand cmd = new SqlCommand("IUPreBalanceAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@TpaParticular", "");

            if (objPre.Mode != "Add")
            {
                cmd.Parameters.AddWithValue("@PatientAccountRowID", objPre.PatientAccountRowID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PatientAccountRowID", 0);
                cmd.Parameters["@PatientAccountRowID"].Direction = ParameterDirection.Output;
            }
            cmd.Parameters.AddWithValue("@PatientRegNo", objPre.PatientRegNo);
            if (Convert.ToString(objPre.OPDIPDID) == null)
            {
                cmd.Parameters.AddWithValue("@OPDIPDID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OPDIPDID", objPre.OPDIPDID);
            }

            cmd.Parameters.AddWithValue("@PatinetType", "IPD");


            cmd.Parameters.AddWithValue("@PaymentType", objPre.PaymentType);
            cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(objPre.BillDate));

            cmd.Parameters.AddWithValue("@PaidAmount", objPre.PaidAmount);
            if (objPre.Narrection == null)
            {
                cmd.Parameters.AddWithValue("@Narrection", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("@Narrection", objPre.Narrection);
            }
            if (objPre.TDSAmount != null)
            {

                cmd.Parameters.AddWithValue("@TDSAmount", objPre.TDSAmount);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TDSAmount", "");
            }
            if (objPre.TPAOtherDeduction != null)
            {
                cmd.Parameters.AddWithValue("@TPAOtherDeduction", objPre.TPAOtherDeduction);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TPAOtherDeduction", "");
            }


            #region PaymentType

            switch (objPre.PaymentType)
            {
                case "Cheque":
                    cmd.Parameters.AddWithValue("@Number", objPre.Number);
                    cmd.Parameters.AddWithValue("@Name", objPre.Name);
                    cmd.Parameters.AddWithValue("@Remarks", objPre.Remarks);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(objPre.Date));
                    break;

                case "Debit":
                    cmd.Parameters.AddWithValue("@Number", objPre.Number);
                    cmd.Parameters.AddWithValue("@Name", objPre.Name);
                    cmd.Parameters.AddWithValue("@Remarks", objPre.Remarks);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(objPre.Date));
                    break;

                case "Credit":
                    cmd.Parameters.AddWithValue("@Number", objPre.Number);
                    cmd.Parameters.AddWithValue("@Name", objPre.Name);
                    cmd.Parameters.AddWithValue("@Remarks", objPre.Remarks);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(objPre.Date));
                    break;

                case "E-Money":
                    cmd.Parameters.AddWithValue("@Number", objPre.Number);
                    cmd.Parameters.AddWithValue("@Name", objPre.Name);
                    cmd.Parameters.AddWithValue("@Remarks", objPre.Remarks);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(objPre.Date));
                    break;

                case "EFT":
                    cmd.Parameters.AddWithValue("@Number", objPre.Number);
                    cmd.Parameters.AddWithValue("@Name", objPre.Name);
                    cmd.Parameters.AddWithValue("@Remarks", objPre.Remarks);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(objPre.Date));
                    break;

                default:
                    cmd.Parameters.AddWithValue("@Number", "Cash");
                    cmd.Parameters.AddWithValue("@Name", "Cash");
                    cmd.Parameters.AddWithValue("@Remarks", "Cash");
                    cmd.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                    break;
            }
            #endregion

            cmd.Parameters.AddWithValue("@CreationID", CreationID);
            cmd.Parameters.AddWithValue("@Mode", "Add");
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            int PatientAccountRowID = Convert.ToInt32(cmd.Parameters["@PatientAccountRowID"].Value);












            //-------------------------------------------------
            OtherAccountRowID = PatientAccountRowID;
            if (OtherAccountRowID > 0)
            {
                return new JsonResult { Data = "Done", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                return new JsonResult { Data = "Rong", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }

        public ActionResult BindGroupOfReport(string chk, int Reg, int patientPrintIPDNo, int BillNo, string srviceGroupID)
        {

            BL_Patient_IPDBill obj_PatientIPDBills_R = new BL_Patient_IPDBill();
            DataSet dsGroupReport = new DataSet();
            dsGroupReport.Reset();
            int PatientIPDOPDNO = Convert.ToInt32(patientPrintIPDNo);

            List<ServiceName> serchadd = new List<ServiceName>();

            if (chk == "3")
            {
                // if (ucPatientIPDBills1.cmbOldBillsNo.SelectedIndex > 1)
                // {
                dsGroupReport = obj_PatientIPDBills_R.GetPatientServiceGroup(Convert.ToInt32(Reg), PatientIPDOPDNO, srviceGroupID, "SaveView");
                if (dsGroupReport.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGroupReport.Tables[1].Rows)
                    {
                        serchadd.Add(new ServiceName
                        {
                            SvcName = dr["ServiceGroupName"].ToString(),
                            sevicedisAmt = dr["Amount"].ToString(),
                            ServiceGroupID = dr["ServiceGroupID"].ToString(),
                        });
                    }
                    //ucPatientIPDBills1.dgvReprintGroupBoxReport.DataSource = dsGroupReport.Tables[1];
                    //// ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["Sr"].Visible = false;
                    //ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["A"].Visible = false;
                    //ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["ChargesType"].Visible = false;
                }
                // }
            }
            else
            {


                dsGroupReport = obj_PatientIPDBills_R.GetPatientServiceGroupProvisionalWise(BillNo, srviceGroupID, "SaveView");
                if (dsGroupReport.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGroupReport.Tables[2].Rows)
                    {
                        serchadd.Add(new ServiceName
                        {
                            SvcName = dr["ServiceGroupName"].ToString(),
                            sevicedisAmt = dr["TotalAmount"].ToString(),
                            ServiceGroupID = dr["ServiceGroupID"].ToString(),
                        });
                    }
                    //ucPatientIPDBills1.dgvReprintGroupBoxReport.DataSource = dsGroupReport.Tables[2];
                    //// ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["Sr"].Visible = false;
                    //ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["A"].Visible = false;
                    ////ucPatientIPDBills1.dgvReprintGroupBoxReport.Columns["ChargesType"].Visible = false;
                }

            }
            return new JsonResult { Data = serchadd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult btnGroupReport_Click(string chk, int PatientRegNO, int PatientIPDOPDNO, string ServiceGroupID, int BillNo)
        {
            BL_Patient_IPDBill obj_PatientIPDBills_R = new BL_Patient_IPDBill();

            string ReportTypeName = "SaveView";

            DataSet ds = new DataSet();
            Session["chksplit"] = chk;
            if (chk == "3")//final
            {
                //ds = obj_PatientIPDBills_R.GetPatientServiceGroup(Convert.ToInt32(PatientRegNO), PatientIPDOPDNO, ServiceGroupID, "SaveView");
                //string chk11 = "";
                //int row = 0;

                //foreach (DataRow dr in ds.Tables[1].Rows)
                //{

                //    if (Convert.ToBoolean(ucPatientIPDBills1.dgvReprintGroupBoxReport.Rows[row].Cells["chk"].Value) == true)
                //    {
                //        chk11 += ds.Tables[1].Rows[row]["ServiceGroupID"].ToString() + ",";

                //    }
                //    row++;
                //}

                DataSet dsnew = obj_PatientIPDBills_R.GetPatientServiceGroup(Convert.ToInt32(PatientRegNO), PatientIPDOPDNO, ServiceGroupID, "SaveView");
                Session["RptPatientIPDFinalBillsGroupWise"] = dsnew;

                // obj_RptPatient.RptPatientIPDFinalBillsGroupWise(dsnew, ReportTypeName);

                // obj_RptPatient.RptPatientIPDFinalBillsGroupWise1(dsnew, ReportTypeName);
                // }
            }
            else
            {

                //int row = ucPatientIPDBills1.dgvOldBillReprint.Rows.Count - 1;

                //int BillNo = Convert.ToInt32(ucPatientIPDBills1.dgvOldBillReprint.Rows[row].Cells["BillNo"].Value);
                //ds = obj_PatientIPDBills_R.GetPatientServiceGroupProvisionalWise(HospitalID, LocationID, BillNo, ServiceGroupID, "SaveView");
                //string chk11 = "";
                //int row1 = 0;
                //foreach (DataRow dr in ds.Tables[2].Rows)
                //{
                //    if (Convert.ToBoolean(ucPatientIPDBills1.dgvReprintGroupBoxReport.Rows[row1].Cells["chk"].Value) == true)
                //    {
                //        chk11 += ds.Tables[2].Rows[row1]["ServiceGroupID"].ToString() + ",";

                //    }
                //    row1++;
                //}

                DataSet dsnew = obj_PatientIPDBills_R.GetPatientServiceGroupProvisionalWise(BillNo, ServiceGroupID, "SaveView");
                Session["GetPatientServiceGroupProvisionalWise"] = dsnew;
                //  obj_RptPatient.RptPatientIPDProvisionalBillsGroupWise(dsnew, ReportTypeName);
                // obj_RptPatient.RptPatientIPDProvisionalBillsGroupWise1(dsnew, ReportTypeName);
            }
            return new JsonResult { Data = "Done", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult RptSplit()
        {
            return View();
        }

        public JsonResult GetIPDPatient(string prefix, bool outside)
        {
            BL_Patient_IPDBill _IPDBill = new BL_Patient_IPDBill();
            DataSet ds = _IPDBill.GetIPDPatient(prefix);
            List<PatientOPDBill> searchList = new List<PatientOPDBill>();
            // if (outside == false)
            //  {
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
