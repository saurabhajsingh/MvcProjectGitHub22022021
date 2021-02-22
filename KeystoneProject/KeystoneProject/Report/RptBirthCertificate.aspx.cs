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
    public partial class RptBirthCertificate : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();        
        protected void Page_Load(object sender, EventArgs e)
        {
            int HospitalID = Convert.ToInt32(Session["HospitalID"].ToString());
            int LocationID = Convert.ToInt32(Session["LocationID"].ToString());
            int CertificateNo = Convert.ToInt32(Session["CertificateNo"].ToString());


            List<byte[]> files = new List<byte[]>();

            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            DataSet dsDischargeSummary = new DataSet();

            SqlCommand cmd = new SqlCommand("RptPatientBirthCertificate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CertificateNo", CertificateNo);
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);
            
            if (ds.Tables[1].Rows.Count != 0)
            {
                if (ds.Tables[2].Rows.Count == 1)
                {
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientBirthCertificate.rpt"));
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Birth_Certificate.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("M_name", ds.Tables["table1"].Rows[0][2].ToString());
                    rd.SetParameterValue("M_age", ds.Tables["table1"].Rows[0][5].ToString());
                    rd.SetParameterValue("F_name", ds.Tables["table1"].Rows[0][3].ToString());
                    rd.SetParameterValue("H_name", ds.Tables["table"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_gender", ds.Tables["table2"].Rows[0][3].ToString());
                    rd.SetParameterValue("C_dob", ds.Tables["table2"].Rows[0][0].ToString());
                    rd.SetParameterValue("C_tob", ds.Tables["table2"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_weight", ds.Tables["table2"].Rows[0][2].ToString());

                    rd.SetParameterValue("C_gender2", "");
                    rd.SetParameterValue("C_dob2", "");
                    rd.SetParameterValue("C_tob2", "");
                    rd.SetParameterValue("C_weight2", "");

                    rd.SetParameterValue("C_gender3", "");
                    rd.SetParameterValue("C_dob3", "");
                    rd.SetParameterValue("C_tob3", "");
                    rd.SetParameterValue("C_weight3", "");

                    rd.SetParameterValue("C_gender4", "");
                    rd.SetParameterValue("C_dob4", "");
                    rd.SetParameterValue("C_tob4", "");
                    rd.SetParameterValue("C_weight4", "");

                    rd.SetParameterValue("print_3child", "");

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));
                }

                if (ds.Tables[2].Rows.Count == 2)
                {
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientBirthCertificate.rpt"));
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Birth_Certificate.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("M_name", ds.Tables["table1"].Rows[0][2].ToString());
                    rd.SetParameterValue("M_age", ds.Tables["table1"].Rows[0][5].ToString());
                    rd.SetParameterValue("F_name", ds.Tables["table1"].Rows[0][3].ToString());
                    rd.SetParameterValue("H_name", ds.Tables["table"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_gender", ds.Tables["table2"].Rows[0][3].ToString());
                    rd.SetParameterValue("C_dob", ds.Tables["table2"].Rows[0][0].ToString());
                    rd.SetParameterValue("C_tob", ds.Tables["table2"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_weight", ds.Tables["table2"].Rows[0][2].ToString());

                    rd.SetParameterValue("C_gender2", ds.Tables["table2"].Rows[1][3].ToString());
                    rd.SetParameterValue("C_dob2", ds.Tables["table2"].Rows[1][0].ToString());
                    rd.SetParameterValue("C_tob2", ds.Tables["table2"].Rows[1][1].ToString());
                    rd.SetParameterValue("C_weight2", ds.Tables["table2"].Rows[1][2].ToString());

                    rd.SetParameterValue("C_gender3", "");
                    rd.SetParameterValue("C_dob3", "");
                    rd.SetParameterValue("C_tob3", "");
                    rd.SetParameterValue("C_weight3", "");

                    rd.SetParameterValue("C_gender4", "");
                    rd.SetParameterValue("C_dob4", "");
                    rd.SetParameterValue("C_tob4", "");
                    rd.SetParameterValue("C_weight4", "");

                    rd.SetParameterValue("print_3child", "");

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));
                }

                if (ds.Tables[2].Rows.Count == 3)
                {

                    string first = ds.Tables["table2"].Rows[0][3].ToString();
                    string second = ds.Tables["table2"].Rows[1][3].ToString();
                    string third = ds.Tables["table2"].Rows[2][3].ToString();

                    string print_3child = "";

                    if (first == second && second == third)
                    {
                        print_3child = "3same";
                    }
                    else if (first == second && second != third)
                    {
                        print_3child = "first_2same";
                    }
                    else
                    {
                        print_3child = "last_2same";
                    }

                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientBirthCertificate.rpt"));
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Birth_Certificate.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("M_name", ds.Tables["table1"].Rows[0][2].ToString());
                    rd.SetParameterValue("M_age", ds.Tables["table1"].Rows[0][5].ToString());
                    rd.SetParameterValue("F_name", ds.Tables["table1"].Rows[0][3].ToString());
                    rd.SetParameterValue("H_name", ds.Tables["table"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_gender", ds.Tables["table2"].Rows[0][3].ToString());
                    rd.SetParameterValue("C_dob", ds.Tables["table2"].Rows[0][0].ToString());
                    rd.SetParameterValue("C_tob", ds.Tables["table2"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_weight", ds.Tables["table2"].Rows[0][2].ToString());

                    rd.SetParameterValue("C_gender2", ds.Tables["table2"].Rows[1][3].ToString());
                    rd.SetParameterValue("C_dob2", ds.Tables["table2"].Rows[1][0].ToString());
                    rd.SetParameterValue("C_tob2", ds.Tables["table2"].Rows[1][1].ToString());
                    rd.SetParameterValue("C_weight2", ds.Tables["table2"].Rows[1][2].ToString());

                    rd.SetParameterValue("C_gender3", ds.Tables["table2"].Rows[2][3].ToString());
                    rd.SetParameterValue("C_dob3", ds.Tables["table2"].Rows[2][0].ToString());
                    rd.SetParameterValue("C_tob3", ds.Tables["table2"].Rows[2][1].ToString());
                    rd.SetParameterValue("C_weight3", ds.Tables["table2"].Rows[2][2].ToString());

                    rd.SetParameterValue("C_gender4", "");
                    rd.SetParameterValue("C_dob4", "");
                    rd.SetParameterValue("C_tob4", "");
                    rd.SetParameterValue("C_weight4", "");

                    rd.SetParameterValue("print_3child", print_3child);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));
                }

                if (ds.Tables[2].Rows.Count == 4)
                {
                    //rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientBirthCertificate.rpt"));
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "Birth_Certificate.rpt"));
                    rd.SetDataSource(ds);

                    rd.SetParameterValue("M_name", ds.Tables["table1"].Rows[0][2].ToString());
                    rd.SetParameterValue("M_age", ds.Tables["table1"].Rows[0][5].ToString());
                    rd.SetParameterValue("F_name", ds.Tables["table1"].Rows[0][3].ToString());
                    rd.SetParameterValue("H_name", ds.Tables["table"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_gender", ds.Tables["table2"].Rows[0][3].ToString());
                    rd.SetParameterValue("C_dob", ds.Tables["table2"].Rows[0][0].ToString());
                    rd.SetParameterValue("C_tob", ds.Tables["table2"].Rows[0][1].ToString());
                    rd.SetParameterValue("C_weight", ds.Tables["table2"].Rows[0][2].ToString());

                    rd.SetParameterValue("C_gender2", ds.Tables["table2"].Rows[1][3].ToString());
                    rd.SetParameterValue("C_dob2", ds.Tables["table2"].Rows[1][0].ToString());
                    rd.SetParameterValue("C_tob2", ds.Tables["table2"].Rows[1][1].ToString());
                    rd.SetParameterValue("C_weight2", ds.Tables["table2"].Rows[1][2].ToString());

                    rd.SetParameterValue("C_gender3", ds.Tables["table2"].Rows[2][3].ToString());
                    rd.SetParameterValue("C_dob3", ds.Tables["table2"].Rows[2][0].ToString());
                    rd.SetParameterValue("C_tob3", ds.Tables["table2"].Rows[2][1].ToString());
                    rd.SetParameterValue("C_weight3", ds.Tables["table2"].Rows[2][2].ToString());

                    rd.SetParameterValue("C_gender4", ds.Tables["table2"].Rows[3][3].ToString());
                    rd.SetParameterValue("C_dob4", ds.Tables["table2"].Rows[3][0].ToString());
                    rd.SetParameterValue("C_tob4", ds.Tables["table2"].Rows[3][1].ToString());
                    rd.SetParameterValue("C_weight4", ds.Tables["table2"].Rows[3][2].ToString());

                    rd.SetParameterValue("print_3child", "");

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));
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