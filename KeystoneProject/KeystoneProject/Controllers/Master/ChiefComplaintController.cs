using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class ChiefComplaintController : Controller
    {
        //
        // GET: /ChiefComplaint/
        BL_ChiefCompliant obj = new BL_ChiefCompliant();

        public JsonResult ShowChiefComplaint()
        {

            BL_ChiefCompliant db = new BL_ChiefCompliant();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
          [HttpGet]
        public ActionResult ChiefComplaint()
        {
            return View();
        }

          [HttpPost]
          public ActionResult ChiefComplaint(ChiefComplaint objChiefCompliant)
          {
              try
              {
                  if (obj.CheckChiefComplaint(objChiefCompliant.ChiefComplaintID, objChiefCompliant.ChiefComplaintName))
                  {
                      if (objChiefCompliant.ChiefComplaintID > 0 )
                      {
                          if (obj.Save(objChiefCompliant))
                          {

                              TempData["msg"] = "Chief Complaint Updated Successfully";
                          }
                          
                      }
                      else
                      {
                          if (obj.Save(objChiefCompliant))
                          {

                              TempData["msg"] = "Chief Complaint Saved Successfully";
                          }
                      }
                     
                  }
                  else
                  {
                      TempData["msg"] = "Chief Complaint Already Exist's";
                  }
                  return RedirectToAction("ChiefComplaint", "ChiefComplaint");

              }
              catch (Exception Ex)
              {
                  TempData["msg"] = Ex.Message;
                  return RedirectToAction("ChiefComplaint", "ChiefComplaint");
              }

          }


          [HttpGet]

          public JsonResult EditChiefComplaint(int id)
          {

              ModelState.Clear();

              return new JsonResult { Data = obj.GetChiefComplaint(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

          }


          public JsonResult DeleteChiefComplaint(int ChiefComplaintID)
          {
              string _Del = null;
              try
              {
                  string DependaincyName = obj.Delete(Convert.ToInt32(ChiefComplaintID));

                  _Del = "ChiefComplaint Deleted Successfully";


              }
              catch (Exception)
              {
                  throw;
              }
              return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
          }      
	}
}