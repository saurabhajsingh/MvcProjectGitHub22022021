using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class ChiefHistoryController : Controller
    {
        //
        // GET: /ChiefHistory/
        BL_ChiefHistory obj = new BL_ChiefHistory();

        public JsonResult ShowChiefHistory()
        {

            BL_ChiefHistory db = new BL_ChiefHistory();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult ChiefHistory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChiefHistory(ChiefHistory objChiefHistory)
        {
            try
            {
                if (obj.CheckChiefHistory(objChiefHistory.ChiefHistoryID, objChiefHistory.ChiefHistoryName))
                {
                    if(objChiefHistory.ChiefHistoryID>0)
                    {
                        if (obj.Save(objChiefHistory))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Chief History Updated Successfully";
                        }
                    }
                    else
                    {
                        if (obj.Save(objChiefHistory))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Chief History Saved Successfully";
                        }
                    }
                    
                }
                else
                {
                    TempData["msg"] = "Chief History Already exist";
                }
                return RedirectToAction("ChiefHistory", "ChiefHistory");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("ChiefHistory", "ChiefHistory");
            }

        }


        [HttpGet]

        public JsonResult EditChiefHistory(int id)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetChiefHistory(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult DeleteChiefHistory(int ChiefHistoryID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(ChiefHistoryID));

                _Del = "ChiefHistory Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
	}
}