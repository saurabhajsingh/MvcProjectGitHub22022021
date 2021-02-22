using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ConsentMaster
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

        public bool Save(ConsentMaster obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUConsentMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.ConsentID == "0" || obj.ConsentID == "" || obj.ConsentID == null)
            {
                cmd.Parameters.AddWithValue("@ConsentID", 0);
                cmd.Parameters["@ConsentID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ConsentID", obj.ConsentID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");

            }

           
            cmd.Parameters.AddWithValue("@Language", obj.Language);

            if (obj.ConsentName == null)
                cmd.Parameters.AddWithValue("@ConsentName", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ConsentName", obj.ConsentName);

            if (obj.Description == null)
            {
                cmd.Parameters.AddWithValue("@Path", "");
            }
            else
            {

                if (obj.Description == "<p><br></p>" || obj.Description == "<h1><br></h1>" || obj.Description == "<h2><br></h2>" || obj.Description == "<h3><br></h3>")
                {
                    cmd.Parameters.AddWithValue("@Path", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Path", (obj.Description));
                }


            }
           
            if (obj.PathName == null)
                cmd.Parameters.AddWithValue("@PathName", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PathName", obj.PathName);

            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);

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

        public List<ConsentMaster> GetAllConsentMaster()
        {
            List<ConsentMaster> searchList = new List<ConsentMaster>();
            Connect();

            SqlCommand cmd = new SqlCommand("GetAllConsentMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new ConsentMaster
                    {
                        ConsentID = item["ConsentID"].ToString(),
                        ConsentName = item["ConsentName"].ToString(),
                        

                    });


            }
            return searchList;

        }

        public bool CheckConsentMaster(string ConsentID, string ConsentName)
        {
            string t = "";
            if (ConsentID == null || ConsentID == "")
            {
                t = "0";
            }
            else
            {
                t = ConsentID;
            }
            Connect();
            bool Table;
            bool flag;
            try
            {
                SqlCommand checkHearder = new SqlCommand("CheckConsentMaster", con);
                checkHearder.CommandType = CommandType.StoredProcedure;
                checkHearder.Parameters.AddWithValue("@HospitalID", HospitalID);
                checkHearder.Parameters.AddWithValue("@LocationID", LocationID);
                checkHearder.Parameters.AddWithValue("@ConsentID", t);
                checkHearder.Parameters.AddWithValue("@ConsentName", ConsentName);
                checkHearder.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
                checkHearder.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = checkHearder.ExecuteNonQuery();
                Table = (bool)checkHearder.Parameters["@NameExists"].Value;
                con.Close();
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
        public DataSet GetConsentMaster(int ConsentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@ConsentID", SqlDbType.Int);
                param[1].Value = ConsentID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetConsentMaster", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public bool DeleteConsentMaster(int ConsentID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@ConsentID", SqlDbType.Int);
                aParams[1].Value = ConsentID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteConsentMaster", aParams);
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
    }           
}