using KeystoneProject.Models.Hospital;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_ChangePassword
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<ChangePassword> li_pswd = new List<ChangePassword>();

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<ChangePassword> SelectAlluser()
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("select LoginName,UserID from Users where HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus=0 and loginname <> ''", con);
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    li_pswd.Add(
                        new ChangePassword
                        {
                            username = Convert.ToString(dr["LoginName"]),
                            usernameID = Convert.ToInt32(dr["UserID"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return li_pswd;

        }

        public DataTable GetUSer(string id)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@UserID", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public bool SAVE(ChangePassword obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("UpdatePassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.username == "" || obj.username == null)
            {
                cmd.Parameters.AddWithValue("@UserID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UserID", obj.username);
            }

            if (obj.oldpswd == "" || obj.oldpswd == null)
            {
                cmd.Parameters.AddWithValue("@OldPassword", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OldPassword", obj.oldpswd);
            }

            if (obj.newpswd == "" || obj.newpswd == null)
            {
                cmd.Parameters.AddWithValue("@NewPassword", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@NewPassword", obj.newpswd);
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

    }
}