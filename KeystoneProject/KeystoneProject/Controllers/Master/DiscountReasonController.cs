using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class DiscountReasonController : Controller
    {
        //
        // GET: /DiscountReason/
        [HttpGet]
        public ActionResult DiscountReason()
        {
            KeystoneProject.Models.Master.DiscountReason location = new Models.Master.DiscountReason();
            KeystoneProject.Buisness_Logic.Master.BL_DiscountReason Bl_obj = new Buisness_Logic.Master.BL_DiscountReason();
            location.storeAllDiscount = Bl_obj.SelectAllData();
            return View(location);
        }

        [HttpPost]
        public ActionResult DiscountReason(DiscountReason obj)
        {
            KeystoneProject.Buisness_Logic.Master.BL_DiscountReason objBlDiscount = new Buisness_Logic.Master.BL_DiscountReason();
         
            
            if (objBlDiscount.CheckDiscountReason(obj.DiscountReasonID, obj.DiscountReasonName))
            {
                if(obj.DiscountReasonID>0)
                {
                    objBlDiscount.save(obj);
                    ViewData["msg"] = obj.Message;
                    ModelState.Clear();

                    TempData["Msg"] = " Discount Reason Updated Successfully";
                    return RedirectToAction("DiscountReason", "DiscountReason");
                }
                else
                {
                    objBlDiscount.save(obj);
                    ViewData["msg"] = obj.Message;
                    ModelState.Clear();

                    TempData["Msg"] = " Discount Reason Saved Successfully";
                    return RedirectToAction("DiscountReason", "DiscountReason");
                }
                
                    //return RedirectToAction("ShowDiscountReason", "DiscountReason");
                
            }
            else
            {
                TempData["Msg"] = " Discount Reason Already Exist's";
                return RedirectToAction("DiscountReason", "DiscountReason");
            }
            
            obj.storeAllDiscount = objBlDiscount.SelectAllData();
           // return RedirectToAction("DiscountReason", "DiscountReason");
            return View(obj);
        }
        [HttpPost]
        public JsonResult DeleteDiscountReason(int DiscountReasonID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Master.BL_DiscountReason objBlDiscount = new Buisness_Logic.Master.BL_DiscountReason();
          
                objBlDiscount.Delete(DiscountReasonID);
                data = "Done";
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}