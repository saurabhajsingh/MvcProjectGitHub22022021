using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Models.Keystone;
using KeystoneProject.Models.Models_Application;

namespace KeystoneProject.Buisness_Logic.Application
{
    public class Application
    {

        int HospitalID;
        int LocationID;
        int UserID;
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }
        public void HospitlLocationID()
        {

            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public bool UsersLogin(Models_Application obj)
        {
            DataView dataView3 = null;
            SqlCommand cmd = null;
            Connect();
            if (obj.UserName == "admin" && obj.Password == "admin")
            {

                cmd = new SqlCommand("select HospitalID ,LocationID,   UserID , Password, Password2 from Users where LoginName = '" + obj.UserName + "' and password = '" + obj.Password + "'", con);
                //   cmd = new SqlCommand("Select ur.HospitalID,ur.LocationID from UserRights ur where  Rowstatus = 0 Group By ur.HospitalID,ur.LocationID", con);

                //  SqlCommand cmd = new SqlCommand("ValidateUser",con);
                //  cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.AddWithValue("Username", obj.UserName);/
                //  cmd.Parameters.AddWithValue("Password", obj.Password);
                //SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows[0]["Password"].ToString() == obj.Password)
                    {
                        dataView3 = new DataView(dt, " Password = '" + obj.Password + "'", "", DataViewRowState.CurrentRows);

                        HttpContext.Current.Session["Password"] = obj.Password;
                        HttpContext.Current.Session["Password2"] = "";
                    }
                    if (dt.Rows[0]["Password2"].ToString() == obj.Password)
                    {
                        dataView3 = new DataView(dt, " Password2 = '" + obj.Password + "'", "", DataViewRowState.CurrentRows);
                        HttpContext.Current.Session["Password2"] = obj.Password;
                        HttpContext.Current.Session["Password"] = "";
                    }

                    string UserID = "admin";

                    string HospitalID = dataView3.ToTable().Rows[0][0].ToString();
                    string LocationID = dataView3.ToTable().Rows[0][1].ToString();
                    HttpContext.Current.Session["UserID"] = 1;
                    // HttpContext.Current.Session["UserID"] = 1;
                    HttpContext.Current.Session["HospitalID"] = HospitalID;
                    HttpContext.Current.Session["LocationID"] = LocationID;

                    return true;
                }
                else
                {

                    return false;
                }


            }
            else
            {

                cmd = new SqlCommand("Select   u.UserID,ur.HospitalID,ur.LocationID,u.Password,u.Password2 ,u.LoginName from Users u Inner Join UserRights ur on  u.UserID = ur.UserID where  u.Rowstatus = 0 and u.LoginName='" + obj.UserName + "' and u.Password='" + obj.Password + "' or u.Password2='" + obj.Password + "' Group By u.UserID,ur.HospitalID,ur.LocationID,u.Password,u.Password2,u.LoginName", con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {

                    dataView3 = new DataView(dt, " Password = '" + obj.Password.ToString() + "'   and  LoginName='" + obj.UserName.ToString() + "'", "", DataViewRowState.CurrentRows);

                    if (dataView3.ToTable().Rows.Count == 0)
                    {
                        dataView3 = new DataView(dt, " Password2 = '" + obj.Password.ToString() + "'   and  LoginName='" + obj.UserName.ToString() + "'", "", DataViewRowState.CurrentRows);
                        HttpContext.Current.Session["Password2"] = dataView3.ToTable().Rows[0]["Password2"].ToString();
                        HttpContext.Current.Session["Password"] = "";
                    }
                    else
                    {
                        HttpContext.Current.Session["Password2"] = "";
                        HttpContext.Current.Session["Password"] = dataView3.ToTable().Rows[0]["Password"].ToString();
                    }



                    string UserID = dataView3.ToTable().Rows[0][0].ToString();
                    string HospitalID = dataView3.ToTable().Rows[0]["HospitalID"].ToString();
                    string LocationID = dataView3.ToTable().Rows[0]["LocationID"].ToString();
                    HttpContext.Current.Session["UserID"] = UserID;
                    HttpContext.Current.Session["HospitalID"] = HospitalID;
                    HttpContext.Current.Session["LocationID"] = LocationID;

                    return true;
                }
                else
                {

                    return false;
                }

            }
        }

        public DataSet UserStatus(string obj)
        {
            SqlDataAdapter dt = new SqlDataAdapter();
            DataSet ds = new DataSet();

            HospitlLocationID();
            Connect();
            SqlCommand da = new SqlCommand("update Users set UserStatus = " +"'true'"+ " where UserID = '" + obj+ "' and LocationID = "+LocationID+ " and HospitalID = " + HospitalID + "", con);
            dt.SelectCommand = da;
            con.Open();  
            dt.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet CheckUserStatus(string obj)
        {
            SqlDataAdapter dt = new SqlDataAdapter();
            DataSet ds = new DataSet();

            HospitlLocationID();
            Connect();
            SqlCommand da = new SqlCommand("SELECT UserStatus FROM Users where UserID = '" + obj + "' and LocationID = " + LocationID + " and HospitalID = " + HospitalID + "", con);
            dt.SelectCommand = da;
            con.Open();
            dt.Fill(ds);
            con.Close();
            return ds;

        }
        public DataSet UserStatusFalse(string obj)
        {
            SqlDataAdapter dt = new SqlDataAdapter();
            DataSet ds = new DataSet();

            HospitlLocationID();
            Connect();
            SqlCommand da = new SqlCommand("update Users set UserStatus = " + "'false'" + " where UserID = '" + obj + "' and LocationID = " + LocationID + " and HospitalID = " + HospitalID + "", con);
            dt.SelectCommand = da;
            con.Open();
            dt.Fill(ds);
            con.Close();
            return ds;

        }

          #region LocationChange

        public List<HospitalLocation> GetLocationByUserID(string UID, string selLoc)
        {
            Connect();
            List<HospitalLocation> locidlist = new List<HospitalLocation>();
            SqlCommand cmd = null;
            if (UID == "1")
                //cmd = new SqlCommand("select LocationID,LocationName from HospitalLocation where LocationID in ( select LocationID from UserRights group by LocationID)", con);
                cmd = new SqlCommand("select LocationID,LocationName from HospitalLocation where LocationID in ( select LocationID from HospitalLocation where Rowstatus = 0 ) and Rowstatus = 0", con);
            else
                cmd = new SqlCommand("select LocationID,LocationName from HospitalLocation where LocationID in ( select LocationID from UserRights where UserID=" + UID + "  group by LocationID) and Rowstatus = 0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                locidlist.Add(
                    new HospitalLocation
                    {
                        LocationID = Convert.ToInt32(dr["LocationID"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        HospitalName = selLoc
                    });
            }
            return locidlist;
        }


        #endregion
    }
}