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
    public partial class RptAdmissionForm : System.Web.UI.Page
    {

        ReportDocument Admission = new ReportDocument();
        ReportDocument Admission1 = new ReportDocument();


        int strHospital = 0;
        int strLocation = 0;
        int PatientRegNo = 0;
        int IpdNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";
            DataTable dt = new DataTable();
            strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            PatientRegNo = Convert.ToInt32(Session["Registration"].ToString());
            IpdNo = Convert.ToInt32(Session["IPDNo"].ToString());
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("RptAdmissionForm", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);//localhost:3636/Report/RptAdmissionForm.aspx.csital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", IpdNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);

            dt = Bind_hospDetail();

            if (dt.Rows.Count > 0)
            {
                h_name = dt.Rows[0][0].ToString();
                h_email = dt.Rows[0][1].ToString();
            }

            List<byte[]> files = new List<byte[]>();
            DataSet paymentRecipt1 = GetRptPatientIPDAdvanceAmount(strHospital, strLocation);



            if (ds.Tables[0].Rows.Count != 0)
            {


                string ServiceName = "";

                String str = "";

                Admission.Load(Path.Combine(Server.MapPath("~/Report"), "RptAdmissionFormNew.rpt"));
                Admission.SetDataSource(ds);

                Admission.SetParameterValue("Hosp_name", h_name);
                Admission.SetParameterValue("Hosp_email", h_email);

                Stream stream = Admission.ExportToStream(ExportFormatType.PortableDocFormat);

                files.Add(PrepareBytes(stream));

                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Crystal.pdf");
                //--------------------------------------Payment Report 
                //if (Convert.ToInt32(paymentRecipt1.Tables[1].Rows[0]["CrAmount"]) > 0)
                //{
                Admission1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDAdvanceAmount.rpt"));
                Admission1.SetDataSource(paymentRecipt1);

                Admission1.SetParameterValue("Hosp_name", h_name);
                Admission1.SetParameterValue("Hosp_email", h_email);

                Stream stream1 = Admission1.ExportToStream(ExportFormatType.PortableDocFormat);
                files.Add(PrepareBytes(stream1));
                // }

            
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            //merge the all reports & show the reports            
            Response.BinaryWrite(MergeReports(files).ToArray());

            Response.End();


        }
        
        public DataSet GetRptPatientIPDAdvanceAmount(int strHospital, int strLocation)
        {
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("RptPatientIPDAdvanceAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", IpdNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);

            return ds;
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
            if (this.Admission != null)
            {
                this.Admission.Close();
                this.Admission.Dispose();
            }
        }

        public DataTable Bind_hospDetail()
        {
            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd1 = new SqlCommand("HOSPNAME", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd1.Parameters.AddWithValue("@LocationID", strLocation);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            con.Open();
            da1.Fill(dt);
            con.Close();

            return dt;
        }

    }
}