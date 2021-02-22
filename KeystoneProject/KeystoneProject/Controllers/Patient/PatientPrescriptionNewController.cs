using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Net;
 

namespace KeystoneProject.Controllers.Patient
{
    public class PatientPrescriptionNewController : Controller
    {
        //
        // GET: /PatientPrescriptionNew/
        int HospitalID;
        int LocationID;
        int CreationID;

        BL_PatientPrescriptionNew blobj = new BL_PatientPrescriptionNew();
        PatientPrescriptionNew objmodel = new PatientPrescriptionNew();
        //public ActionResult PatientPrescriptionNew()
        //{
        //    HospitlLocationID();
        //    return View();
        //}
        [HttpGet]
        public ActionResult PatientPrescriptionNew()
        {
            HospitlLocationID();
            Session["tabsection1"] = "true";
            Session["tabsection2"] = "false";
            return View();
        }
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        [HttpPost]
       

        public JsonResult BindName(string prefix)
        {
            HospitlLocationID();

            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();

            DataSet ds = BL_obj.SearchPatientOPDByNameID( HospitalID, LocationID, prefix);

            List<PatientPrescriptionNew> lists = new List<PatientPrescriptionNew>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientPrescriptionNew obj = new PatientPrescriptionNew();

                obj.PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString());
                obj.PatientName = dr["Name"].ToString();

