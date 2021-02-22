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

namespace KeystoneProject.Report
{
    public partial class CrystalReport1WebForm : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";
            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            int strBillNo = Convert.ToInt32(Session["BillIDReport"].ToString());
            // int strRadioReport = Convert.ToInt32(Session["RadioReport"].ToString());

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("RptPatientIPDBills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@BillNo", strBillNo);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //con.Open();
            da.Fill(ds);
            con.Close();
            
            SqlCommand cmd1 = new SqlCommand("HOSPNAME", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd1.Parameters.AddWithValue("@LocationID", strLocation);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            con.Open();
            da1.Fill(dt);
            con.Close();
           
            if (dt.Rows.Count >0 )
            {
                h_name = dt.Rows[0][0].ToString();
                h_email = dt.Rows[0][1].ToString();
            }
          
            List<byte[]> files = new List<byte[]>();
            DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

            if (ds.Tables[0].Rows.Count != 0)
            {
                string ServiceName = "";

                String str = "";

                rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_IPD.rpt"));
                rd.SetDataSource(ds);

                rd.SetParameterValue("Hosp_name", h_name);
                rd.SetParameterValue("Hosp_email", h_email);

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);

                files.Add(PrepareBytes(stream));

                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Crystal.pdf");
                //--------------------------------------Payment Report 
                if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                {
                    rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                    rd1.SetDataSource(paymentRecipt);

                    rd1.SetParameterValue("Hosp_name", h_name);
                    rd1.SetParameterValue("Hosp_email", h_email);

                    Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream1));
                }

            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            //merge the all reports & show the reports            
            Response.BinaryWrite(MergeReports(files).ToArray());

            Response.End();

        }       

        public DataSet RptPaymentRecipt(DataSet ds1, int strHospital, int strLocation)
        {

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("RptBalanceAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@BillNo", ds1.Tables[0].Rows[0]["BillNo1"].ToString());
            cmd.Parameters.AddWithValue("@BillType", ds1.Tables[0].Rows[0]["BillType"].ToString());
            cmd.Parameters.AddWithValue("@FinancialYearID", ds1.Tables[0].Rows[0]["FinancialYearID"].ToString());
            //cmd.Parameters.AddWithValue("@BillNo", 864);
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
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
        }
    }
}