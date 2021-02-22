using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
using System.Dynamic;

namespace KeystoneProject.Controllers.Master
{
    public class ServicesPackageController : Controller
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;
        Services AddServiceMod = new Services();
        ServicesPackage AddServicesPackagesMod = new ServicesPackage();

        DataSet dsServicePackage = new DataSet();
        DataSet dsServicesCharges = new DataSet();
        DataSet dsServicePAckagesCharges = new DataSet();
        DataSet dsServicesNTests = new DataSet();
        //  public  List<ServicesPackage> add { get; set; }
        KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
        List<Services> search1 = new List<Services>();

        IList<ServicesPackage> studentList = new List<ServicesPackage>();

        //public Nullable  List<Services> search {


        //}
        public void HospitalLocation()
        {

            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);


        }
        //
        // GET: /ServicesPackage/    

        public ActionResult ServicesPackage()
        {

            //HospitalID = Convert.ToInt32(Session["HospitalID"]);
            //LocationID = Convert.ToInt32(Session["LocationID"]);
            //UserID = Convert.ToInt32(Session["UserID"]);
            //ServicesPackage location = new ServicesPackage();


            //List<DataSet> add = new List<DataSet>();
            //[0].Columns.Add("HospitalID");
            //location.dsServicesCharges.Tables[0].Columns.Add("LocationID");
            //location.dsServicesCharges.Tables[0].Columns.Add("ServiceDetailsPackagelID");
            //location.dsServicesCharges.Tables[0].Columns.Add("ServiceID");
            //location.dsServicesCharges.Tables[0].Columns.Add("ServiceOrTestID");
            //location.dsServicesCharges.Tables[0].Columns.Add("ServiceOrTestName");
            //location.dsServicesCharges.Tables[0].Columns.Add("ServiceType");
            //location.dsServicesCharges.Tables[0].Columns.Add("Quantity");
            //location.dsServicesCharges.Tables[0].Columns.Add("Rechange");
            //location.dsServicesCharges.Tables[0].Columns.Add("CreationID");
            //location.dsServicesCharges.Tables[0].Columns.Add("Mode");
            //KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            //AddServiceMod.dsServicesCharges = Bl_obj.GetAllWardName1();

            //Session.Add("chk", AddServiceMod.dsServicesCharges);

            //int column = AddServiceMod.dsServicesCharges.Tables[0].Columns.Count;
            //string columAndRow[
            //foreach (DataRow dr in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            //{



            //}

            return View();
        }
        [HttpPost]
        public ActionResult ServicesPackage(ServicesPackage objServicePackage, ServicesPackage_ServicesCharges obj, ServicePackage_OPDCharges objSerPkgOPD)
        {
            try
            {
                Services objservice = new Services();

                if (Request.Form["ServiceID1"].ToString() == "")
                {
                    objServicePackage.serviceID1 = 0;
                }
                else
                {
                    objServicePackage.serviceID1 = Convert.ToInt32(Request.Form["ServiceID1"].ToString());
                }


                objServicePackage.ServiceGroupID = Request.Form["ServiceGroupID"].ToString();
                objservice.UnitID = Request.Form["UnitID"].ToString();

                if (Request.Form["chkrechange"] == "on")
                {
                    objServicePackage.chkrechange = true;
                }
                else
                {
                    objServicePackage.chkrechange = false;
                }


                objservice.ServiceType = Request.Form["ServiceType"].ToString();

                if (objservice.ServiceType == "OPD Package")
                {
                    objSerPkgOPD.ServiceNTestNameOPD = Request.Form["ServiceNTestNameOPD"].Split(',');
                    objSerPkgOPD.ServiceOrTestIDOPD = Request.Form["ServiceOrTestIDOPD"].Split(',');
                    objSerPkgOPD.QuantityOPD = Request.Form["QuantityOPD"].Split(',');
                    objSerPkgOPD.OPDEmergencyCharges = Request.Form["OPDEmergencyCharges"].Split(',');
                    objSerPkgOPD.OPDGenralCharges = Request.Form["OPDGenralCharges"].Split(',');
                }

                else
                {
                    obj.ServiceNTestName = Request.Form["ServiceNTestName"].Split(',');
                    obj.ServiceOrTestID = Request.Form["ServiceOrTestID"].Split(',');
                    obj.Quantity = Request.Form["Quantity"].Split(',');
                    obj.WardID = Request.Form["WardID"].Split(',');
                    obj.GeneralCharges1 = Request.Form["GeneralCharges1"].Split(',');
                    obj.EmergencyCharges1 = Request.Form["EmergencyCharges1"].Split(',');

                }



                if (Bl_obj.Save(objServicePackage, obj, objSerPkgOPD))
                {
                    TempData["Msg"] = "ServicePackage Save Successfully ";
                    ModelState.Clear();
                   
                }

                return RedirectToAction("ServicesPackage", "ServicesPackage");

            }
            catch (Exception Ex)
            {
                return RedirectToAction("ServicesPackage", "ServicesPackage");
            }



        }
        public JsonResult GetAllWard()
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();

            AddServiceMod.dsServicesCharges = Bl_obj.GetAllWardName();


            List<Services> search = new List<Services>();
            foreach (DataRow dr in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            {
                dr["GeneralCharges"] = 0;
                dr["EmergencyCharges"] = 0;
                search.Add(new Services
                {

                    WardID = Convert.ToInt16(dr["WardID"]),
                    WordName = dr["WardName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"]),
                    EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"]),

                });


            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region Delete ServicesPackage

        [HttpPost]
        public JsonResult DeleteServices(int ServiceID, string ServiceType)
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Services Bl_obj = new Buisness_Logic.Master.Bl_Services();
            Services AddServiceMod = new Services();

            string _Del = null;
            try
            {
                int Dependency = Bl_obj.DeleteServices(Convert.ToInt32(ServiceID), ServiceType);
                if (Dependency == 1)
                {
                    _Del = "Services Deleted Successfully";
                }
                else
                {
                    _Del = "You Delete First " + Dependency;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion



        public JsonResult GetServicesPackageforServiceNTest()
        {
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();

            dsServicesNTests = Bl_obj.GetServicesPackageforServiceNTest();

            List<Services> search = new List<Services>();
            foreach (DataRow dr in dsServicesNTests.Tables[0].Rows)
            {

                search.Add(new Services
                {

                    ServiceID = Convert.ToInt32(dr["ServiceID"]),
                    ServiceName = dr["ServiceName"].ToString(),

                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetServiceDetailsWardWise1(int ServiceID)
        {
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();

            dsServicesNTests = Bl_obj.GetServiceDetailsPackage(ServiceID);
            int count = 1;
         DataSet dsWord=   BindServiceCharges();
            foreach (DataRow ds in dsServicesNTests.Tables[1].Rows)
            {
               
            }

            List<ServicesPackage> search = new List<ServicesPackage>();
            foreach (DataRow dr in dsServicesNTests.Tables[1].Rows)
            {
                search.Add(new ServicesPackage
               {
                  
                   SPWchargeID	= dr["ServiceName"].ToString(),
                   ServicePackageID	=dr["ServicePackageID"].ToString(),
                   ServiceType = dr["ServiceType"].ToString(),
                   ServiceOrTestID = Convert.ToInt32( dr["ServiceOrTestID"]),
                   WardID = Convert.ToInt32(dr["WardID"]),
                   WardName = dr["WardName"].ToString(),
                   Quantity = Convert.ToInt32(dr["Quantity"]),
                   GeneralCharges = Convert.ToInt32(dr["GeneralCharges"]),




               });  
                    
                   
            }
            

            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public DataSet BindServiceCharges()
        {
            // dsServicesCharges.Reset();

            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();


            AddServiceMod.dsServicesCharges = Bl_obj.GetAllWardName1();
            //AddServiceMod.dsServicesCharges.Tables[0].Columns.Add("ServiceChargesID");
            //AddServiceMod.dsServicesCharges.Tables[0].Columns.Add("ServiceID");
            //AddServiceMod.dsServicesCharges.Tables[0].Columns.Add("CreationID");
            //AddServiceMod.dsServicesCharges.Tables[0].Columns.Add("Mode");

            //foreach (DataRow dr in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            //{

            //    dr["GeneralCharges"] = 0;
            //    dr["EmergencyCharges"] = 0;
            //}

            return AddServiceMod.dsServicesCharges;
        }

        public DataSet BindServicePackagesCharges(int ServiceID)
        {
            // dsServicesCharges.Reset();

            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();


            AddServicesPackagesMod.dsServicePackagesCharges = Bl_obj.GetAllWardNameWithPackageDetails(ServiceID);

            return AddServicesPackagesMod.dsServicePackagesCharges;
        }

        public JsonResult AddWardForPackage()
        {
            List<Services> search = new List<Services>();

            dsServicesCharges = BindServiceCharges();

            dsServicesCharges.Tables[0].Columns.Add("HospitalID");
            dsServicesCharges.Tables[0].Columns.Add("LocationID");
            dsServicesCharges.Tables[0].Columns.Add("ServiceDetailsPackagelID");
            dsServicesCharges.Tables[0].Columns.Add("ServiceID");
            dsServicesCharges.Tables[0].Columns.Add("ServiceOrTestID");
            dsServicesCharges.Tables[0].Columns.Add("ServiceOrTestName");
            dsServicesCharges.Tables[0].Columns.Add("ServiceType");
            dsServicesCharges.Tables[0].Columns.Add("Quantity");
            dsServicesCharges.Tables[0].Columns.Add("Rechange");
            dsServicesCharges.Tables[0].Columns.Add("CreationID");
            dsServicesCharges.Tables[0].Columns.Add("Mode");

            foreach (DataRow dr in dsServicesCharges.Tables[0].Rows)
            {
                search.Add(new Services
                {
                    WordName = dr["WardName"].ToString(),
                    WardID = Convert.ToInt32(dr["WardID"])
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult TableFromCharges(int ServiceOrTestID, string ServiceType)
        {
            HospitalLocation();
            string TableColumnsWordIDName = "";
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            List<Services> search = new List<Services>();

            dsServicesNTests = Bl_obj.GetServicesPackageforServiceNTest();

            AddServiceMod.dsServicesCharges = BindServiceCharges();
            decimal GeneralCharges = 0;



            dsServicesNTests = Bl_obj.GetServiceDetailsPackage(ServiceOrTestID);
            int count = 1;
            DataSet dsWord = BindServiceCharges();
           
            var modeladd = new List<dynamic>(AddServiceMod.dsServicesCharges.Tables[0].Rows.Count);
            var rowcount = 0;
            foreach (DataRow row in AddServiceMod.dsServicesCharges.Tables[0].Rows)
            {
                var obj = (IDictionary<string, object>)new ExpandoObject();
                rowcount = 0;
                foreach (DataColumn col in AddServiceMod.dsServicesCharges.Tables[0].Columns)
                {
                    string col1 = dsServicesNTests.Tables[1].Rows[rowcount]["GeneralCharges"].ToString();
                    obj.Add(col.ColumnName, dsServicesNTests.Tables[1].Rows[rowcount]["GeneralCharges"]);
                    rowcount++;
                }
                
                 modeladd.Add(obj);
            }



            return new JsonResult { Data = modeladd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ServicePkgesDetailsCharges(int ServiceID, string ServiceType)
        {
            HospitalLocation();
            string TableColumnsWordIDName = "";
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            List<Services> search = new List<Services>();
            List<ServicesPackage> objmodel = new List<ServicesPackage>();

            dsServicesNTests = Bl_obj.GetServicesPackageforServiceNTest();

            AddServicesPackagesMod.dsServicePackagesCharges = BindServicePackagesCharges(ServiceID);
            decimal GeneralCharges = 0;


            foreach (DataRow dr in AddServicesPackagesMod.dsServicePackagesCharges.Tables[1].Rows)
            {
                objmodel.Add(new ServicesPackage
                {

                    ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),

                    ServiceOrTestID = Convert.ToInt32(dr["ServiceOrTestID"].ToString()),
                    ServiceOrTestName = dr["ServiceOrTestName"].ToString(),
                    Quantity = Convert.ToInt32(dr["Quantity"].ToString()),
                    ServiceType = dr["ServiceType"].ToString(),
                    ServiceDetailsPackagelID = Convert.ToInt32(dr["ServiceDetailsPackagelID"].ToString())

                });
            }

            var modeladd = new List<dynamic>(AddServicesPackagesMod.dsServicePackagesCharges.Tables[0].Rows.Count);

            foreach (DataRow row in AddServicesPackagesMod.dsServicePackagesCharges.Tables[0].Rows)
            {
                var obj = (IDictionary<string, object>)new ExpandoObject();

                foreach (DataColumn col in AddServicesPackagesMod.dsServicePackagesCharges.Tables[0].Columns)
                {
                    obj.Add(col.ColumnName, row[col]);
                }

                modeladd.Add(obj);
            }

            return Json(new { modeladd = modeladd, objmodel = objmodel }, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult ShowAllServiceDetailsPackage()
        {
            ServicesPackage location = new ServicesPackage();
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            List<ServicesPackage> serch = new List<ServicesPackage>();
            DataSet ds = Bl_obj.GetAllServiceDetailsPackage();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new ServicesPackage
                {

                    ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),
                    ServiceGroupID = dr["ServiceGroupID"].ToString(),
                    ServiceName = dr["ServiceName"].ToString(),
                    GeneralCharges = Convert.ToDecimal(dr["General Charges"].ToString()),
                    EmergencyCharges = Convert.ToDecimal(dr["Emergency Charges"].ToString()),
                    ServiceType = dr["ServiceType"].ToString(),
                    ServiceGroupName = dr["ServiceGroupName"].ToString(),
                    UnitName = dr["UnitName"].ToString(),
                    chkrechange = Convert.ToBoolean(dr["Rechange"].ToString())



                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServiceCharges(int ServiceID)
        {

            ServicesPackage location = new ServicesPackage();
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            List<ServicesPackage> serch = new List<ServicesPackage>();

            DataSet ds = Bl_obj.GetServicesCharges(ServiceID);

            ServicesPackage obj = new ServicesPackage();


            // obj.ServiceChargesID = ds.Tables[0].Rows[0]["Advise"].ToString();
            // serch.Add(obj);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new ServicesPackage
                {
                    WardID = Convert.ToInt32(dr["WardID"].ToString()),
                    WordName = dr["WardName"].ToString(),
                    ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),

                    GeneralCharges = Convert.ToDecimal(dr["GeneralCharges"].ToString()),
                    EmergencyCharges = Convert.ToDecimal(dr["EmergencyCharges"].ToString()),


                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetServicesChargesOPD(int ServiceID)
        {

            ServicesPackage location = new ServicesPackage();
            KeystoneProject.Buisness_Logic.Master.BL_ServicesPackage Bl_obj = new Buisness_Logic.Master.BL_ServicesPackage();
            List<ServicePackage_OPDCharges> serch = new List<ServicePackage_OPDCharges>();

            DataSet ds = Bl_obj.GetServicesChargesOPD(ServiceID);

            ServicesPackage obj = new ServicesPackage();


            // obj.ServiceChargesID = ds.Tables[0].Rows[0]["Advise"].ToString();
            // serch.Add(obj);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new ServicePackage_OPDCharges
                {

                    ServiceID = Convert.ToInt32(dr["ServiceID"].ToString()),
                    Quantity = Convert.ToInt32(dr["Quantity"].ToString()),
                    ServiceOrTestID = Convert.ToInt32(dr["ServiceOrTestID"].ToString()),
                    ServiceNTestName = dr["ServiceOrTestName"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                    ServiceType = dr["ServiceType"].ToString(),



                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}