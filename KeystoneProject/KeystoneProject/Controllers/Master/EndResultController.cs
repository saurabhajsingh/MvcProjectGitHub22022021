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
    public class EndResultController : Controller
    {
        //
        // GET: /EndResult/

        BL_EndResult db = new BL_EndResult();
        public JsonResult ShowEndResult()
        {

            BL_EndResult db = new BL_EndResult();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        public ActionResult EndResult()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EndResult(EndResult obj1,FormCollection collection)
        {
            BL_EndResult db = new BL_EndResult();
            try
            {

                if (db.CheckEndResult(obj1.EndResultID, obj1.EndResultName))
                {
                    if (db.Save(obj1))
                    {
                        if (obj1.EndResultID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "EndResult Update Successfully";
                            return RedirectToAction("EndResult", "EndResult");

                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "EndResult Saved Successfully";
                            return RedirectToAction("EndResult", "EndResult");
                        }
                       

                    }
                }
                else
                {
                    //TempData["Msg"] = "Error";
                    TempData["Msg"] = "EndResult Already Exist's";
                    return RedirectToAction("EndResult", "EndResult");

                }

                return RedirectToAction("EndResult", "EndResult");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("EndResult", "EndResult");

            }
        }

        [HttpGet]

        public ActionResult EditEndResult(int id)
        {
            BL_EndResult db = new BL_EndResult();
            ModelState.Clear();
            return View(db.GetEndResult(id));
        }

        [HttpPost]
        public JsonResult EditEndResult(int EndResultID, string EndResultName, EndResult EndResult1)
        {

            string _Edit = null;
            if (db.CheckEndResult(EndResultID, EndResultName))
            {
                if (db.Save(EndResult1))
                {
                    _Edit = "EndResult Edited Successfully";
                }
            }
            else
            {
                _Edit = "EndResult Already Exist's";
            }
            return new JsonResult { Data = _Edit, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Delete(int EndResultID)
        {

            string val = "";

            BL_EndResult db = new BL_EndResult();
            if (db.DeleteEndResult(EndResultID))
            {
                val = "EndResult Deleted Successfully";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }


	}
}