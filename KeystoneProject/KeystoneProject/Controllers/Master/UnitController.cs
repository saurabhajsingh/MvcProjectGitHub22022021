using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class UnitController : Controller
    {
        //
        // GET: /Unit/
        BL_Unit dbunit = new BL_Unit();


        public JsonResult GetAllUnit()
        {
            return new JsonResult { Data = dbunit.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Unit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Unit(Unit objunit)
        {
            try
            {
                if (dbunit.CheckUnit(objunit.UnitID, objunit.UnitName))
                {
                    if (dbunit.Save(objunit))
                    {
                        if (objunit.UnitID > 0)
                        {
                            TempData["Msg"] = "Unit Updated Successfully ";
                            ModelState.Clear();
                        }
                        else
                        {
                            TempData["Msg"] = "Unit Saved Successfully";
                            return RedirectToAction("Unit", "Unit");
                        }
                         

                    }
                }
                else
                {
                    TempData["Msg"] = "Unit Already Exist's";
                    return RedirectToAction("Unit", "Unit");
                }
               
            }
            catch(Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Unit", "Unit");
            }
            return RedirectToAction("Unit", "Unit");
        }


        [HttpGet]

        public JsonResult EditUnit( int id)
        {
            return new JsonResult { Data = dbunit.GetUnit(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteUnit(int UnitID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = dbunit.Delete(Convert.ToInt32(UnitID));
                _Del = "Unit Deleted Successfully";
               
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      


	}
}