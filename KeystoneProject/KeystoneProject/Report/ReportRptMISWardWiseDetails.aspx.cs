
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
    public partial class ReportRptMISWardWiseDetails : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["RptMISWardWiseDetails"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptMISWardWiseDetails.rpt"));
                rd.SetDataSource(ds);

                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptMISWardWiseDetails.pdf");
                Response.End();
            }
        }
    }
}