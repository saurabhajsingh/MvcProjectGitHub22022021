using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class DeliveryTypeController : Controller
    {

        BL_DeliveryType bl = new BL_DeliveryType();
        //
        // GET: /DeliveryType/
        public ActionResult DeliveryType()
        {
            return View();
        }

        public JsonResult ShowEndResult()
        {

            return new JsonResult { Data = bl.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult DeliveryType(DeliveryType obj1, FormCollection collection)
        {
            BL_DeliveryType db = new BL_DeliveryType();
            try
            {

                if (db.CheckDeliveryType(obj1.DeliveryTypeID, obj1.DeliveryType1))
                {
                    if (db.Save(obj1))
                    {
                        if (obj1.DeliveryTypeID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "DeliveryType Update Successfully";
                            return RedirectToAction("DeliveryType", "DeliveryType");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "DeliveryType Saved Successfully";
                            return RedirectToAction("DeliveryType", "DeliveryType");
                        }
                      

                    }
                }
                else
                {

                    TempData["Msg"] = "DeliveryType Already Exist's";
                    return RedirectToAction("DeliveryType", "DeliveryType");

                }

                return RedirectToAction("DeliveryType", "DeliveryType");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("DeliveryType", "DeliveryType");

            }
        }

        public ActionResult Delete(int DeliveryTypeID)
        {

            string val = "";

            BL_DeliveryType db = new BL_DeliveryType();
            if (db.DeleteEndResult(DeliveryTypeID))
            {
                val = "DeliveryType Deleted Successfully";
            }
           
            return Json(val);
        }

	}
}