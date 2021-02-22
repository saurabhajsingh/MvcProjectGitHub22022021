using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using KeystoneProject.Buisness_Logic.Master;
using System.IO;
namespace KeystoneProject.Controllers.Master
{
    public class DoctorController : Controller
    {
        BL_Doctor Bl_Doctor = new BL_Doctor();
        //
        // GET: /Doctor/
        [HttpGet]
        public ActionResult Doctor()
        {
            Doctor obj_Module = new Models.Master.Doctor();
            obj_Module.StoreAllDoctor = Bl_Doctor.SelectAllData();
            return View(obj_Module);
        }

        [HttpPost]
        public ActionResult Doctor(Doctor obj_Module)
         {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    return new EmptyResult();
                }

                //obj_Module.DoctorFName = Request.Form["DoctorFirstName"].ToString();
                TempData["Msg"] = "";
                obj_Module.StoreAllDoctor = Bl_Doctor.SelectAllData();
                if (Request.Form["DoctorFName"].ToString() != "")
                {

                    if (Bl_Doctor.CheckDoctor(obj_Module.DoctorID,obj_Module.DoctorPrintName, obj_Module.DoctorType))

                    {

                        if (obj_Module.DoctorID > 0)
                        {
                            if (Bl_Doctor.Save(obj_Module))
                            {
                               // obj_Module.DoctorFName = "";
                                TempData["Msg"] = "Doctor Updated Successfully";
                                ModelState.Clear();
                                RedirectToAction("Doctor", "Doctor");
                            }
                        }
                       else
                        {
                            if (Bl_Doctor.Save(obj_Module))
                            {
                               // obj_Module.DoctorFName = "";
                                TempData["Msg"] = "Doctor Saved Successfully";
                                ModelState.Clear();
                                RedirectToAction("Doctor", "Doctor");
                            }
                        }

                        //try
                        //{
                        //    if (Bl_Doctor.Save(obj_Module))
                        //    {
                        //        if (obj_Module.DoctorID > 0)
                        //        {
                        //            obj_Module.DoctorFName = "";
                        //            TempData["Msg"] = "Doctor Save Succussfully";
                        //            ModelState.Clear();
                        //            return View(obj_Module);
                        //        }
                        //        TempData["Msg"] = "Doctor Save Succussfully";
                        //        ModelState.Clear();
                        //        RedirectToAction("Doctor", "Doctor");
                        //    }




                    }
                    else
                    {
                        TempData["Msg"] = "Doctor Already Exist's";
                        RedirectToAction("Doctor", "Doctor");
                    }
                       
                        
                    

                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
            }

