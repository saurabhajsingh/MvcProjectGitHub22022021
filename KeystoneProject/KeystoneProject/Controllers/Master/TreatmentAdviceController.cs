using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class TreatmentAdviceController : Controller
    {
        //
        // GET: /TreatmentTreatmentAdvice/
        BL_TreatmentAdvice obj = new BL_TreatmentAdvice();

        public JsonResult ShowTreatmentAdvice()
        {

            BL_TreatmentAdvice db = new BL_TreatmentAdvice();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult TreatmentAdvice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TreatmentAdvice(TreatmentAdvice objTreatmentAdvice)
        {
            try
            {
                if (obj.CheckTreatmentAdvice(objTreatmentAdvice.TreatmentAdviceID, objTreatmentAdvice.TreatmentAdviceName))
                {
                    if(objTreatmentAdvice.TreatmentAdviceID>0)
                    {
                        if (obj.Save(objTreatmentAdvice))
                        {
                            if (objTreatmentAdvice.TreatmentAdviceID > 0)
                            {
                                ModelState.Clear();
                                TempData["msg"] = "Treatment Advice Updated Successfully";
                                return RedirectToAction("TreatmentAdvice", "TreatmentAdvice");
                            }
                            else
                            {
                                ModelState.Clear();
                                TempData["msg"] = "Treatment Advice Saved Successfully";
                                return RedirectToAction("TreatmentAdvice", "TreatmentAdvice");
                            }
                           
                        }
                    }
                    else
                    {
                        if (obj.Save(objTreatmentAdvice))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Treatment Advice Saved Successfully";
                        }
                    }
                    
                }
                else
                {
                    TempData["msg"] = "TreatmentAdvice Already exist";
                }
                return RedirectToAction("TreatmentAdvice", "TreatmentAdvice");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("TreatmentAdvice", "TreatmentAdvice");
            }

        }


        [HttpGet]

        public JsonResult EditTreatmentAdvice(int id)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetTreatmentAdvice(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult DeleteTreatmentAdvice(int TreatmentAdviceID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(TreatmentAdviceID));

                _Del = "TreatmentAdvice Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
	}
}