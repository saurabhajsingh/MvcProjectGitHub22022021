using KeystoneProject.Models.FinancialAccount;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.FinancialAccount
{
    public class BL_ChequeLayout
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<AccountStatement> accountstatement = new List<AccountStatement>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        #region Get Account Group Data
        public DataSet GetChequeLayout(int HospitalID, int LocationID, int ChequeLayoutID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ChequeLayoutID", SqlDbType.Int);
                param[2].Value = ChequeLayoutID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetChequeLayout", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion
        #region Get All Account Group Data
        public DataSet GetAllChequeLayout(int HospitalID, int LocationID)
        {
            try
            {
                Connect();
                
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllChequeLayout", param);
                
            }
          
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        # endregion

        #region Get Account Group Data
        public DataSet GetChequeLayout(int ChequeLayoutID)
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
                param[2] = new SqlParameter("@ChequeLayoutID", SqlDbType.Int);
                param[2].Value = ChequeLayoutID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetChequeLayout", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion


    }
}