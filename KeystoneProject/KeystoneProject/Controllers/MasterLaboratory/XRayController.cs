using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class XRayController : Controller
    {
        private SqlConnection con;

        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);

        public void HospitalLocation()
        {
            HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        }
        //
        // GET: /XRay/
        public ActionResult XRay()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XRay(XRay obj, FormCollection fc)
        {
            try
            {
                BL_XRay _TestMaster = new BL_XRay();
                if (_TestMaster.CheckTestMaster(obj.TestID, obj.ReportName))
                {
                    if (obj.ReportName != null)
                {
                    obj.ReportName = fc["Reportname"].ToString();
                }
                else
                {
                    obj.ReportName = "";
                }
                if (obj.HSNCode != null)
                {
                    obj.HSNCode = fc["HSNCode"].ToString();
                }
                else
                {
                    obj.HSNCode = "";
                }
                if (obj.TestID == null)
                {
                    obj.TestID = "";
                }
                else
                {
                    obj.TestID = fc["TestID"].ToString();
                }

                if (obj.PrintAs != "" || obj.PrintAs != null)
                {
                    obj.PrintAs = fc["printAs"].ToString();
                }
                else
                {
                    obj.PrintAs = "";
                }
                  
                if (obj.CategoryName != "" || obj.CategoryName != null)
                {
                    obj.CategoryName = fc["CategoryName"].ToString();
                }
                else
                {
                    obj.CategoryName = "";
                }

                if (obj.CategoryID != "" || obj.CategoryID != null)
                {
                    obj.CategoryID = fc["CategoryID"].ToString();
                }
                else
                {
                    obj.CategoryID = "";
                }
             
                if (obj.OPDGeneralCharges != "" || obj.OPDGeneralCharges != null)
                {
                    obj.OPDGeneralCharges = fc["OPDGeneralCharges"].ToString();
                }
                else
                {
                    obj.OPDGeneralCharges = "0.00";
                }

                if (obj.OPDEmergencyCharges != "" || obj.OPDEmergencyCharges != null)
                {
                    obj.OPDEmergencyCharges = fc["OPDEmergencyCharges"].ToString();
                }
                else
                {
                    obj.OPDEmergencyCharges = "0.00";
                }

               
                if (obj.WardID == null)
                {
                    obj.WardID = "0";
                }
                else
                {
                    obj.WardID = fc["WardID"].ToString();
                }
                if (obj.WardName == null)
                {
                    obj.WardName = "";
                }
                else
                {
                    obj.WardName = fc["WardName"].ToString();
                }
                if (obj.TestMasterDetailIPDChargeID == null)
                {
                    obj.TestMasterDetailIPDChargeID = "";
                }
                else
                {
                    obj.TestMasterDetailIPDChargeID = fc["TestMasterDetailIPDChargeID"].ToString();
                }


                if (fc["OPDEmergencyCharges"].ToString() == null || fc["OPDEmergencyCharges"].ToString() == "")
                {
                    obj.OPDEmergencyCharges = "0";
                }
                else
                {
                    obj.OPDEmergencyCharges = fc["OPDEmergencyCharges"].ToString();
                }

                if (fc["OPDGeneralCharges"].ToString() == null || fc["OPDGeneralCharges"].ToString() == "")
                {
                    obj.OPDGeneralCharges = "0";
                }
                else
                {
                    obj.OPDGeneralCharges = fc["OPDGeneralCharges"].ToString();
                }
                if (fc["GeneralCharges"].ToString() == null || fc["GeneralCharges"].ToString() == "")
                {
                    obj.GenralWardIDPCharges = "0";
                }
                else
                {
                    obj.GenralWardIDPCharges = fc["GeneralCharges"].ToString();
                }

                if (fc["EmergencyCharges"].ToString() == null || fc["EmergencyCharges"].ToString() == "")
                {
                    obj.EmergencyIPDCharges = "0";
                }
                else
                {
                    obj.EmergencyIPDCharges = fc["EmergencyCharges"].ToString();
                }
                    if (fc["unit"].ToString() == null || fc["unit"].ToString() == "")
                    {
                        obj.unit = "0";
                    }
                    else
                    {
                        obj.unit = fc["unit"].ToString();
                    }
                    if (obj.TestMasterDetailIPDChargeID == null)
                    {
                        obj.TestMasterDetailIPDChargeID = "";
                    }
                    else
                    {
                        obj.TestMasterDetailIPDChargeID = fc["TestMasterDetailIPDChargeID"].ToString();
                    }
                    if (obj.TestID == "")
                    {
                        obj.TestID = "0";
                    }
                    int testid = Convert.ToInt32(obj.TestID);

                    if (_TestMaster.SaveXRay(obj))
                {

                        if (testid > 0)
                        {
                            ModelState.Clear();

                            TempData["msg"] = "Reporting Updated Successfully";
                            return RedirectToAction("XRay", "XRay");
                        }
                        else
                        {
                            ModelState.Clear();

                            TempData["msg"] = "Reporting Saved Successfully";
                            return RedirectToAction("XRay", "XRay");
                        }
                  
                }
                }
                else
                {
                    TempData["Msg"] = "Reporting Name Already Exist's";
                }
                return RedirectToAction("XRay", "XRay");
            }
            catch (Exception ex)
            {
                return RedirectToAction("XRay", "XRay");
            }


        }
        public JsonResult GetTestMasterForBindCategory(string prefix)
        {
            BL_XRay _TestMaster = new BL_XRay();

            return new JsonResult { Data = _TestMaster.GetTestMasterForBindCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetUnitID(string prefix)
        {
            BL_XRay _TestMaster = new BL_XRay();
            return new JsonResult { Data = _TestMaster.GetUnitID(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllWard(string prefix)
        {
            List<XRay> ser = new List<XRay>();
            BL_XRay BL_obj = new BL_XRay();
            XRay obj = new XRay();

            obj.dsAllTestMaster = BL_obj.GetAllWard();

            foreach (DataRow dr in obj.dsAllTestMaster.Tables[0].Rows)
            {
                XRay obj1 = new XRay();
                obj1.WardID = dr["WardID"].ToString();
                obj1.WardName = dr["WardName"].ToString();
                obj1.GeneralCharges = dr["GeneralCharges"].ToString();
                obj1.EmergencyCharges = dr["EmergencyCharges"].ToString();
                ser.Add(obj1);
            }



            return Json(ser, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllXray()
        {
            BL_XRay BL_obj = new BL_XRay();
            return new JsonResult { Data = BL_obj.GetAllParameter(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Edit(int TestID)
        {
            BL_XRay _TestMaster = new BL_XRay();
            XRay obj = new XRay();
            List<XRay> GetList = new List<XRay>();
            DataSet ds = _TestMaster.GetTestMaster(TestID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetList.Add(new XRay

                {
                    HospitalID = Convert.ToInt32(dr["HospitalID"]),

                    LocationID = Convert.ToInt32(dr["LocationID"]),
                    TestID = dr["TestID"].ToString(),
                    ReportName = dr["TestName"].ToString(),
                    PrintAs = dr["PrintAs"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),                   
                    CategoryID = dr["Category"].ToString(),
                    CategoryName = dr["CategoryName"].ToString(),
                    Footer = dr["Footer"].ToString(),
                    unit = dr["Unit"].ToString(),
                });
            }



            return new JsonResult { Data = GetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetMasterDetails(int TestID)
        {
          
            BL_XRay _TestMaster = new BL_XRay();
            XRay obj = new XRay();
            obj.dsGetAllWardName = _TestMaster.GetTestMasterDetailIPDCharge(TestID);
            List<TestMaster> GetWard = new List<TestMaster>();
            foreach (DataRow dr in obj.dsGetAllWardName.Tables[0].Rows)
            {
                GetWard.Add(new TestMaster
                {
                    WardID = dr["WardID"].ToString(),
                    WardName = dr["WardName"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                    TestMasterDetailIPDChargeID = dr["TestMasterDetailIPDChargeID"].ToString(),
                    TestID = dr["TestID"].ToString(),
                    Mode = dr["Mode"].ToString(),


                });
            }

            return Json(new {GetWard = GetWard }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int TestID)
        {
            string val = "";
            XRay obj = new XRay();
            BL_XRay _TestMaster = new BL_XRay();
            if (_TestMaster.DeleteTestMaster(HospitalID, TestID, LocationID))
            {
                val = "Reporting Deleted Successfully";
            }           
            return Json(val);
        }

    }

}