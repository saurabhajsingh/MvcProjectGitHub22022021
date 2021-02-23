using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_Patient_IPDBill
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
             HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
             LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
             UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        }

        public IPDBill AddIPDBill(IPDBill bill)
        {
            try
            {
                int PatientLabDetailID = 0;
                Connect();
                if (bill.BillType == "IPDProvisionalBill")
                {
                    SqlCommand cmd = new SqlCommand("IUProvisionalBills", con);
                    //SqlCommand cmd = new SqlCommand("GetStudentDetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    //  cmd.Parameters.Add(new SqlParameter("@PrintBillNo ", bill.PrintBillNo));

                    cmd.Parameters.AddWithValue("@ProvisionalBillNo", 0);
                    cmd.Parameters["@ProvisionalBillNo"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(bill.BillDate)));
                    //  cmd.Parameters.Add(new SqlParameter("@BillDate", DateTime.ParseExact(bill.BillDate, "dd/MM/yyyy hh:mm:ss", null)));

                    cmd.Parameters.Add(new SqlParameter("@PatientRegNO", bill.PatientRegNO));
                    cmd.Parameters.Add(new SqlParameter("@OPDIPDID", bill.OPDIPDID));
                    //cmd.Parameters.Add(new SqlParameter("@BillType ", bill.BillType));
                    cmd.Parameters.Add(new SqlParameter("@GrossAmount", Convert.ToDouble(bill.GrossAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@TaxPercent", Convert.ToDouble(bill.TaxPercent)));
                    cmd.Parameters.Add(new SqlParameter("@TaxAmount", Convert.ToDouble(bill.TaxAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@ReffCommission", Convert.ToDouble(bill.ReffCommission)));
                    //cmd.Parameters.Add(new SqlParameter("@Commisson", Convert.ToDouble(bill.Commisson)));
                    cmd.Parameters.Add(new SqlParameter("@TotalAmount", Convert.ToDouble(bill.TotalAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(bill.DiscountPercent)));

                    if (bill.DiscountAmount != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(bill.DiscountAmount)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(0.00)));
                    }
                    //cmd.Parameters.Add(new SqlParameter("@DiscountReason", bill.DiscountReason));
                    cmd.Parameters.Add(new SqlParameter("@NetPayableAmount", Convert.ToDouble(bill.NetPayableAmount)));
                    cmd.Parameters.Add(new SqlParameter("@BalanceAmount", Convert.ToDouble(bill.BalanceAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble(bill.PreBalanceAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@IsPaid", bill.IsPaid));
                    //cmd.Parameters.Add(new SqlParameter("@IsBillMade", bill.IsPaid));
                    cmd.Parameters.Add(new SqlParameter("@PaidAmount", Convert.ToDouble(bill.PaidAmount)));
                    cmd.Parameters.Add(new SqlParameter("@DipositeAmount", Convert.ToDouble(0.00)));
                    //cmd.Parameters.Add(new SqlParameter("@PaymentType ", bill.PaymentType));
                    //cmd.Parameters.Add(new SqlParameter("@Number", bill.Number));
                    //cmd.Parameters.Add(new SqlParameter("@Name", bill.Name));
                    // cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(bill.Date)));
                    //cmd.Parameters.Add(new SqlParameter("@Date ", DateTime.ParseExact(bill.Date, "dd/MM/yyyy hh:mm:ss", null)));
                    //cmd.Parameters.Add(new SqlParameter("@Remarks ", bill.Remarks));
                    //cmd.Parameters.Add(new SqlParameter("@SancationAmount", Convert.ToDouble(bill.SancationAmount)));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialYearID   ", bill.FinancialYearID));
                    cmd.Parameters.Add(new SqlParameter("@CreationID", bill.CreationID));

                    cmd.Parameters.Add(new SqlParameter("@Mode", "ADD"));

                    int i = 0;
                    con.Open();
                    i = cmd.ExecuteNonQuery();

                    int BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                    bill.BillNo = BillNo;


                    if (i > 0)
                    {

                        for (int x = 0; x < bill.Services.Length; x++)
                        {
                            SqlCommand svccmd;
                            SqlDataAdapter svcda;

                            if (bill.Services[x].ServiceType == "IPDLabBills")
                            {
                                svccmd = new SqlCommand("select Category.CategoryName from Category where RowStatus=0 and LocationID=" + LocationID + " and HospitalID=" + HospitalID + " and CategoryID in (select Category from TestMaster where RowStatus=0 and LocationID=" + LocationID+" and HospitalID="+ HospitalID + " and TestID  = " + bill.Services[x].SvcID + ")", con);

                                svcda = new SqlDataAdapter(svccmd);
                            }
                            else
                            {
                                 svccmd = new SqlCommand("select ServiceGroupName from ServiceGroup where RowStatus=0 and LocationID=" + LocationID + " and HospitalID=" + HospitalID + " and ServiceGroupID in (select ServiceGroupID from Services where RowStatus=0 and LocationID=" + LocationID + " and HospitalID=" + HospitalID + " and ServiceID = " + bill.Services[x].SvcID + ")", con);
                                svcda = new SqlDataAdapter(svccmd);

                            }
                            DataTable svcdt = new DataTable();
                            svcda.Fill(svcdt);
                            string svcgrpname = svcdt.Rows[0][0].ToString();
                            SqlCommand cmd1 = new SqlCommand("IUProvisionalBillsDetail", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@ProvisionalBillNo", BillNo);
                            cmd1.Parameters.AddWithValue("@ProvisionalDetailID", 0);
                            cmd1.Parameters.AddWithValue("@ServiceID", bill.Services[x].SvcID);
                            cmd1.Parameters.AddWithValue("@Servicename", bill.Services[x].SvcName);
                            cmd1.Parameters.AddWithValue("@ServiceGroupName", svcgrpname);

                            //cmd1.Parameters.AddWithValue("@UnitID", 1);
                            cmd1.Parameters.AddWithValue("@ChargesType", "General");

                            cmd1.Parameters.AddWithValue("@Rate", bill.Services[x].Rate);
                            cmd1.Parameters.AddWithValue("@Quantity", bill.Services[x].Quantity);
                            cmd1.Parameters.AddWithValue("@Discount", bill.Services[x].sevicedisAmt);
                            //cmd1.Parameters.AddWithValue("@Commisson", "0.00");
                            //cmd1.Parameters.AddWithValue("@ReffCommission", "0.00");
                            //cmd1.Parameters.AddWithValue("@ServiceType", "IPD");
                            cmd1.Parameters.AddWithValue("@TotalAmount", bill.Services[x].Total);
                            cmd1.Parameters.AddWithValue("@Discount_Service", bill.Services[x].Discount_Service);
                            cmd1.Parameters.AddWithValue("@DiscountServiceType", bill.Services[x].DiscountServiceType);
                            //cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");
                            //cmd1.Parameters.AddWithValue("@DoctorID", bill.Services[x].DoctorID);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Mode", "ADD");
                            int BillsDetails = cmd1.ExecuteNonQuery();
                        }



                        #region Bill Print IPD Bills
                        /*
                     *       */
                        /*
                        int HospitalID = 1;
                        int LocationID = 1;
                        int BillNO = BillNo;

                        DBHandle obj_RptPatientIPDBills = new DBHandle();
                        obj_RptPatientIPDBills.GetRptPatientIPDBills(HospitalID, LocationID, BillNO, "IPD", "SaveView");




                        decimal PaidAmt = Convert.ToDecimal(bill.PaidAmount);

                        if (PaidAmt > 0)
                        {
                            DBHandle objMoneyReceipt = new DBHandle();
                            objMoneyReceipt.RptMoneyReceipt(HospitalID, LocationID, BillNo, "SaveView");
                        }

                        return "1";
                        */

                        #endregion

                        return bill;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("IUPatientBills", con);
                    //SqlCommand cmd = new SqlCommand("GetStudentDetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    //  cmd.Parameters.Add(new SqlParameter("@PrintBillNo ", bill.PrintBillNo));

                    cmd.Parameters.AddWithValue("@BillNo", 0);
                    cmd.Parameters["@BillNo"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(bill.BillDate)));
                    // cmd.Parameters.Add(new SqlParameter("@BillDate", DateTime.ParseExact(bill.BillDate, "dd/MM/yyyy hh:mm:ss", null)));

                    cmd.Parameters.Add(new SqlParameter("@PatientRegNO", bill.PatientRegNO));
                    cmd.Parameters.Add(new SqlParameter("@OPDIPDID", bill.OPDIPDID));
                    cmd.Parameters.Add(new SqlParameter("@BillType ", bill.BillType));
                    if (bill.GrossAmount=="")
                    {
                        bill.GrossAmount = "0";
                    }
                    if(bill.TaxPercent=="")
                    {
                        bill.TaxPercent = "0";
                    }
                    if (bill.TaxAmount == "")
                    {
                        bill.TaxAmount = "0";
                    }
                    cmd.Parameters.Add(new SqlParameter("@GrossAmount", Convert.ToDouble(bill.GrossAmount)));
                    cmd.Parameters.Add(new SqlParameter("@TaxPercent", Convert.ToDouble(bill.TaxPercent)));
                    cmd.Parameters.Add(new SqlParameter("@TaxAmount", Convert.ToDouble(bill.TaxAmount)));
                    cmd.Parameters.Add(new SqlParameter("@ReffCommission", Convert.ToDouble(bill.ReffCommission)));
                    cmd.Parameters.Add(new SqlParameter("@Commisson", Convert.ToDouble(bill.Commisson)));
                    cmd.Parameters.Add(new SqlParameter("@TotalAmount", Convert.ToDouble(bill.TotalAmount)));

                    //cmd.Parameters.Add(new SqlParameter("@NonMedicalExp", Convert.ToDouble(bill.NonMedicalExp)));
                    if (bill.DiscountPercent != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(bill.DiscountPercent)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(0.00)));
                    }
                    if (bill.DiscountAmount != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(bill.DiscountAmount)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(0.00)));
                    }
                    cmd.Parameters.Add(new SqlParameter("@DiscountReason", bill.DiscountReason));
                    cmd.Parameters.Add(new SqlParameter("@NetPayableAmount", Convert.ToDouble(bill.NetPayableAmount)));

                    if (bill.BalanceAmount == "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@BalanceAmount", Convert.ToDouble(0.00)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@BalanceAmount", Convert.ToDouble(bill.BalanceAmount)));

                    }
                    cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble(bill.PreBalanceAmount)));

                    cmd.Parameters.Add(new SqlParameter("@IsPaid", bill.IsPaid));
                    //cmd.Parameters.Add(new SqlParameter("@IsBillMade", bill.IsPaid));
                    cmd.Parameters.Add(new SqlParameter("@PaidAmount", Convert.ToDouble(bill.PaidAmount)));
                    cmd.Parameters.Add(new SqlParameter("@DipositAmount ", Convert.ToDouble(0.00)));
                    cmd.Parameters.Add(new SqlParameter("@PaymentType ", bill.PaymentType));
                    cmd.Parameters.Add(new SqlParameter("@Number", bill.Number));
                    cmd.Parameters.Add(new SqlParameter("@Name", bill.Name));
                    cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(bill.Date)));
                    // cmd.Parameters.Add(new SqlParameter("@Date ", DateTime.ParseExact(bill.Date, "dd/MM/yyyy hh:mm:ss", null)));
                    cmd.Parameters.Add(new SqlParameter("@Remarks ", bill.Remarks));
                    cmd.Parameters.Add(new SqlParameter("@SancationAmount", Convert.ToDouble(bill.SancationAmount)));
                    cmd.Parameters.Add(new SqlParameter("@FinancialYearID   ", bill.FinancialYearID));
                    cmd.Parameters.Add(new SqlParameter("@CreationID", bill.CreationID));
                    cmd.Parameters.AddWithValue("@ForAuthorization",0);
                    cmd.Parameters.Add(new SqlParameter("@Mode", "ADD"));

                    int i = 0;
                    con.Open();
                    i = cmd.ExecuteNonQuery();

                    int BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);
                    bill.BillNo = BillNo;

                    string ServiceType = "";
                    if (i > 0)
                    {
                        if (bill.BillType == "IPDBill" || bill.BillType == "IPDFinalBill")
                        {

                            //    string[] doctorcharges = bill.DoctorCharges.Split(',');
                            if (bill.Services!= null)
                            {
                                for (int x = 0; x < bill.Services.Length; x++)
                                {
                                    SqlCommand cmd1 = new SqlCommand("[IUPatientBillsDetails]", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmd1.Parameters.AddWithValue("@BillNo", BillNo);
                                    cmd1.Parameters.AddWithValue("@ServiceID", bill.Services[x].SvcID);
                                    cmd1.Parameters.AddWithValue("@Servicename", bill.Services[x].SvcName);
                                    cmd1.Parameters.AddWithValue("@UnitID", bill.Services[x].UnitID);
                                    cmd1.Parameters.AddWithValue("@ChargesType", "General");
                                    cmd1.Parameters.AddWithValue("@DoctorCharges", 0);
                                    cmd1.Parameters.AddWithValue("@Rate", bill.Services[x].Rate);
                                    cmd1.Parameters.AddWithValue("@Quantity", bill.Services[x].Quantity);
                                    cmd1.Parameters.AddWithValue("@Discount", bill.Services[x].sevicedisAmt);
                                    cmd1.Parameters.AddWithValue("@Commisson", "0.00");
                                    cmd1.Parameters.AddWithValue("@ReffCommission", "0.00");
                                    cmd1.Parameters.AddWithValue("@ServiceType", bill.Services[x].ServiceType);
                                    cmd1.Parameters.AddWithValue("@TotalAmount", bill.Services[x].Total);
                                    cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");
                                    cmd1.Parameters.AddWithValue("@Discount_Service", bill.Services[x].Discount_Service);
                                    cmd1.Parameters.AddWithValue("@DiscountServiceType", bill.Services[x].DiscountServiceType);

                                    if (bill.Services[x].DoctorID.ToString() != "undefined" && bill.Services[x].DoctorID.ToString()!= "0")
                                    {
                                        cmd1.Parameters.AddWithValue("@DoctorID", bill.Services[x].DoctorID);
                                    }
                                    else
                                    {
                                        cmd1.Parameters.AddWithValue("@DoctorID", bill.Doctor);
                                    }
                                    cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                    cmd1.Parameters.AddWithValue("@ForAuthorization", 0);
                                    cmd1.Parameters.AddWithValue("@Mode", "ADD");
                                    int BillsDetails = cmd1.ExecuteNonQuery();
                                    if (bill.Services[x].ServiceType == "IPDLabBills")
                                    {
                                        ServiceType = bill.Services[x].ServiceType;
                                    }

                                }
                            }
                        }

                        #region LAB


                        //#region  add Patient Lab Bills
                        //string TestStatusforLab = "BILLING";
                        //foreach (DataRow drLab in dsPatientIPDBillsDetails.Tables[0].Rows)
                        //{
                        //    if (drLab["ServiceType"].ToString().Trim() == "IPDLabBills" || drLab["ServiceType"].ToString().Trim() == "IPDLabFinalBill" || drLab["ServiceType"].ToString().Trim() == "IPDPackageBills")
                        //    {
                        //        if (drLab["ServiceType"].ToString().Trim() == "IPDPackageBills")
                        //        {
                        //            DataSet dsPakageService = new DataSet();

                        //            dsPakageService = obj_PatientIPDBills_R.GetPatientIPDBillsForServiceDetailsPackage(HospitalID, LocationID, Convert.ToInt32(drLab["ServiceID"].ToString()));

                        //            foreach (DataRow drPackage in dsPakageService.Tables[0].Rows)
                        //            {
                        //                if (drPackage["ServiceType"].ToString() == "Test")
                        //                {
                        //                    AddPatientLabDetail(Convert.ToInt32(drPackage["ServiceOrTestID"].ToString()), TestStatusforLab, this.Mode.ToString());
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            AddPatientLabDetail(Convert.ToInt32(drLab["ServiceID"].ToString()), TestStatusforLab, this.Mode.ToString());
                        //        }
                        //    }
                        //}
                        //PatientLabWithParameter();

                        //#endregion

                        #region PatientLab
                        if (ServiceType == "IPDLabBills")
                        {
                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                            cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", bill.OPDIPDID);
                            cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", bill.FinancialYearID);
                            cmdPatientLab.Parameters.AddWithValue("@LabType", "IPDLab");

                            cmdPatientLab.Parameters.AddWithValue("@BillNo", bill.BillNo);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                            //     con.Close();

                            int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                            int LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                        #endregion

                            // Laboratory.BL_PatientLabBills
                            Laboratory.BL_PatientLabBills objlabParamiter = new Laboratory.BL_PatientLabBills();

                            for (int j = 0; j < bill.Services.Length; j++)
                            {
                                if (bill.Services[j].ServiceType == "IPDLabBills")
                                {
                                    if (LabNo > 0)
                                    {

                                        SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                                        cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                                        cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                                        cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;

                                        //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                                        PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                                        cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                                        cmdPatientLabDetails.Parameters.AddWithValue("@TestID", bill.Services[j].SvcID);

                                        cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");

                                        cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", 0);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Add");

                                        int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                                        PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value.ToString());
                                        //   PatientLabDetail.Add(obPatient.PatientLabDetailID.ToString());

                                    }

                                    DataSet ds = new DataSet();
                                    int TestID2 = 0;
                                    TestID2 = Convert.ToInt32(bill.Services[j].SvcID);
                                    ds = objlabParamiter.GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);
                                    foreach (DataRow drParameter in ds.Tables[1].Rows)
                                    {
                                        DataView dv = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                                        DataSet dsOnlyParameter = new DataSet();
                                        DataTable dt = new DataTable();
                                        dt = dv.ToTable();
                                        dsOnlyParameter.Tables.Add(dt);
                                        //  DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obj.TotalDay + " and DaysTo >=" + obj.TotalDay + " and Gender ='" + obj.Gender + "')", "", DataViewRowState.CurrentRows);

                                        SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                                        cmdParamiter.CommandType = CommandType.StoredProcedure;
                                        if (dv.Count != 0)
                                        {
                                            //  DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();
                                            cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                            cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                            cmdParamiter.Parameters.AddWithValue("@LabNo", LabNo);

                                            cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                            // cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                            //  int PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
                                            cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", PatientLabDetailID);
                                            cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);
                                            cmdParamiter.Parameters.AddWithValue("@ParameterID", dv[0]["ParameterID"].ToString());
                                            cmdParamiter.Parameters.AddWithValue("@NormalRangeID", dv[0]["NormalRangeID"].ToString());
                                            cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                            cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                            cmdParamiter.Parameters.AddWithValue("@NLH", 0);
                                            cmdParamiter.Parameters.AddWithValue("@ConvLow", dv[0]["ConvLow"].ToString());
                                            cmdParamiter.Parameters.AddWithValue("@ConvHigh", dv[0]["ConvHigh"].ToString());
                                            cmdParamiter.Parameters.AddWithValue("@Remark", 0);
                                            cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                            cmdParamiter.Parameters.AddWithValue("@CreationID", UserID);
                                            cmdParamiter.Parameters.AddWithValue("@Mode", "Add");
                                            //dsPatientLabWithParameter.Tables[0].Rows.Add(drW);

                                        }
                                        else
                                        {
                                            // DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();
                                            cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                            cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                            cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                            //cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                            // int PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
                                            cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", PatientLabDetailID);
                                            cmdParamiter.Parameters.AddWithValue("@LabNo", LabNo);
                                            // cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", 0);
                                            cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);
                                            cmdParamiter.Parameters.AddWithValue("@ParameterID", drParameter["ParameterID"].ToString());
                                            cmdParamiter.Parameters.AddWithValue("@NormalRangeID", 0);
                                            cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                            cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                            cmdParamiter.Parameters.AddWithValue("@NLH", "");
                                            cmdParamiter.Parameters.AddWithValue("@ConvLow", "");
                                            cmdParamiter.Parameters.AddWithValue("@ConvHigh", "");
                                            cmdParamiter.Parameters.AddWithValue("@Remark", 0);
                                            cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                            cmdParamiter.Parameters.AddWithValue("@CreationID", UserID);
                                            cmdParamiter.Parameters.AddWithValue("@Mode", "Add");
                                            // dsPatientLabWithParameter.Tables[0].Rows.Add(drW);
                                        }



                                        cmdParamiter.ExecuteNonQuery();

                                    }
                                }

                            }
                        }
                        //--------------------------------------------end




                        #endregion
                        bill.Message = null;
                        return bill;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                bill.Message = ex.Message;
                return bill;
            }

            //else
            //    return "0";

        }

        public IPDBill UpdateIPDBill(IPDBill bill)
        {
            string ServiceType = "";
            Connect();
            con.Open();
            #region BillsModify
            #region ForAuthorization
            int ForAuthorization = 0;
            // Aurthorise
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj4 = new Buisness_Logic.Master.BL_MasterSetting();
            DataSet dsMasterSetting = new DataSet();
            dsMasterSetting = obj4.GetMasterSetting();
            KeystoneProject.Buisness_Logic.Hospital.BL_Users user = new Buisness_Logic.Hospital.BL_Users();
            DataSet dsAuthorizationRights = new DataSet();
            // List<Users> Search = new List<Users>();
            dsAuthorizationRights = user.GetUsers(UserID);
            if (dsAuthorizationRights.Tables[0].Rows.Count > 0)
            {
                string chkAurthoriseUserWise = dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();
                bill.ForAuthorization = "0";

                if (chkAurthoriseUserWise == "True")
                {
                    //  dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                    bill.ForAuthorization = "0";

                }
                    if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
                    {
                        bill.ForAuthorization = "1";
                        con.Close();
                        SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                        cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                        cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                        // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                        cmdIUForAuthorization.Parameters.AddWithValue("@UserID", UserID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", bill.BillNo);
                        cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", bill.BillNo);
                        cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "IPDBill");
                        cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", bill.PatientRegNO);
                        cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", UserID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                        con.Open();
                        cmdIUForAuthorization.ExecuteNonQuery();
                    }

                }
            
            #endregion
            Laboratory.BL_PatientLabBills BillModifyDetails = new Laboratory.BL_PatientLabBills();
            DataSet dsPatientOldBills = new DataSet();
            try
            {
                dsPatientOldBills = GetPatientBills(Convert.ToInt32(bill.BillNo));
                if(con.State.ToString()=="Closed")
                {
                    con.Open();
                }
                foreach (DataRow dr in dsPatientOldBills.Tables[0].Rows)
                {
                    SqlCommand cmdBillModify = new SqlCommand("IUPatientBillsModify", con);
                    cmdBillModify.CommandType = CommandType.StoredProcedure;
                    cmdBillModify.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdBillModify.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdBillModify.Parameters.AddWithValue("@BillNo", dr["BillNo"]);

                    cmdBillModify.Parameters.AddWithValue("@PrintBillNo", dr["PrintBillNo"]);
                    cmdBillModify.Parameters.AddWithValue("@FinancialYearID", dr["FinancialYearID"]);
                    cmdBillModify.Parameters.AddWithValue("@SancationAmount", dr["SancationAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@NonMedicalExp", dr["NonMedicalExp"]);
                    cmdBillModify.Parameters.AddWithValue("@BillDate", dr["BillDate"]);
                    cmdBillModify.Parameters.AddWithValue("@PatientRegNO", dr["PatientRegNO"]);
                    cmdBillModify.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                    cmdBillModify.Parameters.AddWithValue("@BillType", dr["BillType"]);
                    cmdBillModify.Parameters.AddWithValue("@GrossAmount", dr["GrossAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@TaxPercent", dr["TaxPercent"]);
                    cmdBillModify.Parameters.AddWithValue("@ReffCommission", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@Commisson", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@TotalAmount", dr["TotalAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@DiscountPercent", "");
                    cmdBillModify.Parameters.AddWithValue("@DiscountAmount", dr["DiscountAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@NetPayableAmount", dr["NetPayableAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@BalanceAmount", dr["BalanceAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@PreBalanceAmount", dr["PreBalanceAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@IsPaid", dr["IsPaid"]);
                    cmdBillModify.Parameters.AddWithValue("@PaidAmount", dr["PaidAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@TaxAmount", dr["TaxAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@DipositAmount", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@ForAuthorization", bill.ForAuthorization);
                    // cmdBillModify.Parameters.AddWithValue(" @DipositAmount", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@IsBillMade", "false");
                    cmdBillModify.Parameters.AddWithValue("@CreationID", UserID);
                    cmdBillModify.Parameters.AddWithValue("@Mode", "Add");
                    //   con.Close();
                    // con.Open();
                    int IUPatientBillsModify = cmdBillModify.ExecuteNonQuery();
                   
                }
                //con.Close();

            #endregion

                #region BillsDetailsModify

                //BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
                DataSet dsPatientBllDetails = new DataSet();
                dsPatientBllDetails = BillModifyDetails.GetPatientLabOLdBillsDetails(HospitalID, LocationID, Convert.ToInt32(bill.BillNo));
                SqlCommand updateauth = new SqlCommand("update PatientBillsDetailsModify set RowStatus = 2 where BillNo = '" + bill.BillNo + "' and RowStatus = 0", con);
                 updateauth.ExecuteNonQuery();
                foreach (DataRow dr1 in dsPatientBllDetails.Tables[0].Rows)
                {

                    SqlCommand cmdBillModifyDetail = new SqlCommand("IUPatientBillsDetailsModify", con);
                    cmdBillModifyDetail.CommandType = CommandType.StoredProcedure;
                    cmdBillModifyDetail.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@BillModifyID", 0);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceID", dr1["ServiceID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceName", dr1["ServiceName"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@BillNo", dr1["BillNo"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@UnitID ", dr1["UnitID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ChargesType", dr1["ChargesType"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Rate", dr1["Rate"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Quantity", dr1["Quantity"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Discount", dr1["Discount"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ReffCommission", "0.00");
                    cmdBillModifyDetail.Parameters.AddWithValue("@Commisson", "0.00");
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceType", dr1["ServiceType"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@TotalAmount", dr1["TotalAmount"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@HideInBilling", dr1["HideInBilling"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@DoctorID", dr1["DoctorID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ForAuthorization", 1);
                    cmdBillModifyDetail.Parameters.AddWithValue("@DiscountServiceType", dr1["DiscountServiceType"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Discount_Service", dr1["Discount_Service"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@DoctorCharges", dr1["DoctorCharges"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@CreationID", UserID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Mode", "Add");
                    // con.Close();
                    // con.Open();

                    int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();

                    // con.Close();
                }

                #endregion



                SqlCommand cmd = new SqlCommand("IUPatientBills", con);
                //SqlCommand cmd = new SqlCommand("GetStudentDetails", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                //  cmd.Parameters.Add(new SqlParameter("@PrintBillNo ", bill.PrintBillNo));

                //cmd.Parameters.AddWithValue("@BillNo", 0);
                //cmd.Parameters["@BillNo"].Direction = ParameterDirection.Output;

                //cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(bill.BillDate)));
                cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(bill.BillDate)));
                cmd.Parameters.Add(new SqlParameter("@BillNo", bill.BillNo));
                cmd.Parameters.Add(new SqlParameter("@PatientRegNO", bill.PatientRegNO));
                cmd.Parameters.Add(new SqlParameter("@OPDIPDID", bill.OPDIPDID));
                cmd.Parameters.Add(new SqlParameter("@BillType ", bill.BillType));
                cmd.Parameters.Add(new SqlParameter("@GrossAmount", Convert.ToDouble(bill.GrossAmount)));
                cmd.Parameters.Add(new SqlParameter("@TaxPercent", Convert.ToDouble(bill.TaxPercent)));
                cmd.Parameters.Add(new SqlParameter("@TaxAmount", Convert.ToDouble(bill.TaxAmount)));
                cmd.Parameters.Add(new SqlParameter("@ReffCommission", Convert.ToDouble(bill.ReffCommission)));
                cmd.Parameters.Add(new SqlParameter("@Commisson", Convert.ToDouble(bill.Commisson)));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", Convert.ToDouble(bill.TotalAmount)));
                cmd.Parameters.Add(new SqlParameter("@NonMedicalExp", Convert.ToDouble(bill.NonMedicalExp)));
                if (bill.DiscountPercent != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(bill.DiscountPercent)));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(0.00)));
                }
                if (bill.DiscountAmount != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(bill.DiscountAmount)));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(0.00)));
                }
                cmd.Parameters.Add(new SqlParameter("@DiscountReason", bill.DiscountReason));
                cmd.Parameters.Add(new SqlParameter("@NetPayableAmount", Convert.ToDouble(bill.NetPayableAmount)));
                cmd.Parameters.Add(new SqlParameter("@BalanceAmount", Convert.ToDouble(bill.BalanceAmount)));
                if (bill.PreBalanceAmount == String.Empty)
                    cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble("0.0")));
                else
                    cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble(bill.PreBalanceAmount)));
                cmd.Parameters.Add(new SqlParameter("@IsPaid", bill.IsPaid));
                //cmd.Parameters.Add(new SqlParameter("@IsBillMade", bill.IsPaid));
                cmd.Parameters.Add(new SqlParameter("@PaidAmount", Convert.ToDouble(bill.PaidAmount)));
                cmd.Parameters.Add(new SqlParameter("@DipositAmount ", Convert.ToDouble(bill.DipositAmount)));
                cmd.Parameters.Add(new SqlParameter("@PaymentType ", bill.PaymentType));
                cmd.Parameters.Add(new SqlParameter("@Number", bill.Number));
                cmd.Parameters.Add(new SqlParameter("@Name", bill.Name));
                // cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(bill.Date)));
                cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(bill.Date)));
                cmd.Parameters.Add(new SqlParameter("@Remarks ", bill.Remarks));
                cmd.Parameters.Add(new SqlParameter("@SancationAmount", Convert.ToDouble(bill.SancationAmount)));
                cmd.Parameters.Add(new SqlParameter("@FinancialYearID   ", bill.FinancialYearID));
                cmd.Parameters.Add(new SqlParameter("@CreationID", bill.CreationID));
                cmd.Parameters.AddWithValue("@ForAuthorization", bill.ForAuthorization);
                cmd.Parameters.Add(new SqlParameter("@Mode", "EDIT"));

                int i = 0;
                //con.Open();
                i = cmd.ExecuteNonQuery();


                if (i > 0)
                {
                    // int BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);
                    SqlCommand update = new SqlCommand("update PatientBillsDetails set RowStatus = 2 where BillNo = '" + bill.BillNo + "' and RowStatus = 0", con);
                int po=    update.ExecuteNonQuery();
                    //if (bill.BillType != "IPDFinalBill")
                    //{
                if (bill.Services != null)
                {
                    for (int x = 0; x < bill.Services.Length; x++)
                    {
                            if(bill.Services[x].Total=="NaN")
                            {
                                bill.Services[x].Total = "0";
                            }
                        SqlCommand cmd1 = new SqlCommand("IUPatientBillsDetails", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd1.Parameters.AddWithValue("@BillNo", bill.BillNo);
                        cmd1.Parameters.AddWithValue("@ServiceID", bill.Services[x].SvcID);
                        cmd1.Parameters.AddWithValue("@Servicename", bill.Services[x].SvcName);
                        cmd1.Parameters.AddWithValue("@UnitID", bill.Services[x].UnitID);
                        cmd1.Parameters.AddWithValue("@ChargesType", "General");
                        cmd1.Parameters.AddWithValue("@DoctorCharges", 0);
                        cmd1.Parameters.AddWithValue("@Rate", bill.Services[x].Rate);
                        cmd1.Parameters.AddWithValue("@Quantity", bill.Services[x].Quantity);
                        cmd1.Parameters.AddWithValue("@Discount", bill.Services[x].sevicedisAmt);
                        cmd1.Parameters.AddWithValue("@Commisson", "0.00");
                        cmd1.Parameters.AddWithValue("@ReffCommission", "0.00");
                        cmd1.Parameters.AddWithValue("@ServiceType", bill.Services[x].ServiceType);
                        cmd1.Parameters.AddWithValue("@TotalAmount", bill.Services[x].Total);
                        cmd1.Parameters.AddWithValue("@Discount_Service", bill.Services[x].Discount_Service);
                        cmd1.Parameters.AddWithValue("@DiscountServiceType", bill.Services[x].DiscountServiceType);
                        cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");

                        if (bill.Services[x].DoctorID == "undefined")
                        {
                            cmd1.Parameters.AddWithValue("@DoctorID", 0);
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@DoctorID", bill.Services[x].DoctorID);
                        }


                        cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@ForAuthorization", bill.Services[x].Authorization);
                            cmd1.Parameters.AddWithValue("@Mode", "ADD");
                        int BillsDetails = cmd1.ExecuteNonQuery();


                        if (bill.Services[x].ServiceType == "IPDLabBills")
                        {
                            ServiceType = bill.Services[x].ServiceType;
                        }
                    }
                    // }
                }
                }
                //---------AddLab


                #region PatientLab

                DataSet dsLabDetail = new DataSet();
                DataSet dsLab = new DataSet();
                Laboratory.BL_PatientLabBills BL_Lab = new Laboratory.BL_PatientLabBills();
                dsLab = BL_Lab.GetPatientLab(HospitalID, LocationID, bill.BillNo);
                dsLabDetail = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, bill.BillNo);
                int LabNo = 0;
                if (ServiceType == "IPDLabBills")
                {
                    if (dsLab.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsLab.Tables[0].Rows)
                        {
                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", dr["LabNo"]);
                            //  cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);

                            cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", bill.FinancialYearID);


                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                            cmdPatientLab.Parameters.AddWithValue("@LabType", dr["LabType"]);
                            cmdPatientLab.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Edit");
                            //     con.Close();

                            int IUPatientL = cmdPatientLab.ExecuteNonQuery();
                            LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                        }

                    }
                    else
                    {

                        SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                        cmdPatientLab.CommandType = CommandType.StoredProcedure;
                        cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                        cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                        cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                        cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", bill.OPDIPDID);
                        cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", bill.FinancialYearID);

                        cmdPatientLab.Parameters.AddWithValue("@LabType", "IPDLab");

                        cmdPatientLab.Parameters.AddWithValue("@BillNo", bill.BillNo);
                        cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                        //     con.Close();
                        int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                        LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);

                    }



                #endregion



                    #region LabDetail

                    int DetailrowEdit = 0;
                    int PatientLabDetailID = 0;
                    for (int j = 0; j < bill.Services.Length; j++)
                    {

                        SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                        cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                        cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                        if (bill.Services[j].ServiceType == "IPDLabBills")
                        {
                            // if (bill.Services[j].Mode == "Add")
                            // {

                            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                            cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                            PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", bill.Services[j].SvcID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");

                            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);

                            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Add");

                            //  }

                            //else
                            //{
                            //    if (bill.Services[j].ServiceType == "OPDLabBills")
                            //    {

                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", dsLabDetail.Tables[0].Rows[DetailrowEdit]["PatientLabDetailID"].ToString());
                            //            //cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //            //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                            //            //   PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", bill.Services[j].SvcID);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");

                            //            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Edit");
                            //            DetailrowEdit++;

                            //    }
                            //}

                            int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                            PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value.ToString());
                            //   PatientLabDetail.Add(obPatient.PatientLabDetailID.ToString());

                    #endregion


                            #region IUPatientLabWithParameter
                            if (bill.Services[j].Mode == "Add")
                            {
                                Laboratory.BL_PatientLabBills objlabParamiter = new Laboratory.BL_PatientLabBills();

                                DataSet ds = new DataSet();
                                int TestID2 = 0;
                                TestID2 = Convert.ToInt32(bill.Services[j].SvcID);

                                ds = objlabParamiter.GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);

                                foreach (DataRow drParameter in ds.Tables[1].Rows)
                                {

                                    DataView dv = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                                    DataSet dsOnlyParameter = new DataSet();
                                    DataTable dt = new DataTable();
                                    dt = dv.ToTable();
                                    dsOnlyParameter.Tables.Add(dt);


                                    // DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obj.TotalDay + " and DaysTo >=" + obj.TotalDay + " and Gender ='" + obj.Gender + "')", "", DataViewRowState.CurrentRows);
                                    SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                                    cmdParamiter.CommandType = CommandType.StoredProcedure;
                                    if (dv.Count != 0)
                                    {


                                        //  DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();


                                        cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmdParamiter.Parameters.AddWithValue("@LabNo", LabNo);

                                        cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                        // cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                        //  int PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
                                        cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", PatientLabDetailID);
                                        cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);
                                        cmdParamiter.Parameters.AddWithValue("@ParameterID", dv[0]["ParameterID"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@NormalRangeID", dv[0]["NormalRangeID"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                        cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                        cmdParamiter.Parameters.AddWithValue("@NLH", 0);
                                        cmdParamiter.Parameters.AddWithValue("@ConvLow", dv[0]["ConvLow"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@ConvHigh", dv[0]["ConvHigh"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@Remark", 0);
                                        cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                        cmdParamiter.Parameters.AddWithValue("@CreationID", UserID);
                                        cmdParamiter.Parameters.AddWithValue("@Mode", "Add");
                                        //dsPatientLabWithParameter.Tables[0].Rows.Add(drW);


                                        cmdParamiter.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (dsLab.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsLab.Tables[0].Rows)
                        {

                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", dr["LabNo"]);
                            //  cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                            cmdPatientLab.Parameters.AddWithValue("@LabType", dr["LabType"]);
                            cmdPatientLab.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Edit");
                            //     con.Close();

                            int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                            LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);

                        }
                    }
                }
                            #endregion

                bill.Message = null;
                return bill;
                //End----------
            }
            catch(Exception ex)
            {
                bill.Message = ex.Message;
                return bill;
            }
            

        }


        public DataSet GetAllPackageServiceWise(int ServiceID, int WardID, string PatientOPIDNO)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllPackageServiceWise", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
            cmd.Parameters.AddWithValue("@WardID", WardID);
            cmd.Parameters.AddWithValue("@PatientIPDNO", PatientOPIDNO);
            DataSet ds = new DataSet();
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            //  ds = new DataSet();

            da.Fill(ds);

            return ds;

        }


        

        #region ServiceGroupBind
        public List<OPDBill> GetServiceGrp(string Name,string Type)
        {
            Connect();
            List<OPDBill> servicegrouplist = new List<OPDBill>();
            SqlCommand cmd = new SqlCommand("select ServiceGroupID,ServiceGroupName ,HSNCode,ServiceType from ServiceGroup where ServiceGroupName like '%" + Name + "%' and  ServiceType!='"+ Type + "' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                servicegrouplist.Add(
                    new OPDBill
                    {
                        ServiceGroupCode = dr["HSNCode"].ToString(),
                        ServiceGroupID = Convert.ToInt16(dr["ServiceGroupID"]),
                        ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
                        ServiceType="",
                    });

            }

            // LAB Group
            SqlCommand cmd1 = new SqlCommand("select CategoryID ,CategoryName  from Category where CategoryName like '%" + Name + "%'   and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0", con);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            con.Open();
            sd1.Fill(dt1);
            con.Close();
            foreach (DataRow dr in dt1.Rows)
            {
                servicegrouplist.Add(
                    new OPDBill
                    {
                        ServiceGroupID = Convert.ToInt16(dr["CategoryID"]),
                        ServiceGroupName = Convert.ToString(dr["CategoryName"]) + "   ( Lab )",
                        ServiceType = "Lab",
                    });

            }
            return servicegrouplist;
        }

        #endregion

        #region ServiceBind
        public List<ServiceName> GetServiceName(string Name, string GroupID)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();
            //SqlCommand cmd = new SqlCommand("GetRole", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Select ServiceName,ServiceID,ServiceGroupID from Services where ServiceType in ('IPD','Both') AND ServiceName like '%" + Name + "%' and ServiceGroupID = '" + GroupID + "' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                servicenamelist.Add(
                    new ServiceName
                    {
                        ServiceGroupID = Convert.ToString(dr["ServiceGroupID"]),
                        SvcName = Convert.ToString(dr["ServiceName"]),
                        SvcID = Convert.ToString(dr["ServiceID"])
                    });
            }
            return servicenamelist;
        }
        #endregion

        #region Service_Leve_EventFillData
        public DataSet GetServiceDetailByName(string NameID, string GroupID, string ServiceType)
        {
            Connect();
            SqlCommand cmd;
            List<ServiceName> servicenamelist = new List<ServiceName>();
            if (ServiceType != "Package")
            {
                if (GroupID == "%")
                {
                    cmd = new SqlCommand("select HSNCode, BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,     (select ServiceGroup.ServiceGroupName from ServiceGroup where ServiceGroup.ServiceGroupID = Services.ServiceGroupID) as 'ServiceGroupName'   ,   cast( GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges   from Services where ServiceType in ('IPD','Both') AND ServiceName like '" + NameID + "%' and  ServiceGroupID like'" + GroupID + "'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
                }
                else
                {
                    cmd = new SqlCommand("select HSNCode, BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,      (select ServiceGroup.ServiceGroupName from ServiceGroup where ServiceGroup.ServiceGroupID = Services.ServiceGroupID) as 'ServiceGroupName'   ,cast( GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges   from Services where ServiceType in ('IPD','Both') AND ServiceName like '" + NameID + "%' and  ServiceGroupID='" + GroupID + "'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

                }
            }
            else
            {
                cmd = new SqlCommand("select distinct (select ServiceGroup.ServiceGroupName from ServiceGroup where ServiceGroupID= Services.ServiceGroupID) as 'ServiceGroupName',   BillCharges,Services. ServiceName,Services.ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,        cast( GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges  ,HSNCode from ServiceDetailsPackage   left  join Services on Services.ServiceID = ServiceDetailsPackage.ServiceID where Services.RowStatus = 0 and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + " AND ServiceName like '" + NameID + "%' ", con);

            }
                SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
          
            return ds;
        }

        #endregion
        #region ServiceTPA
        public DataSet GetServiceDetailByNameTPA(string NameID, string GroupID, string Code, int OrganizationID)
        {
            SqlCommand cmd;
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();
            if (GroupID == "%")
            {
                 cmd = new SqlCommand("select distinct  Services.UnitID, Services.TPAHSNCode, Services.ServiceID  ,upper(ServiceName) as ServiceName ,Services.ServiceGroupID ,ServiceGroup.ServiceGroupName,ServiceGroup.HSNCode from Services left join ServicesTPAWise on ServicesTPAWise.ServiceID=Services.ServiceID left join ServiceGroup on ServiceGroup.ServiceGroupID=Services.ServiceGroupID  and ServicesTPAWise.HospitalID=" + HospitalID + " and ServicesTPAWise.LocationID=" + LocationID + "  and ServicesTPAWise.RowStatus=0 where   ServiceGroup.RowStatus=0 and ServiceGroup.LocationID=" + LocationID + " and  ServiceGroup.HospitalID=" + HospitalID + " and Services.RowStatus = 0 and ServicesTPAWise.HideInBilling='Yes'  and ServicesTPAWise.OrganizationID = " + OrganizationID + "   and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + "  and Services.ServiceType in ('IPD','Both') and (Services.TPAHSNCode like '" + Code + "%' or Services.TPAHSNCode is null) and   Services.ServiceGroupID like '" + GroupID + "' and  Services.ServiceName like '" + NameID + "%'  order by  ServiceName asc", con);
            }
            else
            {
                cmd = new SqlCommand("select distinct  Services.UnitID, Services.TPAHSNCode, Services.ServiceID  ,upper(ServiceName) as ServiceName ,Services.ServiceGroupID ,ServiceGroup.ServiceGroupName,ServiceGroup.HSNCode from Services left join ServicesTPAWise on ServicesTPAWise.ServiceID=Services.ServiceID left join ServiceGroup on ServiceGroup.ServiceGroupID=Services.ServiceGroupID  and ServicesTPAWise.HospitalID=" + HospitalID + " and ServicesTPAWise.LocationID=" + LocationID + "  and ServicesTPAWise.RowStatus=0 where   ServiceGroup.RowStatus=0 and ServiceGroup.LocationID=" + LocationID + " and  ServiceGroup.HospitalID=" + HospitalID + " and Services.RowStatus = 0 and ServicesTPAWise.HideInBilling='Yes'  and ServicesTPAWise.OrganizationID = " + OrganizationID + "   and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + "  and Services.ServiceType in ('IPD','Both') and (Services.TPAHSNCode like '" + Code + "%' or Services.TPAHSNCode is null) and   Services.ServiceGroupID ='" + GroupID + "' and  Services.ServiceID like '" + NameID + "'  order by  ServiceName asc", con);

            }
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        #region ServiceTestGropTPAWise

        public DataSet GetTestGroupTpaWise(int OrganizationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@OrganizationID", SqlDbType.Int);
                param[2].Value = OrganizationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestGroupTpaWise", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        #endregion
        #region GetPatientLab Service  ForTestMaster And TPA

        public DataSet GetPatientLabBillsForTestMaster(int DoctorID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                param[2].Value = DoctorID;
                param[3] = new SqlParameter("@Type", SqlDbType.NVarChar);
                param[3].Value = "CONS";

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientLabBillsForTestMaster", param);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion
        public DataSet GetServicesDetailsforIPDBillsTPA(int ServiceID, int WardID, int OrganizationID)
        {
               Connect();
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@ServiceID", SqlDbType.Int);
                    param[0].Value = ServiceID;
                    param[1] = new SqlParameter("@WardID", SqlDbType.Int);
                    param[1].Value = WardID;
                    param[2] = new SqlParameter("@OrganizationID", SqlDbType.Int);
                    param[2].Value = OrganizationID;
                    param[3] = new SqlParameter("@HosapitalID", SqlDbType.Int);
                    param[3].Value = HospitalID;
                    param[4] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[4].Value = LocationID;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetServicesDetailsforIPDBillsBasedOnTPA", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        

        public DataSet GetServiceDetailByCode(string Code)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();

            SqlCommand cmd = new SqlCommand("select HSNCode, BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,GeneralCharges,EmergencyCharges from Services where ServiceType in ('IPD','Both') AND HSNCode LIKE '" + Code + "%'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();

            return ds;
        }

       

        #region ServiceDoctorBind
        public List<OPDBill> GetDoctorName(string Name)
        {
            Connect();
            List<OPDBill> doctorlist = new List<OPDBill>();
          
            SqlCommand cmd = new SqlCommand("select DoctorID, DoctorPrintName from Doctor where DoctorPrintName LIKE '%" + Name + "%' AND DoctorType = 'Consultant' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                doctorlist.Add(
                    new OPDBill
                    {
                        DoctorID = Convert.ToInt16(dr["DoctorID"]),
                        DoctorName = Convert.ToString(dr["DoctorPrintName"])
                    });
            }
            return doctorlist;
        }

        #endregion


        #region FillDataOldBillEditToAll
        public List<IPDBill> FillDataOldBillEditToAll(string PatientIPDNo,int BillNo)
        {
            Connect();
            List<IPDBill> ipdbilllist = new List<IPDBill>();
            SqlCommand cmd = new SqlCommand("GetPatientBillsForIPDNo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientIPDNo", Convert.ToInt32(PatientIPDNo)));
            //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(ds);
            sd.Fill(dt);
            if (HttpContext.Current.Session["GetPatientBillsForIPDNosection"] == null)
            {
                HttpContext.Current.Session["GetPatientBillsForIPDNosection"] = ds;
            }

            //test.Business_Logic.Laboratory.BL_PatientLabBills BL_Lab = new test.Business_Logic.Laboratory.BL_PatientLabBills();
            string DisCountResionID = "";
            string DisCountResion = "";
            if (BillNo > 0)
            {
               //DataTable dt1 = (DataTable) HttpContext.Current.Session["GetPatientBillsForIPDNo"];
                DataView dv1 = new DataView(ds.Tables[0], "BillNo = " + BillNo + "", "", DataViewRowState.CurrentRows);
                dt.Reset();

                dt = dv1.ToTable();

                if (dt.Rows.Count > 0)
                {
                    BL_PatientOPDBill objdiscountresion = new BL_PatientOPDBill();
                    DataSet dsdisResion = objdiscountresion.GetDiscountReason("%");
                    if (dt.Rows[0]["DiscountReason"].ToString() != "")
                    {
                        DataView dv = new DataView(dsdisResion.Tables[0], "DiscountReasonID=" + dt.Rows[0]["DiscountReason"].ToString() + "", "", DataViewRowState.CurrentRows);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            DisCountResionID = dv.ToTable().Rows[0]["DiscountReasonID"].ToString();
                            DisCountResion = dv.ToTable().Rows[0]["DiscountReason"].ToString();
                        }
                    }
                }
            }  

            Laboratory.BL_PatientLabBills BL_Lab = new Laboratory.BL_PatientLabBills();

            for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand cmd2 = new SqlCommand("GetPatientBillsDetailsIPDNo", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    cmd2.Parameters.Add(new SqlParameter("@PatientIPDNo", Convert.ToInt32(PatientIPDNo)));
                    // cmd2.Parameters.Add(new SqlParameter("@BillNo", 56));

                    SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    //con.Open();
                    sd2.Fill(ds2);
                    con.Close();
                    ServiceName[] ipdbillssvc = new ServiceName[ds2.Tables[0].Rows.Count];
                    DataSet ds1 = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(ds.Tables[0].Rows[i]["BillNo"]));
                    string Hederchange = "";
                    if (BillNo == 0)
                    {
                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                    {

                        string ServiceID = ds2.Tables[0].Rows[j]["ServiceID"].ToString();
                        DataView dv1 = new DataView(ds1.Tables[0], "TestID = " + ServiceID + "", "", DataViewRowState.CurrentRows);


                        DataRow dr2 = ds2.Tables[0].Rows[j];
                        ipdbillssvc[j] = new ServiceName();
                        if (Hederchange != dr2["BillNo"].ToString())
                        {
                            ipdbillssvc[j].BillNoChangeHeder = "ChangeRow";
                            Hederchange = dr2["BillNo"].ToString();
                        }
                        ipdbillssvc[j].BillNo = dr2["BillNo"].ToString();
                        ipdbillssvc[j].PBillNo = dr2["PrintBillNo"].ToString();
                        ipdbillssvc[j].BillDate = Convert.ToDateTime(dr2["BillDate"]).ToString("dd-MM-yyyy");
                        ipdbillssvc[j].SvcID = Convert.ToString(dr2["ServiceID"]);
                        ipdbillssvc[j].SvcName = Convert.ToString(dr2["ServiceName"]);
                        ipdbillssvc[j].Rate = Convert.ToString(dr2["Rate"]);
                        ipdbillssvc[j].Quantity = Convert.ToString(dr2["Quantity"]);
                        ipdbillssvc[j].Total = Convert.ToString(dr2["TotalAmount"]);
                        ipdbillssvc[j].UnitID = Convert.ToString(dr2["UnitID"]);
                        ipdbillssvc[j].UnitName = "NUMBER";
                        ipdbillssvc[j].DiscountServiceType = dr2["DiscountServiceType"].ToString();
                        ipdbillssvc[j].Discount_Service = dr2["Discount_Service"].ToString();
                        ipdbillssvc[j].DoctorID = dr2["DoctorID"].ToString();
                        // ipdbillssvc[j].ServiceType = "";
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            ipdbillssvc[j].ServiceType = "IPDLabBills";
                        }
                        else
                        {
                            ipdbillssvc[j].ServiceType = "IPD";
                        }
                    }
                }
                    DataRow dr = dt.Rows[i];
                string DateTimestr = Convert.ToString(dr["BillDate"]);
                DateTime DateTime = Convert.ToDateTime(DateTimestr);
                ipdbilllist.Add(
                new IPDBill
                {
                    //  PatientId = Convert.ToInt32(dr["LocationID"]),


                    BillDateStr = DateTime.ToString("dd/MM/yyyy"),
                    BillTimeStr = DateTime.ToString("hh:mm:ss"),
                    GrossAmount = Convert.ToString(dr["GrossAmount"]),
                    TotalAmount = Convert.ToString(dr["TotalAmount"]),
                    DiscountPercent = Convert.ToString(dr["DiscountPercent"]),
                    DiscountAmount = Convert.ToString(dr["DiscountAmount"]),
                    DiscountReason = Convert.ToString(dr["DiscountReason"]),
                    NetPayableAmount = Convert.ToString(dr["NetPayableAmount"]),
                    TaxAmount = Convert.ToString(dr["TaxAmount"]),
                    BalanceAmount = Convert.ToString(dr["BalanceAmount"]),
                    DipositAmount = Convert.ToString(dr["DipositAmount"]),
                    PreBalanceAmount = Convert.ToString(dr["PreBalanceAmount"]),
                    NonMedicalExp = dr["NonMedicalExp"].ToString(),
                    IsPaid = Convert.ToString(dr["IsPaid"]),
                    PaidAmount = Convert.ToString(dr["PaidAmount"]),
                    Services = ipdbillssvc,
                    Name = dr["Name"].ToString(),
                    Number = dr["Number"].ToString(),
                    Date = dr["Date"].ToString(),
                    Remarks = dr["Remarks"].ToString(),
                    PaymentType = dr["PaymentType"].ToString(),
                    DiscountReasonID = DisCountResionID,
                    DisCountResion = DisCountResion,

                });

            }


            return ipdbilllist;
        }

        #endregion

        public IPDBill UpdateAllIPDBill(IPDBill bill)
        {
     
            string ServiceType = "";
            Connect();
            con.Open();
            #region BillsModify

            Laboratory.BL_PatientLabBills BillModifyDetails = new Laboratory.BL_PatientLabBills();
            DataSet dsPatientOldBills = new DataSet();
            try
            {
                dsPatientOldBills = BillModifyDetails.GetPatientOldLabBills(Convert.ToInt32(HospitalID), Convert.ToInt32(LocationID), Convert.ToInt32(bill.BillNo));
                foreach (DataRow dr in dsPatientOldBills.Tables[0].Rows)
                {
                    SqlCommand cmdBillModify = new SqlCommand("IUPatientBillsModify", con);
                    cmdBillModify.CommandType = CommandType.StoredProcedure;
                    cmdBillModify.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdBillModify.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdBillModify.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                    cmdBillModify.Parameters.AddWithValue("@PatientRegNO", dr["PatientRegNO"]);
                    cmdBillModify.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                    cmdBillModify.Parameters.AddWithValue("@BillType", dr["BillType"]);
                    cmdBillModify.Parameters.AddWithValue("@GrossAmount", dr["GrossAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@TaxPercent", dr["TaxPercent"]);
                    cmdBillModify.Parameters.AddWithValue("@ReffCommission", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@Commisson", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@TotalAmount", dr["TotalAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@DiscountPercent", "");
                    cmdBillModify.Parameters.AddWithValue("@DiscountAmount", dr["DiscountAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@NetPayableAmount", dr["NetPayableAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@BalanceAmount", dr["BalanceAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@PreBalanceAmount", dr["PreBalanceAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@IsPaid", dr["IsPaid"]);
                    cmdBillModify.Parameters.AddWithValue("@PaidAmount", dr["PaidAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@TaxAmount", dr["TaxAmount"]);
                    cmdBillModify.Parameters.AddWithValue("@DipositAmount", "0.00");

                    // cmdBillModify.Parameters.AddWithValue(" @DipositAmount", "0.00");
                    cmdBillModify.Parameters.AddWithValue("@IsBillMade", "false");
                    cmdBillModify.Parameters.AddWithValue("@CreationID", UserID);
                    cmdBillModify.Parameters.AddWithValue("@Mode", "Add");
                    //   con.Close();
                    // con.Open();

                    int IUPatientBillsModify = cmdBillModify.ExecuteNonQuery();
                    con.Close();
                }
                //con.Close();

            #endregion

                #region BillsDetailsModify

                //BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
                DataSet dsPatientBllDetails = new DataSet();
                dsPatientBllDetails = BillModifyDetails.GetPatientLabOLdBillsDetails(HospitalID, LocationID, Convert.ToInt32(bill.BillNo));

                foreach (DataRow dr1 in dsPatientBllDetails.Tables[0].Rows)
                {

                    SqlCommand cmdBillModifyDetail = new SqlCommand("IUPatientBillsDetailsModify", con);
                    cmdBillModifyDetail.CommandType = CommandType.StoredProcedure;
                    cmdBillModifyDetail.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceID", dr1["ServiceID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceName", dr1["ServiceName"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@BillNo", dr1["BillNo"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@UnitID ", dr1["UnitID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ChargesType", dr1["ChargesType"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Rate", dr1["Rate"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Quantity", dr1["Quantity"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Discount", dr1["Discount"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@ReffCommission", "0.00");
                    cmdBillModifyDetail.Parameters.AddWithValue("@Commisson", "0.00");
                    cmdBillModifyDetail.Parameters.AddWithValue("@ServiceType", dr1["ServiceType"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@TotalAmount", dr1["TotalAmount"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@HideInBilling", dr1["HideInBilling"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@DoctorID", dr1["DoctorID"]);
                    cmdBillModifyDetail.Parameters.AddWithValue("@CreationID", UserID);
                    cmdBillModifyDetail.Parameters.AddWithValue("@Mode", "Add");
                    // con.Close();
                    // con.Open();
                    int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();

                    // con.Close();
                }

                #endregion

                DataSet dsGetPatientBills = (DataSet)HttpContext.Current.Session["GetPatientBillsForIPDNosection"];
                
                int i = 0;
                foreach (DataRow dr in dsGetPatientBills.Tables[0].Rows)
                {
                    SqlCommand cmd = new SqlCommand("IUPatientBills", con);
                    //SqlCommand cmd = new SqlCommand("GetStudentDetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    //  cmd.Parameters.Add(new SqlParameter("@PrintBillNo ", bill.PrintBillNo));

                    //cmd.Parameters.AddWithValue("@BillNo", 0);
                    //cmd.Parameters["@BillNo"].Direction = ParameterDirection.Output;

                    //cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(bill.BillDate)));
                    cmd.Parameters.Add(new SqlParameter("@BillDate", Convert.ToDateTime(dr["BillDate"])));
                    cmd.Parameters.Add(new SqlParameter("@BillNo", dr["BillNo"]));
                    cmd.Parameters.Add(new SqlParameter("@PatientRegNO", dr["PatientRegNO"]));
                    cmd.Parameters.Add(new SqlParameter("@OPDIPDID", dr["OPDIPDID"]));
                    cmd.Parameters.Add(new SqlParameter("@BillType ", dr["BillType"]));
                    cmd.Parameters.Add(new SqlParameter("@GrossAmount", Convert.ToDouble(dr["GrossAmount"])));
                    if(dr["TaxPercent"].ToString()=="")
                    {
                        cmd.Parameters.Add(new SqlParameter("@TaxPercent", Convert.ToDouble(0)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@TaxPercent", Convert.ToDouble(dr["TaxPercent"])));
                    }
                   
                    cmd.Parameters.Add(new SqlParameter("@TaxAmount", Convert.ToDouble(dr["TaxAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@ReffCommission", Convert.ToDouble(dr["ReffCommission"])));
                    cmd.Parameters.Add(new SqlParameter("@Commisson", Convert.ToDouble(dr["Commisson"])));
                    cmd.Parameters.Add(new SqlParameter("@TotalAmount", Convert.ToDouble(dr["TotalAmount"])));
                    if(dr["NonMedicalExp"].ToString()=="")
                    {
                        cmd.Parameters.Add(new SqlParameter("@NonMedicalExp", Convert.ToDouble(0)));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@NonMedicalExp", Convert.ToDouble(dr["NonMedicalExp"])));
                    }

                   

                    if (dr["DiscountAmount"].ToString() != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(dr["DiscountPercent"])));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountPercent", Convert.ToDouble(0.00)));
                    }
                    if (dr["DiscountPercent"].ToString() != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(dr["DiscountAmount"])));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@DiscountAmount ", Convert.ToDouble(0.00)));
                    }
                   cmd.Parameters.Add(new SqlParameter("@DiscountReason", dr["DiscountReason"]));
                    cmd.Parameters.Add(new SqlParameter("@NetPayableAmount", Convert.ToDouble(dr["NetPayableAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@BalanceAmount", Convert.ToDouble(dr["BalanceAmount"])));
                    if (dr["PreBalanceAmount"].ToString() == String.Empty)
                        cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble("0.0")));
                    else
                        cmd.Parameters.Add(new SqlParameter("@PreBalanceAmount", Convert.ToDouble(dr["PreBalanceAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@IsPaid", dr["IsPaid"]));
                    //cmd.Parameters.Add(new SqlParameter("@IsBillMade", bill.IsPaid));
                    cmd.Parameters.Add(new SqlParameter("@PaidAmount", Convert.ToDouble(dr["PaidAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@DipositAmount ", Convert.ToDouble(dr["DipositAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@PaymentType ", dr["PaymentType"]));
                    cmd.Parameters.Add(new SqlParameter("@Number", dr["PaymentType"]));
                    cmd.Parameters.Add(new SqlParameter("@Name", dr["Name"]));
                    // cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(bill.Date)));
                    cmd.Parameters.Add(new SqlParameter("@Date ", Convert.ToDateTime(dr["Date"])));
                    cmd.Parameters.Add(new SqlParameter("@Remarks ", dr["Remarks"]));
                    cmd.Parameters.Add(new SqlParameter("@SancationAmount", Convert.ToDouble(dr["SancationAmount"])));
                    cmd.Parameters.Add(new SqlParameter("@FinancialYearID   ", dr["FinancialYearID"]));
                    cmd.Parameters.Add(new SqlParameter("@CreationID", bill.CreationID));

                    cmd.Parameters.Add(new SqlParameter("@Mode", "EDIT"));

                   
                    //con.Open();
                    i = cmd.ExecuteNonQuery();
                    SqlCommand update = new SqlCommand("update PatientBillsDetails set RowStatus = 2 where BillNo = '" + dr["BillNo"] + "'", con);
                    int po = update.ExecuteNonQuery();
                }

                if (i > 0)
                {
                    // int BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);
               
                    //if (bill.BillType != "IPDFinalBill")
                    //{
                    if (bill.Services != null)
                    {
                        for (int x = 0; x < bill.Services.Length; x++)
                        {
                            SqlCommand cmd1 = new SqlCommand("IUPatientBillsDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@BillNo", bill.Services[x].BillNoChangeHeder);
                            cmd1.Parameters.AddWithValue("@ServiceID", bill.Services[x].SvcID);
                            cmd1.Parameters.AddWithValue("@Servicename", bill.Services[x].SvcName);
                            cmd1.Parameters.AddWithValue("@UnitID", bill.Services[x].UnitID);
                            cmd1.Parameters.AddWithValue("@ChargesType", "General");
                            cmd1.Parameters.AddWithValue("@DoctorCharges", 0);
                            cmd1.Parameters.AddWithValue("@Rate", bill.Services[x].Rate);
                            cmd1.Parameters.AddWithValue("@Quantity", bill.Services[x].Quantity);
                            cmd1.Parameters.AddWithValue("@Discount", bill.Services[x].sevicedisAmt);
                            cmd1.Parameters.AddWithValue("@Commisson", "0.00");
                            cmd1.Parameters.AddWithValue("@ReffCommission", "0.00");
                            cmd1.Parameters.AddWithValue("@ServiceType", bill.Services[x].ServiceType);
                            cmd1.Parameters.AddWithValue("@TotalAmount", bill.Services[x].Total);
                            cmd1.Parameters.AddWithValue("@Discount_Service", bill.Services[x].Discount_Service);
                            cmd1.Parameters.AddWithValue("@DiscountServiceType", bill.Services[x].DiscountServiceType);
                            cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");

                            if (bill.Services[x].DoctorID == "undefined")
                            {
                                cmd1.Parameters.AddWithValue("@DoctorID", 0);
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@DoctorID", bill.Services[x].DoctorID);
                            }


                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Mode", "ADD");
                            int BillsDetails = cmd1.ExecuteNonQuery();


                            if (bill.Services[x].ServiceType == "IPDLabBills")
                            {
                                ServiceType = bill.Services[x].ServiceType;
                            }
                        }
                        // }
                    }
                }
                //---------AddLab


                #region PatientLab

                DataSet dsLabDetail = new DataSet();
                DataSet dsLab = new DataSet();
                Laboratory.BL_PatientLabBills BL_Lab = new Laboratory.BL_PatientLabBills();
                dsLab = BL_Lab.GetPatientLab(HospitalID, LocationID, bill.BillNo);
                dsLabDetail = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, bill.BillNo);
                int LabNo = 0;
                if (ServiceType == "IPDLabBills")
                {
                    if (dsLab.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in dsLab.Tables[0].Rows)
                        {
                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", dr["LabNo"]);
                            //  cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                            cmdPatientLab.Parameters.AddWithValue("@LabType", dr["LabType"]);
                            cmdPatientLab.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Edit");
                            //     con.Close();

                            int IUPatientL = cmdPatientLab.ExecuteNonQuery();
                            LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                        }

                    }
                    else
                    {

                        SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                        cmdPatientLab.CommandType = CommandType.StoredProcedure;
                        cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                        cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                        cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                        cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", bill.OPDIPDID);

                        cmdPatientLab.Parameters.AddWithValue("@LabType", "IPDLab");

                        cmdPatientLab.Parameters.AddWithValue("@BillNo", bill.BillNo);
                        cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                        //     con.Close();
                        int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                        LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);

                    }



                #endregion



                    #region LabDetail

                    int DetailrowEdit = 0;
                    int PatientLabDetailID = 0;
                    for (int j = 0; j < bill.Services.Length; j++)
                    {

                        SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                        cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                        cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                        if (bill.Services[j].ServiceType == "IPDLabBills")
                        {
                            // if (bill.Services[j].Mode == "Add")
                            // {

                            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                            cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                            PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", bill.Services[j].SvcID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");

                            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);

                            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Add");

                            //  }

                            //else
                            //{
                            //    if (bill.Services[j].ServiceType == "OPDLabBills")
                            //    {

                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", dsLabDetail.Tables[0].Rows[DetailrowEdit]["PatientLabDetailID"].ToString());
                            //            //cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //            //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                            //            //   PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", bill.Services[j].SvcID);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");

                            //            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);
                            //            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Edit");
                            //            DetailrowEdit++;

                            //    }
                            //}

                            int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                            PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value.ToString());
                            //   PatientLabDetail.Add(obPatient.PatientLabDetailID.ToString());

                    #endregion


                            #region IUPatientLabWithParameter
                            if (bill.Services[j].Mode == "Add")
                            {
                                Laboratory.BL_PatientLabBills objlabParamiter = new Laboratory.BL_PatientLabBills();

                                DataSet ds = new DataSet();
                                int TestID2 = 0;
                                TestID2 = Convert.ToInt32(bill.Services[j].SvcID);

                                ds = objlabParamiter.GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);

                                foreach (DataRow drParameter in ds.Tables[1].Rows)
                                {

                                    DataView dv = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                                    DataSet dsOnlyParameter = new DataSet();
                                    DataTable dt = new DataTable();
                                    dt = dv.ToTable();
                                    dsOnlyParameter.Tables.Add(dt);


                                    // DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obj.TotalDay + " and DaysTo >=" + obj.TotalDay + " and Gender ='" + obj.Gender + "')", "", DataViewRowState.CurrentRows);
                                    SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                                    cmdParamiter.CommandType = CommandType.StoredProcedure;
                                    if (dv.Count != 0)
                                    {


                                        //  DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();


                                        cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                        cmdParamiter.Parameters.AddWithValue("@LabNo", LabNo);

                                        cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                        // cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                        //  int PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
                                        cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", PatientLabDetailID);
                                        cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);
                                        cmdParamiter.Parameters.AddWithValue("@ParameterID", dv[0]["ParameterID"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@NormalRangeID", dv[0]["NormalRangeID"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                        cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                        cmdParamiter.Parameters.AddWithValue("@NLH", 0);
                                        cmdParamiter.Parameters.AddWithValue("@ConvLow", dv[0]["ConvLow"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@ConvHigh", dv[0]["ConvHigh"].ToString());
                                        cmdParamiter.Parameters.AddWithValue("@Remark", 0);
                                        cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                        cmdParamiter.Parameters.AddWithValue("@CreationID", UserID);
                                        cmdParamiter.Parameters.AddWithValue("@Mode", "Add");
                                        //dsPatientLabWithParameter.Tables[0].Rows.Add(drW);


                                        cmdParamiter.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (dsLab.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsLab.Tables[0].Rows)
                        {

                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", dr["LabNo"]);
                            //  cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", bill.PatientRegNO);
                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", dr["OPDIPDID"]);
                            cmdPatientLab.Parameters.AddWithValue("@LabType", dr["LabType"]);
                            cmdPatientLab.Parameters.AddWithValue("@BillNo", dr["BillNo"]);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Edit");
                            //     con.Close();

                            int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                            LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);

                        }
                    }
                }
                            #endregion

                bill.Message = null;
                return bill;
                //End----------
            }
            catch (Exception ex)
            {
                bill.Message = ex.Message;
                return bill;
            }


        }
        #region FillDataOldBill
        public List<IPDBill> GetBillDetailByBillNo(string BillNo,string chk)
        {
            int table = 0;
            if(chk=="2")
            {
                table = 1;
            }
            List<IPDBill> ipdbilllist = new List<IPDBill>();
            if (BillNo != "")
            {
                Connect();
               
                SqlCommand cmd = new SqlCommand("GetPatientBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                cmd.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNo)));
                //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                sd.Fill(ds);
                sd.Fill(dt);
                //test.Business_Logic.Laboratory.BL_PatientLabBills BL_Lab = new test.Business_Logic.Laboratory.BL_PatientLabBills();

                Laboratory.BL_PatientLabBills BL_Lab = new Laboratory.BL_PatientLabBills();
                for (int i = 0; i < ds.Tables[table].Rows.Count; i++)
                {
                    SqlCommand cmd2=new SqlCommand();
                      string ForAuthorization = ds.Tables[table].Rows[0]["ForAuthorization"].ToString();

                    if (ForAuthorization == "1" )
                    {
                        SqlCommand cmd1 = new SqlCommand("GetPatientBillsForAuthorization", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd1.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd1.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNo)));
                        //cmd.Parameters.Add(new SqlParameter("@BillNo", 56));
                        SqlDataAdapter sd1 = new SqlDataAdapter(cmd1);
                        ds.Reset();
                        dt.Reset();
                        sd1.Fill(ds);
                        sd1.Fill(dt);

                    }





                        if (ForAuthorization == "0" || ForAuthorization=="" || ForAuthorization==null)
                    {
                        cmd2 = new SqlCommand("GetPatientBillsDetails", con);
                    }
                    else
                    {
                        if (ForAuthorization == "1")
                        {
                            cmd2 = new SqlCommand("GetPatientBillsDetailsAurthorise", con);
                        }
                        
                    }
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                    cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                    cmd2.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(BillNo)));
                    // cmd2.Parameters.Add(new SqlParameter("@BillNo", 56));

                    SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    //con.Open();
                    sd2.Fill(ds2);
                    con.Close();
                    ServiceName[] ipdbillssvc = new ServiceName[ds2.Tables[table].Rows.Count];
                    DataSet ds1 = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(BillNo));

                    for (int j = 0; j < ds2.Tables[table].Rows.Count; j++)
                    {
                        string ServiceID = ds2.Tables[table].Rows[j]["ServiceID"].ToString();
                        DataView dv1 = new DataView(ds1.Tables[0], "TestID = " + ServiceID + "", "", DataViewRowState.CurrentRows);
                        
                        DataRow dr2 = ds2.Tables[table].Rows[j];
                        if (dr2["DiscountServiceType"].ToString() == "")
                        {
                            dr2["DiscountServiceType"] = 0;
                            dr2["DiscountServiceType"] = "Rs";
                        }
                        ipdbillssvc[j] = new ServiceName();
                        ipdbillssvc[j].SvcID = Convert.ToString(dr2["ServiceID"]);
                        ipdbillssvc[j].SvcName = Convert.ToString(dr2["ServiceName"]);
                        ipdbillssvc[j].Rate = Convert.ToString(dr2["Rate"]);
                        ipdbillssvc[j].Quantity = Convert.ToString(dr2["Quantity"]);
                        ipdbillssvc[j].Total = Convert.ToString(dr2["TotalAmount"]);
                        ipdbillssvc[j].UnitID = Convert.ToString(dr2["UnitID"]);
                        ipdbillssvc[j].UnitName = "NUMBER";
                        ipdbillssvc[j].DiscountServiceType = dr2["DiscountServiceType"].ToString();
                        ipdbillssvc[j].Discount_Service = dr2["Discount_Service"].ToString();
                        ipdbillssvc[j].DoctorID = dr2["DoctorID"].ToString();
                        // ipdbillssvc[j].ServiceType = "";
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            ipdbillssvc[j].ServiceType = "IPDLabBills";
                        }
                        else
                        {
                            ipdbillssvc[j].ServiceType = "IPD";
                        }
                    }
                    DataRow dr = ds.Tables[table].Rows[i];
                    string DateTimestr = Convert.ToString(dr["BillDate"]);
                    BL_PatientOPDBill objdiscountresion = new BL_PatientOPDBill();
                    DataSet dsdisResion = objdiscountresion.GetDiscountReason("%");
                    DataView dv = new DataView();
                    string DisCountResionID = "";
                    string DisCountResion = "";
                    if (dr["DiscountReason"].ToString() != "")
                    {
                        dv = new DataView(dsdisResion.Tables[0], "DiscountReasonID=" + dr["DiscountReason"].ToString() + "", "", DataViewRowState.CurrentRows);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            DisCountResionID = dv.ToTable().Rows[0]["DiscountReasonID"].ToString();
                            DisCountResion = dv.ToTable().Rows[0]["DiscountReason"].ToString();
                        }
                    }
                        DateTime DateTime = Convert.ToDateTime(DateTimestr);
                   
                    
                   
                    ipdbilllist.Add(
                    new IPDBill
                    {
                        //  PatientId = Convert.ToInt32(dr["LocationID"]),
                        ForAuthorization= ds.Tables[table].Rows[0]["ForAuthorization"].ToString(),
                    BillDateStr = DateTime.ToString("yyyy-MM-dd"),
                        BillTimeStr = DateTime.ToString("hh:mm"),
                        GrossAmount = Convert.ToString(dr["GrossAmount"]),
                        TotalAmount = Convert.ToString(dr["TotalAmount"]),
                        DiscountPercent = Convert.ToString(dr["DiscountPercent"]),
                        DiscountAmount = Convert.ToString(dr["DiscountAmount"]),
                        DiscountReason = DisCountResion,
                        DiscountReasonID = DisCountResionID.ToString(),
                        NetPayableAmount = Convert.ToString(dr["NetPayableAmount"]),
                        TaxAmount = Convert.ToString(dr["TaxAmount"]),
                        
                        TaxPercent = Convert.ToString(dr["TaxPercent"]),

                        BalanceAmount = Convert.ToString(dr["BalanceAmount"]),
                        DipositAmount = Convert.ToString(dr["DipositAmount"]),
                        PreBalanceAmount = Convert.ToString(dr["PreBalanceAmount"]),
                        NonMedicalExp = dr["NonMedicalExp"].ToString(),
                        IsPaid = Convert.ToString(dr["IsPaid"]),
                        PaidAmount = Convert.ToString(dr["PaidAmount"]),
                        Services = ipdbillssvc,
                        Name = dr["Name"].ToString(),
                        Number = dr["Number"].ToString(),
                        Date = dr["Date"].ToString(),
                        Remarks = dr["Remarks"].ToString(),
                        PaymentType = dr["PaymentType"].ToString(),
                        PrintBillNo=dr["PrintBillNo"].ToString()
                        
                    });

                }
                
            }
            return ipdbilllist;
        }

        #endregion

        #region PatientBindFillData
        public List<Patient1> GetPatientDetailByReg(string RegNo, string RB, int PaidAmount, string IPDNo)
        {
            DataSet ds2 = new DataSet();
            bool PrivalageCard = false;
            string PrivName = "";
            BL_PatientOPDBill objPrivalage = new BL_PatientOPDBill();
            Laboratory.BL_PatientLabBills BL_Lab = new Laboratory.BL_PatientLabBills();
            Connect();
            List<Patient1> patientlist = new List<Patient1>();
            if (RegNo != "")
            {
                SqlCommand cmd = new SqlCommand("GetPatientForIPDBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                cmd.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
                cmd.Parameters.Add(new SqlParameter("@PatientOPIDNo", IPDNo));
                cmd.CommandTimeout = 500;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                con.Open();
                sd.Fill(ds);
                //sd.Fill(dt);
                HttpContext.Current.Session["PrivalageCard"] = false;
                // Prevelage Card

                DataSet dsPatientPrivilege = new System.Data.DataSet();
                if(ds.Tables[0].Rows.Count>0)
                {
                    dsPatientPrivilege = objPrivalage.GetPatientPrivilegeDiscount(HospitalID, LocationID, Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNo"]));
                    if (dsPatientPrivilege.Tables[0].Rows.Count > 0)
                    {

                        if (Convert.ToDateTime(dsPatientPrivilege.Tables[0].Rows[0]["ValidDate"].ToString()) > DateTime.Now.Date && Convert.ToBoolean(ds.Tables[0].Rows[0]["TPAStatus"]) == false)
                        {
                            PrivalageCard = true;
                            //  ucPatientOPDBills1.chkPrivilegeCard.Checked = true;
                            PrivName = dsPatientPrivilege.Tables[0].Rows[0]["CardName"].ToString();

                            HttpContext.Current.Session["PrivalageCard"] = true;
                        }
                    }
                }
                  
                //    string TPA = obj["TPA"];
               
                //-------------------
                con.Close();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    DataRow dr = ds.Tables[0].Rows[i];
                    string DateTimestr = Convert.ToString(dr["AddmissionDate"]);
                    DateTime DateTime = Convert.ToDateTime(DateTimestr);
                    if (ds.Tables[1].Rows[0][0].ToString() == "")
                    {
                        ds.Tables[1].Rows[0][0] = 0;
                    }
                    Double Prebalance = Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());
                    SqlCommand cmd2 = null;
                    if (RB == "1")
                    {
                        BL_PatientOPDBill objAurth = new BL_PatientOPDBill();
                        if (objAurth.ForAuthorizationGetMasterSetting() == false)
                        {
                            cmd2 = new SqlCommand("GetPatientOLDBillsNO", con);
                        }
                        else
                        {
                            cmd2 = new SqlCommand("GetPatientOLDBillsNOForAuthorization", con);
                            
                        }
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(dr["PatientRegNo"])));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToString(dr["PatientIPDNO"])));

                        cmd2.Parameters.Add(new SqlParameter("@BillType", "IPDBill"));

                        SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                      
                        con.Open();
                        sd2.Fill(ds2);
                        if (objAurth.ForAuthorizationGetMasterSetting() == true)
                        {
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    if (ds2.Tables[0].Rows[j]["RowStatus"].ToString() == "1" && ds2.Tables[0].Rows[j]["ForAuthorization"].ToString() == "0")
                                    {
                                        ds2.Tables[0].Rows[j].Delete();
                                    }

                                }
                            }
                            ds2.AcceptChanges();
                        }
                        con.Close();
                    }
                    else if (RB == "2")
                    {
                        cmd2 = new SqlCommand("GetPatientOLDBillsNO", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(dr["PatientRegNo"])));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToString(dr["PatientIPDNO"])));

                        cmd2.Parameters.Add(new SqlParameter("@BillType", "IPDBill"));
                        SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                        con.Open();
                        sd2.Fill(ds2);
                        con.Close();
                    }
                    else if (RB == "3")
                    {
                        BL_PatientOPDBill objAurth = new BL_PatientOPDBill();
                        if (objAurth.ForAuthorizationGetMasterSetting() == false)
                        {
                            cmd2 = new SqlCommand("GetPatientOLDBillsNO", con);
                        }
                        else
                        {
                            cmd2 = new SqlCommand("GetPatientOLDBillsNOForAuthorization", con);

                        }
                       
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd2.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(dr["PatientRegNo"])));
                        //cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToInt32(RegNo)));
                        cmd2.Parameters.Add(new SqlParameter("@PatientOPDIPDNO", Convert.ToString(dr["PatientIPDNO"])));

                        cmd2.Parameters.Add(new SqlParameter("@BillType", "IPDFinalBill"));
                        HttpContext.Current.Session["PatientIPDNoReport"] = Convert.ToString(dr["PatientIPDNO"]);

                        SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                        
                        con.Open();
                        sd2.Fill(ds2);
                        con.Close();
                    }

                    if (RB == "1")
                    {
                        //SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                        //DataSet ds2 = new DataSet();
                        //con.Open();
                        //sd2.Fill(ds2);
                        //con.Close();
                        IPDBill[] ipdbills = new IPDBill[ds2.Tables[0].Rows.Count];
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            DataRow dr2 = ds2.Tables[0].Rows[j];
                            ipdbills[j] = new IPDBill();
                            ipdbills[j].PatientRegNO = Convert.ToString(dr2["P_RegNo"]);
                            ipdbills[j].PrintBillNo = Convert.ToString(dr2["P_BillNo"]);
                            ipdbills[j].BillType = Convert.ToString(dr2["BillType"]);
                            ipdbills[j].BillNoDate = Convert.ToString(dr2["BillNo&Date"]);
                            ipdbills[j].PatientRegNO = Convert.ToString(dr2["PatientRegNo"]);
                            ipdbills[j].BillNo = Convert.ToInt32(Convert.ToString(dr2["BillNo"]));

                        }

                        patientlist.Add(
                            new Patient1
                            {
                                //  PatientId = Convert.ToInt32(dr["LocationID"]),
                                PrivalageCard = PrivalageCard,
                                PrivalageCardName = PrivName,
                                PatientName = Convert.ToString(dr["PatientName"]),
                                PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                                PatientIPDNO = Convert.ToString(dr["PatientIPDNO"]),
                                PatientType = Convert.ToString(dr["PatientType"]),
                                GuardianName = Convert.ToString(dr["GuardianName"]),
                                TPA_ID = Convert.ToString(dr["TPA_ID"]),
                                TPAName = Convert.ToString(dr["TPAName"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                ConsultantDrID = Convert.ToString(dr["ConsultantDrID"]),
                                DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                                DepartmentID = Convert.ToString(dr["DepartmentID"]),
                                DepartmentName = Convert.ToString(dr["DepartmentName"]),
                                ReferredByDoctorID = Convert.ToString(dr["ReferredByDoctorID"]),
                                DoctorPrintName1 = Convert.ToString(dr["DoctorPrintName1"]),
                                AdvanceAmount = Convert.ToString(dr["AdvanceAmount"]),
                                AddmissionDate = Convert.ToDateTime(dr["AddmissionDate"]).ToString("yyyy-MM-dd"),
                                AddmDate = DateTime.ToString("yyyy-MM-dd"),
                                AddmTime = DateTime.ToString("hh:mm"),
                                AddmissionType = Convert.ToString(dr["AddmissionType"]),
                                WardID = Convert.ToString(dr["WardID"]),
                                WardName = Convert.ToString(dr["WardName"]),
                                RoomID = Convert.ToString(dr["RoomID"]),
                                RoomName = Convert.ToString(dr["RoomName"]),
                                BedID = Convert.ToString(dr["BedID"]),
                                BedName = Convert.ToString(dr["BedNo"]),
                                Gender = Convert.ToString(dr["Gender"]),
                                Age = Convert.ToString(dr["Age"]),
                                PrintRegNO = Convert.ToString(dr["PrintRegNO"]),
                                FinancialYearID = Convert.ToString(dr["FinancialYearID"]),
                                FinancialYear = Convert.ToString(dr["FinancialYear"]),
                                PrintIPDNO = Convert.ToString(dr["PrintIPDNO"]),
                                Bill = ipdbills,
                                PreBalance = Prebalance,
                                NameOfPackege = dr["NameOfPackage"].ToString(),
                                 
                            });


                    }
                    else if (RB == "2" || RB == "3")
                    {
                        //SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                        //DataSet ds2 = new DataSet();
                        //con.Open();
                        //sd2.Fill(ds2);
                        //con.Close();
                        int tbl = 0;
                        if (RB == "3")
                        {
                            tbl = 0;
                        }
                        if (RB == "2")
                        {
                            tbl = 1;
                        }
                        IPDBill[] ipdbills = new IPDBill[ds2.Tables[tbl].Rows.Count];
                        for (int j = 0; j < ds2.Tables[tbl].Rows.Count; j++)
                        {
                            DataRow dr2 = ds2.Tables[tbl].Rows[j];
                            ipdbills[j] = new IPDBill();
                            ipdbills[j].BillNoDate = Convert.ToString(dr2["BillNo&Date"]);
                            ipdbills[j].PatientRegNO = Convert.ToString(dr2["PatientRegNo"]);
                            ipdbills[j].BillNo = Convert.ToInt32(Convert.ToString(dr2["BillNo"]));
                        }
                        SqlCommand cmd3 = new SqlCommand("GetPatientForProvisionalBills", con);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                        cmd3.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                        cmd3.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(dr["PatientRegNo"])));
                        cmd3.Parameters.Add(new SqlParameter("@PatientIPDNo", Convert.ToInt32(Convert.ToString(dr["PatientIPDNO"]))));
                        SqlDataAdapter sd3 = new SqlDataAdapter(cmd3);
                        DataSet ds3 = new DataSet();
                        con.Open();
                        sd3.Fill(ds3);
                        con.Close();
                        //  DataSet ds11 = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(ds2.Tables[0].Rows[0]["BillNo"]));
                        BL_PatientOPDBill obj = new BL_PatientOPDBill();
                        DataSet dsService = new DataSet();

                        dsService = obj.GetTest("%");
                        ServiceName[] svc = new ServiceName[ds3.Tables[0].Rows.Count];
                        for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                        {
                            DataView dv1 = new DataView(dsService.Tables[0], "ServiceID = " + ds3.Tables[0].Rows[j]["ServiceID"].ToString() + "", "", DataViewRowState.CurrentRows);
                            DataRow dr3 = ds3.Tables[0].Rows[j];
                            svc[j] = new ServiceName();
                            svc[j].SvcID = Convert.ToString(dr3["ServiceID"]);
                            svc[j].SvcName = Convert.ToString(dr3["ServiceName"]);
                            svc[j].Quantity = Convert.ToString(dr3["Quantity"]);
                            svc[j].Rate = Convert.ToString(dr3["Rate"]);
                            svc[j].Total = Convert.ToString(dr3["TotalAmount"]);
                            svc[j].DiscountServiceType = dr3["DiscountServiceType"].ToString();
                            svc[j].Discount_Service = dr3["Discount_Service"].ToString();
                            svc[j].DoctorID = dr3["DoctorID"].ToString();
                            svc[j].DoctorName = dr3["DoctorPrintName"].ToString();
                            svc[j].UnitID = dr3["UnitID"].ToString();
                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                svc[j].ServiceType = "IPDLabBills";
                            }
                            else
                            {
                                svc[j].ServiceType = "IPD";
                            }
                        }


                        patientlist.Add(
                new Patient1
                {
                    //  PatientId = Convert.ToInt32(dr["LocationID"]),
                    PatientName = Convert.ToString(dr["PatientName"]),
                    PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                    PatientIPDNO = Convert.ToString(dr["PatientIPDNO"]),
                    PatientType = Convert.ToString(dr["PatientType"]),
                    GuardianName = Convert.ToString(dr["GuardianName"]),
                    TPA_ID = Convert.ToString(dr["TPA_ID"]),
                    TPAName = Convert.ToString(dr["TPAName"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    ConsultantDrID = Convert.ToString(dr["ConsultantDrID"]),
                    DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                    DepartmentID = Convert.ToString(dr["DepartmentID"]),
                    DepartmentName = Convert.ToString(dr["DepartmentName"]),
                    ReferredByDoctorID = Convert.ToString(dr["ReferredByDoctorID"]),
                    DoctorPrintName1 = Convert.ToString(dr["DoctorPrintName1"]),
                    AdvanceAmount = Convert.ToString(dr["AdvanceAmount"]),
                    AddmissionDate = Convert.ToDateTime(dr["AddmissionDate"]).ToString("yyyy-MM-dd"),
                    AddmDate = DateTime.ToString("yyyy-MM-dd"),
                    AddmTime = DateTime.ToString("hh:mm"),
                    AddmissionType = Convert.ToString(dr["AddmissionType"]),
                    WardID = Convert.ToString(dr["WardID"]),
                    WardName = Convert.ToString(dr["WardName"]),
                    RoomID = Convert.ToString(dr["RoomID"]),
                    RoomName = Convert.ToString(dr["RoomName"]),
                    BedID = Convert.ToString(dr["BedID"]),
                    BedName = Convert.ToString(dr["BedNo"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Age = Convert.ToString(dr["Age"]),
                    PrintRegNO = Convert.ToString(dr["PrintRegNO"]),
                    FinancialYearID = Convert.ToString(dr["FinancialYearID"]),
                    FinancialYear = Convert.ToString(dr["FinancialYear"]),
                    PrintIPDNO = Convert.ToString(dr["PrintIPDNO"]),
                    Bill = ipdbills,
                    PreBalance = Prebalance,
                    Services = svc,
                    PaidAmountProvisnal = PaidAmount.ToString(),
                    NameOfPackege = dr["NameOfPackage"].ToString()
                });

                    }

                    else if (RB == "3")
                    {
                        SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
                       // DataSet ds2 = new DataSet();
                        con.Open();
                        sd2.Fill(ds2);
                        con.Close();
                        IPDBill[] ipdbills = new IPDBill[ds2.Tables[0].Rows.Count];

                        //DataSet ds1 = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(ds2.Tables[0].Rows[0]["BillNo"].ToString()));

                        DataSet ds3 = new DataSet();
                        ServiceName[] svc = new ServiceName[0];
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            DataRow dr2 = ds2.Tables[0].Rows[j];
                            ipdbills[j] = new IPDBill();
                            ipdbills[j].BillNoDate = Convert.ToString(dr2["BillNo&Date"]);
                            ipdbills[j].PatientRegNO = Convert.ToString(dr2["PatientRegNo"]);
                            ipdbills[j].BillNo = Convert.ToInt32(Convert.ToString(dr2["BillNo"]));
                            ipdbills[j].PrintBillNo = Convert.ToString(dr2["P_BillNo"]);
                            SqlCommand cmd3 = new SqlCommand("GetPatientBillsDetails", con);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
                            cmd3.Parameters.Add(new SqlParameter("@LocationID", LocationID));
                            cmd3.Parameters.Add(new SqlParameter("@BillNo", Convert.ToInt32(ds2.Tables[0].Rows[0]["BillNo"])));
                            //cmd3.Parameters.Add(new SqlParameter("@PatientIPDNo", Convert.ToInt32(Convert.ToString(dr["PatientIPDNO"]))));
                            SqlDataAdapter sd3 = new SqlDataAdapter(cmd3);

                            con.Open();
                            sd3.Fill(ds3);
                            con.Close();
                        }
                        //string aaa = ds3.Tables[0].Rows.Count.ToString();

                        if (ds3.Tables.Count != 0)
                        {

                            svc = new ServiceName[ds3.Tables[0].Rows.Count];
                            // DataSet ds11 = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(ds2.Tables[0].Rows[0]["BillNo"].ToString()));
                            BL_PatientOPDBill obj = new BL_PatientOPDBill();
                            DataSet dsService = new DataSet();
                            dsService = obj.GetTest("%");
                            for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                            {
                                string ServiceID = ds3.Tables[0].Rows[j]["ServiceName"].ToString();
                                // DataView dv1 = new DataView(dsService.Tables[0], "ServiceName = " + ServiceID + "", "", DataViewRowState.CurrentRows);
                                // DataView dv1 = new DataView(dsService.Tables[0], "ServiceName like " + ServiceID.ToString() + " %", "", DataViewRowState.CurrentRows);

                                DataRow dr3 = ds3.Tables[0].Rows[j];
                                svc[j] = new ServiceName();
                                svc[j].SvcID = Convert.ToString(dr3["ServiceID"]);
                                svc[j].SvcName = Convert.ToString(dr3["ServiceName"]);
                                svc[j].Quantity = Convert.ToString(dr3["Quantity"]);
                                svc[j].Rate = Convert.ToString(dr3["Rate"]);
                                svc[j].Total = Convert.ToString(dr3["TotalAmount"]);
                                svc[j].Doctorcharges = dr3["DoctorCharges"].ToString();
                                svc[j].DiscountServiceType = dr3["DiscountServiceType"].ToString();
                                svc[j].Discount_Service = dr3["Discount_Service"].ToString();

                                if (ds3.Tables[0].Rows[j]["ServiceType"].ToString() == "IPDLabFinalBill")
                                {
                                    svc[j].ServiceType = "IPDLabFinalBill";
                                }
                                else
                                {
                                    svc[j].ServiceType = "IPDFinalBill";
                                }
                            }

                        }
                        patientlist.Add(
                new Patient1
                {
                    //  PatientId = Convert.ToInt32(dr["LocationID"]),

                    PatientName = Convert.ToString(dr["PatientName"]),
                    PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                    PatientIPDNO = Convert.ToString(dr["PatientIPDNO"]),
                    PatientType = Convert.ToString(dr["PatientType"]),
                    GuardianName = Convert.ToString(dr["GuardianName"]),
                    TPA_ID = Convert.ToString(dr["TPA_ID"]),
                    TPAName = Convert.ToString(dr["TPAName"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    ConsultantDrID = Convert.ToString(dr["ConsultantDrID"]),
                    DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                    DepartmentID = Convert.ToString(dr["DepartmentID"]),
                    DepartmentName = Convert.ToString(dr["DepartmentName"]),
                    ReferredByDoctorID = Convert.ToString(dr["ReferredByDoctorID"]),
                    DoctorPrintName1 = Convert.ToString(dr["DoctorPrintName1"]),
                    AdvanceAmount = Convert.ToString(dr["AdvanceAmount"]),
                    AddmissionDate = Convert.ToDateTime(dr["AddmissionDate"]).ToString("yyyy-MM-dd"),
                    AddmDate = DateTime.ToString("yyyy-MM-dd"),
                    AddmTime = DateTime.ToString("hh:mm"),
                    AddmissionType = Convert.ToString(dr["AddmissionType"]),
                    WardID = Convert.ToString(dr["WardID"]),
                    WardName = Convert.ToString(dr["WardName"]),
                    RoomID = Convert.ToString(dr["RoomID"]),
                    RoomName = Convert.ToString(dr["RoomName"]),
                    BedID = Convert.ToString(dr["BedID"]),
                    BedName = Convert.ToString(dr["BedNo"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Age = Convert.ToString(dr["Age"]),
                    PrintRegNO = Convert.ToString(dr["PrintRegNO"]),
                    FinancialYearID = Convert.ToString(dr["FinancialYearID"]),
                    FinancialYear = Convert.ToString(dr["FinancialYear"]),
                    PrintIPDNO = Convert.ToString(dr["PrintIPDNO"]),
                    Bill = ipdbills,
                    PreBalance = Prebalance,
                    Services = svc
                });

                    }
                   
                    if (RB == "2")
                    {

                        //Provisnal Calculate
                        DataSet dsPatientSummary = new DataSet();
                        SqlCommand cmdGetPatientBillsWithSummary = new SqlCommand("GetPatientBillsWithSummary", con);
                        cmdGetPatientBillsWithSummary.CommandType = CommandType.StoredProcedure;
                        cmdGetPatientBillsWithSummary.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdGetPatientBillsWithSummary.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdGetPatientBillsWithSummary.Parameters.AddWithValue("@PatientRegNO", ds.Tables[0].Rows[0]["PatientRegNO"].ToString());
                        cmdGetPatientBillsWithSummary.Parameters.AddWithValue("@PatientIPDNO", ds.Tables[0].Rows[0]["PatientIPDNO"].ToString());
                        SqlDataAdapter sd2 = new SqlDataAdapter(cmdGetPatientBillsWithSummary);
                        con.Open();
                        sd2.Fill(dsPatientSummary);
                        con.Close();
                        decimal GrossAmount = 0;
                        decimal TotalAmount = 0;
                        decimal Discount = 0;
                        decimal TaxAmount = 0;
                       decimal PaidAmountPro = 0;
                        decimal NetPayableAmount = 0;
                        decimal BalanceAmount = 0;
                        DataSet dsrefund = new DataSet();
                        foreach (DataRow drPatientSummary in dsPatientSummary.Tables[0].Rows)
                        {
                            GrossAmount = GrossAmount + Convert.ToDecimal(drPatientSummary["TotalAmount"].ToString());
                        }
                        patientlist[0].GrossAmount = GrossAmount.ToString();
                       
                        dsrefund = GetPatientIPDFinalForBalance( Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNO"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["PatientIPDNO"].ToString()));
                        if(dsrefund.Tables[0].Rows[0]["PaidAmount"].ToString()=="")
                        {
                            dsrefund.Tables[0].Rows[0]["PaidAmount"] = 0;
                        }
                        patientlist[0].PaidAmountProvisnal =dsrefund.Tables[0].Rows[0]["PaidAmount"].ToString();

                        if (dsrefund.Tables[1].Rows[0]["RefundBills"].ToString() != "")
                        {
                            if (Convert.ToDecimal(dsrefund.Tables[1].Rows[0]["RefundBills"].ToString()) > 0)
                            {
                                patientlist[0].PaidAmountProvisnal = Convert.ToString((Convert.ToDecimal(dsrefund.Tables[0].Rows[0]["PaidAmount"].ToString()) - (Convert.ToDecimal(dsrefund.Tables[1].Rows[0]["RefundBills"].ToString()))));
                            }
                        }
                        if (dsrefund.Tables.Count > 2)
                        {
                            if (dsrefund.Tables[2].Rows[0]["OPDBalance"].ToString() != "")
                            {
                                patientlist[0].PaidAmountProvisnal = Convert.ToString((Convert.ToDecimal(patientlist[0].PaidAmountProvisnal) - (Convert.ToDecimal(patientlist[0].PaidAmountProvisnal = dsrefund.Tables[2].Rows[0]["OPDBalance"].ToString()))));
                            }
                        }
                        PaidAmountPro = Convert.ToDecimal(patientlist[0].PaidAmountProvisnal.ToString());
                        DataSet dsOLDBill = new DataSet();
                        dsOLDBill = GetPatientFinalBills("IPD", Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNO"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["PatientIPDNO"].ToString()));

                        foreach (DataRow drdsOLDBill in dsOLDBill.Tables[0].Rows)
                        {
                            // TotalAmount = TotalAmount + Convert.ToDecimal(dr["TotalAmount"].ToString());
                            Discount = Discount + Convert.ToDecimal(drdsOLDBill["DiscountAmount"].ToString());
                            TaxAmount = TaxAmount + Convert.ToDecimal(drdsOLDBill["TaxAmount"].ToString());
                        }
                       // patientlist[0].b

                       TotalAmount = GrossAmount + TaxAmount;
                       NetPayableAmount = TotalAmount - Discount;
                       BalanceAmount = NetPayableAmount - PaidAmount;
                        patientlist[0].PreBalance  =Convert.ToDouble(0.00);
                        patientlist[0].TaxAmount = TaxAmount.ToString();
                        patientlist[0].TotalAmount = TotalAmount;
                        patientlist[0].DiscountAmount  = Discount.ToString();
                        patientlist[0].NetPayableAmount = NetPayableAmount;
                        patientlist[0].BalanceAmount = BalanceAmount;


                    }

                    SqlCommand cmdfinal = new SqlCommand("CheckFinalBill", con);
                    cmdfinal.CommandType = CommandType.StoredProcedure;
                    cmdfinal.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdfinal.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdfinal.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
                    cmdfinal.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                    cmdfinal.Parameters.AddWithValue("@PatientRegNo", RegNo);
                    cmdfinal.Parameters.AddWithValue("@OPDIPDID", ds.Tables[0].Rows[0]["PatientIPDNO"].ToString());
                    con.Open();
                    int g = cmdfinal.ExecuteNonQuery();
                    con.Close();
                    bool flag;
                    string Table = cmdfinal.Parameters["@NameExists"].Value.ToString();
                    if (Convert.ToInt32(Table) == 1)
                    {
                        flag = true;

                        patientlist.Add(new Patient1 { flagfinal = "true" });
                        // flag = false;
                    }
                    else
                    {
                        flag = false;
                        patientlist.Add(new Patient1 { flagfinal = "false" });

                    }
                }
                /*
                foreach (DataRow dr in dt.Rows)
                {
                    string DateTimestr = Convert.ToString(dr["AddmissionDate"]);
                    DateTime DateTime = Convert.ToDateTime(DateTimestr);

                    patientlist.Add(
                    new Patient
                    {
                        //  PatientId = Convert.ToInt32(dr["LocationID"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                        PatientIPDNO = Convert.ToString(dr["PatientIPDNO"]),
                        PatientType = Convert.ToString(dr["PatientType"]),
                        GuardianName = Convert.ToString(dr["GuardianName"]),
                        TPA_ID = Convert.ToString(dr["TPA_ID"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        ConsultantDrID = Convert.ToString(dr["ConsultantDrID"]),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                        DepartmentID = Convert.ToString(dr["DepartmentID"]),
                        DepartmentName = Convert.ToString(dr["DepartmentName"]),
                        ReferredByDoctorID = Convert.ToString(dr["ReferredByDoctorID"]),
                        DoctorPrintName1 = Convert.ToString(dr["DoctorPrintName1"]),
                        AdvanceAmount = Convert.ToString(dr["AdvanceAmount"]),
                        AddmissionDate = Convert.ToString(dr["AddmissionDate"]),
                        AddmDate = DateTime.ToString("dd/MM/yyyy"),
                        AddmTime = DateTime.ToString("hh:mm:ss"),
                        AddmissionType = Convert.ToString(dr["AddmissionType"]),
                        WardID = Convert.ToString(dr["WardID"]),
                        WardName = Convert.ToString(dr["WardName"]),
                        RoomID = Convert.ToString(dr["RoomID"]),
                        RoomName = Convert.ToString(dr["RoomName"]),
                        BedID = Convert.ToString(dr["BedID"]),
                        BedName = Convert.ToString(dr["BedNo"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Age = Convert.ToString(dr["Age"]),
                        PrintRegNO = Convert.ToString(dr["PrintRegNO"]),
                        FinancialYearID = Convert.ToString(dr["FinancialYearID"]),
                        FinancialYear = Convert.ToString(dr["FinancialYear"]),
                        PrintIPDNO = Convert.ToString(dr["PrintIPDNO"])
                    });
                }
                */
            }
            return patientlist;
        }
        #endregion
        public DataSet GetPatientFinalBills( string BillType, int PatientRegNO, int IPDID)
        {
            // using (aSqlConnection sConn = new SqlConnection(_sDbConnection))
            // {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@BillType", SqlDbType.NVarChar, 15);
                param[2].Value = BillType;
                param[3] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[3].Value = PatientRegNO;
                param[4] = new SqlParameter("@OPDIPDID", SqlDbType.Int);
                param[4].Value = IPDID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsForFinalBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
            // }
        }

        public DataSet GetPatientIPDFinalForBalance(int PatientRegNo, int OPDIPD)
        {
            DataSet dsfinalRefundAmt = new DataSet();

            SqlCommand cmd1 = new SqlCommand("GetPatientIPDFinalForBalance", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd1.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd1.Parameters.Add(new SqlParameter("@PatientRegNO", Convert.ToInt32(PatientRegNo)));
            cmd1.Parameters.Add(new SqlParameter("@PatientIPDNO", Convert.ToInt32(OPDIPD)));
            cmd1.CommandTimeout = 500;
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd1);
            sd1.Fill(dsfinalRefundAmt);
            return dsfinalRefundAmt;
        }
        #region chk
        public bool CheckFinalBill(int HospitalID, int LocationID, int PatientRegNo, int OPDIPDID)
        {
            Connect();
          
            SqlParameter[] aParams = new SqlParameter[5];
            try
            {
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@NameExists", SqlDbType.Bit);
                aParams[2].Direction = ParameterDirection.Output;
                aParams[3] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[3].Value = PatientRegNo;
                aParams[4] = new SqlParameter("@OPDIPDID", SqlDbType.Int);
                aParams[4].Value = OPDIPDID;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "CheckFinalBill", aParams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return Convert.ToBoolean(aParams[2].Value);
        }

        #endregion

        #region PatientSearch
        public List<PatientSerchDetails> GetPatientName(string search)
        {
            Connect();
        
            List<PatientSerchDetails> patientnamelist = new List<PatientSerchDetails>();
            //SqlCommand cmd = new SqlCommand("GetRole", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Select GuardianName,Gender ,Address,MobileNo, PatientName,PatientRegNO from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientType = 'IPD' AND PatientName LIKE '%" + search + "%' and RowStatus=0 order by PatientRegNO", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new PatientSerchDetails
                    {
                        Address = Convert.ToString(dr["Address"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        GuardianName = Convert.ToString(dr["GuardianName"]),
                        Gender = Convert.ToString(dr["Gender"]),
                       
                        PatientName = Convert.ToString(dr["PatientName"]),
                       
                     PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                      
                       
                      
                        
                    });

            }
            return patientnamelist;
        }

        #endregion

        public DataSet GetPatientBills(int BillNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[0].Value = BillNo;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientIPDBillsBedChargesAuto(int HospitalID, int LocationID, int PatientRegNO, int PatientIPDNo,string ServiceID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNo;
                param[4] = new SqlParameter("@ServiceID", SqlDbType.Int);
                param[4].Value = ServiceID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientIPDBillsBedChargesAuto", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
   
        public DataSet GetPatientIPDBillsBedCharges(int HospitalID, int LocationID, int PatientRegNO, int PatientIPDNo)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNo;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientIPDBillsBedCharges", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetWordChargesService()
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();

            SqlCommand cmd = new SqlCommand("select BillAutoOne, BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,GeneralCharges,EmergencyCharges from Services where ServiceType in ('IPD','Both') and BillCharges=1 and  BillCharges in (1) or BillAutoOne in (1) and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetWordChargesServiceAutoFill(string IPDNo,string PatientRegNo)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@OPDIPDID", SqlDbType.Int);
            param[0].Value = IPDNo;
            param[1] = new SqlParameter("@PatientRegNo", SqlDbType.NVarChar);
            param[1].Value = PatientRegNo;
            param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
            param[2].Value = HospitalID;
            param[3] = new SqlParameter("@LocationID", SqlDbType.Int);
            param[3].Value = LocationID;
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWordChargesServiceAutoFill", param);
      
            return ds;
        }
        public int SearchPrintBillNo(int BillNo, string BillType)
        {
            BL_PatientOPD obj=new BL_PatientOPD();
            Connect();
          //  string mama = obj.GetAllFinancialYear().Tables[0].Rows[0][0].ToString();   
            SqlCommand cmd = new SqlCommand();
                List<ServiceName> servicenamelist = new List<ServiceName>();
                if (BillType != "OPDBill")
                {
                     cmd = new SqlCommand("select PatientBills.PatientRegNo,PatientBills.BillNo,PatientBills.PrintBillNo ,PatientIPDDetails.PatientIPDNO,PatientIPDDetails.PrintIPDNO  from  PatientBills PatientBills inner join PatientIPDDetails on  PatientBills.[OPD/IPDID] = PatientIPDDetails.PatientIPDNO and PatientIPDDetails.PatientRegNO=PatientBills.PatientRegNo where PatientBills.PrintBillNo='" + BillNo + "' and BillType='" + BillType + "'  and PatientBills.HospitalID='" + HospitalID + "' and PatientBills.LocationID='" + LocationID + "' and PatientIPDDetails.FinancialYearID=(select FinancialYearID from MasterSetting where LocationID ='" + LocationID + "' ) and PatientBills.RowStatus=0 and PatientIPDDetails.RowStatus=0", con);
                }
            else
                {
                    cmd = new SqlCommand("select PatientBills.PatientRegNo, PatientBills.BillNo,PatientBills.PrintBillNo ,PatientOPDDetails.PatientOPDNO as 'PrintIPDNO',PatientOPDDetails.PrintOPDNo  from  PatientBills PatientBills inner join PatientOPDDetails on  PatientBills.[OPD/IPDID] = PatientOPDDetails.PatientOPDNO and PatientOPDDetails.PatientRegNO=PatientBills.PatientRegNo where PatientBills.PrintBillNo='" + BillNo + "' and BillType='" + BillType + "'  and PatientBills.HospitalID='" + HospitalID + "' and PatientBills.LocationID='" + LocationID + "' and PatientBills.FinancialYearID='" + obj.GetAllFinancialYear().Tables[1].Rows[0]["FinancialYearID"] + "' and PatientBills.RowStatus=0 and PatientOPDDetails.RowStatus=0", con);

                }
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
            int PrintIPDNO = 0;
            if(ds.Tables[0].Rows.Count>0)
            {
                if (BillType == "OPDBill")
                {
                    PrintIPDNO = Convert.ToInt32(ds.Tables[0].Rows[0]["PrintOPDNo"]);
                }
                else
                {
                    PrintIPDNO = Convert.ToInt32(ds.Tables[0].Rows[0]["PrintIPDNO"]);
                }
                
            }
            return PrintIPDNO;
        }

        public DataSet GetServicesDetailsforIPDBills(int ServiceID, int WardID)
        {
            Connect();

                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@ServiceID", SqlDbType.Int);
                    param[0].Value = ServiceID;
                    param[1] = new SqlParameter("@WardID", SqlDbType.Int);
                    param[1].Value = WardID;
                    param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[2].Value = HospitalID;
                    param[3] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[3].Value = LocationID;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetServicesDetailsforIPDBills", param);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }

        public List<OPDBill> GetPatientForIPDNoBills(string IPDNo)
        {
            if (IPDNo=="")
            {
                IPDNo = "0";
            }
            Connect();
            List<OPDBill> patientnamelist = new List<OPDBill>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[0].Value = IPDNo;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;
              
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForIPDNoBills", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    patientnamelist.Add(
                        new OPDBill
                        {
                            PatientIPDNo = Convert.ToInt32(dr["PatientIPDNO"]),
                            PatientRegNO = Convert.ToInt32(dr["PatientRegNO"])
                        });

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return patientnamelist;
        }



        public List<PreBalanceAmount> GetBalanceAmountRegistration(string PatientRegNo)
        {
            
            Connect();
            List<PreBalanceAmount> patientnamelist = new List<PreBalanceAmount>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                param[0].Value = PatientRegNo;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetBalanceAmountRegistration", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    patientnamelist.Add(
                        new PreBalanceAmount
                        {

                            PatientRegNo = dr["PrintRegNO"].ToString(),
                            OPDIPDNO = dr["OPDIPDNO"].ToString().Replace("/", "-"),
                            PrintRegNo = dr["PrintRegNO"].ToString(),
                            PatientName = dr["PatientName"].ToString(),
                            DrAmount =Convert.ToDecimal( dr["DrAmount"]),
                            CrAmount = Convert.ToDecimal(dr["DrAmount"]),
                            BalanceAmount = dr["BalanceAmount"].ToString(),
                            PrintOPDIPDNo = dr["PrintOPDNo"].ToString(),
                            Date = Convert.ToDateTime(dr["BillDate"]).ToString("dd-MM-yyyy"),
                            PatinetType = dr["PatientType"].ToString(),
                            BillType = dr["BillType"].ToString(),
                        });

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return patientnamelist;
        }
        public DataSet GetPatientServiceGroupProvisionalWise(int BillNo, string ServiceGroupID, string ReportTypeName)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@BillNo", SqlDbType.Int);
                param[2].Value = BillNo;
                param[3] = new SqlParameter("@ServiceGroupID", SqlDbType.NVarChar);
                param[3].Value= ServiceGroupID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDProvisionalBillsGroupWise", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientServiceGroup( int PatientRegNO, int PatientIPDNO, string ServiceGroupID, string ReportTypeName)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                param[3] = new SqlParameter("@PatientIPDNo", SqlDbType.Int);
                param[3].Value = PatientIPDNO;
                param[4] = new SqlParameter("@ServiceGroupID", SqlDbType.NVarChar);
                param[4].Value = ServiceGroupID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "RptPatientIPDFinalBillsGroupWise", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetIPDPatient(string GetIPDPatient)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName ,Address, MobileNo,PrintRegNO from patient where PatientName like ''+@GetIPDPatient+'%'  and PatientType = 'IPD' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            cmd.Parameters.AddWithValue("@GetIPDPatient", GetIPDPatient);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }
    }
}