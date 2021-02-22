using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using KeystoneProject.Models.Keystone;

namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_RolesRights
    {
        private SqlConnection con;

        private void connect()
        {
             HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
             LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
             UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        Rights right = new Rights();
        public static bool flag = false;
        public static int result = 0;
        public static string strResult = string.Empty;
        public string Mode = string.Empty;
        //DataSet ds;

        DataSet ds = new DataSet();
        public bool addRolesRight(RolesRights roles, Rights right)
        {
            int length = 0;
            connect();
            con.Open();
            try
            {
                SqlCommand cmdIURole = new SqlCommand("IURole", con);
                cmdIURole.CommandType = CommandType.StoredProcedure;
                if (roles.RoleID == "" || roles.RoleID == null)
                {
                    cmdIURole.Parameters.AddWithValue("@RoleID", 0);
                    cmdIURole.Parameters["@RoleID"].Direction = ParameterDirection.Output;
                    Mode = "Add";
                }
                else
                {
                    cmdIURole.Parameters.AddWithValue("@RoleID", roles.RoleID);
                    Mode = "Edit";
                }
                cmdIURole.Parameters.AddWithValue("@RoleName", roles.RoleName);
                cmdIURole.Parameters.AddWithValue("@ReferenceCode", 1);
                cmdIURole.Parameters.AddWithValue("@Description", "");
                cmdIURole.Parameters.AddWithValue("@RowInternal", false);
                cmdIURole.Parameters.AddWithValue("@CreationID", UserID);
                cmdIURole.Parameters.AddWithValue("@Mode", Mode);
                int output = cmdIURole.ExecuteNonQuery();
                if (output > 0)
                {
                    roles.RoleID = cmdIURole.Parameters["@RoleID"].Value.ToString();
                    int Number = 0;
                    if (right.OLDRightID == null)
                    {
                        length = 0;
                    }
                    else
                    {
                        length = right.OLDRightID.Length;
                    }
                    //for (int i = 0; i < right.ParentLevel0ID.Length; i++)
                    //{


                    //DataSet dsexpect=       GetRoleAndRightsByRoleID(Convert.ToInt32( roles.RoleID));
                    //       List<string> OldDataExpect = new List<string>() ;
                    //       foreach(DataRow dr in dsexpect.Tables[0].Rows)
                    //       {
                    //           OldDataExpect.Add(Convert.ToString(dr["ModuleID"].ToString() + "+" + dr["RightCode"].ToString()));
                    //       }
                    //       //string[] l = OldDataExpect.Split('+');


                    //   string[] Level = OldDataExpect.Except(right.ParentLevel4ID).ToArray();
                    if (right.ParentLevel4ID != null)
                    {
                        for (int m = 0; m < right.ParentLevel4ID.Length; m++)
                        {
                            if (length > 0)
                            {
                                //if (right.ParentLevel4ID.Length > right.OLDRightID.Length)
                                //{
                                //    right.ParentLevel1ID = right.ParentLevel4ID.Except(right.OLDRightID).ToArray();
                                //    for (int j = 0; j < right.ParentLevel1ID.Length; j++)
                                //    {
                                string[] parentwithIDs = right.ParentLevel4ID[m].Split('+');
                                roles.Mode = "Add";
                                add(roles, parentwithIDs, Number);
                                //    }
                                //}
                                //else
                                //{
                                //    right.ParentLevel1ID = right.OLDRightID.Except(right.ParentLevel4ID).ToArray();

                                //    Number = 1;
                                //    for (int j = 0; j < right.ParentLevel1ID.Length; j++)
                                //    {
                                //        string[] parentwithIDs = right.ParentLevel1ID[j].Split('+');
                                //        roles.Mode = "Edit";
                                //        add(roles, parentwithIDs, Number);
                                //    }
                                //}
                                //  break;
                            }
                            else
                            {
                                string[] parentwithID = right.ParentLevel4ID[m].Split('+');
                                roles.Mode = "Add";
                                add(roles, parentwithID, Number);
                            }
                        }
                    }
                            if (right.beforAfterchk[0] != "")
                            {
                                beforAfterchk(roles, right);
                            }
                        
                    
                    // }
                    if (result > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }

            catch (Exception ex)

            {
                con.Close();
                flag = false;
            }
            con.Close();
            return flag;
        }

        public void add(RolesRights roles, string[] parentwithIDs, int Number)
        {
            roles.Mode=RoleRightsMode( roles,  parentwithIDs,  Number);
            //  connect();
            string con1 = con.State.ToString();
            if(con1== "Closed")
            {
                con.Open();
            }
            SqlCommand cmdIURoleRights = new SqlCommand("IURoleRights", con);
            cmdIURoleRights.CommandType = CommandType.StoredProcedure;
            cmdIURoleRights.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmdIURoleRights.Parameters.AddWithValue("@locationID", LocationID);
            cmdIURoleRights.Parameters.AddWithValue("@ModuleID", parentwithIDs[0].ToString());
            cmdIURoleRights.Parameters.AddWithValue("@RightCode", parentwithIDs[1].ToString());
            cmdIURoleRights.Parameters.AddWithValue("@RoleRightID", 0);
            cmdIURoleRights.Parameters.AddWithValue("@RoleID", roles.RoleID);
            cmdIURoleRights.Parameters.AddWithValue("@FormMode", 0);
            cmdIURoleRights.Parameters.AddWithValue("@RowInternal", false);
            cmdIURoleRights.Parameters.AddWithValue("@IsAuthorised", 0);
            cmdIURoleRights.Parameters.AddWithValue("@CreationID", UserID);
            cmdIURoleRights.Parameters.AddWithValue("@RowStatus", Number);
            cmdIURoleRights.Parameters.AddWithValue("@Mode", roles.Mode);
            result = cmdIURoleRights.ExecuteNonQuery();
        }

        public string RoleRightsMode(RolesRights roles, string[] parentwithIDs, int Number)
        {
          //  connect();
            SqlDataAdapter ad = new SqlDataAdapter("select*from RoleRights where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 and RoleID=" + roles.RoleID + " and RightCode=" + parentwithIDs[1].ToString() + " and ModuleID="+ parentwithIDs[0].ToString() + "", con);
            DataSet ds = new DataSet();
          //  con.Open();
            ad.Fill(ds);
          //  con.Close();
            if(ds.Tables[0].Rows.Count>0)
            {
                roles.Mode = "Edit";
            }
            else
            {
                roles.Mode = "Add";
            }
            return roles.Mode;
        }



        public void beforAfterchk(RolesRights roles, Rights right)
        {
            for (int m = 0; m < right.beforAfterchk.Length; m++)
            {
                if (right.beforAfterchk[m] != "")
                {
                    string[] parentwithIDs = right.beforAfterchk[m].Split('+');
                    //  connect();
                    SqlCommand cmd = new SqlCommand("update RoleRights set RowStatus=1 where HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 and RoleID=" + roles.RoleID + " and RightCode=" + parentwithIDs[1].ToString() + " and ModuleID=" + parentwithIDs[0].ToString() + "", con);
                    int a = cmd.ExecuteNonQuery();
                }
            }
          
            //  con.Open();
            
            
        }


        public DataSet GetRoleAndRightsByRoleID(int RoleID)
        {
            connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetRoleDataForRoleAndRights", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleID", RoleID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@locationID", LocationID);
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
        public DataSet GetRoleAndRights()
        {
            connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetRoleAndRights", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@locationID", LocationID);
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
        public DataSet GetAllRols()
        {
            connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllRole", con);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@locationID", LocationID);
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
    }
}