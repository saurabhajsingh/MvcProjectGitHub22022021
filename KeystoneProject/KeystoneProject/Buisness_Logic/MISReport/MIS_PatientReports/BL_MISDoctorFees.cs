using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports
{
    public class BL_MISDoctorFees
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

        public DataSet DoctorType()
        {
            Connect();
            DataSet dsDoctorWise = null;

            try
            {
                SqlCommand cmd = new SqlCommand("GetDoctorForCommission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorWise = new DataSet();
                da.Fill(dsDoctorWise);
                con.Close();
                return dsDoctorWise;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public DataSet ReportMISDoctorCommissionSettingForPatients(DateTime DateFrom, DateTime DateTo, string DoctorType, string DoctorID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("ReportMISDoctorCommissionSettingForPatients", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //   cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            cmd.Parameters.AddWithValue("@DoctorType", DoctorType);
            cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
            cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));


            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(ds);
            con.Close();
            return ds;
        }

        DataSet dsOPDCommission2 = new DataSet();
        DataSet dsOPDCommission1 = new DataSet();
        public List<MISDoctorFees> ReportMISDoctorFees(DateTime DateFrom, DateTime DateTo, string DoctorType, string DoctorID)
        {

            List<MISDoctorFees> MISReportMISDoctorFees = new List<MISDoctorFees>();
            if (DoctorID != "")
            {

                DataSet ds = ReportMISDoctorCommissionSettingForPatients(DateFrom, DateTo, DoctorType, DoctorID);

               

                HttpContext.Current.Session["ReportMISDoctorCommissionSettingForPatients"] = ds;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "OPD",
                            PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                            PatientName = Convert.ToString(dr["PatientName"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),


                        });

                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "IPD",
                            PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                            PatientName = Convert.ToString(dr["PatientName"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),


                        });

                }
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "Lab",
                            PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                            PatientName = Convert.ToString(dr["PatientName"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),


                        });

                }
                return MISReportMISDoctorFees;

            }
            else
            {
              DataSet ds=  SelectAll( HospitalID,  LocationID);
          MISReportMISDoctorFees=    btnViewAll_Click(DateFrom, DateTo, DoctorType, DoctorID);

          HttpContext.Current.Session["MISReportMISDoctorFees"] = dsAllDoctorCommission;

            }

            return MISReportMISDoctorFees;

        }
        DataSet dsAllDoctorCommission = new DataSet();
        public List<MISDoctorFees> OPDCommissionPopup1(DateTime DateFrom, DateTime DateTo, string DoctorID, string RegNO, string DoctorType)
        {
         
                Connect();

                List<MISDoctorFees> MISReportMISDoctorFees = new List<MISDoctorFees>();
                SqlCommand cmd = new SqlCommand("ReportMISDoctorCommissionSettingForPatientBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //   cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                cmd.Parameters.AddWithValue("@RegNO", RegNO);
                cmd.Parameters.AddWithValue("@DoctorType", DoctorType);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));


                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(ds);


                con.Close();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "OPD",
                            BillNo = Convert.ToString(dr["BillNo"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),



                        });

                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "IPD",
                            BillNo = Convert.ToString(dr["BillNo"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),


                        });

                }
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {
                            opdtype = "LAB",
                            BillNo = Convert.ToString(dr["BillNo"]),
                            NetAmount = Convert.ToString(dr["NetAmount"]),
                            Fees = Convert.ToString(dr["Fees"]),


                        });

                }
                return MISReportMISDoctorFees;          
            
           
           


              
        }

        public DataSet  SelectAll(int HospitalID, int LocationID)
        {
            Connect();

            List<MISDoctorFees> MISReportMISDoctorFees = new List<MISDoctorFees>();
            SqlCommand cmd = new SqlCommand("GetAllDoctorForCommission", con);
            cmd.CommandType = CommandType.StoredProcedure;
           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
           
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(ds);
            con.Close();

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    MISReportMISDoctorFees.Add(
            //        new MISDoctorFees
            //        {

            //            DoctorID        = Convert.ToString(dr["DoctorID"]),
            //            DoctorName      = Convert.ToString(dr["DoctorName"]),
            //            DoctorType      = Convert.ToString(dr["DoctorType"]),
            //            OPDCommission   = Convert.ToString(dr["OPDCommission"]),
            //            IPDCommission   = Convert.ToString(dr["IPDCommission"]),
            //            LABCommission   = Convert.ToString(dr["LABCommission"]),
            //            TotalCommission = Convert.ToString(dr["TotalCommission"]),



            //        });

            //}
            return ds;
        }


        public List<MISDoctorFees> btnViewAll_Click(DateTime DateFrom, DateTime DateTo, string DoctorType, string DoctorID)
        {
            List<MISDoctorFees> MISReportMISDoctorFees = new List<MISDoctorFees>();
            try
            {
                Decimal TotalOPDComForREF = 0;
                Decimal TotalOPDComForCON = 0;
                Decimal TotalOPDComForREFCON = 0;
                Decimal TotalIPDComForREF = 0;
                Decimal TotalIPDComForCON = 0;
                Decimal TotalIPDComForREFCON = 0;
                Decimal TotalLABComForREF = 0;
                Decimal TotalLABComForCON = 0;
                Decimal TotalLABComForREFCON = 0;
                Decimal TotalComForEachDoc = 0;
                Decimal TotalCommission = 0;
               
                dsAllDoctorCommission.Reset();
                dsAllDoctorCommission = SelectAll(HospitalID, LocationID);

                foreach (DataRow dr in dsAllDoctorCommission.Tables[0].Rows)
                {
                    TotalOPDComForREF = 0;
                    TotalOPDComForCON = 0;
                    TotalOPDComForREFCON = 0;

                    TotalIPDComForREF = 0;
                    TotalIPDComForCON = 0;
                    TotalIPDComForREFCON = 0;

                    TotalLABComForREF = 0;
                    TotalLABComForCON = 0;
                    TotalLABComForREFCON = 0;
                    TotalComForEachDoc = 0;

                    DoctorID = dr["DoctorID"].ToString();
                    DoctorType = "REF";

                    dsOPDCommission1 = ReportMISDoctorCommissionSettingForPatients(DateFrom, DateTo, DoctorType, DoctorID);


                    if (dsOPDCommission1.Tables[0].Rows.Count > 0)
                    {
                        TotalOPDComForREF += Convert.ToDecimal(dsOPDCommission1.Tables[0].Compute("Sum(Fees)", string.Empty));
                    }


                  
                    if (dsOPDCommission1.Tables[1].Rows.Count > 0)
                    {
                        TotalIPDComForREF += Convert.ToDecimal(dsOPDCommission1.Tables[1].Compute("Sum(Fees)", string.Empty));
                    }


                   

                    if (dsOPDCommission1.Tables[2].Rows.Count > 0)
                    {
                        TotalLABComForREF += Convert.ToDecimal(dsOPDCommission1.Tables[2].Compute("Sum(Fees)", string.Empty));
                    }

                    DoctorType = "CON";

                    dsOPDCommission2 = ReportMISDoctorCommissionSettingForPatients(DateFrom, DateTo, DoctorType, DoctorID);




                    if (dsOPDCommission2.Tables[0].Rows.Count > 0)
                    {
                        TotalOPDComForCON += Convert.ToDecimal(dsOPDCommission2.Tables[0].Compute("Sum(Fees)", string.Empty));
                    }

                   
                    if (dsOPDCommission2.Tables[1].Rows.Count > 0)
                    {
                        TotalIPDComForCON += Convert.ToDecimal(dsOPDCommission2.Tables[1].Compute("Sum(Fees)", string.Empty));
                    }

                    if (dsOPDCommission2.Tables[2].Rows.Count > 0)
                    {
                        TotalLABComForCON += Convert.ToDecimal(dsOPDCommission2.Tables[2].Compute("Sum(Fees)", string.Empty));
                    }
                    TotalOPDComForREFCON = Math.Round((TotalOPDComForREF + TotalOPDComForCON), 2);
                    TotalIPDComForREFCON = Math.Round((TotalIPDComForREF + TotalIPDComForCON), 2);
                    TotalLABComForREFCON = Math.Round((TotalLABComForREF + TotalLABComForCON), 2);
                    TotalComForEachDoc = Math.Round((TotalOPDComForREFCON + TotalIPDComForREFCON + TotalLABComForREFCON), 2);
                    TotalCommission += TotalComForEachDoc;

                    dr["OPD Commission"] = TotalOPDComForREFCON.ToString();
                    dr["IPD Commission"] = TotalIPDComForREFCON.ToString();
                    dr["LAB Commission"] = TotalLABComForREFCON.ToString();
                    dr["Total Commission"] = TotalComForEachDoc.ToString();


                }
                foreach (DataRow dr in dsAllDoctorCommission.Tables[0].Rows)
                {
                    if (Convert.ToDecimal(dr["OPD Commission"]) == 0)
                    {
                        dr["OPD Commission"] = "0.00";
                    }
                    if (Convert.ToDecimal(dr["IPD Commission"]) == 0)
                    {
                        dr["IPD Commission"] = "0.00";
                    }
                    if (Convert.ToDecimal(dr["LAB Commission"]) == 0)
                    {
                        dr["LAB Commission"] = "0.00";
                    }
                    if (Convert.ToDecimal(dr["Total Commission"]) == 0)
                    {
                        dr["Total Commission"] = "0.00";
                    }
                
                        MISReportMISDoctorFees.Add(
                        new MISDoctorFees
                        {

                            DoctorID = Convert.ToString(dr["DoctorID"]),
                            DoctorName = Convert.ToString(dr["Doctor Name"]),
                            DoctorType = Convert.ToString(dr["DoctorType"]),
                            OPDCommission = Convert.ToString(dr["OPD Commission"]),
                            IPDCommission = Convert.ToString(dr["IPD Commission"]),
                            LABCommission = Convert.ToString(dr["LAB Commission"]),
                            TotalCommission = Convert.ToString(dr["Total Commission"]),


                        });


                }
              

            }
            catch (Exception ex)
            {

            }
            return MISReportMISDoctorFees;
        }

    }
}