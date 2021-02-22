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
using System.Web.SessionState;

namespace KeystoneProject.Report
{
    public partial class RptPatientPrescriptionForOPD : System.Web.UI.Page
    {
        ReportDocument OPDRegister = new ReportDocument();

        int strHospital = 0;
        int strLocation = 0;
        int PatientRegNo = 0;
        int OpdNo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";

            strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            PatientRegNo = Convert.ToInt32(Session["Registration"].ToString());
            OpdNo = Convert.ToInt32(Session["OPDNo"].ToString());

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("RptPatientPrescritionForOPD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientOPDNo", OpdNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);

            SqlCommand cmd1 = new SqlCommand("HOSPNAME", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd1.Parameters.AddWithValue("@LocationID", strLocation);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            con.Open();
            da1.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                h_name = dt.Rows[0][0].ToString();
                h_email = dt.Rows[0][1].ToString();
            }

            List<byte[]> files = new List<byte[]>();
            
            if (ds.Tables[0].Rows.Count != 0)
            {

                OPDRegister.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientPrescriptionOPD.rpt"));
                OPDRegister.SetDataSource(ds);

                OPDRegister.SetParameterValue("Hosp_name", h_name);
                OPDRegister.SetParameterValue("Hosp_email", h_email);

                Stream stream = OPDRegister.ExportToStream(ExportFormatType.PortableDocFormat);
                files.Add(PrepareBytes(stream));
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
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
        
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.OPDRegister != null)
            {
                this.OPDRegister.Close();
                this.OPDRegister.Dispose();
            }
        } 
    }
}