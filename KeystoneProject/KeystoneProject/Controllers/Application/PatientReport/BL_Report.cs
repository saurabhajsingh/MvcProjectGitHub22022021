using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Financial;
using KeystoneProject.Models.MISReport.PatientReport;
using KeystoneProject.Controllers.MISReport;

namespace KeystoneProject.Buisness_Logic.PatientReport
{
    public class BL_Report
    {
        private SqlConnection con;
        int HospitalID; int LocationID; int UserID;

        private void Connect()
        {
             HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
             LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
             UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

            string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(constring);
        }


        #region Get All Consultant Dr
        public DataSet GetAllConsultantDr(int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllConsultantDr", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion

        #region Get All Department Data
        public DataSet GetAllDepartment(int HospitalID, int LocationID)
        {
            try
            {
                Connect();
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllDepartment", param);
                
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
        }
        #endregion

        #region Get All Referred Dr
        public DataSet GetAllReferredDr(int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllReferredDr", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion

        #region Get All Users
        public DataSet GetAllUsers()
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[1];
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllUsers", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion

        #region Get Report Financial Account Bill Wise Collection
        public DataSet GetReportFABillWiseCollection(int HospitalID, int LocationID, DateTime DateFrom, DateTime DateTo, string CollectionType, string CollectionPaymentType, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@CollectionType", SqlDbType.NVarChar, 20);
                param[2].Value = CollectionType;
                param[3] = new SqlParameter("@CollectionPaymentType", SqlDbType.NVarChar, 20);
                param[3].Value = CollectionPaymentType;
                param[4] = new SqlParameter("@UserID", SqlDbType.NVarChar, 20);
                param[4].Value = UserID;
                param[5] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                param[5].Value = DateFrom;
                param[6] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                param[6].Value = DateTo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportFABillWiseCollection", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        # endregion

        #region Get All Report FA Patient Wise Collection
        public DataSet ReportFAPatientWiseCollection(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string PatientType, string PatientRegNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@PatientType", SqlDbType.NVarChar, 20);
                param[4].Value = PatientType;
                param[5] = new SqlParameter("@PatientRegNo", SqlDbType.NVarChar, 20);
                param[5].Value = PatientRegNo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportFAPatientWiseCollection", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        # endregion

        #region Rpt Employee Salary Department Wise
        public DataSet RptEmployeeSalaryDepartmentWise(int HospitalID, int LocationID, string Year, string Month, string EmployeeCatagoryID)
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
                param[2] = new SqlParameter("@Year", SqlDbType.NVarChar);
                param[2].Value = Year;
                param[3] = new SqlParameter("@Month", SqlDbType.NVarChar, 20);
                param[3].Value = Month;
                //param[4] = new SqlParameter("@EmployeeID", SqlDbType.NVarChar);
                //param[4].Value = EmployeeID;
                param[4] = new SqlParameter("@EmployeeCatagoryID", SqlDbType.NVarChar);
                param[4].Value = EmployeeCatagoryID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptEmployeeSalaryDepartmentWise", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

       
        #endregion

        #region Get All Patient OPD New
        public DataSet GetAllPatientOPDNew(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientOPDNew", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

       
        # endregion

        #region Get All Patient IPD New
        public DataSet GetAllPatientIPDNew(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            { 
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientIPDNew", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        #endregion

        public DataSet GetAllPatientOPDRevisit(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientOPDRevisit", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        #region Get All Patient IPD ReAdmission
        public DataSet GetAllPatientIPDReAdmission(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientIPDReAdmission", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #region Get All Patient OPD To IPD
        public DataSet GetAllPatientOPDToIPD(int HospitalID, int LocationID, DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientOPDToIPD", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

      
        # endregion
        #region Get All Patient IPD All
        public DataSet ReportPatientAll(DateTime FromDate, DateTime ToDate, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[2].Value = FromDate;
                param[3] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[3].Value = ToDate;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientAll", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        
        # endregion



        public DataSet ReportPatientIPDAdmit(int HospitalID, int LocationID, DateTime Date, string ConsDrID, string RefDrID, string DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[3] = new SqlParameter("@Date", SqlDbType.DateTime);
                param[3].Value = Date;
                param[4] = new SqlParameter("@ConsDrID", SqlDbType.NVarChar, 20);
                param[4].Value = ConsDrID;
                param[5] = new SqlParameter("@RefDrID", SqlDbType.NVarChar, 20);
                param[5].Value = RefDrID;
                param[6] = new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 20);
                param[6].Value = DepartmentID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientIPDAdmit", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        # endregion

        #region Financial Year
        public DataSet GetAllFinancialYear()
        {
            
                DataSet ds = new DataSet();
                try
                {
                   SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllFinancialYear", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            
        }
        #endregion
        public DataSet GetReportPatientHistory(int HospitalID, int LocationID, string FinancialYearID, string PatientRegNo, string PatientType)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@FinancialYearID", SqlDbType.NVarChar);
                param[2].Value = FinancialYearID;
                param[3] = new SqlParameter("@PatientRegNo", SqlDbType.NVarChar);
                param[3].Value = PatientRegNo;
                param[4] = new SqlParameter("@PatientType", SqlDbType.NVarChar);
                param[4].Value = PatientType;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportPatientHistory", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet VoucherName()
        {
            DataSet ds = new DataSet();
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select VoucherTypeID,VoucherTypeName,ReferenceCode,Narration from VoucherType where CodeBlock='' and  RowStatus = 0 and HospitalID =  " + HospitalID + " and LocationID = " + LocationID + " ",con);
            con.Open();
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet RptMISVoucherWise( DateTime DateFrom, DateTime DateTo, string VoucharName)
        {
            DataSet dsVoucher = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;

                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;

                param[2] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
                param[2].Value = DateFrom;

                param[3] = new SqlParameter("@DateTo", SqlDbType.DateTime);
                param[3].Value = DateTo;

                param[4] = new SqlParameter("@VoucharName", SqlDbType.NVarChar);
                param[4].Value = VoucharName;


                dsVoucher = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptMISVoucherWise", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return dsVoucher;
        }
        //public DataSet GetReportMISMonthlyConsultancyCharges( string PatientType, string DoctorID, DateTime DateFrom, DateTime DateTo)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        Connect();
        //        SqlParameter[] param = new SqlParameter[8];
        //        param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
        //        param[0].Value = HospitalID;
        //        param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
        //        param[1].Value = LocationID;
        //        param[2] = new SqlParameter("@DateFrom", SqlDbType.DateTime);
        //        param[2].Value = DateFrom;
        //        param[3] = new SqlParameter("@DateTo", SqlDbType.DateTime);
        //        param[3].Value = DateTo;
        //        param[4] = new SqlParameter("@PatientType", SqlDbType.NVarChar, 20);
        //        param[4].Value = PatientType;
        //        param[5] = new SqlParameter("@DoctorID", SqlDbType.NVarChar, 20);
        //        param[5].Value = DoctorID;
        //        ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "ReportMISMonthlyConsultancyCharges", param);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.Publish(ex);
        //        throw ex;
        //    }
        //    return ds;
        //}

        public DataSet GetReportMISMonthlyConsultancyCharges(DateTime FromDate, DateTime ToDate, string PatientType, string DoctorID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISMonthlyConsultancyCharges", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
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

        public DataSet GetAllConsultantDr()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllConsultantDr", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

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

        public DataSet GetAllPerformedby(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  DoctorID,DoctorPrintName  from  Doctor where DoctorFName  like  @prefix+'%'  and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorID asc", con);
            cmd.Parameters.AddWithValue("@prefix", prefix);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllOpinionBy(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select  DoctorID,DoctorPrintName  from  Doctor where DoctorFName  like  @prefix+'%'  and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by  DoctorID asc", con);
            cmd.Parameters.AddWithValue("@prefix", prefix);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllProblem()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ProblemID,Problem  from GovernmentRecordProblem where HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and  RowStatus= 0 order by  ProblemID asc", con);
            //cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet RptMISGovernmentRecords(DateTime FromDate, DateTime ToDate, string PatientType, string Pro,string Opn,string Per)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("RptMISGovernmentRecords", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@OpinionBy", Opn);
                cmd.Parameters.AddWithValue("@PerformedBy", Per);
                cmd.Parameters.AddWithValue("@ProblemID", Pro);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
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


        public DataSet GetAllDoctor()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   DoctorID, DoctorFName +' '+ DoctorLName as 'DoctorName'  from  Doctor  where  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and DoctorType = 'Consultant'  order by  DoctorPrintName asc", con);
            //cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetAllPatient()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select   PatientRegNO,PatientName  from  Patient  where  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " ", con);
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

        public DataSet GetPaidAmt(DateTime FromDate, DateTime ToDate, string PatientType, string PaidType, string ser, string dr)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISPaidUnpaidPatientWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DoctorID", dr);
                cmd.Parameters.AddWithValue("@PaidType", PaidType);
                cmd.Parameters.AddWithValue("@ServiceID", ser);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);

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

        public bool Save(MISConsultantDrService obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUConsultantPaidAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.PaidAmtID == "")
            {
                cmd.Parameters.AddWithValue("@PaidAmtID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PaidAmtID", obj.PaidAmtID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@BillDate", obj.BillDt);
            cmd.Parameters.AddWithValue("@DoctorName", obj.DrName);
            cmd.Parameters.AddWithValue("@DoctorID", obj.DrID);
            cmd.Parameters.AddWithValue("@patientname", obj.PatientName);
            cmd.Parameters.AddWithValue("@PatientRegNo", obj.PatientID);
            cmd.Parameters.AddWithValue("@ServiceName", obj.ServiceName);
            cmd.Parameters.AddWithValue("@serviceamount", obj.ServiceAmount);
            cmd.Parameters.AddWithValue("@paidamount", obj.PaidAmount);
            cmd.Parameters.AddWithValue("@PaidDate", obj.PaidDate);
            cmd.Parameters.AddWithValue("@ServiceID", obj.ServiceID);
            cmd.Parameters.AddWithValue("@BillNo", obj.BillNo);
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