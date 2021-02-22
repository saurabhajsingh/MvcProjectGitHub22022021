using KeystoneProject.Buisness_Logic.MasterFinacialAccounts;
using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MasterFinacialAccounts
{
    public class ScheduleController : Controller
    {
        BL_Schedule objbl = new BL_Schedule();
        Schedule objmodel = new Schedule();

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
        public ActionResult Schedule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Schedule(Schedule objmodels)
        {
            try
            {
                if (objbl.Save(objmodels))
                {
                    if (objmodels.ScheduleID > 0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "ScheduleName Updated Successfully"; 
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "ScheduleName Saved Successfully"; 
                    }
                    
                }
                else
                {
                    TempData["msg"] = "ScheduleName Not Save";
                }
                return RedirectToAction("Schedule", "Schedule");

            }
            catch (Exception)
            {

                return RedirectToAction("Schedule", "Schedule"); ;
            }          
        }

        public JsonResult ShowAllSchedule()
        {
            BL_Schedule db = new BL_Schedule();
           
            return new JsonResult { Data = db.SelectAllSchedule(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindScheduleName(string prefix)
        {
            DataSet ds = objbl.BindMasterSchedule(prefix);
            List<Schedule> Serchlist = new List<Schedule>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Serchlist.Add(new Schedule
                {
                    ScheduleID = Convert.ToInt32(dr["ScheduleID"].ToString()),
                    ScheduleName = dr["ScheduleName"].ToString(),                   
                });
            }
            return new JsonResult { Data = Serchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteSchedule(int ScheduleID)
        {
            string _Del = null;
            try
            {
                Buisness_Logic.MasterFinacialAccounts.BL_Schedule objBLServiceGrp = new BL_Schedule();
                Schedule objSG = new Models.MasterFinacialAccounts.Schedule();

                int DependaincyName = objBLServiceGrp.DeleteSchedule(ScheduleID);

                if (DependaincyName == 1)
                {
                    _Del = "Schedule Deleted Successfully";
                }
                else
                {
                    _Del = "Can not Delete";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}