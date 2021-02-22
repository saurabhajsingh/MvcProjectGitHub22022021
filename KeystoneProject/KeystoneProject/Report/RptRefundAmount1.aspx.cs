using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Report
{
    public partial class RptRefundAmount1 : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();

        # region VARIABLES
        /// <summary>
        /// Connection String Variable
        /// </summary>
        private string _sDbConnection = "";
        DataSet ds1 = new DataSet();
        DataSet dsBedChargeMerge = new DataSet();
        string ServiceNameOld;
        DataSet dsBedChargeold = new DataSet();
        string FromDate;
        string ToDate;
        decimal Rate = 0;
        string ser2;
        string ser3;



        decimal Amount = 0;
        int Qty = 0;
        string BillDate;
        string ServiceGroupName;
        string ChargesType;
        string DoctorPrintName;
        string A;
        int i = 1;
        # endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            int HospitalID = Convert.ToInt32(Session["HospitalID"]);
            int LocationID = Convert.ToInt32(Session["LocationID"]);

            int RowID = Convert.ToInt32(Session["OtherAccountRowID"]);
            string BillType = ""; //Session["BillTypePaymentAndDeposit"].ToString();
            DataSet ds = new DataSet();
            if (BillType == "IPDFinalBill")
            {

            }
            else
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptRefundAmount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@RowID", RowID);
                //cmd.Parameters.AddWithValue("@BillNo", 864);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //con.Open();
                da.Fill(ds);
            }

            if (ds.Tables[0].Rows.Count != 0)
            {
                String str = "";
                try
                {
                    str = "1";

                    str = "2";
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptRefundAmount.rpt"));
                    str = "3";
                    rd.SetDataSource(ds);
                    str = "4";
                    CrystalReportViewer1.ReportSource = rd;
                    str = "5";
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptRefundAmount.pdf");
                    Response.End();
                    CrystalReportViewer1.DataBind();
                    str = "6";
                    CrystalReportViewer1.RefreshReport();
                    str = "7";




                }
                catch (Exception ex)
                {
                    Response.Write(str);
                    //throw;
                }

            }
        }
    }
}