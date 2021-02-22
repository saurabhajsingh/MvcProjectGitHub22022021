using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Buisness_Logic.MasterLaboratory;

namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class HeaderController : Controller
    {
        BL_Header header = new BL_Header();

        public JsonResult ShowHeader()
        {

            BL_Header header1 = new BL_Header();

            return new JsonResult { Data = header1.SelectAllHeader(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        [HttpGet]
        public ActionResult Header()
        {
            return View();
        }

         [HttpPost]
        public ActionResult Header(Header obj)
        {
            try
            {
                BL_Header header2 = new BL_Header();
                if (header2.CheckHeader(obj.HeaderID, obj.HeaderName))
                {
                    if (header2.Header(obj))
                    {
                        if (Convert.ToInt32(obj.HeaderID) > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Header Updated Successfully ";
                            return RedirectToAction("Header", "Header");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Header Saved Successfully ";
                            return RedirectToAction("Header", "Header");
                        }
                       
                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Header Already Exist's ";
                }

                return RedirectToAction("Header", "Header");
            }
            catch (Exception)
            {
                return RedirectToAction("Header", "Header");
            }

        }
         [HttpGet]

         public JsonResult EditHeader1(int id)
         {
             BL_Header header1 = new BL_Header();
             ModelState.Clear();

             return new JsonResult { Data = header1.GetHeader(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
             //  return View(db.GetDepartment(id));
         }
         public ActionResult DeleteHeader(int HeaderId)
         {

             string del = "";
             HeaderId = Convert.ToInt32(Request.Form["HeaderId"]);
             BL_Header header1 = new BL_Header();

             if (header1.DeleteHeader(HeaderId))
             {
                 del = "Header Deleted Succussfully";
             }

             return Json(del, JsonRequestBehavior.AllowGet);
         } 
	}
}