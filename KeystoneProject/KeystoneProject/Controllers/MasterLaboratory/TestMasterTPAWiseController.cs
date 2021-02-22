using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class TestMasterTPAWiseController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["hpid"]);
            LocationID = Convert.ToInt32(Session["hlid"]);
            CreationID = Convert.ToInt32(Session["usid"]);
        }

        BL_TestMasterTPAWise Blobj = new BL_TestMasterTPAWise();
        TestMasterTPAWise objmodel = new TestMasterTPAWise();
        public ActionResult ShowAllTestMasterTPAWise()
        {
            objmodel.ds = Blobj.GetAllTestMasterTPAWise();
            return View(objmodel);
        }
        [HttpGet]
        public ActionResult TestMasterTPAWise(TestMasterTPAWise objmodel)
        
        
        {

            //DataSet ds = Blobj.GetTestMasterTPAWise(TestTPAWiseID);
            //objmodel.TPAName = ds.Tables[0].Rows[0]["OrganizationName"].ToString();
            //objmodel.TestName = ds.Tables[0].Rows[0]["TestName"].ToString();
            //objmodel.GeneralCharges = ds.Tables[0].Rows[0]["GeneralCharges"].ToString();
            //objmodel.EmergencyCharges = ds.Tables[0].Rows[0]["EmergencyCharges"].ToString();
            //objmodel.NonBillable = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();

            //objmodel.dsgrid = Blobj.GetTestMasterDetailsTPAWise(TestTPAWiseID);
            //GetTestMasterDetailsTPAWise
            return View();
        }

        [HttpPost]
        public ActionResult TestMasterTPAWise(TestMasterTPAWise objmodel, FormCollection form)
        {
            string TestDetailsTPAWiseID = form["TestDetailsTPAWiseID"];
            string WardID = form["WardID"].ToString();
            string GeneralChargess = form["GeneralCharges1"].ToString();
            string EmergencyCharge = form["EmergencyCharges1"].ToString();

            string[] TestDetailsTPAWise_ID = TestDetailsTPAWiseID.Split(',');
            string[] Ward_ID = WardID.Split(',');
            string[] General_Chargess = GeneralChargess.Split(',');
            string[] Emergency_Charge = EmergencyCharge.Split(',');

            WardDetail_class[] ward = new WardDetail_class[Ward_ID.Length];
            for (int i = 0; i < Ward_ID.Length; i++)
            {
                ward[i] = new WardDetail_class();
                ward[i].TestDetailsTPAWiseID = Convert.ToInt32(TestDetailsTPAWise_ID[i]);
                ward[i].WardId = Convert.ToInt32(Ward_ID[i]);
                ward[i].GeneralCharges = General_Chargess[i].ToString();
                ward[i].EmergencyCharges = Emergency_Charge[i].ToString();
            }
            try
            {


                objmodel.ward = ward;

                if (Blobj.Edit(objmodel))
                {
                    if (Convert.ToInt32(objmodel.TestTPAWiseID) > 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TestMasterTPAWise Updated Successfully ";
                        return RedirectToAction("TestMasterTPAWise", "TestMasterTPAWise");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TestMasterTPAWise Saved Successfully ";
                        return RedirectToAction("TestMasterTPAWise", "TestMasterTPAWise");
                    }
                    
                }
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
         public JsonResult GetTpaName(string prefix)
        {
            DataSet ds = Blobj.GetTpaName(prefix);
            List<TestMasterTPAWise> searchlist = new List<TestMasterTPAWise>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new TestMasterTPAWise
                {
                    TPAId = dr["OrganizationID"].ToString(),
                    TPAName = dr["OrganizationName"].ToString()
                });
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
         public JsonResult GetTestName(string TPAIDName)
        {
             DataSet ds1 = Blobj.GetTpaName(TPAIDName);

             DataSet ds = Blobj.GetTestName(ds1.Tables[0].Rows[0]["OrganizationID"].ToString());
             List<TestMasterTPAWise> searchlist = new List<TestMasterTPAWise>();
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 searchlist.Add(new TestMasterTPAWise
                 {
                     TestName = dr["TestName"].ToString(),
                     TestTPAWiseID = dr["TestTPAWiseID"].ToString()
                 });
             }
             return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }

         public JsonResult GetTestMasterDetailsTPAWise(int TestDetailID)
         {
             BL_TestMasterTPAWise Blobj = new BL_TestMasterTPAWise();
             TestMasterTPAWise AddServiceMod = new TestMasterTPAWise();

             AddServiceMod.dsgrid = Blobj.GetTestMasterDetailsTPAWise1(TestDetailID);


             List<WardDetail_class> search = new List<WardDetail_class>();
             foreach (DataRow dr in AddServiceMod.dsgrid.Tables[0].Rows)
             {
                 dr["GeneralCharges"] = 0;
                 dr["EmergencyCharges"] = 0;
                 search.Add(new WardDetail_class
                 {
                     WardId = Convert.ToInt16(dr["WardId"]),
                     WardName = dr["WardName"].ToString(),
                     GeneralCharges = dr["GeneralCharges"].ToString(),
                     EmergencyCharges = dr["EmergencyCharges"].ToString(),



                 });
             }
             return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
         public JsonResult GetAllWard()
         {
            BL_TestMasterTPAWise Blobj = new BL_TestMasterTPAWise();
            TestMasterTPAWise AddServiceMod = new TestMasterTPAWise();

            AddServiceMod.dsgrid = Blobj.GetAllWardName();


            List<WardDetail_class> search = new List<WardDetail_class>();
             foreach (DataRow dr in AddServiceMod.dsgrid.Tables[0].Rows)
             {
                 dr["GeneralCharges"] = 0;
                 dr["EmergencyCharges"] = 0;
                 search.Add(new WardDetail_class
                 {
                     WardId = Convert.ToInt16(dr["WardId"]),
                     WardName = dr["WardName"].ToString(),
                     GeneralCharges = dr["GeneralCharges"].ToString(),
                     EmergencyCharges = dr["EmergencyCharges"].ToString(),



                 });
             }
             return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }

         public JsonResult GetTestMasterTPAWise( string TPAID)
            {
             BL_TestMasterTPAWise Blobj2 = new BL_TestMasterTPAWise();
             TestMasterTPAWise AddServiceMod2 = new TestMasterTPAWise();
             AddServiceMod2.dsgrid = new DataSet();
             AddServiceMod2.dsgrid = Blobj2.GetTestMasterTPAWiseDetails(TPAID);


             List<TestMasterTPAWise> search = new List<TestMasterTPAWise>();
             foreach (DataRow dr in AddServiceMod2.dsgrid.Tables[0].Rows)
             {
                 
                 search.Add(new TestMasterTPAWise
                 {
                    
                     GeneralCharges = dr["GeneralCharges"].ToString(),
                     EmergencyCharges = dr["EmergencyCharges"].ToString(),



                 });
             }
             return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
         public JsonResult SelectAllTestTPAWise()
         {
             BL_TestMasterTPAWise Blobj1 = new BL_TestMasterTPAWise();
             TestMasterTPAWise AddServiceMod1 = new TestMasterTPAWise();

          


             AddServiceMod1.dsgrid = Blobj1.GetAllTestMasterTPAWise();
             List<TestMasterTPAWise> searchList = new List<TestMasterTPAWise>();
             foreach (DataRow dr in AddServiceMod1.dsgrid.Tables[0].Rows)
             {
                 searchList.Add(new TestMasterTPAWise
                 {
                     OrganisationName = dr["OrganizationName"].ToString(),
                     TestName = dr["TestName"].ToString(),
                     TestTPAWiseID = dr["TestTPAWiseID"].ToString(),
                     TestID = dr["TestID"].ToString(),

                 });
             }

             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
         }

         public JsonResult EditServiceTPAWise(int TestTPAWiseID)
         {
             BL_TestMasterTPAWise Blobj1 = new BL_TestMasterTPAWise();
             List<TestMasterTPAWise> searchList = new List<TestMasterTPAWise>();
             DataSet ds = Blobj1.GetTPAWiseID(TestTPAWiseID);
             TestMasterTPAWise AddServiceTPAWise = new TestMasterTPAWise();

             AddServiceTPAWise.TPAName = ds.Tables[0].Rows[0]["OrganizationName"].ToString();
             AddServiceTPAWise.TPAId = ds.Tables[0].Rows[0]["OrganizationID"].ToString();

             AddServiceTPAWise.TestName = ds.Tables[0].Rows[0]["TestName"].ToString();
             AddServiceTPAWise.TestID = ds.Tables[0].Rows[0]["TestID"].ToString();
            
             AddServiceTPAWise.GeneralCharges = ds.Tables[0].Rows[0]["GeneralCharges"].ToString();
             AddServiceTPAWise.EmergencyCharges = ds.Tables[0].Rows[0]["EmergencyCharges"].ToString();
            AddServiceTPAWise.RecommendedByDoctor = ds.Tables[0].Rows[0]["RecommendedByDoctor"].ToString();
            
             AddServiceTPAWise.TestTPAWiseID = Convert.ToInt32(TestTPAWiseID).ToString();

             ds = Blobj1.GetTestDetailsTPAWiseID(TestTPAWiseID);
             searchList.Add(AddServiceTPAWise);

             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 searchList.Add(new TestMasterTPAWise
                 {

                     WardName = dr["WardName"].ToString(),
                     GeneralCharges = dr["GeneralCharges"].ToString(),
                     EmergencyCharges = dr["EmergencyCharges"].ToString(),
                     WardId = Convert.ToInt32(dr["WardID"]),
                     TestDetailsTPAWiseID = Convert.ToInt32(dr["TestDetailsTPAWiseID"]),

                 });
             }
             return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
         }


    }
}
