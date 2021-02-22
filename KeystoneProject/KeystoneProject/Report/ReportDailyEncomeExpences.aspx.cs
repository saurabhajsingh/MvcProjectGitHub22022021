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
    public partial class ReportDailyEncomeExpences : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["RptReportDailyEncomeExpences"];
            List<byte> file = new List<byte>();
            Session["Type"].ToString();
            if (Session["Type"].ToString() == "AllReport")
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISDailyIncomeExpences.rpt"));
                    rd.SetDataSource(ds);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISDailyIncomeExpences.pdf");
                    Response.End();
                //}
            }
            if (Session["Type"].ToString() == "All")
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISUserWiseDailyIncomeExpences.rpt"));
                    rd.SetDataSource(ds);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISUserWiseDailyIncomeExpences.pdf");
                    Response.End();
                //}
            }
            if (Session["Type"].ToString() == "Expenses")
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISParticularUserWiseDailyExpenses.rpt"));
                    rd.SetDataSource(ds);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISParticularUserWiseDailyExpenses.pdf");
                    Response.End();
                //}
            }
            if (Session["Type"].ToString() == "Income")
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISParticularUserWiseDailyIncome.rpt"));
                    rd.SetDataSource(ds);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ReportMISParticularUserWiseDailyIncome.pdf");
                    Response.End();
                //}
            }
        }
    }
}