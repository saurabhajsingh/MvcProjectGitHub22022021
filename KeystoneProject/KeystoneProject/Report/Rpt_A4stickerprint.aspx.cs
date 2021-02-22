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
    public partial class Rpt_A4stickerprint : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());

            //int PatientRegNo = 2;
            //int PatientIPDNo = 1;

            int PatientRegNo = Convert.ToInt32(Session["PatientRegNoReport"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNoReport"].ToString());
            string ReportTypeName = Session["ReportType"].ToString();
            string WardName = Session["WardName"].ToString();
            int Qty = Convert.ToInt32(Session["Quantity"].ToString());

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("RptAdmissionForm", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            for (int a = 0; a < Qty / 1; a++)
            {
                ds.Tables[2].Rows.Add(ds.Tables[1].Rows[0]["PatientName"].ToString(), ds.Tables[1].Rows[0]["Age"].ToString(), ds.Tables[1].Rows[0]["AddmissionDate"].ToString(), ds.Tables[1].Rows[0]["PrintRegNO"].ToString(),  ds.Tables[1].Rows[0]["BedNo"].ToString(), ds.Tables[1].Rows[0]["ConsultantDr"].ToString(), ds.Tables[1].Rows[0]["Gender"].ToString(), ds.Tables[1].Rows[0]["PatientIPDNO"].ToString());                
            }

            List<byte[]> files = new List<byte[]>();
            
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "A4size_sticker.rpt"));
            rd.SetDataSource(ds);

            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            files.Add(PrepareBytes(stream));


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