using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_ConsentPatient
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        List<ConsentPatient> ConsentPatientList = new List<ConsentPatient>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public DataSet GetAllFinancialYear()
        {
            Connect();
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    con.Open();
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllFinancialYear", param);
                    con.Close();
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        }

        public DataSet GetGetPatientConsent(int RegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientConsent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", RegNO);

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

        public List<ConsentPatient> GetConsentMaster()
        {
            List<KeystoneProject.Models.Patient.ConsentPatient> serachlist = new List<ConsentPatient>();

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllConsentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new KeystoneProject.Models.Patient.ConsentPatient
                    {
                        ConsentID  = dr["ConsentID"].ToString(),
                        ConsentName = dr["ConsentName"].ToString(),
                        Path = dr["Path"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
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

        public DataSet GetConsentMaster(int ConsentID)
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
                param[3] = new SqlParameter("@ConsentID", SqlDbType.Int);
                param[3].Value = ConsentID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetConsentMaster", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public List<ConsentPatient> GetAllConsentPatient()
        {
            List<ConsentPatient> searchList = new List<ConsentPatient>();
            Connect();

            SqlCommand cmd = new SqlCommand("GetAllConsentPatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new ConsentPatient
                    {
                        ConsentID = item["ConsentID"].ToString(),
                        ConsentName = item["ConsentName"].ToString(),
                        ConsentDetailID = item["ConsentDetailID"].ToString(),
                        PatientRegNo = item["PatientRegNO"].ToString(),
                        OPDIPDID = item["PatientOPDNO"].ToString(),
                        Footer = item["Footer"].ToString(),
                        PatientRegistrationDate = item["PatientRegistrationDate"].ToString(),
                        PatientType = item["PatientType"].ToString(),
                        ConsultantDrID = item["DoctorID"].ToString(),
                        ReferredByDoctorID = item["ReferredByDoctorID"].ToString(),
                    });


            }
            return searchList;

        }

        public bool Save(ConsentPatient obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUConsentPatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.ConsentDetailID == "0" || obj.ConsentDetailID == "" || obj.ConsentDetailID == null)
            {
                cmd.Parameters.AddWithValue("@ConsentDetailID", 0);
                cmd.Parameters["@ConsentDetailID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ConsentDetailID", obj.ConsentDetailID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");

            }


            cmd.Parameters.AddWithValue("@ConsentID", obj.ConsentName);

            if (obj.ConsentNameform == null)
                cmd.Parameters.AddWithValue("@ConsentName", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ConsentName", obj.ConsentNameform);

            if (obj.Description == null)
            {
                cmd.Parameters.AddWithValue("@Footer", "");
            }
            else
            {

                if (obj.Description == "<p><br></p>" || obj.Description == "<h1><br></h1>" || obj.Description == "<h2><br></h2>" || obj.Description == "<h3><br></h3>")
                {
                    cmd.Parameters.AddWithValue("@Footer", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Footer", (obj.Description));
                }

            }

            if (obj.PatientRegNo == null)
                cmd.Parameters.AddWithValue("@PatientRegNO", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNo);

            if (obj.OPDIPDID == null)
                cmd.Parameters.AddWithValue("@PatientOPDNO", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PatientOPDNO", obj.OPDIPDID);

            if (obj.opdIpdNumberID == null)
                cmd.Parameters.AddWithValue("@PrintOPDNo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PrintOPDNo", obj.opdIpdNumberID);

            if (obj.billDate == null)
                cmd.Parameters.AddWithValue("@PatientRegistrationDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PatientRegistrationDate", obj.billDate);

            if (obj.PatientType1 == null)
                cmd.Parameters.AddWithValue("@PatientType", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PatientType", obj.PatientType1);

            if (obj.TPA_ID == null)
                cmd.Parameters.AddWithValue("@TPA_ID", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@TPA_ID", obj.TPA_ID);


            if (obj.ConsultantDrID == null)
                cmd.Parameters.AddWithValue("@DoctorID", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@DoctorID", obj.ConsultantDrID);


            if (obj.DepartmentID == null)
                cmd.Parameters.AddWithValue("@DepartmentID", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);

            if (obj.ReferredDrID == null)
                cmd.Parameters.AddWithValue("@ReferredByDoctorID", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ReferredByDoctorID", obj.ReferredDrID);

            if (obj.Weight == null)
                cmd.Parameters.AddWithValue("@Weight", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Weight", obj.Weight);

            if (obj.BloodPressure == null)
                cmd.Parameters.AddWithValue("@BloodPressure", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BloodPressure", obj.BloodPressure);

 
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetPatientConsentOLDBillsNO(int PatientRegNO)
        {
            DataSet ds = new DataSet();
            Connect();
            try
            {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[0].Value = PatientRegNO;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientConsentOLDBillsNO", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetConsentPatient(int ConsentDetailID)
        {
            DataSet ds = new DataSet();
            Connect();
            try
            {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@ConsentDetailID", SqlDbType.Int);
                param[0].Value = ConsentDetailID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetConsentPatient", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public bool DeleteConsentPatient(int ConsentDetailID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@ConsentDetailID", SqlDbType.Int);
                aParams[1].Value = ConsentDetailID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteConsentPatient", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;

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