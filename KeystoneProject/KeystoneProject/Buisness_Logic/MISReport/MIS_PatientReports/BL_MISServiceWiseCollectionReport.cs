using KeystoneProject.Models.Patient;
using KeystoneProject.Models.Report;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport
{
    public class BL_MISServiceWiseCollectionReport
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MISServiceWiseCollectionReport> questionheadlist = new List<MISServiceWiseCollectionReport>();
        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet BindServiceGroupName(string prefix)
        {
            Connect();

           SqlCommand cmd = new SqlCommand("select ServiceGroupID , ServiceGroupName,ServiceType,ServicesOrder  from  ServiceGroup where  HospitalID = " + HospitalID + "  and LocationID =   " + LocationID + " and RowStatus = 0 order by  ServiceGroupName asc", con);

           con.Open();
           cmd.ExecuteNonQuery();
           DataSet ds = new DataSet();
           SqlDataAdapter da = new SqlDataAdapter(cmd);
           da.Fill(ds);
           con.Close();
           return ds;

        }
        public DataSet BindSeviceName(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ServiceID ,ServiceName,ServiceGroupID,GeneralCharges,EmergencyCharges   from  Services  where HospitalID = " + HospitalID + "  and LocationID =   " + LocationID + "  and RowStatus = 0 order by  ServiceName asc", con);

            con.Open();
            cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public List<Patient1> GetPatientName(string search)
        {
            Connect();

            List<Patient1> patientnamelist = new List<Patient1>();
            SqlCommand cmd = new SqlCommand("select PatientName,PatientRegNO,MobileNo,EmailID,Address from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new Patient1
                    {
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNO = Convert.ToString(dr["PatientRegNO"])
                    });

            }
            return patientnamelist;
        }

        public void ReportMISServicesWiseCollectionNew(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ServicesID, string ServiceGroupID, string ServiceType, string ReportType)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] para = new SqlParameter[8];
                para[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                para[0].Value = HospitalID;
                para[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                para[1].Value = LocationID;
                para[2] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                para[2].Value = FromDate;
                para[3] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                para[3].Value = ToDate;
                para[4] = new SqlParameter("@ServicesID", SqlDbType.NVarChar, 20);
                para[4].Value = ServicesID;
                para[5] = new SqlParameter("@ServiceGroupID", SqlDbType.NVarChar, 50);
                para[5].Value = ServiceGroupID;
                para[6] = new SqlParameter("@ServiceType", SqlDbType.NVarChar, 100);
                para[6].Value = ServiceType;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportMISServicesWiseCollectionNew", para);

                HttpContext.Current.Session["ReportMISServicesWiseCollectionNew"] = ds;

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }

        public DataSet ReportServicePatientWise(DateTime DateFrom, DateTime DateTo, string SrvID, string ServiceGrpID, string Patientregno, string ServiceType, string ReportType)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                param[2].Value = DateFrom;
                param[3] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                param[3].Value = DateTo;
                param[4] = new SqlParameter("@SrvID", SqlDbType.NVarChar, 20);
                param[4].Value = SrvID;
                param[5] = new SqlParameter("@ServiceGrpID", SqlDbType.NVarChar, 50);
                param[5].Value = ServiceGrpID;
                param[6] = new SqlParameter("@ServiceType", SqlDbType.NVarChar, 100);
                param[6].Value = ServiceType;
                param[7] = new SqlParameter("@Patientregno", SqlDbType.NVarChar, 100);
                param[7].Value = Patientregno;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptServicepatientwise", param);
                HttpContext.Current.Session["RptServicepatientwise"] = ds;


                ReportMISServicesWiseCollectionNew(HospitalID, LocationID, DateFrom, DateTo, SrvID, ServiceGrpID, ServiceType, "");
   
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return ds;
        }
        public DataSet ReportMISServicesWiseCollection(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string ServicesID, string ServiceGroupID, string ServiceType,string PatientReg)
        {
            if (PatientReg == "")
            {
                PatientReg = "%";
            }
            if (ServiceType=="")
            {
                ServiceType = "All";
            }
            Connect();
            DataSet ds = new DataSet();
            try
            {
                Connect();
                con.Open();
                SqlCommand cmd = new SqlCommand("ReportMISServicesWiseCollection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
                cmd.Parameters.AddWithValue("@ServicesID", ServicesID);
                cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID);
                cmd.Parameters.AddWithValue("@ServiceType", ServiceType);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Close();
                DataSet dssr = new DataSet();
                ad.Fill(ds);
                dssr = ReportServicePatientWise(DateFrom, DateTo, ServicesID, ServiceGroupID, PatientReg, ServiceType, "");
  
            }
            catch (Exception ex)
            {
                // ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

       
    }
}