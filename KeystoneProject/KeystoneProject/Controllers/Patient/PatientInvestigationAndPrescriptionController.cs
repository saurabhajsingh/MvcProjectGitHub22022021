using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
namespace KeystoneProject.Controllers.Patient
{
    public class PatientInvestigationAndPrescriptionController : Controller
    {
        BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
        private SqlConnection con;
        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }

        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        //
        // GET: /PatientInvestigationAndPrescription/
        public ActionResult PatientInvestigationAndPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientInvestigationAndPrescription(FormCollection form, OPrativeNote objOprative, Discharge objDicharge)
        
       
       {
            PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();
            InvestigationForPrescription investigation = new InvestigationForPrescription();
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
          //  HospitlLocationID();

            if (form["old_bill_no"].ToString() == "Select")
            {
                patientDischarge.Mode = "Add";
            }
            else
            {
                patientDischarge.Mode = "Edit";
                objDicharge.PatientIPDDischargeID = Convert.ToInt32( Request.Form["old_bill_no"]);
            }
       

            if (form["ICDCodeID"] != null)
            {
                patientDischarge.ICDCodeID = form["ICDCodeID"].ToString();
                patientDischarge.ICDName = form["ICDName"].ToString();
                patientDischarge.ICDCode = form["ICDCode"].ToString();

             
            }

            string RegNo = form["RegNo"].ToString();
            patientDischarge.CheckupAndPrecriptionID = form["CheckupAndPrecriptionID"].ToString();
            // patientDischarge.CheckupID = form["CheckupIDeathIDD"].ToString();
            patientDischarge.PatientRegNo = Convert.ToInt32(form["RegNo"].ToString());
            patientDischarge.PatientIPDNO = form["patient_ipd_no"].ToString();
            patientDischarge.ConsultantDrID = form["consultant_dr_hid"].ToString();
           // patientDischarge = form["discharge_date"].ToString() + " " + form["discharge_time"].ToString();
            patientDischarge.MedicalHistory = form["medical_history"].ToString();
            patientDischarge.OperativeNotes = form["operative_notes"].ToString();
            patientDischarge.PersonalHistory = form["personal_history"].ToString();
            patientDischarge.NameOfSurgary = form["name_of_surgery"].ToString();
            patientDischarge.Temperature = form["temperature"].ToString();
            patientDischarge.Pulse = form["pulse"].ToString();
            patientDischarge.BP = form["blood_pressure"].ToString();
            patientDischarge.RS = form["rs"].ToString();
            patientDischarge.CVS = form["cvs"].ToString();
            patientDischarge.CNS = form["cns"].ToString();
            //  patientDischarge.PastHistory = form["past_history"].ToString();
            patientDischarge.MentrualHistory = form["mentrual_history"].ToString();
            patientDischarge.Advices = form["advice_discharge"].ToString();
            patientDischarge.ConditionOnDischarge = form["ConditiononDischarge"].ToString();
         //   patientDischarge.ChiefHistoryWithClinicalFindings = form["ChiefHistoryWithClinicalFindings"].ToString();
            patientDischarge.BirthDetails = form["Birth"].ToString();
            patientDischarge.Weight = form["rr"].ToString();
            patientDischarge.HGT = form["spo2"].ToString();
            patientDischarge.Urin = form["pallor"].ToString();
            patientDischarge.Jaundice = form["icterus"].ToString();
            patientDischarge.ECG = form["oedema"].ToString();
            patientDischarge.Allergy = form["abd"].ToString();
            patientDischarge.OtherFinding = form["other_finding"].ToString();
            patientDischarge.TreatmentGiven = form["treatment_given"].ToString();
            patientDischarge.PreciseHistory = form["precise_history"].ToString();
            patientDischarge.OtherInformation = form["ConditiononDischarge"].ToString();
            patientDischarge.HistoryOfDialysis = form["history_dialysis"].ToString();
            patientDischarge.FollowUp = form["follow_up"].ToString();
            if (form["investigation"] ==null)
            {
                patientDischarge.Investigation = "";
            }
            else
            {
                patientDischarge.Investigation = form["investigation"].ToString();
            }
            
            patientDischarge.ChiefComplaint = form["chief_complaint"].ToString();
            patientDischarge.pasthistory = form["past_history"].ToString();
            patientDischarge.HistoryOfChiefComplaint = form["past_history"].ToString();
            patientDischarge.CourseDuringHospitalization = form["course_hospitalization"].ToString();

            patientDischarge.OtherInformation1 = form["OtherInformation1"].ToString();
            patientDischarge.OtherInformation2 = form["OtherInformation2"].ToString();
            patientDischarge.InvestigationAdvice = form["InvestigationAdvice"].ToString();

            patientDischarge.ChiefHistoryWithClinicalFindings = form["ChiefHistoryWithClinicalFindings"].ToString();

            patientDischarge.ChiefHistory = form["ChiefHistory"].ToString();
            // Discharge chk

            // op .Image1 = form["file1"].ToString();
            //patientDischarge.Image2 = form["file"].ToString();
            patientDischarge.ConditionOnDischarge1 = form["condition_admisson1"].ToString();
            patientDischarge.Pulse1 = form["pulse1"].ToString();
            patientDischarge.BP1 = form["blood_pressure1"].ToString();
            patientDischarge.RS1 = form["rs1"].ToString();
            patientDischarge.CVS1 = form["cvs1"].ToString();
            patientDischarge.CNS1 = form["cns1"].ToString();
            patientDischarge.Jaundice1 = form["icterus1"].ToString();
            patientDischarge.ECG1 = form["ECG1"].ToString();
            patientDischarge.Allergy1 = form["Allergy1"].ToString();
            patientDischarge.Urin1 = form["Urin1"].ToString();
            patientDischarge.HGT1 = form["spo2_1"].ToString();
            patientDischarge.OtherFinding1 = form["other_finding1"].ToString();
            patientDischarge.Temperature1 = form["temperature1"].ToString();
            //patientDischarge.ECG1 = form["oedema"].ToString();
            patientDischarge.Weight1 = form["rr1"].ToString();
           // patientDischarge.Urin1 = form["Urin1"].ToString();


           // patientDischarge.CheckupAndPrecriptionID = form["PatientIPDDischargeID"].ToString();
            patientDischarge.PrecriptionID = form["PrecriptionID"].ToString();
          //  patientDischarge.CheckupID = form["CheckupID"].ToString();
           // patientDischarge.CheckServiceID = form["CheckServiceID"].ToString();
            objOprative.Image1 = form["OPDImage1"].ToString();
            objOprative.Image2 = form["OPDImage2"].ToString();

            String TestID;
            String InvestigationName;
            String Remark;
            String LabNo;
            String TestDate;
            String DrugID;
            String AdvicesName;
            String Day;
            String Dose;
            String FastFood;
            String DoseID;
            String oldDose;


            String[] TestIDs;
            String[] InvestigationNames;
            String[] Remarks;
            String[] LabNos;
            String[] TestDates;
            String[] DrugIDs;
            String[] AdvicesNames;
            String[] Days;
            String[] Doses;
            String[] FastFoods;
            String[] DoseIDs;
            String[] oldDoses;



            if (form["serviceID1"] != null)
            {
                string serviceID1 = form["serviceID1"].ToString();
                string Services1 = form["Services1"];
            string serviceRemarks1 = form["serviceRemarks1"];
               
            investigation.ServicesID = serviceID1.Split(',');
            investigation.Services = Services1.Split(',');
            investigation.serviceRemarks = serviceRemarks1.Split(',');
                

            }

            

            if (form["TestID1"] != null)
            {
               // patientDischarge.CheckupID = form["CheckupID"];
                TestID = form["TestID1"].ToString();
                InvestigationName = form["InvestigationName"];
                Remark = form["Remark"];
                LabNo = form["LabNo"];
                TestDate = form["TestDate"];

                TestIDs = TestID.Split(',');
                InvestigationNames = InvestigationName.Split(',');
                Remarks = Remark.Split(',');
                LabNos = LabNo.Split(',');
                TestDates = TestDate.Split(',');
                investigation.TestIDs = TestIDs;
                investigation.InvestigationNames = InvestigationNames;
                investigation.Remarks = Remarks;
                investigation.LabNo = LabNos;
                investigation.TestDates = TestDates;
            }
            // if (form["DrugID"] != "" || form["DrugID"] != null)
            if (form["DrugID1"] != null)
            {
                DrugID = form["DrugID1"];
                AdvicesName = form["AdvicesName"];
                Day = form["Days"];
                Dose = form["Dose1"];
                FastFood = form["FastFood"];
                oldDose = form["oldDoseID"];

                //DrugIDs = DrugID.Split(',');
                //if (DrugIDs[0] != "")
                //{
                //    if (DrugIDs.Length > 0)
                //    {
                DrugIDs = DrugID.Split(',');
                AdvicesNames = AdvicesName.Split(',');
                Days = Day.Split(',');
                Doses = Dose.Split(',');
                FastFoods = FastFood.Split(',');
                oldDoses = oldDose.Split(',');
              
                investigation.DrugIDs = DrugIDs;
                investigation.AdvicesNames = AdvicesNames;
                investigation.Days = Days;
                investigation.Doses = Doses;
                investigation.FastFoods = FastFoods;
                investigation.oldDoseIDs = oldDoses;

                //String[] ICDCodes;
                //string ICDCode;

                //if (form["ICDCode"] != null)
                //{
                //    ICDCode = form["ICDCodes"];
                //    ICDCodes = ICDCode.Split(',');
                //}
                //investigation.oldDoseIDs = oldDoses.Skip(1).ToArray();
                //   }
                //  }
            }
            if (db.SaveData(patientDischarge, investigation, objOprative, objDicharge))
            {
                Session["PatientRegNo"] = Convert.ToInt32(patientDischarge.PatientRegNo);
             //   Session["PatientIPDNO"] = Convert.ToInt32(patientDischarge.PatientIPDNO);
                
                if (objDicharge.chkFinalDischarge == "off" || objDicharge.chkFinalDischarge == null)
                {
                    Session["chkFinalDischarge"] = "False";
                    Session["CheckupAndPrecriptionID"] = Convert.ToInt32(patientDischarge.CheckupAndPrecriptionID);

                }
                else
                {
                    Session["chkFinalDischarge"] = "True";
                    Session["PatientIPDDischargeID"] = objDicharge.PatientIPDDischargeID;
                    Session["CheckupAndPrecriptionID"] = Convert.ToInt32(0);
                    Session["EndResult"] = "";
                    Session["chkDeathSummary"] = false;
                    if (objDicharge.EndResult == "Transfer")
                    {
                        Session["EndResult"] = objDicharge.EndResult;
                    }
                    if (objDicharge.chkDeathSummary == "on")
                    {
                        Session["chkDeathSummary"] = true;
                    }

                }
                return RedirectToAction("RptPatientIPDPrescriptionInvestigation", "PatientInvestigationAndPrescription");
            }
            else
            {
                return RedirectToAction("PatientInvestigationAndPrescription", "PatientInvestigationAndPrescription");
            }
        }

       
        public ActionResult DeletePatientInvestigationAndPrescription(int CheckupAndPrecriptionID, string chkfinalDis)
        {
            string del = null;
            BL_PatientInvestigationAndPrescription obj = new BL_PatientInvestigationAndPrescription();

            if (chkfinalDis == "false")
            {
                if (obj.DeletePatientCheckupAndPrecription(CheckupAndPrecriptionID))
                {
                    del = "Delete";
                    ViewData["Delete"] = "Delete";
                }
            }
            if (chkfinalDis == "true")
            {
                if (obj.DeletePatientIPDDischarge(CheckupAndPrecriptionID))
                {
                    del = "Delete";
                    ViewData["Delete"] = "Delete";
                }
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RptPatientIPDPrescriptionInvestigation()
        {
            return View();
        }

        public ActionResult RptPatientIPDPrescriptionOperativeNotes()
        {
            return View();
        }

        public ActionResult RptBtnOldBillPrint(int investigationID_And_DischargeID, string chkfinalDis, string RegNo)
        {
            Session["PatientRegNo"] = Convert.ToInt32(RegNo);
            if (chkfinalDis == "off" || chkfinalDis == null)
            {
                Session["chkFinalDischarge"] = "False";
                Session["CheckupAndPrecriptionID"] = investigationID_And_DischargeID;

            }
            else
            {
                Session["chkFinalDischarge"] = "True";
                Session["PatientIPDDischargeID"] = investigationID_And_DischargeID;
                Session["CheckupAndPrecriptionID"] = Convert.ToInt32(0);

            }
            return Json(new { data = "Govind" }, JsonRequestBehavior.AllowGet);
           
        }

        public JsonResult GetPatientForPatientCheckupNPreparation(string RegNo,int PrintIPDNo, string FinalDischargechk)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> patientdischargelist = new List<PatientInvestigationAndPrescription_Model>();
            PatientInvestigationAndPrescription_Model OdlBill = new PatientInvestigationAndPrescription_Model();
            List<PatientInvestigationAndPrescription_Model> productList = new List<PatientInvestigationAndPrescription_Model>();
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();

            try
            {
                HospitlLocationID();
                SqlCommand cmd = new SqlCommand();
                if (FinalDischargechk == "false")
                {
                    cmd = new SqlCommand("GetPatientForPatientCheckupNPreparation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    cmd.Parameters.Add(new SqlParameter("@PatientRegNO", RegNo));
                }
                else
                {
                    if (PrintIPDNo > 0 )
                    {
                        cmd = new SqlCommand("GetPatientIPDDischargeAndSummaryWithIPDNo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd.Parameters.Add(new SqlParameter("@PatientIPDNO", PrintIPDNo));

                        RegNo = "0";
                    }
                    else
                    {
                        cmd = new SqlCommand("GetPatientIPDDischargeAndSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd.Parameters.Add(new SqlParameter("@PatientRegNO", RegNo));
                    }
                }
               

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sd.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {

               
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();
                    //patientDischarge.HospitalID = Convert.ToInt32(row["HospitalID"]);
                    //patientDischarge.LocationID = Convert.ToInt32(row["LocationID"]);
                    patientDischarge.PatientRegNo = Convert.ToInt32(row["PatientRegNO"]);
                    patientDischarge.PatientName = Convert.ToString(row["PatientName"]);
                    patientDischarge.GuardianName = Convert.ToString(row["GuardianName"]);
                    patientDischarge.PatientIPDNO = Convert.ToString(row["PrintIPDNO"]);

                    patientDischarge.patientipdno = Convert.ToString(row["PatientIPDNO"]);
                    
                    Session["PatientIPDNO"] = Convert.ToString(row["PatientIPDNO"]);
                   
                    patientDischarge.AddmissionDate = Convert.ToDateTime(row["AddmissionDate"]).ToString("yyyy-MM-dd");
                    patientDischarge.ConsultantDrID = Convert.ToString(row["ConsultantDrID"]);
                    patientDischarge.ConsultantDoctor = Convert.ToString(row["DoctorPrintName"]);
                    patientDischarge.DepartmentID = Convert.ToString(row["DepartmentID"]);
                    patientDischarge.DepartmentName = Convert.ToString(row["DepartmentName"]);
                    patientDischarge.ReferredByDoctorID = Convert.ToString(row["ReferredByDoctorID"]);
                    patientDischarge.ReferredByDoctorName = Convert.ToString(row["RefeeredDoctorPrintName"]);
                    //   patientDischarge.CreationID = Convert.ToInt32(row["CreationID"]);
                    patientDischarge.WardID = Convert.ToString(row["WardID"]);
                    patientDischarge.WardName = Convert.ToString(row["WardName"]);
                    patientDischarge.RoomID = Convert.ToString(row["RoomID"]);
                    patientDischarge.RoomNo = Convert.ToString(row["RoomName"]);
                    patientDischarge.BedID = Convert.ToString(row["BedID"]);
                    patientDischarge.BedNo = Convert.ToString(row["BedNo"]);
                    patientDischarge.Gender = Convert.ToString(row["Gender"]);
                    patientDischarge.MobileNo = Convert.ToString(row["MobileNo"]);
                    patientDischarge.Address = Convert.ToString(row["Address"]);
                    patientDischarge.Age = Convert.ToString(row["Age"]);
                    //   patientDischarge.BedStatus = Convert.ToString(row["BedStatus"]);

                    // patientDischarge.CheckupAndPrecriptionID = Convert.ToString(row["CheckupAndPrecriptionID"]);

                    patientdischargelist.Add(patientDischarge);
                    if (FinalDischargechk == "false")
                    
                    {
                       
                       
                        //if (ds.Tables[1].Rows[0]["OPD/IPDID"].ToString() == PrintIPDNo)
                        if (ds.Tables[1].Rows.Count > 0)
                        { 
                            foreach (DataRow row1 in ds.Tables[1].Rows)

                        {
                            int id = Convert.ToInt32(row1["OPD/IPDID"]);
                          int id1=0;
                          if (PrintIPDNo==0)
                          {
                        id1 = Convert.ToInt32(patientDischarge.PatientIPDNO);
                          }
                          else
                          {
                              id1 = PrintIPDNo;
                          }
                         
                        if (id == id1)
                      
                        {
                            //foreach (DataRow dr in ds.Tables[1].Rows)
                           
                          //id=dr["OPD/IPDID"].ToString();
                                //if (dr["OPD/IPDID"].ToString() = PrintIPDNo)
                                {


                                    patientdischargelist.Add(new PatientInvestigationAndPrescription_Model
                                    {
                                        CheckupAndPrecriptionID = row1["CheckupAndPrecriptionID"].ToString(),
                                        OldCheckupAndPrecription = row1["OldCheckupAndPrecription"].ToString(),
                                    });
                                }
                            

                        }
                    }
                    }
                    }
                    else
                    {
                        //if (db.CheckDischargePatient(HospitalID, LocationID, Convert.ToInt32(RegNo), Convert.ToInt32(row["PatientIPDNO"])))
                        //{
                        if (Convert.ToInt32(Session["PatientIPDNO"]) > 0)
                        {
                            PrintIPDNo = Convert.ToInt32(Session["PatientIPDNO"]);
                            //RegNo = "0";
                        }

                            DataSet dsOldFinalBill = new DataSet();
                            dsOldFinalBill = db.GetPatientOldIPDFInalBill(RegNo, PrintIPDNo);

                            if (dsOldFinalBill.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr in dsOldFinalBill.Tables[0].Rows)
                                {
                                    patientdischargelist.Add(new PatientInvestigationAndPrescription_Model
                                    {
                                        CheckupAndPrecriptionID = dr["PatientIPDDischargeID"].ToString(),
                                        OldCheckupAndPrecription = dr["DischargeIDWithdate"].ToString(),
                                         Message = "IPD Patient Already Discharge.",
                                    });
                                }



                            }

                        //}
                        //else
                        //{
                             
                        //              patientdischargelist.Add(new PatientInvestigationAndPrescription_Model
                        //               {
                                     
                        //                 Message = null
                        //            });
                        //}
                    }


                    break;
                  }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }



            return Json(new { patientdischargelist = patientdischargelist, productList = productList }, JsonRequestBehavior.AllowGet);

        }


        #region Discharge

        public JsonResult   cmbOldFinalBill_SelectedIndexChanged(string PatientRegNO)
        {
            BL_PatientInvestigationAndPrescription Bl = new BL_PatientInvestigationAndPrescription();
           DataSet dsPatientIPD=new DataSet();
           PatientInvestigationAndPrescription_Model PatientCheckupNPreparation = new PatientInvestigationAndPrescription_Model();
           Discharge objDischarge = new Discharge();
           OPrativeNote objOIprativeNote = new OPrativeNote();
           List<Discharge> addDischarge = new List<Discharge>();
           List<OPrativeNote> addOPrativeNote = new List<OPrativeNote>();
           List<PatientInvestigationAndPrescription_Model> addPatientCheckupNPreparation = new List<PatientInvestigationAndPrescription_Model>();
           List<InvestigationForPrescription1> OBJPreparationtable = new List<InvestigationForPrescription1>();

                //DataSet dsPatientIPD = new DataSet();
               // dsPatientIPD.Reset();
               dsPatientIPD = Bl.GetPatientIPDDischargeAndSummary(PatientRegNO.ToString());


               foreach (DataRow dr in dsPatientIPD.Tables[3].Rows)
               {
                   OBJPreparationtable.Add(new InvestigationForPrescription1
                   {
                       DrugIDs = dr["ProductID"].ToString(),
                       AdvicesNames = dr["ProductName"].ToString(),
                       Doses = dr["Doses"].ToString(),
                       DoseIDs = "0",
                       Days = dr["Timing"].ToString(),
                       FastFoods = dr["DrugDescription"].ToString(),
                   });

               }

               foreach (DataRow dr in dsPatientIPD.Tables[8].Rows)
               {
                   addPatientCheckupNPreparation.Add(new PatientInvestigationAndPrescription_Model
                   {

                       ICDCodeID = dr["ICDCodeID"].ToString(),
                       ICDCode = dr["ICDCode"].ToString(),
                       ICDName = dr["ICDName"].ToString()
                   });
               }
               foreach (DataRow dr in dsPatientIPD.Tables[2].Rows)
               {
                   OBJPreparationtable.Add(new InvestigationForPrescription1
                   {
                       InvestigationNames = dr["TestName"].ToString(),

                       TestIDs = dr["TestID"].ToString(),
                       Remarks = dr["Remark"].ToString(),

                   });

               }

                //dsOperative1.Reset();
                //dsOperative1.Tables.Add(dsPatientIPD.Tables[5].Copy());
                if (dsPatientIPD.Tables[0].Rows.Count > 0)
                {
                    #region Fill All Text Boxes
                    if(dsPatientIPD.Tables[0].Rows[0]["PatientIPDDischargeID"].ToString()!="")
                    {
                     objDischarge.PatientIPDDischargeID = Convert.ToInt32(dsPatientIPD.Tables[0].Rows[0]["PatientIPDDischargeID"].ToString());
                    }
                    else
                    {
                        objDischarge.PatientIPDDischargeID =0;
                    }
                    if (dsPatientIPD.Tables[1].Rows.Count > 0)
                    {
                        PatientCheckupNPreparation.PreciseHistory = dsPatientIPD.Tables[1].Rows[0]["PreciseHistory"].ToString();
                        PatientCheckupNPreparation.Pulse = dsPatientIPD.Tables[1].Rows[0]["Pulse"].ToString();
                        PatientCheckupNPreparation.RS = dsPatientIPD.Tables[1].Rows[0]["RS"].ToString();
                        PatientCheckupNPreparation.Temperature = dsPatientIPD.Tables[1].Rows[0]["Temperature"].ToString();
                        PatientCheckupNPreparation.TreatmentGiven = dsPatientIPD.Tables[1].Rows[0]["TreatmentGiven"].ToString();
                        PatientCheckupNPreparation.Weight = dsPatientIPD.Tables[1].Rows[0]["Weight"].ToString();
                    PatientCheckupNPreparation.Urin = dsPatientIPD.Tables[1].Rows[0]["Urin"].ToString();
                    objDischarge.DischargeDate = Convert.ToDateTime(dsPatientIPD.Tables[1].Rows[0]["Dischargedate"]).ToString("yyyy-MM-dd");
                        objDischarge.Dischargetime = Convert.ToDateTime(dsPatientIPD.Tables[1].Rows[0]["Dischargedate"]).ToString("hh:mm");
                        objDischarge.EndResult = dsPatientIPD.Tables[1].Rows[0]["EndResult"].ToString();
                        objDischarge.FinalDiagnosis = dsPatientIPD.Tables[1].Rows[0]["FinalDiagnosis"].ToString();
                        //objDischarge.DischargeDate = dsPatientIPD.Tables[1].Rows[0]["Dischargedate"].ToString();
                        PatientCheckupNPreparation.Advices = dsPatientIPD.Tables[1].Rows[0]["Advices"].ToString();
                        PatientCheckupNPreparation.Allergy = dsPatientIPD.Tables[1].Rows[0]["Allergy"].ToString();
                        PatientCheckupNPreparation.BP = dsPatientIPD.Tables[1].Rows[0]["BP"].ToString();
                        PatientCheckupNPreparation.CNS = dsPatientIPD.Tables[1].Rows[0]["CNS"].ToString();
                        PatientCheckupNPreparation.CVS = dsPatientIPD.Tables[1].Rows[0]["CVS"].ToString();
                        PatientCheckupNPreparation.ECG = dsPatientIPD.Tables[1].Rows[0]["ECG"].ToString();

                        PatientCheckupNPreparation.OtherInformation1 = dsPatientIPD.Tables[1].Rows[0]["OtherInformation1"].ToString();
                        PatientCheckupNPreparation.OtherInformation2 = dsPatientIPD.Tables[1].Rows[0]["OtherInformation2"].ToString();

                        PatientCheckupNPreparation.InvestigationAdvice = dsPatientIPD.Tables[1].Rows[0]["InvestigationAdvice"].ToString();

                        PatientCheckupNPreparation.FinalDiagnosis = dsPatientIPD.Tables[1].Rows[0]["FinalDiagnosis"].ToString();
                        PatientCheckupNPreparation.FollowUp = dsPatientIPD.Tables[1].Rows[0]["FollowUp"].ToString();
                        PatientCheckupNPreparation.Investigation = dsPatientIPD.Tables[1].Rows[0]["InvestigationNew"].ToString();
                        PatientCheckupNPreparation.HGT = dsPatientIPD.Tables[1].Rows[0]["HGT"].ToString();
                        PatientCheckupNPreparation.HistoryOfDialysis = dsPatientIPD.Tables[1].Rows[0]["HistoryOfDialysis"].ToString();
                        PatientCheckupNPreparation.MedicalHistory = dsPatientIPD.Tables[1].Rows[0]["MedicalHistory"].ToString();
                        PatientCheckupNPreparation.NameOfSurgary = dsPatientIPD.Tables[1].Rows[0]["NameOfSurgary"].ToString();
                        PatientCheckupNPreparation.OperativeNotes = dsPatientIPD.Tables[1].Rows[0]["OperativeNotes"].ToString();
                        PatientCheckupNPreparation.OtherInformation = dsPatientIPD.Tables[1].Rows[0]["OtherInformation"].ToString();
                        PatientCheckupNPreparation.PatientIPDNO = dsPatientIPD.Tables[1].Rows[0]["PatientIPDID"].ToString();
                        PatientCheckupNPreparation.PatientRegNo = Convert.ToInt32(dsPatientIPD.Tables[1].Rows[0]["PatientRegNo"]);
                        PatientCheckupNPreparation.PersonalHistory = dsPatientIPD.Tables[1].Rows[0]["PersonalHistory"].ToString();
                        PatientCheckupNPreparation.ChiefComplaint = dsPatientIPD.Tables[1].Rows[0]["ChiefComplaint"].ToString();
                        PatientCheckupNPreparation.ChiefHistoryWithClinicalFindings = dsPatientIPD.Tables[1].Rows[0]["ChiefHistoryClinicalFindings"].ToString();
                        PatientCheckupNPreparation.HistoryOfChiefComplaint = dsPatientIPD.Tables[1].Rows[0]["HistoryOfChiefComplaint"].ToString();
                        PatientCheckupNPreparation.MentrualHistory = dsPatientIPD.Tables[1].Rows[0]["MentrualHistory"].ToString();
                        PatientCheckupNPreparation.CourseDuringHospitalization = dsPatientIPD.Tables[1].Rows[0]["CourseDuringHospitalization"].ToString();
                        PatientCheckupNPreparation.PreciseHistory = dsPatientIPD.Tables[1].Rows[0]["PreciseHistory"].ToString();
                        PatientCheckupNPreparation.Jaundice = dsPatientIPD.Tables[1].Rows[0]["Jaundice"].ToString();
                        PatientCheckupNPreparation.OtherFinding = dsPatientIPD.Tables[1].Rows[0]["OtherFinding"].ToString();
                        PatientCheckupNPreparation.ConditionOnDischarge = dsPatientIPD.Tables[1].Rows[0]["ConditionOnDischarge"].ToString();
                        // Discharge chk
                        PatientCheckupNPreparation.Jaundice1 = dsPatientIPD.Tables[1].Rows[0]["Jaundice1"].ToString();
                        PatientCheckupNPreparation.OtherFinding1 = dsPatientIPD.Tables[1].Rows[0]["OtherFinding1"].ToString();
                        PatientCheckupNPreparation.Allergy1 = dsPatientIPD.Tables[1].Rows[0]["Allergy1"].ToString();
                        PatientCheckupNPreparation.ConditionOnDischarge1 = dsPatientIPD.Tables[1].Rows[0]["ConditionOnDischarge1"].ToString();
                        PatientCheckupNPreparation.Pulse1 = dsPatientIPD.Tables[1].Rows[0]["Pulse1"].ToString();
                        PatientCheckupNPreparation.RS1 = dsPatientIPD.Tables[1].Rows[0]["RS1"].ToString();
                        PatientCheckupNPreparation.Temperature1 = dsPatientIPD.Tables[1].Rows[0]["Temperature1"].ToString();
                        PatientCheckupNPreparation.Urin1 = dsPatientIPD.Tables[1].Rows[0]["Urin1"].ToString();
                        PatientCheckupNPreparation.Weight1 = dsPatientIPD.Tables[1].Rows[0]["Weight"].ToString();
                        PatientCheckupNPreparation.BP1 = dsPatientIPD.Tables[1].Rows[0]["BP1"].ToString();
                        PatientCheckupNPreparation.CNS1 = dsPatientIPD.Tables[1].Rows[0]["CNS1"].ToString();
                        PatientCheckupNPreparation.CVS1 = dsPatientIPD.Tables[1].Rows[0]["CVS1"].ToString();
                        PatientCheckupNPreparation.ECG1 = dsPatientIPD.Tables[1].Rows[0]["ECG1"].ToString();
                        PatientCheckupNPreparation.HGT1 = dsPatientIPD.Tables[1].Rows[0]["HGT1"].ToString();
                    }
                    if (dsPatientIPD.Tables[5].Rows.Count > 0)
                    {
                        objOIprativeNote.Surgery = dsPatientIPD.Tables[5].Rows[0]["SurgeryPerformed"].ToString();
                        objOIprativeNote.Emergency = dsPatientIPD.Tables[5].Rows[0]["EmergencyElective"].ToString();
                        objOIprativeNote.Anaesthesia = dsPatientIPD.Tables[5].Rows[0]["Anaesthesia"].ToString();
                        objOIprativeNote.Anaesthelist = dsPatientIPD.Tables[5].Rows[0]["Anaesthetist"].ToString();
                        objOIprativeNote.Surgeons = dsPatientIPD.Tables[5].Rows[0]["Surgeons"].ToString();
                        objOIprativeNote.Associate = dsPatientIPD.Tables[5].Rows[0]["AssistantAssociateSurgeons"].ToString();
                        objOIprativeNote.OperativeID = Convert.ToInt32(dsPatientIPD.Tables[5].Rows[0]["OperativeID"]);

                        if (dsPatientIPD.Tables[5].Rows[0]["OperationDateandTime"].ToString().Length > 3)
                        {
                            objOIprativeNote.OperationdDate = Convert.ToDateTime(dsPatientIPD.Tables[5].Rows[0]["OperationDateandTime"]).ToString("yyyy-MM-dd");
                            objOIprativeNote.Time = Convert.ToDateTime(objOIprativeNote.OperationdDate).ToString("hh:mm:ss");
                        }

                        //ucPatientIPDDischargeAndSummary1.dtpOperativeDate.Text =dsPatientIPD.Tables[5].Rows[0]["OperationDateandTime"].ToString();


                        // ucPatientIPDDischargeAndSummary1.time.Text = dsPatientIPD.Tables[5].Rows[0]["OperationDateandTime"].ToString();

                        objOIprativeNote.Operation = dsPatientIPD.Tables[5].Rows[0]["OperationNotes"].ToString();
                        objOIprativeNote.Treatment = dsPatientIPD.Tables[5].Rows[0]["TreatMentAdvised"].ToString();
                        objOIprativeNote.Pre_Operative = dsPatientIPD.Tables[5].Rows[0]["PreOperativeDiagnosis"].ToString();
                        objOIprativeNote.Conjunctival = dsPatientIPD.Tables[5].Rows[0]["ConjunctivalFlap"].ToString();
                        objOIprativeNote.Cautery = dsPatientIPD.Tables[5].Rows[0]["Cautery"].ToString();
                        objOIprativeNote.Incision = dsPatientIPD.Tables[5].Rows[0]["Incision"].ToString();
                        objOIprativeNote.Side = dsPatientIPD.Tables[5].Rows[0]["SidePort"].ToString();
                        objOIprativeNote.Size = dsPatientIPD.Tables[5].Rows[0]["Size"].ToString();
                        objOIprativeNote.AC_Maintainer = dsPatientIPD.Tables[5].Rows[0]["ACMaintainer"].ToString();
                        objOIprativeNote.Capsulotomy = dsPatientIPD.Tables[5].Rows[0]["Capsulotomy"].ToString();
                        objOIprativeNote.Hydro = dsPatientIPD.Tables[5].Rows[0]["Hydrodissection"].ToString();

                        objOIprativeNote.Hydro_Dilineation = dsPatientIPD.Tables[5].Rows[0]["HydroDelineation"].ToString();
                        objOIprativeNote.Nucleus = dsPatientIPD.Tables[5].Rows[0]["NucleusRotation"].ToString();
                        objOIprativeNote.Pacho = dsPatientIPD.Tables[5].Rows[0]["PhacoTechnique"].ToString();
                        objOIprativeNote.Posterior = dsPatientIPD.Tables[5].Rows[0]["PosteriorCapsule"].ToString();
                        objOIprativeNote.IOL = dsPatientIPD.Tables[5].Rows[0]["IOL"].ToString();
                        objOIprativeNote.Wound = dsPatientIPD.Tables[5].Rows[0]["WoundClosure"].ToString();
                       // oboprative.a = dsPatientIPD.Tables[5].Rows[0]["AdditionalNotes"].ToString();


                        //ucPatientIPDDischargeAndSummary1.pbBeforeOperative.Image = new Bitmap(ImagePath + "" + dsPatientIPD.Tables[5].Rows[0]["Image1"].ToString());
                        //ucPatientIPDDischargeAndSummary1.PbAfterOperative.Image = new Bitmap(ImagePath + "" + dsPatientIPD.Tables[5].Rows[0]["Image2"].ToString());


                        if (dsPatientIPD.Tables[5].Rows[0]["Image1"].ToString() == "")
                        {
                            objOIprativeNote.Image1 = null;
                        }
                        else
                        {
                            try
                            {
                                objOIprativeNote.Image1 = dsPatientIPD.Tables[5].Rows[0]["Image1"].ToString() ;
                               
                            }
                            catch (Exception ex)
                            {
                            }
                        }


                        if (dsPatientIPD.Tables[5].Rows[0]["Image2"].ToString() == "")
                        {
                            objOIprativeNote.Image2 = null;
                        }
                        else
                        {
                            try
                            {
                                objOIprativeNote.Image2 = dsPatientIPD.Tables[5].Rows[0]["Image2"].ToString();
                             
                            }
                            catch (Exception ex)
                            {
                            }
                        }


                    }

                    //////////////Death Summary /////////////
                  
                    if (dsPatientIPD.Tables[6].Rows.Count > 0)
                    {
                        objDischarge.ExpierdDate = Convert.ToDateTime(dsPatientIPD.Tables[6].Rows[0]["DateOfExpired"]).ToString("yyyy-MM-dd");
                        objDischarge.ExpierdTime = Convert.ToDateTime(dsPatientIPD.Tables[6].Rows[0]["DateOfExpired"]).ToString("hh:mm:ss");

                        objDischarge.Causer = dsPatientIPD.Tables[6].Rows[0]["CauseOfIncident"].ToString();
                        objDischarge.Medico = dsPatientIPD.Tables[6].Rows[0]["MedicoLegalIssue"].ToString();
                        objDischarge.Reason = dsPatientIPD.Tables[6].Rows[0]["Reason"].ToString();
                        objDischarge.TreatmentDuring  = dsPatientIPD.Tables[6].Rows[0]["TreatmentDuringHospitalization"].ToString();
                        objDischarge.Line = dsPatientIPD.Tables[6].Rows[0]["LineOfIVIM"].ToString();
                        objDischarge.OtherInformation = dsPatientIPD.Tables[6].Rows[0]["OtherInformation"].ToString();
                        objDischarge.DeathID = Convert.ToInt32(dsPatientIPD.Tables[6].Rows[0]["DeathID"]);
                        //if (ucPatientIPDDischargeAndSummary1.ChkDeathSummary.Checked == null)
                        //{
                        //    objDischarge.ChkDeathSummary.Checked = Convert.ToBoolean(dsPatientIPD.Tables[6].Rows[0]["DeathStatus"].ToString());
                        //}
                        //else
                        //{
                        //    objDischarge.ChkDeathSummary.Checked = Convert.ToBoolean(dsPatientIPD.Tables[6].Rows[0]["DeathStatus"].ToString());
                        //}


                    }
                    ///////////////////////////////////////////    Transfer Summary     ///////////////////////////////////

                    if (dsPatientIPD.Tables[7].Rows.Count > 0)
                    {

                        objDischarge.ChiefHistory = dsPatientIPD.Tables[7].Rows[0]["ChiefHistoryWithClinicalFindings"].ToString();
                        objDischarge.ChiefComplaint = dsPatientIPD.Tables[7].Rows[0]["ChiefComplaintWithDuration"].ToString();
                        objDischarge.Namesurgery = dsPatientIPD.Tables[7].Rows[0]["PastHistory"].ToString();
                        objDischarge.Diagnosis = dsPatientIPD.Tables[7].Rows[0]["Diagnosis"].ToString();
                        objDischarge.TransferID = Convert.ToInt32( dsPatientIPD.Tables[7].Rows[0]["TransferID"]);
                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                 
                    #endregion

                }
                addPatientCheckupNPreparation.Add(PatientCheckupNPreparation);
                addOPrativeNote.Add(objOIprativeNote);
                addDischarge.Add(objDischarge);

                return Json(new { PatientCheckupNPreparation = PatientCheckupNPreparation, objOIprativeNote = objOIprativeNote, OBJPreparationtable = OBJPreparationtable, objDischarge = objDischarge, addPatientCheckupNPreparation = addPatientCheckupNPreparation }, JsonRequestBehavior.AllowGet);


            
        }


        #endregion


        public JsonResult GetInvestigationName(string Investigation)
            {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            //  return new JsonResult { Data = db.GetInvestigationName(Investigation), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            string nam = db.GetInvestigationName(Investigation)[0].CheckupID.ToString();
            return Json(db.GetInvestigationName(Investigation), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDrugsName(string Drug)
        {

            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();

            return new JsonResult { Data = db.GetDrugsName(Drug), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetICDCodes(string ICDCodes, string SearchICDBy)
        {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();

            return new JsonResult { Data = db.GetICDCodes(ICDCodes, SearchICDBy), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BindInvestigationParameter(string PatientRegNo, string OPDIPDNo)
        {
            DataSet ds = db.GetBindPatientName(PatientRegNo, OPDIPDNo);
            List<PatientInvestigationAndPrescription_Model> searchList = new List<PatientInvestigationAndPrescription_Model>();
            string txtInvestigation = "";
            int count = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //searchList.Add(new PatientInvestigationAndPrescription_Model
                //         {
                //             //txtInvestigation += "\n",
                //             ResultValue+= dr["BillDate"].ToString() + "\n",
                //             TestName = dr["TestName"].ToString() + ":-",
                //             TestName = dr["ResultValue"].ToString(),
                             
                //         });
                //count++;

                txtInvestigation += "\n";
                txtInvestigation += dr["BillDate"].ToString() + "\n";
                txtInvestigation += dr["TestName"].ToString() + ":-";
                txtInvestigation += dr["ResultValue"].ToString();
                count++;
               
            }
            TempData["count"] = count;
            return new JsonResult { Data = txtInvestigation, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }
        
        public JsonResult FillICDCodes(int prefix)
        {
            BL_PatientInvestigationAndPrescription BL_obj = new BL_PatientInvestigationAndPrescription();
            List<PatientInvestigationAndPrescription_Model> lists = new List<PatientInvestigationAndPrescription_Model>();
            HospitlLocationID();
            try
            {
                DataSet dsDetails = BL_obj.FillICDCodes(HospitalID, LocationID, Convert.ToInt32(prefix));

                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsDetails.Tables[0].Rows)
                    {
                        lists.Add(new PatientInvestigationAndPrescription_Model
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

        public JsonResult GetDoses(string Dose)
        {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            return new JsonResult { Data = db.GetDoses(Dose), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult GetPatientDetailByReg(string RegNo)
        public JsonResult GetPatientDetailByRegForDischarge(string RegNo)
        {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            return new JsonResult { Data = db.GetPatientDischargeDetailsRegNo(RegNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientNameValue(string search)
        {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            //return new JsonResult { Data = db.GetHospitals(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = db.GetPatientName(search), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetServices(string search)
        {
          
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            //return new JsonResult { Data = db.GetHospitals(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = db.GetServiceName(search), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetPatientRegNoByName(string PatientName)
        {
            BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
            return Json(db.GetPatientRegNoByName(PatientName), JsonRequestBehavior.AllowGet);
        }





        public JsonResult FillOldBillDetails(int CheckupAndPrecriptionID)
        {
            OPrativeNote objOIprativeNote = new OPrativeNote();
           PatientInvestigationAndPrescription_Model PatientCheckupNPreparation = new PatientInvestigationAndPrescription_Model();
            BL_PatientInvestigationAndPrescription obj_PatientInvestigationAndPrescription_R=new BL_PatientInvestigationAndPrescription();
           List< InvestigationForPrescription1> OBJPreparationtable = new List<InvestigationForPrescription1>();



            DataSet dsCheckup=new DataSet();
             DataSet dsPreparation=new DataSet();
             DataSet dsServices=new DataSet();
             DataSet dsPatientCheckupNPreparation=new DataSet();
             DataSet dsOperative=new DataSet();
            DataSet dsFill = new DataSet();
           
             dsFill = obj_PatientInvestigationAndPrescription_R.GetPatientCheckupAndPrecriptiont(CheckupAndPrecriptionID);
            

          
            dsCheckup.Tables.Add(dsFill.Tables[0].Copy());
            dsPreparation.Tables.Add(dsFill.Tables[1].Copy());


            dsServices.Tables.Add(dsFill.Tables[4].Copy());
            dsPatientCheckupNPreparation.Reset();
            dsPatientCheckupNPreparation.Tables.Add(dsFill.Tables[3].Copy());
          //  dsOperative.Reset();
            dsOperative.Tables.Add(dsFill.Tables[5].Copy());

            foreach(DataRow dr in dsPreparation.Tables[0].Rows)
            {
                OBJPreparationtable.Add(new InvestigationForPrescription1
                {
                    precriptionid = dr["PrecriptionID"].ToString(),
                    DrugIDs = dr["ProductID"].ToString(),
                    AdvicesNames = dr["ProductName"].ToString(),
                    Doses = dr["Doses"].ToString(),
                    DoseIDs = "0",
                    Days = dr["Timing"].ToString(),
                    FastFoods = dr["DrugDescription"].ToString(),
            });
               
            }



            foreach (DataRow dr in dsServices.Tables[0].Rows)
            {
                OBJPreparationtable.Add(new InvestigationForPrescription1
                {
                    CheckServiceID1 = dr["CheckupServiceID"].ToString(),
                    Services = dr["ServiceName"].ToString(),
                    ServicesID = dr["ServiceID"].ToString(),
                    serviceRemarks = dr["Remark"].ToString(),
                    
                });

            }


            foreach (DataRow dr in dsCheckup.Tables[0].Rows)
            {
                OBJPreparationtable.Add(new InvestigationForPrescription1
                {
                    Checkupin = dr["CheckupID"].ToString(),
                    InvestigationNames = dr["TestName"].ToString(),

                    TestIDs = dr["TestID"].ToString(),
                    Remarks = dr["Remark"].ToString(),

                });

            }
            //dsCheckup = obj_PatientInvestigationAndPrescription_R.GetPatientCheckupAndPrecriptiont(CheckupAndPrecriptionID);


           // ucPatientCheckupNPreparation1.dgvChackup.DataSource = dsCheckup.Tables[0];
         //   ucPatientCheckupNPreparation1.dgvPrescription.DataSource = dsPreparation.Tables[0];
        //    ucPatientCheckupNPreparation1.dgvServiceName.DataSource = dsServices.Tables[0];

            if (dsCheckup.Tables[0].Rows.Count > 0)
            {



             
            
            }
          

              
            if (dsServices.Tables[0].Rows.Count > 0)
            {



                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["CheckupAndPrecriptionID"].Visible = false;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["CheckupServiceID"].Visible = false;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["ServiceID"].Visible = false;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["ServiceName"].Visible = true;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["Remark"].Visible = true;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["CreationID"].Visible = false;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["Mode"].Visible = false;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["status"].Visible = false;


                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["ServiceName"].DisplayIndex = 0;
                //ucPatientCheckupNPreparation1.dgvServiceName.Columns["Remark"].DisplayIndex = 1;

            }
            PatientCheckupNPreparation.CheckupAndPrecriptionID = dsPatientCheckupNPreparation.Tables[0].Rows[0]["CheckupAndPrecriptionID"].ToString();
            PatientCheckupNPreparation .InvestDate= dsPatientCheckupNPreparation.Tables[0].Rows[0]["InvestDate"].ToString();
           // PatientCheckupNPreparation.dtpTime.Text = dsPatientCheckupNPreparation.Tables[0].Rows[0]["InvestDate"].ToString();
            PatientCheckupNPreparation.MedicalHistory = dsPatientCheckupNPreparation.Tables[0].Rows[0]["MedicalHistory"].ToString();
            PatientCheckupNPreparation.OperativeNotes = dsPatientCheckupNPreparation.Tables[0].Rows[0]["OperativeNotes"].ToString();
            PatientCheckupNPreparation.OtherInformation1 = dsPatientCheckupNPreparation.Tables[0].Rows[0]["OtherInformation1"].ToString();
            PatientCheckupNPreparation.OtherInformation2 = dsPatientCheckupNPreparation.Tables[0].Rows[0]["OtherInformation2"].ToString();
            PatientCheckupNPreparation.InvestigationAdvice = dsPatientCheckupNPreparation.Tables[0].Rows[0]["InvestigationAdvice"].ToString();

            PatientCheckupNPreparation.PersonalHistory = dsPatientCheckupNPreparation.Tables[0].Rows[0]["PersonalHistory"].ToString();
            PatientCheckupNPreparation.NameOfSurgary = dsPatientCheckupNPreparation.Tables[0].Rows[0]["NameOfSurgary"].ToString();
            PatientCheckupNPreparation.Temperature = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Temperature"].ToString();
            PatientCheckupNPreparation.Pulse = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Pulse"].ToString();
            PatientCheckupNPreparation.BP = dsPatientCheckupNPreparation.Tables[0].Rows[0]["BP"].ToString();
            PatientCheckupNPreparation.Allergy = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Allergy"].ToString();
            PatientCheckupNPreparation.Weight = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Weight"].ToString();
            PatientCheckupNPreparation.RS = dsPatientCheckupNPreparation.Tables[0].Rows[0]["RS"].ToString();
            PatientCheckupNPreparation.CVS = dsPatientCheckupNPreparation.Tables[0].Rows[0]["CVS"].ToString();
            PatientCheckupNPreparation.ECG = dsPatientCheckupNPreparation.Tables[0].Rows[0]["ECG"].ToString();
            PatientCheckupNPreparation.CNS = dsPatientCheckupNPreparation.Tables[0].Rows[0]["CNS"].ToString();
            PatientCheckupNPreparation.HGT = dsPatientCheckupNPreparation.Tables[0].Rows[0]["HGT"].ToString();
            PatientCheckupNPreparation.OtherFinding = dsPatientCheckupNPreparation.Tables[0].Rows[0]["OtherFinding"].ToString();
            PatientCheckupNPreparation.Jaundice = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Jaundice"].ToString();
            PatientCheckupNPreparation.TreatmentGiven = dsPatientCheckupNPreparation.Tables[0].Rows[0]["TreatmentGiven"].ToString();
            PatientCheckupNPreparation.PersonalHistory = dsPatientCheckupNPreparation.Tables[0].Rows[0]["PersonalHistory"].ToString();
            PatientCheckupNPreparation.HistoryOfDialysis = dsPatientCheckupNPreparation.Tables[0].Rows[0]["HistoryOfDialysis"].ToString();
            PatientCheckupNPreparation.Advices = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Advices"].ToString();
            PatientCheckupNPreparation.OtherInformation = dsPatientCheckupNPreparation.Tables[0].Rows[0]["OtherInformation"].ToString();
            PatientCheckupNPreparation.FollowUp = dsPatientCheckupNPreparation.Tables[0].Rows[0]["FollowUp"].ToString();
            PatientCheckupNPreparation.Investigation = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Investigation"].ToString();
            PatientCheckupNPreparation.PreciseHistory = dsPatientCheckupNPreparation.Tables[0].Rows[0]["PreciseHistory"].ToString();
            PatientCheckupNPreparation.BirthDetails = dsPatientCheckupNPreparation.Tables[0].Rows[0]["BirthDetails"].ToString();
            PatientCheckupNPreparation.ChiefHistoryWithClinicalFindings = dsPatientCheckupNPreparation.Tables[0].Rows[0]["ChiefHistoryClinicalFindings"].ToString();
            PatientCheckupNPreparation.ConditionOnDischarge = dsPatientCheckupNPreparation.Tables[0].Rows[0]["ConditionOnDischarge"].ToString();
            PatientCheckupNPreparation.Urin = dsPatientCheckupNPreparation.Tables[0].Rows[0]["Urin"].ToString();
         //  PatientCheckupNPreparation.ChiefHistoryWithClinicalFindings = dsPatientCheckupNPreparation.Tables[0].Rows[0]["ChiefHistoryClinicalFindings"].ToString();
            PatientCheckupNPreparation.ChiefComplaint = dsPatientCheckupNPreparation.Tables[0].Rows[0]["ChiefComplaint"].ToString();
            PatientCheckupNPreparation.HistoryOfChiefComplaint = dsPatientCheckupNPreparation.Tables[0].Rows[0]["HistoryOfChiefComplaint"].ToString();
     //--       PatientCheckupNPreparation.B = dsPatientCheckupNPreparation.Tables[0].Rows[0]["BirthDetails"].ToString();
            PatientCheckupNPreparation.MentrualHistory = dsPatientCheckupNPreparation.Tables[0].Rows[0]["MentrualHistory"].ToString();
            PatientCheckupNPreparation.CourseDuringHospitalization = dsPatientCheckupNPreparation.Tables[0].Rows[0]["CourseDuringHospitalization"].ToString();


            //////////////////////////////////////////////////////////////////OperativeNotes///////////////////////////////////////

            if (dsOperative.Tables[0].Rows.Count > 0)
            {
                objOIprativeNote.OperativeID = Convert.ToInt32( dsOperative.Tables[0].Rows[0]["OperativeID"]);
                objOIprativeNote.Surgeons = dsOperative.Tables[0].Rows[0]["Surgeons"].ToString();
                objOIprativeNote.Treatment = dsOperative.Tables[0].Rows[0]["TreatmentAdvised"].ToString();
                objOIprativeNote.Emergency = dsOperative.Tables[0].Rows[0]["EmergencyElective"].ToString();
                objOIprativeNote.Anaesthesia = dsOperative.Tables[0].Rows[0]["Anaesthesia"].ToString();
                objOIprativeNote.Anaesthelist = dsOperative.Tables[0].Rows[0]["Anaesthetist"].ToString();
                objOIprativeNote.Surgery = dsOperative.Tables[0].Rows[0]["SurgeryPerformed"].ToString();
                objOIprativeNote.Associate = dsOperative.Tables[0].Rows[0]["AssistantAssociateSurgeons"].ToString();

                //  ucPatientCheckupNPreparation1.dtpOperationDate.Text = "";
                if (dsOperative.Tables[0].Rows[0]["OperationDateandTime"].ToString().Length > 3)
                {
                    objOIprativeNote.OperationdDate =Convert.ToDateTime( dsOperative.Tables[0].Rows[0]["OperationDateandTime"]).ToString("yyyy-MM-dd");
                    objOIprativeNote.Time =Convert.ToDateTime(dsOperative.Tables[0].Rows[0]["OperationDateandTime"]).ToString("HH:mm:ss");
                }
                objOIprativeNote.Operation = dsOperative.Tables[0].Rows[0]["OperationNotes"].ToString();
        //-        objOIprativeNote. = dsOperative.Tables[0].Rows[0]["TreatMentAdvised"].ToString();


                objOIprativeNote.Pre_Operative = dsOperative.Tables[0].Rows[0]["PreOperativeDiagnosis"].ToString();
                objOIprativeNote.Conjunctival = dsOperative.Tables[0].Rows[0]["ConjunctivalFlap"].ToString();
               objOIprativeNote.Cautery = dsOperative.Tables[0].Rows[0]["Cautery"].ToString();
                objOIprativeNote.Incision = dsOperative.Tables[0].Rows[0]["Incision"].ToString();
                objOIprativeNote.Side = dsOperative.Tables[0].Rows[0]["SidePort"].ToString();
                objOIprativeNote.Size = dsOperative.Tables[0].Rows[0]["Size"].ToString();
                objOIprativeNote.AC_Maintainer = dsOperative.Tables[0].Rows[0]["ACMaintainer"].ToString();
                objOIprativeNote.Capsulotomy = dsOperative.Tables[0].Rows[0]["Capsulotomy"].ToString();
                objOIprativeNote.Hydro = dsOperative.Tables[0].Rows[0]["Hydrodissection"].ToString();

                objOIprativeNote.Hydro_Dilineation = dsOperative.Tables[0].Rows[0]["HydroDelineation"].ToString();
                objOIprativeNote.Nucleus = dsOperative.Tables[0].Rows[0]["NucleusRotation"].ToString();
                objOIprativeNote.Pacho = dsOperative.Tables[0].Rows[0]["PhacoTechnique"].ToString();
                objOIprativeNote.Posterior = dsOperative.Tables[0].Rows[0]["PosteriorCapsule"].ToString();
                objOIprativeNote.IOL = dsOperative.Tables[0].Rows[0]["IOL"].ToString();
                objOIprativeNote.Wound= dsOperative.Tables[0].Rows[0]["WoundClosure"].ToString();
                objOIprativeNote.Treatment = dsOperative.Tables[0].Rows[0]["TreatMentAdvised"].ToString();


           //--     objOIprativeNote. = dsOperative.Tables[0].Rows[0]["AdditionalNotes"].ToString();

                //ucPatientCheckupNPreparation1.pbBeforeOperative.Image = new Bitmap(ImagePath + "" + dsOperative.Tables[0].Rows[0]["Image1"].ToString());
                //ucPatientCheckupNPreparation1.PbAfterOperative.Image = new Bitmap(ImagePath + "" + dsOperative.Tables[0].Rows[0]["Image2"].ToString());

                if (dsOperative.Tables[0].Rows[0]["Image1"].ToString() == "")
                {
                    objOIprativeNote.Image1 = null;
                }
                else
                {
                    try
                    {
                        objOIprativeNote.Image1 = dsOperative.Tables[0].Rows[0]["Image1"].ToString();
                       
                    }
                    catch (Exception ex)
                    {
                    }
                }


                if (dsOperative.Tables[0].Rows[0]["Image2"].ToString() == "")
                {
                    objOIprativeNote.Image2 = null;
                }
                else
                {
                    try
                    {
                        objOIprativeNote.Image2 = dsOperative.Tables[0].Rows[0]["Image2"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            return Json(new { PatientCheckupNPreparation = PatientCheckupNPreparation, objOIprativeNote = objOIprativeNote, OBJPreparationtable = OBJPreparationtable }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult imageuplod(PatientInvestigationAndPrescription_Model model)
        {
            string path = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file = model.ImageFile;
            if (file != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;

                file.SaveAs(path);
                path = "/MRDFiles/" + file.FileName;
                ViewData["ImagePath"] = "/MRDFiles/" + file.FileName;
                // Session["Paper"] = path;
               // ViewData["ImagePath"] = path;
            }
            return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult imageuplod1(PatientInvestigationAndPrescription_Model model)
        {
            string path1 = "";
            //PatientMRD_Model Model
            // PatientMRD_Model obj = new PatientMRD_Model();
            var file1 = model.ImageFile1;
            if (file1 != null)
            {
                //var path = "";
                var filename = Path.GetFileName(file1.FileName);
                var extention = Path.GetExtension(file1.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file1.FileName);
                //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                path1 = Server.MapPath("~/") + "MRDFiles/" + file1.FileName;

                file1.SaveAs(path1);
                // Session["Paper"] = path;
                path1 = "/MRDFiles/" + file1.FileName;
                ViewData["ImagePath"] = "/MRDFiles/" + file1.FileName;
               // ViewData["ImagePath"] = path;
            }
            return new JsonResult { Data = path1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientForIPDNoBills(string IPDNo)
        {
            BL_PatientInvestigationAndPrescription obj_PatientInvestigationAndPrescription_R = new BL_PatientInvestigationAndPrescription();

            return new JsonResult { Data = obj_PatientInvestigationAndPrescription_R.GetPatientForIPDNoBills(IPDNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult GetPatientIPDDischargeAndSummarForDailyHistory(int CheckupAndPrecriptionID, string chkFinalDischarge, int PatientRegNO, int PatientIPDNO)
                {
            OPrativeNote objOIprativeNote = new OPrativeNote();
            PatientInvestigationAndPrescription_Model PatientCheckupNPreparation = new PatientInvestigationAndPrescription_Model();
    
            List<InvestigationForPrescription1> OBJPreparationtable = new List<InvestigationForPrescription1>();
            Discharge objDischarge = new Discharge();
            List<Discharge> addDischarge = new List<Discharge>();
            DataSet dsPatientIPD = new DataSet();
            DataSet dsCheckup = new DataSet();
            DataSet dsPreparation = new DataSet();
            DataSet dsServices = new DataSet();
            DataSet dsPatientCheckupNPreparation = new DataSet();
            DataSet dsOperative = new DataSet();
            DataSet dsPatintDailyHistory = new DataSet();
            BL_PatientInvestigationAndPrescription obj_PatientInvestigationAndPrescription_R = new BL_PatientInvestigationAndPrescription();

            dsPatintDailyHistory = obj_PatientInvestigationAndPrescription_R.GetPatientIPDDischargeAndSummarForDailyHistory(PatientRegNO, PatientIPDNO);
            dsPatientIPD = obj_PatientInvestigationAndPrescription_R.GetPatientIPDDischargeAndSummary(PatientRegNO.ToString());
            dsCheckup.Tables.Add(dsPatintDailyHistory.Tables[1].Copy());
            dsPreparation.Tables.Add(dsPatintDailyHistory.Tables[2].Copy());


            
            dsPatientCheckupNPreparation.Reset();
            //  dsOperative.Reset();
            dsOperative.Tables.Add(dsPatintDailyHistory.Tables[3].Copy());

            foreach (DataRow dr in dsPreparation.Tables[0].Rows)
            {
                OBJPreparationtable.Add(new InvestigationForPrescription1
                {
                    DrugIDs = dr["ProductID"].ToString(),
                    AdvicesNames = dr["ProductName"].ToString(),
                    Doses = dr["Doses"].ToString(),
                    DoseIDs = "0",
                    Days = dr["Timing"].ToString(),
                    FastFoods = dr["DrugDescription"].ToString(),
                });

            }



         


            foreach (DataRow dr in dsCheckup.Tables[0].Rows)
            {
                OBJPreparationtable.Add(new InvestigationForPrescription1
                {
                    InvestigationNames = dr["TestName"].ToString(),

                    TestIDs = dr["TestID"].ToString(),
                    Remarks = dr["Remark"].ToString(),

                });

            }

            if (dsOperative.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOperative.Tables[0].Rows.Count; i++)
                {
                    //objOIprativeNote.OperativeID = Convert.ToInt32(dsOperative.Tables[0].Rows[0]["OperativeID"])+",";

                    if (dsOperative.Tables[0].Rows[i]["Surgeons"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Surgeons"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Surgeons"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Surgeons = dsOperative.Tables[0].Rows[i]["Surgeons"].ToString() + ",";
                    }
                    if (dsOperative.Tables[0].Rows[i]["EmergencyElective"].ToString() == "" || dsOperative.Tables[0].Rows[i]["EmergencyElective"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["EmergencyElective"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Emergency = dsOperative.Tables[0].Rows[i]["EmergencyElective"].ToString() + ",";
                    }
                    if (dsOperative.Tables[0].Rows[i]["Anaesthesia"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Anaesthesia"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Anaesthesia"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Anaesthesia = dsOperative.Tables[0].Rows[i]["Anaesthesia"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["Anaesthetist"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Anaesthetist"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Anaesthetist"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Anaesthelist = dsOperative.Tables[0].Rows[i]["Anaesthetist"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["SurgeryPerformed"].ToString() == "" || dsOperative.Tables[0].Rows[i]["SurgeryPerformed"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["SurgeryPerformed"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Surgery = dsOperative.Tables[0].Rows[i]["SurgeryPerformed"].ToString() + ",";
                    }
                    if (dsOperative.Tables[0].Rows[i]["AssistantAssociateSurgeons"].ToString() == "" || dsOperative.Tables[0].Rows[i]["AssistantAssociateSurgeons"].ToString() ==null)
                    {
                        dsOperative.Tables[0].Rows[i]["AssistantAssociateSurgeons"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Associate = dsOperative.Tables[0].Rows[i]["AssistantAssociateSurgeons"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["TreatMentAdvised"].ToString() == "" || dsOperative.Tables[0].Rows[i]["TreatMentAdvised"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["TreatMentAdvised"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Treatment = dsOperative.Tables[0].Rows[i]["TreatMentAdvised"].ToString() + ",";    
                    }
                    
                    

                    //  ucPatientCheckupNPreparation1.dtpOperationDate.Text = "";
                    if (dsOperative.Tables[0].Rows[0]["OperationDateandTime"].ToString().Length > 3)
                    {
                       
                        objOIprativeNote.OperationdDate = Convert.ToDateTime(dsOperative.Tables[0].Rows[i]["OperationDateandTime"]).ToString("yyyy-MM-dd");
                        objOIprativeNote.Time = Convert.ToDateTime(dsOperative.Tables[0].Rows[i]["OperationDateandTime"]).ToString("HH:mm:ss");
                    }

                    if (dsOperative.Tables[0].Rows[i]["OperationNotes"].ToString() == "" || dsOperative.Tables[0].Rows[i]["OperationNotes"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["OperationNotes"].ToString(); 
                    }
                    else
                    {
                        objOIprativeNote.Operation = dsOperative.Tables[0].Rows[i]["OperationNotes"].ToString() + ",";
                    }
                    
                    //-        objOIprativeNote. = dsOperative.Tables[0].Rows[0]["TreatMentAdvised"].ToString();

                    if (dsOperative.Tables[0].Rows[i]["PreOperativeDiagnosis"].ToString() == "" || dsOperative.Tables[0].Rows[i]["PreOperativeDiagnosis"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["PreOperativeDiagnosis"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Pre_Operative = dsOperative.Tables[0].Rows[i]["PreOperativeDiagnosis"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["ConjunctivalFlap"].ToString() == "" || dsOperative.Tables[0].Rows[i]["ConjunctivalFlap"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["ConjunctivalFlap"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Conjunctival = dsOperative.Tables[0].Rows[i]["ConjunctivalFlap"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[0]["Cautery"].ToString() == "" || dsOperative.Tables[0].Rows[0]["Cautery"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[0]["Cautery"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Cautery = dsOperative.Tables[0].Rows[0]["Cautery"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["Incision"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Incision"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Incision"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Incision = dsOperative.Tables[0].Rows[i]["Incision"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["SidePort"].ToString() == "" || dsOperative.Tables[0].Rows[i]["SidePort"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["SidePort"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Side = dsOperative.Tables[0].Rows[i]["SidePort"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["Size"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Size"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Size"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Size = dsOperative.Tables[0].Rows[i]["Size"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Capsulotomy = dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString() + ",";
                    }
                   // objOIprativeNote.Capsulotomy += dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString() + ",";

                    if (dsOperative.Tables[0].Rows[i]["ACMaintainer"].ToString() == "" || dsOperative.Tables[0].Rows[i]["ACMaintainer"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["ACMaintainer"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.AC_Maintainer = dsOperative.Tables[0].Rows[i]["ACMaintainer"].ToString() + ",";
                    }
                   
                   // objOIprativeNote.Capsulotomy += dsOperative.Tables[0].Rows[i]["Capsulotomy"].ToString() + ",";

                    if (dsOperative.Tables[0].Rows[i]["Hydrodissection"].ToString() == "" || dsOperative.Tables[0].Rows[i]["Hydrodissection"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["Hydrodissection"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Hydro = dsOperative.Tables[0].Rows[i]["Hydrodissection"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["HydroDelineation"].ToString() == "" || dsOperative.Tables[0].Rows[i]["HydroDelineation"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["HydroDelineation"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Hydro_Dilineation = dsOperative.Tables[0].Rows[i]["HydroDelineation"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["NucleusRotation"].ToString() == "" || dsOperative.Tables[0].Rows[i]["NucleusRotation"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["NucleusRotation"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Nucleus = dsOperative.Tables[0].Rows[i]["NucleusRotation"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["PhacoTechnique"].ToString() == "" || dsOperative.Tables[0].Rows[i]["PhacoTechnique"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["PhacoTechnique"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Pacho = dsOperative.Tables[0].Rows[i]["PhacoTechnique"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["PosteriorCapsule"].ToString() == "" || dsOperative.Tables[0].Rows[i]["PosteriorCapsule"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["PosteriorCapsule"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Posterior = dsOperative.Tables[0].Rows[i]["PosteriorCapsule"].ToString() + ",";
                    }

                    if (dsOperative.Tables[0].Rows[i]["IOL"].ToString() == "" || dsOperative.Tables[0].Rows[i]["IOL"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["IOL"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.IOL = dsOperative.Tables[0].Rows[i]["IOL"].ToString() + ",";
                    }
                    if (dsOperative.Tables[0].Rows[i]["WoundClosure"].ToString() == "" || dsOperative.Tables[0].Rows[i]["WoundClosure"].ToString() == null)
                    {
                        dsOperative.Tables[0].Rows[i]["WoundClosure"].ToString();
                    }
                    else
                    {
                        objOIprativeNote.Wound = dsOperative.Tables[0].Rows[i]["WoundClosure"].ToString() + ",";
                    }
                   
                }
                int j = 0;
                foreach (DataRow dr in dsPatintDailyHistory.Tables[0].Rows)
                {
                    //PatientCheckupNPreparation.CheckupAndPrecriptionID = dsPatintDailyHistory.Tables[0].Rows[j]["CheckupAndPrecriptionID"].ToString();
                    //PatientCheckupNPreparation.InvestDate = dsPatintDailyHistory.Tables[0].Rows[j]["InvestDate"].ToString() + ","; ;
                    // PatientCheckupNPreparation.dtpTime.Text = dsPatientCheckupNPreparation.Tables[0].Rows[0]["InvestDate"].ToString();
                    PatientCheckupNPreparation.MedicalHistory += dsPatintDailyHistory.Tables[0].Rows[j]["MedicalHistory"].ToString() + ",";
                    PatientCheckupNPreparation.OperativeNotes += dsPatintDailyHistory.Tables[0].Rows[j]["OperativeNotes"].ToString() + ",";
                    PatientCheckupNPreparation.OtherInformation1 += dsPatintDailyHistory.Tables[0].Rows[j]["OtherInformation1"].ToString() + ",";
                    PatientCheckupNPreparation.OtherInformation2 += dsPatintDailyHistory.Tables[0].Rows[j]["OtherInformation2"].ToString() + ",";
                    PatientCheckupNPreparation.InvestigationAdvice += dsPatintDailyHistory.Tables[0].Rows[j]["InvestigationAdvice"].ToString() + ",";
                    PatientCheckupNPreparation.PersonalHistory += dsPatintDailyHistory.Tables[0].Rows[j]["PersonalHistory"].ToString() + ",";
                    PatientCheckupNPreparation.NameOfSurgary += dsPatintDailyHistory.Tables[0].Rows[j]["NameOfSurgary"].ToString() + ",";
                    PatientCheckupNPreparation.Temperature += dsPatintDailyHistory.Tables[0].Rows[j]["Temperature"].ToString() + ",";
                    PatientCheckupNPreparation.Pulse += dsPatintDailyHistory.Tables[0].Rows[j]["Pulse"].ToString() + ",";
                    PatientCheckupNPreparation.BP += dsPatintDailyHistory.Tables[0].Rows[j]["BP"].ToString() + ",";
                    PatientCheckupNPreparation.Allergy += dsPatintDailyHistory.Tables[0].Rows[j]["Allergy"].ToString() + ",";
                    PatientCheckupNPreparation.Weight += dsPatintDailyHistory.Tables[0].Rows[j]["Weight"].ToString() + ",";
                    PatientCheckupNPreparation.RS += dsPatintDailyHistory.Tables[0].Rows[j]["RS"].ToString() + ",";
                    PatientCheckupNPreparation.CVS += dsPatintDailyHistory.Tables[0].Rows[j]["CVS"].ToString() + ",";
                    PatientCheckupNPreparation.ECG += dsPatintDailyHistory.Tables[0].Rows[j]["ECG"].ToString() + ",";
                    PatientCheckupNPreparation.CNS += dsPatintDailyHistory.Tables[0].Rows[j]["CNS"].ToString() + ",";
                    PatientCheckupNPreparation.HGT += dsPatintDailyHistory.Tables[0].Rows[j]["HGT"].ToString() + ",";
                    PatientCheckupNPreparation.OtherFinding += dsPatintDailyHistory.Tables[0].Rows[j]["OtherFinding"].ToString() + ",";
                    PatientCheckupNPreparation.Jaundice += dsPatintDailyHistory.Tables[0].Rows[j]["Jaundice"].ToString() + ",";
                    PatientCheckupNPreparation.TreatmentGiven += dsPatintDailyHistory.Tables[0].Rows[j]["TreatmentGiven"].ToString() + ",";
                  //  PatientCheckupNPreparation.PersonalHistory += dsPatintDailyHistory.Tables[0].Rows[j]["PersonalHistory"].ToString() + ",";
                    PatientCheckupNPreparation.HistoryOfDialysis += dsPatintDailyHistory.Tables[0].Rows[j]["HistoryOfDialysis"].ToString() + ",";
                    PatientCheckupNPreparation.Advices += dsPatintDailyHistory.Tables[0].Rows[j]["Advices"].ToString() + ",";
                    PatientCheckupNPreparation.OtherInformation += dsPatintDailyHistory.Tables[0].Rows[j]["OtherInformation"].ToString() + ",";
                    PatientCheckupNPreparation.FollowUp += dsPatintDailyHistory.Tables[0].Rows[j]["FollowUp"].ToString() + ",";
                    PatientCheckupNPreparation.Investigation += dsPatintDailyHistory.Tables[0].Rows[j]["Investigation"].ToString() + ",";
                    PatientCheckupNPreparation.PreciseHistory += dsPatintDailyHistory.Tables[0].Rows[j]["PreciseHistory"].ToString() + ",";
                    PatientCheckupNPreparation.ConditionOnDischarge += dsPatintDailyHistory.Tables[0].Rows[j]["ConditionOnDischarge"].ToString() + ",";
                    PatientCheckupNPreparation.Urin += dsPatintDailyHistory.Tables[0].Rows[j]["Urin"].ToString() + ",";
                    PatientCheckupNPreparation.ChiefHistoryWithClinicalFindings = dsPatintDailyHistory.Tables[0].Rows[0]["ChiefHistoryClinicalFindings"].ToString()+",";
                    PatientCheckupNPreparation.ChiefComplaint += dsPatintDailyHistory.Tables[0].Rows[j]["ChiefComplaint"].ToString() + ",";
                    PatientCheckupNPreparation.HistoryOfChiefComplaint += dsPatintDailyHistory.Tables[0].Rows[j]["HistoryOfChiefComplaint"].ToString() + ",";
                    PatientCheckupNPreparation.BirthDetails = dsPatintDailyHistory.Tables[0].Rows[0]["BirthDetails"].ToString()+",";
                    PatientCheckupNPreparation.MentrualHistory += dsPatintDailyHistory.Tables[0].Rows[j]["MentrualHistory"].ToString() + ",";
                    PatientCheckupNPreparation.CourseDuringHospitalization += dsPatintDailyHistory.Tables[0].Rows[j]["CourseDuringHospitalization"].ToString() + ",";

                    j++;
                }





            }

            if (dsPatientIPD.Tables[1].Rows.Count > 0)
            {

                objDischarge.DischargeDate = Convert.ToDateTime(dsPatientIPD.Tables[1].Rows[0]["Dischargedate"]).ToString("yyyy-MM-dd");
                objDischarge.Dischargetime = Convert.ToDateTime(dsPatientIPD.Tables[1].Rows[0]["Dischargedate"]).ToString("hh:mm:ss");
                objDischarge.EndResult = dsPatientIPD.Tables[1].Rows[0]["EndResult"].ToString();
                objDischarge.FinalDiagnosis = dsPatientIPD.Tables[1].Rows[0]["FinalDiagnosis"].ToString();
                // Discharge chk
                PatientCheckupNPreparation.Jaundice1 = dsPatientIPD.Tables[1].Rows[0]["Jaundice1"].ToString();
                PatientCheckupNPreparation.OtherFinding1 = dsPatientIPD.Tables[1].Rows[0]["OtherFinding1"].ToString();
                PatientCheckupNPreparation.Allergy1 = dsPatientIPD.Tables[1].Rows[0]["Allergy1"].ToString();
                PatientCheckupNPreparation.ConditionOnDischarge1 = dsPatientIPD.Tables[1].Rows[0]["ConditionOnDischarge1"].ToString();
                PatientCheckupNPreparation.Pulse1 = dsPatientIPD.Tables[1].Rows[0]["Pulse1"].ToString();
                PatientCheckupNPreparation.RS1 = dsPatientIPD.Tables[1].Rows[0]["RS1"].ToString();
                PatientCheckupNPreparation.Temperature1 = dsPatientIPD.Tables[1].Rows[0]["Temperature1"].ToString();
                PatientCheckupNPreparation.Urin1 = dsPatientIPD.Tables[1].Rows[0]["Urin1"].ToString();
                PatientCheckupNPreparation.Weight1 = dsPatientIPD.Tables[1].Rows[0]["Weight"].ToString();
                PatientCheckupNPreparation.BP1 = dsPatientIPD.Tables[1].Rows[0]["BP1"].ToString();
                PatientCheckupNPreparation.CNS1 = dsPatientIPD.Tables[1].Rows[0]["CNS1"].ToString();
                PatientCheckupNPreparation.CVS1 = dsPatientIPD.Tables[1].Rows[0]["CVS1"].ToString();
                PatientCheckupNPreparation.ECG1 = dsPatientIPD.Tables[1].Rows[0]["ECG1"].ToString();
                PatientCheckupNPreparation.HGT1 = dsPatientIPD.Tables[1].Rows[0]["HGT1"].ToString();
            }

            addDischarge.Add(objDischarge);

            return Json(new { PatientCheckupNPreparation = PatientCheckupNPreparation, objOIprativeNote = objOIprativeNote, OBJPreparationtable = OBJPreparationtable, objDischarge = objDischarge }, JsonRequestBehavior.AllowGet);
      
        }

        public JsonResult GetEndResult(string prefix)
        {
            BL_PatientInvestigationAndPrescription Investication = new BL_PatientInvestigationAndPrescription();


            return new JsonResult { Data = Investication.GetEndResult(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //public JsonResult GetPatientRegNoByName(string PatientName)
        //{
        //    BL_PatientInvestigationAndPrescription db = new BL_PatientInvestigationAndPrescription();
        //    return Json(db.GetPatientRegNoByName(PatientName), JsonRequestBehavior.AllowGet);
        //}



	}
}