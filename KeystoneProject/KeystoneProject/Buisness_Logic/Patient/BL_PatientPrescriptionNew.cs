using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Net;
namespace KeystoneProject.Buisness_Logic.Patient

{
    public class BL_PatientPrescriptionNew
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public bool IUPatientIPDWardTransfer(KeystoneProject.Models.Patient.PatientPrescriptionNew obj)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand("IUPatientIPDWardTransfer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                
                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
        }

        public DataSet GetPatient(int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForPatientPrescriptionNewForModal", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientData(int HospitalID, int LocationID, int PatientRegNO)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForPatientPrescriptionNew", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPackagesForPrescription(int HospitalID, int LocationID, int PackagesID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PackagesID", SqlDbType.Int);
                param[2].Value = PackagesID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPackagesForPrescription", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet FillMedicinesOnMedicines(int HospitalID, int LocationID, int MedicineLibraryID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@MedicineLibraryID", SqlDbType.Int);
                param[2].Value = MedicineLibraryID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "FillMedicinesOnMedicines", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet FillICDCodes(int HospitalID, int LocationID, int ICDCodeID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
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
        public DataSet GetMedicine(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select MedicineLibraryID ,upper(MedicineLibrary.Medicines) as Medicines from MedicineLibrary where Medicines like''+@prefix+'%' and RowStatus = 0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by Medicines asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetICDCodes(string prefix,string icd)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllICDCodes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prefix", prefix);
                cmd.Parameters.AddWithValue("@icd", icd);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetPackages(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  PackagesID,PackagesName from  Packages where PackagesName like ''+@prefix+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  PackagesName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }


        #region SearchPatientOPDByNameID
        public DataSet SearchPatientOPDByNameID(int HospitalID, int LocatioID, string PatientName)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[5];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocatioID;
                aParams[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                aParams[2].Value = Convert.ToInt32(0);
                aParams[3] = new SqlParameter("@PatientName", SqlDbType.VarChar, 100);
                aParams[3].Value = PatientName;


                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchPatientOPDByNameID", aParams);

            }
            catch (Exception ex)
            {
                // ExceptionManager.Publish(ex);
                throw ex;

            }
            return ds;
        }
        #endregion


        public DataSet GetAllTestMaster(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllTestMasterForProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
              
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }
        public DataSet GetAllTreatmentAdvices(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllTreatmentAdviceForPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
              
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }

        public DataSet GetAllProvisionalDiagonosis(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllProvisionalDiagonosisForPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
               
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }

        public DataSet GetAllChiefHistory(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllChiefHistoryForPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
               
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }

        public DataSet GetAllAllergies(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllAllergiesForPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }

        public DataSet GetAllChiefComplaint(int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllChiefComplaintForPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
              
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }

        }


        public DataSet GetOLDPatientPrescriptionNew(string PatientPrescriptionNewID, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetOLDPatientPrescriptionNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", PatientPrescriptionNewID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }
        public DataSet GetPatientPrescriptionNewMedicine(string PatientPrescriptionNewID, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientPrescriptionNewMedicine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", PatientPrescriptionNewID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public DataSet GetPatientPrescriptionICDCodes(string PatientPrescriptionNewID, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientPrescriptionICDCodes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", PatientPrescriptionNewID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;
        }

        public bool Save(PatientPrescriptionNew objmodel)
        {
            Connect();
            try
            {
                 
              //  string[] Investigation = objmodel.Investigations.Split(',');

                string Newid = objmodel.PatientPrescriptionNewID;

                SqlCommand cmd1 = new SqlCommand("IUPatientPrescriptionNew", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                if (objmodel.PatientPrescriptionNewID != "" && objmodel.PatientPrescriptionNewID != null)
                {
                    cmd1.Parameters.AddWithValue("@PatientPrescriptionNewID", objmodel.PatientPrescriptionNewID);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@PatientPrescriptionNewID", 0);
                    cmd1.Parameters["@PatientPrescriptionNewID"].Direction = ParameterDirection.Output;
                }

                cmd1.Parameters.AddWithValue("@PatientRegNo", objmodel.PatientRegNO); 
                cmd1.Parameters.AddWithValue("@OPDIPDID", objmodel.OPDIPDID);
                cmd1.Parameters.AddWithValue("@Date", DateTime.Now); 
                cmd1.Parameters.AddWithValue("@Height", objmodel.Height);
                cmd1.Parameters.AddWithValue("@Weight", objmodel.Weight);
                cmd1.Parameters.AddWithValue("@Pulse", objmodel.Pulse);
                cmd1.Parameters.AddWithValue("@BP", objmodel.BP);
                cmd1.Parameters.AddWithValue("@BMI", objmodel.BMI);
                cmd1.Parameters.AddWithValue("@Temp", objmodel.Temp);

                if (objmodel.txtChiefComplaintName!=null)
                {
                    cmd1.Parameters.AddWithValue("@ChiefComplaint", WebUtility.HtmlEncode(objmodel.txtChiefComplaintName).Replace("\r\n", "<br>"));
            
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ChiefComplaint", "");
            
                }

                if (objmodel.txtAllergiesName != null)
                {
                    cmd1.Parameters.AddWithValue("@Allergies", WebUtility.HtmlEncode(objmodel.txtAllergiesName).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Allergies","");

                }

                if (objmodel.txtClinicalFindingName != null)
                {
                    cmd1.Parameters.AddWithValue("@ClinicalFindings", WebUtility.HtmlEncode(objmodel.txtClinicalFindingName).Replace("\r\n", "<br>"));
             
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ClinicalFindings", "");
             
                }

                if (objmodel.txtProvitionalDiagnosisName != null)
                {
                    cmd1.Parameters.AddWithValue("@ProvisionalDiagonosis", WebUtility.HtmlEncode(objmodel.txtProvitionalDiagnosisName).Replace("\r\n", "<br>"));
           
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ProvisionalDiagonosis", "");
            
                }

                if (objmodel.txtInvestigationsName != null)
                {
                    cmd1.Parameters.AddWithValue("@Investigations", WebUtility.HtmlEncode(objmodel.txtInvestigationsName).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Investigations", "");


                }
                if (objmodel.txtTreatmentAdviceName != null)
                {
                    cmd1.Parameters.AddWithValue("@TreatmentAdvice", WebUtility.HtmlEncode(objmodel.txtTreatmentAdviceName).Replace("\r\n", "<br>"));
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@TreatmentAdvice","");

                }
                
                                
                cmd1.Parameters.AddWithValue("@Remark", objmodel.Remark);
                cmd1.Parameters.AddWithValue("@FollowUpDate", objmodel.FollowUpDate);
                cmd1.Parameters.AddWithValue("@FollowUpTime", Convert.ToDateTime(objmodel.FollowUpTime).ToString("hh:mm:ss tt"));
                 string date= Convert.ToDateTime(objmodel.FollowUpTime).ToString("hh:mm:ss tt");

                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                if (objmodel.PatientPrescriptionNewID != "" && objmodel.PatientPrescriptionNewID != null)
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Edit");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                }

                con.Open();
                int i = cmd1.ExecuteNonQuery();

                con.Close();
                

                objmodel.PatientPrescriptionNewID = Convert.ToString(cmd1.Parameters["@PatientPrescriptionNewID"].Value);

                string[] Medicines = objmodel.Medicines.Split(',');
                Medicines = Medicines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] Strength = objmodel.Strength.Split(',');
                string[] Frequency = objmodel.Frequency.Split(',');
                string[] Days = objmodel.Days.Split(',');
                string[] Instruction = objmodel.Instruction.Split(',');
                string[] MedicinesID = objmodel.MedicineLibraryID.Split(',');


                #region Patient prescription Med


                for (int k = 0; k < Medicines.Length; k++)
                {
                    if (Medicines[k].ToString() != "")
                    {
                        #region IUPatientPrescriptionNewMedicine

                        SqlCommand cmd = new SqlCommand("IUPatientPrescriptionNewMedicine", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", objmodel.PatientPrescriptionNewID);
                        cmd.Parameters.AddWithValue("@PatientPrescriptionNewMedicineID", 0);
                        cmd.Parameters["@PatientPrescriptionNewMedicineID"].Direction = ParameterDirection.Output;
                      
                        cmd.Parameters.AddWithValue("@PatientRegNo", objmodel.PatientRegNO);
                        cmd.Parameters.AddWithValue("@OPDIPDID", objmodel.OPDIPDID);
                        cmd.Parameters.AddWithValue("@MedicineLibraryID", MedicinesID[k]);

                        cmd.Parameters.AddWithValue("@Medicines", Medicines[k]);
                        cmd.Parameters.AddWithValue("@Strength", Strength[k]);
                        cmd.Parameters.AddWithValue("@Frequency", Frequency[k]);
                        cmd.Parameters.AddWithValue("@Instruction", Instruction[k]);
                        cmd.Parameters.AddWithValue("@Days", Days[k]);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                        con.Open();
                        int j = cmd.ExecuteNonQuery();
                        con.Close();
                       
                        #endregion
                    }
                }

                #endregion
                if (objmodel.ICDCodeID != null)
                {

                    #region ICDCodes
                    string[] ICDCodeID = objmodel.ICDCodeID.Split(',');
                    string[] ICDName = objmodel.ICDName.Split(',');
                    string[] ICDCode = objmodel.ICDCode.Split(',');

                    for (int m = 0; m < ICDCodeID.Length; m++)
                    {
                        if (ICDCodeID[m].ToString() != "")
                        {
                            #region IUPatientPrescriptionNewICDCodes

                            SqlCommand cmd = new SqlCommand("IUPatientPrescriptionNewICDCodes", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", objmodel.PatientPrescriptionNewID);
                            cmd.Parameters.AddWithValue("@PatientPrescriptionNewICDCodesID", 0);
                            cmd.Parameters["@PatientPrescriptionNewICDCodesID"].Direction = ParameterDirection.Output;

                            cmd.Parameters.AddWithValue("@PatientRegNo", objmodel.PatientRegNO);
                            cmd.Parameters.AddWithValue("@OPDIPDID", objmodel.OPDIPDID);
                            cmd.Parameters.AddWithValue("@ICDCodeID", ICDCodeID[m]);

                            cmd.Parameters.AddWithValue("@ICDName", ICDName[m]);
                            cmd.Parameters.AddWithValue("@ICDCode", ICDCode[m]);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);
                            cmd.Parameters.AddWithValue("@Mode", "Add");
                            con.Open();
                            int j = cmd.ExecuteNonQuery();
                            con.Close();

                            #endregion
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public bool Save1(PatientPrescriptionNew objmodel)
        {
            Connect();
            try
            {

                //  string[] Investigation = objmodel.Investigations.Split(',');

                string Newid = objmodel.PatientPrescriptionNewID;

                SqlCommand cmd1 = new SqlCommand("IUPatientPrescriptionNew", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                if (objmodel.PatientPrescriptionNewID != "" && objmodel.PatientPrescriptionNewID != null)
                {
                    cmd1.Parameters.AddWithValue("@PatientPrescriptionNewID", objmodel.PatientPrescriptionNewID);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@PatientPrescriptionNewID", 0);
                    cmd1.Parameters["@PatientPrescriptionNewID"].Direction = ParameterDirection.Output;
                }

                cmd1.Parameters.AddWithValue("@PatientRegNo", objmodel.PatientRegNO1);
                cmd1.Parameters.AddWithValue("@OPDIPDID", objmodel.OPDIPDID1);
                cmd1.Parameters.AddWithValue("@Date", objmodel.Date1);
                cmd1.Parameters.AddWithValue("@Height", objmodel.Height1);
                cmd1.Parameters.AddWithValue("@Weight", objmodel.Weight1);
                cmd1.Parameters.AddWithValue("@Pulse", objmodel.Pulse1);
                cmd1.Parameters.AddWithValue("@BP", objmodel.BP1);
                cmd1.Parameters.AddWithValue("@BMI", objmodel.BMI1);
                cmd1.Parameters.AddWithValue("@Temp", objmodel.Temp1);



                if (objmodel.ChiefComplaint1 != null)
                {
                    cmd1.Parameters.AddWithValue("@ChiefComplaint", WebUtility.HtmlEncode(objmodel.ChiefComplaint1).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ChiefComplaint", "");

                }

                if (objmodel.Allergies1 != null)
                {
                    cmd1.Parameters.AddWithValue("@Allergies", WebUtility.HtmlEncode(objmodel.Allergies1).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Allergies", "");

                }

                if (objmodel.ClinicalFinding1 != null)
                {
                    cmd1.Parameters.AddWithValue("@ClinicalFindings", WebUtility.HtmlEncode(objmodel.ClinicalFinding1).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ClinicalFindings", "");

                }

                if (objmodel.ProvitionalDiagnosis1 != null)
                {
                    cmd1.Parameters.AddWithValue("@ProvisionalDiagonosis", WebUtility.HtmlEncode(objmodel.ProvitionalDiagnosis1).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ProvisionalDiagonosis", "");

                }

                if (objmodel.Investigation1 != null)
                {
                    cmd1.Parameters.AddWithValue("@Investigations", WebUtility.HtmlEncode(objmodel.Investigation1).Replace("\r\n", "<br>"));

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Investigations", "");


                }
                if (objmodel.TreatmentAdvice1 != null)
                {
                    cmd1.Parameters.AddWithValue("@TreatmentAdvice", WebUtility.HtmlEncode(objmodel.TreatmentAdvice1).Replace("\r\n", "<br>"));
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@TreatmentAdvice", "");

                }


                cmd1.Parameters.AddWithValue("@Remark", objmodel.Remark1);
                cmd1.Parameters.AddWithValue("@FollowUpDate", objmodel.FollowUpDate1);
                cmd1.Parameters.AddWithValue("@FollowUpTime", Convert.ToDateTime(objmodel.FollowUpTime1).ToString("hh:mm:ss tt"));
                string date = Convert.ToDateTime(objmodel.FollowUpTime1).ToString("hh:mm:ss tt");

                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                if (objmodel.PatientPrescriptionNewID != "" && objmodel.PatientPrescriptionNewID != null)
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Edit");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                }

                con.Open();
                int i = cmd1.ExecuteNonQuery();

                con.Close();


                objmodel.PatientPrescriptionNewID = Convert.ToString(cmd1.Parameters["@PatientPrescriptionNewID"].Value);

                string[] Medicines1 = objmodel.Medicines1.Split(',');
                Medicines1 = Medicines1.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] Strength1 = objmodel.Strength1.Split(',');
                string[] Frequency1 = objmodel.Frequency1.Split(',');
                string[] Days1 = objmodel.Days1.Split(',');
                string[] Instruction1 = objmodel.Instruction1.Split(',');
                string[] MedicinesID1 = objmodel.MedicineLibraryID1.Split(',');




                for (int k = 0; k < Medicines1.Length; k++)
                {
                    if (Medicines1[k].ToString() != "")
                    {
                        #region IUPatientPrescriptionNewMedicine

                        SqlCommand cmd = new SqlCommand("IUPatientPrescriptionNewMedicine", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@PatientPrescriptionNewID", objmodel.PatientPrescriptionNewID);
                        cmd.Parameters.AddWithValue("@PatientPrescriptionNewMedicineID", 0);
                        cmd.Parameters["@PatientPrescriptionNewMedicineID"].Direction = ParameterDirection.Output;

                        cmd.Parameters.AddWithValue("@PatientRegNo", objmodel.PatientRegNO1);
                        cmd.Parameters.AddWithValue("@OPDIPDID", objmodel.OPDIPDID1);
                        cmd.Parameters.AddWithValue("@MedicineLibraryID", MedicinesID1[k]);

                        cmd.Parameters.AddWithValue("@Medicines", Medicines1[k]);
                        cmd.Parameters.AddWithValue("@Strength", Strength1[k]);
                        cmd.Parameters.AddWithValue("@Frequency", Frequency1[k]);
                        cmd.Parameters.AddWithValue("@Instruction", Instruction1[k]);
                        cmd.Parameters.AddWithValue("@Days", Days1[k]);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                        con.Open();
                        int j = cmd.ExecuteNonQuery();
                        con.Close();

                        #endregion
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public DataSet GetPatientPrescriptionNewOLDBillsNO(string patientReg, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientPrescriptionNewOLDBillsNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNo", patientReg);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public string GetPrintNo_ToRegNo(string PrintRegNO)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select*from Patient where  Patient.PrintRegNO=" + PrintRegNO + " and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string RegNo = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                RegNo = ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            }
            return RegNo;
        }

        public DataSet GetConsentMaster(int PatientRegNo)
        {
            DataSet ds = new DataSet();
            try
            {

                Connect();
                SqlParameter[] param = new SqlParameter[6];
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[3].Value = PatientRegNo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForPatientPrescriptionNew", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo,PrintRegNO,PatientName,PatientType from Patient where PatientName like '" + PatientName + '%' + "' and HospitalID = '" + HospitalID + "' and LocationID ='" + LocationID + "' and RowStatus = 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetPatientRegNo(int PatientRegNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO ,upper(Patient.PatientName) as PatientName ,Patient.PatientRegNO as RegNO, Patient.PatientType as PatientType,Patient.FinancialYearID from Patient where  Patient.PatientRegNO = " + PatientRegNo + " and Patient.HospitalID = '" + HospitalID + "' and Patient.LocationID = '" + LocationID + "' and Patient.RowStatus= 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
    }
}