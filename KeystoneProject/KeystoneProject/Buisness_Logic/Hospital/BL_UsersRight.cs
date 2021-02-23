using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Data;
using KeystoneProject.Models.Keystone;
namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_UsersRight
    {
        UserRights usersRight = new UserRights();
        //GetAllUsers
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        public static bool flag = false;
        public static int result = 0;
        public static string strResult = string.Empty;
        public string Mode = string.Empty;
        private SqlConnection con;
#pragma warning disable CS0169 // The field 'BL_UsersRight.ds' is never used
        DataSet ds;
#pragma warning restore CS0169 // The field 'BL_UsersRight.ds' is never used
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        public DataSet getAllUsers()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return ds;
            }
            return ds;
        }

        public DataSet GetHospitalLocation(int UserID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetHospitalLocationForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return ds;
            }
            return ds;
        }


        public DataSet GetRoleDataForUserRights(int UserID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetRoleDataForUserRights", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return ds;
            }
            return ds;
        }

        string Password2 = "";
        string Password = "";
        public void add(UserRights useRights, string[] parentwithIDs, int Number)
        {
           
           
            Connect();
            SqlCommand cmdIURoleRights = new SqlCommand("IUUserRights", con);
            cmdIURoleRights.CommandType = CommandType.StoredProcedure;
            cmdIURoleRights.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmdIURoleRights.Parameters.AddWithValue("@LocationID", LocationID);
            cmdIURoleRights.Parameters.AddWithValue("@UserID", useRights.UserID);
            cmdIURoleRights.Parameters.Add(new SqlParameter("@EffectiveDateFrom", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
            cmdIURoleRights.Parameters.Add(new SqlParameter("@EffectiveDateTo", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
            cmdIURoleRights.Parameters.AddWithValue("@ModuleID", parentwithIDs[0].ToString());
            cmdIURoleRights.Parameters.AddWithValue("@Rights", parentwithIDs[1].ToString());
            cmdIURoleRights.Parameters.AddWithValue("@UserRightsID", 0);

            cmdIURoleRights.Parameters.AddWithValue("@Password", Password);
            cmdIURoleRights.Parameters.AddWithValue("@Password2", Password2);

            //  cmdIURoleRights.Parameters.AddWithValue("@UserID", useRights.UserID);
            cmdIURoleRights.Parameters.AddWithValue("@FormMode", 0);
            cmdIURoleRights.Parameters.AddWithValue("@RowInternal", false);
            cmdIURoleRights.Parameters.AddWithValue("@IsAuthorised", 0);
            cmdIURoleRights.Parameters.AddWithValue("@CreationID", UserID);
            cmdIURoleRights.Parameters.AddWithValue("@RowStatus", Number);
            cmdIURoleRights.Parameters.AddWithValue("@Mode", useRights.Mode);
            con.Open();
            result = cmdIURoleRights.ExecuteNonQuery();
            con.Close();
        }

        public DataSet GetUserRightPassword(int UserID)
        { Connect();
            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter("select*from UserRights where UserID="+UserID+" and LocationID ="+LocationID+" and HospitalID="+HospitalID+" and RowStatus=0", con);
            con.Open();
            ad.Fill(ds);
            con.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Password2 = ds.Tables[0].Rows[0]["Password2"].ToString();
                Password = ds.Tables[0].Rows[0]["Password"].ToString();
            }
            return ds;
        }
    }
}