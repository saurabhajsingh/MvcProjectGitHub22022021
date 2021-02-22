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


namespace KeystoneProject.Report
{
    public partial class RptVoucherEntryPrint : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            int HospitalID = Convert.ToInt32(Session["HospitalID"].ToString());
            int LocationID = Convert.ToInt32(Session["LocationID"].ToString());
           

            DataSet ds = (DataSet)Session["RptVoucherEntry"];

        
            
         

            List<byte[]> files = new List<byte[]>();

            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptVoucherEntry.rpt"));
            rd.SetDataSource(ds);

            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptVoucherEntry.pdf");
            Response.End();


        }
    }
}