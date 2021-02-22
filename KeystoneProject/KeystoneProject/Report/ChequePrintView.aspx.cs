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
    public partial class ChequePrintView : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        RptPatientPrescriptionNew1 rd1 = new RptPatientPrescriptionNew1();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
           ds= GetChequeLayoutForCheckPrinttblClick(3);
          
            //  report context = new report();
           rd.Load(Path.Combine(Server.MapPath("~/Report"), "ChequePrint.rpt"));
           rd.SetDataSource(ds);
           TextObject objACPaye = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["ACPaye"];
           objACPaye.Text = "A/C Payee";

           MemoryStream stream = new MemoryStream((byte[])ds.Tables[0].Rows[0]["BackImage"]);
     
            BlobFieldObject objBlobFieldObject = (BlobFieldObject)rd.ReportDefinition.Sections[0].ReportObjects["BackImage1"];
            objBlobFieldObject.Height = Convert.ToInt32(ds.Tables[0].Rows[0]["ImageHieght"]);
            objBlobFieldObject.Width = Convert.ToInt32(ds.Tables[0].Rows[0]["IMageWidth"]);

            TextObject objPayName = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["Payeename"];
            objPayName.Left = Convert.ToInt32(ds.Tables[0].Rows[0]["Payee_L"]);
            objPayName.Top = Convert.ToInt32(ds.Tables[0].Rows[0]["Payee_T"]);
            TextObject objDate = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["Date"];
            objDate.Left = Convert.ToInt32(ds.Tables[0].Rows[0]["Date_L"]);
            objDate.Top = Convert.ToInt32(ds.Tables[0].Rows[0]["Date_T"]);
         
            TextObject objAmountInWord1 = (TextObject)rd.ReportDefinition.Sections[0].ReportObjects["AmountInWord1"];
            objAmountInWord1.Left = Convert.ToInt32(ds.Tables[0].Rows[0]["AmtWord1_L"]);
            objAmountInWord1.Top = Convert.ToInt32(ds.Tables[0].Rows[0]["AmtWord1_T"]);
           // rd.SetDataSource(ds);
          
            CrystalReportViewer1.ReportSource = rd;

            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ChequePrint.pdf");
            Response.End();
            CrystalReportViewer1.DataBind();
            
            CrystalReportViewer1.RefreshReport();
            
            //  rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Crystal.pdf");
        }

        public DataSet GetChequeLayoutForCheckPrinttblClick(int BooKNameID)
        {
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
         //   con = new SqlConnection("server=GOVIND;uid=sa;pwd=pass@123;database=Live1516");
            //int HospitalID = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            //int LocationID = Convert.ToInt32(Session["LocationIDReport"].ToString());
            //int PatientRegNo = Convert.ToInt32(Session["PatientRegNoReport"].ToString());
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ChequeBookID", SqlDbType.Int);
                param[0].Value = Session["chqueBookNameID"];

                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = 1;

                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = 1;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetChequeLayoutForCheckPrint", param);
            
            return ds;
        }
            
        
    }
}