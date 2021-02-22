using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Controllers.Master
{
    public class PriviledgeCardController : Controller
    {
        //
        // GET: /PriviledgeCard/
        BL_PrivilegeCard dbPrivilege = new BL_PrivilegeCard();
        public JsonResult GetAllPrivilageCardServiceGroup()
        {
            return new JsonResult { Data = dbPrivilege.GetAllPrivilageCardServiceGroup(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllPrivilegeCard()
        {
            return new JsonResult { Data = dbPrivilege.GetAllPrivilegeCard(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetPrivilageCard(int PrivilegeCardID)
        {
            return new JsonResult { Data = dbPrivilege.GetPrivilageCard(PrivilegeCardID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PriviledgeCard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PriviledgeCard(PrivilegeCard obPri, priviledgecardarray Arry)
        {
            try
            {
                if (dbPrivilege.CheckPrivilegeCard(obPri.PrivilegeCardID, obPri.CardName))
                {

                    var id = obPri.PrivilegeCardID;


                    //int PrivilegeCardID = Convert.ToInt32(obPri.PrivilegeCardID);
                    string message = dbPrivilege.Save(obPri, Arry);

                    if (id >0)
                    {
                        if (message == "Save")
                        {

                            TempData["msg"] = "Privilege Card Updated Successfully !";
                            return RedirectToAction("PriviledgeCard", "PriviledgeCard");
                        }
                    }
                    else
                    {
                        if (message == "Save")
                        {

                            TempData["msg"] = "Privilege Card Saved Successfully !";
                            return RedirectToAction("PriviledgeCard", "PriviledgeCard");
                        }
                    }
                   
                }
                else
                {
                    TempData["msg"] = "Privilege Card Already Exists !";
                    return RedirectToAction("PriviledgeCard", "PriviledgeCard");
                }
                return RedirectToAction("PriviledgeCard", "PriviledgeCard");
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = ex.Message;
                return RedirectToAction("PriviledgeCard", "PriviledgeCard");
            }
           
            
        }


        public JsonResult GetServiceBind(string prefix)
        {
            return new JsonResult { Data = dbPrivilege.Services(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetTestBind(string prefix)
        {
            return new JsonResult { Data = dbPrivilege.Test(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeletePrivilegeCard(int PrivilegeCardID)
        {
            string data = "";
            try
            {
             
                string a = dbPrivilege.DeletePrivilegeCard(PrivilegeCardID);
                if (a == "Done")
                {
                    data = "Done";
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