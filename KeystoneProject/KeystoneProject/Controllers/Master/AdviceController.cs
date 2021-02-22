using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class AdviceController : Controller
    {
        //
        // GET: /Advice/


        BL_Advice obj = new BL_Advice();

        public JsonResult ShowAdvice()
        {

            BL_Advice db = new BL_Advice();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult Advice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Advice(Advice objadvice)
        {
            try
            {
                if (obj.CheckAdvice(objadvice.AdviceID, objadvice.AdviceName))
                {
                    if (obj.Save(objadvice))
                    {
                        if (objadvice.AdviceID > 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Advice Updated Successfully";
                            return RedirectToAction("Advice", "Advice");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Advice Saved Successfully";
                            return RedirectToAction("Advice", "Advice");
                        }
                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                }
                  return RedirectToAction("Advice", "Advice");
              
            }
            catch(Exception Ex)
            {
                return RedirectToAction("Advice", "Advice");
            }
          
        }


        [HttpGet]

        public JsonResult EditAdvice(int id)
        {
            
            ModelState.Clear();

            return new JsonResult { Data = obj.GetAdvice(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }


        public JsonResult DeleteAdvice(int AdviceID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(AdviceID));
               
                    _Del = "Advice Deleted Successfully";
               
             
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      

	}
}