using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_EndResult
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

        public List<EndResult> SelectAllData()
        {
            Connect();
            List<EndResult> EndResultList = new List<EndResult>();

            SqlCommand cmd = new SqlCommand("GetAllEndResult", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                EndResultList.Add(
                    new EndResult
                    {
                        EndResultID = Convert.ToInt32(dr["EndResultID"]),
                        EndResultName = Convert.ToString(dr["EndResultName"]),

                    });
            }
            return EndResultList;
        }

        public EndResult GetEndResult(int EndResultID)
        {
            Connect();
            EndResult reason = new EndResult();

            SqlCommand cmd = new SqlCommand("GetEndResult", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EndResultID", EndResultID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                reason.EndResultID = Convert.ToInt32(dr["EndResultID"]);
                reason.EndResultName = Convert.ToString(dr["EndResultName"]);

            }

            return reason;
        }


        public bool Save(EndResult obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUEndResult", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.EndResultID == 0)
            {
                cmd.Parameters.AddWithValue("@EndResultID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@EndResultID", obj.EndResultID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@EndResultName", obj.EndResultName);

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

        #region Delete
        public bool DeleteEndResult(int EndResultID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@EndResultID", SqlDbType.Int);
                aParams[1].Value = EndResultID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteEndResult", aParams);
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
        #endregion
        public bool CheckEndResult(int EndResultID, string EndResultName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckEndResult", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@EndResultID", EndResultID);
                cmd.Parameters.AddWithValue("@EndResultName", EndResultName.ToUpper());
                cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@NameExists"].Value;
                if (Convert.ToInt32(Table) == 1)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}