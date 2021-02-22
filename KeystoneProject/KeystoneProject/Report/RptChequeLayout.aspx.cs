using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Report
{
    public partial class RptChequeLayout : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                KeystoneProject.Models.FinancialAccount.ChequeLayout obj = new Models.FinancialAccount.ChequeLayout();

                // ChequePrint rpt1 = new ChequePrint();
                // DataSet dsChequeLayout = new System.Data.DataSet();
                obj.dsChequeLayout = new System.Data.DataSet();
                obj.dsChequeLayout = (DataSet)Session["dsChequeLayout"];
                obj.dsChequeLayout.Tables[0].Rows[0]["BankID"] = obj.BankID;
                //dr["BackImage"] = "Santosh Gupta";
                obj.txtHieght =Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["ImageHieght"]);
                obj.txtWidth =  Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["IMageWidth"]);
                obj.DateL = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Date_L"].ToString());
                obj.DateT = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Date_T"].ToString());
                obj.PayeeL = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Payee_L"].ToString());
                obj.PayeeT = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Payee_T"].ToString());
                obj.AmtInWord1L = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord1_L"].ToString());
                obj.AmtINWord1T = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord1_T"].ToString());
                obj.AmtINWord2L = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord2_L"].ToString());
                obj.AmtInWord2T = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["AmtWord2_T"].ToString());
                obj.AmountL = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Amt_L"].ToString());
                obj.AmountT = Convert.ToInt32(obj.dsChequeLayout.Tables[0].Rows[0]["Amt_T"].ToString());
                MemoryStream stream = new MemoryStream((byte[])obj.dsChequeLayout.Tables[0].Rows[0]["BackImage"]);

                //obj.dsChequeLayout.Tables[0].Rows[0]["BackImage"] = "";
                //obj.dsChequeLayout.Tables[0].Columns["BackImage"].DataType = typeof(string);
                obj.Flag = false;
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "ChequePrint.rpt"));
                rd.SetDataSource(obj.dsChequeLayout);


                BlobFieldObject objBlobFieldObject = (BlobFieldObject)rd.ReportDefinition.Sections[0].ReportObjects["BackImage1"];
                objBlobFieldObject.Height = Convert.ToInt32(obj.txtHieght);
                objBlobFieldObject.Width = Convert.ToInt32(obj.txtWidth);

                TextObject objPayName = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["Payeename"];
                objPayName.Left =Convert.ToInt32( Session["PayeeL"]);
                objPayName.Top = Convert.ToInt32( Session["PayeeT"]);

                TextObject objDate = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["Date"];
                objDate.Left = Convert.ToInt32( Session["DateL"]);
                objDate.Top = Convert.ToInt32(Session["DateT"]);

                TextObject objAmountInWord1 = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["AmountInWord1"];
                objAmountInWord1.Left = Convert.ToInt32( Session["AmtInWord1L"]);
                objAmountInWord1.Top = Convert.ToInt32( Session["AmtINWord1T"]);

                TextObject objAmountInWord2 = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["AmountInWord2"];
                objAmountInWord2.Left = Convert.ToInt32( Session["AmtINWord2L"]);
                objAmountInWord2.Top = Convert.ToInt32( Session["AmtInWord2T"]);

                TextObject objAmount = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["Amount"];
                objAmount.Left = Convert.ToInt32( Session["AmountL"]);
                objAmount.Top = Convert.ToInt32(Session["AmountT"]);
                CrystalReportViewer1.ReportSource = rd;
                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ChequeLayout.pdf");
                Response.End();
                CrystalReportViewer1.DataBind();

                CrystalReportViewer1.RefreshReport();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
        }
    }
}