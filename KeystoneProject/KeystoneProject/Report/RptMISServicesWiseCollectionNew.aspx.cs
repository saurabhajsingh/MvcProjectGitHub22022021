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
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KeystoneProject.Models.MISReport;
using KeystoneProject.Models.Report;
using KeystoneProject.Buisness_Logic.MISReport;

namespace KeystoneProject.Report
{
    public partial class RptMISServicesWiseCollectionNew : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ReportMISServicesWiseCollectionNew"];



            //con.Open();

            List<byte[]> files = new List<byte[]>();



            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISServicesWiseCollectionNew.rpt"));
            rd.SetDataSource(ds);


            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISServicesWiseCollectionNew.pdf");
            Response.End();
            // //   CrystalReportViewer1.DataBind();

            // //   CrystalReportViewer1.RefreshReport();

        }
    }
}