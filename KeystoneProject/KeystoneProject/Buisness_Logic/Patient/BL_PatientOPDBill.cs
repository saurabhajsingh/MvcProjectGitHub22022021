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
    public class BL_PatientOPDBill
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public OPDBill Save1(OPDBill obj)
         {
            try
            {


                int BillNo = 0;
                int PatientLabDetailID = 0;
                string ServiceType = "";
                Connect();
                con.Open();
                if (obj.BillNoId == 0)
                {
                    #region Patient
                    // string regNO = GetNextPatientRegNO(obPatient).Tables[0].Rows[0]["PatientRegNo"].ToString();

                    DataSet dsRegnew = new DataSet();
                    dsRegnew = GetOPDPatient("%");
                    DataView dv1 = new DataView(dsRegnew.Tables[0], "PatientRegNO='" + obj.PatientRegNO + "'", "", DataViewRowState.CurrentRows);
                    if (dv1.ToTable().Rows.Count == 0)
                    {

                        SqlCommand cmdPatient = new SqlCommand("IUPatient", con);
                        cmdPatient.CommandType = CommandType.StoredProcedure;
                        cmdPatient.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatient.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdPatient.Parameters.AddWithValue("@PatientRegNO", 0);
                        cmdPatient.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;
                        cmdPatient.Parameters.AddWithValue("@PatientType", "OPD");
                        cmdPatient.Parameters.AddWithValue("@PatientName", obj.PatientName);
                        cmdPatient.Parameters.AddWithValue("@PFPatientName", obj.PatientName);
                        cmdPatient.Parameters.AddWithValue("@PFirstName", "");
                        cmdPatient.Parameters.AddWithValue("@PMiddleName", "");
                        cmdPatient.Parameters.AddWithValue("@PLastName", "");
                        cmdPatient.Parameters.AddWithValue("@ReferenceCode", 0);
                        cmdPatient.Parameters.AddWithValue("@PFGuardianName", "");
                        cmdPatient.Parameters.AddWithValue("@GuardianName", obj.GuardianName);
                        cmdPatient.Parameters.AddWithValue("@Gender", obj.Gender);
                        cmdPatient.Parameters.AddWithValue("@Age", obj.Year);
                        cmdPatient.Parameters.AddWithValue("@AgeType", "Years");
                        cmdPatient.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(obj.DateOfBirth));
                        cmdPatient.Parameters.AddWithValue("@Height", "");
                        cmdPatient.Parameters.AddWithValue("@BloodGroup", "");
                        cmdPatient.Parameters.AddWithValue("@Address", obj.Address);
                        cmdPatient.Parameters.AddWithValue("@CityID", "");
                        cmdPatient.Parameters.AddWithValue("@PinCode", "");
                        cmdPatient.Parameters.AddWithValue("@StateID", "");
                        cmdPatient.Parameters.AddWithValue("@Photo", "");
                        cmdPatient.Parameters.AddWithValue("@CountryID", "");
                        cmdPatient.Parameters.AddWithValue("@PhoneNo", "");
                        cmdPatient.Parameters.AddWithValue("@MobileNo", "");
                        cmdPatient.Parameters.AddWithValue("@EmailID", "");
                        cmdPatient.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                        cmdPatient.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatient.Parameters.AddWithValue("@Mode", "Add");


                      //  cmdPatient.ExecuteNonQuery().ToString();
                        obj.PatientRegNO = Convert.ToInt32(cmdPatient.Parameters["@PatientRegNO"].Value);

                    }
                    #endregion



                    #region Add Patient OPD Details

                    SqlCommand cmdPatientOPDDetail = new SqlCommand("IUPatientOPDDetails", con);
                    cmdPatientOPDDetail.CommandType = CommandType.StoredProcedure;
                    cmdPatientOPDDetail.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);

                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientOPDNO", obj.PatientOPDNo);
                    cmdPatientOPDDetail.Parameters["@PatientOPDNO"].Direction = ParameterDirection.Output;
                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientRegistrationDate", Convert.ToDateTime(obj.BillDate));// ucPatientOPD1.dtpRegDate.Text + " " + ucPatientOPD1.dtpTime.Text;

                    // cmdPatientOPDDetail.Parameters.Add(new SqlParameter("@PatientRegistrationDate", DateTime.ParseExact(obj.PatientRegistrationDate, "dd/MM/yyyy hh:mm:ss", null)));
                    //cmdPatientOPDDetail.Parameters.Add(new SqlParameter("@PatientRegistrationDate", DateTime.ParseExact(obj.PatientRegistrationDate, "dd/MM/yyyy", null)));


                    //cmdPatientOPDDetail.Parameters.Add(new SqlParameter("@PatientRegistrationDate", DateTime.ParseExact(obj.PatientRegistrationDate+" 00:00:00", "yyyy-MM-dd hh:mm:ss", null)));
                    //cmdPatientOPDDetail.Parameters.Add(new SqlParameter("@PatientRegistrationDate", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));

                    cmdPatientOPDDetail.Parameters.AddWithValue("@PatientType", "OPD");//"OPD" ;
                    if (obj.TPA_ID != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", obj.TPA_ID);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@TPA_ID", "");
                    }

                    if (obj.DoctorID != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@DoctorID", obj.DoctorID);

                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@DoctorID", "");
                    }
                    if (obj.DepartmentID != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@DepartmentID", "");
                    }
                    if (obj.ReferredByDoctorID != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@ReferredByDoctorID", obj.ReferredByDoctorID);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@ReferredByDoctorID", "");
                    }
                    cmdPatientOPDDetail.Parameters.AddWithValue("@Weight", "");
                    if (obj.BloodPressure != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@BloodPressure", obj.BloodPressure);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@BloodPressure", "");

                    }
                    if (obj.Reason != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@Reason", obj.Reason);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@Reason", "");
                    }
                    cmdPatientOPDDetail.Parameters.AddWithValue("@RegistrationCharges", "00.0");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@ConsultionCharges", "00.0");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountInPer", "00.0");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountInRS", "00.0");
                    cmdPatientOPDDetail.Parameters.AddWithValue("@TotalAmount", "00.0");
                    if (obj.PaymentType != null)
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    }
                    else
                    {
                        cmdPatientOPDDetail.Parameters.AddWithValue("@PaymentType", "");
                    }


                    switch (obj.PaymentType)
                    {
                        case "Cheque":
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Date", obj.Date);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remarks);
                            break;
                        case "Debit Card":
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Date", obj.Date);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remarks);
                            break;
                        case "Credit Card":
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Date", obj.Date);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remarks);
                            break;
                        case "E-money":
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Date", obj.Date);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remarks);
                            break;
                        case "EFT":
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", obj.Number);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", obj.Name);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Date", obj.Date);
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", obj.Remarks);
                            break;
                        default:
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Number", "");
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Name", "");
                            cmdPatientOPDDetail.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                            cmdPatientOPDDetail.Parameters.AddWithValue("@Remark", "");
                            break;
                    }
                    cmdPatientOPDDetail.Parameters.AddWithValue("@DiscountReason",obj.DiscountReason);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@CreationID", UserID);
                    cmdPatientOPDDetail.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);

                    cmdPatientOPDDetail.Parameters.AddWithValue("@Mode", "Add");


                   // int OPDvalue = cmdPatientOPDDetail.ExecuteNonQuery();

                    obj.PatientOPDNo = Convert.ToInt32(cmdPatientOPDDetail.Parameters["@PatientOPDNO"].Value.ToString());





                    //  con.Close();


                    #endregion



                    #region IUPatientBillsOPD _PatientBill
                    SqlCommand cmd = new SqlCommand("IUPatientBills", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@BillNo", 0);
                    cmd.Parameters["@BillNo"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obj.BillDate));
                    cmd.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
                    cmd.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNo);
                    cmd.Parameters.AddWithValue("@BillType", "OPDBill");
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grosstotal);
                    cmd.Parameters.AddWithValue("@TaxPercent", obj.TaxPercent);
                    cmd.Parameters.AddWithValue("@TaxAmount", obj.SerTaxAmount);
                    cmd.Parameters.AddWithValue("@Commisson", obj.Commisson);
                    cmd.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                    cmd.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                    cmd.Parameters.AddWithValue("@DiscountPercent", obj.DiscountPercent);
                    cmd.Parameters.AddWithValue("@DiscountAmount", obj.DiscountAmount);
                    cmd.Parameters.AddWithValue("@DiscountReason", obj.DiscountReason);
                    cmd.Parameters.AddWithValue("@NetPayableAmount", obj.NetPayableAmount);
                    cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                    cmd.Parameters.AddWithValue("@PreBalanceAmount", obj.PreBalance);
                    //cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                    decimal BAmt = Convert.ToDecimal(obj.BalanceAmount);
                    if (BAmt <= 0)
                    {
                        cmd.Parameters.AddWithValue("@IsPaid", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsPaid", 1);

                    }
                    cmd.Parameters.AddWithValue("@PaidAmount", obj.PaidAmount);
                    cmd.Parameters.AddWithValue("@DipositAmount", "0.00");
                    // cmd.Parameters.AddWithValue("@IsBillMade", 0);
                    cmd.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    cmd.Parameters.AddWithValue("@Number", obj.Number);
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.Date));
                    cmd.Parameters.AddWithValue("@Remarks", obj.Remarks);
                    cmd.Parameters.AddWithValue("@SancationAmount", obj.SancationAmount);
                    cmd.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@ForAuthorization", obj.ForAuthorization);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                    int i = cmd.ExecuteNonQuery();

                    #endregion BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);


                    BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);

                    #region IUPatientBillsDetails
                    if (i > 0)
                    {
                        for (int j = 0; j < obj.Services.Length; j++)
                        {
                            SqlCommand cmd1 = new SqlCommand("[IUPatientBillsDetails]", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@BillNo", BillNo);
                            cmd1.Parameters.AddWithValue("@ServiceID", obj.Services[j].SvcID);
                            cmd1.Parameters.AddWithValue("@Servicename", obj.Services[j].SvcName);
                            cmd1.Parameters.AddWithValue("@UnitID", obj.UnitID);
                            cmd1.Parameters.AddWithValue("@ChargesType", obj.ChargesType);
                            cmd1.Parameters.AddWithValue("@Rate", obj.Services[j].Rate);
                            cmd1.Parameters.AddWithValue("@Quantity", obj.Services[j].Quantity);
                            cmd1.Parameters.AddWithValue("@Discount_Service", obj.Services[j].Discount_Service);
                            cmd1.Parameters.AddWithValue("@DiscountServiceType", obj.Services[j].DiscountServiceType);


                            cmd1.Parameters.AddWithValue("@Discount", obj.Services[j].sevicedisAmt);
                            cmd1.Parameters.AddWithValue("@Commisson", obj.Commisson);
                            cmd1.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);

                            cmd1.Parameters.AddWithValue("@DoctorCharges", "00");
                            cmd1.Parameters.AddWithValue("@ServiceType", obj.Services[j].ServiceType);
                            cmd1.Parameters.AddWithValue("@TotalAmount", obj.Services[j].Total);
                            cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");


                            //cmd1.Parameters.AddWithValue("@DoctorID", obj.Services[j].DoctorID);


                            if (obj.Services[j].DoctorID.ToString() == "undefined" || obj.Services[j].DoctorID.ToString() == "0")
                            {
                                cmd1.Parameters.AddWithValue("@DoctorID", obj.Services[j].DoctorID);
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@DoctorID", obj.Doctor);
                            }

                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@ForAuthorization", obj.ForAuthorization);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            int BillsDetails = cmd1.ExecuteNonQuery();

                            if (obj.Services[j].ServiceType == "OPDLabBills")
                            {
                                ServiceType = obj.Services[j].ServiceType;
                            }


                        }

                    }
                    #endregion
                    //------------------LAB



                    #region LabDataSaveAllTable
                    #region PatientLab


                    if (ServiceType == "OPDLabBills")
                    {
                        SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                        cmdPatientLab.CommandType = CommandType.StoredProcedure;
                        cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                        cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                        cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNO);
                        cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNo);
                        cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);

                        if (obj.PatientType == "OPD")
                        {
                            cmdPatientLab.Parameters.AddWithValue("@LabType", "OPDLab");

                        }
                        else
                        {
                            cmdPatientLab.Parameters.AddWithValue("@LabType", "OPDLab");

                        }
                        cmdPatientLab.Parameters.AddWithValue("@BillNo", BillNo);
                        cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                        //     con.Close();

                        int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                        int LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                    #endregion



                        for (int j = 0; j < obj.Services.Length; j++)
                        {
                            if (obj.Services[j].ServiceType == "OPDLabBills")
                            {
                                if (LabNo > 0)
                                {


                                    SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                                    cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                                    cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                                    //cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                                    //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                                    PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                                    cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                                    cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                                    cmdPatientLabDetails.Parameters.AddWithValue("@TestID", obj.Services[j].SvcID);

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



                                //--------------------------------------------------

                                //  test.Business_Logic.Laboratory.BL_PatientLabBills objlabParamiter = new Laboratory.BL_PatientLabBills();


                                KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills objlabParamiter = new KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills();



                                DataSet ds = new DataSet();
                                int TestID2 = 0;


                                TestID2 = Convert.ToInt32(obj.Services[j].SvcID);

                                ds = objlabParamiter.GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);

                                foreach (DataRow drParameter in ds.Tables[1].Rows)
                                {

                                    DataView dvTest = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                                    DataSet dsOnlyParameter = new DataSet();
                                    DataTable dt = new DataTable();
                                    dt = dvTest.ToTable();
                                    dsOnlyParameter.Tables.Add(dt);


                                    DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obj.TotalDay + " and DaysTo >=" + obj.TotalDay + " and Gender ='" + obj.Gender + "')", "", DataViewRowState.CurrentRows);
                                    SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                                    cmdParamiter.CommandType = CommandType.StoredProcedure;
                                    if (dv.Count != 0)
                                    {

                                        if ((obj.Gender.Trim() == dv[0]["Gender"].ToString().Trim()))
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

                        //--------------------------------------------end
                    #endregion
                    }

                    con.Close();

                    # region Bill Print

                    //int HospitalID = 1;
                    //int LocationID = 1;
                    //int BillNO = BillNo;
                    //BL_PatientOPDBill obj_RptPatientOPDBills = new BL_PatientOPDBill();
                    //obj_RptPatientOPDBills.GetRptPatientOPDBills(HospitalID, LocationID, BillNO, "SaveView");


                    //decimal PaidAmt = Convert.ToDecimal(obj.PaidAmount);
                    //if (PaidAmt > 0)
                    //{
                    //    BL_PatientOPDBill objMoneyReceipt = new BL_PatientOPDBill();
                    //    objMoneyReceipt.RptMoneyReceipt(HospitalID, LocationID, BillNo, "SaveView");
                    //   }   

                    # endregion

                    obj.BillNo = BillNo;
                    obj.MessageError = "Done";
                    return obj;
                }
               
                else
                {
                    con.Close();
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

                    con.Open();

                    if (dsAuthorizationRights.Tables[0].Rows.Count>0)
                    { 
                    string chkAurthoriseUserWise= dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();
                    if(chkAurthoriseUserWise=="True")
                    {
                        dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                        obj.ForAuthorization = "0";
                    }
                    }
                      if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"])==true)
                     {
                        obj.ForAuthorization = "1";
                       
                        SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                        cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                        cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                       // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                        cmdIUForAuthorization.Parameters.AddWithValue("@UserID",UserID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", obj.txtinput);
                        cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", obj.BillNoId);
                        cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "OPDBill");
                        cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
                        cmdIUForAuthorization.Parameters.AddWithValue("@CreationID",UserID);
                        cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                        
                        cmdIUForAuthorization.ExecuteNonQuery();



                    }
                    #endregion
                    //end
                    BillNo = obj.BillNo;
                    #region BillsModify
                    KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills BillModifyDetails = new KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills();

                    // test.Business_Logic.Laboratory.BL_PatientLabBills BillModifyDetails = new Laboratory.BL_PatientLabBills();
                    DataSet dsPatientOldBills = new DataSet();

                    dsPatientOldBills = BillModifyDetails.GetPatientOldLabBills(Convert.ToInt32(HospitalID), Convert.ToInt32(LocationID), Convert.ToInt32(obj.BillNoId));
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
                        cmdBillModify.Parameters.AddWithValue("@PrintBillNo", dr["PrintBillNo"]);
                        cmdBillModify.Parameters.AddWithValue("@FinancialYearID", dr["FinancialYearID"]);
                        cmdBillModify.Parameters.AddWithValue("@BillDate", dr["BillDate"]);
                        cmdBillModify.Parameters.AddWithValue("@ForAuthorization", obj.ForAuthorization);
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
                    dsPatientBllDetails = BillModifyDetails.GetPatientLabOLdBillsDetails(HospitalID, LocationID, Convert.ToInt32(obj.BillNoId));
                    SqlCommand updateauth = new SqlCommand("update PatientBillsDetailsModify set RowStatus = 2 where BillNo = '" + obj.BillNoId + "' and RowStatus = 0", con);
                    int cha=   updateauth.ExecuteNonQuery();
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

                        cmdBillModifyDetail.Parameters.AddWithValue("@DoctorCharges", dr1["DoctorCharges"]);
                        cmdBillModifyDetail.Parameters.AddWithValue("@Discount_Service", dr1["Discount_Service"]);
                        cmdBillModifyDetail.Parameters.AddWithValue("@DiscountServiceType", dr1["DiscountServiceType"]);
                        cmdBillModifyDetail.Parameters.AddWithValue("@ForAuthorization", 1);


                        cmdBillModifyDetail.Parameters.AddWithValue("@CreationID", UserID);
                        cmdBillModifyDetail.Parameters.AddWithValue("@Mode", "Add");
                        
                        int IUPatientBillsModifyDetail = cmdBillModifyDetail.ExecuteNonQuery();

                        
                    }

                    #endregion
                    //-------------------------------------------------------------

                    SqlCommand cmd = new SqlCommand("[IUPatientBillsOPD]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
                    cmd.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNo);
                    cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obj.BillDate));
                    cmd.Parameters.AddWithValue("@BillType", "OPDBill");
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grosstotal);
                    cmd.Parameters.AddWithValue("@TaxAmount", obj.SerTaxAmount);
                    cmd.Parameters.AddWithValue("@Commisson", obj.Commisson);
                    cmd.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                    cmd.Parameters.AddWithValue("@TaxPercent", obj.TaxPercent);
                    cmd.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                    cmd.Parameters.AddWithValue("@DiscountPercent", obj.DiscountPercent);
                    cmd.Parameters.AddWithValue("@DiscountAmount", obj.DiscountAmount);
                    cmd.Parameters.AddWithValue("@DiscountReason", obj.DiscountReason);
                    cmd.Parameters.AddWithValue("@NetPayableAmount", obj.NetPayableAmount);
                    cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                    cmd.Parameters.AddWithValue("@PreBalanceAmount", obj.PreBalance);
                    decimal BAmt = Convert.ToDecimal(obj.BalanceAmount);
                    if (BAmt <= 0) // ucPatientOPDBills1.txtBalAmount.Text.Trim()) <= 0)
                    {
                        cmd.Parameters.AddWithValue("@IsPaid", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsPaid", 1);

                    }
                    cmd.Parameters.AddWithValue("@PaidAmount", obj.PaidAmount);
                    cmd.Parameters.AddWithValue("@DipositAmount", "0.00");
                    cmd.Parameters.AddWithValue("@IsBillMade", 0);
                    cmd.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    cmd.Parameters.AddWithValue("@Number", obj.Number);
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.Date));
                    cmd.Parameters.AddWithValue("@Remarks", obj.Remarks);
                    cmd.Parameters.AddWithValue("@SancationAmount", obj.SancationAmount);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@BillNo", obj.BillNoId);
                    cmd.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                    int i = cmd.ExecuteNonQuery();

                    BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);
                    SqlCommand update = new SqlCommand("update PatientBillsDetails set RowStatus = 2 where BillNo = '" + BillNo + "' and RowStatus=0", con);
                    update.ExecuteNonQuery();

                    #region IUPatientBillsOPD _PatientBill

                    SqlCommand cmdPatientBills = new SqlCommand("IUPatientBills", con);
                    cmdPatientBills.CommandType = CommandType.StoredProcedure;
                    cmdPatientBills.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdPatientBills.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdPatientBills.Parameters.AddWithValue("@BillNo", BillNo);
                    //cmdPatientBills.Parameters["@BillNo"].Direction = ParameterDirection.Output;
                    cmdPatientBills.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obj.BillDate));
                    cmdPatientBills.Parameters.AddWithValue("@PatientRegNO", obj.PatientRegNO);
                    cmdPatientBills.Parameters.AddWithValue("@OPDIPDID", obj.PatientOPDNo);
                    cmdPatientBills.Parameters.AddWithValue("@BillType", "OPDBill");
                    cmdPatientBills.Parameters.AddWithValue("@GrossAmount", obj.grosstotal);
                    cmdPatientBills.Parameters.AddWithValue("@TaxPercent", obj.TaxPercent);
                    cmdPatientBills.Parameters.AddWithValue("@TaxAmount", obj.SerTaxAmount);
                    cmdPatientBills.Parameters.AddWithValue("@Commisson", obj.Commisson);
                    cmdPatientBills.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                    cmdPatientBills.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                    cmdPatientBills.Parameters.AddWithValue("@DiscountPercent", obj.DiscountPercent);
                    cmdPatientBills.Parameters.AddWithValue("@DiscountAmount", obj.DiscountAmount);
                    cmdPatientBills.Parameters.AddWithValue("@DiscountReason", obj.DiscountReason);
                    cmdPatientBills.Parameters.AddWithValue("@NetPayableAmount", obj.NetPayableAmount);
                    cmdPatientBills.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                    cmdPatientBills.Parameters.AddWithValue("@PreBalanceAmount", obj.PreBalance);
                    //cmd.Parameters.AddWithValue("@BalanceAmount", obj.BalanceAmount);
                    decimal BAmt1 = Convert.ToDecimal(obj.BalanceAmount);
                    if (BAmt1 <= 0)
                    {
                        cmdPatientBills.Parameters.AddWithValue("@IsPaid", 0);
                    }
                    else
                    {
                        cmdPatientBills.Parameters.AddWithValue("@IsPaid", 1);

                    }
                    cmdPatientBills.Parameters.AddWithValue("@PaidAmount", obj.PaidAmount);
                    cmdPatientBills.Parameters.AddWithValue("@DipositAmount", "0.00");
                    // cmd.Parameters.AddWithValue("@IsBillMade", 0);
                    cmdPatientBills.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                    cmdPatientBills.Parameters.AddWithValue("@Number", obj.Number);
                    cmdPatientBills.Parameters.AddWithValue("@Name", obj.Name);
                    cmdPatientBills.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.Date));
                    cmdPatientBills.Parameters.AddWithValue("@Remarks", obj.Remarks);
                    cmdPatientBills.Parameters.AddWithValue("@SancationAmount", obj.SancationAmount);
                    cmdPatientBills.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);
                    cmdPatientBills.Parameters.AddWithValue("@CreationID", UserID);
                    cmdPatientBills.Parameters.AddWithValue("@ForAuthorization", obj.ForAuthorization);
                    cmdPatientBills.Parameters.AddWithValue("@Mode", "Edit");
                     i = cmdPatientBills.ExecuteNonQuery();

                    #endregion





                    if (i > 0)
                    {
                        for (int j = 0; j < obj.Services.Length; j++)
                        {
                            SqlCommand cmd1 = new SqlCommand("[IUPatientBillsDetails]", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@BillNo", BillNo);
                            cmd1.Parameters.AddWithValue("@ServiceID", obj.Services[j].SvcID);
                            cmd1.Parameters.AddWithValue("@Servicename", obj.Services[j].SvcName);
                            cmd1.Parameters.AddWithValue("@UnitID", obj.UnitID);
                            cmd1.Parameters.AddWithValue("@ChargesType", obj.ChargesType);
                            cmd1.Parameters.AddWithValue("@Rate", obj.Services[j].Rate);
                            cmd1.Parameters.AddWithValue("@Quantity", obj.Services[j].Quantity);
                            cmd1.Parameters.AddWithValue("@Discount", obj.Services[j].sevicedisAmt);
                            cmd1.Parameters.AddWithValue("@Commisson", obj.Commisson);
                            cmd1.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                            cmd1.Parameters.AddWithValue("@ServiceType", obj.Services[j].ServiceType);
                            cmd1.Parameters.AddWithValue("@DoctorCharges", "");
                            cmd1.Parameters.AddWithValue("@TotalAmount", obj.Services[j].Total);
                            cmd1.Parameters.AddWithValue("@HideInBilling", "Yes");
                            cmd1.Parameters.AddWithValue("@DoctorID", obj.Services[j].DoctorID);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Discount_Service", obj.Services[j].Discount_Service);
                            cmd1.Parameters.AddWithValue("@DiscountServiceType", obj.Services[j].DiscountServiceType);
                            cmd1.Parameters.AddWithValue("@ForAuthorization", obj.Services[j].Authorization);
                            
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            int BillsDetails = cmd1.ExecuteNonQuery();

                            if (obj.Services[j].ServiceType == "OPDLabBills")
                            {
                                ServiceType = obj.Services[j].ServiceType;
                            }


                        }
                    }




                    #region PatientLab

                    DataSet dsLabDetail = new DataSet();
                    DataSet dsLab = new DataSet();

                    KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills BL_Lab = new KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills();


                    dsLab = BL_Lab.GetPatientLab(HospitalID, LocationID, BillNo);
                    dsLabDetail = BL_Lab.GetPatientLabDetails(HospitalID, LocationID, BillNo);
                    int LabNo = 0;

                    if (ServiceType == "OPDLabBills")
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
                                cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNO);
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
                        else
                        {
                            SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                            cmdPatientLab.CommandType = CommandType.StoredProcedure;
                            cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                            cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                            cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNO);
                            cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", obj.PrintOPDNo);

                            cmdPatientLab.Parameters.AddWithValue("@LabType", "OPDLab");

                            cmdPatientLab.Parameters.AddWithValue("@BillNo", BillNo);
                            cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                            //     con.Close();

                            int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                            LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                        }



                    #endregion



                        #region LabDetail


                        int row = 0;
                        for (int j = 0; j < obj.Services.Length; j++)
                        {
                            // DataSet ds = GetTest("%");
                            //  DataView dv1 = new DataView(ds.Tables[0], " ServiceID = " + obj.Services[j].SvcID + " ", "", DataViewRowState.CurrentRows);


                            SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                            cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                            cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);

                            if (obj.Services[j].ServiceType == "OPDLabBills")
                            {
                                if (obj.Services[j].Mode == "Add")
                                {
                                    cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                                    cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                                    //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                                    PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                                    cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                                    cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                                    cmdPatientLabDetails.Parameters.AddWithValue("@TestID", obj.Services[j].SvcID);
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

                                }
                                else
                                {
                                    if (obj.Services[j].ServiceType == "OPDLabBills")
                                    {
                                        cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", dsLabDetail.Tables[0].Rows[row]["PatientLabDetailID"].ToString());
                                        //cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                                        //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                                        //      PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                                        cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", LabNo);
                                        cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                                        cmdPatientLabDetails.Parameters.AddWithValue("@TestID", obj.Services[j].SvcID);
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

                                        cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Edit");
                                        row++;
                                    }
                                }
                                int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                                PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value.ToString());
                                //   PatientLabDetail.Add(obPatient.PatientLabDetailID.ToString());

                        #endregion


                                #region IUPatientLabWithParameter
                                if (obj.Services[j].Mode == "Add")
                                {
                                    KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills objlabParamiter = new KeystoneProject.Buisness_Logic.Laboratory.BL_PatientLabBills();

                                    DataSet ds = new DataSet();
                                    int TestID2 = 0;
                                    TestID2 = Convert.ToInt32(obj.Services[j].SvcID);

                                    ds = objlabParamiter.GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);

                                    foreach (DataRow drParameter in ds.Tables[1].Rows)
                                    {

                                        DataView dvTest = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                                        DataSet dsOnlyParameter = new DataSet();
                                        DataTable dt = new DataTable();
                                        dt = dvTest.ToTable();
                                        dsOnlyParameter.Tables.Add(dt);


                                        DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obj.TotalDay + " and DaysTo >=" + obj.TotalDay + " and Gender ='" + obj.Gender + "')", "", DataViewRowState.CurrentRows);
                                        SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                                        cmdParamiter.CommandType = CommandType.StoredProcedure;
                                        if (dv.Count != 0)
                                        {

                                            if ((obj.Gender.Trim() == dv[0]["Gender"].ToString().Trim()))
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

                                #endregion



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
                                cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNO);
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



                }
                con.Close();
                obj.BillNo = BillNo;
                obj.MessageError = "Done";
            }
            catch (Exception ex)
            {
                obj.MessageError = ex.Message;
            }


            return obj;
        }

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


        #region Get Data For Report in Patient OPD Bills
        //public bool GetRptPatientOPDBills(int HospitalID, int LocationID, int BillNO, string ReportTypeName)
        //{
        //    try
        //    {
        //        Connect();

        //        SqlCommand cmd = new SqlCommand("RptPatientOPDBills", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@HospitalID", 1);
        //        cmd.Parameters.AddWithValue("@LocationID", 1);
        //        cmd.Parameters.AddWithValue("@BillNo", BillNO);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        //  con.Open();
        //        da.Fill(ds);
        //        //con.Close();
        //        //return ds;
        //        if (ds.Tables[0].Rows.Count != 0)
        //        {

        //            DialogResult ObjDialogResult = MessageBox.Show("With Balance Amount", "Keystone ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            if (ObjDialogResult == DialogResult.No)
        //            {
        //                ds.Tables[0].Rows[0]["PreBalance"] = 0;

        //            }


        //            RptPatientOPDBills rptOPDBills = new RptPatientOPDBills();
        //            RptPatientPrescriptionOPD rptPatiPriscription_D1 = new RptPatientPrescriptionOPD();

        //            switch (ReportTypeName.Trim())
        //            {
        //                case "Save":

        //                    break;

        //                case "SavePrint":
        //                    rptOPDBills.SetDataSource(ds);
        //                    rptOPDBills.PrintToPrinter(1, false, 0, 0);
        //                    break;

        //                case "SaveView":
        //                    BlobFieldObject objBlobFieldObject = (BlobFieldObject)rptOPDBills.ReportDefinition.Sections[0].ReportObjects["Logo1"];
        //                    objBlobFieldObject.Height = Convert.ToInt32(ds.Tables[0].Rows[0]["LogoSize"].ToString());

        //                    //BlobFieldObject objBlobFieldObject1 = (BlobFieldObject)rptPatiPriscription_D1.ReportDefinition.Sections[0].ReportObjects["Logo1"];
        //                    //objBlobFieldObject1.Height = Convert.ToInt32(ds.Tables[0].Rows[0]["LogoSize"].ToString());

        //                    PrintPreview objPrintPreview = new PrintPreview();
        //                    rptOPDBills.SetDataSource(ds);
        //                    objPrintPreview.crvPrintPreview.ReportSource = rptOPDBills;
        //                    objPrintPreview.ShowDialog();

        //                    //PrintPreview objPrintPreviewForPres = new PrintPreview();
        //                    //rptPatiPriscription_D1.SetDataSource(ds);
        //                    //objPrintPreviewForPres.crvPrintPreview.ReportSource = rptPatiPriscription_D1;
        //                    //objPrintPreviewForPres.ShowDialog();

        //                    break;

        //                case "SaveEmail":

        //                    break;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.Publish(ex);
        //        throw ex;
        //    }
        //    return true;
        //}

        #endregion


        #region Get Data For Money Receipt Report
        //public bool RptMoneyReceipt(int HospitalID, int LocationID, int BillNo, string ReportTypeName)
        //{
        //    try
        //    {
        //        Connect();

        //        SqlCommand cmd = new SqlCommand("RptMoneyReceipt", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@HospitalID", 1);
        //        cmd.Parameters.AddWithValue("@LocationID", 1);
        //        cmd.Parameters.AddWithValue("@BillNo", BillNo);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables[1].Rows.Count != 0)
        //        {
        //            RptPreBalanceAmount rptPreBalanceAmount = new RptPreBalanceAmount();
        //            //ReportDocument rpt = new ReportDocument();
        //            switch (ReportTypeName.Trim())
        //            {
        //                case "Save":

        //                    break;

        //                case "SavePrint":
        //                    //rpt.Load("D:\\Keystone\\Reports\\keystone_Reports\\Rpt_Patients\\Reports\\RptPatientIPDBills.rpt");
        //                    rptPreBalanceAmount.SetDataSource(ds);
        //                    //crystalReportViewer1.ReportSource = crDoc; 
        //                    //rpt.PrintOptions.PrinterName = "";
        //                    rptPreBalanceAmount.PrintToPrinter(1, false, 0, 0);
        //                    //rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "D:\\Keystone\\Upload\\" + DateTime.Now.ToString() + "_" + HospitalID + "_" + LocationID + "_" + PatientRegNo);
        //                    break;
        //                case "SaveView":

        //                    BlobFieldObject objBlobFieldObject = (BlobFieldObject)rptPreBalanceAmount.ReportDefinition.Sections[0].ReportObjects["Logo3"];
        //                    objBlobFieldObject.Height = Convert.ToInt32(ds.Tables[0].Rows[0]["LogoSize"].ToString());


        //                    PrintPreview objPrintPreview = new PrintPreview();
        //                    rptPreBalanceAmount.SetDataSource(ds);
        //                    objPrintPreview.crvPrintPreview.ReportSource = rptPreBalanceAmount;
        //                    objPrintPreview.ShowDialog();

        //                    #region Report Viewer
        //                    /*crystalReportViewer1.ShowGroupTreeButton = false;
        //                      crystalReportViewer1.DisplayGroupTree = false;
        //                      crystalReportViewer1.Top = 15;
        //                      crystalReportViewer1.Left = 20;
        //                      crystalReportViewer1.Height = 670;
        //                      crystalReportViewer1.Width = 1040;
        //                      btn.Top = 20;
        //                      btn.Left = 340;
        //                      btn.Height = 23;
        //                      btn.Width = 75;
        //                      frm.MaximizeBox = false;
        //                      frm.MinimizeBox = false;
        //                      btn.Text = "Close";
        //                      frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        //                      btn.Click += new EventHandler(btn_Click);
        //                      frm.Controls.Add(this.crystalReportViewer1);
        //                      frm.Controls.Add(this.btn);
        //                      //rpt.Load("D:\\Keystone\\Reports\\keystone_Reports\\Rpt_Patients\\Reports\\RptPatientIPDBills.rpt");
        //                      rptPatientIPDBills.SetDataSource(ds);
        //                      crystalReportViewer1.ReportSource = rptPatientIPDBills;
        //                      frm.Show();
        //                      frm.Focus();
        //                      btn.BringToFront();*/
        //                    #endregion
        //                    //ReportViewer rv = new ReportViewer();
        //                    //ReportDocument rpt1 = new ReportDocument();
        //                    //rpt1.Load("D:\\Keystone\\Reports\\keystone_Reports\\Rpt_Patients\\Reports\\RptPatientOPDRegistration.rpt");
        //                    //rv.rpt1.SetDataSource(ds);
        //                    //rv.Show();
        //                    break;

        //                case "SaveEmail":

        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.Publish(ex);
        //        throw ex;
        //    }
        //    return true;
        //}
        #endregion
        public DataSet GetTest(string GetServiceGrp)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select TestID as ServiceID ,upper(TestName) as ServiceName ,HSNCode,CAST(GeneralCharges as decimal(18,2)) as GeneralCharges ,CAST(EmergencyCharges as decimal(18,2)) as EmergencyCharges ,Category as CategoryID ,(select CategoryName from Category where CategoryID =TestMaster.Category) as 'ServiceGroupName'  from TestMaster   where Category like ''+@GetServiceGrp+'%'  and  RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "  order by  ServiceName asc", con);
            cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
            //cmd.Parameters.AddWithValue("@TestID", TestID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetPatientOLDBillsNO(int PatientOPDIPDNO, int PatientRegNO, string BillType, int HospitalID, int LocationID)
        {
            Connect();
            DataSet  dsPatientBillNo = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@PatientOPDIPDNO", SqlDbType.Int);
                param[0].Value = PatientOPDIPDNO;
                param[1] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[1].Value = PatientRegNO;
                param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[2].Value = HospitalID;
                param[3] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[3].Value = LocationID;
                param[4] = new SqlParameter("@BillType", SqlDbType.NVarChar);
                param[4].Value = BillType;
                con.Open();
        
                if (ForAuthorizationGetMasterSetting() == false)
                {
                    dsPatientBillNo = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientOLDBillsNO", param);
                }
                else
                {
                     dsPatientBillNo = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientOLDBillsNOForAuthorization", param);

                    if (dsPatientBillNo.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPatientBillNo.Tables[0].Rows.Count; i++)
                        {
                            if (dsPatientBillNo.Tables[0].Rows[i]["RowStatus"].ToString() == "1" && dsPatientBillNo.Tables[0].Rows[i]["ForAuthorization"].ToString() == "0")
                            {
                                dsPatientBillNo.Tables[0].Rows[i].Delete();
                            }
                            
                        }
                        dsPatientBillNo.AcceptChanges();
                    }


                }
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return dsPatientBillNo;
        }

        public DataSet TPA(int TPA)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select Organizationname, OrganizationID from Organization where OrganizationID = '" + TPA + "'", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet Services()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select ServiceID  ,upper(Services.ServiceName) as ServiceName from Services where RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " AND ServiceType in ('OPD','Both') order by  ServiceName asc", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetServicesDetailsforOPDBills(int ServiceID)
        {
            Connect();
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@ServiceID", SqlDbType.Int);
                    param[0].Value = ServiceID;
                    param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[1].Value = HospitalID;
                    param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[2].Value = LocationID;
                    con.Open();
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetServicesDetailsforOPDBills", param);
                    con.Close();
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        }

        public DataSet GetPatientBillsForDoctorDetail(int HospitalID, int LocationID, int DoctorID)
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
                param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                param[2].Value = DoctorID;
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsForDoctorDetail", param);
                con.Close();
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
            if (IPDNo == "")
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientForOPDNoBills", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    patientnamelist.Add(
                        new OPDBill
                        {
                            PatientOPDNo = Convert.ToInt32(dr["PatientOPDNO"]),
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
        public DataSet GetPatientBills(int BillNo, int HospitalID, int LocationID)
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
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBills", param);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetPatientBillsDetails(int BillNo, int HospitalID, int LocationID)
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
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsDetails", param);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientBillsDetailsAurthorise(int BillNo, int HospitalID, int LocationID)
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
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsDetailsAurthorise", param);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetServicesDetailsforOPDBillsBasedOnTPA(int ServiceID, int OrganizationID)
        {
            {
                DataSet ds = new DataSet();
                try
                {
                    Connect();
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@ServiceID", SqlDbType.Int);
                    param[0].Value = ServiceID;
                    param[1] = new SqlParameter("@OrganizationID", SqlDbType.Int);
                    param[1].Value = OrganizationID;
                    con.Open();
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetServicesDetailsforOPDBillsBasedOnTPA", param);
                    con.Close();
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        }

        public DataSet GetServiceDetailByCode(string Code)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();

            SqlCommand cmd = new SqlCommand("select HSNCode, BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,GeneralCharges,EmergencyCharges from Services where ServiceType in ('OPD','Both') AND HSNCode LIKE '" + Code + "%'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();

            return ds;
        }
        public DataSet GetAllFinancialYear()
        {

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
        

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
           
        }


        public DataSet GetDiscountReason(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DiscountReasonID, DiscountReason from DiscountReason where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and DiscountReason like '%'+@prefix+'%' and RowStatus = 0 ", con);
            cmd.Parameters.AddWithValue("@prefix", prefix);
            con.Open();
            cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetDiscountReasonID(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DiscountReasonID, DiscountReason from DiscountReason where DiscountReasonID = '" + prefix + "' and RowStatus = 0 ", con);
            con.Open();
            cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet AddDiscountReason(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUDiscountReason", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DiscountReasonID", 0);
            cmd.Parameters.AddWithValue("@DiscountReason", prefix.ToUpper());
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "Add");
            con.Open();
            cmd.ExecuteNonQuery();
            SqlCommand cmd1 = new SqlCommand("select DiscountReasonID, DiscountReason from DiscountReason where DiscountReason = '" + prefix + "' and RowStatus = 0 ", con);
            cmd1.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetFinancialYearID(string prefix)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select FinancialYearID, FinancialYear from FinancialYear where FinancialYear = '" + prefix + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetOPDPatient(string GetOPDPatient)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName ,Address, MobileNo,PrintRegNO from patient where PatientName like ''+@GetOPDPatient+'%'  and PatientType = 'OPD' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            cmd.Parameters.AddWithValue("@GetOPDPatient", GetOPDPatient);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }
        public DataSet GetConsDoctor(string GetConsDoctor)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from  Doctor  where DoctorID =" + GetConsDoctor + "   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and DoctorType = 'Consultant'", con);
            //  cmd.Parameters.AddWithValue("@GetConsDoctor", GetConsDoctor);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetServiceGrp(string ServiceName, string ServiceGroupID, string HSNCode)
        {
            Connect();
            //if (Charges == "TPA")
            //{
            // SqlCommand cmd = new SqlCommand("select ServiceGroupID,ServiceGroupName from ServiceGroup where ServiceGroupName like ''+@GetServiceGrp+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            
            //   //SqlCommand cmd = new SqlCommand("Select GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            //}
            //else if (Charges == "Emergency")
            //{

            SqlCommand cmd = new SqlCommand("Select HSNCode, GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%'   and ServiceGroupID like '" + ServiceGroupID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
          
            //}
            //else
            //{
            //    SqlCommand cmd = new SqlCommand("Select HSNCode, GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%'   and ServiceGroupID like '" + ServiceGroupID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
               
            //}


            cmd.Parameters.AddWithValue("@GetServiceGrp", ServiceName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetServiceGrpGen(string ServiceName, string ServiceGroupID, string HSNCode)
        {
            Connect();
            //if (Charges == "TPA")
            //{
            // SqlCommand cmd = new SqlCommand("select ServiceGroupID,ServiceGroupName from ServiceGroup where ServiceGroupName like ''+@GetServiceGrp+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            //   //SqlCommand cmd = new SqlCommand("Select GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            //}
            //else if (Charges == "Emergency")
            //{

            SqlCommand cmd = new SqlCommand("Select HSNCode, GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%'   and ServiceGroupID like '" + ServiceGroupID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

            //}
            //else
            //{
            //    SqlCommand cmd = new SqlCommand("Select HSNCode, GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%'   and ServiceGroupID like '" + ServiceGroupID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

            //}


            cmd.Parameters.AddWithValue("@GetServiceGrp", ServiceName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        #region ServiceTPA
        public DataSet GetServiceDetailByNameTPA(string NameID, string Code, int OrganizationID)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();

            SqlCommand cmd = new SqlCommand("select distinct Services.TPAHSNCode, Services.ServiceID ,ServicesTPAWise.GeneralCharges,ServicesTPAWise.EmergencyCharges ,upper(ServiceName) as ServiceName,Services.ServiceGroupID ,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName  from Services left join ServicesTPAWise on ServicesTPAWise.ServiceID=Services.ServiceID and ServicesTPAWise.HospitalID=" + HospitalID + " and ServicesTPAWise.LocationID=" + LocationID + "  and ServicesTPAWise.RowStatus=0 where Services.RowStatus = 0 and ServicesTPAWise.HideInBilling='Yes' and ServicesTPAWise.OrganizationID = " + OrganizationID + "   and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + "  and Services.ServiceType in ('OPD','Both') and (Services.TPAHSNCode is null or Services.TPAHSNCode like '" + Code + "%')  order by  ServiceName asc", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion

        public DataSet GetServiceDetailByName(string NameID, string GroupID, string ServiceType)
        {
            Connect();
            SqlCommand cmd;
            List<ServiceName> servicenamelist = new List<ServiceName>();
            if (ServiceType != "Package")
            {
                if (GroupID == "%")
                {
                    cmd = new SqlCommand("select HSNCode,BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName, cast(GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges ,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName  from Services where ServiceType in ('OPD','Both') AND ServiceName like '" + NameID + "%' and  ServiceGroupID like'" + GroupID + "'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
                }
                else
                {
                    cmd = new SqlCommand("select HSNCode,BillCharges, ServiceName,ServiceID,ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName, cast(GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges ,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName  from Services where ServiceType in ('OPD','Both') AND ServiceName like '" + NameID + "%' and  ServiceGroupID='" + GroupID + "'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
                }
               
            }
            else
            {
                cmd = new SqlCommand("select distinct  BillCharges,Services. ServiceName,Services.ServiceID,Services.ServiceGroupID,UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,        cast( ServiceDetailsPackageOPD.GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( ServiceDetailsPackageOPD.EmergencyCharges as decimal(18,2))  as EmergencyCharges,(select   ServiceGroupName from ServiceGroup where ServiceGroupID = Services.ServiceGroupID) as ServiceGroupName   from ServiceDetailsPackageOPD   left  join Services on Services.ServiceID = ServiceDetailsPackageOPD.ServiceID where Services.RowStatus = 0 and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + " AND ServiceDetailsPackageOPD.ServiceOrTestName like '" + NameID + "%' ", con);

            }
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();

            return ds;
        }

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
        public DataSet GetServiceGrpTPA(string ServiceName, string ServiceGroupID, string HSNCode)
        {
            Connect();
            //if (Charges == "TPA")
            //{
            // SqlCommand cmd = new SqlCommand("select ServiceGroupID,ServiceGroupName from ServiceGroup where ServiceGroupName like ''+@GetServiceGrp+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            //   //SqlCommand cmd = new SqlCommand("Select GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            //}
            //else if (Charges == "Emergency")
            //{

            SqlCommand cmd = new SqlCommand("Select Services.HSNCode, ServicesTPAWise.GeneralCharges,ServicesTPAWise.EmergencyCharges, Services.ServiceName, Services.ServiceGroupID ,ServicesTPAWise.ServiceID from Services left join  ServicesTPAWise on ServicesTPAWise.ServiceID=Services.ServiceID where Services.ServiceName like ''+@GetServiceGrp+'%'   and services.ServiceGroupID like '" + ServiceGroupID + "%' and ServicesTPAWise.RowStatus=0 and ServicesTPAWise.HospitalID = " + HospitalID + " and ServicesTPAWise.LocationID =  " + LocationID + "", con);

            //}
            //else
            //{
            //    SqlCommand cmd = new SqlCommand("Select HSNCode, GeneralCharges,EmergencyCharges, servicename, ServiceGroupID,ServiceID from Services where ServiceName like ''+@GetServiceGrp+'%'   and ServiceGroupID like '" + ServiceGroupID + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

            //}


            cmd.Parameters.AddWithValue("@GetServiceGrp", ServiceName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public string GetServicechk(string prefix)
        {
            Connect();
            var da = new SqlDataAdapter("select ServiceGroupID,ServiceGroupName from ServiceGroup where ServiceGroupName  = '" + prefix + "' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            var dt = new DataTable();
            con.Open();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToString(true);
            }
            else
            {
                return Convert.ToString(false);
            }
#pragma warning disable CS0162 // Unreachable code detected
            con.Close();
#pragma warning restore CS0162 // Unreachable code detected
        }
        //public DataSet GetServiceGrpBind(string GetServiceGrp)
        //{
        //    Connect();
        //    SqlCommand cmd = new SqlCommand("select ServiceGroupID,ServiceGroupName,ServiceType  from ServiceGroup where ServiceGroupName like ''+@GetServiceGrp+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
        //    cmd.Parameters.AddWithValue("@GetServiceGrp", GetServiceGrp);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();
        //    return ds;
        //}
        #region ServiceTPA
        public DataSet GetServiceDetailByNameOPDTPA(string NameID, string Code, int OrganizationID)
        {
            Connect();
            List<ServiceName> servicenamelist = new List<ServiceName>();

            SqlCommand cmd = new SqlCommand("select distinct  Services.ServiceGroupID ,ServiceGroup.ServiceGroupName,ServiceGroup.HSNCode from Services left join ServicesTPAWise on ServicesTPAWise.ServiceID=Services.ServiceID left join ServiceGroup on ServiceGroup.ServiceGroupID=Services.ServiceGroupID  and ServicesTPAWise.HospitalID=" + HospitalID + " and ServicesTPAWise.LocationID=" + LocationID + "  and ServicesTPAWise.RowStatus=0 where Services.RowStatus = 0 and ServicesTPAWise.HideInBilling='Yes' and ServicesTPAWise.OrganizationID = " + OrganizationID + "   and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + "  and Services.ServiceType in ('OPD','Both') and Services.TPAHSNCode like '" + Code + "%'", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();
            return ds;
        }
        #endregion
        public List<OPDBill> GetServiceGrp(string Name,string Type)
        {
            Connect();
            List<OPDBill> servicegrouplist = new List<OPDBill>();

            SqlCommand cmd = new SqlCommand("select HSNCode, ServiceGroupID,ServiceGroupName ,ServiceType from ServiceGroup where ServiceGroupName like '%" + Name + "%' and ServiceType!='"+Type+"'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
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
                            ServiceGroupID = Convert.ToInt16(dr["ServiceGroupID"]),
                            ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
                            ServiceGroupCode = Convert.ToString(dr["HSNCode"]),
                            ServiceType = "",
                        });

                }

                // LAB Group
                SqlCommand cmd1 = new SqlCommand("select HSNCode,CategoryID ,CategoryName  from Category where CategoryName like '%" + Name + "%' and not CategoryName like 'x-ray%'   and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0", con);
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
                            ServiceGroupCode = Convert.ToString(dr["HSNCode"]),
                            ServiceGroupID = Convert.ToInt16(dr["CategoryID"]),
                            ServiceGroupName = Convert.ToString(dr["CategoryName"]) + "   ( Lab )",
                            ServiceType = "Lab",
                        });

                }


            // XRAY Group
            SqlCommand cmd2 = new SqlCommand("select HSNCode,CategoryID ,CategoryName  from Category where CategoryName like 'x-ray%'   and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0", con);
            SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            con.Open();
            sd2.Fill(dt2);
            con.Close();
            foreach (DataRow dr in dt2.Rows)
            {
                servicegrouplist.Add(
                    new OPDBill
                    {
                        ServiceGroupCode = Convert.ToString(dr["HSNCode"]),
                        ServiceGroupID = Convert.ToInt16(dr["CategoryID"]),
                        ServiceGroupName = Convert.ToString(dr["CategoryName"]) + "   ( Xray )",
                        ServiceType = "Xray",
                    });

            }


            //if (TPAID !="0")
            //{
            //    DataSet ds = GetServiceDetailByNameOPDTPA("%", "%", Convert.ToInt32(TPAID));
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        servicegrouplist.Add(
            //      new OPDBill
            //      {
            //          ServiceGroupID = Convert.ToInt16(dr["ServiceGroupID"]),
            //          ServiceGroupName = Convert.ToString(dr["ServiceGroupName"]),
            //          ServiceGroupCode = dr["HSNCode"].ToString(),
            //          ServiceType = "",
            //      });


            //    }

            //}


            return servicegrouplist;
        }
        public DataSet GetPatientLabBillsForTestMaster(int HospitalID, int LocationID, int DoctorID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                param[2].Value = 0;
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientLabBillsForTestMaster", param);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public List<OPDBill> GetServiceDetailsPackage()
        {
            Connect();
            SqlCommand cmd;

            List<OPDBill> servicelist = new List<OPDBill>();

            cmd = new SqlCommand("select distinct  HSNCode,BillCharges,Services. ServiceName,Services.ServiceID,ServiceGroupID,UnitID, Services.UnitID,(select UnitName from Unit where UnitID = Services.UnitID) as UnitName,        cast( GeneralCharges    as decimal(18,2)) as GeneralCharges ,  cast( EmergencyCharges as decimal(18,2))  as EmergencyCharges , (select ServiceGroup.ServiceGroupName from ServiceGroup where  ServiceGroup.ServiceGroupID=Services.ServiceGroupID  and ServiceGroup.HospitalID = " + HospitalID + " and ServiceGroup.LocationID = " + LocationID + " and ServiceGroup.RowStatus=0) as 'ServiceGroupName' from ServiceDetailsPackage   left  join Services on Services.ServiceID = ServiceDetailsPackage.ServiceID where Services.RowStatus = 0 and Services.HospitalID = " + HospitalID + " and Services.LocationID = " + LocationID + "", con);


            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                servicelist.Add(
                    new OPDBill
                    {
                        ServiceHSNCode = dr["HSNCode"].ToString(),
                        ServiceGroupName = dr["ServiceGroupName"].ToString(),
                        ServiceGroupID =  Convert.ToInt32( dr["ServiceGroupID"]),
                        ServiceID = dr["ServiceID"].ToString(),
                        servicename = dr["ServiceName"].ToString(),
                        UnitID =  Convert.ToInt32(dr["UnitID"]),
                        GeneralCharges = dr["GeneralCharges"].ToString(),
                        EmergencyCharges = dr["EmergencyCharges"].ToString()
                        
                    });
            }
            return servicelist;
        }

        public DataSet GetPatientForOPDBills(int PatientRegNO,int PatientOPDNO, int HospitalID, int LocationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientForOPDBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmd.Parameters.AddWithValue("@PatientOPDNO", PatientOPDNO);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetPatientPrivilegeDiscount(int HospitalID, int LocationID, int PatietnRegNo)
        {
            SqlParameter[] aParams = new SqlParameter[4];
            try
            {
                Connect();
                DataSet ds = new DataSet();
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@PatietRegNo", SqlDbType.Int);
                aParams[2].Value = PatietnRegNo;
                con.Open();
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientPrivilegeDiscount", aParams);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }

        }
        public bool DeletePatientBills(int RegNo, int BillNo,string txtinput)
        {
            Connect();
            SqlParameter[] apram = new SqlParameter[5];
            apram[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[0].Value = HospitalID;
            apram[1] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[1].Value = LocationID;
            apram[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
            apram[2].Value = RegNo;
            apram[3] = new SqlParameter("@BillNo", BillNo);
            apram[3].Value = BillNo;
            apram[4] = new SqlParameter("@CreationID", SqlDbType.Int);
            apram[4].Value = UserID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeletePatientBills", apram);
            con.Close();


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
      
            if (chkAurthoriseUserWise == "True")
            {
                dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
               
            }
                if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
                {

                    con.Close();
                    SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                    cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                    cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                    // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                    cmdIUForAuthorization.Parameters.AddWithValue("@UserID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", txtinput);
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", BillNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "OPDBill");
                    cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", RegNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                    con.Open();
                    cmdIUForAuthorization.ExecuteNonQuery();
                }


            }
            #endregion



            return true;

        }

       

        public bool ForAuthorizationGetMasterSetting()
        {
            #region ForAuthorization
            int ForAuthorization = 0;
            // Aurthorise
            bool chk = false;
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
                
                if (chkAurthoriseUserWise == "True")
                {
                    dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                    chk = false;
                }
                if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
                {

                    chk = true;

                }
            }
            return chk;
            #endregion
        }
        public DataSet GetNextPatientRegNO()
        {
            Connect();
            DataSet ds = new DataSet();

            try
            {

                SqlCommand cmd = new SqlCommand("GetNextPatientRegNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FinancialYearID", GetAllFinancialYear().Tables[1].Rows[0]["FinancialYearID"].ToString());
                cmd.Parameters.AddWithValue("@PatientRegNO", 0);
                // cmd.Parameters["@PatientRegNO".Value=ParameterDirection.Output ;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public List<OPDBill> Consultant(string GetDoctor, string DoctorID)
        {
            List<OPDBill> searchList = new List<OPDBill>();

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Select DoctorID,DoctorPrintName,DepartmentID from Doctor where Rowstatus = 0 and HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and DoctorID like  @filter+'%' and   DoctorType='Consultant'  and  DoctorPrintName  like '" + "%" + GetDoctor + "%" + "'", con);
                cmd.Parameters.AddWithValue("@filter", DoctorID);

                SqlDataAdapter ad = new SqlDataAdapter();
                DataTable dt = new DataTable();
                ad.SelectCommand = cmd;
                con.Open();
                ad.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    searchList.Add(new OPDBill
                    {
                        DoctorPrintName = dr["DoctorPrintName"].ToString(),
                        DepartmentID = Convert.ToInt32(dr["DepartmentID"]),
                        DoctorID = Convert.ToInt32(dr["DoctorID"])
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return searchList;

        }
        #region Department
        public DataSet Department(string DepartmentName)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("select  DepartmentID,DepartmentName from  Department where HospitalID='" + HospitalID + "' and LocationID ='" + LocationID + "'  and   DepartmentName  like  @filter+'%' and RowStatus=0  order by  DepartmentName asc", con);
                cmd.Parameters.AddWithValue("@filter", DepartmentName);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        #endregion

        public DataSet GetReferred_Dr(string DoctorPrintName, string DoctorID)
        {
            DataSet ds = new DataSet();

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("Select DoctorID,DoctorPrintName from Doctor where Rowstatus = 0 and DoctorType='Referred' and DoctorID like '" + DoctorID + "%" + "' AND LocationID='" + LocationID + "' and DoctorPrintName  like '" + "%" + DoctorPrintName + "%" + "'", con);
                // cmd.Parameters.AddWithValue("@filter", DoctorPrintName);

                // SqlCommand cmd = new SqlCommand("Select DoctorID,DoctorPrintName from Doctor where DoctorType='Referred' and  DoctorID like '" + DoctorID + "%" + "' and  DoctorPrintName  like '" + "Dr. " + DoctorPrintName + "%" + "'", con);
                //cmd.Parameters.AddWithValue("@GetDoctor", DoctorPrintName);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception EX)
            {

            }
            return ds;
        }

      
    }


}