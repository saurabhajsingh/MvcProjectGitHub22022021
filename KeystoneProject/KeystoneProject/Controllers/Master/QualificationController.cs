using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
namespace KeystoneProject.Controllers.Master
{
    public class QualificationController : Controller
    {
        //
        // GET: /Qualification/

        [HttpGet]
        public ActionResult Qualification()
        {
          
            return View();
        }


        public ActionResult QualificationGetAll()
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Qualification obj = new Buisness_Logic.Master.Bl_Qualification();

            Qualification location = new Qualification();
            location.StoreAllQualification = obj.SelectAllDataQualification();


            List<Qualification> searchList = new List<Qualification>();
            foreach (DataRow dr in location.StoreAllQualification.Tables[0].Rows)
            {
                searchList.Add(new Qualification
                {
                    QualifictionID = Convert.ToInt32( dr["QualifictionID"]),
                    QualifictionName = dr["QualifictionName"].ToString()

                });

            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult Qualification(Qualification location)
        {

            KeystoneProject.Buisness_Logic.Master.Bl_Qualification obj = new Buisness_Logic.Master.Bl_Qualification();
           
          //  Qualification location = new Qualification();
            location.StoreAllQualification = obj.SelectAllDataQualification();
            string a = location.QualifictionName.TrimEnd().TrimStart();
            if (obj.CheckQualification(location.QualifictionName.TrimEnd().TrimStart(), location.QualifictionID))
            {
                if (location.QualifictionName != "")
                {
                    if (obj.Save(location))
                    {
                        if (location.QualifictionID > 0)
                        {
                            TempData["Msg"] = "Qualification Updated Successfully ";
                            ModelState.Clear();
                            RedirectToAction("Qualification", "Qualification");
                        }
                        else
                        {
                            TempData["Msg"] = "Qualification Saved Successfully ";
                            ModelState.Clear();
                            RedirectToAction("Qualification", "Qualification");
                        }
                       
                    }
                
                    // return RedirectToAction("ShowAllQualification", "Qualification");
                }
            }
            else
            {
                try
                {
                    TempData["Msg"] = "Qualification Already Exist's";

                    return RedirectToAction("Qualification", "Qualification");
                  
                }
                catch (Exception)
                {
                    throw;
                }
            }

          return  RedirectToAction("Qualification", "Qualification");
        }
        public JsonResult EditData(int QualifictionID)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Qualification obj = new Buisness_Logic.Master.Bl_Qualification();
            List<Qualification> search = new List<Models.Master.Qualification>();
             Qualification location = new Qualification();
             location.StoreAllQualification = obj.SelectAllDataQualification();
             DataSet ds = obj.SelectByEditMode(QualifictionID);

             location.QualifictionID = Convert.ToInt32(ds.Tables[0].Rows[0]["QualifictionID"].ToString());
             location.QualifictionName = ds.Tables[0].Rows[0]["QualifictionName"].ToString();

             search.Add(location);
             return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteQualification(int QualifictionID)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Qualification obj = new Buisness_Logic.Master.Bl_Qualification();

              Qualification location = new Qualification();
            location.StoreAllQualification = obj.SelectAllDataQualification();
            string _Del = null;
            try
            {
                string DependaincyName = obj.DeleteQualification(Convert.ToInt32(QualifictionID));
                if (DependaincyName == "Delete")
                {
                    _Del = "Qualification Deleted Successfully";

                }
                else
                {
                    _Del = "" + DependaincyName;

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