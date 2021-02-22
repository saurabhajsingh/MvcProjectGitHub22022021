using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using  KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Financial;
namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PrebalanceAmount
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

        public int PatientAccountRowID = 0;

        #region SavePrebalanceAmount
        public PreBalanceAmount SavePrebalAmt(PreBalanceAmount PreBalAmt)
        {
            PreBalAmt.TpaParticular = "";
            if (PreBalAmt.chkTPA == "true" && PreBalAmt.TDSAmount>0)
            {
               PreBalAmt.TpaParticular = "TPA";
            }

           // PreBalAmt.TpaParticular = "";
            if (PreBalAmt.chkTPA == "true" && PreBalAmt.TPAOtherDeduction > 0)
            {
                PreBalAmt.TpaParticular = "TPA";
            }
            int i = 0;
            string[] tblPaymentType = PreBalAmt.PaymentType.Split(',');
            string[] tblName = PreBalAmt.Name.Split(',');
            string[] tblDate = PreBalAmt.Date.Split(',');
            string[] tblRemarks = PreBalAmt.Remarks.Split(',');
            string[] tblNumber = PreBalAmt.Number.Split(',');
            string[] tblPaid = PreBalAmt.PaidAmount.Split(',');
            Connect();
            int PatientRegNo = Convert.ToInt32(PreBalAmt.PatientRegNo);

            DataSet dsPatientPrintIPDOpdID = new DataSet();
            dsPatientPrintIPDOpdID = GetRefoundIPDOPDID(PatientRegNo);

            DataSet dsPatientIPDOpdID = new DataSet();
            string[] PrintOPDIPDNoAndType = PreBalAmt.OPDIPDNO.Split('-');
          //  PreBalAmt.OPDIPDNO.Split()
            string PatientType=PrintOPDIPDNoAndType[1];
           
            string PrintOPDIPDNo = dsPatientPrintIPDOpdID.Tables[0].Rows[0]["OPDIPDNO"].ToString();
          //  string PrintOPDIPDNo = PrintOPDIPDNoAndType[0];
           // string PrintOPDIPDNo = dsPatientPrintIPDOpdID.Tables[0].Rows[0]["OPDIPDNO"].ToString();
            dsPatientIPDOpdID = GetIPDOPDIDForPrintIPDNo(PatientRegNo, Convert.ToInt32(PrintOPDIPDNo), PatientType);
            try
            {
                con.Open();
                if (dsPatientIPDOpdID.Tables[0].Rows.Count > 0)
                {
                   // PreBalAmt.OPDIPDID = dsPatientIPDOpdID.Tables[0].Rows[0]["[OPD/IPDID]"].ToString();
                    PreBalAmt.OPDIPDID = PrintOPDIPDNoAndType[0];
                    //  PreBalAmt.PatinetType = dsPatientIPDOpdID.Tables[0].Rows[0]["PatientType"].ToString();
                    PreBalAmt.PatinetType = PatientType;
                }
                //PreBalAmt.Name = "";
                //PreBalAmt.Number = "";
                //PreBalAmt.Remarks = "";
                //PreBalAmt.Date = "";
                for (int j = 0; j < tblPaymentType.Length; j++)
                {
                 PreBalAmt.Name=   tblName[j];
                 PreBalAmt.Number = tblNumber[j];
                 PreBalAmt.Remarks = tblRemarks[j];
                 PreBalAmt.Date = tblDate[j];
                 PreBalAmt.PaidAmount = tblPaid[j];

                 decimal PaidAmount = Convert.ToDecimal(tblPaid[j]);
                    if(PreBalAmt.BillType== "IPD SecurityDeposit RefundBills" || PreBalAmt.BillType == "IPD RefundBills" || PreBalAmt.BillType == "OPD RefundBills")
                    {
                        if (PreBalAmt.BillType == "IPD SecurityDeposit RefundBills")
                        {
                            PreBalAmt.BillType = "SecurityDeposit RefundBills";
                        }
                        else
                        {
                            PreBalAmt.BillType = "RefundBills";
                        }

                        SqlCommand cmd = new SqlCommand("IURefoundAmount", con);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);

                        cmd.Parameters.AddWithValue("@LocationID", LocationID);

                        cmd.Parameters.AddWithValue("@PatientAccountRowID", PreBalAmt.PatientAccountRowID);
                        if (PreBalAmt.BillType == "SecurityDeposit RefundBills")
                        {
                            cmd.Parameters.AddWithValue("@BillType", PreBalAmt.BillType);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@BillType", PreBalAmt.BillType);
                        }


                     //   cmd.Parameters["@PatientAccountRowID"].Direction = ParameterDirection.Output;

                        cmd.Parameters.AddWithValue("@PatientRegNo", PreBalAmt.PatientRegNo);

                        cmd.Parameters.AddWithValue("@PaidAmount", Convert.ToDecimal( PreBalAmt.PaidAmount));

                        cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(PreBalAmt.BillDate).ToString("dd-MMM-yyyy"));

                        cmd.Parameters.AddWithValue("@PaymentType", PreBalAmt.PaymentType);
                        cmd.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDNO.Split('-')[0]);
                        switch (PreBalAmt.PaymentType)
                        {

                            case "Cheque":
                                cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                                cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                                break;
                            case "Dedit Card":
                                cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                                cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                                break;
                            case "Credit Card":
                                cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                                cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                                break;
                            case "E-Money":
                                cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                                cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                                break;
                            case "EFT":
                                cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                                cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                                break;
                            default:
                                cmd.Parameters.AddWithValue("@Number", "Cash");
                                cmd.Parameters.AddWithValue("@Name", "Cash");
                                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);
                                cmd.Parameters.AddWithValue("@Remarks", "Cash");
                                break;
                        }

                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                       // con.Open();
                        i = cmd.ExecuteNonQuery();
                       // int OtherAccountRowID = Convert.ToInt32(cmd.Parameters["@PatientAccountRowID"].Value);

                    }
                    else
                    { 
                    SqlCommand cmd = new SqlCommand("IUPreBalanceAmount", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@TpaParticular", PreBalAmt.TpaParticular);

                    if (PreBalAmt.Mode != "Add")
                    {
                        cmd.Parameters.AddWithValue("@PatientAccountRowID", PreBalAmt.PatientAccountRowID);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PatientAccountRowID", 0);
                        cmd.Parameters["@PatientAccountRowID"].Direction = ParameterDirection.Output;
                    }
                    cmd.Parameters.AddWithValue("@PatientRegNo", PreBalAmt.PatientRegNo);
                    if (Convert.ToString(PreBalAmt.OPDIPDID) == null)
                    {
                        cmd.Parameters.AddWithValue("@OPDIPDID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDID);
                    }
                    if (PreBalAmt.PatinetType != null)
                    {
                         if (PreBalAmt.TPAStatus == "on")
                         {
                        cmd.Parameters.AddWithValue("@PatinetType", PreBalAmt.PatinetType +" "+"SecurityDeposit");
                         }
                        else
                         {
                         cmd.Parameters.AddWithValue("@PatinetType", PreBalAmt.PatinetType);

                         }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PatinetType", "");
                    }

                    cmd.Parameters.AddWithValue("@PaymentType", tblPaymentType[j]);
                    cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(PreBalAmt.BillDate));
                   
                    cmd.Parameters.AddWithValue("@PaidAmount", PaidAmount);
                    if (PreBalAmt.Narrection == null)
                    {
                        cmd.Parameters.AddWithValue("@Narrection", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Narrection", PreBalAmt.Narrection);
                    }
                    if (PreBalAmt.TDSAmount != null)
                    {
                        
                           cmd.Parameters.AddWithValue("@TDSAmount",PreBalAmt.TDSAmount);
                    }
                    else
	                {
                        cmd.Parameters.AddWithValue("@TDSAmount","");
	                }
                    if (PreBalAmt.TPAOtherDeduction != null)
                    {
                        cmd.Parameters.AddWithValue("@TPAOtherDeduction", PreBalAmt.TPAOtherDeduction);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TPAOtherDeduction", "");
                    }
                   

                    #region PaymentType
                    if (PreBalAmt.Name == null)
                    {
                        PreBalAmt.Name = "";
                    }
                    if (PreBalAmt.Remarks == null)
                    {
                        PreBalAmt.Remarks = "";
                    }

                    switch (tblPaymentType[j])
                    {
                        case "Cheque":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        case "Debit Card":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        case "Credit Card":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        case "E-Money":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        case "EFT":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        default:
                            cmd.Parameters.AddWithValue("@Number", "Cash");
                            cmd.Parameters.AddWithValue("@Name", "Cash");
                            cmd.Parameters.AddWithValue("@Remarks", "Cash");
                            cmd.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                            break;
                    }
                    #endregion

                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@Mode", PreBalAmt.Mode);
                   
                     i = cmd.ExecuteNonQuery();
                     int PatientAccountRowID = Convert.ToInt32(cmd.Parameters["@PatientAccountRowID"].Value);
                     PreBalAmt.PatientAccountRowID = PatientAccountRowID;
                     PreBalAmt.PrintPaymentTypeCount+= PatientAccountRowID.ToString()+ ",";
                }

                    if (PreBalAmt.TPAStatus == "on")
                    {
                        if(PreBalAmt.Mode == "Add")
                        {
                            SqlCommand cmdSecurityDeposite = new SqlCommand("IUSecurityDeposite", con);
                            cmdSecurityDeposite.CommandType = CommandType.StoredProcedure;
                            cmdSecurityDeposite.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@PatientRegNO", PreBalAmt.PatientRegNo);
                            cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", 0);
                            cmdSecurityDeposite.Parameters["@SecurityDeposityID"].Direction = ParameterDirection.Output;
                            cmdSecurityDeposite.Parameters.AddWithValue("@PatientAccountRowID", PreBalAmt.PatientAccountRowID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Add");
                            cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", "Cash");
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", "");
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", "");
                            // PatientIPDDetails.Parameters.AddWithValue("@Date", "");//ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;
                            //PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(obj.AddmissionDate, "dd/MM/yyyy hh:mm:ss", null)));
                            cmdSecurityDeposite.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", "");
                            cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "IPDSecurityDeposit PreBalanceBills");
                            //cmdSecurityDeposite.Parameters.AddWithValue("@PrintSecurityDeposite", obj.PrintSecurityDeposite);
                            cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", PreBalAmt.PaidAmount);
                            cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", 00);
                            cmdSecurityDeposite.Parameters.AddWithValue("@FinancialYearID", 2);
                            cmdSecurityDeposite.Parameters.AddWithValue("@CreationID", UserID);
                            //  cmdSecurityDeposite.Parameters.AddWithValue("Mode", Mode);
                            //    con.Open();

                            int Value = cmdSecurityDeposite.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand cmdSecurityDeposite = new SqlCommand("IUSecurityDeposite", con);
                            cmdSecurityDeposite.CommandType = CommandType.StoredProcedure;
                            cmdSecurityDeposite.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@PatientRegNO", PreBalAmt.PatientRegNo);
                            cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", 0);
                            cmdSecurityDeposite.Parameters["@SecurityDeposityID"].Direction = ParameterDirection.Output;
                            cmdSecurityDeposite.Parameters.AddWithValue("@PatientAccountRowID", PreBalAmt.PatientAccountRowID);
                            cmdSecurityDeposite.Parameters.AddWithValue("@Mode", "Edit");
                            cmdSecurityDeposite.Parameters.AddWithValue("@PaymentType", "Cash");
                            cmdSecurityDeposite.Parameters.AddWithValue("@Number", "");
                            cmdSecurityDeposite.Parameters.AddWithValue("@Name", "");
                            // PatientIPDDetails.Parameters.AddWithValue("@Date", "");//ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;
                            //PatientIPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(obj.AddmissionDate, "dd/MM/yyyy hh:mm:ss", null)));
                            cmdSecurityDeposite.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                            cmdSecurityDeposite.Parameters.AddWithValue("@Remark", "");
                            cmdSecurityDeposite.Parameters.AddWithValue("@BillType", "IPDSecurityDeposit PreBalanceBills");
                            //cmdSecurityDeposite.Parameters.AddWithValue("@PrintSecurityDeposite", obj.PrintSecurityDeposite);
                            cmdSecurityDeposite.Parameters.AddWithValue("@DrAmount", PreBalAmt.PaidAmount);
                            cmdSecurityDeposite.Parameters.AddWithValue("@CrAmount", 00);
                            cmdSecurityDeposite.Parameters.AddWithValue("@FinancialYearID", 2);
                            cmdSecurityDeposite.Parameters.AddWithValue("@CreationID", UserID);
                            //  cmdSecurityDeposite.Parameters.AddWithValue("Mode", Mode);
                            //    con.Open();

                            int Value = cmdSecurityDeposite.ExecuteNonQuery();
                        }
                       
                    }
                }


                    con.Close();
                    if (i > 0)
                    {
                    //    PreBalAmt.PatientAccountRowID = PatientAccountRowID;
                        return PreBalAmt;
                    }
                    else
                    {
                        return null;
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get PatientName
        public DataSet GetPatientName1(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo,PrintRegNO,PatientName,PatientType from Patient where  PatientRegNo like '" + PatientName + '%' + "' and HospitalID = '" + HospitalID + "' and LocationID ='" + LocationID + "' and RowStatus = 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo,PrintRegNO,PatientName,PatientType from Patient where PatientName like '" + PatientName + '%' + "' and HospitalID = '" + HospitalID + "' and LocationID ='" + LocationID + "' and RowStatus = 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        #region Get PatientRegNo
        public DataSet GetPatientRegNo(int PatientRegNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO ,upper(Patient.PatientName) as PatientName ,Patient.PatientRegNO as RegNO, Patient.PatientType as PatientType,Patient.FinancialYearID from Patient where  Patient.PatientRegNO = " + PatientRegNo + " and Patient.HospitalID = '" + HospitalID + "' and Patient.LocationID = '" + LocationID + "' and Patient.RowStatus= 0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        public string GetPrintNo_ToRegNo(string PrintRegNO)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select * from Patient where  Patient.PrintRegNO=" + PrintRegNO + " and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string RegNo = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                RegNo = ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            }
            return RegNo;
        }
        public string GetPassword(string Password)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select top 1 Password from MasterSetting where   HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0 ", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string pass = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                pass = ds.Tables[0].Rows[0]["Password"].ToString();
            }
            return pass;
        }
        public DataSet GetRefoundAmount(int PatientRegNO)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetRefoundAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetIPDPatient(string GetIPDPatient)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName ,Address, MobileNo,PrintRegNO from patient where PatientName like ''+@GetIPDPatient+'%'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            cmd.Parameters.AddWithValue("@GetIPDPatient", GetIPDPatient);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }
        public DataSet GetRefoundIPDOPDID(int PatientRegNO)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetRefoundIPDOPDID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet RptPatientIPDAdvanceAmount(int PatientRegNO,int PatientIPDNo)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("RptPatientIPDAdvanceAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            HttpContext.Current.Session["RptPatientIPDAdvanceAmount"] = ds;
            con.Close();
            return ds;
        }
        public DataSet GetIPDOPDIDForPrintIPDNo(int PatientRegNO, int PrintIPDOPDNo, string PatientType)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetIPDOPDIDForPrintIPDNo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
            cmd.Parameters.AddWithValue("@PrintIPDOPDNO", PrintIPDOPDNo);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public string DeletePreBalanceAmount(int PatientAccountRowID,string DeleteReason, String BillType, int RegNo, string SecurityDeposityID)
        
        {
            Connect();
            string Result = null;
            try
            {
                SqlCommand cmd = new SqlCommand("DeletePreBalance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientAccountRowID", PatientAccountRowID);
                cmd.Parameters.AddWithValue("@DeleteReason", DeleteReason);
                con.Open();
                Result = cmd.ExecuteNonQuery().ToString();
              //  con.Close();
                if (BillType == "IPD SecurityDeposit PreBalanceBills")
                {
                    cmd = new SqlCommand("update SecurityDepositeTPA set RowStatus=1 where PatientRegNo=" + RegNo + " and RowStatus=0 and HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and BillType='IPDSecurityDeposit PreBalanceBills'  and  SecurityDeposityID="+SecurityDeposityID+"", con);
                    cmd.ExecuteNonQuery();

                }
                con.Close();
                #region ForAuthorization
             //   int ForAuthorization = 0;
                // Aurthorise
                KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj4 = new Buisness_Logic.Master.BL_MasterSetting();
                DataSet dsMasterSetting = new DataSet();
                dsMasterSetting = obj4.GetMasterSetting();
                KeystoneProject.Buisness_Logic.Hospital.BL_Users user = new Buisness_Logic.Hospital.BL_Users();
                DataSet dsAuthorizationRights = new DataSet();
                // List<Users> Search = new List<Users>();
                dsAuthorizationRights = user.GetUsers(UserID);
                string chkAurthoriseUserWise = dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();
               string ForAuthorization = "0";
                if (chkAurthoriseUserWise == "True")
                {
                    dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                    ForAuthorization = "0";
                }
                if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
                {
                   ForAuthorization = "1";
                    con.Close();
                    SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                    cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                    cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                     cmdIUForAuthorization.Parameters.AddWithValue("@PatientAccountRowID", PatientAccountRowID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@UserID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", "change");
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", 0);
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillType", BillType);
                    cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", RegNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                    con.Open();
                    cmdIUForAuthorization.ExecuteNonQuery();

                }
                #endregion


                return Result;
            }
            catch (Exception)
            {
                return Result;
            }
        }

        public List<PreBalanceAmount> GetRefoundIPDOPDID(string PatientRegNo)
        {

            Connect();
        
            List<PreBalanceAmount> patientnamelist = new List<PreBalanceAmount>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[0].Value = PatientRegNo;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetRefoundIPDOPDID", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string OPDIPDNO = dr["OPDIPDNO"].ToString() + "-" + dr["PatientType"].ToString();
                    patientnamelist.Add(
                        new PreBalanceAmount
                        {

                            PrintOPDIPDNo = OPDIPDNO,
                            //PatientName = dr["PatientName"].ToString(),
                            //DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                            //CrAmount = Convert.ToDecimal(dr["DrAmount"]),
                            //BalanceAmount = dr["BalanceAmount"].ToString(),
                            OPDIPDNO = dr["OPD/IPDID"].ToString() + "-" + dr["PatientType"].ToString(),
                            //Date = Convert.ToDateTime(dr["BillDate"]).ToString("dd-MM-yyyy"),
                            //PatinetType = dr["PatientType"].ToString()
                        });

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }

                
            return patientnamelist;
        }
        public DataSet GetRefoundAmountPatientAccountModyFyModyFy( int PatientRegNo)
        {
           
                DataSet ds = new DataSet();
                try
                {
                Connect();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                    param[0].Value = PatientRegNo;
                    param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[1].Value = HospitalID;
                    param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[2].Value = LocationID;

                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetRefoundAmountPatientAccountModyFyModyFy", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        
        public List<PreBalanceAmount> GetRefoundAmountOPDIPD(string PrintIPDOPDNo, string PatientType)
        {
            Connect();
            List<PreBalanceAmount> search = new List<PreBalanceAmount>();

            SqlCommand cmd = new SqlCommand("GetRefoundAmountOPDIPD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OPDIPDNo", PrintIPDOPDNo);
            cmd.Parameters.AddWithValue("@PatientType", PatientType);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
              foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (dr["TDSAmount"].ToString() == "" || dr["TPAOtherDeduction"].ToString() == "")
                    {
                        dr["TDSAmount"] = 0;
                        dr["TPAOtherDeduction"] = 0;
                    }
                    search.Add(new PreBalanceAmount {
                        BalanceAmount = ds.Tables[0].Rows[0]["PreBalance"].ToString(),

                     OPDIPDNO=   dr["OPDIPDNO"].ToString(), 
                     BillType=   dr["BillType"].ToString(), 
                       DrAmount= Convert.ToDecimal( dr["DrAmount"].ToString()), 
                       P_BillNo= dr["P_BillNo"].ToString(),
                     TDSAmount = Convert.ToDecimal(dr["TDSAmount"].ToString()),
                     TPAOtherDeduction = Convert.ToDecimal(dr["TPAOtherDeduction"].ToString()),
                      CrAmount=   Convert.ToDecimal(dr["CrAmount"].ToString()), 
                       BillDate= Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                      BillNo=  dr["BillNo"].ToString(), 
                     P_RegNo=   dr["P_RegNo"].ToString(),
                      PaymentType=  dr["PaymentType"].ToString(),
                    OPDIPDID=   dr["OPD/IPDID"].ToString(),
                        PatientName = dr["PatientName"].ToString(),
                      PatientAccountRowID= Convert.ToInt32( dr["PatientAccountRowID"].ToString()),
                        PatinetType = dr["PatientType"].ToString(),
                    });
                }
              return search;
            }
            
       
        
    }
}