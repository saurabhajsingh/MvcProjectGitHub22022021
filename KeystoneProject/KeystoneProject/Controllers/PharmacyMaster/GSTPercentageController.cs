using KeystoneProject.Buisness_Logic;
using KeystoneProject.Buisness_Logic.PharmacyMaster;
using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class GSTPercentageController : Controller
    {
        BL_GSTPercentage bl_gst = new BL_GSTPercentage();
        // GET: /GSTPercentage/
        public ActionResult GSTPercentage()
        {
            return View();
        }

        public JsonResult Bind_Table()
        {
            return new JsonResult { Data = bl_gst.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        [HttpPost]
        public ActionResult GSTPercentage(GSTPercentage obj, FormCollection fc)
        {
            if (bl_gst.Check_Tax(obj.gstTax, obj.gstTaxID))
            {
                if (bl_gst.SAVE(obj))
                {
                    if (obj.gstTaxID == 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "GST Percentage Saved Successfully !";
                        return RedirectToAction("GSTPercentage", "GSTPercentage");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "GST Percentage Updated Successfully !";
                        return RedirectToAction("GSTPercentage", "GSTPercentage");
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "GST Percentage Not Saved !";
                    return RedirectToAction("GSTPercentage", "GSTPercentage");
                }
            }
            else
            {
                ViewData["flag"] = "Error";
                TempData["Msg"] = "GST Percentage Already Exists !";
                return RedirectToAction("GSTPercentage", "GSTPercentage");
            }
        }

        public JsonResult Rebind_Data(int gstTaxID)
        {
            return new JsonResult { Data = bl_gst.Bind_From_Table(gstTaxID), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Delete_Tax(int gstTaxID)
        {
            string data = "";
            int a = bl_gst.Delete_Data(gstTaxID);
            if(a==1)
            {
                data = "GST Percentage Deleted Successfully !";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}