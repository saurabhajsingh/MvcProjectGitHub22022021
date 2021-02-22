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
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;
//using Microsoft.Reporting.WebForms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KeystoneProject.Report
{
    public partial class OtherRptPreBalanceAmountWebForm : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();

        private string _sDbConnection = "";
        DataSet ds1 = new DataSet();
        DataSet dsBedChargeMerge = new DataSet();
        string ServiceNameOld;
        DataSet dsBedChargeold = new DataSet();
        string FromDate;
        string ToDate;
        decimal Rate = 0;
        string ser2;
        string ser3;



        decimal Amount = 0;
        int Qty = 0;
        string BillDate;
        string ServiceGroupName;
        string ChargesType;
        string DoctorPrintName;
        string A;
        int i = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<byte[]> files = new List<byte[]>();
            int strHospital = Convert.ToInt32(Session["HospitalID"].ToString());
            int strLocation = Convert.ToInt32(Session["LocationID"].ToString());

            string[] strPatientAccountRowID = Session["PrintPaymentTypeCount"].ToString().Split(',');
            string BillType = Session["BillType"].ToString();
            string RegNo = Session["RegNo"].ToString();
            string OPDIPDNO = Session["OPDIPDNO"].ToString();
            string BillNo = Session["BillNo"].ToString();


            if (BillType == "SaveView")
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                for (int row = 0; row < strPatientAccountRowID.Length; row++)
                {
                    if (strPatientAccountRowID[row].ToString() != "")
                    {
                        SqlCommand cmd = new SqlCommand("RptOtherPreBalanceAmount", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@OtherAccountRowID", strPatientAccountRowID[row]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        string h_name = "";
                        string h_email = "";

                        DataTable dt = new DataTable();
                        dt = Bind_hospDetail();

                        if (dt.Rows.Count > 0)
                        {
                            h_name = dt.Rows[0][0].ToString();
                            h_email = dt.Rows[0][1].ToString();
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptOtherPreBalanceAmount.rpt"));
                            rd.SetDataSource(ds);

                            rd.SetParameterValue("Hosp_name", h_name);
                            rd.SetParameterValue("Hosp_email", h_email);

                            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                            files.Add(PrepareBytes(stream));

                        }
                    }
                }
            }
            else if(BillType == "IPDOtherBill" || BillType == "OPDOtherBill")
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                for (int row = 0; row < strPatientAccountRowID.Length; row++)
                {
                    if (strPatientAccountRowID[row].ToString() != "")
                    {
                        SqlCommand cmd = new SqlCommand("RptPatientOtherBills", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@BillNo", BillNo);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        string h_name = "";
                        string h_email = "";

                        DataTable dt = new DataTable();
                        dt = Bind_hospDetail();

                        if (dt.Rows.Count > 0)
                        {
                            h_name = dt.Rows[0][0].ToString();
                            h_email = dt.Rows[0][1].ToString();
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientOtherBills.rpt"));
                            rd.SetDataSource(ds);

                            rd.SetParameterValue("Hosp_name", h_name);
                            rd.SetParameterValue("Hosp_email", h_email);

                            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                            files.Add(PrepareBytes(stream));

                        }
                    }
                }
            }            
            else if(BillType == "OtherPreBalanceBills")
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                for (int row = 0; row < strPatientAccountRowID.Length; row++)
                {
                    if (strPatientAccountRowID[row].ToString() != "")
                    {
                        SqlCommand cmd = new SqlCommand("RptOtherPreBalanceAmount", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@OtherAccountRowID", strPatientAccountRowID[row]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        string h_name = "";
                        string h_email = "";

                        DataTable dt = new DataTable();
                        dt = Bind_hospDetail();

                        if (dt.Rows.Count > 0)
                        {
                            h_name = dt.Rows[0][0].ToString();
                            h_email = dt.Rows[0][1].ToString();
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptOtherPreBalanceAmount.rpt"));
                            rd.SetDataSource(ds);

                            rd.SetParameterValue("Hosp_name", h_name);
                            rd.SetParameterValue("Hosp_email", h_email);

                            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                            files.Add(PrepareBytes(stream));

                        }
                    }
                }
            }
            else if(BillType == "OtherRefundBills")
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                for (int row = 0; row < strPatientAccountRowID.Length; row++)
                {
                    if (strPatientAccountRowID[row].ToString() != "")
                    {
                        SqlCommand cmd = new SqlCommand("RptOtherRefundAmount", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@RowID", strPatientAccountRowID[row]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        string h_name = "";
                        string h_email = "";

                        DataTable dt = new DataTable();
                        dt = Bind_hospDetail();

                        if (dt.Rows.Count > 0)
                        {
                            h_name = dt.Rows[0][0].ToString();
                            h_email = dt.Rows[0][1].ToString();
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptOtherRefundAmount.rpt"));
                            rd.SetDataSource(ds);

                            rd.SetParameterValue("Hosp_name", h_name);
                            rd.SetParameterValue("Hosp_email", h_email);

                            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                            files.Add(PrepareBytes(stream));

                        }
                    }
                }
            }
            else if (BillType == "IPDOtherFinalBill")
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                for (int row = 0; row < strPatientAccountRowID.Length; row++)
                {
                    if (strPatientAccountRowID[row].ToString() != "")
                    {
                        SqlCommand cmd = new SqlCommand("RptPatientIPDOtherFinalBillsSummaryNew", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", strHospital);
                        cmd.Parameters.AddWithValue("@LocationID", strLocation);
                        cmd.Parameters.AddWithValue("@PatientRegNo", strPatientAccountRowID[row]);
                        cmd.Parameters.AddWithValue("@PatientIPDNo", strPatientAccountRowID[row]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        string h_name = "";
                        string h_email = "";

                        DataTable dt = new DataTable();
                        dt = Bind_hospDetail();

                        if (dt.Rows.Count > 0)
                        {
                            h_name = dt.Rows[0][0].ToString();
                            h_email = dt.Rows[0][1].ToString();
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDOtherFinalBillsSummaryNew.rpt"));
                            rd.SetDataSource(ds);

                            rd.SetParameterValue("Hosp_name", h_name);
                            rd.SetParameterValue("Hosp_email", h_email);

                            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                            files.Add(PrepareBytes(stream));

                        }
                    }
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
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

        public DataSet FinalBillPrint()
        {
            int HospitalID = Convert.ToInt32(Session["hpid"]);
            int LocationID = Convert.ToInt32(Session["hlid"]);

            int PatientRegNO = Convert.ToInt32(Session["RegNo1"]);
            int PatientIPDNo = Convert.ToInt32(Session["OPDIPDNO1"]);
            DataSet dsMaster = new DataSet();
            DataSet ds = new DataSet();
            //test.Controllers.Hospital.SMSSettingController objMasterSeting = new Controllers.Hospital.SMSSettingController();

            //dsMaster = objMasterSeting.GetMasterSetting(HospitalID, LocationID);
            //if (Convert.ToBoolean(dsMaster.Tables[0].Rows[0]["FinalBillWithHeader"].ToString()) == true)
            //{
            GetRptPatientIPDFinalBillsSummaryNew(HospitalID, LocationID, PatientRegNO, PatientIPDNo, "SaveView");
            GetRptPatientIPDFinalBillsSummary(HospitalID, LocationID, PatientRegNO, PatientIPDNo, "SaveView");

            //}
            //else
            //{
            //    GetRptPatientIPDFinalBillsInDetails(HospitalID, LocationID, PatientRegNO, PatientIPDNo, "SaveView");
            //    GetRptPatientIPDFinalBillsSummary(HospitalID, LocationID, PatientRegNO, PatientIPDNo, "SaveView");

            //}

            return ds;
        }

        #region Get Data For Report in Patient IPD Final Bills In Details
        public bool GetRptPatientIPDFinalBillsInDetails(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo, string ReportTypeName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[2].Value = PatientRegNo;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsSummaryNew1", param);
                dsBedChargeold = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsSummaryForBedCharge", param);

                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    ds.Tables[1].ImportRow(dr);
                }

                if (ds.Tables[0].Rows.Count != 0)
                {
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

                    string ServiceName = "";
                    int i = 1;
                    //if (ds.Tables[0].Rows[0]["OrganizationName"].ToString() != "")
                    //{

                    //}

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();

                        //if (ServiceName.Contains("VISIT CHARGES")).
                        //{
                        //    dr["ServiceName"] += " By " + dr["DoctorPrintName"];
                        //}

                        if (Convert.ToInt32(dr["A"]) == 300)
                        {
                            dr["Sr"] = DBNull.Value;
                            dr["ServiceGroupName"] = "PACKAGE";
                            dr["ServiceName"] = DBNull.Value;
                            dr["BillDate"] = DBNull.Value;
                            dr["Rate"] = DBNull.Value;
                            dr["Quantity"] = DBNull.Value;
                            dr["Amount"] = DBNull.Value;
                        }
                        if (Convert.ToInt32(dr["A"]) == 200)
                        {
                            dr["Sr"] = i.ToString();
                            dr["ServiceGroupName"] = "PACKAGE";
                            i++;
                        }
                    }

                    GetEmptyDataPatientIPDBillsDetailsReport();

                    Rate = 0;
                    Qty = 0;
                    Amount = 0;

                    if (dsBedChargeold.Tables[6].Rows.Count > 0)
                    {
                        if (dsBedChargeold.Tables[0].Rows.Count > 0)
                        {
                            int n = 0;
                            Boolean flag = false;
                            int rowindexTo = 0;
                            int rowindexFrom = 0;

                            string service = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                            for (int j = 1; j <= dsBedChargeold.Tables[6].Rows.Count; j++)
                            {
                                if (j < dsBedChargeold.Tables[6].Rows.Count)
                                {
                                    ser2 = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    ser3 = dsBedChargeold.Tables[6].Rows[j]["ServiceName1"].ToString();
                                }
                                else if (j == dsBedChargeold.Tables[6].Rows.Count)
                                {
                                    ser2 = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    ser3 = dsBedChargeold.Tables[6].Rows[j - 1]["ServiceName1"].ToString();
                                }
                                if ((ser2 == ser3) || (ser2 != ser3))
                                {
                                    ServiceNameOld = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    FromDate = dsBedChargeold.Tables[6].Rows[rowindexFrom]["ServiceName5"].ToString();
                                    BillDate = dsBedChargeold.Tables[6].Rows[n]["BillDate"].ToString();
                                    ServiceGroupName = dsBedChargeold.Tables[6].Rows[n]["ServiceGroupName"].ToString();
                                    ChargesType = dsBedChargeold.Tables[6].Rows[n]["ChargesType"].ToString();
                                    DoctorPrintName = dsBedChargeold.Tables[6].Rows[n]["DoctorPrintName"].ToString();
                                    Rate = Convert.ToDecimal(dsBedChargeold.Tables[6].Rows[n]["Rate"].ToString());
                                    A = dsBedChargeold.Tables[6].Rows[n]["A"].ToString();

                                    Amount = Amount + Convert.ToDecimal(dsBedChargeold.Tables[6].Rows[n]["Amount"].ToString());
                                    Qty = Qty + Convert.ToInt32(dsBedChargeold.Tables[6].Rows[n]["Quantity"].ToString());


                                    if ((ser2 != ser3) || (j == dsBedChargeold.Tables[6].Rows.Count))
                                    {
                                        flag = true;
                                        rowindexTo = n;
                                        ToDate = dsBedChargeold.Tables[6].Rows[rowindexTo]["ServiceName3"].ToString();
                                    }

                                    if (flag == true)
                                    {
                                        DataRow dr2 = dsBedChargeMerge.Tables[0].NewRow();
                                        dr2["Sr"] = i.ToString();
                                        dr2["ServiceGroupName"] = ServiceGroupName;
                                        dr2["ServiceName"] = ServiceNameOld + " --" + FromDate + " To  " + ToDate;
                                        dr2["BillDate"] = BillDate;
                                        dr2["DoctorPrintName"] = DoctorPrintName;
                                        dr2["ServiceGroupName"] = ServiceGroupName;
                                        dr2["ChargesType"] = ChargesType;
                                        dr2["Rate"] = Rate;
                                        dr2["Quantity"] = Qty;
                                        dr2["Amount"] = Amount;

                                        dr2["A"] = A;

                                        dsBedChargeMerge.Tables[0].AcceptChanges();
                                        dsBedChargeMerge.Tables[0].Rows.Add(dr2);
                                        Amount = 0;
                                        Qty = 0;
                                        rowindexFrom = j;
                                        flag = false;
                                    }
                                    n++;
                                }
                            }
                        }
                    }

                    ds.Tables[1].Merge(dsBedChargeMerge.Tables[0], true, MissingSchemaAction.Ignore);
                    DataView dv = new DataView(ds.Tables[1], "", "A asc", DataViewRowState.CurrentRows);
                    DataTable dtNew = dv.ToTable();
                    ds.Tables.Remove(ds.Tables[1]);
                    ds.Tables.Add(dtNew);

                    // RptPatientIPDFinalBillsInDetails rptPatientIPDFinalBillsInDetails = new RptPatientIPDFinalBillsInDetails();

                    String str = "";
                    try
                    {
                        str = "1";

                        str = "2";
                        rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsInDetails.rpt"));
                        str = "3";
                        rd.SetDataSource(ds);
                        str = "4";
                        CrystalReportViewer1.ReportSource = rd;
                        str = "5";
                        CrystalReportViewer1.DataBind();
                        str = "6";
                        CrystalReportViewer1.RefreshReport();
                        str = "7";




                    }
                    catch (Exception ex)
                    {
                        Response.Write(str);
                        //throw;
                    }



                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;
        }
        #endregion
        #region Get Data For Report in Patient IPD Final Bills New
        public bool GetRptPatientIPDFinalBillsSummaryNew(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo, string ReportTypeName)
        {
            DataSet ds = new DataSet();
            DataSet dsBedChargeold = new DataSet();
            try
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[2].Value = PatientRegNo;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsSummaryNew", param);
                dsBedChargeold = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsSummaryForBedCharge", param);
                ////foreach (DataRow dr in ds.Tables[4].Rows)
                ////{
                ////    ds.Tables[1].ImportRow(dr);       


                ////}

                if (ds.Tables[0].Rows.Count != 0)
                {
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

                    string ServiceName = "";
                    int i = 1;

                    //if (ds.Tables[0].Rows[0]["OrganizationName"].ToString() != "")
                    //{
                    //DialogResult ObjDialogResult = MessageBox.Show("With Paid Amount ", "Keystone ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //if (ObjDialogResult == DialogResult.No)
                    //{
                    //    ds.Tables[2].Rows[0]["CrAmount"] = 0;
                    //    ds.Tables[2].Rows[0]["DrAmount"] = 0;
                    //}
                    //}


                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();

                        //if (ServiceName.Contains("VISIT CHARGES"))
                        //{
                        //    dr["ServiceName"] += " By " + dr["DoctorPrintName"];
                        //}

                        if (Convert.ToInt32(dr["A"]) == 300)
                        {
                            dr["Sr"] = DBNull.Value;
                            dr["ServiceGroupName"] = "PACKAGE";
                            //dr["BillDate"] = DBNull.Value;
                            dr["Rate"] = DBNull.Value;
                            dr["Quantity"] = DBNull.Value;
                            dr["Amount"] = DBNull.Value;
                        }
                        if (Convert.ToInt32(dr["A"]) == 200)
                        {
                            dr["Sr"] = i.ToString();
                            dr["ServiceGroupName"] = "PACKAGE";
                            i++;
                        }
                    }
                    //  group by bed charges --- sulochana

                    GetEmptyDataPatientIPDBillsDetailsReport();

                    Rate = 0;
                    Qty = 0;
                    Amount = 0;

                    if (dsBedChargeold.Tables[6].Rows.Count > 0)
                    {
                        if (dsBedChargeold.Tables[0].Rows.Count > 0)
                        {
                            int n = 0;
                            Boolean flag = false;
                            int rowindexTo = 0;
                            int rowindexFrom = 0;

                            string service = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                            for (int j = 1; j <= dsBedChargeold.Tables[6].Rows.Count; j++)
                            {
                                if (j < dsBedChargeold.Tables[6].Rows.Count)
                                {
                                    ser2 = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    ser3 = dsBedChargeold.Tables[6].Rows[j]["ServiceName1"].ToString();
                                }
                                else if (j == dsBedChargeold.Tables[6].Rows.Count)
                                {
                                    ser2 = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    ser3 = dsBedChargeold.Tables[6].Rows[j - 1]["ServiceName1"].ToString();
                                }
                                if ((ser2 == ser3) || (ser2 != ser3))
                                {
                                    ServiceNameOld = dsBedChargeold.Tables[6].Rows[n]["ServiceName1"].ToString();
                                    FromDate = dsBedChargeold.Tables[6].Rows[rowindexFrom]["ServiceName5"].ToString();
                                    BillDate = dsBedChargeold.Tables[6].Rows[n]["BillDate"].ToString();
                                    ServiceGroupName = dsBedChargeold.Tables[6].Rows[n]["ServiceGroupName"].ToString();
                                    ChargesType = dsBedChargeold.Tables[6].Rows[n]["ChargesType"].ToString();
                                    DoctorPrintName = dsBedChargeold.Tables[6].Rows[n]["DoctorPrintName"].ToString();
                                    Rate = Convert.ToDecimal(dsBedChargeold.Tables[6].Rows[n]["Rate"].ToString());
                                    A = dsBedChargeold.Tables[6].Rows[n]["A"].ToString();

                                    Amount = Amount + Convert.ToDecimal(dsBedChargeold.Tables[6].Rows[n]["Amount"].ToString());
                                    Qty = Qty + Convert.ToInt32(dsBedChargeold.Tables[6].Rows[n]["Quantity"].ToString());


                                    if ((ser2 != ser3) || (j == dsBedChargeold.Tables[6].Rows.Count))
                                    {
                                        flag = true;
                                        rowindexTo = n;
                                        ToDate = dsBedChargeold.Tables[6].Rows[rowindexTo]["ServiceName3"].ToString();
                                    }

                                    if (flag == true)
                                    {
                                        DataRow dr2 = dsBedChargeMerge.Tables[0].NewRow();
                                        dr2["Sr"] = i.ToString();
                                        dr2["ServiceGroupName"] = ServiceGroupName;
                                        dr2["ServiceName"] = ServiceNameOld + " --" + FromDate + " To  " + ToDate;
                                        dr2["BillDate"] = BillDate;
                                        dr2["DoctorPrintName"] = DoctorPrintName;
                                        dr2["ServiceGroupName"] = ServiceGroupName;
                                        dr2["ChargesType"] = ChargesType;
                                        dr2["Rate"] = Rate;
                                        dr2["Quantity"] = Qty;
                                        dr2["Amount"] = Amount;

                                        dr2["A"] = A;

                                        dsBedChargeMerge.Tables[0].AcceptChanges();
                                        dsBedChargeMerge.Tables[0].Rows.Add(dr2);
                                        Amount = 0;
                                        Qty = 0;
                                        rowindexFrom = j;
                                        flag = false;
                                    }
                                    n++;
                                }
                            }
                        }
                    }

                    ds.Tables[1].Merge(dsBedChargeMerge.Tables[0], true, MissingSchemaAction.Ignore);
                    DataView dv = new DataView(ds.Tables[1], "", "A asc", DataViewRowState.CurrentRows);
                    DataTable dtNew = dv.ToTable();
                    ds.Tables.Remove(ds.Tables[1]);
                    ds.Tables.Add(dtNew);

                    RptPatientIPDFinalBillsSummaryNew1 rptPatientIPDFinalBillsSummaryNew = new RptPatientIPDFinalBillsSummaryNew1();


                    String str = "";
                    try
                    {
                        str = "1";

                        str = "2";
                        rd.Load(Path.Combine(Server.MapPath("~/Report"), "RptPatientIPDFinalBillsSummaryNew1.rpt"));
                        str = "3";
                        rd.SetDataSource(ds);
                        str = "4";
                        CrystalReportViewer1.ReportSource = rd;
                        str = "5";
                        CrystalReportViewer1.DataBind();
                        str = "6";
                        CrystalReportViewer1.RefreshReport();
                        str = "7";




                    }
                    catch (Exception ex)
                    {
                        Response.Write(str);
                        //throw;
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;
        }

        public void GetEmptyDataPatientIPDBillsDetailsReport()
        {
            dsBedChargeMerge.Reset();
            dsBedChargeMerge.Tables.Add(new DataTable());
            dsBedChargeMerge.Tables.Add(new DataTable("PatientBillsDetails"));

            dsBedChargeMerge.Tables[0].Columns.Add("Sr");
            dsBedChargeMerge.Tables[0].Columns.Add("ServiceGroupName");
            dsBedChargeMerge.Tables[0].Columns.Add("ServiceName");
            dsBedChargeMerge.Tables[0].Columns.Add("BillDate");
            dsBedChargeMerge.Tables[0].Columns.Add("ChargesType");

            dsBedChargeMerge.Tables[0].Columns.Add("DoctorPrintName");
            dsBedChargeMerge.Tables[0].Columns.Add("Quantity");

            dsBedChargeMerge.Tables[0].Columns.Add("Rate");
            dsBedChargeMerge.Tables[0].Columns.Add("Amount");
            dsBedChargeMerge.Tables[0].Columns.Add("A");
        }

        #endregion
        #region Get Data For Report in Patient IPD Final Bills & Summary
        public bool GetRptPatientIPDFinalBillsSummary(int HospitalID, int LocationID, int PatientRegNo, int PatientIPDNo, string ReportTypeName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con;
                string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
                con = new SqlConnection(constring);
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[2].Value = PatientRegNo;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsSummary", param);

                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    ds.Tables[1].ImportRow(dr);
                }

                if (ds.Tables[0].Rows.Count != 0)
                {
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

                    string ServiceName = "";
                    int i = 1;

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ServiceName = dr["ServiceName"].ToString();

                        //if (ServiceName.Contains("VISIT CHARGES"))
                        //{
                        //    dr["ServiceName"] += " By " + dr["DoctorPrintName"];
                        //}

                        if (Convert.ToInt32(dr["A"]) == 300)
                        {
                            dr["Sr"] = DBNull.Value;
                            dr["ServiceGroupName"] = "PACKAGE";
                            dr["BillDate"] = DBNull.Value;
                            dr["Rate"] = DBNull.Value;
                            dr["Quantity"] = DBNull.Value;
                            dr["Amount"] = DBNull.Value;
                        }
                        if (Convert.ToInt32(dr["A"]) == 200)
                        {
                            dr["Sr"] = i.ToString();
                            dr["ServiceGroupName"] = "PACKAGE";
                            i++;
                        }
                    }


                    //RptPatientIPDFinalBillsSummary rptPatientIPDFinalBills = new RptPatientIPDFinalBillsSummary();
                    //ReportDocument rpt = new ReportDocument();
                    String str = "";
                    try
                    {
                        str = "1";

                        str = "2";
                        rd.Load(Path.Combine(Server.MapPath("~/Report"), "rptPatientIPDFinalBills.rpt"));
                        str = "3";
                        rd.SetDataSource(ds);
                        str = "4";
                        CrystalReportViewer1.ReportSource = rd;
                        str = "5";
                        CrystalReportViewer1.DataBind();
                        str = "6";
                        CrystalReportViewer1.RefreshReport();
                        str = "7";

                    }
                    catch (Exception ex)
                    {
                        Response.Write(str);
                        //throw;
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;
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
            //int strHospital = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            //int strLocation = Convert.ToInt32(Session["LocationIDReport"].ToString());

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