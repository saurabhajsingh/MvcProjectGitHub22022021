using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class PatientCWProfileController : Controller
    {

        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        List<PatientCWProfile> PatientCWProfileList = new List<PatientCWProfile>();
        BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
        PatientCWProfile PatientCWProfileobj = new PatientCWProfile();
       
        public ActionResult PatientCWProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientCWProfile(PatientCWProfile obj,FormCollection fc)
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            Connect();
            try
            {

            obj.OrganizationID = Convert.ToInt32(fc["OrgID"].ToString());
            obj.OrganizationName = fc["OrgName"].ToString();
            obj.ProfileID = fc["ProfileID"].ToString();
            obj.ProfileName = fc["ProfileName"].ToString();
           if( obj.PatientCWProfileID=="0" || obj.PatientCWProfileID==null)
          {
                    obj.PatientCWProfileID = "0";
          }
                else
                {
                    obj.PatientCWProfileID = fc["PatientCWProfileID"].ToString();
                }
            
            obj.GeneralCharges = fc["GeneralChg"].ToString();
            obj.EmergencyCharges = fc["EmergChg"].ToString();

          
          if (_PatientCWProfile.Save(obj))
                {
                    if (Convert.ToInt32(obj.PatientCWProfileID) > 0)
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        TempData["Msg"] = "Record Updated Successfully ";
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        TempData["Msg"] = "Record Saved Successfully ";
                    }
                  
                }
                // }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Doesn't Save Successfully ";
                }
          return RedirectToAction("PatientCWProfile", "PatientCWProfile");
            }
            catch (Exception)
            {
                return RedirectToAction("PatientCWProfile", "PatientCWProfile");
            }

           
        }

        

        public ActionResult BindOrganization(string prefix)
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            List<PatientCWProfile> PatientCWProfileList = new List<PatientCWProfile>();
            DataSet ds = _PatientCWProfile.BindOrganization(prefix);
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientCWProfile catl = new PatientCWProfile();

                catl.OrganizationID = Convert.ToInt32(dr["OrganizationID"].ToString());

                catl.OrganizationName = dr["OrganizationName"].ToString();

                PatientCWProfileList.Add(catl);
            }
            return new JsonResult { Data = PatientCWProfileList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindOrg(string id)
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            List<PatientCWProfile> PatientCWProfileList = new List<PatientCWProfile>();
            DataSet ds = _PatientCWProfile.BindOrg(id);
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientCWProfile catl = new PatientCWProfile();

                catl.OrganizationID = Convert.ToInt32(dr["OrganizationID"].ToString());

                catl.OrganizationName = dr["OrganizationName"].ToString();

                PatientCWProfileList.Add(catl);
            }
            return new JsonResult { Data = PatientCWProfileList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindProfile(string prefix, string OrgID)
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            List<PatientCWProfile> PatientCWProfileList = new List<PatientCWProfile>();
            DataSet ds = _PatientCWProfile.BindProfile(prefix, OrgID);
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientCWProfile catl = new PatientCWProfile();

                catl.ProfileID = dr["ProfileID"].ToString();

                catl.ProfileName = dr["Name"].ToString();
                catl.PatientCWProfileID = dr["PatientCWProfileID"].ToString();
                PatientCWProfileList.Add(catl);
            }
            return new JsonResult { Data = PatientCWProfileList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllPatientCWProfile()
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            PatientCWProfile AddServiceMod = new PatientCWProfile();

            DataSet ds = _PatientCWProfile.GetAllPatientCWProfileRate();


            List<PatientCWProfile> search = new List<PatientCWProfile>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                search.Add(new PatientCWProfile
                {
                    ProfileID =dr["ProfileID"].ToString(),
                    ProfileName = dr["Name"].ToString(),
                    OrganizationID=Convert.ToInt32(dr["OrganizationID"].ToString()),
                    OrganizationName=dr["OrganizationName"].ToString(),
                    PatientCWProfileID = dr["PatientCWProfileID"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientCWProfileRate(int id)
        {
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();
            PatientCWProfile AddServiceMod = new PatientCWProfile();

            DataSet ds = _PatientCWProfile.GetPatientCWProfileRate(id);


            List<PatientCWProfile> search = new List<PatientCWProfile>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                search.Add(new PatientCWProfile
                {

                    ProfileID = dr["ProfileID"].ToString(),
                    ProfileName = dr["Name"].ToString(),
                    OrganizationID = Convert.ToInt32(dr["OrganizationID"].ToString()),
                  //  OrganizationName = dr["OrganizationName"].ToString(),
                    PatientCWProfileID = dr["PatientCWProfileID"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
       
        public ActionResult Delete(string id)
        {
          

            string del = "";
            //id = Request.Form["PatientCWProfileID"];
            BL_PatientCWProfile _PatientCWProfile = new BL_PatientCWProfile();

            if (_PatientCWProfile.DeletePatientCWProfileRate(id))
            {
                del = "Delete";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        }
	}
}