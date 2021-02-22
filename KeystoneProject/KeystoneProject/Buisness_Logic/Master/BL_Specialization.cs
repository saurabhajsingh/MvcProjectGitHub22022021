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
    public class BL_Specialization
    {

        int HospitalID= 0;
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

        #region CheckSpecialization
        public bool CheckSpecialization(int SpecializationID, string SpecializationName)
        {
            Connect();
            string Table;
            bool flag;
            SqlCommand cmd = new SqlCommand("CheckSpecialization", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@SpecializationID", SpecializationID);
            cmd.Parameters.AddWithValue("@SpecializationName", SpecializationName.ToUpper());
            
            
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

        #region SaveSpecialization
        public bool Save(Specialization objSpecial1)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUSpecialization", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            //  cmd.Parameters.AddWithValue("@SpecializationID", 0);
            if (objSpecial1.SpecializationID == 0)
            {
                cmd.Parameters.AddWithValue("@SpecializationID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@SpecializationID", objSpecial1.SpecializationID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@SpecializationName", objSpecial1.SpecializationName);
            if (Convert.ToString(objSpecial1.Description) == null)
                cmd.Parameters.AddWithValue("@Description", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Description", objSpecial1.Description);
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            //  cmd.Parameters.AddWithValue("@Mode", "ADD");
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

        #region SelectAllSpecialization
        public DataSet SelectAllSpecialization()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllSpecialization", con);
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

        #region SelectSpecializationbyID
        public List<Specialization> SelectSpecializationbyID(int SpecializationID)
        {
                Connect();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("GetSpecialization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpecializationID", SpecializationID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                List<Specialization> SpecializationList = new List<Specialization>();
                DataTable dt = new DataTable();
                con.Open();
                sd.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    SpecializationList.Add(
                        new Specialization
                        {
                            SpecializationID = Convert.ToInt32(dr["SpecializationID"]),
                            SpecializationName = Convert.ToString(dr["SpecializationName"]),
                            Description = Convert.ToString(dr["Description"]),
                        });
                }
                return SpecializationList;
        }

        #endregion

        #region UpdateSpecializationData
        //public bool UpdateSpecializationData(Specialization SpecialM)
        //{
        //    SqlConnection con = null;
        //    string result = "";
        //    try
        //    {
        //        Connect();
        //        con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
        //        SqlCommand cmd = new SqlCommand("IUSpecialization", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
        //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        //        cmd.Parameters.AddWithValue("@SpecializationID", SpecialM.SpecializationID);
        //        cmd.Parameters.AddWithValue("@SpecializationName", SpecialM.SpecializationName);
        //        if (Convert.ToString(SpecialM.Description) == null)
        //            cmd.Parameters.AddWithValue("@Description", DBNull.Value);
        //        else
        //            cmd.Parameters.AddWithValue("@Description", SpecialM.Description.ToUpper());
        //        cmd.Parameters.AddWithValue("@ReferenceCode", 1);
        //        cmd.Parameters.AddWithValue("@CreationID", UserID);
        //        cmd.Parameters.AddWithValue("@Mode", "Edit");
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        result = cmd.ToString();

        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }
           
        //}

        #endregion

        #region DeleteSpecialization
        public string DeleteSpecialization(int SpecializationID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteSpecialization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpecializationID", SpecializationID);
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