using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.FinancialAccountReport
{
    public class BL_MISProfitAndLossReport
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connection()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }


        public DataSet GetExpensesDetails(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            DataSet ds = new DataSet();
            Connection();
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ScheduleID", SqlDbType.Int);
                param[2].Value = ScheduleID;
                param[3] = new SqlParameter("@ScheduleName", SqlDbType.NVarChar);
                param[3].Value = ScheduleName;
                param[4] = new SqlParameter("@Type", SqlDbType.NVarChar);
                param[4].Value = Type;
                param[5] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                param[5].Value = DateFrom;
                param[6] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                param[6].Value = DateTo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetExpensesDetails", param);
            }

            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;


        }
        public DataSet GetIncomeDetails(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            DataSet ds = new DataSet();
            Connection();
            try
            {

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ScheduleID", SqlDbType.Int);
                param[2].Value = ScheduleID;
                param[3] = new SqlParameter("@ScheduleName", SqlDbType.NVarChar);
                param[3].Value = ScheduleName;
                param[4] = new SqlParameter("@Type", SqlDbType.NVarChar);
                param[4].Value = Type;
                param[5] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                param[5].Value = DateFrom;
                param[6] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                param[6].Value = DateTo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetIncomeDetails", param);
            }

            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }
        public DataSet GetTrialBalance(DateTime FromDate, DateTime ToDate, string AccountsID, string Type, int ScheduleID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connection();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@toDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@AccountsID", SqlDbType.NVarChar, 50);
                param[4].Value = AccountsID;
                param[5] = new SqlParameter("@Type", SqlDbType.NVarChar, 20);
                param[5].Value = Type;
                param[6] = new SqlParameter("@ScheduleID", SqlDbType.Int);
                param[6].Value = ScheduleID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTrialBalance", param);
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