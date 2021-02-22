using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ForAuthorization
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }
        public DataSet StoreAllForAuthorization(DateTime DateFrom, DateTime DateTo)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetDetailsForAuthorization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
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
        public DataSet GetDetailsBeforeAfterAuthorization( int PatientRegNo, int BillNo, int AuthorizationID, string PatientAccountRowID)
        {
            DataSet ds = new DataSet();
            try
            {
                if(PatientAccountRowID=="  ")
                {
                    PatientAccountRowID = "0";
                }
                Connect();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[2].Value = PatientRegNo;
                param[3] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[3].Value = BillNo;
                param[4] = new SqlParameter("@AuthorizationID", SqlDbType.Int);
                param[4].Value = AuthorizationID;
                param[5] = new SqlParameter("@PatientAccountRowID", SqlDbType.Int);
                param[5].Value = PatientAccountRowID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetDetailsBeforeAfterAuthorization", param);

            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return ds;
        }
        #region   Edit After Authorization

        public int IUAfterAuthorization(int PatientRegNo, int BillNo, int AuthorizationID, string PatientAccountRowID,string AuthorizedRemark)
        {
            int row = 0;
            try
            {
                Connect();
                if (PatientAccountRowID == "  ")
                {
                    PatientAccountRowID = "0";
                }


                SqlCommand cmdIUForAuthorization = new SqlCommand("IUAfterAuthorization", con);
                        cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                        cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                        // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                        cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", BillNo);
                        cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", AuthorizationID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizedBy", UserID);
                      
                        cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizedRemark", AuthorizedRemark);
                        cmdIUForAuthorization.Parameters.AddWithValue("@Authorise", "Accept");
                        cmdIUForAuthorization.Parameters.AddWithValue("@PatientAccountRowID", PatientAccountRowID);
                        
                        cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Edit");
                        con.Open();
                row= cmdIUForAuthorization.ExecuteNonQuery();

                    

                    

                }
            
         
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
               
                throw ex;
            }

            return row;// DoctorID;
        }

        # endregion
    }
}