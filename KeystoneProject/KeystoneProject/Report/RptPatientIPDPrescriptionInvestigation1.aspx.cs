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
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;
using System.Web.Mvc;


namespace KeystoneProject.Report
{
    public partial class RptPatientIPDPrescriptionInvestigation1 : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        ReportDocument rd1 = new ReportDocument();
        ReportDocument rd2 = new ReportDocument();
        ReportDocument rd3 = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            string h_name = "";
            string h_email = "";
            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNo"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNO"].ToString());
            int CheckupAndPrecriptionID = Convert.ToInt32(Session["CheckupAndPrecriptionID"].ToString());
            string chkFinalDischarge = Session["chkFinalDischarge"].ToString();


            List<byte[]> files = new List<byte[]>();
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            DataSet dsDischargeSummary = new DataSet();            
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = Bind_hospDetail();

            if (dt.Rows.Count > 0)
            {
                h_name = dt.Rows[0][0].ToString();
                h_email = dt.Rows[0][1].ToString();
            }

            if (chkFinalDischarge == "True")
            {
                string EndResult = Session["EndResult"].ToString();
                string chkDeathSummary = Session["chkDeathSummary"].ToString();
                int PatientIPDDischargeID = Convert.ToInt32(Session["PatientIPDDischargeID"].ToString());

                if (chkDeathSummary == "False")
                {

                    dsDischargeSummary = RptPatientIPDDischargeAndSummaryNew();
                    dt1 = DrugAdviceDetail();
                    dsDischargeSummary.Tables.Add("DrugAdvice").Merge(dt1);

                    dt2 = Drug_Examination();
                    dsDischargeSummary.Tables.Add("Examination").Merge(dt2);

                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDDischargeAndSummaryNew.rpt"));
                    rd.SetDataSource(dsDischargeSummary);

                    rd.SetParameterValue("Hosp_name", h_name);
                    rd.SetParameterValue("Hosp_email", h_email);

                    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream));
                }

                // Discharge Oparative Note

                dsDischargeSummary = RptPatientIPDDischargeOperativeNotes();


                rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDDischargeOperativeNotes.rpt"));
                rd1.SetDataSource(dsDischargeSummary);

                rd1.SetParameterValue("Hosp_name", h_name);
                rd1.SetParameterValue("Hosp_email", h_email);

                Stream stream1 = rd1.ExportToStream(ExportFormatType.PortableDocFormat);
                files.Add(PrepareBytes(stream1));

                if (chkDeathSummary == "True")
                {
                    DataSet dsDeath = new DataSet();
                    dsDeath = RptDeathSummary(strHospital, strLocation, PatientRegNo, PatientIPDNo, PatientIPDDischargeID, "");

                    rd2.Load(Path.Combine(Server.MapPath("~/Report"), "RptDeathSummary.rpt"));
                    rd2.SetDataSource(dsDeath);

                    rd2.SetParameterValue("Hosp_name", h_name);
                    rd2.SetParameterValue("Hosp_email", h_email);

                    Stream stream2 = rd2.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream2));
                }
                if (EndResult == "Transfer")
                {
                    DataSet dsRptTransferSummary = new DataSet();
                    dsRptTransferSummary = RptTransferSummary(strHospital, strLocation, PatientRegNo, PatientIPDDischargeID, "");

                    rd3.Load(Path.Combine(Server.MapPath("~/Report"), "RptTransferSummary.rpt"));
                    rd3.SetDataSource(dsRptTransferSummary);

                    rd3.SetParameterValue("Hosp_name", h_name);
                    rd3.SetParameterValue("Hosp_email", h_email);

                    Stream stream3 = rd3.ExportToStream(ExportFormatType.PortableDocFormat);
                    files.Add(PrepareBytes(stream3));
                }


            }
            else
            {

                SqlCommand cmd = new SqlCommand("RptPatientIPDPrescriptionInvestigation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                cmd.Parameters.AddWithValue("@LocationID", strLocation);
                cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmd.Parameters.AddWithValue("@OPDIPDID", PatientIPDNo);
                cmd.Parameters.AddWithValue("@CheckupAndPrecriptionID", CheckupAndPrecriptionID);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //   con.Open();
                da.Fill(ds);
                
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDPrescriptionInvestigation.rpt"));
                rd.SetDataSource(ds);

                rd.SetParameterValue("Hosp_name", h_name);
                rd.SetParameterValue("Hosp_email", h_email);

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                files.Add(PrepareBytes(stream));
                
                SqlCommand cmdPrescriptionOperativeNotes = new SqlCommand("RptPatientIPDPrescriptionOperativeNotes", con);
                cmdPrescriptionOperativeNotes.CommandType = CommandType.StoredProcedure;
                cmdPrescriptionOperativeNotes.Parameters.AddWithValue("@HospitalID", strHospital);
                cmdPrescriptionOperativeNotes.Parameters.AddWithValue("@LocationID", strLocation);
                cmdPrescriptionOperativeNotes.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
                cmdPrescriptionOperativeNotes.Parameters.AddWithValue("@CheckupAndPrecriptionID", CheckupAndPrecriptionID);

                SqlDataAdapter da1 = new SqlDataAdapter(cmdPrescriptionOperativeNotes);
                DataSet ds1 = new DataSet();               
                da1.Fill(ds1);

                rd1.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDPrescriptionOperativeNotes.rpt"));
                rd1.SetDataSource(ds1);

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

        public DataSet RptPatientIPDDischargeAndSummaryNew()
        {

            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNo"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNO"].ToString());
            int PatientIPDDischargeID = Convert.ToInt32(Session["PatientIPDDischargeID"].ToString());
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("RptPatientIPDDischargeAndSummaryNew", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);
            cmd.Parameters.AddWithValue("@PatientIPDDischargeID", PatientIPDDischargeID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //   con.Open();
            da.Fill(ds);
            return ds;

        }   

        public DataSet RptPatientIPDDischargeOperativeNotes()
        {

            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNo"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNO"].ToString());
            int PatientIPDDischargeID = Convert.ToInt32(Session["PatientIPDDischargeID"].ToString());
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("RptPatientIPDDischargeOperativeNotes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);

            cmd.Parameters.AddWithValue("@PatientIPDDischargeID", PatientIPDDischargeID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            //   con.Open();
            da.Fill(ds);
            return ds;

        }

        public DataSet RptDeathSummary(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo, int PatientIPDDischargeID, string ReportTypeName)
        {
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            DataSet ds = new DataSet();

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
            param[0].Value = HospitalID;
            param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
            param[1].Value = LocationID;
            param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
            param[2].Value = PatientRegNo;
            //param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
            //param[3].Value = PatientIPDNo;
            param[3] = new SqlParameter("@PatientIPDDischargeID", SqlDbType.Int);
            param[3].Value = PatientIPDDischargeID;



            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptDeathSummary", param);

            if (ds.Tables[0].Rows.Count != 0)
            {

                //DataView dvInvest = new DataView(ds.Tables[1], " attribute = 'Investigation'  ", "", DataViewRowState.CurrentRows);

                //if (dvInvest.Count > 0)
                //{
                //    dvInvest[0]["PrintAs"] = "Investigation";
                //}

                //DataView dvDrug = new DataView(ds.Tables[1], " attribute = 'DrugName'  ", "", DataViewRowState.CurrentRows);

                //if (dvDrug.Count > 0)
                //{
                //    dvDrug[0]["PrintAs"] = "DrugName";
                //}


            }
            return ds;
        }

        #region Get Data For Report in Patient Transfer Summary
        public DataSet RptTransferSummary(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDDischargeID, string ReportTypeName)
        {
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            DataSet ds = new DataSet();


            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[2].Value = PatientRegNo;
                //param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                //param[3].Value = PatientIPDNo;
                param[3] = new SqlParameter("@PatientIPDDischargeID", SqlDbType.Int);
                param[3].Value = PatientIPDDischargeID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptDeathSummary", param);

                //if (ds.Tables[0].Rows.Count != 0)
                //{
                string Data = "";
                for (int a = 0; a < ds.Tables[3].Columns.Count; a++)
                {
                    if (a > 4)
                    {
                        Data += ds.Tables[3].Rows[0][a].ToString();
                    }
                }
                int aa = Data.Length;
                if (aa > 1)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {




                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion

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

        public DataTable DrugAdviceDetail()
        {

            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNo"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNO"].ToString());
            int PatientIPDDischargeID = Convert.ToInt32(Session["PatientIPDDischargeID"].ToString());
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("DIS_SUM_DRUG_ADVICE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;

        }

        public DataTable Drug_Examination()
        {

            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());
            int PatientRegNo = Convert.ToInt32(Session["PatientRegNo"].ToString());
            int PatientIPDNo = Convert.ToInt32(Session["PatientIPDNO"].ToString());
            int PatientIPDDischargeID = Convert.ToInt32(Session["PatientIPDDischargeID"].ToString());
            SqlConnection con;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);

            SqlCommand cmd = new SqlCommand("DIS_SUM_EXAMINATION", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", strHospital);
            cmd.Parameters.AddWithValue("@LocationID", strLocation);
            cmd.Parameters.AddWithValue("@PatientRegNo", PatientRegNo);
            cmd.Parameters.AddWithValue("@PatientIPDNo", PatientIPDNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;

        }

    }
}