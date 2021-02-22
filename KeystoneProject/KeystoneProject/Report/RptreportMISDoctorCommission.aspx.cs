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

namespace KeystoneProject.Report
{
    public partial class RptreportMISDoctorCommission : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        ReportDocument rd2 = new ReportDocument();
        ReportDocument rd3 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ReportMISDoctorCommissionSettingForPatients"];
            DataSet ds1 = (DataSet)Session["MISReportMISDoctorFees"];

            List<byte[]> files = new List<byte[]>();

            string a = Session["CommistionType"].ToString();
            if (Session["CommistionType"].ToString() == "OPD")
            {

                if (ds.Tables[0].Rows.Count != 0)
                {
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISOPDPatientWiseDoctorCommissionReport.rpt"));
                    rd.SetDataSource(ds);
                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);

                    files.Add(PrepareBytes(stream));

                }
            }
            string a1 = Session["CommistionType"].ToString();
            if (Session["CommistionType"].ToString() == "IPD")
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    rd1.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISIPDPatientWiseDoctorCommissionReport.rpt"));
                    rd1.SetDataSource(ds);
                    Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);

                    files.Add(PrepareBytes(stream1));
                }

            }
            string a2 = Session["CommistionType"].ToString();
            if (Session["CommistionType"].ToString() == "LAB")
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    rd2.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISLABPatientWiseDoctorCommissionReport.rpt"));
                    rd2.SetDataSource(ds);
                    Stream stream2 = rd2.ExportToStream(ExportFormatType.PortableDocFormat);

                    files.Add(PrepareBytes(stream2));
                }

            }
            string a3 = Session["CommistionType"].ToString();
            if (Session["CommistionType"].ToString() == "ViewAllReport")
            {

                if (ds1.Tables[0].Rows.Count != 0)
                {
                    rd3.Load(Path.Combine(Server.MapPath("~/Report"), "ReportMISAllDoctorOPDIPDLABWiseCommissionReport.rpt"));
                    rd3.SetDataSource(ds1);
                    Stream stream3 = rd3.ExportToStream(ExportFormatType.PortableDocFormat);

                    files.Add(PrepareBytes(stream3));

                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            //merge the all reports & show the reports            
            Response.BinaryWrite(MergeReports(files).ToArray());

            Response.End();   
        }

        private byte[] PrepareBytes(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[stream.Length];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private MemoryStream MergeReports(List<byte[]> files)
        {
            if (files.Count > 1)
            {
                PdfReader pdfFile;
                Document doc;
                PdfWriter pCopy;
                MemoryStream msOutput = new MemoryStream();

                pdfFile = new PdfReader(files[0]);

                doc = new Document();
                pCopy = new PdfSmartCopy(doc, msOutput);

                doc.Open();

                for (int k = 0; k < files.Count; k++)
                {
                    pdfFile = new PdfReader(files[k]);
                    for (int i = 1; i < pdfFile.NumberOfPages + 1; i++)
                    {
                        ((PdfSmartCopy)pCopy).AddPage(pCopy.GetImportedPage(pdfFile, i));
                    }
                    pCopy.FreeReader(pdfFile);
                }

                pdfFile.Close();
                pCopy.Close();
                doc.Close();

                return msOutput;
            }
            else if (files.Count == 1)
            {
                return new MemoryStream(files[0]);
            }

            return null;
        }

    }
}