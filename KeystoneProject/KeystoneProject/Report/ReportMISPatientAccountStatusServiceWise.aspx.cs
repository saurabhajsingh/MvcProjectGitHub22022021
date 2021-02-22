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
    public partial class ReportMISPatientAccountStatusServiceWise1 : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ReportMISPatientAccountStatus"];

            List<byte[]> files = new List<byte[]>();

            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISPatientAccountStatusServiceWise.rpt"));
            rd.SetDataSource(ds);

            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISPatientAccountStatusServiceWise.pdf");
            Response.End();
        }
    }
}