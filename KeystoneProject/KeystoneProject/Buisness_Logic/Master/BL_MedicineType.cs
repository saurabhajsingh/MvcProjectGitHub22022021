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
    public class BL_MedicineType
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicineType> MedicineTypeList = new List<MedicineType>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public bool Save(MedicineType obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUMedicineType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.MedicineTypeID == 0)
            {
                cmd.Parameters.AddWithValue("@MedicineTypeID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@MedicineTypeID", obj.MedicineTypeID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            if (obj.MedicineTypeName == null)
            {
                cmd.Parameters.AddWithValue("@MedicineTypeName", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MedicineTypeName", obj.MedicineTypeName);
            }

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

        public List<MedicineType> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllMedicineType", con);
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
                MedicineTypeList.Add(
                    new MedicineType
                    {
                        MedicineTypeID = Convert.ToInt32(dr["MedicineTypeID"]),
                        MedicineTypeName = Convert.ToString(dr["MedicineTypeName"]),

                    });
            }
            return MedicineTypeList;
        }

        public bool CheckMedicineType(int MedicineTypeID, string MedicineTypeName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckMedicineType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@MedicineTypeID", MedicineTypeID);
                cmd.Parameters.AddWithValue("@MedicineTypeName", MedicineTypeName.ToUpper());
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
        public List<MedicineType> GetMedicineType(int MedicineTypeID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetMedicineType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MedicineTypeID", MedicineTypeID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                MedicineTypeList.Add(
                    new MedicineType
                    {
                        MedicineTypeID = Convert.ToInt32(dr["MedicineTypeID"]),
                        MedicineTypeName = Convert.ToString(dr["MedicineTypeName"]),

                    });
            }
            return MedicineTypeList;
        }

        public string Delete(int MedicineTypeID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteMedicineType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MedicineTypeID", MedicineTypeID);
                //cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                //cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;

                con.Open();
                int i = cmd.ExecuteNonQuery();

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

    }
}