using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
namespace KeystoneProject.Controllers.Master
{
    public class DoctorCommissionSettingController : Controller
    {
        //
        // GET: /DoctorCommissionSetting/
        [HttpGet]
        public ActionResult DoctorCommissionSetting()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoctorCommissionSetting(DoctorCommissionSetting obj, DoctorCommissiondgv objdgv,OPDDGV objOPDDGV,IPDDGV objIPD ,LAB objLABDgv,FormCollection fc)
        {
           
         //   obj.OPDdgv[] = obj1;

            try
            {
                //string OPDDGV = Request.Form["ServiceNamedgv"].ToString();
                string DoctorID = fc["DoctorID"];
                if (fc["DoctorID"] != "")
                {

                    //objOPDDGV.ModeDgvOPD = fc["ModeDgvOPD"];
                    string tick = fc["tick"];
                    string m = fc["Mode"];
                    (obj.doc) = (DoctorID);


                    if (m == "Edit")
                    {
                        obj.Mode = m;
                    }
                    else
                    {
                        obj.Mode = "Add";
                    }
                    Buisness_Logic.Master.BL_DoctorCommissionSetting BLobj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();
                    if (tick == null)
                    {
                        obj.CheckRow = "0";
                    }
                    else
                    {
                        obj.CheckRow = (tick);
                    }
                    var id = obj.DoctorCommissionSettingID;
                    if (BLobj.Save(obj, objdgv, objOPDDGV, objIPD, objLABDgv))
                    {

                        if (id == 0)
                        {
                            TempData["Msg"] = "Record Saved Successfully";
                            return RedirectToAction("DoctorCommissionSetting", "DoctorCommissionSetting");
                        }
                        else
                        {
                            TempData["Msg"] = "Record Updated Successfully";
                            return RedirectToAction("DoctorCommissionSetting", "DoctorCommissionSetting");
                        }
                        
                    }
                }
                TempData["Msg"] = "Select DoctorName";
                
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("DoctorCommissionSetting", "DoctorCommissionSetting");
            }
            return RedirectToAction("DoctorCommissionSetting", "DoctorCommissionSetting");
        }

        public ActionResult BindServiceNameOPDIPD(string ServiceName, string ServiceGroupID, string Type)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.BindServiceNameOPDIPD(ServiceName, ServiceGroupID, Type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindServiceGroupNameOPDIPD(string ServiceGroupName, string Type)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.BindServiceGroupNameOPDIPD(ServiceGroupName, Type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetLabGroupName(string ServiceGroupID)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.GetLabGroupName(ServiceGroupID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceGroup(string ServiceGroupID, string Type)
        {
            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();
            return new JsonResult { Data = obj.GetServiceGroup(ServiceGroupID, Type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult DeleteDoctorCommissionSetting(int DoctorCommissionSettingID)
        {
            string Delete = "";
            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();
            if (obj.DeleteDoctorCommissionSetting(DoctorCommissionSettingID))
            {
                Delete = "Delete";
            }
            else
            {
                Delete = "Not Delete";
            }
            return new JsonResult { Data = Delete, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindServiceGroupNameLAB(string ServiceGroupName)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.BindServiceGroupNameLAB(ServiceGroupName), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindServiceNameLAB(string ServiceName, string Type)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.BindServiceNameLAB(ServiceName), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetDoctor(string DoctorID)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.GetDoctor(DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetDoctorName(string prefix)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.GetDoctorName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult EdiillData(string DoctorID)
        {

            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();

            return new JsonResult { Data = obj.GetDoctorCommissionSetting(DoctorID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetAllDoctorCommissionSetting()
        {
           
            Buisness_Logic.Master.BL_DoctorCommissionSetting obj = new Buisness_Logic.Master.BL_DoctorCommissionSetting();
           
            return new JsonResult { Data = obj.GetAllDoctorCommissionSetting(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}