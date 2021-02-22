using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class AllergiesController : Controller
    {
        //
        // GET: /Allergies/
        BL_Allergies obj = new BL_Allergies();

        public JsonResult ShowAllergies()
        {

            BL_Allergies db = new BL_Allergies();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult Allergies()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Allergies(Allergies objAllergies)
        {
            try
            {
                if (obj.CheckAllergies(objAllergies.AllergiesID, objAllergies.AllergiesName))
                {
                    if (objAllergies.AllergiesID > 0)
                    {
                        if (obj.Save(objAllergies))
                        {

                            TempData["msg"] = "Allergies Updated Successfully";
                        }
                    }
                    else
                    {


                        if (obj.Save(objAllergies))
                        {

                            TempData["msg"] = "Allergies Saved Successfully";
                        }
                    }
                }
                else
                {
                    TempData["msg"] = "Allergies Already exist's";
                }
                return RedirectToAction("Allergies", "Allergies");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("Allergies", "Allergies");
            }

        }


        [HttpGet]

        public JsonResult EditAllergies(int id)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetAllergies(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult DeleteAllergies(int AllergiesID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(AllergiesID));

                _Del = "Allergies Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }      
	}
}