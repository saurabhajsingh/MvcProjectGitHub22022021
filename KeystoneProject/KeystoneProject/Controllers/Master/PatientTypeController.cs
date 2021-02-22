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
    public class PatientTypeController : Controller
    {
        //
        // GET: /PatientType/
        BL_PatientType objPatient = new BL_PatientType();
        public ActionResult PatientType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientType(PatientType obj,FormCollection fc)
        {
            BL_PatientType objPatient = new BL_PatientType();
            try
            {
                
                //if (obj.PatientTypeID == null || obj.PatientTypeID == "" )
                //{
                //    obj.PatientTypeID ="0";
                //}
                //else
                //{
                //    obj.PatientTypeID = (fc["PatientTypeID"]).ToString();
                //}
                //if (fc["PatientTypeName"].ToString() == null || fc["PatientTypeName"].ToString() == "")
                //{
                //    obj.PatientTypeName = "";
                //}
                //else
                //{
                //    obj.PatientTypeName = fc["PatientTypeName"].ToString();
                //}
                if (objPatient.CheckPatientType(obj.PatientTypeID, obj.PatientTypeName))
                {
                    if (objPatient.Save(obj))
                    {
                        if (Convert.ToInt32(obj.PatientTypeID) > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "PatientType Update Successfully";
                            return RedirectToAction("PatientType", "PatientType");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "PatientType Saved Successfully";
                            return RedirectToAction("PatientType", "PatientType");

                        }

                    }
                }
                else
                {
                    //TempData["Msg"] = "Error";
                    TempData["Msg"] = "PatientType Already Exist's";
                    return RedirectToAction("PatientType", "PatientType");

                }

                return RedirectToAction("PatientType", "PatientType");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("PatientType", "PatientType");

            }

        }

        public JsonResult ShowPatientType()
        {

            BL_PatientType objPatient = new BL_PatientType();

            return new JsonResult { Data = objPatient.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [HttpGet]

        public ActionResult EditPatientType(int id)
        {
            BL_PatientType objPatient = new BL_PatientType();
            ModelState.Clear();
            return View(objPatient.GetPatientType(id));
        }

        [HttpPost]
        public JsonResult EditPatientType(string PatientTypeID, string PatientTypeName, PatientType PatientType1)
        {

            string _Edit = null;
            if (objPatient.CheckPatientType(PatientTypeID, PatientTypeName))
            {
                if (objPatient.Save(PatientType1))
                {
                    _Edit = "PatientType Edited Successfully";
                }
            }
            else
            {
                _Edit = "PatientType Already Exist's";
            }
            return new JsonResult { Data = _Edit, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Delete(int PatientTypeID)
        {

            string val = "";

            BL_PatientType objPatient = new BL_PatientType();
            if (objPatient.DeletePatientType(PatientTypeID))
            {
                val = "PatientType Deleted Successfully";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }
	}
}