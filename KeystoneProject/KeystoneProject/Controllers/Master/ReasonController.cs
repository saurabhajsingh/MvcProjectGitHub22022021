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
    public class ReasonController : Controller
    {
        //
        // GET: /Reason/
        BL_Reason reason = new BL_Reason();

        public JsonResult ShowReason()
        {

            BL_Reason db = new BL_Reason();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        [HttpGet]
        public ActionResult Reason()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reason(Reason obj,FormCollection fc)
        
        {
            try
            {
                BL_Reason Bl_reason = new BL_Reason();
                if (Bl_reason.CheckReason(obj.ReasonID, obj.ReasonName))
                {
                    if (Bl_reason.Save(obj))
                    {
                        if (obj.ReasonID>0)
                        {
                            TempData["Msg"] = "Reason Update Successfully";
                            return RedirectToAction("Reason", "Reason");

                        }
                        else
                        {
                            TempData["Msg"] = "Reason Saved Successfully";
                            return RedirectToAction("Reason", "Reason");

                        }


                    }
               }
                else
                {
                    //TempData["Msg"] = "Error";
                    TempData["Msg"] = "Reason Already Exist's";
                    return RedirectToAction("Reason", "Reason");

                }

                return RedirectToAction("Reason", "Reason");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Reason", "Reason");

            }

          
           
        }

      
        [HttpGet]

        public ActionResult EditReason(int id)
        {
            BL_Reason db = new BL_Reason();
            ModelState.Clear();
            return View(db.GetReason(id));
        }

        [HttpPost]
        public JsonResult EditReason(int ReasonID, string ReasonName, Reason reason1)
        {

            string _Edit = null;
            if (reason.CheckReason(ReasonID, ReasonName))
            {
                if (reason.Save(reason1))
                {
                    _Edit = "Reason Edited Successfully";
                }
            }
            else
            {
                _Edit = "Reason Already Exist's";
            }
            return new JsonResult { Data = _Edit, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Delete(int ReasonID)
        {

            string val = "";

            BL_Reason db = new BL_Reason();
            if (db.DeleteReason(ReasonID))
            {
                val = "Reason Deleted Successfully";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }
	}
}