using KeystoneProject.Buisness_Logic.PharmacyMaster;
using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class HSNCodeController : Controller
    {
        BL_HSNCode bl_hsn = new BL_HSNCode();
        // GET: HSNCode
        public ActionResult HSNCode()
        {
            return View();
        }

        public JsonResult Bind_Table()
        {
            return new JsonResult { Data = bl_hsn.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Rebind_Data(int id)
        {
            return new JsonResult { Data = bl_hsn.Bind_from_table(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        [HttpPost]
        public ActionResult HSNCode(HSNCode obj, FormCollection fc)
        {
            try
            {
                               
                //if (bl_hsn.Check_Tax(obj.gstTax, obj.gstTaxID))
                //{
                if (bl_hsn.SAVE(obj))
                {
                    if (obj.hsnCodeID == 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "HSN Code Saved Successfully !";
                        return RedirectToAction("HSNCode", "HSNCode");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "HSN Code Updated Successfully !";
                        return RedirectToAction("HSNCode", "HSNCode");
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "HSN Code Not Saved !";
                    return RedirectToAction("HSNCode", "HSNCode");
                }
                //}
                //else
                //{
                //    ViewData["flag"] = "Error";
                //    TempData["Msg"] = "HSN Code Already Exists !";
                //    return RedirectToAction("GSTPercentage", "GSTPercentage");
                //}
            }
            catch (Exception ex)
            {
                return RedirectToAction("HSNCode", "HSNCode");
            }
        }

        //public JsonResult DeleteHSNcode(int hsnCodeID)
        //{
        //    string data = "";
        //    try
        //    {
        //        int a = bl_hsn.DeleteHSN(hsnCodeID);
        //        if (a == 1)
        //        {
        //            data = "HSN Code Deleted Successfully";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data = ex.Message;
        //    }
        //    return Json(data, JsonRequestBehavior.AllowGet);

        //}

    }
}