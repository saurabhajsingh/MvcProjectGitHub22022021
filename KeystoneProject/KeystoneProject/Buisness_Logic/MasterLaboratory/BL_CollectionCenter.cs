using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Controllers.MasterLaboratory;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_CollectionCenter
    {

        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public bool SaveTest(CollectionCenter obj)
        {
            Connect();
           
          
            SqlCommand cmd = new SqlCommand("IUCollectionCenter", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if(obj.CollectionID == "" || obj.CollectionID == null || obj.CollectionID== "0")
            {
                cmd.Parameters.AddWithValue("@CollectionID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@CollectionID", obj.CollectionID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
           
            cmd.Parameters.AddWithValue("@CollectionName", obj.CollectionName);
            cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
            cmd.Parameters.AddWithValue("@MobileNo", obj.Mobile);
            cmd.Parameters.AddWithValue("@Address", obj.Address);
            cmd.Parameters.AddWithValue("@EmailID", obj.Email);
            cmd.Parameters.AddWithValue("@Admin", obj.AdminInCharge);
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
        public bool CheckCollectionCenter(string CollectionID, string CollectionName)
        {
            string t = "";
            if (CollectionID == null || CollectionID == "")
            {
                t = "0";
            }
            else
            {
                t = CollectionID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckCollectionCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@CollectionID", t);
                cmd.Parameters.AddWithValue("@CollectionName", CollectionName.ToUpper());
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
        public List<CollectionCenter> GetCollectionCenter(int CollectionID)
        {
            Connect();
            List<CollectionCenter> CollectionIDList = new List<CollectionCenter>();

            SqlCommand cmd = new SqlCommand("GetCollectionCenter", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@CollectionID", CollectionID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                CollectionIDList.Add(
                    new CollectionCenter
                    {
                        HospitalID=Convert.ToInt32(dr["HospitalID"]),
                        LocationID=Convert.ToInt32(dr["LocationID"]),
                        CollectionID = (dr["CollectionID"]).ToString(),
                        CollectionName = Convert.ToString(dr["CollectionName"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Mobile = Convert.ToString(dr["MobileNo"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["EmailID"]),
                        AdminInCharge = Convert.ToString(dr["Admin"]),
                    });
            }
            return CollectionIDList;
        }

        public List<CollectionCenter> SelectAllData()
        {
            Connect();
            List<CollectionCenter> CollectionIDList = new List<CollectionCenter>();

            SqlCommand cmd = new SqlCommand("GetAllCollectionCenter", con);
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
                CollectionIDList.Add(
                    new CollectionCenter
                    {
                        CollectionID = (dr["CollectionID"]).ToString(),
                        CollectionName = Convert.ToString(dr["CollectionName"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Mobile = Convert.ToString(dr["MobileNo"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["EmailID"]),
                        AdminInCharge = Convert.ToString(dr["Admin"]),
                    });
            }
            return CollectionIDList;
        }


        #region Delete
        public bool DeleteCollectionCenter(int CollectionID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@CollectionID", SqlDbType.Int);
                aParams[1].Value = CollectionID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteCollectionCenter", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;

        }
        #endregion

    }
}