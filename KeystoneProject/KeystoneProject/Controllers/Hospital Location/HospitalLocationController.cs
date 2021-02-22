using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Controllers.Hospital_Location;
using System.Data;
using KeystoneProject.Models.Keystone;
using System.IO;


namespace KeystoneProject.Controllers
{
    public class HospitalLocationController : Controller
    {
        BL_HospitalLocation objbl = new BL_HospitalLocation();
        KeystoneProject.Models.Keystone.HospitalLocation objmodel = new Models.Keystone.HospitalLocation();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }


        [HttpGet]
        public ActionResult HospitalLocation()
        {
            //KeystoneProject.Models.Keystone.HospitalLocation obj_Module = new KeystoneProject.Models.Keystone.HospitalLocation();
            //obj_Module.StoreAllDoctor = objbl.SelectAllData();
            //return View(obj_Module);
            return View();

        }

        [HttpPost]

        public ActionResult HospitalLocation(HospitalLocation obj)
        {
            obj.LocationName = Request.Form["LocationName"];
            obj.HospitalName = Request.Form["HospitalName"];
            obj.GroupName = Request.Form["GroupName"];
            obj.Logo = Request.Form["Logo"];
            obj.ManagingBody = Request.Form["ManagingBody"];
            obj.Adminstrator = Request.Form["Adminstrator"];
            obj.Address = Request.Form["Address"];
            obj.CityID = Request.Form["CityID"];
            obj.Pincode = Request.Form["Pincode"];
            obj.StateID = Request.Form["StateID"];
            obj.CountryID = Request.Form["CountryID"];
            obj.PhoneNumber = Request.Form["PhoneNumber"];
            obj.PhoneNumber1 = Request.Form["PhoneNumber1"];
            obj.Fax = Request.Form["Fax"];
            obj.MobileNo = Request.Form["MobileNo"];
            obj.EmailID = Request.Form["EmailID"];
            obj.EmailPassword = Request.Form["EmailPassword"];
            obj.URL = Request.Form["URL"];
           

            obj.RegistrationNo = Request.Form["RegistrationNo"];
            obj.ServiceTaxNo = Request.Form["ServiceTaxNo"];
            obj.PANNo = Request.Form["PANNo"];
            obj.TANo = Request.Form["TANo"];
            obj.TDSCircle = Request.Form["TDSCircle"];
            obj.RegistrationCharge = Request.Form["RegistrationCharge"];
            obj.RegistrationRenwalCharges = Request.Form["RegistrationRenwalCharges"];
            obj.TDSProfessional = Request.Form["TDSProfessional"];
            obj.TDSContrator = Request.Form["TDSContrator"];

   

           
            
            try
            {
                BL_HospitalLocation Bl_Dept = new BL_HospitalLocation();
                if (Bl_Dept.Save(obj))
                {
                    ModelState.Clear();
                    TempData["Msg"] = " HospitalLocation Save successfully";
                }

                return RedirectToAction("HospitalLocation", "HospitalLocation");
            }
            catch (Exception)
            {

                return View();
            }


        }

