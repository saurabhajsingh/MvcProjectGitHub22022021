using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master; 
 

namespace KeystoneProject.Controllers.Master
{
    public class ComplaintForOPDController : Controller
    {

        BL_ComplaintForOPD complaint = new BL_ComplaintForOPD();

        public JsonResult ShowComplaint()
        {

            BL_ComplaintForOPD db = new BL_ComplaintForOPD();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        //
        // GET: /ComplaintForOPD/
        [HttpGet]
        public ActionResult ComplaintForOPD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ComplaintForOPD(ComplaintForOPD obj)
        {
            try
            {
                BL_ComplaintForOPD Bl_ComplaintForOPD = new BL_ComplaintForOPD();
                //if (Bl_reason.CheckReason(obj.ReasonID, obj.ReasonName))
                //{
                if (Bl_ComplaintForOPD.Save(obj))
                {
                    ModelState.Clear();
                    ViewData["flag"] = "Done";
                }
                // }
                else
                {
                    ViewData["flag"] = "Error";
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }



        }

        [HttpGet]

        public ActionResult EditComplaintForOPD(int id)
        {
            BL_ComplaintForOPD db = new BL_ComplaintForOPD();
            ModelState.Clear();
            return View(db.GetComplaint(id));
        }

        [HttpPost]
        public JsonResult EditComplaintForOPD(int ComplaintID, string ComplaintName, ComplaintForOPD Complaint1)
        {

            string _Edit = null;
            if (complaint.CheckComplaint(ComplaintID, ComplaintName))
            {
                if (complaint.Save(Complaint1))
                {
                    _Edit = "Department Edited Successfully";
                }
            }
            else
            {
                _Edit = "Department Name Already Exist's";
            }
            return new JsonResult { Data = _Edit, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}