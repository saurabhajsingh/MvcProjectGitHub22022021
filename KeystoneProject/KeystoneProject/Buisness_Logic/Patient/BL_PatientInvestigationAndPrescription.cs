using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientInvestigationAndPrescription
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void connection()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public List<OPDBill> GetPatientForIPDNoBills(string IPDNo)
        {
            if (IPDNo == "")
            {
                IPDNo = "0";
            }
            connection();
            List<OPDBill> patientnamelist = new List<OPDBill>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[0].Value = IPDNo;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForIPDNoBills", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    patientnamelist.Add(
                        new OPDBill
                        {

                            PatientRegNO = Convert.ToInt32(dr["PatientRegNO"])

                        });

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return patientnamelist;
        }
        public List<string> GetPatientDischargeDetailsRegNo(string RegNo)
        {
            connection();
            List<string> regnolist = new List<string>();
            SqlCommand cmd = new SqlCommand("select PatientRegNO from Patient where PatientRegNO LIKE '%" + RegNo + "%'", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                regnolist.Add(Convert.ToString(dr["PatientRegNO"]));
            }
            return regnolist;
        }


        public bool SaveData(PatientInvestigationAndPrescription_Model PatientInvestigationAndPrescription, InvestigationForPrescription investigation, OPrativeNote objOprative,Discharge objDischarge)
        {
            int intResult = 0;
            bool flag = false;
            string Mode = "";
            connection();
            con.Open();
            PatientInvestigationAndPrescription.HospitalID = HospitalID;
            PatientInvestigationAndPrescription.LocationID = LocationID;
            try
            {
                #region IUPatientCheckupAndPrecription
                if (objDischarge.chkFinalDischarge == "off" || objDischarge.chkFinalDischarge == null)
                {
                    SqlCommand cmd1 = new SqlCommand("IUPatientCheckupAndPrecription", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                    cmd1.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                    if (PatientInvestigationAndPrescription.CheckupAndPrecriptionID == null || PatientInvestigationAndPrescription.CheckupAndPrecriptionID == "")
                    {
                        cmd1.Parameters.AddWithValue("@CheckupAndPrecriptionID", 0);
                        cmd1.Parameters["@CheckupAndPrecriptionID"].Direction = ParameterDirection.Output;
                        Mode = "Add";
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);
                        Mode = "Edit";
                    }
                    cmd1.Parameters.AddWithValue("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo);
                    cmd1.Parameters.AddWithValue("@OPDIPDID", PatientInvestigationAndPrescription.PatientIPDNO);
                    cmd1.Parameters.AddWithValue("@PatientType", "IPD");
                    cmd1.Parameters.AddWithValue("@InvestDate", System.DateTime.Now);
                    cmd1.Parameters.AddWithValue("@MedicalHistory", PatientInvestigationAndPrescription.MedicalHistory);
                    cmd1.Parameters.AddWithValue("@OperativeNotes", PatientInvestigationAndPrescription.OperativeNotes);

                    cmd1.Parameters.AddWithValue("@OtherInformation1", PatientInvestigationAndPrescription.OtherInformation1);
                    cmd1.Parameters.AddWithValue("@OtherInformation2", PatientInvestigationAndPrescription.OtherInformation2);
                    cmd1.Parameters.AddWithValue("@InvestigationAdvice", PatientInvestigationAndPrescription.InvestigationAdvice);
                    
                    cmd1.Parameters.AddWithValue("@PersonalHistory", PatientInvestigationAndPrescription.PersonalHistory);
                    cmd1.Parameters.AddWithValue("@NameOfSurgary", PatientInvestigationAndPrescription.NameOfSurgary);
                    cmd1.Parameters.AddWithValue("@Temperature", PatientInvestigationAndPrescription.Temperature);
                    //cmd1.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Pallor);
                    cmd1.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Urin);
                    cmd1.Parameters.AddWithValue("@Pulse", PatientInvestigationAndPrescription.Pulse);
                    cmd1.Parameters.AddWithValue("@BP", PatientInvestigationAndPrescription.BP);
                    cmd1.Parameters.AddWithValue("@BirthDetails", PatientInvestigationAndPrescription.BirthDetails);
                    cmd1.Parameters.AddWithValue("@ChiefHistoryClinicalFindings", PatientInvestigationAndPrescription.ChiefHistoryWithClinicalFindings);
                    cmd1.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.Allergy);
                    cmd1.Parameters.AddWithValue("@Weight", PatientInvestigationAndPrescription.Weight);
                    cmd1.Parameters.AddWithValue("@RS", PatientInvestigationAndPrescription.RS);
                    cmd1.Parameters.AddWithValue("@CVS", PatientInvestigationAndPrescription.CVS);
                    cmd1.Parameters.AddWithValue("@ECG", PatientInvestigationAndPrescription.ECG);
                    cmd1.Parameters.AddWithValue("@CNS", PatientInvestigationAndPrescription.CNS);
                    cmd1.Parameters.AddWithValue("@HGT", PatientInvestigationAndPrescription.HGT);
                    cmd1.Parameters.AddWithValue("@OtherFinding", PatientInvestigationAndPrescription.OtherFinding);
                    cmd1.Parameters.AddWithValue("@Jaundice", PatientInvestigationAndPrescription.Jaundice);
                    cmd1.Parameters.AddWithValue("@TreatmentGiven", PatientInvestigationAndPrescription.TreatmentGiven);
                    cmd1.Parameters.AddWithValue("@PreciseHistory", PatientInvestigationAndPrescription.PreciseHistory);
                    cmd1.Parameters.AddWithValue("@ConditionOnDischarge", PatientInvestigationAndPrescription.ConditionOnDischarge);
                    cmd1.Parameters.AddWithValue("@HistoryOfDialysis", PatientInvestigationAndPrescription.HistoryOfDialysis);
                    cmd1.Parameters.AddWithValue("@Advices", PatientInvestigationAndPrescription.Advices);
                    cmd1.Parameters.AddWithValue("@OtherInformation", PatientInvestigationAndPrescription.OtherInformation);
                    cmd1.Parameters.AddWithValue("@FollowUp", PatientInvestigationAndPrescription.FollowUp);
                    cmd1.Parameters.AddWithValue("@Investigation", PatientInvestigationAndPrescription.Investigation);
                    cmd1.Parameters.AddWithValue("@ChiefComplaint", PatientInvestigationAndPrescription.ChiefComplaint);
                    cmd1.Parameters.AddWithValue("@HistoryOfChiefComplaint", PatientInvestigationAndPrescription.HistoryOfChiefComplaint);
                    cmd1.Parameters.AddWithValue("@CourseDuringHospitalization", PatientInvestigationAndPrescription.CourseDuringHospitalization);
                    cmd1.Parameters.AddWithValue("@MentrualHistory", PatientInvestigationAndPrescription.MentrualHistory);
                    cmd1.Parameters.AddWithValue("@CreationID", UserID);
                    cmd1.Parameters.AddWithValue("@Mode", Mode);
                    cmd1.Parameters.AddWithValue("@ReferenceCode", "");

                   // con.Open();
                    intResult = cmd1.ExecuteNonQuery();
                    PatientInvestigationAndPrescription.PatientIPDNO = Convert.ToString(cmd1.Parameters["@OPDIPDID"].Value);
                    PatientInvestigationAndPrescription.CheckupAndPrecriptionID = Convert.ToString(cmd1.Parameters["@CheckupAndPrecriptionID"].Value);



                    if (intResult > 0)
                    {



                        //---- PatientCheckupAndPrecriptionOperativeNotes




                        SqlCommand cmdOperativeNotes = new SqlCommand("IUPatientCheckupAndPrecriptionOperativeNotes", con);
                        cmdOperativeNotes.CommandType = CommandType.StoredProcedure;
                        cmdOperativeNotes.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                        cmdOperativeNotes.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                        if (objOprative.OperativeID == null || objOprative.OperativeID == 0)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperativeID", 0);
                            cmdOperativeNotes.Parameters["@OperativeID"].Direction = ParameterDirection.Output;
                            Mode = "Add";
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperativeID", objOprative.OperativeID);
                            Mode = "Edit";
                        }

                        cmdOperativeNotes.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);
                        cmdOperativeNotes.Parameters.AddWithValue("@SurgeryPerformed", objOprative.Surgery);
                        cmdOperativeNotes.Parameters.AddWithValue("@EmergencyElective", "IPD");
                        if (objOprative.Anaesthesia == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Anaesthesia", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Anaesthesia", objOprative.Anaesthesia);
                        }

                        if (objOprative.Anaesthelist == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Anaesthetist", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Anaesthetist", objOprative.Anaesthelist);
                        }
                        if (objOprative.Surgeons == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Surgeons", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Surgeons", objOprative.Surgeons);
                        }
                        if (objOprative.Associate == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@AssistantAssociateSurgeons", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@AssistantAssociateSurgeons", objOprative.Associate);
                        }
                        if (objOprative.OperationdDate == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperationDateandTime", DateTime.Now);
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperationDateandTime", objOprative.OperationdDate + " " + objOprative.Time);
                        }
                        if (objOprative.Operation == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperationNotes", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@OperationNotes", objOprative.Operation);
                        }

                        if (objOprative.Treatment == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@TreatMentAdvised", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@TreatMentAdvised", objOprative.Treatment);
                        }

                        if (objOprative.Pre_Operative == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PreOperativeDiagnosis", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PreOperativeDiagnosis", objOprative.Pre_Operative);
                        }

                        //cmd1.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Pallor);
                        if (objOprative.Conjunctival == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@ConjunctivalFlap", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@ConjunctivalFlap", objOprative.Conjunctival);
                        }

                        if (objOprative.Cautery == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Cautery", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Cautery", objOprative.Cautery);
                        }
                        //cmd1.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.ABD);
                        if (objOprative.Incision == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Incision", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Incision", objOprative.Incision);
                        }

                        if (objOprative.Side == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@SidePort", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@SidePort", objOprative.Side);
                        }
                        if (objOprative.Size == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Size", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Size", objOprative.Size);
                        }

                        if (objOprative.AC_Maintainer == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@ACMaintainer", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@ACMaintainer", objOprative.AC_Maintainer);
                        }

                        if (objOprative.Capsulotomy == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Capsulotomy", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Capsulotomy", objOprative.Capsulotomy);
                        }
                        if (objOprative.Hydro == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Hydrodissection", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@Hydrodissection", objOprative.Hydro);
                        }
                        if (objOprative.Hydro_Dilineation == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@HydroDelineation", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@HydroDelineation", objOprative.Hydro_Dilineation);
                        }
                        if (objOprative.Nucleus == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@NucleusRotation", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@NucleusRotation", objOprative.Nucleus);
                        }
                        if (objOprative.Pacho == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PhacoTechnique", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PhacoTechnique", objOprative.Pacho);
                        }
                        if (objOprative.Posterior == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PosteriorCapsule", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@PosteriorCapsule", objOprative.Posterior);
                        }

                        if (objOprative.IOL == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@IOL", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@IOL", objOprative.IOL);
                        }

                        if (objOprative.Wound == null)
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@WoundClosure", "");
                        }
                        else
                        {
                            cmdOperativeNotes.Parameters.AddWithValue("@WoundClosure", objOprative.Wound);
                        }

                        cmdOperativeNotes.Parameters.AddWithValue("@AdditionalNotes", "");

                        cmdOperativeNotes.Parameters.AddWithValue("@Image1", objOprative.Image1);
                        cmdOperativeNotes.Parameters.AddWithValue("@Image2",objOprative.Image2);
                        cmdOperativeNotes.Parameters.AddWithValue("@CreationID", UserID);
                        cmdOperativeNotes.Parameters.AddWithValue("@Mode", Mode);
                        //  cmdOperativeNotes.Parameters.AddWithValue("@ReferenceCode", "");


                        intResult = cmdOperativeNotes.ExecuteNonQuery();

                        intResult = 0;

                        if (investigation.TestIDs != null)
                        {
                            for (int i = 0; i < investigation.TestIDs.Length; i++)
                            {
                                SqlCommand cmd2 = new SqlCommand("IUPatientCheckupAndPrecriptionInvestigation", con);
                                cmd2.CommandType = CommandType.StoredProcedure;

                                cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd2.Parameters.AddWithValue("@LocationID", LocationID);

                                cmd2.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);
                                if (PatientInvestigationAndPrescription.CheckupID == null || PatientInvestigationAndPrescription.CheckupID =="")
                                {
                                    cmd2.Parameters.AddWithValue("@CheckupID", 0);
                                    Mode = "Add";
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@CheckupID", PatientInvestigationAndPrescription.CheckupID);
                                    Mode = "Edit";
                                }
                                cmd2.Parameters.AddWithValue("@OPDIPDID", PatientInvestigationAndPrescription.PatientIPDNO);
                                cmd2.Parameters.AddWithValue("@TestID", Convert.ToInt32(investigation.TestIDs[i]));
                                cmd2.Parameters.AddWithValue("@TestID1", Convert.ToInt32(investigation.TestIDs[i]));
                                cmd2.Parameters.AddWithValue("@TestName", investigation.InvestigationNames[i].ToString());
                                cmd2.Parameters.AddWithValue("@LabNo", Convert.ToInt32(investigation.LabNo[i]));
                                cmd2.Parameters.AddWithValue("@Status", 0);
                                cmd2.Parameters.AddWithValue("@InvestDate", investigation.TestDates[i].ToString());
                                cmd2.Parameters.AddWithValue("@Result2", investigation.Remarks[i].ToString());
                                cmd2.Parameters.AddWithValue("@Remark", investigation.Remarks[i].ToString());
                                cmd2.Parameters.AddWithValue("@CreationID", UserID);
                                cmd2.Parameters.AddWithValue("@Mode", Mode);

                                intResult = cmd2.ExecuteNonQuery();

                                //if (intResult > 0)
                                //{
                                //    flag = true;
                                //}
                                //else
                                //{
                                //    flag = false;
                                //}
                            }
                        }

                        if (investigation.DrugIDs != null)
                        {
                            //   con.Open();
                            for (int i = 0; i < investigation.DrugIDs.Length; i++)
                            {
                                intResult = 0;

                                SqlCommand cmd3 = new SqlCommand("IUPatientCheckupAndPrecriptionPrecription", con);
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd3.Parameters.AddWithValue("@LocationID",LocationID);


                                cmd3.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);

                                if (PatientInvestigationAndPrescription.PrecriptionID == "" || PatientInvestigationAndPrescription.PrecriptionID == null)
                                {
                                    cmd3.Parameters.AddWithValue("@PrecriptionID", 0);
                                    cmd3.Parameters["@PrecriptionID"].Direction = ParameterDirection.Output;
                                    Mode = "Add";
                                }
                                else
                                {
                                    cmd3.Parameters.AddWithValue("@PrecriptionID", PatientInvestigationAndPrescription.PrecriptionID);
                                    Mode = "Edit";
                                }
                                cmd3.Parameters.AddWithValue("@OPDIPDID", PatientInvestigationAndPrescription.PatientIPDNO);
                                cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(investigation.DrugIDs[i]));
                                cmd3.Parameters.AddWithValue("@ProductName", investigation.AdvicesNames[i]);
                                cmd3.Parameters.AddWithValue("@DrugDescription", investigation.FastFoods[i]);
                                cmd3.Parameters.AddWithValue("@Timing", investigation.Days[i]);
                                cmd3.Parameters.AddWithValue("@Doses", investigation.Doses[i]);
                                cmd3.Parameters.AddWithValue("@CreationID", UserID);
                                cmd3.Parameters.AddWithValue("@Mode", Mode);

                                intResult = cmd3.ExecuteNonQuery();

                                if (intResult > 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }


                        //service


                        if (investigation.ServicesID != null)
                        {
                            //   con.Open();
                            for (int i = 0; i < investigation.ServicesID.Length; i++)
                            {
                                intResult = 0;

                                SqlCommand cmd3 = new SqlCommand("IUPatientCheckupAndPrecriptionSevices", con);
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);

                                if (PatientInvestigationAndPrescription.CheckServiceID == "" || PatientInvestigationAndPrescription.CheckServiceID == null)
                                {
                                    cmd3.Parameters.AddWithValue("@CheckupServiceID", 0);
                                    //cmd3.Parameters["@PrecriptionID"].Direction = ParameterDirection.Output;
                                    Mode = "Add";
                                }
                                else
                                {
                                    cmd3.Parameters.AddWithValue("@CheckupServiceID", PatientInvestigationAndPrescription.CheckServiceID);
                                    Mode = "Edit";
                                }
                                cmd3.Parameters.AddWithValue("@ServiceID", investigation.ServicesID[i]);
                                cmd3.Parameters.AddWithValue("@ServiceName", investigation.Services[i]);
                                cmd3.Parameters.AddWithValue("@status", true);
                                cmd3.Parameters.AddWithValue("@Remark", investigation.serviceRemarks[i]);
                                cmd3.Parameters.AddWithValue("@CreationID", UserID);
                                cmd3.Parameters.AddWithValue("@Mode", Mode);

                                intResult = cmd3.ExecuteNonQuery();

                                if (intResult > 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }

                        //----------------------------------------Discharge  
                    }
                }
                #endregion
                #region IUPatientIPDDischarge


                SqlCommand cmd = new SqlCommand("GetPatientForPatientCheckupNPreparation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                cmd.Parameters.Add(new SqlParameter("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo));

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sd.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PatientInvestigationAndPrescription.WardID = Convert.ToString(row["WardID"]);
                    PatientInvestigationAndPrescription.WardName = Convert.ToString(row["WardName"]);
                    PatientInvestigationAndPrescription.RoomID = Convert.ToString(row["RoomID"]);
                    PatientInvestigationAndPrescription.RoomNo = Convert.ToString(row["RoomName"]);
                    PatientInvestigationAndPrescription.BedID = Convert.ToString(row["BedID"]);
                    PatientInvestigationAndPrescription.BedNo = Convert.ToString(row["BedNo"]);
                    PatientInvestigationAndPrescription.PatientIPDNO = Convert.ToString(row["PatientIPDNO"]);
                }
                    string DischargeDate1 = Convert.ToDateTime(objDischarge.DischargeDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(objDischarge.Dischargetime).ToString("hh:mm:ss tt");
                        
                    if (objDischarge.chkFinalDischarge == "on")
                    {
                        SqlCommand cmdPatientIPDDischarge = new SqlCommand("IUPatientIPDDischarge", con);
                        cmdPatientIPDDischarge.CommandType = CommandType.StoredProcedure;
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                        if (objDischarge.PatientIPDDischargeID == null || objDischarge.PatientIPDDischargeID == 0)
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientIPDDischargeID", 0);
                            cmdPatientIPDDischarge.Parameters["@PatientIPDDischargeID"].Direction = ParameterDirection.Output;
                            Mode = "Add";
                        }
                        else
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                            Mode = "Edit";
                        }
                    
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientIPDNO", PatientInvestigationAndPrescription.PatientIPDNO);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@WardID", PatientInvestigationAndPrescription.WardID);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@RoomID", PatientInvestigationAndPrescription.RoomID);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@BedID", PatientInvestigationAndPrescription.BedID);

                        if (objDischarge.FinalDiagnosis == null)
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@FinalDiagnosis", "");
                        }
                        else
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@FinalDiagnosis", objDischarge.FinalDiagnosis);
                        }


                        if (objDischarge.EndResult == null)
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@EndResult", "");
                        }
                        else
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@EndResult", objDischarge.EndResult);
                        }
                        if (objDischarge.DischargeDate == null)
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@Dischargedate", "");
                        }
                        else
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@Dischargedate", Convert.ToDateTime(DischargeDate1));

                        }
                        if (objDischarge.FinalDiagnosis == null)
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientDischargeDetail", "");
                        }
                        else
                        {
                            cmdPatientIPDDischarge.Parameters.AddWithValue("@PatientDischargeDetail", objDischarge.FinalDiagnosis);

                        }
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatientIPDDischarge.Parameters.AddWithValue("@Mode", Mode);

                        intResult = cmdPatientIPDDischarge.ExecuteNonQuery();
                      
                        objDischarge.PatientIPDDischargeID = Convert.ToInt32(cmdPatientIPDDischarge.Parameters["@PatientIPDDischargeID"].Value);
                    #endregion
                    //------------------end

                    //------------------IUPatientIPDDischargeSummary
                    if (objDischarge.PatientIPDDischargeID != 0)
                    {
                        string DischargeDate = Convert.ToDateTime(objDischarge.DischargeDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(objDischarge.Dischargetime).ToString("hh:mm:ss tt");
                        #region IUPatientIPDDischargeSummary
                        SqlCommand cmdIPDDischargeSummary = new SqlCommand("IUPatientIPDDischargeSummary", con);
                        cmdIPDDischargeSummary.CommandType = CommandType.StoredProcedure;
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                        //if (PatientInvestigationAndPrescription.CheckupAndPrecriptionID == null || PatientInvestigationAndPrescription.CheckupAndPrecriptionID == "")
                        //{
                        //    cmdIPDDischargeSummary.Parameters.AddWithValue("@CheckupAndPrecriptionID", 0);
                        //    cmdIPDDischargeSummary.Parameters["@CheckupAndPrecriptionID"].Direction = ParameterDirection.Output;
                        //    Mode = "Add";
                        //}
                        //else
                        //{
                        //    cmdIPDDischargeSummary.Parameters.AddWithValue("@CheckupAndPrecriptionID", PatientInvestigationAndPrescription.CheckupAndPrecriptionID);
                        //    Mode = "Edit";
                        //}
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo);
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@PatientIPDID", PatientInvestigationAndPrescription.PatientIPDNO);
                        //   cmdIPDDischargeSummary.Parameters.AddWithValue("@PatientType", "IPD");
                        if (objDischarge.EndResult == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@EndResult", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@EndResult", objDischarge.EndResult);
                        }

                        cmdIPDDischargeSummary.Parameters.AddWithValue("@Dischargedate", Convert.ToDateTime(DischargeDate));
                        if (objDischarge.FinalDiagnosis == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@FinalDiagnosis", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@FinalDiagnosis", objDischarge.FinalDiagnosis);
                        }


                        if (objDischarge.ChiefHistoryWithClinicalFindings == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ChiefHistoryClinicalFindings", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ChiefHistoryClinicalFindings", objDischarge.ChiefHistoryWithClinicalFindings);
                        }


                        if (PatientInvestigationAndPrescription.MedicalHistory == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@MedicalHistory", "");
                        }

                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@MedicalHistory", PatientInvestigationAndPrescription.MedicalHistory);

                        }
                        if (PatientInvestigationAndPrescription.OperativeNotes == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OperativeNotes", "");

                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OperativeNotes", PatientInvestigationAndPrescription.OperativeNotes);

                        }
                        if (PatientInvestigationAndPrescription.OtherInformation1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation1", PatientInvestigationAndPrescription.OtherInformation1);

                        }

                        if (PatientInvestigationAndPrescription.OtherInformation2 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation2", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation2", PatientInvestigationAndPrescription.OtherInformation2);

                        }

                        if (PatientInvestigationAndPrescription.InvestigationAdvice == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@InvestigationAdvice", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@InvestigationAdvice", PatientInvestigationAndPrescription.InvestigationAdvice);

                        }
                        if (PatientInvestigationAndPrescription.PersonalHistory == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@PersonalHistory", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@PersonalHistory", PatientInvestigationAndPrescription.PersonalHistory);
                        }
                        if (PatientInvestigationAndPrescription.NameOfSurgary == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@NameOfSurgary", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@NameOfSurgary", PatientInvestigationAndPrescription.NameOfSurgary);
                        }
                        if (PatientInvestigationAndPrescription.Temperature == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Temperature", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Temperature", PatientInvestigationAndPrescription.Temperature);
                        }
                        if (PatientInvestigationAndPrescription.Urin == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Urin);
                        }
                        //cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Pallor);
                        if (PatientInvestigationAndPrescription.Pulse == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Pulse", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Pulse", PatientInvestigationAndPrescription.Pulse);
                        }
                        if (PatientInvestigationAndPrescription.BP == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@BP", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@BP", PatientInvestigationAndPrescription.BP);
                        }
                        //cmd1.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.ABD);
                        if (PatientInvestigationAndPrescription.Allergy == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Allergy", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.Allergy);
                        }
                        if (PatientInvestigationAndPrescription.Weight == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Weight", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Weight", PatientInvestigationAndPrescription.Weight);
                        }
                        if (PatientInvestigationAndPrescription.RS == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@RS", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@RS", PatientInvestigationAndPrescription.RS);
                        }
                        if (PatientInvestigationAndPrescription.CVS == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CVS", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CVS", PatientInvestigationAndPrescription.CVS);
                        }
                        if (PatientInvestigationAndPrescription.ECG == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ECG", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ECG", PatientInvestigationAndPrescription.ECG);
                        }
                        if (PatientInvestigationAndPrescription.CNS == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CNS", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CNS", PatientInvestigationAndPrescription.CNS);
                        }
                        if (PatientInvestigationAndPrescription.HGT == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HGT", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HGT", PatientInvestigationAndPrescription.HGT);
                        }
                        if (PatientInvestigationAndPrescription.OtherFinding == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherFinding", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherFinding", PatientInvestigationAndPrescription.OtherFinding);
                        }
                        if (PatientInvestigationAndPrescription.Jaundice == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Jaundice", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Jaundice", PatientInvestigationAndPrescription.Jaundice);

                        }
                        if (PatientInvestigationAndPrescription.TreatmentGiven == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@TreatmentGiven", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@TreatmentGiven", PatientInvestigationAndPrescription.TreatmentGiven);
                        }
                        if (PatientInvestigationAndPrescription.PreciseHistory == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@PreciseHistory", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@PreciseHistory", PatientInvestigationAndPrescription.PreciseHistory);
                        }
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@ConditionOnDischarge", PatientInvestigationAndPrescription.ConditionOnDischarge);
                        if (PatientInvestigationAndPrescription.HistoryOfDialysis == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HistoryOfDialysis", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HistoryOfDialysis", PatientInvestigationAndPrescription.HistoryOfDialysis);
                        }
                        if (PatientInvestigationAndPrescription.Advices == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Advices", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Advices", PatientInvestigationAndPrescription.Advices);
                        }
                        if (objDischarge.OtherInformation == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherInformation", objDischarge.OtherInformation);
                        }
                        if (PatientInvestigationAndPrescription.FollowUp == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@FollowUp", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@FollowUp", PatientInvestigationAndPrescription.FollowUp);
                        }

                        cmdIPDDischargeSummary.Parameters.AddWithValue("@FollowupDate", Convert.ToDateTime(objDischarge.DischargeDate));
                        if (PatientInvestigationAndPrescription.Investigation == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@InvestigationNew", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@InvestigationNew", PatientInvestigationAndPrescription.Investigation);
                        }
                        if (PatientInvestigationAndPrescription.ChiefComplaint == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ChiefComplaint", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ChiefComplaint", PatientInvestigationAndPrescription.ChiefComplaint);

                        }
                        if (PatientInvestigationAndPrescription.HistoryOfChiefComplaint == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HistoryOfChiefComplaint", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HistoryOfChiefComplaint", PatientInvestigationAndPrescription.HistoryOfChiefComplaint);
                        }
                        if (PatientInvestigationAndPrescription.CourseDuringHospitalization == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CourseDuringHospitalization", "");

                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CourseDuringHospitalization", PatientInvestigationAndPrescription.CourseDuringHospitalization);
                        }
                        if (PatientInvestigationAndPrescription.MentrualHistory == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@MentrualHistory", "");

                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@MentrualHistory", PatientInvestigationAndPrescription.MentrualHistory);

                        }

                        // Discharge chk

                        if (PatientInvestigationAndPrescription.Temperature1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Temperature1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Temperature1", PatientInvestigationAndPrescription.Temperature1);
                        }
                        if (PatientInvestigationAndPrescription.Urin1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin1", PatientInvestigationAndPrescription.Urin1);
                        }
                        //cmdIPDDischargeSummary.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Pallor);
                        if (PatientInvestigationAndPrescription.Pulse1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Pulse1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Pulse1", PatientInvestigationAndPrescription.Pulse1);
                        }
                        if (PatientInvestigationAndPrescription.BP1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@BP1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@BP1", PatientInvestigationAndPrescription.BP1);
                        }
                        //cmd1.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.ABD);
                        if (PatientInvestigationAndPrescription.Allergy1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Allergy1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Allergy1", PatientInvestigationAndPrescription.Allergy1);
                        }
                        if (PatientInvestigationAndPrescription.Weight1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Weight1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Weight1", PatientInvestigationAndPrescription.Weight1);
                        }
                        if (PatientInvestigationAndPrescription.RS1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@RS1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@RS1", PatientInvestigationAndPrescription.RS1);
                        }
                        if (PatientInvestigationAndPrescription.CVS1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CVS", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CVS1", PatientInvestigationAndPrescription.CVS1);
                        }
                        if (PatientInvestigationAndPrescription.ECG1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ECG1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@ECG1", PatientInvestigationAndPrescription.ECG1);
                        }
                        if (PatientInvestigationAndPrescription.CNS1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CNS1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@CNS1", PatientInvestigationAndPrescription.CNS1);
                        }
                        if (PatientInvestigationAndPrescription.HGT1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HGT1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@HGT1", PatientInvestigationAndPrescription.HGT1);
                        }
                        if (PatientInvestigationAndPrescription.OtherFinding1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherFinding1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@OtherFinding1", PatientInvestigationAndPrescription.OtherFinding1);
                        }
                        if (PatientInvestigationAndPrescription.Jaundice1 == null)
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Jaundice1", "");
                        }
                        else
                        {
                            cmdIPDDischargeSummary.Parameters.AddWithValue("@Jaundice1", PatientInvestigationAndPrescription.Jaundice1);

                        }


                        cmdIPDDischargeSummary.Parameters.AddWithValue("@ConditionOnDischarge1", PatientInvestigationAndPrescription.ConditionOnDischarge1);


                        //--------------------


                        cmdIPDDischargeSummary.Parameters.AddWithValue("@CreationID", UserID);
                        cmdIPDDischargeSummary.Parameters.AddWithValue("@Mode", Mode);

                        // con.Open();
                        intResult = cmdIPDDischargeSummary.ExecuteNonQuery();
                        //       PatientInvestigationAndPrescription.PatientIPDNO = Convert.ToString(cmdIPDDischargeSummary.Parameters["@OPDIPDID"].Value);
                        // PatientInvestigationAndPrescription.CheckupAndPrecriptionID = Convert.ToString(cmdIPDDischargeSummary.Parameters["@CheckupAndPrecriptionID"].Value);
                        #endregion

                        //-------------------------------------------------------IPDDischargeSummaryOperativeNotes
                        #region IUPatientIPDDischargeSummaryOperativeNotes
                        SqlCommand cmdischargeSummaryOperativeNotes = new SqlCommand("IUPatientIPDDischargeSummaryOperativeNotes", con);
                        cmdischargeSummaryOperativeNotes.CommandType = CommandType.StoredProcedure;
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                        if (objOprative.OperativeID == null || objOprative.OperativeID == 0)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperativeID", 0);
                            cmdischargeSummaryOperativeNotes.Parameters["@OperativeID"].Direction = ParameterDirection.Output;
                            Mode = "Add";
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperativeID", objOprative.OperativeID);
                            Mode = "Edit";
                        }
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                        if (objOprative.Surgery == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@SurgeryPerformed", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@SurgeryPerformed", objOprative.Surgery);
                        }
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@EmergencyElective", "IPD");
                        if (objOprative.Anaesthesia == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Anaesthesia", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Anaesthesia", objOprative.Anaesthesia);
                        }

                        if (objOprative.Anaesthelist == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Anaesthetist", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Anaesthetist", objOprative.Anaesthelist);
                        }
                        if (objOprative.Surgeons == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Surgeons", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Surgeons", objOprative.Surgeons);
                        }
                        if (objOprative.Associate == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@AssistantAssociateSurgeons", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@AssistantAssociateSurgeons", objOprative.Associate);
                        }
                        if (objOprative.OperationdDate == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperationDateandTime", DateTime.Now);
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperationDateandTime", objOprative.OperationdDate + " " + objOprative.Time);
                        }
                        if (objOprative.Operation == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperationNotes", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@OperationNotes", objOprative.Operation);
                        }

                        if (objOprative.Treatment == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@TreatMentAdvised", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@TreatMentAdvised", objOprative.Treatment);
                        }

                        if (objOprative.Pre_Operative == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PreOperativeDiagnosis", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PreOperativeDiagnosis", objOprative.Pre_Operative);
                        }

                        //cmd1.Parameters.AddWithValue("@Urin", PatientInvestigationAndPrescription.Pallor);
                        if (objOprative.Conjunctival == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@ConjunctivalFlap", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@ConjunctivalFlap", objOprative.Conjunctival);
                        }

                        if (objOprative.Cautery == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Cautery", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Cautery", objOprative.Cautery);
                        }
                        //cmd1.Parameters.AddWithValue("@Allergy", PatientInvestigationAndPrescription.ABD);
                        if (objOprative.Incision == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Incision", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Incision", objOprative.Incision);
                        }

                        if (objOprative.Side == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@SidePort", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@SidePort", objOprative.Side);
                        }
                        if (objOprative.Size == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Size", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Size", objOprative.Size);
                        }

                        if (objOprative.AC_Maintainer == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@ACMaintainer", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@ACMaintainer", objOprative.AC_Maintainer);
                        }

                        if (objOprative.Capsulotomy == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Capsulotomy", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Capsulotomy", objOprative.Capsulotomy);
                        }
                        if (objOprative.Hydro == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Hydrodissection", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Hydrodissection", objOprative.Hydro);
                        }
                        if (objOprative.Hydro_Dilineation == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@HydroDelineation", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@HydroDelineation", objOprative.Hydro_Dilineation);
                        }
                        if (objOprative.Nucleus == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@NucleusRotation", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@NucleusRotation", objOprative.Nucleus);
                        }
                        if (objOprative.Pacho == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PhacoTechnique", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PhacoTechnique", objOprative.Pacho);
                        }
                        if (objOprative.Posterior == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PosteriorCapsule", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@PosteriorCapsule", objOprative.Posterior);
                        }

                        if (objOprative.IOL == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@IOL", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@IOL", objOprative.IOL);
                        }

                        if (objOprative.Wound == null)
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@WoundClosure", "");
                        }
                        else
                        {
                            cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@WoundClosure", objOprative.Wound);
                        }

                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@AdditionalNotes", "");

                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Image1", objOprative.Image1);
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Image2", objOprative.Image2);
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@CreationID", UserID);
                        cmdischargeSummaryOperativeNotes.Parameters.AddWithValue("@Mode", Mode);
                        //  cmdOperativeNotes.Parameters.AddWithValue("@ReferenceCode", "");


                        intResult = cmdischargeSummaryOperativeNotes.ExecuteNonQuery();


                        #endregion

                        #region IUDeathHistory

                        if (objDischarge.PatientIPDDischargeID != 0)
                        {
                            objDischarge.ExpierdDate = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        string DateOfExpired = Convert.ToDateTime(objDischarge.ExpierdDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(objDischarge.ExpierdTime).ToString("hh:mm:ss tt");
                        if (objDischarge.PatientIPDDischargeID != 0)
                        {
                            SqlCommand cmdIUDeathHistory = new SqlCommand("IUDeathHistory", con);
                            cmdIUDeathHistory.CommandType = CommandType.StoredProcedure;
                            cmdIUDeathHistory.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                            cmdIUDeathHistory.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                            if (objDischarge.DeathID == null || objDischarge.DeathID == 0)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@DeathID", 0);
                                cmdIUDeathHistory.Parameters["@DeathID"].Direction = ParameterDirection.Output;
                                Mode = "Add";
                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@DeathID", objDischarge.DeathID);
                                Mode = "Edit";
                            }
                            cmdIUDeathHistory.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                            cmdIUDeathHistory.Parameters.AddWithValue("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo);
                            cmdIUDeathHistory.Parameters.AddWithValue("@PatientIPDNO", PatientInvestigationAndPrescription.PatientIPDNO);

                            cmdIUDeathHistory.Parameters.AddWithValue("@DateOfExpired", DateOfExpired);
                            if (objDischarge.Causer == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@CauseOfIncident", "");

                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@CauseOfIncident", objDischarge.Causer);
                            }
                            if (objDischarge.Medico == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@MedicoLegalIssue", objDischarge.Medico);
                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@MedicoLegalIssue", objDischarge.Medico);

                            }
                            //    cmdPatientIPDDischarge.Parameters.AddWithValue("@FinalDiagnosis", objDischarge.RoomID);
                            if (objDischarge.Reason == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@Reason", "");
                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@Reason", objDischarge.Reason);
                            }
                            if (objDischarge.TreatmentDuring == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@TreatmentDuringHospitalization", "");

                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@TreatmentDuringHospitalization", objDischarge.TreatmentDuring);

                            }
                            if (objDischarge.Line == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@LineOfIVIM", objDischarge.Line);

                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@LineOfIVIM", objDischarge.Line);
                            }
                            if (objDischarge.OtherInformation == null)
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@OtherInformation", "");

                            }
                            else
                            {
                                cmdIUDeathHistory.Parameters.AddWithValue("@OtherInformation", objDischarge.OtherInformation);

                            }

                            cmdIUDeathHistory.Parameters.AddWithValue("@DeathStatus", "");

                            cmdIUDeathHistory.Parameters.AddWithValue("@CreationID", UserID);
                            cmdIUDeathHistory.Parameters.AddWithValue("@Mode", Mode);

                            intResult = cmdIUDeathHistory.ExecuteNonQuery();

                        }

                        #endregion


                        #region IUPatientIPDDischargeICDCode



                        if (objDischarge.ICDCodeID != null)
                        {

                            string[] ICDName = PatientInvestigationAndPrescription.ICDName.Split(',');
                            ICDName = ICDName.Where(name => !string.IsNullOrEmpty(name)).ToArray();

                            string[] ICDCode = PatientInvestigationAndPrescription.ICDCode.Split(',');
                            ICDCode = ICDCode.Where(name => !string.IsNullOrEmpty(name)).ToArray();

                            string[] ICDCodeID = PatientInvestigationAndPrescription.ICDCodeID.Split(',');
                            ICDCodeID = ICDCodeID.Where(name => !string.IsNullOrEmpty(name)).ToArray();

                            for (int k = 0; k < ICDName.Length; k++)
                            {

                                SqlCommand cmdICDCodeID = new SqlCommand("IUPatientIPDDischargeICDCode", con);
                                cmdICDCodeID.CommandType = CommandType.StoredProcedure;
                                cmdICDCodeID.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                                cmdICDCodeID.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                                if (PatientInvestigationAndPrescription.PatientIPDDischargeICDCodeID == null || PatientInvestigationAndPrescription.PatientIPDDischargeICDCodeID == 0)
                                {
                                    cmdICDCodeID.Parameters.AddWithValue("@PatientIPDDischargeICDCodeID", 0);
                                    cmdICDCodeID.Parameters["@PatientIPDDischargeICDCodeID"].Direction = ParameterDirection.Output;
                                    Mode = "Add";
                                }
                                else
                                {
                                    cmdICDCodeID.Parameters.AddWithValue("@PatientIPDDischargeICDCodeID", PatientInvestigationAndPrescription.PatientIPDDischargeICDCodeID);
                                    Mode = "Edit";
                                }

                                cmdICDCodeID.Parameters.AddWithValue("@ICDCodeID", ICDCodeID[k]);
                                cmdICDCodeID.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);


                                cmdICDCodeID.Parameters.AddWithValue("@CreationID", UserID);
                                cmdICDCodeID.Parameters.AddWithValue("@Mode", Mode);

                                intResult = cmdICDCodeID.ExecuteNonQuery();

                            }
                        }



                        #endregion


                        if (objDischarge.PatientIPDDischargeID != 0)
                        {
                            #region IUTransferHistory

                            SqlCommand cmdIUTransferHistory = new SqlCommand("IUTransferHistory", con);
                            cmdIUTransferHistory.CommandType = CommandType.StoredProcedure;
                            cmdIUTransferHistory.Parameters.AddWithValue("@HospitalID", PatientInvestigationAndPrescription.HospitalID);
                            cmdIUTransferHistory.Parameters.AddWithValue("@LocationID", PatientInvestigationAndPrescription.LocationID);

                            if (objDischarge.TransferID == null || objDischarge.TransferID == 0)
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@TransferID", 0);
                                cmdIUTransferHistory.Parameters["@TransferID"].Direction = ParameterDirection.Output;
                                Mode = "Add";
                            }
                            else
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@TransferID", objDischarge.TransferID);
                                Mode = "Edit";
                            }
                            cmdIUTransferHistory.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                            cmdIUTransferHistory.Parameters.AddWithValue("@PatientRegNO", PatientInvestigationAndPrescription.PatientRegNo);
                            cmdIUTransferHistory.Parameters.AddWithValue("@PatientIPDNO", PatientInvestigationAndPrescription.PatientIPDNO);
                            if (objDischarge.ChiefComplaint == null)
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@ChiefComplaintWithDuration", "");

                            }
                            else
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@ChiefComplaintWithDuration", objDischarge.ChiefComplaint);

                            }
                            if (objDischarge.ChiefHistory == null)
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@ChiefHistoryWithClinicalFindings", "");

                            }
                            else
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@ChiefHistoryWithClinicalFindings", objDischarge.ChiefHistory);
                            }
                            if (PatientInvestigationAndPrescription.pasthistory == null)
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@PastHistory", "");

                            }
                            else
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@PastHistory", PatientInvestigationAndPrescription.pasthistory);

                            }
                            //    cmdPatientIPDDischarge.Parameters.AddWithValue("@FinalDiagnosis", objDischarge.RoomID);
                            if (objDischarge.Diagnosis == null)
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@Diagnosis", "");

                            }
                            else
                            {
                                cmdIUTransferHistory.Parameters.AddWithValue("@Diagnosis", objDischarge.Diagnosis);

                            }

                            cmdIUTransferHistory.Parameters.AddWithValue("@CreationID", UserID);
                            cmdIUTransferHistory.Parameters.AddWithValue("@Mode", Mode);

                            intResult = cmdIUTransferHistory.ExecuteNonQuery();


                        }



                        #endregion

                        #region IUPatientIPDDischargeSummaryInvestigation
                        if (investigation != null)
                        {
                            if (investigation.TestIDs != null)
                            {
                                for (int i = 0; i < investigation.TestIDs.Length; i++)
                                {
                                    SqlCommand cmd2 = new SqlCommand("IUPatientIPDDischargeSummaryInvestigation", con);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                                    if (PatientInvestigationAndPrescription.CheckupID == null || PatientInvestigationAndPrescription.CheckupID == "")
                                    {
                                        cmd2.Parameters.AddWithValue("@CheckupID", 0);
                                        Mode = "Add";
                                    }
                                    else
                                    {
                                        cmd2.Parameters.AddWithValue("@CheckupID", PatientInvestigationAndPrescription.CheckupID);
                                        Mode = "Edit";
                                    }
                                    cmd2.Parameters.AddWithValue("@TestID", Convert.ToInt32(investigation.TestIDs[i]));
                                    cmd2.Parameters.AddWithValue("@TestName", investigation.InvestigationNames[i].ToString());
                                    cmd2.Parameters.AddWithValue("@LabNo", Convert.ToInt32(investigation.LabNo[i]));
                                    cmd2.Parameters.AddWithValue("@Status", 0);
                                    cmd2.Parameters.AddWithValue("@InvestDate", Convert.ToDateTime(objOprative.InvestDate));
                                    cmd2.Parameters.AddWithValue("@Result2", investigation.Remarks[i].ToString());
                                    cmd2.Parameters.AddWithValue("@Remark", investigation.Remarks[i].ToString());
                                    cmd2.Parameters.AddWithValue("@CreationID", UserID);
                                    cmd2.Parameters.AddWithValue("@Mode", Mode);
                                    //  con.Open();
                                    intResult = cmd2.ExecuteNonQuery();
                                    // con.Close();
                                    if (intResult > 0)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        flag = false;
                                    }

                                }
                            }
                            #endregion

                        }

                        #region IUPatientIPDDischargeSummaryPrecraption
                        if (investigation!= null)
                        {
                            if (investigation.DrugIDs != null)
                            {
                                for (int i = 0; i < investigation.DrugIDs.Length; i++)
                                {
                                    intResult = 0;

                                    SqlCommand cmd3 = new SqlCommand("IUPatientIPDDischargeSummaryPrecraption", con);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@PatientIPDDischargeID", objDischarge.PatientIPDDischargeID);
                                    if (PatientInvestigationAndPrescription.PrecriptionID == "" || PatientInvestigationAndPrescription.PrecriptionID == null)
                                    {
                                        cmd3.Parameters.AddWithValue("@PrecriptionID", 0);
                                        cmd3.Parameters["@PrecriptionID"].Direction = ParameterDirection.Output;
                                        Mode = "Add";
                                    }
                                    else
                                    {
                                        cmd3.Parameters.AddWithValue("@PrecriptionID", PatientInvestigationAndPrescription.PrecriptionID);
                                        Mode = "Edit";
                                    }
                                    cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(investigation.DrugIDs[i]));
                                    cmd3.Parameters.AddWithValue("@ProductName", investigation.AdvicesNames[i]);
                                    cmd3.Parameters.AddWithValue("@DrugDescription", investigation.FastFoods[i]);
                                    cmd3.Parameters.AddWithValue("@Timing", investigation.Days[i]);
                                    cmd3.Parameters.AddWithValue("@Doses", investigation.Doses[i]);
                                    cmd3.Parameters.AddWithValue("@CreationID", UserID);
                                    cmd3.Parameters.AddWithValue("@Mode", Mode);
                                    //   con.Open();
                                    intResult = cmd3.ExecuteNonQuery();
                                    // con.Close();

                                }


                            }
                            if (intResult > 0)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                            #endregion
                        }
                    }
                    }
                con.Close();
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }
        public DataSet GetPatientCheckupAndPrecriptiont(int CheckupAndPrecriptionID)
        {
            connection();
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand("GetPatientCheckupAndPrecriptiont", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@CheckupAndPrecriptionID", CheckupAndPrecriptionID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
           
            con.Open();
          
            sd.Fill(ds);
            con.Close();
                return ds;
            
        }

        public DataSet GetPatientIPDDischargeAndSummary(string RegNo)
        {
            DataSet ds = new DataSet();
            connection();
            List<PatientInvestigationAndPrescription_Model> patientlist = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("GetPatientIPDDischargeAndSummary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
           
            con.Open();
            sd.Fill(ds);
            return ds;
        }

        public DataSet GetPatientOldIPDFInalBill(string RegNo,int IPDNo)
        {
            DataSet ds = new DataSet();
            connection();
            List<PatientInvestigationAndPrescription_Model> patientlist = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("GetPatientOldIPDFInalBill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
            cmd.Parameters.Add(new SqlParameter("@PatientIPDNO", Convert.ToInt32(IPDNo)));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            con.Open();
            sd.Fill(ds);
            return ds;
        }

        public DataSet GetBindPatientName(string PatientRegNo, string OPDIPDNo)
        {
            connection();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("InvestigationParameter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@OPDIPDNo", OPDIPDNo);
               
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public List<PatientInvestigationAndPrescription_Model> GetEndResult()
        {
            List<PatientInvestigationAndPrescription_Model> serachlist = new List<PatientInvestigationAndPrescription_Model>();

            connection();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllEndResult", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new PatientInvestigationAndPrescription_Model
                    {
                        EndResult = dr["EndResultName"].ToString(),
                        EndResultID = dr["EndResultID"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }




        public bool CheckDischargePatient(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo)
        {
            SqlParameter[] aParams = new SqlParameter[5];
            try
            {
                connection();
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@NameExists", SqlDbType.Bit);
                aParams[2].Direction = ParameterDirection.Output;
                aParams[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[3].Value = PatientRegNo;
                aParams[4] = new SqlParameter("@PatientIPDno", SqlDbType.Int);
                aParams[4].Value = PatientIPDNo;


                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "CheckDischargePatient", aParams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return Convert.ToBoolean(aParams[2].Value);
        }

        public List<PatientInvestigationAndPrescription_Model> GetPatientForPatientCheckupNPreparation(string RegNo)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> patientlist = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("GetPatientForPatientCheckupNPreparation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientlist.Add(
                    new PatientInvestigationAndPrescription_Model
                    {
                        HospitalID = Convert.ToInt32(dr["HospitalID"]),
                        LocationID = Convert.ToInt32(dr["LocationID"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                        OPDIPDID = Convert.ToInt32(dr["PatientIPDNO"]),
                        GuardianName = Convert.ToString(dr["GuardianName"]),
                        AddmissionDate = Convert.ToString(dr["AddmissionDate"]),
                        ConsultantDrID = Convert.ToString(dr["ConsultantDrID"]),
                        ConsultantDoctor = Convert.ToString(dr["ConsultantDoctor"]),
                        DepartmentID = Convert.ToString(dr["DepartmentID"]),
                        DepartmentName = Convert.ToString(dr["DepartmentName"]),
                        ReferredByDoctorID = Convert.ToString(dr["ReferredByDoctorID"]),
                        ReferredByDoctorName = Convert.ToString(dr["ReferredDoctor"]),

                    });
            }
            return patientlist;
        }


        public List<PatientInvestigationAndPrescription_Model> GetPatientRegNoByName(string PatientName)
        {
            DataTable dt = new DataTable();

            connection();
            List<PatientInvestigationAndPrescription_Model> patientdischargelist = new List<PatientInvestigationAndPrescription_Model>();
            //SqlCommand cmd = new SqlCommand("Select distinct Patient.PatientRegNO,PatientIPDNO,PatientName from patient left join PatientIPDWardDetails on Patient.PatientRegNO=PatientIPDWardDetails.PatientRegNO where PatientName like '" + PatientName + "%' and PatientIPDNO is not null and IsCurrentBed=0", con);
            //SqlCommand cmd = new SqlCommand("Select distinct Patient.PatientRegNO,PatientIPDNO,PatientName from patient left join PatientIPDWardDetails on Patient.PatientRegNO=PatientIPDWardDetails.PatientRegNO where PatientName like '" + PatientName + "%' and PatientIPDNO is not null", con);

            SqlCommand cmd = new SqlCommand("Select distinct Patient.PatientRegNO,PatientName from patient inner join PatientIPDDetails on Patient.PatientRegNO=PatientIPDDetails.PatientRegNO where PatientName like '" + PatientName + "%' and Patient.PatientType in ('IPD','EMG') and Patient.RowStatus = 0   and PatientIPDDetails.RowStatus = 0  and PatientIPDDetails.HospitalID='" + HospitalID + "' and PatientIPDDetails.LocationID='" + LocationID + "'   and patient.HospitalID='" + HospitalID + "' and patient.LocationID='" + LocationID + "' group by  Patient.PatientRegNO ,PatientName", con);

            con.Open();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            sd.Fill(dt);
            List<string> listPatient = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();
                    patientDischarge.PatientRegNo = Convert.ToInt32(row["PatientRegNO"]);
                    patientDischarge.PatientName = row["PatientName"].ToString();
                    patientdischargelist.Add(patientDischarge);
                }
                con.Close();
            }
            else
            {
                patientdischargelist = null;
            }
            return patientdischargelist;
        }

        public List<PatientInvestigationAndPrescription_Model> GetInvestigationName(string InvestigationName)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> InvestigationList = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("select TestID,TestName from TestMaster where TestName like '" + InvestigationName + "%'", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();
                patientDischarge.CheckupID = dr["TestID"].ToString();
                patientDischarge.InvestigationName = dr["TestName"].ToString();
                InvestigationList.Add(patientDischarge);
            }
            return InvestigationList;
        }


        public List<PatientInvestigationAndPrescription_Model> GetDrugsName(string Drug)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> druglist = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("select top 10 DrugID,DrugName from Drug where DrugName like '"+Drug+"%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                //PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();

                druglist.Add(new PatientInvestigationAndPrescription_Model  {
                
                 DrugID = dr["DrugID"].ToString(),
                DrugName = dr["DrugName"].ToString()
                });
            }
            return druglist;
        }
        public List<PatientInvestigationAndPrescription_Model> GetICDCodes(string ICDCodes, string SearchICDBy)
        {
            connection();
            SqlCommand cmd;
            List<PatientInvestigationAndPrescription_Model> dricdcode = new List<PatientInvestigationAndPrescription_Model>();
            if (SearchICDBy == "Search By Name")
            {
                 cmd = new SqlCommand("select top 50 ICDCodeID,ICDCode,ICDName from ICDCode where ICDName like '" + ICDCodes + "%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            }
            else
            {
                cmd = new SqlCommand("select top 50 ICDCodeID,ICDCode,ICDName from ICDCode where ICDCode like '" + ICDCodes + "%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            }
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                dricdcode.Add(new PatientInvestigationAndPrescription_Model
                {

                    ICDCodeID = dr["ICDCodeID"].ToString(),
                    ICDCode = dr["ICDCode"].ToString(),
                    ICDName = dr["ICDName"].ToString()
                });
            }
            return dricdcode;
        }

        public DataSet FillICDCodes(int HospitalID, int LocationID, int ICDCodeID)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ICDCodeID", SqlDbType.Int);
                param[2].Value = ICDCodeID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "FillICDCodes", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
      
        public List<PatientInvestigationAndPrescription_Model> GetDoses(string Dose)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> Advicelist = new List<PatientInvestigationAndPrescription_Model>();
            SqlCommand cmd = new SqlCommand("select  PackagesName, PackagesID from Advice where PackagesName like '" + Dose + "%' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientInvestigationAndPrescription_Model patientDischarge = new PatientInvestigationAndPrescription_Model();
                patientDischarge.AdviceID = dr["PackagesID"].ToString();
                patientDischarge.AdviceName = dr["PackagesName"].ToString();
                Advicelist.Add(patientDischarge);
            }
            return Advicelist;
        }


        public List<PatientInvestigationAndPrescription_Model> GetPatientName(string search)
        {
            connection();
            List<PatientInvestigationAndPrescription_Model> patientnamelist = new List<PatientInvestigationAndPrescription_Model>();
            //SqlCommand cmd = new SqlCommand("GetRole", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Select PatientName,PatientRegNO from Patient where PatientType = 'IPD' AND PatientName LIKE '%" + search + "%' and RowStatus=0 order by PatientRegNO", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new PatientInvestigationAndPrescription_Model
                    {
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"])
                    });

            }
            return patientnamelist;
        }



        public List<InvestigationForPrescription1> GetServiceName(string search)
        {
            connection();
            List<InvestigationForPrescription1> patientnamelist = new List<InvestigationForPrescription1>();
            //SqlCommand cmd = new SqlCommand("GetRole", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("select ServiceID  ,upper(Services.ServiceName) as ServiceName from Services where RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " AND ServiceType in ('IPD','Both')   order by  ServiceName asc", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new InvestigationForPrescription1
                    {
                         ServicesID= dr["ServiceID"].ToString(),
                        Services= dr["ServiceName"].ToString(),
                    });

            }
            return patientnamelist;
        }

        # region Delete Patient Checkup And Precription
        public bool DeletePatientCheckupAndPrecription(int CheckupAndPrecriptionID)
        {
            try
            {
                connection();
                SqlParameter[] aParams = new SqlParameter[4];
                aParams[0] = new SqlParameter("@CheckupAndPrecriptionID", SqlDbType.Int);
                aParams[0].Value = CheckupAndPrecriptionID;

                aParams[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[1].Value = HospitalID;

                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientCheckupAndPrecription", aParams);
                return true;

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        # endregion

        public bool DeletePatientIPDDischarge(int PatientIPDDischargeID)
        {
            try
            {
                connection();
                SqlParameter[] aParams = new SqlParameter[1];
                aParams[0] = new SqlParameter("@PatientIPDDischargeID", SqlDbType.Int);
                aParams[0].Value = PatientIPDDischargeID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientIPDDischarge", aParams);
                return true;
                
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }



        public DataSet GetPatientIPDDischargeAndSummarForDailyHistory(int PatientRegNO, int PatientIPDNO)
        {
            connection();
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                    param[2].Value = PatientRegNO;
                    param[3] = new SqlParameter("@PatientIPDNO", SqlDbType.Int);
                   param[3].Value = PatientIPDNO;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientIPDDischargeAndSummarForDailyHistory", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            
        }
            
    }
}