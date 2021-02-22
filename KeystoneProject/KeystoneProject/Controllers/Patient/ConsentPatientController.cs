using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Patient
{
    public class ConsentPatientController : Controller
    {

        private SqlConnection con;

        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        List<ConsentPatient> ConsentPatientList = new List<ConsentPatient>();
        BL_ConsentPatient BLConsentPatient = new BL_ConsentPatient();
        ConsentPatient ConsentPatientmodel = new ConsentPatient();
        // GET: ConsentPatient

        [HttpGet]
        public ActionResult ConsentPatient()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConsentPatient(FormCollection fc, ConsentPatient obj)
        {

            try
            {
                //if (consentMaster.CheckConsentMaster(obj.ConsentID, obj.ConsentName))
                //{

                obj.billDate = fc["billDate"] + " " + fc["billTime"];

                if (obj.PatientRegNo != null)
                {
                    obj.PatientRegNo = fc["RegNo"].ToString();
                }
                else
                {
                    obj.PatientRegNo = "";
                }
                if (obj.opdIpdNumberID != null)
                {
                    obj.opdIpdNumberID = fc["opdIpdNumberid"].ToString();
                }
                else
                {
                    obj.opdIpdNumberID = "";
                }
                if (obj.PatientName != null)
                {
                    obj.PatientName = fc["patientName"].ToString();
                }
                else
                {
                    obj.PatientName = "";
                }
                if (obj.PatientType1 != null)
                {
                    obj.PatientType1 = fc["PatientType1"].ToString();
                }
                else
                {
                    obj.PatientType = "";
                }

                if (obj.Gender != null)
                {
                    obj.Gender = fc["Gender"].ToString();
                }
                else
                {
                    obj.Gender = "";
                }
                if (obj.Address != null)
                {
                    obj.Address = fc["Address"].ToString();
                }
                else
                {
                    obj.Address = "";
                }
                if (obj.GuardianName != null)
                {
                    obj.GuardianName = fc["guardianName"].ToString();
                }
                else
                {
                    obj.GuardianName = "";
                }
                if (obj.ConsultantDrID != null)
                {
                    obj.ConsultantDrID = fc["ConsultantDrID"].ToString();
                }
                else
                {
                    obj.ConsultantDrID = "";
                }
                if (obj.ReferredDrID != null)
                {
                    obj.ReferredDrID = fc["ReferredDrID"].ToString();
                }
                else
                {
                    obj.ReferredDrID = "";
                }
                if (obj.ConsentDetailID == "")
                {
                    obj.ConsentDetailID = "0";
                }

                int consentDetailID = Convert.ToInt32(obj.ConsentDetailID);

                if (BLConsentPatient.Save(obj))
                {

                    if (consentDetailID > 0)
                    {
                        ModelState.Clear();

                        TempData["msg"] = "Record Updated Successfully";
                        return RedirectToAction("ConsentPatient", "ConsentPatient");
                    }
                    else
                    {
                        ModelState.Clear();

                        TempData["msg"] = "Record Saved Successfully";
                        return RedirectToAction("ConsentPatient", "ConsentPatient");
                    }

                }
                //}
                //else
                //{
                //    TempData["Msg"] = "ConsentMaster Already Exist's";
                //}
                return RedirectToAction("ConsentPatient", "ConsentPatient");
            }
            catch (Exception exe)
            {
                throw;
            }
            return RedirectToAction("ConsentPatient", "ConsentPatient");

        }

        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = BLConsentPatient.GetAllFinancialYear();

            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                ConsentPatientList.Add(new ConsentPatient
                {
                    FinancialYear = dr["FinancialYear"].ToString(),
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"])
                });
            }
            return new JsonResult { Data = ConsentPatientList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult FillRegDetails(string PregNo)
        {
            List<ConsentPatient> fillList = new List<ConsentPatient>();
            ConsentPatient catl = new ConsentPatient();

            DataSet dsPatientOPD = new DataSet();
            DataSet dsPatientOPDDetails = new DataSet();
            DataSet dsPrivilegeCard = new DataSet();

            int IPDNo = 0;
            BL_ConsentPatient BL_obj = new BL_ConsentPatient();
            DataSet dsPatient = new DataSet();
            dsPatient.Reset();
            dsPrivilegeCard.Reset();
            if (PregNo != "")
                dsPatient = BL_obj.GetGetPatientConsent(Convert.ToInt32(PregNo));
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        IPDNo = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["OPDIPDID"].ToString());
                    }

                }
            }
            if (dsPatient.Tables.Count > 0)
            {
                if (dsPatient.Tables[0].Rows.Count > 0)
                {
                    catl.PatientRegNo =  dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    catl.PrintRegNO = dsPatient.Tables[0].Rows[0]["PrintRegNO"].ToString();
                    catl.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    catl.GuardianName = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
                    catl.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
                    catl.Gender = dsPatient.Tables[0].Rows[0]["Gender"].ToString();
                    
                    catl.Age = dsPatient.Tables[0].Rows[0]["Age"].ToString();
                    catl.DateOfBirth = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["DateOfBirth"]).ToString("yyyy-MM-dd");
                    catl.MobileNo = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
                    catl.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
                    
                    if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                    {
                        #region IPD Detail Of Patient
                                               
                        catl.OPDIPDID = dsPatient.Tables[0].Rows[0]["OPDIPDID"].ToString();
                        if (dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString() != "" || dsPatient.Tables[0].Rows[0]["ReferredDrID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredDrID"].ToString();
                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["ConsultantDrName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["ReferredDrName"].ToString();
                        }
                     
                        #endregion
                    }
                    else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
                    {
                        #region OPD Detail Of Patient

                        if (dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString() != "" && dsPatient.Tables[0].Rows[0]["ReferredDrID"].ToString() != "0")
                        {
                            catl.ConsultantDrID = dsPatient.Tables[0].Rows[0]["ConsultantDrID"].ToString();
                            catl.ReferredByDoctorID = dsPatient.Tables[0].Rows[0]["ReferredDrID"].ToString();

                            catl.DoctorPrintName = dsPatient.Tables[0].Rows[0]["ConsultantDrName"].ToString();
                            catl.ReffDoctorName = dsPatient.Tables[0].Rows[0]["ReferredDrName"].ToString();
                        }

                        catl.OPDIPDID = dsPatient.Tables[0].Rows[0]["OPDIPDID"].ToString();
                        #endregion
                    }

                }
            }
            fillList.Add(catl);
            return Json(fillList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetConsentMaster()
        {
            return new JsonResult { Data = BLConsentPatient.GetConsentMaster(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_ConsentPatient BL_Reg = new BL_ConsentPatient();
            string RegNo = BL_Reg.GetPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetConsentPatient(int ConsentID)
        {
            BL_ConsentPatient BL_obj = new BL_ConsentPatient();
            DataSet ds = new DataSet();
            List<ConsentPatient> searclist = new List<Models.Patient.ConsentPatient>();

            ds = BL_obj.GetConsentMaster(ConsentID);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ConsentPatient obj = new ConsentPatient();
                obj.Path = ds.Tables[0].Rows[0]["Path"].ToString();
                obj.ConsentID = ds.Tables[0].Rows[0]["ConsentID"].ToString();
                obj.ConsentName = ds.Tables[0].Rows[0]["ConsentName"].ToString();
               

                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllConsentPatient()
        {
            BL_ConsentPatient BL_obj = new BL_ConsentPatient();
            return new JsonResult { Data = BL_obj.GetAllConsentPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult FillPatientConsentOLDBillsNO(int RegNo)
        {
            BL_ConsentPatient BL_obj = new BL_ConsentPatient();
            DataSet ds = new DataSet();
            List<ConsentPatient> searclist = new List<Models.Patient.ConsentPatient>();

            ds = BL_obj.GetPatientConsentOLDBillsNO(RegNo);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searclist.Add(new ConsentPatient
                {
                oldBill = dr["BillNo&Date"].ToString(),
                ConsentDetailID = dr["ConsentDetailID"].ToString(),
                });
            }
            return new JsonResult { Data = searclist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            

        }

        public ActionResult OLDBillDataFill(int BillNo)
        {
            BL_ConsentPatient BL_obj = new BL_ConsentPatient();
            DataSet ds = new DataSet();
            List<ConsentPatient> searclist = new List<Models.Patient.ConsentPatient>();

            ds = BL_obj.GetConsentPatient(BillNo);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searclist.Add(new ConsentPatient
                {
                    Footer = dr["Footer"].ToString(),
                    ConsentDetailID = dr["ConsentDetailID"].ToString(),
                    ConsentID = dr["ConsentID"].ToString(),
                    ConsentName = dr["ConsentName"].ToString(),
                    billDate = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("yyyy-MM-dd"),
                    billTime = Convert.ToDateTime(dr["PatientRegistrationDate"]).ToString("hh:mm"),
            });
            }
            return new JsonResult { Data = searclist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult Delete(int ConsentDetailID)
        {
            string val = "";
            ConsentPatient obj = new ConsentPatient();
            BL_ConsentPatient consentPatient = new BL_ConsentPatient();
            if (consentPatient.DeleteConsentPatient(ConsentDetailID))
            {
                val = "ConsentPatient Deleted Successfully";
            }
            return Json(val);
        }

        public JsonResult GetPatientName(string prefix)
        {

            DataSet ds = BLConsentPatient.GetPatientName(prefix);
            List<ConsentPatient> SearchList = new List<ConsentPatient>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new ConsentPatient
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNO"].ToString(),
                    PrintRegNO = dr["PrintRegNO"].ToString()
                });
            }

            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetPatientRegNo(int PatientRegNo)
            {
            DataSet ds = BLConsentPatient.GetPatientRegNo(PatientRegNo);
            List<ConsentPatient> SearchList = new List<ConsentPatient>();

            if (ds.Tables[0].Rows.Count > 0)
            {

                ConsentPatientmodel.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                ConsentPatientmodel.PatientRegNo = ds.Tables[0].Rows[0]["RegNO"].ToString();
                SearchList.Add(ConsentPatientmodel);

            }
            else
            {
               // SearchList.Add("Record Not Found");
            }
            // TempData["Msg"] = "Record Not Found";
            return Json(SearchList);
        }
    }
}