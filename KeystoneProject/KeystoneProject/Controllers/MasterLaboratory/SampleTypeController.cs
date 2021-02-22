using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class SampleTypeController : Controller
    {

        BL_SampleType sampletype = new BL_SampleType();

        public JsonResult ShowSampleType()
        {

            BL_SampleType sampletype1 = new BL_SampleType();

            return new JsonResult { Data = sampletype1.SelectAllSampleType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

       [HttpGet]
        public ActionResult SampleType()
        {
            return View();
        }

        [HttpPost]
       public ActionResult SampleType(SampleType obj)
        {


            try
            {
                BL_SampleType sampletype2 = new BL_SampleType();
                if (sampletype2.CheckSampleType(obj.SampleTypeID, obj.SampleTypeName))
                {
                    if (sampletype2.SampleType(obj))
                    {
                        if (obj.SampleTypeID > 0)
                        {
                            ModelState.Clear();
                            //ViewData["flag"] = "SampleType Save Successfully";
                            TempData["msg"] = "SampleType Update Successfully";
                            return RedirectToAction("SampleType", "SampleType");
                        }
                        else
                        {
                            ModelState.Clear();
                            //ViewData["flag"] = "SampleType Save Successfully";
                            TempData["msg"] = "SampleType Saved Successfully";
                            return RedirectToAction("SampleType", "SampleType");
                        }
                       
                    }
                }
                else
                {
                    TempData["msg"] = "SampleType Already Exist's";
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
           
        }

        
        [HttpGet]

        public JsonResult EditSampleType1(int id)
        {
            BL_SampleType sampletype1 = new BL_SampleType();

            ModelState.Clear();

            return new JsonResult { Data = sampletype1.GetSampleType(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return View(db.GetDepartment(id));
        }

        public ActionResult DeleteSampleType(int SampleTypeID)
        {
            string del = "";
            SampleTypeID = Convert.ToInt32(Request.Form["SampleTypeID"]);
            BL_SampleType sampletype1 = new BL_SampleType();

            
          
            if (sampletype1.DeleteSampleType(SampleTypeID))
            {

                del = "SampleType Deleted Successfully";
            }
             
              return Json(del, JsonRequestBehavior.AllowGet);
      
        }      


	}
}