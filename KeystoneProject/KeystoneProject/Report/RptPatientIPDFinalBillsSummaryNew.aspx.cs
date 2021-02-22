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
    public partial class RptPatientIPDFinalBillsSummaryNew : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";
            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNoReport"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNoReport"].ToString());
            int strRadioReport = Convert.ToInt32(Session["RadioReport"].ToString());


            if (strRadioReport == 1)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
            else if (strRadioReport == 2)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //con.Open();
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Consolidated_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
            else if (strRadioReport == 3)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //con.Open();
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Detailed_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
            else if (strRadioReport == 4)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //con.Open();
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Detailed_Diagnosis_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
            else if (strRadioReport == 5)
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //con.Open();
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
            else
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("RptPatientIPDFinalBillsSummaryNew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //con.Open();
                da.Fill(ds);
                DataSet paymentRecipt = RptPaymentRecipt(ds, strHospital, strLocation);

                DataTable dt = new DataTable();
                dt = Bind_hospDetail();

                if (dt.Rows.Count > 0)
                {
                    h_name = dt.Rows[0][0].ToString();
                    h_email = dt.Rows[0][1].ToString();
                }

                List<byte[]> files = new List<byte[]>();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    string ServiceName = "";

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        if (dr["CrAmount"] == DBNull.Value)
                        {
                            dr["CrAmount"] = "0.00";
                        }

                        if (dr["DrAmount"] == DBNull.Value)
                        {
                            dr["DrAmount"] = "0.00";
                        }
                    }

                    String str = "";

                    str = "1";
                    str = "2";

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Summerized_Final.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                    if (Convert.ToInt32(paymentRecipt.Tables[1].Rows[0]["CrAmount"]) > 0)
                    {
                        rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPreBalanceAmount.rpt"));
                        rd1.SetDataSource(paymentRecipt);

                        rd1.SetParameterValue("Hosp_name", h_name);
                        rd1.SetParameterValue("Hosp_email", h_email);

                        Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                        files.Add(PrepareBytes(stream1));
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                    Response.End();
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.rd != null)
            {
                this.rd.Close();
                this.rd.Dispose();
            }
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
            cmd.Parameters.AddWithValue("@BillNo", ds1.Tables[3].Rows[0]["BillNo"].ToString());
            cmd.Parameters.AddWithValue("@BillType", "IPDFinalBill");
            cmd.Parameters.AddWithValue("@FinancialYearID", ds1.Tables[3].Rows[0]["FinancialYearID"].ToString());
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