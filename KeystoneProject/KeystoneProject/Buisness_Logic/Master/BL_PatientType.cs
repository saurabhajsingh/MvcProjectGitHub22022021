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
using KeystoneProject.Buisness_Logic.Master;

namespace KeystoneProject.Buisness_Logic.Master
{


    public class BL_PatientType
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

        public List<PatientType> SelectAllData()
        {
            Connect();
            List<PatientType> PatientTypeList = new List<PatientType>();

            SqlCommand cmd = new SqlCommand("GetAllPatientType1", con);
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
                PatientTypeList.Add(
                    new PatientType
                    {
                        PatientTypeID = (dr["PatientTypeID"]).ToString(),
                        PatientTypeName = Convert.ToString(dr["PatientType"]),

                    });
            }
            return PatientTypeList;
        }

        public PatientType GetPatientType(int PatientTypeID)
        {
            Connect();
            PatientType obj = new PatientType();

            SqlCommand cmd = new SqlCommand("GetPatientType1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PatientTypeID", PatientTypeID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                obj.PatientTypeID = (dr["PatientTypeID"]).ToString();
                obj.PatientTypeName = Convert.ToString(dr["PatientType"]);

            }

            return obj;
        }


        public bool Save(PatientType obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUPatientType1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.PatientTypeID =="" || obj.PatientTypeID==null)
            {
                cmd.Parameters.AddWithValue("@PatientTypeID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PatientTypeID", obj.PatientTypeID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@PatientType", obj.PatientTypeName);

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
        public bool DeletePatientType(int PatientTypeID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@PatientTypeID", SqlDbType.Int);
                aParams[1].Value = PatientTypeID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientType1", aParams);
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
        public bool CheckPatientType(string PatientTypeID, string PatientTypeName)
        {
            string t = "";
            if(PatientTypeID=="" || PatientTypeID == null)
            {
                t = "0";
            }
            else
            {
                t = PatientTypeID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPatientType1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@PatientTypeID", t);
                cmd.Parameters.AddWithValue("@PatientType", PatientTypeName.ToUpper());
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