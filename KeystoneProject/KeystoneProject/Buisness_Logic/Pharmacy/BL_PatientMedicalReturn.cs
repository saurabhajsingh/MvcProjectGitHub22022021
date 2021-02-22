using KeystoneProject.Models.Pharmacy;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.Pharmacy
{
    public class BL_PatientMedicalReturn
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet BindProductName(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select ProductID,ProductName,ProductGroup.ProductGroupName,Packing,Contain,ProductLowerUnitName,SellLoose,GenericInformation.GenericName ,MfrName,MaxQtyLevel,Scheduled,CategoryID,BarCode,DiscontinueDate,ExtraField,DetailWithExtraField from Product left join ProductGroup on ProductGroup.ProductGroupID = Product.ProductGroupID  left join GenericInformation on GenericInformation.GenericID  = Product.GenericID  where Product.HospitalID =" + HospitalID + " and  Product.LocationID =" + LocationID + "  and Product.RowStatus=0 and ProductName like '" + Name + "%' order by Product.ProductName", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindCustomer(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select  CustomerID,CustomerName ,CustomerPrintName ,Gender ,MobileNo,Address  from  CanteenCustomer  where CustomerType='Medical' and CustomerName like '" + Name + "%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "   order by  CustomerName  asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindSalesPerson(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select SalesPersonName,SalesPersonID from SalesPerson where SalesPersonName like '" + Name + "%' and SalesPerson.HospitalID = " + HospitalID + " and SalesPerson.LocationID = " + LocationID + " and SalesPerson.RowStatus= 0  ORDER BY SalesPerson.SalesPersonName asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindDoctor(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from Doctor where RowStatus = 0 and DoctorPrintName like '" + Name + "%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "  order by  DoctorPrintName asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindPatient(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO ,Patient.PatientName, GuardianName ,Gender ,DateOfBirth, Patient.PatientType ,BloodGroup from Patient  where  Patient.HospitalID = " + HospitalID + " and Patient.LocationID = " + LocationID + " and PatientName like '" + Name + "%' and Patient.RowStatus= 0  ORDER BY Patient.PatientName asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet BindRegno(string patnm)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO from Patient where PatientName = @Pname and Patient.HospitalID = " + HospitalID + " and Patient.LocationID = " + LocationID + " and Patient.RowStatus= 0  ORDER BY Patient.PatientRegNO Desc", con);
            cmd.Parameters.AddWithValue("@Pname", patnm);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable Bind_patDetail(int regno)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientMedicalBills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", regno);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable GetOldBill(int regno)
        {
            DataTable dt1 = Bind_patDetail(regno);
            DataTable dt = new DataTable();
            if (dt1.Rows.Count > 0)
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetPatientOLDMedicalBill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", regno);
                cmd.Parameters.AddWithValue("@BillType", dt1.Rows[0]["PatientType"].ToString() + "MedicalBill");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public DataSet BindCustID(string Name)
        {
            DataSet ds = new DataSet();

            Connect();
            SqlCommand cmd = new SqlCommand("select CustomerID from CanteenCustomer where CustomerType='Medical' and CustomerName = '" + Name + "' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "   order by  CustomerName  asc", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable Bind_CustDetail(int CustomerID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetCanteenCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataSet GetMedicalBillForProductStock(int ProductID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ProductID", SqlDbType.Int);
                param[0].Value = ProductID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetMedicalBillForProductStock", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetServicesDetailsforMedicalBills(int ProductDetailsID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID ", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID ", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@ProductDetailsID  ", SqlDbType.Int);
                param[2].Value = ProductDetailsID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientMedicalBillsForProduct", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }

        public bool Save(PatientMedicalReturn obj)
        {
            try
            {
                int regno = Convert.ToInt32(obj.patientRegNo);
                DataTable dt = Bind_patDetail(regno);

                Connect();
                SqlCommand cmd = new SqlCommand("IUPatientBillsMedicalReturn", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.medicalReturnID == 0)
                {
                    cmd.Parameters.AddWithValue("@MedicalBillID", 0);
                    cmd.Parameters["@MedicalBillID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MedicalBillID", obj.medicalReturnID);
                    cmd.Parameters.AddWithValue("@Mode", "EDIT");
                }

                if (obj.oldBillNumber == null || obj.oldBillNumber == "" || obj.oldBillNumber == "--New--")
                {
                    cmd.Parameters.AddWithValue("@BillNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillNo", obj.oldBillNumber);
                }

                if (obj.referenceDate == null || obj.referenceDate == "")
                {
                    cmd.Parameters.AddWithValue("@BillDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillDate", obj.referenceDate);
                }

                if (obj.patientRegNo == 0)
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", 0);
                    cmd.Parameters.AddWithValue("@BillType", "GeneralMedicalBill");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", obj.patientRegNo);
                    cmd.Parameters.AddWithValue("@BillType", dt.Rows[0]["PatientType"].ToString() + "MedicalBill");
                }

                if (obj.patientName == null)
                {
                    cmd.Parameters.AddWithValue("@PatientName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientName", obj.patientName);
                }

                if (obj.address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", obj.address);
                }

                if (obj.remark == null)
                {
                    cmd.Parameters.AddWithValue("@Remarks", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remarks", obj.remark);
                }

                if (obj.doctorID == 0)
                {
                    cmd.Parameters.AddWithValue("@DoctorID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorID", obj.doctorID);
                }

                if (obj.doctorName == null)
                {
                    cmd.Parameters.AddWithValue("@DoctorName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorName", obj.doctorName);
                }

                if (obj.salesPersonID == 0)
                {
                    cmd.Parameters.AddWithValue("@SalesPersonID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SalesPersonID", obj.salesPersonID);
                }

                if (obj.salesPerson == null)
                {
                    cmd.Parameters.AddWithValue("@SalesPersonName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SalesPersonName", obj.salesPerson);
                }

                cmd.Parameters.AddWithValue("@PaymentType", "Cash");
                cmd.Parameters.AddWithValue("@Number", "Cash");
                cmd.Parameters.AddWithValue("@Name", "Cash");
                cmd.Parameters.AddWithValue("@ChequeDate", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@Other", "");
                cmd.Parameters.AddWithValue("@Message", "Cash");

                if (obj.grossTotal == 0)
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grossTotal);
                }

                if (obj.discountAmount == 0)
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", obj.discountAmount);
                }

                if (obj.vatAmount == 0)
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", obj.vatAmount);
                }

                cmd.Parameters.AddWithValue("@SalesReturn", 0);

                if (obj.totalAmount == 0)
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", obj.totalAmount);
                }

                if (obj.discountPercentage == 0)
                {
                    cmd.Parameters.AddWithValue("@DiscountPercent", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DiscountPercent", obj.discountPercentage);
                }

                if (obj.otherLess == 0)
                {
                    cmd.Parameters.AddWithValue("@OtherLess", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OtherLess", obj.otherLess);
                }

                if (obj.netAmount == 0)
                {
                    cmd.Parameters.AddWithValue("@NetAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NetAmount", obj.netAmount);
                }

                cmd.Parameters.AddWithValue("@CashReceived", 0);

                if (obj.postingAccount == null || obj.postingAccount == "")
                {
                    cmd.Parameters.AddWithValue("@PostingAccount", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PostingAccount", obj.postingAccount);
                }

                if (obj.referenceNumber == 0)
                {
                    cmd.Parameters.AddWithValue("@RefNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RefNo", obj.referenceNumber);
                }

                if (obj.referenceDate == null || obj.referenceDate == "")
                {
                    cmd.Parameters.AddWithValue("@RefDate", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RefDate", obj.referenceDate);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                obj.medicalReturnID = Convert.ToInt32(cmd.Parameters["@MedicalBillID"].Value);
                con.Close();

                if (i > 0)
                {
                    string[] prdctID = obj.tblProductID.Split(',');
                    string[] prdctNm = obj.tblProductName.Split(',');

                    for (int k = 0; k < prdctNm.Length; k++)
                    {
                        if (prdctNm[k].Trim() != "")
                        {
                            SqlCommand cmd1 = new SqlCommand("IUPatientBillsDetailsMedicalReturn", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@Mode", "ADD");
                            cmd1.Parameters.AddWithValue("@PatientBillsDetailsMedicalID", 0);
                            cmd1.Parameters.AddWithValue("@MedicalBillID", Convert.ToInt32(obj.medicalReturnID));

                            if (obj.oldBillNumber == null || obj.oldBillNumber == "" || obj.oldBillNumber == "--New--")
                            {
                                cmd1.Parameters.AddWithValue("@BillNo", 0);
                            }

                            else
                            {
                                cmd1.Parameters.AddWithValue("@BillNo", obj.oldBillNumber);
                            }

                            cmd1.Parameters.AddWithValue("@ProductID", obj.tblProductID.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@ProductName", obj.tblProductName.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@ProductUnitID", 1);
                            cmd1.Parameters.AddWithValue("@ProductDetailID", obj.ProductDetailsID);
                            cmd1.Parameters.AddWithValue("@BatchNo", obj.tblbatchNumber.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@ExpiryDate", obj.tblexpiryDate.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@MRP", obj.tblmrp.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@SalesRate", obj.tblnetRate.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@Quantity", obj.tblQtyU.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@QuantityL", obj.tblQtyL.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@FreeQuantity", obj.tblfreeQuantity.Split(',')[k]);

                            if (obj.patientRegNo == 0)
                            {
                                cmd1.Parameters.AddWithValue("@BillType", "GeneralMedicalBill");
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@BillType", dt.Rows[0]["PatientType"].ToString() + "MedicalBill");
                            }

                            cmd1.Parameters.AddWithValue("@TotalAmount", obj.tbltotalAmount.Split(',')[k]);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);

                            con.Open();
                            int j = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return true;


        }

        public DataSet GetPatientBillsMedical(int BillNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientBillsMedicalReturn", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@MedicalBillID", BillNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetPatientBillsDetailsMedical(int BillNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientBillsDetailsMedical", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@MedicalBillID", BillNo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int DeleteBill(int medicalReturnID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeletePatientBillsMedical", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MedicalBillID", medicalReturnID);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }

        public List<PatientMedicalReturn> Bind_oldPrint(int regno)
        {
            DataTable dt1 = Bind_patDetail(regno);

            Connect();
            List<PatientMedicalReturn> li_medBill = new List<PatientMedicalReturn>();
            SqlCommand cmd = new SqlCommand("GetPatientOLDMedicalBill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", regno);

            if (dt1.Rows.Count > 0)
            {
                cmd.Parameters.AddWithValue("@BillType", dt1.Rows[0]["PatientType"].ToString() + "MedicalBill");
            }
            else
            {
                cmd.Parameters.AddWithValue("@BillType", "GeneralMedicalBill");
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                li_medBill.Add(
                    new PatientMedicalReturn
                    {
                        patientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                        oldBillNumber = Convert.ToString(dr["BillNo"]),
                        PatientType = Convert.ToString(dr["BillType"]),

                        referenceDate = Convert.ToString(dr["BillNo&Date"]),
                        patientName = Convert.ToString(dr["PatientDetails"]),

                    });
            }
            return li_medBill;
        }
    }
}