                lists.Add(obj);
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMedicine(string prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            DataSet ds = BL_obj.GetMedicine(prefix);
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientPrescriptionNew
                {
                    MedicineLibraryID = dr["MedicineLibraryID"].ToString(),
                    Medicines = dr["Medicines"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetICDCodes(string prefix,string icd)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            DataSet ds = BL_obj.GetICDCodes(prefix,icd);
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientPrescriptionNew
                {
                    ICDCodeID = dr["ICDCodeID"].ToString(),
                    icdcodename = dr["icdcodename"].ToString(),
                    
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }
        public JsonResult GetPackages(string prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            DataSet ds = BL_obj.GetPackages(prefix);

            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new PatientPrescriptionNew
                {
                    PackagesID = dr["PackagesID"].ToString(),
                    PackagesName = dr["PackagesName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult FillDataForModal()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            string guardianname = "";
            try
            {
                DataSet dsDetails = BL_obj.GetPatient(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {
                        if (dr["OPDIPDID"].ToString() == "")
                        {
                            dr["OPDIPDID"] = "0";
                        }
                        searchlist.Add(new PatientPrescriptionNew
                        {

                           PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                           PatientName = dr["PatientName"].ToString(),
                           OPDIPDID = Convert.ToInt32(dr["OPDIPDID"].ToString()),
                            PrintRegNO = Convert.ToInt32(dr["PrintRegNO"].ToString()),
                            GuardianName =  dr["GuardianName"].ToString(),
                    
                           Age =dr["Age"].ToString(),
                           AgeType = dr["AgeType"].ToString(),
                           Gender = dr["Gender"].ToString()
                            
                        });
                    }
                 
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult FillData(string prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> lists = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.GetPatientData(HospitalID, LocationID, Convert.ToInt32(prefix));

               if(dsDetails.Tables[0].Rows.Count>0)
               {
                   PatientPrescriptionNew obj = new PatientPrescriptionNew();
                   obj.Message = "";
                   obj.PatientRegNO =  Convert.ToInt32(dsDetails.Tables[0].Rows[0]["PatientRegNO"].ToString());
                   obj.PatientName = dsDetails.Tables[0].Rows[0]["PatientName"].ToString();
                   obj.OPDIPDID = Convert.ToInt32(dsDetails.Tables[0].Rows[0]["OPDIPDID"].ToString());
                   obj.GuardianName = dsDetails.Tables[0].Rows[0]["GuardianName"].ToString();
                   obj.Age = dsDetails.Tables[0].Rows[0]["Age"].ToString();
                   obj.AgeType = dsDetails.Tables[0].Rows[0]["AgeType"].ToString();
                   obj.Gender = dsDetails.Tables[0].Rows[0]["Gender"].ToString();
                   lists.Add(obj);

               }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(lists, JsonRequestBehavior.AllowGet);

        }


        public JsonResult FillMedicinesOnPackages(int prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> lists = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.GetPackagesForPrescription(HospitalID, LocationID, Convert.ToInt32(prefix));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {
                        lists.Add(new PatientPrescriptionNew { 
                        Message = "",
                        MedicineLibraryID = dr["MedicineLibraryID"].ToString(),
                        Medicines = dr["Medicines"].ToString(),
                       Strength = dr["Strength"].ToString(),
                        Days = dr["Days"].ToString(),
                        Instruction =dr["Instruction"].ToString(),
                        Frequency = dr["Frequency"].ToString(),
                       
                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillMedicinesOnMedicines(int prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> lists = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.FillMedicinesOnMedicines(HospitalID, LocationID, Convert.ToInt32(prefix));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {
                        lists.Add(new PatientPrescriptionNew
                        {
                            Message = "",
                            MedicineLibraryID = dr["MedicineLibraryID"].ToString(),
                            Medicines = dr["Medicines"].ToString(),
                            Strength = dr["Strength"].ToString(),
                            Frequency = dr["Frequency"].ToString(),
                            Instruction = dr["Instruction"].ToString(),
                            Days = dr["Days"].ToString(),

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillICDCodes(int prefix)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> lists = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.FillICDCodes(HospitalID, LocationID, Convert.ToInt32(prefix));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {
                        lists.Add(new PatientPrescriptionNew
                        {
                            Message = "",
                            ICDCodeID = dr["ICDCodeID"].ToString(),
                            ICDCode = dr["ICDCode"].ToString(),
                            ICDName = dr["ICDName"].ToString()
                            

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
  
        public JsonResult GetTabSection1(string tabsection1)
        {

            PatientPrescriptionNew obj = new PatientPrescriptionNew();
            Session["tabsection1"] = tabsection1;
            Session["tabsection2"] = "false";
            return Json("", JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTabSection2(string tabsection2)
        {

            PatientPrescriptionNew obj = new PatientPrescriptionNew();
            Session["tabsection2"] = tabsection2;
            Session["tabsection1"] = "false";
            return Json("", JsonRequestBehavior.AllowGet);
             
        }
        public JsonResult GetPatientPrescriptionOld(string OldPrescriptionID)
         {
            HospitlLocationID();

           
            
            DataSet ds1 = blobj.GetOLDPatientPrescriptionNew(OldPrescriptionID, HospitalID, LocationID);
            DataSet ds = blobj.GetPatientPrescriptionNewMedicine(OldPrescriptionID, HospitalID, LocationID);
            DataSet ds2= blobj.GetPatientPrescriptionICDCodes(OldPrescriptionID, HospitalID, LocationID);
            List<PatientPrescriptionNew> searchlistMed = new List<PatientPrescriptionNew>();
            List<PatientPrescriptionNew> SearchlistOld = new List<PatientPrescriptionNew>();
            List<PatientPrescriptionNew> searchlistICD = new List<PatientPrescriptionNew>();

         PatientPrescriptionNew obj = new PatientPrescriptionNew();

         obj.Tabsection2 = Session["tabsection2"].ToString();
         obj.Tabsection1 = Session["tabsection1"].ToString();
            if(obj.Tabsection1=="true")
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {


                    // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                    obj.PatientPrescriptionNewID = ds1.Tables[0].Rows[0]["PatientPrescriptionNewID"].ToString();
                    obj.Date = Convert.ToDateTime(ds1.Tables[0].Rows[0]["DATE"]).ToString("yyyy-MM-dd");
                    obj.OPDIPDID = Convert.ToInt32(ds1.Tables[0].Rows[0]["OPDIPDID"].ToString());
                    obj.ChiefComplaint = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ChiefComplaint"].ToString());
                    obj.Allergies = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Allergies"].ToString());
                    obj.Height = ds1.Tables[0].Rows[0]["Height"].ToString();
                    obj.Weight = ds1.Tables[0].Rows[0]["Weight"].ToString();
                    obj.Pulse = ds1.Tables[0].Rows[0]["Pulse"].ToString();
                    obj.BP = ds1.Tables[0].Rows[0]["BP"].ToString();
                    obj.BMI = ds1.Tables[0].Rows[0]["BMI"].ToString();
                    obj.Temp = ds1.Tables[0].Rows[0]["Temp"].ToString();
                    obj.ClinicalFindings = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ClinicalFindings"].ToString());
                    obj.ProvisionalDiagonosis = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ProvisionalDiagonosis"].ToString());
                    obj.Investigations = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Investigations"].ToString());
                    obj.TreatmentAdvice = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["TreatmentAdvice"].ToString());
                    obj.Remark = ds1.Tables[0].Rows[0]["Remark"].ToString();

                    if (ds1.Tables[0].Rows[0]["FollowUpDate"].ToString() == "")
                    {
                        obj.FollowUpDate = ds1.Tables[0].Rows[0]["FollowUpDate"].ToString();
                    }
                    else
                    {
                        obj.FollowUpDate = Convert.ToDateTime(ds1.Tables[0].Rows[0]["FollowUpDate"]).ToString("yyyy-MM-dd");

                    }
                    if (ds1.Tables[0].Rows[0]["FollowUpDate"].ToString() == "")
                    {
                        obj.FollowUpDate = ds1.Tables[0].Rows[0]["FollowUpTime"].ToString();
                    }
                    else
                    {
                        obj.FollowUpTime = Convert.ToDateTime(ds1.Tables[0].Rows[0]["FollowUpTime"]).ToString("hh:mm:ss");

                    }

                    SearchlistOld.Add(obj);
                }

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    searchlistMed.Add(new PatientPrescriptionNew
                    {

                        // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                        MedicineLibraryID = dr1["MedicineLibraryID"].ToString(),
                        Medicines = dr1["Medicines"].ToString(),
                        Frequency = dr1["Frequency"].ToString(),
                        Days = dr1["Days"].ToString(),
                        Instruction = dr1["Instruction"].ToString(),
                        Strength = dr1["Strength"].ToString()


                    });
                }
                if (ds2.Tables[0].Rows.Count >0)
                {
                    foreach (DataRow dr1 in ds2.Tables[0].Rows)
                    {
                        searchlistICD.Add(new PatientPrescriptionNew
                        {

                            // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                            ICDCode = dr1["ICDCode"].ToString(),
                            ICDName = dr1["ICDName"].ToString(),
                            ICDCodeID = dr1["ICDCodeID"].ToString(),


                        });
                    }
                }
                
                
            }
            else if (obj.Tabsection2 == "true")
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {

                    // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                    obj.PatientPrescriptionNewID1 = ds1.Tables[0].Rows[0]["PatientPrescriptionNewID"].ToString();
                    obj.Date1 = Convert.ToDateTime(ds1.Tables[0].Rows[0]["DATE"]).ToString("yyyy-MM-dd");
                    obj.OPDIPDID1 = Convert.ToInt32(ds1.Tables[0].Rows[0]["OPDIPDID"].ToString());
                    obj.ChiefComplaint1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ChiefComplaint"].ToString());
                    obj.Allergies1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Allergies"].ToString());
                    obj.Height1 = ds1.Tables[0].Rows[0]["Height"].ToString();
                    obj.Weight1 = ds1.Tables[0].Rows[0]["Weight"].ToString();
                    obj.Pulse1 = ds1.Tables[0].Rows[0]["Pulse"].ToString();
                    obj.BP1 = ds1.Tables[0].Rows[0]["BP"].ToString();
                    obj.BMI1 = ds1.Tables[0].Rows[0]["BMI"].ToString();
                    obj.Temp1 = ds1.Tables[0].Rows[0]["Temp"].ToString();
                    obj.ClinicalFinding1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ClinicalFindings"].ToString());
                    obj.ProvitionalDiagnosis1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ProvisionalDiagonosis"].ToString());
                    obj.Investigation1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Investigations"].ToString());
                    obj.TreatmentAdvice1 = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["TreatmentAdvice"].ToString());
                    obj.Remark1 = ds1.Tables[0].Rows[0]["Remark"].ToString();

                    if (ds1.Tables[0].Rows[0]["FollowUpDate"].ToString() == "")
                    {
                        obj.FollowUpDate1 = ds1.Tables[0].Rows[0]["FollowUpDate"].ToString();
                    }
                    else
                    {
                        obj.FollowUpDate1 = Convert.ToDateTime(ds1.Tables[0].Rows[0]["FollowUpDate"]).ToString("yyyy-MM-dd");

                    }
                    if (ds1.Tables[0].Rows[0]["FollowUpDate"].ToString() == "")
                    {
                        obj.FollowUpDate1 = ds1.Tables[0].Rows[0]["FollowUpTime"].ToString();
                    }
                    else
                    {
                        obj.FollowUpTime1 = Convert.ToDateTime(ds1.Tables[0].Rows[0]["FollowUpTime"]).ToString("hh:mm:ss");

                    }

                    SearchlistOld.Add(obj);
                }

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    searchlistMed.Add(new PatientPrescriptionNew
                    {

                        // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                        MedicineLibraryID1 = dr1["MedicineLibraryID"].ToString(),
                        Medicines1 = dr1["Medicines"].ToString(),
                        Frequency1 = dr1["Frequency"].ToString(),
                        Days1 = dr1["Days"].ToString(),
                        Instruction1 = dr1["Instruction"].ToString(),
                        Strength1 = dr1["Strength"].ToString()


                    });
                }

            }



            return Json(new { SearchlistOld = SearchlistOld, searchlistMed = searchlistMed, searchlistICD = searchlistICD }, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        [ValidateInput(false)] 
        public ActionResult PatientPrescriptionNew(PatientPrescriptionNew objPatientPrescriptionNew)
        {
            try
            {
                
                objPatientPrescriptionNew.PatientPrescriptionNewID = Request.Form["OldPrescriptionID"].ToString();
                objPatientPrescriptionNew.Tabsection1 = Session["tabsection1"].ToString();
                objPatientPrescriptionNew.Tabsection2 = Session["tabsection2"].ToString();

                if(objPatientPrescriptionNew.Tabsection1=="true")
                {
                    //objPatientPrescriptionNew.txtChiefComplaint = WebUtility.HtmlEncode(Request.Form["txtChiefComplaintName"]).Replace("\r\n", "<br>");
                    //objPatientPrescriptionNew.txtAllergies = Request.Form["txtAllergiesName"].ToString();
                    //objPatientPrescriptionNew.txtClinicalFinding = Request.Form["txtClinicalFindingName"].ToString();
                    //objPatientPrescriptionNew.txtProvitionalDiagnosis = Request.Form["txtProvitionalDiagnosisName"].ToString();
                    //objPatientPrescriptionNew.txtInvestigations = Request.Form["txtInvestigationsName"].ToString();
                    //objPatientPrescriptionNew.txtTreatmentAdvice = Request.Form["txtTreatmentAdviceName"].ToString();


                    objPatientPrescriptionNew.MedicineLibraryID = Request.Form["MedicineLibraryID"].ToString();
                    objPatientPrescriptionNew.Medicines = Request.Form["Medicines"].ToString();
                    objPatientPrescriptionNew.Strength = Request.Form["Strength"].ToString();
                    objPatientPrescriptionNew.Frequency = Request.Form["Frequency"].ToString();
                    objPatientPrescriptionNew.Days = Request.Form["Days"].ToString();
                    objPatientPrescriptionNew.Instruction = Request.Form["Instruction"].ToString();

                    if (objPatientPrescriptionNew.ICDCodeIDhidden != "" && objPatientPrescriptionNew.ICDCodeIDhidden != null)
                    {
                        objPatientPrescriptionNew.ICDCodeID = Request.Form["ICDCodeID"].ToString();
                        objPatientPrescriptionNew.ICDCode = Request.Form["ICDCode"].ToString();
                        objPatientPrescriptionNew.ICDName = Request.Form["ICDName"].ToString();
                    }
                  

                    if (blobj.Save(objPatientPrescriptionNew))
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                    }


                    Session["HospitalIDReport"] = HospitalID;
                    Session["LocationIDReport"] = LocationID;
                    Session["PatientRegNoReport"] = objPatientPrescriptionNew.PatientRegNO.ToString();
                    Session["PatientPrescriptionNewIDReport"] = objPatientPrescriptionNew.PatientPrescriptionNewID.ToString();
                    Session["PatientOPDIPDReport"] = objPatientPrescriptionNew.OPDIPDID.ToString();
                     
                }
                else
                    if (objPatientPrescriptionNew.Tabsection2 == "true")
                {
                    objPatientPrescriptionNew.MedicineLibraryID1 = Request.Form["MedicineLibraryID1"].ToString();
                    objPatientPrescriptionNew.Medicines1 = Request.Form["Medicines1"].ToString();
                    objPatientPrescriptionNew.Strength1 = Request.Form["Strength1"].ToString();
                    objPatientPrescriptionNew.Frequency1 = Request.Form["Frequency1"].ToString();
                    objPatientPrescriptionNew.Days1 = Request.Form["Days1"].ToString();
                    objPatientPrescriptionNew.Instruction1 = Request.Form["Instruction1"].ToString();

                    if (blobj.Save1(objPatientPrescriptionNew))
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                    }


                    Session["HospitalIDReport"] = HospitalID;
                    Session["LocationIDReport"] = LocationID;
                    Session["PatientRegNoReport"] = objPatientPrescriptionNew.PatientRegNO1.ToString();
                    Session["PatientPrescriptionNewIDReport"] = objPatientPrescriptionNew.PatientPrescriptionNewID.ToString();
                    Session["PatientOPDIPDReport"] = objPatientPrescriptionNew.OPDIPDID1.ToString();
                     
                }

                return RedirectToAction("RptPatientPrescriptionNew", "PatientReport");
            }
            catch (Exception Ex)
            {
                return RedirectToAction("PatientPrescriptionNew", "PatientPrescriptionNew");
            }

        }

        public JsonResult GetPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_PatientPrescriptionNew BL_Reg = new BL_PatientPrescriptionNew();
            string RegNo = BL_Reg.GetPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult FillRegDetails(int PatientRegNO)
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            DataSet ds = new DataSet();
            List<PatientPrescriptionNew> searclist = new List<Models.Patient.PatientPrescriptionNew>();

            ds = BL_obj.GetConsentMaster(PatientRegNO);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientPrescriptionNew obj = new PatientPrescriptionNew();
                obj.PatientRegNO = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNO"]);
                obj.PrintRegNO = Convert.ToInt32(ds.Tables[0].Rows[0]["PrintRegNO"]);
                obj.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                obj.GuardianName = ds.Tables[0].Rows[0]["GuardianName"].ToString();
                obj.OPDIPDID = Convert.ToInt32(ds.Tables[0].Rows[0]["OPDIPDID"]);
                obj.Age = ds.Tables[0].Rows[0]["Age"].ToString();
                obj.AgeType = ds.Tables[0].Rows[0]["AgeType"].ToString();
                obj.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                searclist.Add(obj);

            }
            return Json(searclist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOldPrescription(string PatientReg)
        {
            HospitlLocationID();
            DataSet ds1 = blobj.GetPatientPrescriptionNewOLDBillsNO(PatientReg, HospitalID, LocationID);
            // string OldBill = ds1.Tables[0].Rows[0]["BillNo&Date"].ToString();
            // objmodel.OldBillNo = ds1.Tables[0].Rows[0]["BillNo&Date"].ToString();

            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                searchlist.Add(new PatientPrescriptionNew
                {
                    PatientPrescriptionNewID = dr["PatientPrescriptionNewID"].ToString(),
                    OldBillNo = dr["PrescriptionNo&Date"].ToString()
                });
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult FillChiefComplaint()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();
            
            try
            {
                DataSet dsDetails = BL_obj.GetAllChiefComplaint(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            ChiefComplaintID = Convert.ToInt32(dr["ChiefComplaintID"].ToString()),
                            ChiefComplaint = dr["ChiefComplaint"].ToString(),
                            

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillInvestigation()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();

            try
            {
                DataSet dsDetails = BL_obj.GetAllTestMaster(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            InvestigationsID = Convert.ToInt32(dr["TestID"].ToString()),
                            Investigations = dr["TestName"].ToString(),


                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillTreatmentAdvices()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();

            try
            {
                DataSet dsDetails = BL_obj.GetAllTreatmentAdvices(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            TreatmentAdviceID = Convert.ToInt32(dr["TreatmentAdviceID"].ToString()),
                            TreatmentAdvice = dr["TreatmentAdviceName"].ToString(),


                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillProvisionalDiagonosis()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();

            try
            {
                DataSet dsDetails = BL_obj.GetAllProvisionalDiagonosis(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            ProvisionalDiagonosisID = Convert.ToInt32(dr["ProvitionalDiagnosisID"].ToString()),
                            ProvisionalDiagonosis = dr["ProvitionalDiagnosisName"].ToString(),


                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillChiefHistory()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();

            try
            {
                DataSet dsDetails = BL_obj.GetAllChiefHistory(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            ClinicalFindingsID = Convert.ToInt32(dr["ChiefHistoryID"].ToString()),
                            ClinicalFindings = dr["ChiefHistoryName"].ToString(),


                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FillAllergies()
        {
            BL_PatientPrescriptionNew BL_obj = new BL_PatientPrescriptionNew();
            List<PatientPrescriptionNew> searchlist = new List<PatientPrescriptionNew>();
            HospitlLocationID();

            try
            {
                DataSet dsDetails = BL_obj.GetAllAllergies(HospitalID, LocationID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {

                        searchlist.Add(new PatientPrescriptionNew
                        {

                            AllergiesID = Convert.ToInt32(dr["AllergiesID"].ToString()),
                            Allergies = dr["AllergiesName"].ToString(),


                        });
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult BindCaculteAge(string date)
        {
            string aa = "";
            BL_PatientOPDBill db = new BL_PatientOPDBill();
            if (date != "NaN-NaN-NaN")
            {
                aa = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            }
            return new JsonResult { Data = aa, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetPatientName(string prefix)
        {

            DataSet ds = blobj.GetPatientName(prefix);
            List<PatientPrescriptionNew> SearchList = new List<PatientPrescriptionNew>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new PatientPrescriptionNew
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNO = Convert.ToInt32( dr["PatientRegNO"]),
                    PrintRegNO = Convert.ToInt32(dr["PrintRegNO"])
                });
            }

            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetPatientRegNo(int PatientRegNo)
        {
            DataSet ds = blobj.GetPatientRegNo(PatientRegNo);
            List<PatientPrescriptionNew> SearchList = new List<PatientPrescriptionNew>();

            if (ds.Tables[0].Rows.Count > 0)
            {

                objmodel.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                objmodel.PatientRegNO = Convert.ToInt32( ds.Tables[0].Rows[0]["RegNO"]);
                SearchList.Add(objmodel);

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