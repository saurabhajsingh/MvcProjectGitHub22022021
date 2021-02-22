using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using KeystoneProject.Models.PharmacyMaster;
using KeystoneProject.Buisness_Logic.PharmacyMaster;
//using KeystoneProject.Controllers.PharmacyMaster;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class ProductUnitController : Controller
    {
        //
        // GET: /ProductUnit/
        BL_ProductUnit PU = new BL_ProductUnit();
        public ActionResult ProductUnit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProductUnit(FormCollection fc, ProductUnit obj)

        {

            try
            {

                if (obj.ProductUnitID == 0)
                {
                    obj.ProductUnitID = 0;
                }

                    obj.ProductUnitName = fc["ProductUnitName"].ToString();

                if (PU.CheckProductUnit(obj.ProductUnitID, obj.ProductUnitName))
                {
                    if (PU.Save(obj))
                    {

                        if (obj.ProductUnitID > 0)
                        {


                            ModelState.Clear();
                            TempData["Msg"] = "ProductUnit  Updated Successfully";
                            return RedirectToAction("ProductUnit", "ProductUnit");

                        }
                        else
                        {


                            ModelState.Clear();
                            TempData["Msg"] = " ProductUnit Saved Successfully";
                            return RedirectToAction("ProductUnit", "ProductUnit");



                        }
                    }

                }
                else
                {



                    TempData["Msg"] = " ProductUnit Already Exist's";
                    return RedirectToAction("ProductUnit", "ProductUnit");
                }
                return RedirectToAction("ProductUnit", "ProductUnit");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ProductUnit", "ProductUnit");
            }
            // return RedirectToAction("ProductUnit", "ProductUnit");
        }
    
        public ActionResult showProductUnit()
        {
            return new JsonResult { Data = PU.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult EditProductInformation(string id)
        {
            return new JsonResult { Data = PU.GETProductUnit(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult DeleteProductUnit(int id)
        {
            string data = "";
            try
            {
                int a = PU.DeleteGenericInformation(id);
                if (a == 1)
                {
                    data = "ProductUnit Deleted Successfully";
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
  