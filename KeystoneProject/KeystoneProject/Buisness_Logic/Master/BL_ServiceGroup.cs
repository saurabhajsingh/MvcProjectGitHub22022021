using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ServiceGroup
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
        #region CheckServiceGroup
        public bool CheckServiceGroup(int ServiceGroupID, string ServiceGroupName)
        {
            Connect();
            string Table;
            bool flag;
            SqlCommand cmd = new SqlCommand("CheckServiceGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID);
            cmd.Parameters.AddWithValue("@ServiceGroupName", ServiceGroupName);
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

        #endregion

        #region SaveServiceGroup
        public bool SaveServiceGroup(ServiceGroup obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUServiceGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ServiceGroupID", 0);
            cmd.Parameters.AddWithValue("@ServiceType", obj.ServiceType);
            cmd.Parameters.AddWithValue("@ReferenceCode", "");
            cmd.Parameters.AddWithValue("@ServiceGroupName", obj.ServiceGroupName);
            if (obj.HSNCode == "" || obj.HSNCode == null)
            {
               cmd.Parameters.AddWithValue("@HSNCode",0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode);
           
            }
            cmd.Parameters.AddWithValue("@ServicesOrder", obj.ServicesOrder);
            if (Convert.ToString(obj.SubDirectry) == null)
                cmd.Parameters.AddWithValue("@SubDirectry", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@SubDirectry", obj.SubDirectry);
            if (Convert.ToString(obj.HideInBilling) == null)
                cmd.Parameters.AddWithValue("@HideInBilling", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@HideInBilling", obj.HideInBilling);
            cmd.Parameters.AddWithValue("@DoctorID", 0);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "Add");
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

        #endregion

        #region SelectAllServiceGroup
        public DataSet SelectAllServiceGroup()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServiceGroup", con);
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

        #endregion

        #region SelectServiceGroupbyID
        public DataSet SelectServiceGroupbyID(int ServiceGroupID)
        {
            Connect();
            DataSet ds = null;
            try
            {
               
                SqlCommand cmd = new SqlCommand("GetServiceGroupName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID); // i will pass zero to MobileID beacause its Primary .
                //cmd.Parameters.AddWithValue("@RegisterName", null);
                //cmd.Parameters.AddWithValue("@Remark", null);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

        #region  UpdateServiceGroupData
        public bool UpdateServiceGroupData(ServiceGroup ServiceGpM)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("IUServiceGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGpM.ServiceGroupID);
                cmd.Parameters.AddWithValue("@ServiceGroupName", ServiceGpM.ServiceGroupName);
                cmd.Parameters.AddWithValue("@ServiceType", ServiceGpM.ServiceType);
                cmd.Parameters.AddWithValue("@ServicesOrder", ServiceGpM.ServicesOrder);
                cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                if (ServiceGpM.HSNCode == "" || ServiceGpM.HSNCode == null)
                {
                    cmd.Parameters.AddWithValue("@HSNCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNCode", ServiceGpM.HSNCode);

                }
                if (Convert.ToString(ServiceGpM.SubDirectry) == null)
                    cmd.Parameters.AddWithValue("@SubDirectry", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SubDirectry", ServiceGpM.SubDirectry);
                if (Convert.ToString(ServiceGpM.HideInBilling) == null)
                    cmd.Parameters.AddWithValue("@HideInBilling", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@HideInBilling", ServiceGpM.HideInBilling);
                cmd.Parameters.AddWithValue("@DoctorID", 0);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                con.Open();
                cmd.ExecuteNonQuery();
                result = cmd.ToString();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

        #region DeleteServiceGroup
        public string DeleteServiceGroup(int ServiceGroupID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteServiceGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID);
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@TableName"].Value;
                con.Close();
                if (i > 0)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
            catch (Exception)
            {
                return Table;
            }
        }

        #endregion
    }
}