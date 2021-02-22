using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using KeystoneProject.Models.Keystone;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_Users
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int CreationID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        int HospitalName = Convert.ToInt32(HttpContext.Current.Session["HospitalName"]);
        int ReferenceCode = Convert.ToInt32(HttpContext.Current.Session["ReferenceCode"]);
        private SqlConnection con;
        string Mode = "";
        public void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(constring);
        }
        DataSet dsRoleRight = new DataSet();
        public bool Save(Users users)
        {
            try
            {
                Connect();
                con.Open();
                SqlCommand cmd = new SqlCommand("IUUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                if (users.UserID == 0)
                {
                    cmd.Parameters.AddWithValue("@UserID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                    cmd.Parameters["@UserID"].Direction = ParameterDirection.Output;
                    Mode = "Add";
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UserID", users.UserID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    Mode = "Edit";
                }
                cmd.Parameters.AddWithValue("@EmployeeID", 0);
                cmd.Parameters.AddWithValue("@LoginName", users.LoginName);
                cmd.Parameters.AddWithValue("@EmailID", users.EmailID);
                cmd.Parameters.AddWithValue("@Password", users.Password);
                cmd.Parameters.AddWithValue("@Password2", users.Password2);
                //cmd.Parameters.AddWithValue("@IsAccountLocked", users.IsAccountLocked);
                //cmd.Parameters.AddWithValue("@ADSIObjectID", users.ADSIObjectID);
                cmd.Parameters.AddWithValue("@FullName", users.FullName);
                //cmd.Parameters.AddWithValue("@MaskColor", users.MaskColor);
                cmd.Parameters.AddWithValue("@RowInternal", 0);
                cmd.Parameters.AddWithValue("@RoleID", users.RoleID);
                cmd.Parameters.AddWithValue("@RoleName", users.RoleName);
                cmd.Parameters.AddWithValue("@CreationID", CreationID);

                cmd.ExecuteNonQuery();
                users.UserID = Convert.ToInt32(cmd.Parameters["@UserID"].Value);
                int UserID = Convert.ToInt32(cmd.Parameters["@UserID"].Value);
                //   con.Close();
                //   Convert.ToInt32(ds.Tables[0].Rows[0]["RoleID"].ToString());
                string RoleID = GetUsers(UserID).Tables[0].Rows[0]["RoleID"].ToString();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("IUUsersDetails", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                cmd1.Parameters.AddWithValue("@UserID", UserID);
                cmd1.Parameters.AddWithValue("@AuthorizationRights", Convert.ToBoolean( users.chbAuthorizationRights));
                if (Mode == "Add")
                {
                    cmd1.Parameters.AddWithValue("@UserDetailsID", 0);
                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                    cmd1.Parameters.AddWithValue("@RoleForEdit", "Old");
                    //  cmd1.Parameters["@UserID"].Direction = ParameterDirection.Output;
                    Mode = "Add";
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@UserDetailsID", users.UserDetailsID);
                    if (RoleID == users.RoleID.ToString())
                    {
                        cmd1.Parameters.AddWithValue("@RoleForEdit", "Old");
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@RoleForEdit", "New");
                    }
                    cmd1.Parameters.AddWithValue("@Mode", "Edit");

                    Mode = "Edit";
                }
                cmd1.Parameters.AddWithValue("@RoleID", users.RoleID);
                cmd1.Parameters.AddWithValue("@RoleID2", users.RoleID2);
                cmd1.Parameters.AddWithValue("@RowInternal", 0);

                cmd1.Parameters.AddWithValue("@CreationID", UserID);


                int row = cmd1.ExecuteNonQuery();




                string[] roleID = new string[2];
                string[] Password = new string[2];
                if (users.RoleID == 1)
                {
                    if (users.RoleID2 != "")
                    {
                        roleID[0] = users.RoleID.ToString();
                        roleID[1] = users.RoleID2;
                        Password[0] = users.Password.ToString();
                        Password[1] = users.Password2;
                        for (int j = 0; j < roleID.Length; j++)
                        {

                            if (j == 0)
                            {
                                users.Password2 = "";
                            }
                            else
                            {
                                users.Password2 = Password[1];
                                users.Password = "";
                            }

                            dsRoleRight = GetEmptyUserForRoleRight(Convert.ToInt32(roleID[j]));
                            //  con.Open();
                            IUUserRights(users.UserID, users.Password, users.Password2);

                        }
                    }
                }
                else
                {
                    dsRoleRight = GetEmptyUserForRoleRight(Convert.ToInt32(users.RoleID));
                    IUUserRights(users.UserID, users.Password, users.Password2);
                }


            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public void IUUserRights(int UserID, string Password, string Password2)
        {
            for (int i = 0; i < dsRoleRight.Tables[0].Rows.Count; i++)
            {
                SqlCommand cmdIURoleRights = new SqlCommand("IUUserRights", con);
                cmdIURoleRights.CommandType = CommandType.StoredProcedure;
                cmdIURoleRights.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdIURoleRights.Parameters.AddWithValue("@LocationID", LocationID);
                cmdIURoleRights.Parameters.AddWithValue("@UserID", UserID);
                cmdIURoleRights.Parameters.AddWithValue("@Password", Password);
                cmdIURoleRights.Parameters.AddWithValue("@Password2", Password2);
                cmdIURoleRights.Parameters.Add(new SqlParameter("@EffectiveDateFrom", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                cmdIURoleRights.Parameters.Add(new SqlParameter("@EffectiveDateTo", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                cmdIURoleRights.Parameters.AddWithValue("@ModuleID", Convert.ToInt32(dsRoleRight.Tables[0].Rows[i]["ModuleID"].ToString()));
                cmdIURoleRights.Parameters.AddWithValue("@Rights", Convert.ToInt32(dsRoleRight.Tables[0].Rows[i]["Rights"].ToString()));
                cmdIURoleRights.Parameters.AddWithValue("@UserRightsID", 0);
                //  cmdIURoleRights.Parameters.AddWithValue("@UserID", useRights.UserID);
                cmdIURoleRights.Parameters.AddWithValue("@FormMode", 0);
                cmdIURoleRights.Parameters.AddWithValue("@RowInternal", false);
                cmdIURoleRights.Parameters.AddWithValue("@IsAuthorised", 0);
                cmdIURoleRights.Parameters.AddWithValue("@CreationID", CreationID);
                cmdIURoleRights.Parameters.AddWithValue("@RowStatus", 0);
                cmdIURoleRights.Parameters.AddWithValue("@Mode", "Add");
                int result = cmdIURoleRights.ExecuteNonQuery();
            }
        }
        public DataSet GetEmptyUserForRoleRight(int RoleID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetEmptyUserForRoleRight", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleID", RoleID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
 con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
              //  con.Close();
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;
        }

        public DataSet GetRole(string GetRole)
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select RoleID ,RoleName from  Roles  where RowStatus = 0 and  RoleName like '" + @GetRole + "%'", con);
            cmd.Parameters.AddWithValue("@GetRole", GetRole);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet SelectAllData()
        {

            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllUsers", con);
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
        public DataSet GetUsers(int UserID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetUsers", con);
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
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetAllHospitalLocation()
        {

            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllHospitalLocation", con);
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

        public int DeleteUser(int UserID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("DeleteUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            con.Open();
            int delete = cmd.ExecuteNonQuery();
            return delete;
        }
    }
}