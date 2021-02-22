using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;


namespace KeystoneProject.Controllers.Master
{
    public class MedicineTypeController : Controller
    {

        BL_MedicineType dbunit = new BL_MedicineType();


        public JsonResult GetAllMedicineType()
        {
            return new JsonResult { Data = dbunit.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //
        // GET: /MedicineType/
        public ActionResult MedicineType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MedicineType(MedicineType objMedicineType)
         {
            try
            {
                if (dbunit.CheckMedicineType(objMedicineType.MedicineTypeID, objMedicineType.MedicineTypeName))
                {
                    if (dbunit.Save(objMedicineType))
                    {
                        if (objMedicineType.MedicineTypeID > 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Type Update Successfully";
                            return RedirectToAction("MedicineType", "MedicineType");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Type Saved Successfully";
                            return RedirectToAction("MedicineType", "MedicineType");
                        }
                      
                    }
                }
                else
                {
                    TempData["msg"] = "MedicineType Already Exist's";
                    return RedirectToAction("MedicineType", "MedicineType");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return RedirectToAction("MedicineType", "MedicineType");
            }
        }
        [HttpGet]

        public JsonResult EditMedicineType(int id)
        {
            return new JsonResult { Data = dbunit.GetMedicineType(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteMedicineType(int MedicineTypeID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = dbunit.Delete(Convert.ToInt32(MedicineTypeID));
                _Del = "MedicineType Deleted Successfully";

            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
	}
}