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
    public class BL_Department
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Department> DepartmentList = new List<Department>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


       

        public List<Department> SelectAllData()
        {
            Connect();
           

            SqlCommand cmd = new SqlCommand("GetAllDepartment", con);
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
                DepartmentList.Add(
                    new Department
                    {
                        DepartmentID = Convert.ToInt32(dr["DepartmentID"]),
                        DepartmentName = Convert.ToString(dr["DepartmentName"]),
                        Description = Convert.ToString(dr["Description"]),
                    });
            }
            return DepartmentList;
        }


        public List<Department> GetDepartment(int DepartmentID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                DepartmentList.Add(
                    new Department
                    {
                        DepartmentID = Convert.ToInt32(dr["DepartmentID"]),
                        DepartmentName = Convert.ToString(dr["DepartmentName"]),
                        Description = Convert.ToString(dr["Description"]),
                    });
            }
            return DepartmentList;
        }


        public bool Save(Department obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.DepartmentID == 0)
            {
                cmd.Parameters.AddWithValue("@DepartmentID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@DepartmentName", obj.DepartmentName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            if (obj.Description == null)
                cmd.Parameters.AddWithValue("@Description", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Description", obj.Description);
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

        public bool CheckDepartment(int DepartmentID, string DepartmentName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                cmd.Parameters.AddWithValue("@DepartmentName", DepartmentName.ToUpper());
                
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

        public string Delete(int DepartmentID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
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

    }
}