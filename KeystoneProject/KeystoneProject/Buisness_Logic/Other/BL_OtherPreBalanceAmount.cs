using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Other
{
    public class BL_OtherPreBalanceAmount
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


        public int PatientAccountRowID = 0;

        #region SavePrebalanceAmount
        public PreBalanceAmount SavePrebalAmt(PreBalanceAmount PreBalAmt)
        {
            PreBalAmt.TpaParticular = "";
            if (PreBalAmt.chkTPA == "true" && PreBalAmt.TDSAmount > 0)
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
         //   dsPatientPrintIPDOpdID = GetRefoundIPDOPDID(PatientRegNo);

            DataSet dsPatientIPDOpdID = new DataSet();
            string[] PrintOPDIPDNoAndType = PreBalAmt.OPDIPDNO.Split('-');
          //  string PatientType = PrintOPDIPDNoAndType[1];
           // string PrintOPDIPDNo = PrintOPDIPDNoAndType[0];
          //  dsPatientIPDOpdID = GetIPDOPDIDForPrintIPDNo(PatientRegNo, Convert.ToInt32(PrintOPDIPDNo), PatientType);
            try
            {
                con.Open();
                //if (dsPatientIPDOpdID.Tables[0].Rows.Count > 0)
                //{
                //    PreBalAmt.OPDIPDID = dsPatientIPDOpdID.Tables[0].Rows[0]["[OPD/IPDID]"].ToString();
                //    PreBalAmt.PatinetType = dsPatientIPDOpdID.Tables[0].Rows[0]["PatientType"].ToString();
                //}
                //PreBalAmt.Name = "";
                //PreBalAmt.Number = "";
                //PreBalAmt.Remarks = "";
                //PreBalAmt.Date = "";
                for (int j = 0; j < tblPaymentType.Length; j++)
                {
                    PreBalAmt.Name = tblName[j];
                    PreBalAmt.Number = tblNumber[j];
                    PreBalAmt.Remarks = tblRemarks[j];
                    PreBalAmt.Date = tblDate[j];
                    PreBalAmt.PaidAmount = tblPaid[j];

                    decimal PaidAmount = Convert.ToDecimal(tblPaid[j]);
                    SqlCommand cmd = new SqlCommand("IUOtherPreBalanceAmount", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                   // cmd.Parameters.AddWithValue("@TpaParticular", PreBalAmt.TpaParticular);

                    //if (PreBalAmt.Mode != "ADD")
                    //{
                    //    cmd.Parameters.AddWithValue("@PatientAccountRowID", PreBalAmt.PatientAccountRowID);
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@OtherAccountRowID", 0);
                    cmd.Parameters["@OtherAccountRowID"].Direction = ParameterDirection.Output;
                    // }
                    cmd.Parameters.AddWithValue("@PatientRegNo", PreBalAmt.PatientRegNo);
                    //if (Convert.ToString(PreBalAmt.OPDIPDID) == null)
                    //{
                    //    cmd.Parameters.AddWithValue("@OPDIPDID", DBNull.Value);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDID);
                    //}
                    //if (PreBalAmt.PatinetType != null)
                    //{
                    //    if (PreBalAmt.TPAStatus == "on")
                    //    {
                    //        cmd.Parameters.AddWithValue("@PatinetType", PreBalAmt.PatinetType + " " + "SecurityDeposite");
                    //    }
                    //    else
                    //    {
                    //        cmd.Parameters.AddWithValue("@PatinetType", PreBalAmt.PatinetType);

                    //    }
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@PatinetType", "");
                    //}

                    cmd.Parameters.AddWithValue("@PaymentType", tblPaymentType[j]);
                    cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(PreBalAmt.BillDate));
                    cmd.Parameters.AddWithValue("@PaidAmount", PaidAmount);
                    if (PreBalAmt.Narrection == null)
                    {
                        cmd.Parameters.AddWithValue("@Narration", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Narration", PreBalAmt.Narrection);
                    }
                    //if (PreBalAmt.TDSAmount != null)
                    //{

                    //    cmd.Parameters.AddWithValue("@TDSAmount", PreBalAmt.TDSAmount);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@TDSAmount", "");
                    //}
                    //if (PreBalAmt.TPAOtherDeduction != null)
                    //{
                    //    cmd.Parameters.AddWithValue("@TPAOtherDeduction", PreBalAmt.TPAOtherDeduction);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@TPAOtherDeduction", "");
                    //}


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

                        case "Debit":
                            cmd.Parameters.AddWithValue("@Number", PreBalAmt.Number);
                            cmd.Parameters.AddWithValue("@Name", PreBalAmt.Name);
                            cmd.Parameters.AddWithValue("@Remarks", PreBalAmt.Remarks);
                            cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(PreBalAmt.Date));
                            break;

                        case "Credit":
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
                    cmd.Parameters.AddWithValue("@Mode", "ADD");

                    i = cmd.ExecuteNonQuery();
                    int PatientAccountRowID = Convert.ToInt32(cmd.Parameters["@OtherAccountRowID"].Value);
                    PreBalAmt.PatientAccountRowID = PatientAccountRowID;
                    PreBalAmt.PrintPaymentTypeCount += PatientAccountRowID.ToString() + ",";
                }

                if (PreBalAmt.TPAStatus == "on")
                {
                    SqlCommand cmdSecurityDeposite = new SqlCommand("IUSecurityDeposite", con);
                    cmdSecurityDeposite.CommandType = CommandType.StoredProcedure;
                    cmdSecurityDeposite.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@PatientRegNO", PreBalAmt.PatientRegNo);
                    cmdSecurityDeposite.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDID);
                    cmdSecurityDeposite.Parameters.AddWithValue("@SecurityDeposityID", 0);
                    cmdSecurityDeposite.Parameters["@SecurityDeposityID"].Direction = ParameterDirection.Output;
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
        public DataSet GetPreBalanceAmountOther1(int PatientRegNo, string OPDIPDID)
        {
          
                DataSet ds1 = new DataSet();
                try
                {
                Connect();
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                    param[2].Value = PatientRegNo;
                   param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.NVarChar);
                    param[3].Value = OPDIPDID;

                    ds1 = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPreBalanceAmountOther1", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds1;
            
        }

        #region Delete

        public void DeleteOtherPreBalanceAmount(int OtherAccountRowID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[4];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@OtherAccountRowID", SqlDbType.Int);
                aParams[2].Value = OtherAccountRowID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteOtherPreBalance", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "--1");
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
    }
}