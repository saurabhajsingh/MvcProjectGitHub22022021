using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;


namespace KeystoneProject.Controllers.Master
{
    public class CustomTranslationController : Controller
    {
        //
        // GET: /CustomTranslation/

        BL_CustomTranslation obj = new BL_CustomTranslation();
        public ActionResult CustomTranslation()
        {
            return View();
        }
        public JsonResult ShowCustomTranslation()
        {

            BL_CustomTranslation db = new BL_CustomTranslation();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [HttpGet]
        public JsonResult Edit(int TextID)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetPatientPrivilegeCard(TextID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
       

        [HttpPost]
        public ActionResult CustomTranslation(CustomTranslation objCustomTranslation)
        {
            try
            {
                if (obj.CheckCustomTranslation(objCustomTranslation))
                {
                    if (obj.Save(objCustomTranslation))
                    {
                        if (objCustomTranslation.TextID > 0 )
                        {
                            ModelState.Clear();
                            TempData["msg"] = "CustomTranslation Update Successfully";
                            return RedirectToAction("CustomTranslation", "CustomTranslation");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "CustomTranslation Saved Successfully";
                            return RedirectToAction("CustomTranslation", "CustomTranslation");
                        }
                    }
                }
                else
                {
                    TempData["msg"] = "Error";
                }
                return RedirectToAction("CustomTranslation", "CustomTranslation");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("CustomTranslation", "CustomTranslation");
            }

        }

        [HttpPost]
        public ActionResult Delete(int TextID)
        {
            string del = "";
            TextID = Convert.ToInt32(Request.Form["TextID"]);
            BL_CustomTranslation obj = new BL_CustomTranslation();
           // BL_PatientPrivilegeCard _PatientPrivilegeCard = new BL_PatientPrivilegeCard();
            if (obj.Delete(TextID))
            {
                del = "Delete";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        }




	}
}