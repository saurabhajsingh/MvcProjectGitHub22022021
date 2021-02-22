using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Models.MISReport.PatientReport;
using KeystoneProject.Controllers.PatientReport;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.MISReport.MISPatientReport
{
    public class BL_MISConsultantDrService
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

        public DataSet GetAllDoctor(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   DoctorID, DoctorFName +' '+ DoctorLName as 'DoctorName'  from  Doctor  where  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and DoctorFName like '" + prefix +"%" + "' and DoctorType = 'Consultant'  order by  DoctorPrintName asc", con);
            //cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllPatient(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   PatientRegNO,PatientName  from  Patient  where  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  and PatientName like '" + prefix + "%" + "' ", con);
            //cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetPreBalanceAmount(string id)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetRefoundAmount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNo", id);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet GetAllServiceForDrList(string type)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServiceForDrList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceType", type);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet Getservicename(string type)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("select ServiceName ,ServiceID ,ServiceType from  Getservicename  where HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "'  and ServiceType like '" + type + '%' + "' and RowStatus = 0 order by Getservicename.CreationDate desc ", con);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);


                da.Fill(ds);
                con.Close();
                
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public bool IUGetServices(MISConsultantDrService obj)
        {
            Connect();



            SqlCommand cmd = new SqlCommand("IUGetServices", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ServiceID == "")
            {
                cmd.Parameters.AddWithValue("@ServiceID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ServiceID", obj.ServiceID);

            }
            cmd.Parameters.AddWithValue("@ServiceType", obj.ServiceType);
            cmd.Parameters.AddWithValue("@ServiceName", obj.ServiceName);
            cmd.Parameters.AddWithValue("@Mode", "Add");
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

        public DataSet GetReportMISConsultantDoctor(DateTime FromDate, DateTime ToDate, string PatientType, string ser, string dr, string reg)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISPatientWiseDrServices", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DoctorID", dr);
                cmd.Parameters.AddWithValue("@PatientRegNo", reg);
                cmd.Parameters.AddWithValue("@ServiceID", ser);
                cmd.Parameters.AddWithValue("@ServiceType", PatientType);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet GetPaidAmt(DateTime FromDate, DateTime ToDate, string PatientType, string PaidType, string ser, string dr ,string PatientRegNo)
        {
            string[] ServiceID = ser.Split(',');
            string[] doctorID = dr.Split(',');
            Connect();

            DataSet ds = new DataSet();
            try
            {
                for (int row = 0; row < ServiceID.Length; row++)
                {
                    SqlCommand cmd = new SqlCommand("ReportMISPaidUnpaidPatientWise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID[row]);
                    cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNo);
                    cmd.Parameters.AddWithValue("@PaidType", "PAID");
                    cmd.Parameters.AddWithValue("@ServiceID", ServiceID[row]);
                    cmd.Parameters.AddWithValue("@PatientType", PatientType);

                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    con.Open();
                    ad.Fill(ds);
                }
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }


        public bool Save(MISConsultantDrService obj)
        {
            try
            {
                Connect();
                if (obj.PaidAmtID=="")
                {

                    
                string[] ConAmt = obj.PaidAmount.Split(',');
                string[] SerID = obj.ServiceID.Split(',');
                string[] SerName = obj.ServiceName.Split(',');
                string[] SerAmt = obj.ServiceAmount.Split(',');
                string[] Regno = obj.PatientID.Split(',');
                string[] PatName = obj.PatientName.Split(',');
                string[] DocrID = obj.DrID.Split(',');
                string[] DocName = obj.DrName.Split(',');
                string[] BillAmt = obj.BillAmt.Split(',');
                string[] BillDt = obj.BillDt.Split(',');
                string[] Billno = obj.BillNo.Split(',');

                for (int row = 0; row < ConAmt.Length; row++)
                {
                    if (ConAmt[row].ToString() != "")
                    {

                        SqlCommand cmd = new SqlCommand("IUConsultantPaidAmount", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        if (obj.PaidAmtID == "" || obj.PaidAmtID == null)
                        {
                            cmd.Parameters.AddWithValue("@PaidAmtID", 0);
                            cmd.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PaidAmtID", obj.PaidAmtID);
                            cmd.Parameters.AddWithValue("@Mode", "Edit");
                        }
                        cmd.Parameters.AddWithValue("@BillDate", BillDt[row]);
                        cmd.Parameters.AddWithValue("@ConsultantName", DocName[row]);
                        cmd.Parameters.AddWithValue("@DoctorID", DocrID[row]);
                        cmd.Parameters.AddWithValue("@patientname", PatName[row]);
                        cmd.Parameters.AddWithValue("@PatientRegNo", Regno[row]);
                        cmd.Parameters.AddWithValue("@ServiceName", SerName[row]);
                        cmd.Parameters.AddWithValue("@serviceamount", SerAmt[row]);
                        cmd.Parameters.AddWithValue("@ConsultantAmount", ConAmt[row]);
                        cmd.Parameters.AddWithValue("@PaidDate",obj.PaidDate);
                        cmd.Parameters.AddWithValue("@ServiceID", SerID[row]);
                        cmd.Parameters.AddWithValue("@BillNo", Billno[row]);
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
              
                   
                }
                else
                {
                    string[] PaidID = obj.PaidAmtID.Split(',');
                    for (int i = 0; i < PaidID.Length; i++)
                    {
                        if(PaidID[i]!="")
                        {
                        SqlCommand cmd = new SqlCommand("update  ConsultantPaidAmount set  RowStatus = 1 where HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "'  and PaidAmtID =" + PaidID[i], con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        }
                        
                    }

                }
                

               
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}