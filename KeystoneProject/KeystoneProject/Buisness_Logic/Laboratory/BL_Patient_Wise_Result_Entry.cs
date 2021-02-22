using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Laboratory
{
    public class BL_Patient_Wise_Result_Entry
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
             HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
             LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
             UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }
        public bool SaveRechk(DataSet dsPatientEntry,string col)
        {
            Connect();
            int PatientLabWithParameter = 0;
            con.Open();
          
            string Value = "";
            int row = 0;
            DataSet dsRechk = GetPatientLabWithParameterRecheck(Convert.ToInt32(dsPatientEntry.Tables[0].Rows[0]["LabNo"]), Convert.ToInt32(dsPatientEntry.Tables[0].Rows[0]["TestID"]));
            foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
            {

                SqlCommand cmdRecheck = new System.Data.SqlClient.SqlCommand("IUPatientLabWithParameterRecheck", con);
                cmdRecheck.CommandType = CommandType.StoredProcedure;
                cmdRecheck.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdRecheck.Parameters.AddWithValue("@LocationID", LocationID);
                cmdRecheck.Parameters.AddWithValue("@PatientLabParameterRecheckID", 0);
                cmdRecheck.Parameters.AddWithValue("@PatientLabParameterID", dr["PatientLabParameterID"].ToString());
                cmdRecheck.Parameters.AddWithValue("@LabNo", dr["LabNo"].ToString());
                cmdRecheck.Parameters.AddWithValue("@PatientLabDetailID", dr["PatientLabDetailID"].ToString());
                cmdRecheck.Parameters.AddWithValue("@TestID", dr["TestID"].ToString());
                cmdRecheck.Parameters.AddWithValue("@ParameterID", dr["ParameterID"].ToString());
                cmdRecheck.Parameters.AddWithValue("@NormalRangeID", dr["NormalRangeID"].ToString());
                cmdRecheck.Parameters.AddWithValue("@ResultValue", dr["ResultValue"].ToString());
                if (dsRechk.Tables[0].Rows.Count > 0)
                {
                    cmdRecheck.Parameters.AddWithValue("@Mode", "Edit");
                    if (col == "2")
                    {
                        cmdRecheck.Parameters.AddWithValue("@Value1", dr["ResultValue"].ToString());
                        //  cmdRecheck.Parameters.AddWithValue("@Value2", "");
                        //   cmdRecheck.Parameters.AddWithValue("@Value3", "");
                        Value = "1";
                    }
                    if (col == "3")
                    {
                        cmdRecheck.Parameters.AddWithValue("@Value1", dsRechk.Tables[0].Rows[row]["Value1"]);
                        cmdRecheck.Parameters.AddWithValue("@Value2", dr["ResultValue"].ToString());
                        cmdRecheck.Parameters.AddWithValue("@Value3", "");
                        Value = "2";
                    }
                    if (Value != "2" && Value != "1")
                    {
                        if (col == "4")
                        {
                            //  cmdRecheck.Parameters.AddWithValue("@Value1", "");
                            cmdRecheck.Parameters.AddWithValue("@Value2", dsRechk.Tables[0].Rows[row]["Value2"]);
                            cmdRecheck.Parameters.AddWithValue("@Value1", dsRechk.Tables[0].Rows[row]["Value1"]);
                            cmdRecheck.Parameters.AddWithValue("@Value3", dr["ResultValue"].ToString());

                        }
                    }


                }
                else
                {
                    cmdRecheck.Parameters.AddWithValue("@Value1", dr["ResultValue"].ToString());
                    cmdRecheck.Parameters.AddWithValue("@Value3", "");
                    cmdRecheck.Parameters.AddWithValue("@Value2", "");
                    cmdRecheck.Parameters.AddWithValue("@Mode", "Add");
                }


                cmdRecheck.Parameters.AddWithValue("@Status", dr["Status"].ToString());
                cmdRecheck.Parameters.AddWithValue("@NLH", dr["NLH"].ToString());
                cmdRecheck.Parameters.AddWithValue("@ConvLow", dr["ConvLow"].ToString());
                cmdRecheck.Parameters.AddWithValue("@ConvHigh", dr["ConvHigh"].ToString());
                cmdRecheck.Parameters.AddWithValue("@Remark", dr["Remark"].ToString());
                cmdRecheck.Parameters.AddWithValue("@CompleteBy", dr["CompleteBy"].ToString());
                cmdRecheck.Parameters.AddWithValue("@CreationID", UserID);

                //con.Open();
                PatientLabWithParameter = cmdRecheck.ExecuteNonQuery();
                row++;
            }
            return true;
        }
        public bool Save(DataSet dsPatientEntry, DataSet dscompare,string[] footer)
        {
            //string[] Footersplit = WebUtility.HtmlEncode(footer).Replace("\r\n", "<br>").Split(',');
            Connect();
            int PatientLabWithParameter = 0;
            con.Open();
            int rows = 0;
                foreach (DataRow dr in dsPatientEntry.Tables[0].Rows)
                {

                    SqlCommand cmd = new System.Data.SqlClient.SqlCommand("IUPatientLabWithParameter", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@PatientLabParameterID", dr["PatientLabParameterID"].ToString());
                    cmd.Parameters.AddWithValue("@LabNo", dr["LabNo"].ToString());
                    cmd.Parameters.AddWithValue("@PatientLabDetailID", dr["PatientLabDetailID"].ToString());
                    cmd.Parameters.AddWithValue("@TestID", dr["TestID"].ToString());
                    cmd.Parameters.AddWithValue("@ParameterID", dr["ParameterID"].ToString());
                    cmd.Parameters.AddWithValue("@NormalRangeID", dr["NormalRangeID"].ToString());
                    cmd.Parameters.AddWithValue("@ResultValue", dr["ResultValue"].ToString());
                    cmd.Parameters.AddWithValue("@Status", dr["Status"].ToString());
                    cmd.Parameters.AddWithValue("@NLH", dr["NLH"].ToString());
                    cmd.Parameters.AddWithValue("@ConvLow", dr["ConvLow"].ToString());
                    cmd.Parameters.AddWithValue("@ConvHigh", dr["ConvHigh"].ToString());
                    cmd.Parameters.AddWithValue("@Remark", dr["Remark"].ToString());
                    cmd.Parameters.AddWithValue("@Footer", WebUtility.HtmlEncode(footer[rows].ToString()).Replace("\r\n", "<br>"));
                    cmd.Parameters.AddWithValue("@CompleteBy", dr["CompleteBy"].ToString());
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                    PatientLabWithParameter = cmd.ExecuteNonQuery();
                    rows++;

                    
                }
                con.Close();
            
            return true;
        
    }

        public DataSet GetPatientLabWithParameterRecheck(int LabNo, int TestID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlDataAdapter ad = new SqlDataAdapter("select*from PatientLabWithParameterRecheck where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "'  and  LabNo='" + LabNo + "' and TestID='" + TestID + "'", con);
                con.Open();
                ad.Fill(ds);
              //  con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }
        public DataSet GetCheckAuthorized()
        {


            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetCheckAuthorized", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }


     
        public DataSet GetParamaterHelpValue(int ParameterID)
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
                param[3] = new SqlParameter("@ParameterID", SqlDbType.Int);
                param[3].Value = ParameterID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetParamaterHelpValue", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetParameterOnlyFormulaParameter( int ParameterID)
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
                param[2] = new SqlParameter("@ParameterID", SqlDbType.Int);
                param[2].Value = ParameterID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetParameterOnlyFormulaParameter", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetParameterOnlyFormula( int ParameterID)
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
                param[2] = new SqlParameter("@ParameterID", SqlDbType.Int);
                param[2].Value = ParameterID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetParameterOnlyFormula", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetPatientWiseResultforParameterValue(int LabNo, int TestID, int HospitalID, int LocationID, int UserID)
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseResultforParameterValue", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientWiseResultforRecheckParameterValue(int LabNo, int TestID, int HospitalID, int LocationID, int PatientLabDetailID)
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
                param[5] = new SqlParameter("@PatientLabDetailID", SqlDbType.Int);
                param[5].Value = PatientLabDetailID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseResultforRecheckParameterValue", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientWiseResultForPatientSearchReg(int PatientRegNo)
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
                param[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[3].Value = PatientRegNo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseResultForPatientSearchReg", param);
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


        public DataSet GetPatientWiseResultforParameterValue(int LabNo, int TestID)
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientWiseResultforParameterValue", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
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
                //  dv = new DataView(ds.Tables[0], "[LAB DATE]  >='" + DateFrom.ToString("dd-MM-yyyy HH:mm:ss") + "' And [LAB DATE] <='" + DateTo.ToString("dd-MM-yyyy HH:mm:ss") + "'", "", DataViewRowState.CurrentRows);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;

            }
            return ds;
        }
        public DataSet ValidateUser(string LoginName, string Password)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[4];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@UserName", SqlDbType.VarChar, 25);
                aParams[2].Value = LoginName;
                aParams[3] = new SqlParameter("@Password", SqlDbType.VarChar, 255);
                aParams[3].Value = Password;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ValidateUser", aParams);
            }

            catch (Exception exp)
            {
                ExceptionManager.Publish(exp);
                throw exp;
            }
        }

        public DataSet ValidateUser(string LoginName)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@UserName", SqlDbType.VarChar, 25);
                aParams[2].Value = LoginName;

                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ValidateUser", aParams);
            }

            catch (Exception exp)
            {
                ExceptionManager.Publish(exp);
                throw exp;
            }
        }

        public DataSet GetPatientLabForAuthorizer(int UserID, int LabNo, int TestID)
        {
               Connect();
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    param[2] = new SqlParameter("@UserID", SqlDbType.Int);
                    param[2].Value = UserID;
                    param[3] = new SqlParameter("@LabNo", SqlDbType.Int);
                    param[3].Value = LabNo;
                    param[4] = new SqlParameter("@TestID", SqlDbType.Int);
                    param[4].Value = TestID;

                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientLabForAuthorizer", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            
        }
        public bool IUPatientLabAuthorization(string AuthorzerID, string AuthorzeName, string stauts, string Remark, string LabNo, string TestID, string TestName, string Mode)
        {
            Connect();
                            SqlCommand cmd = new System.Data.SqlClient.SqlCommand("IUPatientLabAuthorization", con);

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd.Parameters.AddWithValue("@AuthorizationID", 0);
                            cmd.Parameters.AddWithValue("@UserID", AuthorzerID);
                            cmd.Parameters.AddWithValue("@AuthoriserName", AuthorzeName);
                            cmd.Parameters.AddWithValue("@Status", stauts);
                            cmd.Parameters.AddWithValue("@Remark", Remark);
                            cmd.Parameters.AddWithValue("@LabNo", LabNo);
                            cmd.Parameters.AddWithValue("@TestID", TestID);
                            cmd.Parameters.AddWithValue("@TestName", TestName);
                            cmd.Parameters.AddWithValue("@CreationID", UserID);
                            con.Open();
                            cmd.Parameters.AddWithValue("@Mode", Mode);
                        var a=   cmd.ExecuteNonQuery();
            con.Close();
            if(a>0)
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