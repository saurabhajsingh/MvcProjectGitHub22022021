using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class ProvitionalDiagnosisController : Controller
    {
        //
        // GET: /ProvitionalDiagnosis/
        BL_ProvitionalDiagnosis obj = new BL_ProvitionalDiagnosis();

        public JsonResult ShowProvitionalDiagnosis()
        {

            BL_ProvitionalDiagnosis db = new BL_ProvitionalDiagnosis();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult ProvitionalDiagnosis()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProvitionalDiagnosis(ProvitionalDiagnosis objProvitionalDiagnosis)
        {
            try
            {
                if (obj.CheckProvitionalDiagnosis(objProvitionalDiagnosis.ProvitionalDiagnosisID, objProvitionalDiagnosis.ProvitionalDiagnosisName))
                {
                    if(objProvitionalDiagnosis.ProvitionalDiagnosisID>0)
                    {
                        if (obj.Save(objProvitionalDiagnosis))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Provisional Diagnosis Updated Successfully !";
                        }
                    }
                    else
                    {
                        if (obj.Save(objProvitionalDiagnosis))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Provisional Diagnosis Saved Successfully !";
                        }
                    }
                   
                }
                else
                {
                    TempData["msg"] = "Provisional Diagnosis Already Exists !";
                }
                return RedirectToAction("ProvitionalDiagnosis", "ProvitionalDiagnosis");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("ProvitionalDiagnosis", "ProvitionalDiagnosis");
            }

        }


        [HttpGet]

        public JsonResult EditProvitionalDiagnosis(int id)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetProvitionalDiagnosis(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult DeleteProvitionalDiagnosis(int ProvitionalDiagnosisID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(ProvitionalDiagnosisID));

                _Del = "Provisional Diagnosis Deleted Successfully !";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
	}
}