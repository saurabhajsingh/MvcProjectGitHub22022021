using CrystalDecisions.CrystalReports.Engine;
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
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KeystoneProject.Report
{
    public partial class medicalBillReturnwebpage : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        CrystalReport1WebForm obj = new CrystalReport1WebForm();
        protected void Page_Load(object sender, EventArgs e)
        {
            int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            int medID = Convert.ToInt32(Session["medID"].ToString());

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("RptPatientMedicalBills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@MedicalBillID", medID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);
            List<byte[]> files = new List<byte[]>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientMedicalReturnBill.rpt"));
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

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
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