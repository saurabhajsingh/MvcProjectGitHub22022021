using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.MasterLaboratory;


namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class CollectionCenterController : Controller
    {
        //
        // GET: /CollectionCenter/
        public ActionResult CollectionCenter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CollectionCenter(CollectionCenter obj,FormCollection fc)
        {
            try
            {
            BL_CollectionCenter _CollectionCenter = new BL_CollectionCenter();
            if (_CollectionCenter.CheckCollectionCenter(obj.CollectionID, obj.CollectionName))
            {
                if (obj.CollectionID == "0" || obj.CollectionID == null)
                {
                    obj.CollectionID = "0";
                }
                else
                {
                    obj.CollectionID = (fc["CollectionID"]);
                }
                if (obj.CollectionName == "" || obj.CollectionName == null)
                {
                    obj.CollectionName = "";
                }
                else
                {
                    obj.CollectionName = fc["CollectionName"].ToString();
                }
                if (obj.Mobile == "")
                {
                    obj.Mobile = "";
                }
                else
                {
                    obj.Mobile = fc["MobileNo"].ToString();
                }
                if (obj.PhoneNo == "")
                {
                    obj.PhoneNo = "";
                }
                else
                {
                    obj.PhoneNo = fc["Phone"].ToString();
                }
                if (obj.Address == "" || obj.Address == null)
                {
                    obj.Address = "";
                }
                else
                {
                    obj.Address = fc["Address"].ToString();
                }
                if (obj.Email == "")
                {
                    obj.Email = "";
                }
                else
                {
                    obj.Email = fc["EmailID"].ToString();
                }
                if (obj.AdminInCharge == "")
                {
                    obj.AdminInCharge = "";
                }
                else
                {
                    obj.AdminInCharge = fc["Admin"].ToString();
                }
                if (_CollectionCenter.SaveTest(obj))
                {
                        if (Convert.ToInt32(obj.CollectionID) > 0)
                        {
                            ModelState.Clear();
                           
                            TempData["Msg"] = "Collection Center Updated Successfully !";
                            return RedirectToAction("CollectionCenter", "CollectionCenter");
                        }
                        else
                        {
                            ModelState.Clear();
                            
                            TempData["Msg"] = "Collection Center Saved Successfully !";
                            return RedirectToAction("CollectionCenter", "CollectionCenter");
                        }
                  
                }
            }
            else
            {
                ViewData["flag"] = "Error";
                TempData["Msg"] = "Collection Center Already Exists !";
            }
            return RedirectToAction("CollectionCenter", "CollectionCenter");
            }
            catch (Exception)
            {
                return RedirectToAction("CollectionCenter", "CollectionCenter");
            }
           

           
        }

        [HttpGet]

        public JsonResult EditDepartment1(int id)
        {
            BL_CollectionCenter _CollectionCenter = new BL_CollectionCenter();
            ModelState.Clear();

            return new JsonResult { Data = _CollectionCenter.GetCollectionCenter(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return View(db.GetDepartment(id));
        }

        public JsonResult ShowCollectionCenter()
        {

            BL_CollectionCenter _CollectionCenter = new BL_CollectionCenter();

            return new JsonResult { Data = _CollectionCenter.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult Delete(int CollectionID)
        {

            string val = "";
            TestMaster obj = new TestMaster();
            BL_CollectionCenter _CollectionCenter = new BL_CollectionCenter();
            if (_CollectionCenter.DeleteCollectionCenter(CollectionID))
            {
                val = "CollectionCenter Deleted Successfully";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }
    }
}
