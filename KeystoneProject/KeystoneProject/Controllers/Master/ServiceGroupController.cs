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
    public class ServiceGroupController : Controller
    {
        //
        // GET: /ServiceGroup/
        public ActionResult ServiceGroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ServiceGroup(ServiceGroup ObjServiceGroup)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
           // ServiceGroup ObjServiceGroup = new ServiceGroup();
            Buisness_Logic.Master.BL_ServiceGroup objBLServiceGrp = new Buisness_Logic.Master.BL_ServiceGroup();
            try
            {
                if (objBLServiceGrp.CheckServiceGroup(ObjServiceGroup.ServiceGroupID, ObjServiceGroup.ServiceGroupName))
                {
                    if (ObjServiceGroup.ServiceGroupID == 0)
                    {
                        if (objBLServiceGrp.SaveServiceGroup(ObjServiceGroup))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "ServiceGroup Saved Successfully";
                            return RedirectToAction("ServiceGroup", "ServiceGroup");
                        }
                    }
                    else
                    {
                        if (objBLServiceGrp.UpdateServiceGroupData(ObjServiceGroup))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "ServiceGroup Updated Successfully";
                            return RedirectToAction("ServiceGroup", "ServiceGroup");
                        }
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "ServiceGroup Already Exist's";
                    return RedirectToAction("ServiceGroup", "ServiceGroup");
                }
                return RedirectToAction("ServiceGroup", "ServiceGroup");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;

                return RedirectToAction("ServiceGroup", "ServiceGroup");
            }   
          
        }

        public ActionResult EditServiceGroupData(int ServiceGroupID)
        {
            List<ServiceGroup> search = new List<ServiceGroup>();
            Buisness_Logic.Master.BL_ServiceGroup objBLServiceGrp = new Buisness_Logic.Master.BL_ServiceGroup();
            ServiceGroup objSG = new Models.Master.ServiceGroup();
            DataSet ds = objBLServiceGrp.SelectServiceGroupbyID(ServiceGroupID);
            ServiceGroup ServiceGroupModel = new ServiceGroup();
            ServiceGroupModel.ServiceGroupID = Convert.ToInt32(ds.Tables[0].Rows[0]["ServiceGroupID"].ToString());
            ServiceGroupModel.ServiceGroupName = ds.Tables[0].Rows[0]["ServiceGroupName"].ToString();
            ServiceGroupModel.ServiceType = ds.Tables[0].Rows[0]["ServiceType"].ToString();
            ServiceGroupModel.ServicesOrder = Convert.ToInt32(ds.Tables[0].Rows[0]["ServicesOrder"].ToString());
            ServiceGroupModel.HSNCode = ds.Tables[0].Rows[0]["HSNCode"].ToString();
            //AddDoctor.cre = ds.Tables[0].Rows[0]["CreationID"].ToString();
            //AddDoctor.DoctorType = ds.Tables[0].Rows[0]["CreationDate"].ToString();

            search.Add(ServiceGroupModel);
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ShowAllServiceGroup()
        {
            Buisness_Logic.Master.BL_ServiceGroup objBLServiceGrp = new Buisness_Logic.Master.BL_ServiceGroup();
            ServiceGroup objSG = new Models.Master.ServiceGroup();
            objSG.StoreAllServiceGroup = objBLServiceGrp.SelectAllServiceGroup();
            ServiceGroup ObjServiceGroup = new ServiceGroup();
            List<ServiceGroup> search = new List<ServiceGroup>();
            foreach (DataRow dr in objSG.StoreAllServiceGroup.Tables[0].Rows)
            {
                search.Add(new ServiceGroup
                {
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    ServiceGroupID = Convert.ToInt16( dr["ServiceGroupID"]),
                    ServicesOrder = Convert.ToInt16( dr["ServicesOrder"].ToString()),
                    HSNCode = dr["HSNCode"].ToString(),

                });

            }



            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public JsonResult DeleteDoctorAppointment(int ServiceGroupID)
        {
            string _Del = null;
            try
            {
                Buisness_Logic.Master.BL_ServiceGroup objBLServiceGrp = new Buisness_Logic.Master.BL_ServiceGroup();
                ServiceGroup objSG = new Models.Master.ServiceGroup();

                string DependaincyName = objBLServiceGrp.DeleteServiceGroup(Convert.ToInt32(ServiceGroupID));

                if (DependaincyName == "Delete")
                {
                    _Del = "ServiceGroup Deleted Successfully";
                }
                else
                {
                    _Del = "Can not Delete";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}