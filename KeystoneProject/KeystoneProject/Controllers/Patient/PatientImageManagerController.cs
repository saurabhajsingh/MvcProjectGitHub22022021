using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using KeystoneProject.Models.Master;
using System.Web.Services.Description;

using WIA;
using System.Net.Mail;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientImageManagerController : Controller
    {

        int HospitalID;
        int LocationID;
        int CreationID;

        BL_PatientImageManager BL_obj = new BL_PatientImageManager();
        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
        //
        // GET: /PatientImageManager/
        public ActionResult PatientImageManager()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientImageManager(PatientImageManager obj)
        {
            BL_PatientImageManager BL_obj = new BL_PatientImageManager();

            HospitalLocationID();
            obj.PatientName = Request.Form["PatientName"];
            obj.Gender = Request.Form["Gender"];
            obj.PatientRegNo = Convert.ToInt32(Request.Form["PatientRegNo"]);
            obj.OPDIPDID = Convert.ToInt32(Request.Form["OPDIPDID"]);
            obj.PatientType = Request.Form["PatientType"];
            obj.PaperName = Request.Form["file"];
            obj.PatientName = Request.Form["PatientName"];
            obj.Gender = Request.Form["Gender"];
            obj.ConsultantDr = Request.Form["DoctorName"];
            obj.ReferredByDoctor = Request.Form["DoctorName"];
            obj.CasePaperID = Request.Form["CasePaperID"].ToString();

            //obj.PaperPath = Request.Form["Paper"].ToString();
            obj.Paper = Request.Form["path"].ToString();
            if (Request.Form["PaperPath"] != null && Request.Form["PaperPath"].ToString() != "")
            {
                obj.PaperPath = Request.Form["PaperPath"].ToString();
            }


            if (BL_obj.Save(obj))
            {
                return RedirectToAction("PatientImageManager", "PatientImageManager");
            }
            return View();
        }
        public JsonResult GetPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_PatientImageManager BL_Reg = new BL_PatientImageManager();
            string RegNo = BL_Reg.GetPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetIPDPatient(string prefix, string outside)
        {
            BL_PatientImageManager BL_Reg = new BL_PatientImageManager();
            //  BL_Patient_IPDBill _IPDBill = new BL_Patient_IPDBill();
            DataSet ds = BL_Reg.GetIPDPatient(prefix, outside);
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

        public JsonResult BindNamebyRegNo(string PatientRegNo)
            {
            HospitalLocationID();

            DataSet ds = BL_obj.GetPatientImageManager(HospitalID, LocationID, Convert.ToInt32(PatientRegNo));
            List<PatientImageManager> lists = new List<PatientImageManager>();
            PatientImageManager PMRDModel = new PatientImageManager();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                if (dr["AgeType"].ToString() == "Days")
                {
                    string Daye = DateTime.Now.AddDays(Convert.ToInt32(dr["Age"])).ToString();

                    dr["Age"] = Convert.ToDateTime(Daye).Year - DateTime.Now.Year;
                }
                //PMRDModel.CasePaperID = dr["CasePaperID"].ToString();
                PMRDModel.PatientName = dr["PatientName"].ToString();
                PMRDModel.PatientRegNo = Convert.ToInt32(dr["PatientRegNO"].ToString());
                PMRDModel.OPDIPDID = Convert.ToInt32(dr["OPDIPDID"].ToString());
                PMRDModel.PatientType = dr["PatientType"].ToString();
                PMRDModel.MobileNo = dr["MobileNo"].ToString();
                PMRDModel.Age = Convert.ToInt32(dr["Age"].ToString());
                PMRDModel.Gender = dr["Gender"].ToString();
                PMRDModel.Address = dr["Address"].ToString();
                PMRDModel.GuardianName = dr["GuardianName"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow rows in ds.Tables[1].Rows)
                {
                    PMRDModel.PaperPath = rows["PaperPath"].ToString();
                }
            }
            lists.Add(PMRDModel);
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetFile(int PatientRegNo)
        {
            SqlDataReader rdr; byte[] Paper = null;
            string PaperName = ""; string PaperPath = "";
            HospitalLocationID();

            DataSet ds = BL_obj.GetPatientImageManager(HospitalID, LocationID, Convert.ToInt32(PatientRegNo));

            List<string[]> search = new List<string[]>();
            PatientImageManager PMRDModel = new PatientImageManager();

            List<PatientImageManager> lists = new List<PatientImageManager>();
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    lists.Add(new Models.Patient.PatientImageManager
                    {
                  CasePaperID= dr["CasePaperID"].ToString(),
                  PatientRegNo = Convert.ToInt32(dr["PatientRegNO"]), 
                   OPDIPDID=   Convert.ToInt32( dr["OPDIPDID"]),
                   PatientType=  dr["PatientType"].ToString(), 
                   PaperName=  dr["PaperName"].ToString() ,
                   PaperPath = dr["PaperPath"].ToString(),

                    });

                }
            }
            return Json(lists);
        }
        public JsonResult imageuplod(PatientImageManager model)
        {

            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;

                file.SaveAs(path);

                // Session["Paper"] = path;
                path = "/MRDFiles/" + file.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file1.FileName;
                Session["Paper"] = "/MRDFiles/" + file.FileName;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public FileResult DownLoadFile(int CasePaperID)
        {
            byte[] bytes;
            string fileName, contentType, PaperPath;

            List<string[]> lists = new List<string[]>();
            PatientImageManager PMRDModel = new PatientImageManager();

            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            SqlConnection con = new SqlConnection(Constring);

            HospitalLocationID();
            SqlCommand cmd = new SqlCommand("GetPatientImageManagerParticular", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CasePaperID", CasePaperID);



            con.Open();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                sdr.Read();
                // bytes = (byte[])sdr["PaperName"];
                PaperPath = sdr["PaperPath"].ToString();
                fileName = sdr["PaperName"].ToString();
                contentType = MimeMapping.GetMimeMapping(PaperPath);
            }
            con.Close();

            return File(PaperPath, contentType, fileName);

        }
        public ActionResult Scan(string PatientRegNo, string Email)
        {
            string path = "";
            CommonDialogClass commonDialogClass = new CommonDialogClass();
            //CommonDialogClass commonDialogClass = new CommonDialogClass();
            try
            {
                string date = AutoGanrateferecnCode();
                Device scannerDevice = commonDialogClass.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, false, false);
                if (scannerDevice != null)
                {
                    Item scannnerItem = scannerDevice.Items[1];
                    AdjustScannerSettings(scannnerItem, 200, 0, 0, 1700, 2300, 0, 0);
                    object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatPNG, false);

                    if (scanResult != null)
                    {
                        ImageFile image = (ImageFile)scanResult;

                      string  fileName = Server.MapPath("~/") + "MRDFiles/" + date + ".JPEG";
                        SaveImageToPNGFile(image, fileName);

                        path= "/MRDFiles/"  + date + ".JPEG";
                    }

                    }
                }
            catch (Exception ex)
            {

                throw;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public String AutoGanrateferecnCode()//string Firstword)
        {
            string Reference = "";
            Reference += "I";//Firstword;
            Reference += (DateTime.Now.Year.ToString()).Substring(2, 2);
            Reference += DateTime.Now.Month.ToString();
            Reference += DateTime.Now.Day.ToString();
            Reference += DateTime.Now.Hour.ToString();
            Reference += DateTime.Now.Minute.ToString();
            Reference += DateTime.Now.Second.ToString();
            Reference += DateTime.Now.Millisecond.ToString();
            return Reference;
        }

        private static void SaveImageToPNGFile(ImageFile image, string fileName)
        {
            ImageProcess imgProcess = new ImageProcess();
            object convertFilter = "Convert";
            string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
            imgProcess.Filters.Add(convertFilterID, 0);
            SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", WIA.FormatID.wiaFormatPNG);
            image = imgProcess.Apply(image);
            image.SaveFile(fileName);
        }
        private static void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
          int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents)
        {
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
        }
        private static void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }
        public JsonResult Delete(int CasePaperID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = BL_obj.DeletePatientImageManager(Convert.ToInt32(CasePaperID));

                _Del = "PatientImageManager Deleted Successfully";


            }
            catch (Exception ex)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Email(string PatientRegNo, string Path,string Email)

        {
            var tosend = "saurabhajsingh24@gmail.com";
            string path = "";
            HospitalLocationID();
            DataSet ds = BL_obj.GetHospital(HospitalID, LocationID);

            List<PatientImageManager> searchlist = new List<PatientImageManager>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientImageManager
                {
                    EmailID = dr["EmailID"].ToString(),
                    EmailPassword = dr["EmailPassword"].ToString()
                });
            }


            string email = ds.Tables[0].Rows[0]["EmailID"].ToString();
            string pass = ds.Tables[0].Rows[0]["EmailPassword"].ToString();

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(email);
            mail.To.Add("info@keystonehms.com");
            // mail.Subject = txtSubject.Text;
            mail.Attachments.Add(new Attachment(@"E:\Govind 02-09-2020\keystone project 16092020\KeystoneProject\KeystoneProject\MRDFiles\Air (1).PNG"));



            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("info@keystonehms.com", "inf@465#");
            SmtpServer.EnableSsl = true;

            SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            try
            {
                SmtpServer.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
               
                    string exp = ex.ToString();
                   // return "Mail Not Sent ... and ther error is " + exp;
               
            }
   
            bool result = false;

            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}