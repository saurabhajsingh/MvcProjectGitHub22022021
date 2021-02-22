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
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using KeystoneProject.Controllers.MISReport.MIS_PatientReports;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;

namespace KeystoneProject.Report
{
    public partial class RptMISHospitalProductPurchaseReportDetails : System.Web.UI.Page
    {
         ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ReportMISHospitalProductPurchaseReport"];
            List<byte> file = new List<byte>();
            string a = Session["Type"].ToString();
            if (a == "ID")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISHospitalProductPurchaseReportDetails.rpt"));
                    rd.SetDataSource(ds);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISHospitalProductPurchaseReportDetails.pdf");
                    Response.End();
                }
            }
            if(a=="All")
            {
            if (ds.Tables[0].Rows.Count > 0)
            {
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISHospitalProductPurchaseReport.rpt"));
                rd.SetDataSource(ds);

                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISHospitalProductPurchaseReport.pdf");
                Response.End();
            }
            }
        }
    }
}