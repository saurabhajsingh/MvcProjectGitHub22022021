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
    public class RefferToDoctorController : Controller
    {
        //
        // GET: /RefferToDoctor/
        BL_RefferToDoctor obj_doctor = new BL_RefferToDoctor();

        public JsonResult ShowRefferDoctor()
        {
            BL_RefferToDoctor db = new BL_RefferToDoctor();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult RefferToDoctor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RefferToDoctor(RefferToDoctor obj)
        {
            try
            {
                if(obj.RefferDoctorID==0)
                {
                    if (obj_doctor.CheckRefferToDoctor(obj.RefferDoctorID, obj.InstituteName.ToString().Trim()) == false)
                    {
                        TempData["msg"] = "Institute Name  Already Exist's";
                    }
                    else
                    {
                        if (obj_doctor.Save(obj))
                        {

                            TempData["msg"] = " Institute Name Saved Successfully";
                        }
                    }
                    
                }
                else
                {
                    if (obj_doctor.Save(obj))
                    {

                        TempData["msg"] = " Institute Name Updated Successfully";
                    }
                }
                return RedirectToAction("RefferToDoctor", "RefferToDoctor");

            }
            catch (Exception)
            {
                return RedirectToAction("RefferToDoctor", "RefferToDoctor");
            }
        }


        public JsonResult DeleteRefferDoctor(int RefferDoctorID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Master.BL_RefferToDoctor bl_doctor = new Buisness_Logic.Master.BL_RefferToDoctor();
                int a = bl_doctor.DeleteRefferDoctor(RefferDoctorID);
                if (a == 1)
                {
                    data = "Done";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}