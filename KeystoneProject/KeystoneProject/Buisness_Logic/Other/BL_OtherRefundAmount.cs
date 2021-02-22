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
    public class BL_OtherRefundAmount
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
        public int IURefoundAmount(Deposit PreBalAmt)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUOtherRefundAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@OtherAccountRowID", 0);


            cmd.Parameters["@OtherAccountRowID"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@PatientRegNo", PreBalAmt.PatientRegNo);
            cmd.Parameters.AddWithValue("@PaidAmount", PreBalAmt.PaidAmount);


            cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(PreBalAmt.BillDate).ToString("dd-MMM-yyyy"));
            cmd.Parameters.AddWithValue("@PaymentType", PreBalAmt.PaymentType);
          //  cmd.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDNO);
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
            cmd.Parameters.AddWithValue("@Mode", "Add");
            con.Open();
            int i = cmd.ExecuteNonQuery();
            int OtherAccountRowID = Convert.ToInt32(cmd.Parameters["@OtherAccountRowID"].Value);
            con.Close();
            if (OtherAccountRowID > 0)
            {
                //   PreBalAmt.OtherAccountRowID = OtherAccountRowID1;
                return OtherAccountRowID;
            }
            else
            {
                return OtherAccountRowID;
            }
        }

        public DataSet GetOtherRefundAmount( int PatientRegNo)
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
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetOtherRefundAmount", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            
        }

    }
}