            return RedirectToAction("Doctor", "Doctor");
        }
        public JsonResult GetDepartmentRecord(string prefix)
        {

            DataSet dsDepartment = new DataSet();
            dsDepartment = Bl_Doctor.GetDepartment(prefix);

            List<Doctor> serch = new List<Doctor>();

            foreach (DataRow dr in dsDepartment.Tables[0].Rows)
            {
                serch.Add(new Doctor
                {

                    DepartmentName = dr["DepartmentName"].ToString(),
                    DepartmentID = dr["DepartmentID"].ToString()

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetQualificationRecord(string prefix)
        {
            DataSet ds = Bl_Doctor.GetQualification(prefix);
            List<Doctor> searchList = new List<Doctor>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Doctor
                {
                    QualifictionName = dr["QualifictionName"].ToString(),
                    QualifictionID = dr["QualifictionID"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSpecializationRecord(string prefix)
        {
            DataSet ds = Bl_Doctor.GetSpecialization(prefix);
            List<Doctor> searchList = new List<Doctor>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Doctor
                {
                    SpecializationName = dr["SpecializationName"].ToString(),
                    SpecializationID = dr["SpecializationID"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult CheckDoctorPatientReg(string RegNo, string DoctorType)
        {
            DataSet ds = Bl_Doctor.CheckDoctorPatientReg(RegNo,DoctorType);
            List<Doctor> searchList = new List<Doctor>();
            string chk = "";
            
            if (ds.Tables[0].Rows.Count >0)
            {
                chk = "Doctor Registration Number is Already Given To Another Doctor";
            }
           
           
            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        public JsonResult DataTableBind()
        {
            Doctor obj_Module = new Models.Master.Doctor();
            obj_Module.StoreAllDoctor = Bl_Doctor.SelectAllData();
            List<Doctor> serch = new List<Doctor>();

            foreach (DataRow dr in obj_Module.StoreAllDoctor.Tables[0].Rows)
            {
                serch.Add(new Doctor
                {

                    DoctorID = Convert.ToInt32(dr["DoctorID"]),
                    DoctorFName = dr["DoctorFName"].ToString(),
                    DoctorLName = dr["DoctorLName"].ToString(),
                    DoctorType = dr["DoctorType"].ToString(),
                    DoctorImage = MimeMapping.GetMimeMapping(dr["DoctorImage"].ToString()),

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //----------------------------------Table Click Fill All Data


          Doctor AddDoctor = new Doctor();
        public JsonResult Fill(int DoctorID)
        {
            Session["DoctorID"] = DoctorID;
            DataSet ds = new DataSet();
            List<Doctor> Search = new List<Models.Master.Doctor>();
            ds = Bl_Doctor.GetDoctor(DoctorID);
          
            string DateBirth, JoinDate = "";
            AddDoctor.DoctorID = Convert.ToInt32(ds.Tables[0].Rows[0]["DoctorID"].ToString());
            AddDoctor.DoctorType = ds.Tables[0].Rows[0]["DoctorType"].ToString();
            AddDoctor.DoctorFName = ds.Tables[0].Rows[0]["DoctorFName"].ToString();
            AddDoctor.DoctorLName = ds.Tables[0].Rows[0]["DoctorLName"].ToString();
            AddDoctor.DoctorPrintName = ds.Tables[0].Rows[0]["DoctorPrintName"].ToString();
            AddDoctor.RegNo = ds.Tables[0].Rows[0]["RegNo"].ToString();
            if (ds.Tables[0].Rows[0]["DateofBirth"].ToString() == "")
            {
                AddDoctor.DateOfBirth = null;
            }
            else
            {
                AddDoctor.DateOfBirth = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateofBirth"].ToString());

                AddDoctor.DateBirth = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateofBirth"]).ToString("yyyy-MM-dd");
            }
            AddDoctor.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
            if (ds.Tables[0].Rows[0]["DepartmentID"].ToString() == "0" || ds.Tables[0].Rows[0]["DepartmentName"].ToString() == "")
            {
                AddDoctor.DepartmentName = "";
            }
            else
            {
                int DepId = Convert.ToInt32(ds.Tables[0].Rows[0]["DepartmentID"].ToString());
                AddDoctor.DepartmentID = DepId.ToString();
                //  string DependaincyDepartName = _Doctor.DepartID(DepId);
                AddDoctor.DepartmentName = ds.Tables[0].Rows[0]["DepartmentName"].ToString();
            }
            if (ds.Tables[0].Rows[0]["QualificationID"].ToString() == "0" || ds.Tables[0].Rows[0]["QualifictionName"].ToString() == "")
            //    if (ds.Tables[0].Rows[0]["QualificationID"].ToString() == "")
            {
                int QuaId = 0;
                AddDoctor.QualifictionID = QuaId.ToString();
                // string DependaincyQualName = _Doctor.QualID(QuaId);

                AddDoctor.QualifictionName = "";
            }
            else
            {
                string QuaId = ds.Tables[0].Rows[0]["QualificationID"].ToString();
                AddDoctor.QualifictionID = QuaId.ToString();
                if (QuaId != "")
                {
                    //string DependaincyQualName = _Doctor.QualID(Convert.ToInt32(QuaId));
                    AddDoctor.QualifictionName = ds.Tables[0].Rows[0]["QualifictionName"].ToString();
                }

            }
            if (ds.Tables[0].Rows[0]["SpecializationID"].ToString() == "0" || ds.Tables[0].Rows[0]["SpecializationName"].ToString() == "")
            {
                AddDoctor.SpecializationName = "";
            }
            else
            {
                int SpeID = Convert.ToInt32(ds.Tables[0].Rows[0]["SpecializationID"].ToString());
                AddDoctor.SpecializationID = SpeID.ToString();
                // string DependaincySpeciaName = _Doctor.SpeciaID(SpeID);
                AddDoctor.SpecializationName = ds.Tables[0].Rows[0]["SpecializationName"].ToString();
            }
            if (ds.Tables[0].Rows[0]["DateofJoining"].ToString() == "")
            {
                AddDoctor.DateOfJoining = null;
            }
            else
            {
                AddDoctor.DateOfJoining = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateofJoining"].ToString());
                AddDoctor.JoinDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateofJoining"].ToString()).ToString("yyyy-MM-dd");
            }
            if (ds.Tables[0].Rows[0]["UploadDocuments"].ToString() == "")
            {
                AddDoctor.UploadDocuments = null;
            }
            else
            {
                AddDoctor.UploadDocuments = ds.Tables[0].Rows[0]["UploadDocuments"].ToString();
            }
            if (ds.Tables[0].Rows[0]["UploadSignature"].ToString() == "")
            {
                AddDoctor.UploadSignature = null;
            }
            else
            {
                AddDoctor.UploadSignature = ds.Tables[0].Rows[0]["UploadSignature"].ToString();
            }
            if (ds.Tables[0].Rows[0]["LevingDate"].ToString() == "")
            {
                AddDoctor.LevingDate = null;
            }
            else
            {
                AddDoctor.LevingDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["LevingDate"]).ToString("yyyy-MM-dd");
            }

            AddDoctor.CounsultancyFees = Convert.ToDecimal(ds.Tables[0].Rows[0]["CounsultancyFees"].ToString());
            AddDoctor.CounsultancyDuration = Convert.ToInt32(ds.Tables[0].Rows[0]["CounsultancyDuration"].ToString());
            AddDoctor.RenewalFee = Convert.ToDecimal(ds.Tables[0].Rows[0]["RenewalFee"].ToString());
            if (ds.Tables[0].Rows[0]["RenewalDuration"].ToString() != "")
            {
                AddDoctor.RenewalDuration = Convert.ToDecimal(ds.Tables[0].Rows[0]["RenewalDuration"].ToString());
            }
            else
            {
                AddDoctor.RenewalDuration = 0;
            }
            if (ds.Tables[0].Rows[0]["Commission"].ToString() != "")
            {
                AddDoctor.Commission = Convert.ToDecimal(ds.Tables[0].Rows[0]["Commission"].ToString());
            }
            else
            {
                AddDoctor.Commission = 0;
            }

            AddDoctor.CommissionType = ds.Tables[0].Rows[0]["CommissionType"].ToString();

            if (ds.Tables[0].Rows[0]["ConsultancyLimit"].ToString() != "")
            {
                AddDoctor.ConsultancyLimit = Convert.ToInt32(ds.Tables[0].Rows[0]["ConsultancyLimit"].ToString());
            }
            else
            {
                AddDoctor.ConsultancyLimit = 0;
            }
            AddDoctor.DoctorImage = ds.Tables[0].Rows[0]["DoctorImage"].ToString();
            AddDoctor.PermanentAddress = ds.Tables[0].Rows[0]["PermanentAddress"].ToString();
            AddDoctor.TelephoneNo = ds.Tables[0].Rows[0]["TelephoneNo"].ToString();
            AddDoctor.FaxNo = ds.Tables[0].Rows[0]["FaxNo"].ToString();
            AddDoctor.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
            AddDoctor.EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
            AddDoctor.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
            AddDoctor.Mode = "Edit";
           AddDoctor.DoctorImage = ds.Tables[0].Rows[0]["DoctorImage"].ToString();

            Search.Add(AddDoctor);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public FileResult DownlodeFile(string UplodeDocument)
        {
           
                DataSet ds = new DataSet();
                int DoctorID = Convert.ToInt32(Session["DoctorID"]);
                ds = Bl_Doctor.GetDoctor(DoctorID);

                byte[] bytes;
                string fileName = "", contentType = "", PaperPath = "";
                Random obj = new Random();
                int a = obj.Next(1, 300);
                string File1 = AddDoctor.UploadDocuments;
                string DateCode = "Service_Excel_Data";

                if (UplodeDocument == "UplodeDocument")
                {
                    if (ds.Tables[0].Rows[0]["UploadDocuments"].ToString() != "")
                    {
                        fileName = Server.MapPath("~/")+ "MRDFiles/" + ds.Tables[0].Rows[0]["UploadDocuments"].ToString();
                    }
                }
                if (UplodeDocument == "UploadSign")
                {
                    if (ds.Tables[0].Rows[0]["UploadSignature"].ToString() != "")
                    {
                        fileName = Server.MapPath("~/")+ "MRDFiles/" + ds.Tables[0].Rows[0]["UploadSignature"].ToString();
                    }
                }
                // workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //  workbook.Close(true, Type.Missing, Type.Missing);

                string Path = fileName;
                string filename = System.IO.Path.GetFileName(fileName);

                // string fullpath = System.IO.Path.Combine(Path, fileName);
                contentType = MimeMapping.GetMimeMapping(Path);
              
                //fileName = "Service_Excel_Data" + Bl_obj.ReffrenceCode()+".xlsx";
          
            return File(Path, contentType, filename);
        }

        public JsonResult imageuplod(Doctor model)
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
                ViewData["ImagePath"] = "/MRDFiles/" + file.FileName;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult DeleteDoctor(int DoctorID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = Bl_Doctor.DeleteDoctor(Convert.ToInt32(DoctorID));
                if (DependaincyName == "Delete")
                {
                    _Del = "Doctor Deleted Successfully";
                }
                else
                {
                    _Del = "You Delete First" + DependaincyName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}