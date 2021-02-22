using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.PharmacyMaster;
using KeystoneProject.Buisness_Logic.PharmacyMaster;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class GenericInformationController : Controller
    {
        BL_GenericInformation Generic = new BL_GenericInformation();
        // GET: GenericInformation
        public ActionResult GenericInformation()
        {
            return View();
        }
       


        [HttpPost]
        public ActionResult GenericInformation(FormCollection fc, GenericInformation obj)
            
        {
            

            try
            {
                
                if(obj.GenericID==0 )
                {
                    obj.GenericID =0;
                }
               
                obj.GenericName = fc["genericName"].ToString();
                if (Generic.CheckGenericInformation(obj.GenericID, obj.GenericName))
                {

                    if (obj.GenericID > 0)
                    {
                        if (Generic.Save(obj))
                        {

                            ModelState.Clear();
                            TempData["Msg"] = " GenericInformation  Updated Successfully";
                            return RedirectToAction("GenericInformation", "GenericInformation");
                        }
                    }
                    else
                    {
                        if (Generic.Save(obj))
                        {

                            ModelState.Clear();
                            TempData["Msg"] = " GenericInformation Saved Successfully";
                            return RedirectToAction("GenericInformation", "GenericInformation");
                        }
                    }

                    

                }
                else
                {

                    TempData["Msg"] = " GenericInformation Already Exist's";
                    return RedirectToAction("GenericInformation", "GenericInformation");
                }
                return RedirectToAction("GenericInformation", "GenericInformation");
            }
            catch (Exception ex)
            {
                return RedirectToAction("GenericInformation", "GenericInformation");
            }
            // return RedirectToAction("GenericInformation", "GenericInformation");
        }
        public ActionResult ShowGenericInformation()
        {
            return new JsonResult { Data = Generic.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult EditGenericInformation(string id)
        {
            return new JsonResult { Data = Generic.GetGenericInformation(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult deleteGenericInformation(int id)
        {
            string data = "";
            try
            {
                 int a = Generic.DeleteGenericInformation(id);
                if (a == 1)
                {
                    data = "GenericInformation Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
       }
    }
    

}