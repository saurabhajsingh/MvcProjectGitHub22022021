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
    public partial class RptPatientIPDProvisionalBills1 : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";
            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            int PatientBillNo = Convert.ToInt32(Session["ProvisionalReportBillNo"].ToString());
            int strRadioReport = Convert.ToInt32(Session["RadioReport"].ToString());

             if (strRadioReport == 1)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();
                
                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
            }
            else if (strRadioReport == 2)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();
                
                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Consolidated_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
            }
            else if (strRadioReport == 3)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();
                
                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Detailed_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
            }
            else if (strRadioReport == 4)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Detailed_Diagnosis_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
            }
            else if (strRadioReport == 5)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();
                
                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
            }
            else
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDProvisionalBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@BillNo", PatientBillNo);

                cmd.CommandTimeout = 500;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                int strHospital1 = Convert.ToInt32(Session["HospitalIDReport"].ToString());
                int strLocation1 = Convert.ToInt32(Session["LocationIDReport"].ToString());
                int strBillNo = 216;

                SqlConnection con1;
                string constring1 = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con1 = new SqlConnection(constring1);
                SqlCommand cmd1 = new SqlCommand("RptPatientIPDBills", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 500;
                cmd1.Parameters.AddWithValue("@HospitalID", strHospital1);
                cmd1.Parameters.AddWithValue("@LocationID", strLocation1);
                cmd1.Parameters.AddWithValue("@BillNo", strBillNo);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();
                
                if (ds.Tables[1].Rows.Count != 0)
                {
                    string ServiceName = "";
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Prov.rpt"));
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDProvisionalBills.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "RptPatientIPDProvisionalBills.pdf");
                    Response.End();

                }
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
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
        }

        public DataTable Bind_hospDetail()
        {
            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());

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