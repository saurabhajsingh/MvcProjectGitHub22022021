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
    public class BL_Reason
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

        public List<Reason> SelectAllData()
        {
            Connect();
            List<Reason> ReasonList = new List<Reason>();

            SqlCommand cmd = new SqlCommand("GetAllReason", con);
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
                ReasonList.Add(
                    new Reason
                    {
                        ReasonID = Convert.ToInt32(dr["ReasonID"]),
                        ReasonName = Convert.ToString(dr["ReasonName"]),
                       
                    });
            }
            return ReasonList;
        }

        //public string Delete(int ReasonID)
        //{
        //    string Table = string.Empty;

        //    try
        //    {
        //        Connect();
        //        SqlCommand cmd = new SqlCommand("DeleteQualification", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ReasonID", ReasonID);
        //        cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 500);
        //        cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
        //        con.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        Table = (string)cmd.Parameters["@TableName"].Value;
        //        con.Close();
        //        if (i >= 1)
        //        {
        //            return Table;
        //        }
        //        else
        //        {
        //            return Table;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public Reason GetReason(int ReasonID)
        {
            Connect();
            Reason reason = new Reason();

            SqlCommand cmd = new SqlCommand("GetReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ReasonID", ReasonID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                reason.ReasonID = Convert.ToInt32(dr["ReasonID"]);
                reason.ReasonName = Convert.ToString(dr["ReasonName"]);
                 
            }

            return reason;
        }


        public bool Save(Reason obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ReasonID == 0)
            {
                cmd.Parameters.AddWithValue("@ReasonID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReasonID", obj.ReasonID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReasonName", obj.ReasonName);
            
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
        public bool DeleteReason(int ReasonID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@ReasonID", SqlDbType.Int);
                aParams[1].Value = ReasonID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteReason1", aParams);
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
        public bool CheckReason(int ReasonID, string ReasonName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckReason1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ReasonID", ReasonID);
                cmd.Parameters.AddWithValue("@ReasonName", ReasonName.ToUpper());
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