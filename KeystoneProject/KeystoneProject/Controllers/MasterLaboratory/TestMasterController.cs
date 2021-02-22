using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;

using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
//\\using ModuleLoader;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Diagnostics;
using System.Drawing;
using Global;
////using WIA;
using System.Net.Mail;


namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class TestMasterController : Controller
    {
        //
        // GET: /TeatMaster/
       
       
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        string ImageFile = "";
       public void HospitalLocation()
        {
             HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
             LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
             UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        }
        BL_TestMaster _TestMaster = new BL_TestMaster();
        public ActionResult TestMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestMaster(TestMaster obj,FormCollection fc)
        
        {
             try
            {
            BL_TestMaster _TestMaster = new BL_TestMaster();
            if (_TestMaster.CheckTestMaster(obj.TestID, obj.TestName))
            {
            if(obj.TestName!=null)
            {
                obj.TestName = fc["TestName"].ToString();
            }
            else
            {
                obj.TestName = "";
            }
            if (obj.HSNCode != null)
            {
                obj.HSNCode = fc["HSNCode"].ToString();
            }
            else
            {
                obj.HSNCode = "";
            }
           if(obj.TestID==null)
           {
               obj.TestID = "";
           }
           else
           {
               obj.TestID = fc["TestID"].ToString();
           }
           

            if (obj.TestType != "" || obj.TestType != null)
            {
                obj.TestType = fc["TestType"].ToString();
                if (obj.TestType == "1")
                {
                    obj.HeaderID = "";
                }
                else if (obj.TestType == "2")
                {
                    obj.HeaderID = "";
                }
                else if(obj.TestType == "3")
                {
                    obj.HeaderID = "";
                }
               
            }
            else
            {
                obj.TestType = "";
            }

            //if (obj.UploadFile != "" || obj.UploadFile != null)
            //{
            //    obj.UploadFile = fc["UploadFile"].ToString();
            //}
            //else
            //{
            //    obj.UploadFile = "";
            //}


            if (obj.PrintAs != "" || obj.PrintAs != null)
            {
                obj.PrintAs = fc["PrintAs"].ToString();
            }
            else
            {
                obj.PrintAs = "";
            }

            if (obj.Methord != "" || obj.Methord != null)
            {
                obj.Methord = fc["Method"].ToString();
            }
            else
            {
                obj.Methord = "";
            }

            if (obj.Category != "" || obj.Category != null)
            {
                obj.Category = fc["Category"].ToString();
            }
            else
            {
                obj.Category = "";
            }

            if (obj.CategoryID != "" || obj.CategoryID != null)
            {
                obj.CategoryID = fc["CategoryID"].ToString();
            }
            else
            {
                obj.CategoryID = "";
            }

            if (obj.Alias != "" || obj.Alias != null)
            {
                obj.Alias = fc["Alias"].ToString();
            }
            else
            {
                obj.Alias = "";
            }

            if (obj.ForGender != "" || obj.ForGender != null)
            {
                obj.ForGender = fc["Gender"].ToString();
            }
            else
            {
                obj.ForGender = "";
            }

            if (obj.Format != "" || obj.Format != null)
            {
                obj.Format = fc["Format"].ToString();
            }
            else
            {
                obj.Format = "";
            }

            if (obj.Client != "" || obj.Client != null)
            {
                obj.Client = fc["Client"].ToString();
            }
            else
            {
                obj.Client = "";
            }

            if (obj.TechnicianIntr != "" || obj.TechnicianIntr != null)
            {
                obj.TechnicianIntr = fc["Technician"].ToString();
            }
            else
            {
                obj.TechnicianIntr = "";
            }

            if (obj.SampleType != "" || obj.SampleType != null)
            {
                obj.SampleType = fc["SampleType"].ToString();
            }
            else
            {
                obj.SampleType = "";
            }

            if (obj.HMSCode != "" || obj.HMSCode != null)
            {
                obj.HMSCode = fc["HMSCode"].ToString();
            }
            else
            {
                obj.HMSCode = "";
            }

            if (obj.ProcessBy != "" || obj.ProcessBy != null)
            {
                obj.ProcessBy = fc["ProcessBy"].ToString();
            }
            else
            {
                obj.ProcessBy = "";
            }
           
                obj.PrintName = "false";
                obj.WardView = "false";
                obj.BothFooter = "false";
                obj.OutsideLab = "false";
                obj.PrintTogether = "false";
         

            if (obj.PatientInstr != "" || obj.PatientInstr != null)
            {
                obj.PatientInstr = fc["PatientInstr"].ToString();
            }
            else
            {
                obj.PatientInstr = "";
            }

            if (obj.Interpretation != "" || obj.Interpretation != null)
            {
                obj.Interpretation = fc["Interpretation"].ToString();
            }
            else
            {
                obj.Interpretation = "";
            }

            if (obj.DisposalDay != "" || obj.DisposalDay != null)
            {
                obj.DisposalDay = fc["DisposalDays"].ToString();
            }
            else
            {
                obj.DisposalDay = "";
            }

            if (obj.IncludeforTPA != "" || obj.IncludeforTPA != null)
            {
                obj.IncludeforTPA = fc["IncludeForTPA"].ToString();
            }
            else
            {
                obj.IncludeforTPA = "";
            }

            if (obj.MachinName != "" || obj.MachinName != null)
            {
                obj.MachinName = fc["MachineName"].ToString();
            }
            else
            {
                obj.MachinName = "";
            }

            if (obj.Footer != "" || obj.Footer != null)
            {
                obj.Footer = fc["Footer"].ToString();
            }
            else
            {
                obj.Footer = "";
            }
            if (obj.PrintOrder != "" || obj.PrintOrder != null)
            {
                obj.PrintOrder = fc["PrintOrder"].ToString();
            }
            else
            {
                obj.PrintOrder = "";
            }
            obj.Discount = "0";

            if (obj.OPDGeneralCharges != "" || obj.OPDGeneralCharges != null)
            {
                obj.OPDGeneralCharges = fc["OPDGeneralCharges"].ToString();
            }
            else
            {
                obj.OPDGeneralCharges = "0.00";
            }

            if (obj.OPDEmergencyCharges != "" || obj.OPDEmergencyCharges != null)
            {
                obj.OPDEmergencyCharges = fc["OPDEmergencyCharges"].ToString();
            }
            else
            {
                obj.OPDEmergencyCharges = "0.00";
            }

            if (obj.ReffCommission != "" || obj.ReffCommission != null)
            {
                obj.ReffCommission = fc["ReffComission"].ToString();
            }
            else
            {
                obj.ReffCommission = "0.00";
            }

            if (obj.ReffCommissionRs != "" || obj.ReffCommissionRs != null)
            {
                obj.ReffCommissionRs = fc["sign"].ToString();
            }
            else
            {
                obj.ReffCommissionRs = "";
            }

            if (obj.Commission != "" || obj.Commission != null)
            {
                obj.Commission = fc["Comission"].ToString();
            }
            else
            {
                obj.Commission = "0.00";
            }

            if (obj.CommissionRs != "" || obj.CommissionRs != null)
            {
                obj.CommissionRs = fc["sign1"].ToString();
            }
            else
            {
                obj.CommissionRs = "";
            }
            if(obj.WardID==null)
            {
                obj.WardID = "0";
            }
            else
            {
                obj.WardID = fc["WardID"].ToString();
            }
            if (obj.WardName == null)
            {
                obj.WardName = "";
            }
            else
            {
                obj.WardName = fc["WardName"].ToString();
            }
          if(obj.TestMasterDetailIPDChargeID==null)
          {
              obj.TestMasterDetailIPDChargeID = "";
          }
          else
          {
              obj.TestMasterDetailIPDChargeID = fc["TestMasterDetailIPDChargeID"].ToString();
          }
            
            if(obj.TestTPAWiseID == "" )
            {
                obj.TestTPAWiseID = "";
            }
            else
            {
                obj.TestTPAWiseID = "";
            }
            if(obj.TestDetailsTPAWiseID=="")
            {
                obj.TestDetailsTPAWiseID = "";
            }
            else
            {
                obj.TestDetailsTPAWiseID = "";
            }
            if(obj.TestDetailsID=="")
            {
                obj.TestDetailsID = "";
            }
            else
            {
                obj.TestDetailsID = "";
            }

            if (fc["EmergencyCharges"].ToString() == null || fc["EmergencyCharges"].ToString() == "")
            {
                obj.EmergencyIPDCharges = "0";
            }
            else
            {
                obj.EmergencyIPDCharges = fc["EmergencyCharges"].ToString();
            }

            if (fc["GeneralCharges"].ToString() == null || fc["GeneralCharges"].ToString() == "")
            {
                obj.GenralWardIDPCharges = "0";
            }
            else
            {
                obj.GenralWardIDPCharges = fc["GeneralCharges"].ToString();
            }

           
            //if (obj.HeaderID == "")
            //{
            //    obj.HeaderID = "0";
            //}
            //else
            //{
            //    obj.HeaderID = fc["HeaderParameterID"].ToString();
            //}
            //if (obj.ParameterID == "" )
            //{
            //    obj.ParameterID = "0";
            //}
            //else
            //{

            //    obj.ParameterID = fc["HeaderParameterID"].ToString();
            //}
            //if (obj.ParameterName == " ")
            //{
            //    obj.ParameterName = "";
            //}
            //else
            //{
            //    obj.ParameterName = fc["ParameterHeaderName"].ToString();
            //}
            //if (obj.Type == "" || obj.Type==null)
            //{
            //    obj.Type = "0";
            //}
            //else
            //{
            //    obj.Type = fc["Type"].ToString();
            //}
            //if (obj.Space == "" || obj.Space == null)
            //{
            //    obj.Space = "0";
            //}
            //else
            //{
            //    obj.Space = fc["Space"].ToString();
            //}
           

            if (_TestMaster.SaveTest(obj))
            {
                        if (Convert.ToInt32(obj.TestID) > 0)
                        {
                            ModelState.Clear();
                            //ViewData["flag"] = "Done";
                            TempData["msg"] = "TestMaster Updated Successfully";
                            return RedirectToAction("TestMaster", "TestMaster");
                        }
                        else
                        {
                            ModelState.Clear();
                            //ViewData["flag"] = "Done";
                            TempData["msg"] = "TestMaster Saved Successfully";
                            return RedirectToAction("TestMaster", "TestMaster");
                        }
               
            }
            }
           else
           {
               TempData["msg"] = "TestMaster Already Exist's";
           }
            return RedirectToAction("TestMaster", "TestMaster");
            }
             catch (Exception ex)
             {
                 return RedirectToAction("TestMaster", "TestMaster");
             }
                       
        }

        [HttpGet]
        public JsonResult GetTestMasterForBindCategory(string prefix)
        {
            BL_TestMaster _TestMaster = new BL_TestMaster();
           

            return new JsonResult { Data = _TestMaster.GetTestMasterForBindCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        
        [HttpGet]
        public JsonResult GetAllTestMaster(string prefix)
        {
            BL_TestMaster BL_obj = new BL_TestMaster();
            List<TestMaster> GetList = new List<TestMaster>();
           DataSet ds= _TestMaster.GetAllTestMaster(HospitalID, LocationID);
           foreach(DataRow dr in ds.Tables[0].Rows)
           {
            GetList.Add(new TestMaster {
          TestID=dr["TestID"].ToString(),
          TestName=dr["TestName"].ToString(),
          PrintAs=dr["PrintAs"].ToString(),
          GeneralCharges=dr["GeneralCharges"].ToString(),
          EmergencyCharges=dr["EmergencyCharges"].ToString(),
          MyCost=dr["MyCost"].ToString(),
          Category = dr["Category"].ToString(),
          TestType=dr["TestType"].ToString(),
          Commission=dr["Commission"].ToString(),
          CommissionRs=dr["CommissionRs"].ToString(),
          ForGender=dr["ForGender"].ToString(),
          Format=dr["Format"].ToString(),
          ProcessDay=dr["ProcessDay"].ToString(),
          PrintOrder=dr["PrintOrder"].ToString(),
          Methord=dr["Methord"].ToString(),
          OutsideLab=dr["OutSideLab"].ToString(),
          DisposalDay=dr["DisposalDay"].ToString(),
          Discount=dr["Discount"].ToString(),
          SampleType=dr["SampleType"].ToString(),
          PrintName=dr["PrintName"].ToString(),
          PrintTogether=dr["PrintTogether"].ToString(),
          WardView=dr["WardView"].ToString(),
          Interpretation=dr["Interpretation"].ToString(),
          HMSCode=dr["HMSCode"].ToString(),
          PatientInstr=dr["PatientInstr"].ToString(),
          Footer=dr["Footer"].ToString(),
          TechnicianIntr=dr["TechnicianIntr"].ToString(),



            
            });
           }
           return new JsonResult { Data = GetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetTestMasterForBindSampleType(string prefix)
        {
            return new JsonResult { Data = _TestMaster.GetTestMasterForBindSampleType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllWard(string prefix)
        {
            List<TestMaster> ser = new List<TestMaster>();
            BL_TestMaster BL_obj = new BL_TestMaster();
            TestMaster obj = new TestMaster();

            obj.dsAllTestMaster = BL_obj.GetAllWard();
           
                foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
                {
                    TestMaster obj1 = new TestMaster();
                  //  obj1.TestMasterDetailIPDChargeID = dr["TestMasterDetailIPDChargeID"].ToString();
                    obj1.WardID = dr["WardID"].ToString();
                    obj1.WardName = dr["WardName"].ToString();
                    obj1.GeneralCharges = dr["GeneralCharges"].ToString();
                    obj1.EmergencyCharges = dr["EmergencyCharges"].ToString();
                    ser.Add(obj1);
                }
           
           

            return Json(ser, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ColorHeader(string prefix)
        {
            List<TestMaster> ser = new List<TestMaster>();
            BL_TestMaster BL_obj = new BL_TestMaster();
            TestMaster obj = new TestMaster();

            obj.dsAllTestMaster = BL_obj.GetAllWard();

            foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
            {
                TestMaster obj1 = new TestMaster();
                obj1.WardID = dr["WardID"].ToString();
                obj1.WardName = dr["WardName"].ToString();
                obj1.GeneralCharges = dr["GeneralCharges"].ToString();
                obj1.EmergencyCharges = dr["EmergencyCharges"].ToString();
                ser.Add(obj1);
            }



            return Json(ser, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllHeader(string prefix)
        {
            List<TestMaster> ser = new List<TestMaster>();
            BL_TestMaster BL_obj = new BL_TestMaster();
            TestMaster obj = new TestMaster();

            obj.dsAllTestMaster = BL_obj.GetAllHeader();

            foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
            {
                TestMaster obj1 = new TestMaster();
                obj1.HeaderID = dr["HeaderID"].ToString();
                obj1.HeaderName = dr["HeaderName"].ToString().Replace(",",";");
               
                ser.Add(obj1);
            }



            return Json(ser, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllParameter(string prefix)
        {
            List<TestMaster> ser = new List<TestMaster>();
            BL_TestMaster BL_obj = new BL_TestMaster();
            TestMaster obj = new TestMaster();

            obj.dsAllTestMaster = BL_obj.GetAllParameter();

            foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
            {
                TestMaster obj1 = new TestMaster();
                obj1.ParameterID = dr["ParameterID"].ToString();
                obj1.ParameterName = dr["ParameterName"].ToString().Replace(",", ";");

                ser.Add(obj1);
            }



            return Json(ser, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(TestMaster obj)
        {
            
            KeystoneProject.Buisness_Logic.MasterLaboratory.BL_TestMaster TestMasterobj =new KeystoneProject.Buisness_Logic.MasterLaboratory.BL_TestMaster();

            string val = "";
            if (TestMasterobj.CheckHeader(obj.HeaderName, Convert.ToInt32(obj.HeaderID)))
            {
                if (obj.HeaderName != "")
                {
                    if (TestMasterobj.Save(obj))
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        val = "Header Saved Successfully !";                        
                    }
                    
                }
            }
            else
            {
                try
                {
                    val = "Header Already Exists !";
                    ViewData["flag"] = "Error";                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json(val);
        }

       [HttpPost]
        public JsonResult Edit(int TestID)
        {
            BL_TestMaster _TestMaster = new BL_TestMaster();
            TestMaster obj = new TestMaster();
            List<TestMaster> GetList = new List<TestMaster>();
            DataSet ds = _TestMaster.GetTestMaster(TestID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               GetList.Add(new TestMaster
                
                {
                    HospitalID=Convert.ToInt32(dr["HospitalID"]),

                LocationID=Convert.ToInt32(dr["LocationID"]),
                    TestID = dr["TestID"].ToString(),
                    TestName = dr["TestName"].ToString(),
                    HSNCode = dr["HSNCode"].ToString(),
                    PrintAs = dr["PrintAs"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                    Client=dr["Client"].ToString(),
                    Alias=dr["Alias"].ToString(),
                    MyCost = dr["MyCost"].ToString(),
                    CategoryID = dr["Category"].ToString(),
                    Category=dr["CategoryName"].ToString(),
                    TestType = dr["TestType"].ToString(),
                    ReffCommission=dr["ReffCommission"].ToString(),
                    ReffCommissionRs=dr["ReffCommissionRs"].ToString(),
                    Commission = dr["Commission"].ToString(),
                    CommissionRs = dr["CommissionRs"].ToString(),
                    ForGender = dr["ForGender"].ToString(),
                    Format = dr["Format"].ToString(),
                    ProcessDay = dr["ProcessDay"].ToString(),
                    PrintOrder = dr["PrintOrder"].ToString(),
                    Methord = dr["Methord"].ToString(),
                    OutsideLab = dr["OutSideLab"].ToString(),
                    DisposalDay = dr["DisposalDay"].ToString(),
                    Discount = dr["Discount"].ToString(),
                    SampleType = dr["SampleType"].ToString(),
                    PrintName = dr["PrintName"].ToString(),
                    PrintTogether = dr["PrintTogether"].ToString(),
                    WardView = dr["WardView"].ToString(),
                    Interpretation = dr["Interpretation"].ToString(),
                    HMSCode = dr["HMSCode"].ToString(),
                    PatientInstr = dr["PatientInstr"].ToString(),
                    Footer = dr["Footer"].ToString(),
                    TechnicianIntr = dr["TechnicianIntr"].ToString(),
                    MachineName = dr["MachinName"].ToString(),
                    



                });
            }
          


            return new JsonResult { Data = GetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       public JsonResult GetMasterDetails(int TestID)
       {
           BL_TestMaster _TestMaster = new BL_TestMaster();
           TestMaster obj = new TestMaster();
           obj.dsGetAllWardName = _TestMaster.GetTestMasterDetailIPDCharge(TestID);
           List<TestMaster> GetWard = new List<TestMaster>();
           foreach(DataRow dr in obj.dsGetAllWardName.Tables[0].Rows)
           {
               GetWard.Add(new TestMaster
                   {
                     WardID=dr["WardID"].ToString(),
               WardName=dr["WardName"].ToString(),
               GeneralCharges=dr["GeneralCharges"].ToString(),
               EmergencyCharges=dr["EmergencyCharges"].ToString(),
               TestMasterDetailIPDChargeID = dr["TestMasterDetailIPDChargeID"].ToString(),
               TestID=dr["TestID"].ToString(),
               Mode=dr["Mode"].ToString(),

              
                   });
           }

           List<TestMaster> GetTest = new List<TestMaster>();
           DataSet dsGetTest = _TestMaster.GetTestMasterDetail(HospitalID, TestID, LocationID);

           obj.dsParameterSet = new DataSet();
           obj.dsParameterSet.Tables.Add(new DataTable());
        
           foreach (DataRow dr in dsGetTest.Tables[0].Rows)
           {
                         GetTest.Add(new TestMaster
                         {
                              HospitalID=Convert.ToInt32(dr["HospitalID"]),
              LocationID = Convert.ToInt32(dr["LocationID"]),
               TestDetailsID=Convert.ToInt32(dr["TestDetailsID"]).ToString(),
              TestID = dr["TestID"].ToString(),
              ParameterName = dr["ParameterName"].ToString(),
              HeaderName=dr["HeaderName"].ToString(),
             HeaderParmater = dr["HeaderParmater"].ToString(),
              Type1 = dr["Type"].ToString(),
               Space1 = dr["Space"].ToString(),
               HeaderID = dr["HeaderID"].ToString(),
              PrintOrder = dr["Order"].ToString(),
             Mode = dr["Mode"].ToString(),
                         
           });
           
           }

           return Json (new { GetTest = GetTest, GetWard = GetWard},JsonRequestBehavior.AllowGet);
       }
     
        [HttpPost]
        public JsonResult BindParameterGrid(string search, string chk)
        {
            
            List<TestMaster> ser = new List<TestMaster>();
            BL_TestMaster BL_obj = new BL_TestMaster();
            TestMaster obj = new TestMaster();
            HospitalLocation();
            obj.dsAllTestMaster = BL_obj.GetParameterForTestMaster(HospitalID,LocationID,search);
            if (chk == "Parameter")
            {
                int count = 0;
                foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
                {
                    TestMaster obj1 = new TestMaster();
                    obj1.ParameterID = dr["ParameterID"].ToString();
                    obj1.ParameterName = dr["ParameterName"].ToString();

                    ser.Add(obj1);
                }
            }
            else
            {
                int count1 = 0;
                foreach (DataRow dr in obj.dsAllTestMaster.Tables[1].Rows)
                {
                    TestMaster obj12 = new TestMaster();

                    obj12.HeaderID = dr["HeaderID"].ToString();
                    obj12.HeaderName = dr["HeaderName"].ToString();
                    ser.Add(obj12);
                }

            }

            return Json(ser, JsonRequestBehavior.AllowGet);
        }

      
        #region upload files
        public JsonResult imageuplod(TestMaster model)
        {
            //var file = model.ImageFile;
            //string path1 = Server.MapPath("~/") + "LaboratoryFiles/" + file.FileName;
            //System.IO.File.Copy("" + ucTestMaster1.txtUploadFile.Text + "", path1 + "" + ucTestMaster1.txtName.Text + ".docx");



            List<TestMaster> TestMasterImage = new List<TestMaster>();

            var file = model.ImageFile;
            if (file != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                string path = Server.MapPath("~/") + "LaboratoryFiles/" + file.FileName;

                file.SaveAs(path);

               // Session["Paper"] = path;

                //TestMasterImage.Add(new TestMaster
                //{
                //    //PaperName = "/MRDFiles/" + file.FileName,
                //    //Paper = path,

                //});
            }
            return new JsonResult { Data = file, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        public ActionResult Delete(int TestID)
        {
           
            string val = "";
            TestMaster obj = new TestMaster();
            BL_TestMaster _TestMaster = new BL_TestMaster();
            if (_TestMaster.DeleteTestMaster(HospitalID, TestID, LocationID))
            {
                val = "TestMaster Deleted Successfully";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }
	}
}