using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Controllers.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_PatientCWProfile
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public DataSet BindOrganization(string GetOrganization)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationName  like '%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
           // cmd.Parameters.AddWithValue("@GetOrganization", GetOrganization);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
       
        public DataSet BindOrg(string id)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationID  = " + id + " and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
            // cmd.Parameters.AddWithValue("@GetOrganization", GetOrganization);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet BindProfile(string GetOrganization,string OrgID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select Profile.ProfileID , Profile.Name,PatientCWProfile.PatientCWProfileID  from PatientCWProfile    left join Profile on Profile.ProfileID  = PatientCWProfile.ProfileID where  PatientCWProfile.OrganizationID   ="+OrgID+" and  PatientCWProfile.HospitalID =" + HospitalID + "  and PatientCWProfile.LocationID =" + LocationID + "  and PatientCWProfile.RowStatus = 0  order by  Profile.Name asc ", con);
           // cmd.Parameters.AddWithValue("@GetOrganization", OrgID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllPatientCWProfileRate()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllPatientCWProfileRate", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet GetPatientCWProfileRate(int id)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientCWProfileRate", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@PatientCWProfileID", id);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public bool Save(PatientCWProfile obj)
        {
            Connect();
            con.Open();
            try
            {
                if(obj.PatientCWProfileID!="0")
                {
                    SqlCommand cmd = new SqlCommand("IUPatientCWProfileRate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@PatientCWProfileID", obj.PatientCWProfileID);
                    cmd.Parameters.AddWithValue("@OrganizationID", obj.OrganizationID);
                    cmd.Parameters.AddWithValue("@ProfileID",obj.ProfileID);
                    cmd.Parameters.AddWithValue("@GeneralCharges", obj.GeneralCharges);
                    cmd.Parameters.AddWithValue("@EmergencyCharges", obj.EmergencyCharges);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@RecommendedByDoctor", "");
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                  
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
                else
                {
                    
                         DataSet dsPatientCWProfile = GetPatientCWProfile();
                        foreach (DataRow NR in dsPatientCWProfile.Tables[0].Rows)
                        {
                            SqlCommand cmd2 = new SqlCommand("IUPatientCWProfile", con);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd2.Parameters.AddWithValue("@PatientCWProfileID", NR["PatientCWProfileID"] = 0);

                            cmd2.Parameters.AddWithValue("@ProfileID", NR["ProfileID"] = obj.ProfileID);
                            cmd2.Parameters.AddWithValue("@GeneralCharges", NR["GeneralCharges"] = obj.GeneralCharges);
                            cmd2.Parameters.AddWithValue("@EmergencyCharges", NR["EmergencyCharges"] = obj.EmergencyCharges);
                            cmd2.Parameters.AddWithValue("@OrganizationID", NR["OrganizationID"]);
                            cmd2.Parameters.AddWithValue("@RecommendedByDoctor", NR["RecommendedByDoctor"]);
                            cmd2.Parameters.AddWithValue("@CreationID", NR["CreationID"] = UserID);
                            cmd2.Parameters.AddWithValue("@Mode", NR["Mode"] = "Add");
                            //NR["CreationID"] = ds.Tables[0].Rows[0]["CreationID"].ToString().Trim();
                            //NR["Mode"] = "Add";
                            con.Open();
                            int CWProfileTest = cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DataSet GetPatientCWProfile()
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientCWProfile", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public bool DeletePatientCWProfileRate(string PatientCWProfileID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientCWProfileID", SqlDbType.Int);
                param[2].Value = PatientCWProfileID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeletePatientCWProfileRate", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
        }
    }
}