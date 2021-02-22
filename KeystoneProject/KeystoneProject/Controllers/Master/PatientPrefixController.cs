using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class PatientPrefixController : Controller
    {
        //
        // GET: /PatientPrefix/

        BL_PatientPrefix obj_prefix = new BL_PatientPrefix();

        public JsonResult ShowPatientPrefix()
        {

            BL_PatientPrefix db = new BL_PatientPrefix();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

           [HttpGet]
        public ActionResult PatientPrefix()
        {
            return View();
        }

        [HttpPost]
           public ActionResult PatientPrefix(PatientPrefix obj,FormCollection fc)
           {
            try
            {
                obj.Gender = fc["Gender"];
               BL_PatientPrefix db = new BL_PatientPrefix();
               if (db.CheckPatientPrefix(obj.PrefixID, obj.PrefixName))
                {
                    if (db.Save(obj))
                {
                    if (obj.PrefixID>0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = " Prefix Updated Successfully";
                        }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = " Prefix Saved Successfully";

                    }
                    
                }
                }
                else
               {

                   TempData["msg"] = " Prefix Already Exist's";
                }
                return RedirectToAction("PatientPrefix", "PatientPrefix");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return RedirectToAction("PatientPrefix", "PatientPrefix");
            }
        }

        [HttpPost]

        public JsonResult DeletePatientPrefix(int PrefixID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Master.BL_PatientPrefix bl_obj = new Buisness_Logic.Master.BL_PatientPrefix();


                int a = bl_obj.DeletePatientPrefix(PrefixID);
                if (a == 1)
                {
                    data = "Done";
                }
            }
            catch(Exception ex)
            {
                data = ex.Message;
            }
            return Json(data , JsonRequestBehavior.AllowGet);
        }
	}
}