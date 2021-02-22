using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using KeystoneProject.Buisness_Logic.Master;
namespace KeystoneProject.Controllers.Master
{
    public class SpecializationController : Controller
    {
        //
        // GET: /Specialization/

        BL_Specialization dept = new BL_Specialization();
        public ActionResult Specialization()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Specialization(Specialization obj, FormCollection dc)
        {
            
           
            try
            {
                BL_Specialization Bl_Dept = new BL_Specialization();
                if (Bl_Dept.CheckSpecialization(obj.SpecializationID, obj.SpecializationName))
                {
                    if (obj.SpecializationID >0)
                    {

                        Bl_Dept.Save(obj);
                        TempData["Msg"] = " Specialization Updated Successfully";
                        ModelState.Clear();

                        TempData["Msg"] = " Specialization Updated Successfully";
                        return RedirectToAction("Specialization", "Specialization");                       
                    }
                    else
                    {
                        Bl_Dept.Save(obj);
                        TempData["Msg"] = "Specialization Saved Successfully";
                        ModelState.Clear();

                        TempData["Msg"] = "Specialization Saved Successfully";
                        return RedirectToAction("Specialization", "Specialization");
                    }
                                     
                }
                else
                {
                    TempData["Msg"] = "Specialization Already Exist's";
                    return RedirectToAction("Specialization", "Specialization");
                }
                return RedirectToAction("Specialization", "Specialization");
            }
            catch (Exception)
            {
                return RedirectToAction("Specialization", "Specialization");
            }
            

        }

        public JsonResult GetAll()
        {
            Specialization location = new Specialization();
            Buisness_Logic.Master.BL_Specialization BL_obj = new Buisness_Logic.Master.BL_Specialization();
            List<Specialization> search = new List<Models.Master.Specialization>();
            DataSet ds = new DataSet();

            ds = BL_obj.SelectAllSpecialization();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                search.Add(new Specialization
                {
                    SpecializationName = dr["SpecializationName"].ToString(),
                    SpecializationID = Convert.ToInt32(dr["SpecializationID"]),
                    Description = dr["Description"].ToString(),


                });
            }


            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EditSpecializationData(int id)
        
        {
            BL_Specialization db = new BL_Specialization();
            ModelState.Clear();

            return new JsonResult { Data = db.SelectSpecializationbyID(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
          
        }


         public JsonResult Delete( int SpecializationID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = dept.DeleteSpecialization(Convert.ToInt32(SpecializationID));
                if (DependaincyName == "Delete")
                {
                    _Del = "Specialization Deleted Successfully";
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