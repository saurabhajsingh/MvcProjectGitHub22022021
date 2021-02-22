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
    public partial class RptPatientLabReportsWebForm : System.Web.UI.Page
    {

        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        DataSet dsMultipleTest = new DataSet();
        DataSet ds = new DataSet();
        List<byte[]> files = new List<byte[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";

            int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());
            string Labno = Session["LabNo"].ToString();
            string[] TestNo = Session["TestID"].ToString().Split(',');
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();


            string chknotgrp = "";
            string chkgrp = "";
            if (Session["chknotgrp"].ToString() != null)
            {
                chkgrp = Session["chkgrp"].ToString();
            }


            if (Session["chknotgrp"].ToString() != null)
            {
                chknotgrp = Session["chknotgrp"].ToString();
            }

            if (chkgrp == "true")
            {
                DataTable table = Session["table"] as DataTable;
                DataTable table1 = Session["table1"] as DataTable;
                DataTable table2 = Session["table2"] as DataTable;
                DataTable table3 = Session["table3"] as DataTable;
                DataTable table4 = Session["table4"] as DataTable;
                DataTable table5 = Session["table5"] as DataTable;


                if (table != null)
                {
                    DataTable dtCopy = table.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }
                if (table1 != null)
                {
                    DataTable dtCopy = table1.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }
                if (table2 != null)
                {
                    DataTable dtCopy = table2.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }
                if (table3 != null)
                {
                    DataTable dtCopy = table3.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }
                if (table4 != null)
                {
                    DataTable dtCopy = table4.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }
                if (table5 != null)
                {
                    DataTable dtCopy = table5.Copy();
                    dsMultipleTest.Tables.Add(dtCopy);

                }

                int i = 0;
                foreach (DataRow dr in dsMultipleTest.Tables[2].Rows)
                {
                    if (dr["Methord"].ToString().Trim() == "TR")
                    {
                        dr["Range"] = DBNull.Value;
                        dr["Unit"] = DBNull.Value;
                    }
                    if (dr["Methord"].ToString().Trim() == "TRN")
                    {
                        dr["Unit"] = DBNull.Value;
                    }
                    if (dr["Methord"].ToString().Trim() == "TRU")
                    {
                        dr["Range"] = DBNull.Value;
                    }
                }
                //for (i = dsMultipleTest.Tables[2].Rows.Count - 1; i >= 0; i--)
                //{
                //    DataRow dr = dsMultipleTest.Tables[2].Rows[i];
                //    if (dr["ResultValue"].ToString().Trim() == "")
                //        dr.Delete();
                //}
                dsMultipleTest.AcceptChanges();

                Session["table"] = null; Session["table1"] = null; Session["table2"] = null; Session["table3"] = null;
                Session["table4"] = null; Session["table5"] = null;
            }

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

            if (dt.Rows.Count > 0)
            {
                h_name = dt.Rows[0][0].ToString();
                h_email = dt.Rows[0][1].ToString();
            }

            if (dsMultipleTest.Tables.Count > 0)
            {
                if (dsMultipleTest.Tables[1].Rows.Count != 0)
                {
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientLabMultipleReport.rpt"));
                    rd.SetDataSource(dsMultipleTest);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));

                }

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";

                Response.BinaryWrite(MergeReports(files).ToArray());
            }

            if (chknotgrp == "true")
            {
                for (int a = 0; a < TestNo.Length; a++)
                {
                    if (TestNo[a] != "")
                    {

                        con = new SqlConnection(constring);
                        SqlCommand cmd = new SqlCommand("RptPatientLabReports", con);
                        cmd.CommandTimeout = 7000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@LabNo", Labno);
                        cmd.Parameters.AddWithValue("@TestID", TestNo[a]);
                       
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        //  con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        
                        try
                        {

                            if (ds.Tables[0].Rows.Count != 0)
                            {

                                if (ds.Tables[0].Rows[0]["LabReportFooter"].ToString().Trim() == null || ds.Tables[0].Rows[0]["LabReportFooter"].ToString().Trim() == "")
                                {
                                    ds.Tables[0].Rows[0]["LabReportFooter"] = 300;
                                }

                                foreach (DataRow dr in ds.Tables[2].Rows)
                                {
                                    if (dr["HeaderName"].ToString() == " No Header")
                                    {
                                        dr["HeaderName"] = "";
                                    }

                                    if (dr["NLH"].ToString() == "")
                                    {
                                        dr["NLH"] = "Normal";
                                    }
                                }
                                if (ds.Tables[2].Rows[0]["Methord"].ToString() == "TR")
                                {
                                    ds.Tables[2].Columns.Remove("Range");
                                    ds.Tables[2].Columns.Remove("Unit");
                                }
                                else if (ds.Tables[2].Rows[0]["Methord"].ToString() == "TRN")
                                {
                                    ds.Tables[2].Columns.Remove("Unit");
                                }
                                for (int i = ds.Tables[2].Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow dr = ds.Tables[2].Rows[i];
                                    if (dr["ResultValue"].ToString().Trim() == "")
                                        dr.Delete();
                                }
                                ds.AcceptChanges();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count != 0)
                            {

                                rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientLabReports.rpt"));
                                rd1.SetDataSource(ds);

                                rd1.SetParameterValue("Hosp_name", h_name);
                                rd1.SetParameterValue("Hosp_email", h_email);

                               // rd1.Refresh();

                                Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                                files.Add(PrepareBytes(stream1));

                            }

                        }
                    }
                    
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";

                    //merge the all reports & show the reports            
                    Response.BinaryWrite(MergeReports(files).ToArray());

                }
            }
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

                Session["abc"] = pdfFile;
                pdfFile.Close();
                pCopy.Close();
                doc.Close();

                return msOutput;
            }
            else if (files.Count == 1)
            {
                //   Session["abc"] = files[0];
                //    List<byte[]> files1 = new List<byte[]>();

                // files1 = Session["abc"];
                Session["abc"] = files[0];
                return new MemoryStream(files[0]);

                // return new MemoryStream(files[0]);

            }

            return null;
        }

    }
}