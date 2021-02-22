using KeystoneProject.Buisness_Logic.PharmacyMaster;
using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class MedicalScheduleController : Controller
    {
        //
        // GET: /MedicalSchedule/

        BL_MedicalSchedule schedule = new BL_MedicalSchedule();
        public ActionResult MedicalSchedule()
        {
            return View();
        }

        public ActionResult Bind_table()
        {
            BL_MedicalSchedule sch = new BL_MedicalSchedule();
            return new JsonResult { Data = sch.GetData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
        [HttpPost]
        public ActionResult MedicalSchedule(MedicalSchedule obj, FormCollection fc)
        {
            try
            {
                BL_MedicalSchedule Bl_Dept = new BL_MedicalSchedule();
                if (obj.generalLedgerPosting == "on")
                     {
                    obj.generalLedgerPosting = Convert.ToString(true);
                }

                else if (obj.generalLedgerPosting == "null")
                {
                    obj.generalLedgerPosting = Convert.ToString(false);
                }
                else
                {
                    obj.generalLedgerPosting = Convert.ToString(false);
                }

                if (obj.showDetailsInReports == "on")
                {
                    obj.showDetailsInReports = Convert.ToString(true);
                }

                else if (obj.showDetailsInReports == null)
                {
                    obj.showDetailsInReports = Convert.ToString(false);
                }
                else
                {
                    obj.showDetailsInReports = Convert.ToString(false);
                }

                if (Bl_Dept.Save(obj))
                    {
                        if (obj.ScheduleID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = " Medical Schedule Updated Successfully";
                        return RedirectToAction("MedicalSchedule", "MedicalSchedule");
                    }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = " Medical Schedule Saved Successfully";
                        return RedirectToAction("MedicalSchedule", "MedicalSchedule");
                    }

                    }

                
                return RedirectToAction("MedicalSchedule", "MedicalSchedule");
            }
            catch (Exception)
            {
                return RedirectToAction("MedicalSchedule", "MedicalSchedule");
            }
        }
        public JsonResult BindScheduleName(string prefix)
        {
            DataSet ds = schedule.BindMasterSchedule(prefix);
            List<MedicalSchedule> Serchlist = new List<MedicalSchedule>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Serchlist.Add(new MedicalSchedule
                {
                    ScheduleID = Convert.ToInt32(dr["ScheduleID"].ToString()),
                    scheduleName = dr["ScheduleName"].ToString(),
                });
            }
            return new JsonResult { Data = Serchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteSchedule(int ScheduleID)
        {
            string _Del = null;
            try
            {
                Buisness_Logic.PharmacyMaster.BL_MedicalSchedule objBLServiceGrp = new BL_MedicalSchedule();
                MedicalSchedule objSG = new Models.PharmacyMaster.MedicalSchedule();

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