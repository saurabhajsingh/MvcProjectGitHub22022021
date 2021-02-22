using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using System.Data;

namespace KeystoneProject.Controllers.Master
 {
    public class ProfileController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["hpid"]);
            LocationID = Convert.ToInt32(Session["hlid"]);
            CreationID = Convert.ToInt32(Session["usid"]);
        }
        BL_Profile Blobj = new BL_Profile();
        Profile objmodel = new Profile();
        [HttpGet]
        public ActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Profile(Profile obj,FormCollection fc)
        {

            BL_Profile _BL_Profile = new BL_Profile();
            try
            {
                if (_BL_Profile.CheckProfile(obj.ProfileID, obj.Name))
            {

            if (obj.Panel == null)
            {
                obj.Panel = "false";

            }
            else
            {
                obj.Panel = fc["Panel"].ToString();
            }


            if (obj.ProfileID == null)
            {
                obj.ProfileID = "";
            }
            else
            {
                obj.ProfileID = fc["ProfileID"].ToString();
            }

           
                obj.TestID = fc["tabletestid"].ToString();

                obj.TestName = fc["TestName"].ToString();

            if (obj.Name != null)
            {
                obj.Name = fc["Name"].ToString();
            }
            else
            {
                obj.Name = ""; 
            }

            if (obj.PrintAs != "" || obj.PrintAs != null)
            {
                obj.PrintAs = fc["PrintAs"].ToString();
            }
            else
            {
                obj.PrintAs = "";
            }

            if (obj.ForGender != "" || obj.ForGender != null)
            {
                obj.ForGender = fc["ForGender"].ToString();
            }
            else
            {
                obj.ForGender = "";
            }

            if (obj.Client != "" || obj.Client != null)
            {
                obj.Client = fc["Client"].ToString();
            }
            else
            {
                obj.Client = "";
            }

            if (obj.HMSCode != "" || obj.HMSCode != null)
            {
                obj.HMSCode = fc["HMSCode"].ToString();
            }
            else
            {
                obj.HMSCode = "";
            }

            if (obj.Discount != "" || obj.Discount != null)
            {
                obj.Discount = fc["Discount"].ToString();
            }
            else
            {
                obj.Discount = "";
            }

            //obj.Discount = "0";


            if (obj.GeneralCharges != "" || obj.GeneralCharges != null)
            {
                obj.GeneralCharges = fc["GeneralCharges"].ToString();
            }
            else
            {
                obj.GeneralCharges = "0.00";
            }

            if (obj.EmergencyCharges != "" || obj.EmergencyCharges != null)
            {
                obj.EmergencyCharges = fc["EmergencyCharges"].ToString();
            }
            else
            {
                obj.EmergencyCharges = "0.00";
            }

            if (obj.Commission != "" || obj.Commission != null)
            {
                obj.Commission = fc["Commission"].ToString();
            }
            else
            {
                obj.Commission = "0.00";
            }


            if (obj.CommissionRs != "" || obj.CommissionRs != null)
            {
                obj.CommissionRs = fc["CommissionRs"].ToString();
            }
            else
            {
                obj.CommissionRs = "";
            }
                if (Blobj.Save(obj))
                {
                        if (Convert.ToInt32(obj.ProfileID) > 0)
                        {
                            ModelState.Clear();
                            ViewData["flag"] = "Done";
                            TempData["Msg"] = "Profile Updated Successfully ";
                            return RedirectToAction("Profile", "Profile");
                        }
                        else
                        {
                            ModelState.Clear();
                            ViewData["flag"] = "Done";
                            TempData["Msg"] = "Profile Saved Successfully ";
                            return RedirectToAction("Profile", "Profile");
                        }
                   
                }
            }
           else
           {
               TempData["msg"] = "Profile Already Exist's";
           }
                return RedirectToAction("Profile", "Profile");
        
        }
           
            catch (Exception ex)
            {
                return RedirectToAction("Profile", "Profile");
            }
        }
        public JsonResult GetAllTestMaster()
        {
            BL_Profile Blobj = new BL_Profile();
            Profile AddServiceMod = new Profile();

            AddServiceMod.dsgrid = Blobj.GetAllTestMaster();


            List<Profile> search = new List<Profile>();
            foreach (DataRow dr in AddServiceMod.dsgrid.Tables[0].Rows)
            {
             
                search.Add(new Profile
                {
                    TestID = dr["TestID"].ToString(),
                    TestName = dr["TestName"].ToString(),
              });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllProfile()
        {
            BL_Profile Blobj = new BL_Profile();
            Profile AddServiceMod = new Profile();

            AddServiceMod.dsgrid = Blobj.GetAllProfile();


            List<Profile> search = new List<Profile>();
            foreach (DataRow dr in AddServiceMod.dsgrid.Tables[0].Rows)
            {

                search.Add(new Profile
                {
                    ProfileID = dr["ProfileID"].ToString(),
                    Name = dr["Name"].ToString(),
                    PrintAs = dr["PrintAs"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        
       
      

         public JsonResult Fill(int ProfileID)
        {
            DataSet ds = new DataSet();
            List<Profile> Search = new List<Models.Master.Profile>();
            ds = Blobj.GetProfile(ProfileID);
            Profile AddProfile = new Profile();
            DataSet ds1 = new DataSet();
            ds1 = Blobj.GetProfileTest(ProfileID);

           AddProfile.ProfileID = ds.Tables[0].Rows[0]["ProfileID"].ToString();
            AddProfile.Name = ds.Tables[0].Rows[0]["Name"].ToString();
            AddProfile.PrintAs = ds.Tables[0].Rows[0]["PrintAs"].ToString();
            AddProfile.Client = ds.Tables[0].Rows[0]["Client"].ToString();
            AddProfile.GeneralCharges = ds.Tables[0].Rows[0]["GeneralCharges"].ToString();
            AddProfile.EmergencyCharges = ds.Tables[0].Rows[0]["EmergencyCharges"].ToString();
            AddProfile.Commission = ds.Tables[0].Rows[0]["Commission"].ToString();
            AddProfile.CommissionRs = ds.Tables[0].Rows[0]["CommissionRs"].ToString();
            AddProfile.HMSCode = ds.Tables[0].Rows[0]["HMSCode"].ToString();
            AddProfile.MyCost = ds.Tables[0].Rows[0]["MyCost"].ToString();
            AddProfile.Discount = ds.Tables[0].Rows[0]["Discount"].ToString();
            AddProfile.ForGender = ds.Tables[0].Rows[0]["ForGender"].ToString();
            AddProfile.Panel = ds.Tables[0].Rows[0]["Panel"].ToString();

            Search.Add(AddProfile);
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                Search.Add(new Profile
                {
                    TestID = dr["TestID"].ToString(),
                    TestName = dr["TestName"].ToString(),

                });
            }

           return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
         #region Delete Profile
         public ActionResult DeleteProfile(int ProfileID)
         {

             string del = "";
             ProfileID = Convert.ToInt32(Request.Form["ProfileID"]);
             BL_Profile Bl_obj = new BL_Profile();

             if (Bl_obj.DeleteProfile(ProfileID))
             {
                 del = "Profile Deleted Successfully  ";
             }

             return Json(del, JsonRequestBehavior.AllowGet);
         }
        #endregion
	}
}