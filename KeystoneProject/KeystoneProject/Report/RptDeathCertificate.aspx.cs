using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
    public partial class RptDeathCertificate : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            int HospitalID = Convert.ToInt32(Session["HospitalID"].ToString());
            int LocationID = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientDeathCertificateID = Convert.ToInt32(Session["PatientDeathCertificateID"].ToString());

            List<byte[]> files = new List<byte[]>();

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            DataSet dsDischargeSummary = new DataSet();

            SqlCommand cmd = new SqlCommand("RptPatientDeathCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientDeathCertificateID", PatientDeathCertificateID);
         


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
           
            da.Fill(ds);
            if (ds.Tables[1].Rows.Count != 0)
            {
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientDeathCertificate.rpt"));

                rd.SetDataSource(ds);
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                files.Add(PrepareBytes(stream));
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