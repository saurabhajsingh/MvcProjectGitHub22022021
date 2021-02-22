using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.Application;
using KeystoneProject.Models.Models_Application;
using System.Data;
namespace KeystoneProject.Controllers.Application
{
    public class ApplicationController : Controller
    {
        //
        // GET: /Login/
        string user;
        DataSet ds = new DataSet();
        public ActionResult Login()
        {
            ViewBag.Message = "Your Login Page";

            return View();
        }
        [HttpPost]

        public ActionResult Login(FormCollection fc)
         {
       
            try
            {
              
                KeystoneProject.Models.Models_Application.Models_Application obj = new Models.Models_Application.Models_Application();
                Buisness_Logic.Application.Application BLobj = new Buisness_Logic.Application.Application();
                if (fc["UserName"] != null)
                {
                    obj.UserName = fc["UserName"].ToString();
                    obj.Password = fc["Password"].ToString();
                    
                    bool login = BLobj.UsersLogin(obj);
                    string selectedLocation = Session["LocationID"].ToString();
                    user = Session["UserID"].ToString();
                    if (login)
                    {
                        ds = BLobj.CheckUserStatus(user);

                        if (ds.Tables[0].Rows[0]["UserStatus"].ToString() == "true")
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "User Already Login To Another System !";
                        }
                        else
                        {
                            BLobj.UserStatus(user);
                            return RedirectToAction("Dashboard", "Home");
                        }
                       
                    }
                    else
                    {
                        return View();
                    }
                }
                return View();
                //
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                //ViewBag.Message = ex.Message;
                return View();
            }
        }


        #region GetLocationChange
        public JsonResult GetLocationByUserID()
        {
            string selectedLocation = null;
            if (Session["LocationID"] != null)
                selectedLocation = Session["LocationID"].ToString();
            Buisness_Logic.Application.Application BLobj = new Buisness_Logic.Application.Application();
            string userID = Session["UserID"].ToString();
            //return new JsonResult { Data = db.GetHospitals(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = BLobj.GetLocationByUserID(userID, selectedLocation), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        #endregion



        public JsonResult ChangeLocationByLocationID(string LocationID)
        {
            Session["LocationID"] = LocationID;
            return new JsonResult { Data = LocationID, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult SessionLogOut(string prefix)

        {
            Buisness_Logic.Application.Application BLobj = new Buisness_Logic.Application.Application();
            string data = "";
            int HospitalID = Convert.ToInt32(Session["HospitalID"]);
            int LocationID = Convert.ToInt32(Session["LocationID"]);
            Session["HospitalID"] = HospitalID;
            Session["LocationID"] = LocationID;
             List<string[]> serch = new List<string[]>();
            if (Session["LocationID"] == null && Session["LocationID"] == null)
            {
                user = Session["UserID"].ToString();
                BLobj.UserStatusFalse(user);
                data = "Session Out Please Login Again..";
            }
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult UserStatusFalse()
        {
           
            Buisness_Logic.Application.Application BLobj = new Buisness_Logic.Application.Application();
            user = Session["UserID"].ToString();
            return new JsonResult { Data = BLobj.UserStatusFalse(user), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult PatientMaster()
        {
            return View();
        }

        public ActionResult Patient()
        {
            return View();
        }

        public ActionResult Laboratory()
        {
            return View();
        }

        public ActionResult Financial()
        {
            return View();
        }
        
        public ActionResult PatientReport()
        {
            return View();
        }

        public ActionResult MISReports()
        {
            return View();
        }

        public ActionResult Hospital()
        {
            return View();
        }
    }
}