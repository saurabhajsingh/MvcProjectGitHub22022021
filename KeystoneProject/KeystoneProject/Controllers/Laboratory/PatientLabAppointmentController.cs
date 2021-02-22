using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Laboratory;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Laboratory;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace KeystoneProject.Controllers.Laboratory
{
    public class PatientLabAppointmentController : Controller
    {
        //
        // GET: /PatientLabAppointment/

        List<PatientLabAppointment> PatientLabBillsList = new List<PatientLabAppointment>();
        BL_PatientLabAppointment _PatientLabBills = new BL_PatientLabAppointment();
        PatientLabAppointment PatientLabAppointmentobj = new PatientLabAppointment();
        public ActionResult PatientLabAppointment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientLabAppointment(PatientLabAppointment obj,FormCollection collection)
        {
            
            BL_PatientLabAppointment _PatientLabAppointment = new BL_PatientLabAppointment();
            
            if (obj.PatientLabAppointmentID != null )
            {
                obj.PatientLabAppointmentID = collection["PatientLabAppointmentID"].ToString();
            }
            else
            {
                obj.PatientLabAppointmentID = "";
            }
            if (obj.PatientLabAppointmentDetailID != null)
            {
                obj.PatientLabAppointmentDetailID = collection["PatientLabAppointmentDetailID"].ToString();
            }
            else
            {
                obj.PatientLabAppointmentDetailID = "";
            }
            if ( obj.PrefixName=="")
            {
                obj.PrefixName = "";
            }
            else
            {
                obj.PrefixName = collection["PFPatientName"].ToString();
            }
            if (obj.PatientName == null || obj.PatientName == "")
            {
                obj.PatientName = "";
            }
            else
            {
                obj.PatientName = collection["PatientName"].ToString();
            }
            if (obj.MobileNo == null || obj.MobileNo == "")
            {
                obj.MobileNo = "";
            }
            else
            {
                obj.MobileNo = collection["MobileNo"].ToString();
            }

            if (obj.Address == null || obj.Address == "")
            {
                obj.Address = "";
            }
            else
            {
                obj.Address = collection["Address"].ToString();
            }
            if (obj.AppointmentDate == "")
            {
                obj.AppointmentDate = "";
            }
            else
            {
                obj.AppointmentDate = collection["AppointmentDate"].ToString();
            }
            //if ( obj.AppointmentTime == "0")
            //{
            //    obj.AppointmentTime = "";
            //}
            //else
            //{
            //    obj.AppointmentTime = collection["AppTime"].ToString();
            //}
            if ( obj.ConsultantDr == "")
            {
                obj.ConsultantDr = "";
            }
            else
            {
                obj.ConsultantDr = collection["ConDR"].ToString();
            }

            if ( obj.ConsultantDrID == "0")
            {
                obj.ConsultantDrID = "0";
            }
            else
            {
                obj.ConsultantDrID = collection["ConDRID"].ToString();
            }


            if ( obj.RefferedDr == "")
            {
                obj.RefferedDr = "";
            }
            else
            {
                obj.RefferedDr = collection["ReffDR"].ToString();
            }

            if ( obj.RefferedDrID == "0")
            {
                obj.RefferedDrID = "0";
            }
            else
            {
                obj.RefferedDrID = collection["ReffDRID"].ToString();
            }

            if (obj.DOB == null || obj.DOB == "")
            {
                obj.DOB = "";
            }
            else
            {
                obj.DOB = collection["DOB"].ToString();
            }

            if ( obj.SampleCollectionBy == "")
            {
                obj.SampleCollectionBy = "";
            }
            else
            {
                obj.SampleCollectionBy = collection["SampleCollectBy"].ToString();
            }

            if (obj.Gender == null || obj.Gender == "")
            {
                obj.Gender = "";
            }
            else
            {
                obj.Gender = collection["Gender"].ToString();
            }

            if (obj.Year == null || obj.Year == "")
            {
                obj.Year = "0";
            }
            else
            {
                obj.Year = collection["Year"].ToString();
            }
            if (obj.CollectionCentreName == "")
            {
                obj.CollectionCentreName = "";
            }
            else
            {
                obj.CollectionCentreName = collection["CollectionCentre"].ToString();
            }
            if ( obj.OrganizationName == "")
            {
                obj.OrganizationName = "";
            }
            else
            {
                obj.OrganizationName = collection["CompanyName"].ToString();
            }
            if(obj.GuardianName==null || obj.GuardianName=="")
            {
                obj.GuardianName = "";
            }
            else
            {
                obj.GuardianName = collection["GuardianName"].ToString();
            }
          
            obj.TestName = collection["TestNamerow"].ToString();
            obj.TestID = collection["TesttableID"].ToString();
            obj.Rate = collection["rate1"].ToString();
            obj.Quantity = collection["Qty"].ToString();
            obj.Mode = collection["mode"].ToString();
            if (Request.Form["TotalAmt"] != null)
            {
                obj.TotalAmount = Request.Form["TotalAmt"].ToString();

            }
            else
            {
                obj.TotalAmount = Convert.ToDecimal(0.00).ToString();

            }
            obj.TestType = collection["ServiceType"].ToString();

            if (_PatientLabAppointment.SaveTest(obj))
            {
                ModelState.Clear();
                ViewData["flag"] = "Done";
            }

            else
            {
                ViewData["flag"] = "Error";
            }
            return RedirectToAction("PatientLabAppointment", "PatientLabAppointment");

        }

        public ActionResult CompanyNameBind(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.CompanyNameBind(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment name = new PatientLabAppointment();
                name.OrganizationID = dr["OrganizationID"].ToString();
                name.OrganizationName = dr["OrganizationName"].ToString();

                searchList.Add(name);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult CollectionCentre(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.CollectionCentreBind(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment name = new PatientLabAppointment();
                name.CollectionCentreID = dr["CollectionID"].ToString();
                name.CollectionCentreName = dr["CollectionName"].ToString();

                searchList.Add(name);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindPatientPrefix(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            return new JsonResult { Data = objPatient.BindPrefixPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetUser(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.GetUser(prefix);


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment cat = new PatientLabAppointment();

                cat.UserID = Convert.ToInt32(dr["UserID"].ToString());

                cat.FullName = dr["FullName"].ToString();

                searchList.Add(cat);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetPatientName(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds1 = objPatient.GetPatient(prefix);
            //  int i = 0;
            // string a = Session["chk"].ToString();
            //  if (chk == "false" || chk == "")
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    PatientLabAppointment Patient = new PatientLabAppointment();

                    Patient.PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString();

                    Patient.PatientName = dr["PatientName"].ToString();
                    Patient.AppointmentID = Convert.ToInt32(dr["AppointmentID"].ToString());
                    Patient.MobileNo = dr["MobileNo"].ToString();
                    Patient.DOB = Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MM-yyyy");
                    Patient.Address = dr["Address"].ToString();
                    Patient.Gender = dr["Gender"].ToString();
                    searchList.Add(Patient);
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetConsDoctor(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.GetConsDoctor(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment cat1 = new PatientLabAppointment();

                cat1.ConsultantDrID = dr["DoctorID"].ToString();

                cat1.ConsultantDr = dr["DoctorPrintName"].ToString();

                searchList.Add(cat1);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetRefferedDoctor(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.GetRefferedDoctor(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment catl = new PatientLabAppointment();

                catl.RefferedDrID = dr["DoctorID"].ToString();

                catl.RefferedDr = dr["DoctorPrintName"].ToString();

                searchList.Add(catl);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Package(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();


            DataSet ds1 = objPatient.GetPackage(prefix);
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                PatientLabAppointment cat = new PatientLabAppointment();
                cat.ProfileID = Convert.ToInt32(dr["TestID"].ToString());

                cat.ProfileName = dr["TestName"].ToString();
                cat.Rate = dr["Rate"].ToString();
                searchList.Add(cat);
            }


            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult TestName(string prefix)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.GetTest(prefix);

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientLabAppointment cat = new PatientLabAppointment();

                cat.TestID =dr["TestID"].ToString().ToString();

                cat.TestName = dr["TestName"].ToString();
                cat.GeneralCharges = dr["GeneralCharges"].ToString();
                cat.EmergencyCharges = dr["EmergencyCharges"].ToString();

                searchList.Add(cat);
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindCharges1(int tpaID, int testID, string select,int DrID)
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            DataSet ds = objPatient.GetPatientLabBillsForTestMaster(DrID);

            if (select == "Test")
            {

                DataView dv1 = new DataView(ds.Tables[1], " TestID = " + testID + " ", "", DataViewRowState.CurrentRows);
                foreach (DataRow dr in dv1.ToTable().Rows)
                {
                    searchList.Add(new PatientLabAppointment
                    {

                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = dr["EmergencyCharges"].ToString(),


                    });
                }
            }
            else
            {
                DataView dv1 = new DataView(ds.Tables[3], " ProfileID = " + testID + " ", "", DataViewRowState.CurrentRows);
                foreach (DataRow dr in dv1.ToTable().Rows)
                {
                    searchList.Add(new PatientLabAppointment
                    {
                        ProfileID = Convert.ToInt32(dr["ProfileID"].ToString()),
                        ProfileName = dr["Name"].ToString(),
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = dr["EmergencyCharges"].ToString(),


                    });
                }
            }


            return Json(new { searchlist = searchList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNextAppointentID()
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            PatientLabAppointment catl = new PatientLabAppointment();

            string PatientLabAppointment = "";

            PatientLabAppointment = objPatient.GetNextAppointentID(catl).Tables[0].Rows[0]["PatientLabAppointmentID"].ToString();

            return Json(PatientLabAppointment);

        }

        public JsonResult GetAllPatientLabAppointment()
        {
            BL_PatientLabAppointment objPatient = new BL_PatientLabAppointment();
            List<PatientLabAppointment> searchList = new List<PatientLabAppointment>();
            PatientLabAppointment catl = new PatientLabAppointment();

            DataSet ds = objPatient.GetAllPatientLabAppointment();


        
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                searchList.Add(new PatientLabAppointment
                {
                    PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    Address = dr["Address"].ToString(),
                    MobileNo = dr["MobileNo"].ToString(),
                    AppointmentDate = Convert.ToDateTime(dr["PatientLabAppointmentDate"]).ToString("dd-MM-yyyy"),
                   
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Edit(string PatientAppID)
        {
            BL_PatientLabAppointment obj=new BL_PatientLabAppointment();
            List<PatientLabAppointment> SearchLabAppointment=new List<PatientLabAppointment>();
              List<PatientLabAppointment> SearchLabAppointmentDetails=new List<PatientLabAppointment>();
            PatientLabAppointment objGet=new PatientLabAppointment();
            DataSet ds1 = obj.GetPatientLabAppointment(PatientAppID);
            DataSet ds = obj.GetPatientLabAppointmentDetails(PatientAppID);

            foreach(DataRow dr in ds1.Tables[0].Rows)
            {
                SearchLabAppointment.Add(new PatientLabAppointment
                    {
                        LocationId = Convert.ToInt32(dr["HospitalID"]),
                        HospitalId = Convert.ToInt32(dr["LocationID"]),
                        PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                        PrefixName = dr["PatientPrefixName"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                        GuardianName=dr["GuardianName"].ToString(),
                        Year = dr["Age"].ToString(),
                        AgeType = dr["AgeType"].ToString(),
                        DOB = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                        Gender = dr["Gender"].ToString(),
                        Address = dr["Address"].ToString(),
                        ConsultantDr = dr["ConsultantDr"].ToString(),
                        ConsultantDrID = dr["ConsultantDrID"].ToString(),
                        RefferedDr = dr["RefferredDr"].ToString(),
                        RefferedDrID = dr["RefferredDrID"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        SampleCollectionBy = dr["SampleCollectedBy"].ToString(),
                        CollectionCentreName = dr["CollectionCentre"].ToString(),
                        AppointmentDate = Convert.ToDateTime(dr["PatientLabAppointmentDate"]).ToString("yyyy-MM-dd"),
                        CreationID = dr["CreationID"].ToString(),
                        Mode = dr["Mode"].ToString(),
                    });
            }

             foreach(DataRow dr in ds.Tables[0].Rows)
            {
                SearchLabAppointmentDetails.Add(new PatientLabAppointment
                    {
                        HospitalId = Convert.ToInt32(dr["HospitalID"]),
                        LocationId = Convert.ToInt32(dr["LocationID"]),
                        PatientLabAppointmentID = dr["PatientLabAppointmentID"].ToString(),
                        PatientLabAppointmentDetailID =dr["PatientLabAppointmentDetailID"].ToString(),
                        TestID = dr["TestID"].ToString(),
                        TestName = dr["TestName"].ToString(),
                        Rate = dr["Rate"].ToString(),
                        Quantity=dr["Quantity"].ToString(),
                        TotalAmount = dr["TotalAmount"].ToString(),
                        TestType=dr["TestType"].ToString(),
                        OrganizationName = dr["TPAWiseCollect"].ToString(),
                        CreationID = dr["CreationID"].ToString(),
                        Mode = dr["Mode"].ToString(),

                    });
            }

             return Json(new { SearchLabAppointmentDetails = SearchLabAppointmentDetails, SearchLabAppointment = SearchLabAppointment }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string PatientAppID)
        {
            string del = null;

            BL_PatientLabAppointment obj = new BL_PatientLabAppointment();
          
            try
            {

                //  
                DataSet DependaincyName = obj.DeletePatientLabAppointment(PatientAppID);

                del = "LabPatient  Appointment  Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }


            return new JsonResult { Data = del, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; ;
        }
	}
}