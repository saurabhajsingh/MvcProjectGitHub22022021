using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ComplaintForOPD
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

        public List<ComplaintForOPD> SelectAllData()
        {
            Connect();
            List<ComplaintForOPD> ComplaintList = new List<ComplaintForOPD>();

            SqlCommand cmd = new SqlCommand("GetAllComplaintMaster", con);
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
                ComplaintList.Add(
                    new ComplaintForOPD
                    {
                        ComplaintID = Convert.ToInt32(dr["ComplaintID"]),
                        ComplaintName = Convert.ToString(dr["ComplaintName"]),

                    });
            }
            return ComplaintList;
        }


        public ComplaintForOPD GetComplaint(int ComplaintID)
        {
            Connect();
            ComplaintForOPD complaint = new ComplaintForOPD();

            SqlCommand cmd = new SqlCommand("GetComplaintMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ComplaintID", ComplaintID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                complaint.ComplaintID = Convert.ToInt32(dr["ComplaintID"]);
                complaint.ComplaintName = Convert.ToString(dr["ComplaintName"]);

            }

            return complaint;
        }


        public bool Save(ComplaintForOPD obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUComplaintMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ComplaintID == 0)
            {
                cmd.Parameters.AddWithValue("@ComplaintID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ComplaintID", obj.ComplaintID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ComplaintName", obj.ComplaintName);

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


        public bool CheckComplaint(int ComplaintID, string ComplaintName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckComplaintMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                //cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ComplaintID", ComplaintID);
                cmd.Parameters.AddWithValue("@ComplaintName", ComplaintName.ToUpper());
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