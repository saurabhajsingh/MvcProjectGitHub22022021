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
namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_Deposit
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

            SqlCommand cmd = new SqlCommand("IURefoundAmount", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);

            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            cmd.Parameters.AddWithValue("@PatientAccountRowID", 0);
            if (PreBalAmt.BillType == "SecurityDeposit RefundBills")
            {
                cmd.Parameters.AddWithValue("@BillType", PreBalAmt.BillType);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BillType", PreBalAmt.BillType);
            }


            cmd.Parameters["@PatientAccountRowID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@PatientRegNo", PreBalAmt.PatientRegNo);

            cmd.Parameters.AddWithValue("@PaidAmount", PreBalAmt.PaidAmount);

            cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(PreBalAmt.BillDate).ToString("dd-MMM-yyyy"));

            cmd.Parameters.AddWithValue("@PaymentType", PreBalAmt.PaymentType);
            cmd.Parameters.AddWithValue("@OPDIPDID", PreBalAmt.OPDIPDNO);
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
            cmd.Parameters.AddWithValue("@Mode", "ADD");
            con.Open();
            int i = cmd.ExecuteNonQuery();
            int OtherAccountRowID = Convert.ToInt32(cmd.Parameters["@PatientAccountRowID"].Value);
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
        public DataSet GetOtherRefundAmount(string PrintIPDOPDNo)
        {
            Connect();
            string[] PrintIPDOPDNo_PatientType = PrintIPDOPDNo.Split(',');

            SqlCommand cmd = new SqlCommand("GetRefoundAmountOPDIPD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OPDIPDNo", PrintIPDOPDNo_PatientType[0]);
            cmd.Parameters.AddWithValue("@PatientType", PrintIPDOPDNo_PatientType[1]);
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }
    }
}