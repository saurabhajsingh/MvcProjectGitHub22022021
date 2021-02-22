using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using KeystoneProject.Models.PharmacyMaster;
using KeystoneProject.Buisness_Logic.PharmacyMaster;


namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class ManufactureController : Controller
    {
        BL_Manufacture1 MN = new BL_Manufacture1();

        // GET: /Manufacture/
        public ActionResult Manufacture()
        {
            return View();
        }
        public JsonResult ShowmManufacture()
        {

            return new JsonResult { Data = MN.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]

        public ActionResult Manufacture(Manufacture obj1, FormCollection fc)
        {
            BL_Manufacture1 MN = new BL_Manufacture1();
            try
            {
                obj1.ManufactureName = fc["ManufactureName"];
                if (MN.CheckManufacture(obj1.ManufactureID, obj1.ManufactureName))
                {
                    if (obj1.ManufactureID==0)
                    {
                        MN.Save(obj1);
                        TempData["Msg"] = "ManufactureName Saved Successfully";
                        return RedirectToAction("Manufacture", "Manufacture");

                    }
                   else
                    {
                        MN.Save(obj1);
                      //  ModelState.Clear();
                        TempData["Msg"] = " ManufactureName Updated Successfully";
                        return RedirectToAction("Manufacture", "Manufacture");
                    }

                }

                else
                {

                    TempData["Msg"] = "ManufactureName Already Exist's";
                    return RedirectToAction("Manufacture", "Manufacture");

                }

                return RedirectToAction("Manufacture", "Manufacture");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Manufacture", "Manufacture");

            }
        }

        public JsonResult Edit(string id)
        {

            ModelState.Clear();

            return new JsonResult { Data =MN.GETtManufacture(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult Delete(int ManufactureID)
        {

            string val = "";

            BL_Manufacture1 MN = new BL_Manufacture1();
            if (MN.DeleteManufacture(ManufactureID))
            {
                val = "ManufactureName Deleted Successfully";
            }

            return Json(val);
        }

    }
}

    

