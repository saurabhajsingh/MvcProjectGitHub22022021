using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class DepartmentController : Controller
    {
        //
        // GET: /Department/

        BL_Department dept = new BL_Department();

        public ActionResult ShowDepartment()
        {
           
         
            BL_Department db = new BL_Department();
      
            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
           
        }


        [OutputCache(Duration = 10)]
    
        [HttpGet]
        public ActionResult Department()
        {         
            return View();
        }

        [HttpPost]
        public ActionResult Department(Department obj,FormCollection dc)
        {
           
            try
            {
                BL_Department Bl_Dept = new BL_Department();
                if (Bl_Dept.CheckDepartment(obj.DepartmentID, obj.DepartmentName))
                {
                    
                    if (Bl_Dept.Save(obj))
                    {
                        if (obj.DepartmentID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = " Department  Updated Successfully";
                            return RedirectToAction("Department", "Department");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = " Department Saved Successfully";
                            return RedirectToAction("Department", "Department");
                        }

                    }
                    
                }
                else
                {
                    
                    TempData["Msg"] = " Department Already Exist's";
                    return RedirectToAction("Department", "Department");
                }
                return RedirectToAction("Department", "Department");
            }
            catch (Exception)
            {
                return RedirectToAction("Department", "Department");
            }
            return RedirectToAction("Department", "Department");
        }


        [HttpGet]

        public JsonResult EditDepartment1(int id)
        {
            BL_Department db = new BL_Department();
            ModelState.Clear();

            return new JsonResult { Data = db.GetDepartment(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
          //  return View(db.GetDepartment(id));
        }


        public JsonResult DeleteDepartment(int DepartmentID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = dept.Delete(Convert.ToInt32(DepartmentID));
                if (DependaincyName == "Delete")
                {
                    ModelState.Clear();
                    _Del = "Department Deleted Successfully";
                }
                else
                {
                    _Del = "You Delete First" + DependaincyName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      


	}
}