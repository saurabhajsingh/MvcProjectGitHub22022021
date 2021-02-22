using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Pharmacy;

namespace KeystoneProject.Buisness_Logic.Pharmacy
{
    public class BL_PatientMedicalBill
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
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


        #region DeletePatientBills Bills
        public void DeletePatientBills(int HospitalID, int LocationID, int PatientRegNo, int BillNo, int CreationID, DataSet ds)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[5];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
                aParams[2].Value = PatientRegNo;
                aParams[3] = new SqlParameter("@BillNo", SqlDbType.Int);
                aParams[3].Value = BillNo;
                aParams[4] = new SqlParameter("@CreationID", SqlDbType.Int);
                aParams[4].Value = CreationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientBills", aParams);

                //foreach(DataRow dr in ds.Tables[0].Rows )
                //{
                //    dr["HospitalID"] = 1;
                //}

                SqlCommand cmd = new SqlCommand("GetPatientMedicalBillsForRefundProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", ds.Tables[0].Rows[0]["HospitalID"].ToString());
                cmd.Parameters.AddWithValue("@LocationID", ds.Tables[0].Rows[0]["LocationID"].ToString());
                cmd.Parameters.AddWithValue("@ServiceID", ds.Tables[0].Rows[0]["ServiceID"].ToString());
                cmd.Parameters.AddWithValue("@Quantity", ds.Tables[0].Rows[0]["Quantity"].ToString());
                cmd.ExecuteNonQuery();
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }


        public void DeleteProduct(int HospitalID, int LocationID, int MedicalBillID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;
                aParams[2] = new SqlParameter("@MedicalBillID", SqlDbType.Int);
                aParams[2].Value = MedicalBillID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeletePatientBillsMedical", aParams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }

        }
        #endregion

        #region Get Discount Reason Data
        public DataSet GetDiscountReason(int HospitalID, int LocationID)
        {
            SqlParameter[] aParams = new SqlParameter[2];
            try
            {
                Connect();
                DataSet ds = new DataSet();
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[1].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllDiscountReason", aParams);
                return ds;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        #endregion

        public DataSet GetProductData(int ProductID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@ProductID", SqlDbType.Int);
                param[3].Value = ProductID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProduct", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        #region Get Patient Bills & Bill Detail Data
        public DataSet GetPatientBillsMedicalReturn(int MedicalBillID, int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@MedicalBillID", SqlDbType.Int);
                param[0].Value = MedicalBillID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsMedicalReturn", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetCanteenCustomer(int CustomerID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@CustomerID", SqlDbType.Int);
                param[3].Value = CustomerID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetCanteenCustomer", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        //#endregion

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
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProductBillsDetails", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetPatientBillsDetailsMedical(int MedicalBillID, int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@MedicalBillID", SqlDbType.Int);
                param[0].Value = MedicalBillID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientBillsDetailsMedical", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        #endregion

        #region Get Services Details for Medical Bills Data
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
        public DataSet GetGSTRatio()
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GSTRatio", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }


        public DataSet GetCheckProduct(int HospitalID, int LocationID, string BatchNo)
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
                param[2] = new SqlParameter("@BatchNo", SqlDbType.NVarChar, 50);
                param[2].Value = BatchNo;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetCheckProduct", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;

        }
        #endregion

        #region Get All Services Data
        public DataSet GetAllServices(int HospitalID, int LocationID)
        {


            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllServicesForOPDBills", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        #endregion

        #region Get Patient OLD Bills No
        public DataSet GetPatientOLDBillsNO(int PatientRegNO, string BillType)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[0].Value = PatientRegNO;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                param[3] = new SqlParameter("@BillType", SqlDbType.NVarChar);
                param[3].Value = BillType;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientOLDMedicalBill", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        #endregion

        #region Get Patient Medical Bills
        public DataSet GetPatientMedicalBills(int PatientRegNO)
        {
            try
            {

                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientMedicalBills", param);
            }

            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }
        #endregion

        //#region Search Patient Medical Bill
        //public DataSet SearchPatientMedicalBillByNameID(string strParameterValue, SearchParameters Parameters, int HospitalID, int LocationID, string FromCity, int iPreNxt)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        Connect();
        //        SqlParameter[] aParams = new SqlParameter[5];
        //        aParams[0] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
        //        aParams[0].Value = Convert.ToInt32(0);
        //        aParams[1] = new SqlParameter("@PatientName", SqlDbType.VarChar, 100);
        //        aParams[1].Value = strParameterValue;
        //        aParams[2] = new SqlParameter("@ReferenceCode", SqlDbType.VarChar, 20);
        //        aParams[2].Value = strParameterValue;
        //        aParams[3] = new SqlParameter("@HospitalID", SqlDbType.Int);
        //        aParams[3].Value = HospitalID;
        //        aParams[4] = new SqlParameter("@LocationID", SqlDbType.Int);
        //        aParams[4].Value = LocationID;

        //        switch (Parameters)
        //        {
        //            case SearchParameters.ID:
        //                ds = SqlHelper.ExecuteDataset(_sDbConnection, CommandType.StoredProcedure, "SearchPatientMedicalBillByNameID", aParams);
        //                break;
        //            case SearchParameters.Name:
        //                ds = SqlHelper.ExecuteDataset(_sDbConnection, CommandType.StoredProcedure, "SearchPatientMedicalBillByNameID", aParams);
        //                break;

        //            default: break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.Publish(ex);
        //        throw ex;

        //    }
        //    return ds;
        //}

        //public DataSet SearchPatientMedicalBillByNameID(string strParameterValue, int iParameters, int HospitalID, int LocationID, string FromCity, int iPreNxt)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();

        //        switch (iParameters)
        //        {
        //            case 0:
        //                ds = SearchPatientMedicalBillByNameID(strParameterValue, SearchParameters.Name, HospitalID, LocationID, FromCity, iPreNxt);
        //                break;
        //            case 1:
        //                ds = SearchPatientMedicalBillByNameID(strParameterValue, SearchParameters.ID, HospitalID, LocationID, FromCity, iPreNxt);
        //                break;
        //            default: break;
        //        }
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.Publish(ex);

        //        throw ex;
        //    }
        //}

        //#endregion

        public DataTable Bind_patDetail(int regno)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientForLabBills", con);
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

        public bool Save(PatientMedicalBill obj)
        {
            try
            {
                int regno = Convert.ToInt32(obj.PatientRegNO);
                DataTable dt = Bind_patDetail(regno);

                Connect();
                SqlCommand cmd = new SqlCommand("IUPatientBillsMedical", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.MedicalBillID == 0)
                {
                    cmd.Parameters.AddWithValue("@MedicalBillID", 0);
                    cmd.Parameters["@MedicalBillID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "ADD");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MedicalBillID", obj.MedicalBillID);
                    cmd.Parameters.AddWithValue("@Mode", "EDIT");
                }

                if (obj.BillNo == null || obj.BillNo == "" || obj.BillNo == "--New--")
                {
                    cmd.Parameters.AddWithValue("@BillNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillNo", obj.BillNo);
                }

                if (obj.BillNoDate == null || obj.BillNoDate == "")
                {
                    cmd.Parameters.AddWithValue("@BillDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillDate", obj.BillNoDate);
                }

                if (obj.PatientRegNO == null || obj.PatientRegNO == "")
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", obj.CustomerID);
                    cmd.Parameters.AddWithValue("@BillType", "GeneralMedicalBill");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNO);
                    cmd.Parameters.AddWithValue("@BillType", dt.Rows[0]["PatientType"].ToString() + "MedicalBill");
                }

                if (obj.PatientName == null)
                {
                    cmd.Parameters.AddWithValue("@PatientName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientName", obj.CustomerName);
                }

                if (obj.Address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                }

                if (obj.message == null)
                {
                    cmd.Parameters.AddWithValue("@Message", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Message", obj.message);
                }

                cmd.Parameters.AddWithValue("@Path", "");

                if (obj.DoctorID == null || obj.DoctorID == "")
                {
                    cmd.Parameters.AddWithValue("@DoctorID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
                }

                if (obj.DoctorPrintName == null)
                {
                    cmd.Parameters.AddWithValue("@DoctorName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorName", obj.DoctorPrintName);
                }

                if (obj.SalesPersonID == null || obj.SalesPersonID == "")
                {
                    cmd.Parameters.AddWithValue("@SalesPersonID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SalesPersonID", obj.SalesPersonID);
                }

                if (obj.SalesPersonName == null)
                {
                    cmd.Parameters.AddWithValue("@SalesPersonName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SalesPersonName", obj.SalesPersonName);
                }

                if (obj.paymentType == null)
                {
                    cmd.Parameters.AddWithValue("@PaymentType", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaymentType", obj.paymentType);
                }

                if (obj.Number == null)
                {
                    cmd.Parameters.AddWithValue("@Number", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Number", obj.Number);
                }

                if (obj.Name == null)
                {
                    cmd.Parameters.AddWithValue("@Name", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                }

                if (obj.paymentDate == null)
                {
                    cmd.Parameters.AddWithValue("@ChequeDate", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ChequeDate", obj.paymentDate);
                }

                cmd.Parameters.AddWithValue("@Other", "");

                if (obj.Remarks == null)
                {
                    cmd.Parameters.AddWithValue("@Remarks", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remarks", obj.Remarks);
                }

                if (obj.grossTotal == null || obj.grossTotal == "")
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrossAmount", obj.grossTotal);
                }

                if (obj.discountAmount == null || obj.discountAmount == "")
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DiscountAmount", obj.discountAmount);
                }

                //extra
                if (obj.vatAmount == null || obj.vatAmount == "")
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxAmount", obj.vatAmount);
                }

                if (obj.salesReturn == null || obj.salesReturn == "")
                {
                    cmd.Parameters.AddWithValue("@SalesReturn", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SalesReturn", obj.salesReturn);
                }

                if (obj.totalAmount == null || obj.totalAmount == "")
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TotalAmount", obj.totalAmount);
                }

                if (obj.discountPercentage == null || obj.discountPercentage == "")
                {
                    cmd.Parameters.AddWithValue("@DiscountPercent", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DiscountPercent", obj.discountPercentage);
                }

                if (obj.otheLess == null || obj.otheLess == "")
                {
                    cmd.Parameters.AddWithValue("@OtherLess", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OtherLess", obj.otheLess);
                }

                if (obj.netAmount == null || obj.netAmount == "")
                {
                    cmd.Parameters.AddWithValue("@NetAmount", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NetAmount", obj.netAmount);
                }

                if (obj.cashRecipt == null || obj.cashRecipt == "")
                {
                    cmd.Parameters.AddWithValue("@CashReceived", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CashReceived", obj.cashRecipt);
                }

                cmd.Parameters.AddWithValue("@CreationID", UserID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                obj.MedicalBillID = Convert.ToInt32(cmd.Parameters["@MedicalBillID"].Value);
                con.Close();

                if (i > 0)
                {
                    string[] prdctID = obj.tblProductID.Split(',');
                    string[] prdctNm = obj.tblProductName.Split(',');

                    for (int k = 0; k < prdctNm.Length; k++)
                    {
                        if (prdctNm[k].Trim() != "")
                        {
                            SqlCommand cmd1 = new SqlCommand("IUPatientBillsDetailsMedical", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@Mode", "ADD");
                            cmd1.Parameters.AddWithValue("@PatientBillsDetailsMedicalID", 0);
                            cmd1.Parameters.AddWithValue("@MedicalBillID", Convert.ToInt32(obj.MedicalBillID));

                            if (obj.BillNo == null || obj.BillNo == "" || obj.BillNo == "--New--")
                            {
                                cmd1.Parameters.AddWithValue("@BillNo", 0);
                            }

                            else
                            {
                                cmd1.Parameters.AddWithValue("@BillNo", obj.BillNo);
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
                            cmd1.Parameters.AddWithValue("@DiscountPer", obj.tbldiscountPercentage.Split(',')[k]);

                            if (obj.PatientRegNO == "" || obj.PatientRegNO == null)
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
            SqlCommand cmd = new SqlCommand("GetPatientBillsMedical", con);
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

        public int DeleteBill(int MedicalBillID)
        {
            int i = 0;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeletePatientBillsMedical", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MedicalBillID", MedicalBillID);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            return i;
        }

        public List<PatientMedicalBill> Bind_NewTable()
        {
            Connect();
            List<PatientMedicalBill> li_medBill = new List<PatientMedicalBill>();
            SqlCommand cmd = new SqlCommand("GetAllMedicalindend", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                li_medBill.Add(
                    new PatientMedicalBill
                    {
                        PatientRegNO = Convert.ToString(dr["PatientRegNo"]),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        CustomerID = Convert.ToString(dr["MedicalIndendId"]),
                        DoctorID = Convert.ToString(dr["MedicalIndendId"]),
                        message = Convert.ToString(dr["Flag"]),
                        PatientType = Convert.ToString(dr["Mode"]),
                    });
            }
            return li_medBill;
        }

        public List<PatientMedicalBill> Rebind_Data(int medID)
        {
            Connect();
            List<PatientMedicalBill> li_medBill = new List<PatientMedicalBill>();
            SqlCommand cmd = new SqlCommand("GetMedicalDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@MedicalIndendId", medID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                li_medBill.Add(
                    new PatientMedicalBill
                    {
                        freeQuantity = Convert.ToString(dr["Qty"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        message = Convert.ToString(dr["Flag"]),                       
                    });
            }
            return li_medBill;
        }

        public List<PatientMedicalBill> Bind_oldPrint(int regno)
        {
            DataTable dt1 = Bind_patDetail(regno);

            Connect();
            List<PatientMedicalBill> li_medBill = new List<PatientMedicalBill>();
            SqlCommand cmd = new SqlCommand("GetPatientOLDMedicalBill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PatientRegNO", regno);
            cmd.Parameters.AddWithValue("@BillType", dt1.Rows[0]["PatientType"].ToString() + "MedicalBill");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();
            
            foreach (DataRow dr in dt.Rows)
            {
                li_medBill.Add(
                    new PatientMedicalBill
                    {
                        PatientRegNO = Convert.ToString(dr["PatientRegNo"]),
                        BillNo = Convert.ToString(dr["BillNo"]),
                        PatientType = Convert.ToString(dr["BillType"]),

                        BillNoDate = Convert.ToString(dr["BillNo&Date"]),
                        PatientName = Convert.ToString(dr["PatientDetails"]),
                     
                    });
            }
            return li_medBill;
        }

    }
}