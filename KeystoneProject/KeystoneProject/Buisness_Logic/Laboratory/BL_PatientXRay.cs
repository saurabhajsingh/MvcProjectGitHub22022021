using KeystoneProject.Models.Laboratory;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Laboratory
{
    public class BL_PatientXRay
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        public DataSet GetPatientRegNo(int RegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientForReg", con);
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
            catch (Exception )
            {
                return ds;
            }

        }


        public DataSet GetPatient(string PatientRegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO ,upper(Patient.PatientName) as PatientName ,Patient.PatientRegNO as RegNO, Patient.PatientType as 'PatientType',Patient.FinancialYearID from Patient where Patient.PatientName like '" + PatientRegNO + "%' and Patient.HospitalID = " + HospitalID + " and Patient.LocationID = " + LocationID + " and Patient.RowStatus= 0 group by Patient.PatientRegNO,upper(Patient.PatientName)   ,Patient.PrintRegNO, Patient.PatientType,Patient.FinancialYearID ORDER BY Patient.PatientRegNO Desc", con);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet SearchPatientLabResultEntryByNameID(string Patientname, DateTime DateFrom, DateTime DateTo)
        {
            DataView dv = new DataView();
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[9];
                aParams[0] = new SqlParameter("@LabNo", SqlDbType.VarChar, 100);
                aParams[0].Value = '%';
                aParams[1] = new SqlParameter("@PatientName", SqlDbType.VarChar, 100);
                aParams[1].Value = Patientname;
                aParams[2] = new SqlParameter("@ReferenceCode", SqlDbType.VarChar, 20);
                aParams[2].Value = "%";
                aParams[4] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[4].Value = LocationID;
                aParams[5] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                aParams[5].Value = Convert.ToDateTime(DateFrom);
                aParams[6] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                aParams[6].Value = Convert.ToDateTime(DateTo);

                aParams[7] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[7].Value = HospitalID;
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchPatientLabResultEntryByNameID", aParams);

                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;

            }
            return ds;
        }

        public DataSet GetPatientWiseResultForPatientSearch(int LabNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@LabNo", SqlDbType.Int);
                param[3].Value = @LabNo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseResultForPatientSearch", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientWiseXrayResult(int LabNo, int TestID)
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
                param[3] = new SqlParameter("@TestID", SqlDbType.Int);
                param[3].Value = TestID;
                param[4] = new SqlParameter("@LabNo", SqlDbType.Int);
                param[4].Value = LabNo;
                param[5] = new SqlParameter("@UserID", SqlDbType.Int);
                param[5].Value = UserID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseXrayResult", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public bool SaveXRay(PatientXRay obj)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUPatientLabWithParameter", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.PatientLabParameterID == 0)
            {

                cmd.Parameters.AddWithValue("@PatientLabParameterID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PatientLabParameterID", obj.PatientLabParameterID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            cmd.Parameters.AddWithValue("@LabNo",obj.LabNo);
            cmd.Parameters.AddWithValue("@PatientLabDetailID",0);
            cmd.Parameters.AddWithValue("@TestID", obj.TestID);
            cmd.Parameters.AddWithValue("@ParameterID", 0);
            cmd.Parameters.AddWithValue("@NormalRangeID", 0);
            cmd.Parameters.AddWithValue("@ResultValue", "");
            cmd.Parameters.AddWithValue("@Status", "");
            cmd.Parameters.AddWithValue("@NLH", "");
            cmd.Parameters.AddWithValue("@ConvLow", "");
            cmd.Parameters.AddWithValue("@ConvHigh", "");
            cmd.Parameters.AddWithValue("@Remark", "");

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

            cmd.Parameters.AddWithValue("@CompleteBy", "");
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
    } 
}