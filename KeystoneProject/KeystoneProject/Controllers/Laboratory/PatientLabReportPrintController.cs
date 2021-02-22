using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Laboratory;
using KeystoneProject.Buisness_Logic.Laboratory;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mail;
using System.IO;
using System.Net.Mail;
using CrystalDecisions.CrystalReports.Engine;

namespace KeystoneProject.Controllers.Laboratory
{
    public class PatientLabReportPrintController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID ;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
        }
       
        List<PatientLabReportPrint> PatientLabReportList = new List<PatientLabReportPrint>();
        DataSet dsMultipleTest = new DataSet();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        BL_PatientLabReportPrint BL_obj = new BL_PatientLabReportPrint();
        public ActionResult PatientLabReportPrint()
        {
            return View();
        }


        public JsonResult SendMail(string Lab, string testid)
         {

            

            var tosend = "saurabhajsingh24@gmail.com";
            string path = "";
            PatientLabReportPrint rptPatientLabReports = new PatientLabReportPrint();
            DataSet ds = BL_obj.SendMail(Lab, testid);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    EmailID = dr["EmailID"].ToString(),
                    EmailPassword = dr["EmailPassword"].ToString()
                });
            }

              

            ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report
            crystalReport.Load(Path.Combine(Server.MapPath("~/Report"),  "RptPatientLabReports.rpt")); // path of report 
            crystalReport.SetDataSource(ds); // binding datatable
            


           
            //SetDataSource(ds);

            int email1 = 0;
            string hospitalname = ds.Tables[0].Rows[0]["HospitalName"].ToString();
            string patname = ds.Tables[1].Rows[0]["PatientName"].ToString();

          

            path = "c:\\LabReports" + "ss"+".pdf";
            email1++;
            //CrystalDecisions.Shared.ExportOptions CrExportOptions;
            CrystalDecisions.Shared.DiskFileDestinationOptions CrDiskFileDestinationOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            CrystalDecisions.Shared.PdfRtfWordFormatOptions CrFormatTypeOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();

            var filename = Path.GetFileName(crystalReport.FileName);
            var extention = Path.GetExtension(crystalReport.FileName);
            var filenamethoutextenction = Path.GetFileNameWithoutExtension(crystalReport.FileName);

            CrDiskFileDestinationOptions.DiskFileName = crystalReport.FileName;
            //CrExportOptions = rptPatientLabReports.ExportOptions;
            //{
            //    CrExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            //    CrExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
            //    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //    CrExportOptions.FormatOptions = CrFormatTypeOptions;
            //}
            //rptPatientLabReports.Export();

            email1++;
            


            string email = ds.Tables[0].Rows[0]["EmailID"].ToString();
            string pass = ds.Tables[0].Rows[0]["EmailPassword"].ToString();

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(email);
            mail.To.Add("guptagovind517@gmail.com");
            // mail.Subject = txtSubject.Text;
            mail.Attachments.Add(new Attachment(@"C:\Users\Saurabh\Downloads\Nilesh (1).pdf"));



            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("saurabhajsingh24@gmail.com", "saurabh@123");
            SmtpServer.EnableSsl = true;

            SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Send(mail);


           
          
            bool result = false;
            return Json(result, JsonRequestBehavior.AllowGet);

        }

      
        //[HttpPost]
        //public ActionResult PrintReport(PatientLabReportPrint objprint)
        //{
        //    try
        //    {

                 
        //        if (Request.Form["ServiceID1"].ToString() == "")
        //        {
        //            //objServicePackage.serviceID1 = 0;
        //        }
        //        else
        //        {
        //           // objServicePackage.serviceID1 = Convert.ToInt32(Request.Form["ServiceID1"].ToString());
        //        }


        //        if (BL_obj.Save(objprint))
        //        {
        //            ModelState.Clear();
        //            ViewData["flag"] = "Done";
        //        }

        //        return RedirectToAction("PatientLabReportPrint", "PatientLabReportPrint");

        //    }
        //    catch (Exception Ex)
        //    {
        //        return RedirectToAction("PatientLabReportPrint", "PatientLabReportPrint");
        //    }



        //}
        public JsonResult GetCategoryBind(string prefix)
        {

            DataSet ds = BL_obj.GetCategory(prefix);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    CategoryID =  dr["CategoryID"].ToString(),
                    CategoryName = dr["CategoryName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RptPatientLabReports()
        {
            return View();
        }
        public JsonResult GetTestNameBind(string prefix)
        {

            DataSet ds = BL_obj.GetTestName(prefix);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    TestID = dr["TestID"].ToString(),
                    TestName = dr["TestName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDepartmentBind(string prefix)
        {

            DataSet ds = BL_obj.GetDepartment(prefix);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    DepartmentID =dr["DepartmentID"].ToString(),
                    DepartmentName = dr["DepartmentName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetDoctorBind(string prefix)
        {

            DataSet ds = BL_obj.GetDoctor(prefix);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    DoctorID =dr["DoctorID"].ToString(),
                    DoctorName = dr["DoctorPrintName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientBind(string prefix)
        {

            DataSet ds = BL_obj.GetPatient(prefix);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    PatientRegNo = dr["PatientRegNo"].ToString(),
                    PatientName = dr["PatientName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetAllShowPatientLabReports(string FromDate, string ToDate, string PatientRegNo, string LabNo, string DoctorID)
        {
            PatientLabReportPrint obj = new PatientLabReportPrint();

           string CategoryID = Request.Form["CategoryID"];
            if(CategoryID=="") 
                obj.CategoryID = ""; 
            else 
                obj.CategoryID = CategoryID; 

            string DepartmentID = Request.Form["DepartmentID"];
            if (DepartmentID == "") 
                obj.DepartmentID = ""; 
            else 
                obj.DepartmentID = DepartmentID;

         
            if (PatientRegNo == "")
            { 
                PatientRegNo = "%";
            }

            if (LabNo == "")
            {
                LabNo = "%";
            }
            if (DoctorID == "")
            {
                DoctorID = "%";
            }

           
            obj.FromDate =FromDate;
            obj.ToDate = ToDate;
            
            DataSet ds = BL_obj.GetAllPatientLabReports(FromDate,ToDate, PatientRegNo, LabNo, DoctorID);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientLabReportPrint
                {
                    LabNo = dr["LabNo"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    LabDate = dr["LabDate"].ToString(),
                    LabType = dr["LabType"].ToString(),
                    DoctorName = dr["DoctorName"].ToString()

                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientLabBillDetails(string LabNo)
        {
           // LabNo = Request.Form["LabNoHidden"];
            DataSet ds = BL_obj.GetPatientLabBillDetails(LabNo);

            List<PatientLabReportPrint> searchlist = new List<PatientLabReportPrint>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string labno = "";
                if (dr["LabNo"].ToString() == "")
                {
                    labno = "";
                }
                else
                {
                    labno = dr["LabNo"].ToString();
                }
                string TestName = "";
                if (dr["TestName"].ToString()=="")
                {
                    TestName = "";
                }
                else
                {
                    TestName = dr["TestName"].ToString();
                }

                string TestStatus = "";
                if (dr["TestStatus"].ToString() == "")
                {
                    TestStatus = "";
                }
                else
                {
                    TestStatus = dr["TestStatus"].ToString();
                }

                string TestType = "";
                if (dr["TestType"].ToString() == "")
                {
                    TestType = "";
                }
                else
                {
                    TestType = dr["TestType"].ToString();
                }
                searchlist.Add(new PatientLabReportPrint
                {
                    TestID = dr["TestID"].ToString(),
                    LabNo = labno,
                    TestName = TestName,
                    TestStatus = TestStatus,
                    TestType = TestType
        

                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Test(string LabNo, string TestIDNo, string TestType, string chkgrp, string chknotgrp)
        {
            DataSet ds = new DataSet();
            string ResultValue = "0";    
            if(chkgrp=="true")
            {
                Session["chkgrp"] = chkgrp;
                ResultValue = "1";    
            }
            else
            {
                Session["chkgrp"] = false;
            }
          
                                    
            if(chknotgrp=="true")
            {
                Session["chknotgrp"] = chknotgrp;
                Boolean check;
                PatientLabReportPrint obj = new Models.Laboratory.PatientLabReportPrint();
                //check = obj.GetRptPatientLabReports(HospitalID, LocationID, LaboratoryNo, ID, Convert.ToBoolean(chk3), print);

                //  check = obj.GetRptPatientLabReports(HospitalID, LocationID, Convert.ToInt32(Labid), TestIDNo, Convert.ToBoolean(CheckB), print);

                //  string ch = check.ToString();
                HospitlLocationID();

                string[] TestID1 = TestIDNo.Split(',');
                TestID1 = TestID1.Where(name => !string.IsNullOrEmpty(name)).ToArray();
                Connect();

                for (int i = 0; i < TestID1.Length; i++)
                {


                    SqlCommand cmd = new SqlCommand("RptPatientLabReports", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@LabNo", LabNo);
                    cmd.Parameters.AddWithValue("@TestID", TestID1[i]);
                    Session["HospitalIDReport"] = HospitalID;
                    Session["LocationIDReport"] = LocationID;
                    Session["LabNo"] = LabNo;
                    Session["TestID"] = TestIDNo;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //  con.Open();
                    da.Fill(ds);
                    ResultValue = "1";
                }
            }

            else
            {
                Session["chknotgrp"] = false;
            }
        

              


            //if (ds.Tables[2].Rows.Count != 0)
            //{
            //    string ResultValue = "0";
            //    foreach (DataRow dr in ds.Tables[2].Rows)
            //    {
            //        // ResultValue = "0";
            //        if (dr["ResultValue"].ToString() != "")
            //        {
            //            ResultValue = "1";
            //        }
            //    }
            //    //  return RedirectToAction("RptPatientLabReports");
            //    // return RedirectToAction("RptPatientLabReports");

            //    // var redirectUrl = new UrlHelper(Request.RequestContext).Action("RptPatientLabReports", "Patient_Lab_Advance_Search");
            //    // return Json(new { Url = redirectUrl });

                if (ResultValue == "1")
                {
                 //   var redirectUrl = new UrlHelper(Request.RequestContext).Action("RptPatientLabReports", "PatientReport");
                    return Json("1", JsonRequestBehavior.AllowGet);
                   // string ch = redirectUrl;
                //    return Json(ch, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Blank", JsonRequestBehavior.AllowGet);
                }
        //    }
          //  else
           // {
             //   return Json("Blank", JsonRequestBehavior.AllowGet);
           // }

        }
     
       
        public ActionResult TestForMultiple(string LabFormultiple, string testidformultiple, string TestTypeformultiple)
        
        {
            Boolean check;
            PatientLabReportPrint obj = new Models.Laboratory.PatientLabReportPrint();
            //check = obj.GetRptPatientLabReports(HospitalID, LocationID, LaboratoryNo, ID, Convert.ToBoolean(chk3), print);

            //  check = obj.GetRptPatientLabReports(HospitalID, LocationID, Convert.ToInt32(Labid), TestIDNo, Convert.ToBoolean(CheckB), print);

            //  string ch = check.ToString();
            HospitlLocationID();
            Connect();
            SqlCommand cmd = new SqlCommand("RptPatientLabReports", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@LabNo", LabFormultiple);
            cmd.Parameters.AddWithValue("@TestID", testidformultiple);
          //  Session["HospitalIDReport"] = HospitalID;
          //  Session["LocationIDReport"] = LocationID;
          //  Session["LabNo"] = LabNo;
        //    Session["TestID"] = TestIDNo;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //  con.Open();
            da.Fill(ds);

            DataTable table = Session["Table"] as DataTable;
            DataTable table1 = Session["Table1"] as DataTable;
            DataTable table2 = Session["Table2"] as DataTable;
            DataTable table3 = Session["Table3"] as DataTable;
            //DataTable table4 = Session["table4"] as DataTable;
            //DataTable table5 = Session["table5"] as DataTable;

            if(table!=null)
            {
                DataTable dtCopy = table.Copy();
                dsMultipleTest.Tables.Add(dtCopy);
              
            }
            if (table1 != null)
            {
                DataTable dtCopy = table1.Copy();
                dsMultipleTest.Tables.Add(dtCopy);

            }
            if (table2 != null)
            {
                DataTable dtCopy = table2.Copy();
                dsMultipleTest.Tables.Add(dtCopy);

            }
            if (table3 != null)
            {
                DataTable dtCopy = table3.Copy();
                dsMultipleTest.Tables.Add(dtCopy);

            }
            //if (table4 != null)
            //{
            //    DataTable dtCopy = table4.Copy();
            //    dsMultipleTest.Tables.Add(dtCopy);

            //}
            //if (table5 != null)
            //{
            //    DataTable dtCopy = table5.Copy();
            //    dsMultipleTest.Tables.Add(dtCopy);

            //}
          


                if (dsMultipleTest.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        dsMultipleTest.Tables[2].ImportRow(dr);
                       
                    }
                    Session["Table"] = dsMultipleTest.Tables[0];
                    Session["Table1"] = dsMultipleTest.Tables[1];
                    Session["Table2"] = dsMultipleTest.Tables[2];
                    Session["Table3"] = dsMultipleTest.Tables[3];
                //    Session["table4"] = dsMultipleTest.Tables[4];
                //    Session["table5"] = dsMultipleTest.Tables[5];
                }
                else
                {
                    dsMultipleTest = ds.Copy();
                    Session["Table"] = dsMultipleTest.Tables[0];
                    Session["Table1"] = dsMultipleTest.Tables[1];
                    Session["Table2"] = dsMultipleTest.Tables[2];
                    Session["Table3"] = dsMultipleTest.Tables[3];
                    //Session["table4"] = dsMultipleTest.Tables[4];
                    //Session["table5"] = dsMultipleTest.Tables[5];
                 //   DataTable table = Session["table"] as DataTable;

                    
                    //  ds = dsMultipleTest.Copy();
                }

            
            return Json("", JsonRequestBehavior.AllowGet);


        }

        public DataSet datatable { get; set; }
    }
}