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
   

    public class BL_ChiefCompliant
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ChiefComplaint> ChiefComplaintList = new List<ChiefComplaint>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public List<ChiefComplaint> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllChiefComplaint", con);
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
                ChiefComplaintList.Add(
                    new ChiefComplaint
                    {
                        ChiefComplaintID = Convert.ToInt32(dr["ChiefComplaintID"]),
                        ChiefComplaintName = Convert.ToString(dr["ChiefComplaint"]),
                        ChiefComplaintDescription = Convert.ToString(dr["ChiefComplaintDescription"]),
                    });
            }
            return ChiefComplaintList;
        }


        public List<ChiefComplaint> GetChiefComplaint(int ChiefComplaintID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetChiefComplaint", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ChiefComplaintID", ChiefComplaintID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                ChiefComplaintList.Add(
                    new ChiefComplaint
                    {
                        ChiefComplaintID = Convert.ToInt32(dr["ChiefComplaintID"]),
                        ChiefComplaintName = Convert.ToString(dr["ChiefComplaint"]),
                        ChiefComplaintDescription = Convert.ToString(dr["ChiefComplaintDescription"]),
                    });
            }
            return ChiefComplaintList;
        }



        public bool Save(ChiefComplaint obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUChiefComplaint", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ChiefComplaintID == 0)
            {
                cmd.Parameters.AddWithValue("@ChiefComplaintID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ChiefComplaintID", obj.ChiefComplaintID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }


            cmd.Parameters.AddWithValue("@ChiefComplaint", obj.ChiefComplaintName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            if (obj.ChiefComplaintDescription == null)
                cmd.Parameters.AddWithValue("@ChiefComplaintDescription", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ChiefComplaintDescription", obj.ChiefComplaintDescription);
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


        public bool CheckChiefComplaint(int ChiefComplaintID, string ChiefComplaintName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckChiefComplaint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ChiefComplaintID", ChiefComplaintID);
                //if (ChiefComplaintID != 0)
                //{
                    cmd.Parameters.AddWithValue("@ChiefComplaint", ChiefComplaintName.ToUpper());
                //}
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


        public string Delete(int ChiefComplaintID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteChiefComplaint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ChiefComplaintID", ChiefComplaintID);

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