        public JsonResult GetCityBind(string prefix)
        {

            DataSet ds = objbl.GetCity(prefix, "%");
            List<KeystoneProject.Models.Keystone.HospitalLocation> searchList = new List<KeystoneProject.Models.Keystone.HospitalLocation>();
            City(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new KeystoneProject.Models.Keystone.HospitalLocation
                {
                    CityID = dr["CityID"].ToString(),
                    City = dr["CityName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void City(string prefix)
        {
            DataSet ds = objbl.GetCity(prefix, "%");
            List<HospitalLocation> searchList = new List<HospitalLocation>();

            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string City)
        {
            KeystoneProject.Buisness_Logic.Hospital.BL_HospitalLocation BL_obj = new BL_HospitalLocation();
            KeystoneProject.Models.Keystone.HospitalLocation obj = new KeystoneProject.Models.Keystone.HospitalLocation();
            List<string> searchList = new List<string>();

            DataTable td = new DataTable();
            DataSet ds = objbl.GetCity(City, "%");
            td = objbl.GetCountryStateID(Convert.ToInt16(ds.Tables[0].Rows[0]["CityID"].ToString()));

            obj.State = td.Rows[0]["StateName"].ToString();
            obj.Country = td.Rows[0]["CountryName"].ToString();
            obj.StateID = td.Rows[0]["StateID"].ToString();
            obj.CountryID = td.Rows[0]["CountryID"].ToString();


            searchList.Add(obj.State);
            searchList.Add(obj.Country);
            searchList.Add(obj.StateID);
            searchList.Add(obj.CountryID);

            return Json(searchList);
        }

        public JsonResult ShowAllHospitalLocation()
        {
            BL_HospitalLocation db = new BL_HospitalLocation();

            return new JsonResult { Data = db.SelectAllHospitalLocation(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Logo(HospitalLocation model)
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
               // ViewData["ImagePath"] = "/MRDFiles/" + file.FileName;

                ViewData["Logo"] = "MRDFiles/" + file.FileName;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ReportSignature(HospitalLocation model)
        {
            string path1 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file1 = model.ImageFile1;
            if (file1 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file1.FileName);
                var extention = Path.GetExtension(file1.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file1.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path1 = Server.MapPath("~/") + "MRDFiles/" + file1.FileName;
                 
                file1.SaveAs(path1);
                
                // Session["Paper"] = path;
              path1 = "/MRDFiles/" + file1.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file1.FileName;
                TempData["ReportSignature"] = "/MRDFiles/" + file1.FileName; 
            }
            return new JsonResult { Data = path1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ReportSignature1(HospitalLocation model)
        {
            string path2 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file2 = model.ImageFile2;
            if (file2 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file2.FileName);
                var extention = Path.GetExtension(file2.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file2.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path2 = Server.MapPath("~/") + "MRDFiles/" + file2.FileName;

                file2.SaveAs(path2);
                
                // Session["Paper"] = path;
                path2 = "/MRDFiles/" + file2.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file2.FileName;
                TempData["ReportSignature1"] = "/MRDFiles/" + file2.FileName;
            }
            return new JsonResult { Data = path2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult OtherSignature(HospitalLocation model)
        {
            string path3 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file3 = model.ImageFile3;
            if (file3 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file3.FileName);
                var extention = Path.GetExtension(file3.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file3.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path3 = Server.MapPath("~/") + "MRDFiles/" + file3.FileName;

                file3.SaveAs(path3);
               
                // Session["Paper"] = path;
                path3 = "/MRDFiles/" + file3.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file3.FileName;
                TempData["OtherSignature"] = "/MRDFiles/" + file3.FileName;
            }
            return new JsonResult { Data = path3, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BillHeader(HospitalLocation model)
        {
            string path4 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file4 = model.ImageFile4;
            if (file4 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file4.FileName);
                var extention = Path.GetExtension(file4.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file4.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path4 = Server.MapPath("~/") + "MRDFiles/" + file4.FileName;

                file4.SaveAs(path4);
               
                // Session["Paper"] = path;
                path4 = "/MRDFiles/" + file4.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file4.FileName;
                TempData["BillHeader"] = "/MRDFiles/" + file4.FileName;
            }
            return new JsonResult { Data = path4, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult LabReportHeader(HospitalLocation model)
        {
            string path5 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file5 = model.ImageFile5;
            if (file5 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file5.FileName);
                var extention = Path.GetExtension(file5.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file5.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path5 = Server.MapPath("~/") + "MRDFiles/" + file5.FileName;

                file5.SaveAs(path5);
               
                // Session["Paper"] = path;
                path5 = "/MRDFiles/" + file5.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file5.FileName;
                TempData["LabReportHeader"] = "/MRDFiles/" + file5.FileName; 
            }
            return new JsonResult { Data = path5, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ReportHeader(HospitalLocation model)
        {
            string path6 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file6 = model.ImageFile6;
            if (file6 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file6.FileName);
                var extention = Path.GetExtension(file6.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file6.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path6 = Server.MapPath("~/") + "MRDFiles/" + file6.FileName;

                file6.SaveAs(path6);
              
                // Session["Paper"] = path;
                path6 = "/MRDFiles/" + file6.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file6.FileName;
                TempData["ReportHeader"] = "/MRDFiles/" + file6.FileName;
            }
            return new JsonResult { Data = path6, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult AdmissionCardFront(HospitalLocation model)
        {
            string path7 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file7 = model.ImageFile7;
            if (file7 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file7.FileName);
                var extention = Path.GetExtension(file7.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file7.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path7 = Server.MapPath("~/") + "MRDFiles/" + file7.FileName;

                file7.SaveAs(path7);
               
                // Session["Paper"] = path;
                path7 = "/MRDFiles/" + file7.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file7.FileName;
                TempData["AdmissionCardFront"] = "/MRDFiles/" + file7.FileName;
            }
            return new JsonResult { Data = path7, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult AdmissionCardBack(HospitalLocation model)
        {
            string path8 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file8 = model.ImageFile8;
            if (file8 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file8.FileName);
                var extention = Path.GetExtension(file8.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file8.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path8 = Server.MapPath("~/") + "MRDFiles/" + file8.FileName;

                file8.SaveAs(path8);
               
                // Session["Paper"] = path;
                path8 = "/MRDFiles/" + file8.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file8.FileName;
                TempData["AdmissionCardBack"] = "/MRDFiles/" + file8.FileName;
            }
            return new JsonResult { Data = path8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult PrivilegeCardFront(HospitalLocation model)
        {
            string path9 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file9 = model.ImageFile9;
            if (file9 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file9.FileName);
                var extention = Path.GetExtension(file9.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file9.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path9 = Server.MapPath("~/") + "MRDFiles/" + file9.FileName;

                file9.SaveAs(path9);
                
                // Session["Paper"] = path;
                path9 = "/MRDFiles/" + file9.FileName;
                //ViewData["ImagePath"] = "/MRDFiles/" + file9.FileName;
                TempData["PrivilegeCardFront"] = "/MRDFiles/" + file9.FileName;
            }
            return new JsonResult { Data = path9, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult PrivilegeCardBack(HospitalLocation model)
        {
            string path10 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file10 = model.ImageFile10;
            if (file10 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file10.FileName);
                var extention = Path.GetExtension(file10.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file10.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path10 = Server.MapPath("~/") + "MRDFiles/" + file10.FileName;

                file10.SaveAs(path10);
                
                // Session["Paper"] = path;
                path10 = "/MRDFiles/" + file10.FileName;
                ////ViewData["ImagePath"] = "/MRDFiles/" + file10.FileName;
                TempData["PrivilegeCardBack"] = "/MRDFiles/" + file10.FileName; 
            }
            return new JsonResult { Data = path10, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Pharmacy(HospitalLocation model)
        {
            string path11 = "";
              var file11 = model.ImageFile11;
            if (file11 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file11.FileName);
                var extention = Path.GetExtension(file11.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file11.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path11 = Server.MapPath("~/") + "MRDFiles/" + file11.FileName;

                file11.SaveAs(path11);

                // Session["Paper"] = path;
                path11 = "/MRDFiles/" + file11.FileName;
                ////ViewData["ImagePath"] = "/MRDFiles/" + file10.FileName;
                TempData["Pharmacy"] = "/MRDFiles/" + file11.FileName;
            }
            return new JsonResult { Data = path11, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteHospitalLocation(int LocationID)
        {
            string _Del = null;
            try
            {
                int DependaincyName = objbl.Delete(LocationID);
                if (DependaincyName == 1)
                {
                    _Del = "HospitalLocation Deleted Successfully";
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