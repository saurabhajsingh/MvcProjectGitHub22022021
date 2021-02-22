using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;
using System.Data.OleDb;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class Bl_Services
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }


        public int ServiceID = 0;

        #region CheckServices
        public bool CheckServices(int ServicesID,string ServiceGroupID, string ServicesName)
        {
            Connect();
            string Table;
            bool flag;
            SqlCommand cmd = new SqlCommand("CheckServices", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ServiceID", ServicesID);
            cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID);
            cmd.Parameters.AddWithValue("@ServiceName", ServicesName);           
            cmd.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
            cmd.Parameters["@NameExists"].Direction = ParameterDirection.Output;
            con.Open();
            int i = cmd.ExecuteNonQuery();
            Table = (string)cmd.Parameters["@NameExists"].Value;
            if (Convert.ToInt32(Table) == 1)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        #endregion

        #region Save & Edit

        public bool Save(Services ServicesMod, ServicesCharges servicesCharges)
        {
            //  ServicesCharges servicesCharges = new ServicesCharges();


            bool flag = true;
            Connect();
            con.Open();

            SqlCommand cmd;
            if (ServicesMod.ServiceID > 0)
            {
                ServicesMod.Mode = "Edit";
            }
           
                #region ----------------------Add Services--------------------------------

                cmd = new SqlCommand("IUServices", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (ServicesMod.Mode == "Add")
                {
                    cmd.Parameters.AddWithValue("@ServiceID", 0);
                    cmd.Parameters["@ServiceID"].Direction = ParameterDirection.Output;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ServiceID", ServicesMod.ServiceID);
                }

                if (ServicesMod.ServiceGroupID == null)
                    cmd.Parameters.AddWithValue("@ServiceGroupID", 0);
                else
                    cmd.Parameters.AddWithValue("@ServiceGroupID", ServicesMod.ServiceGroupID);

                cmd.Parameters.AddWithValue("@ReferenceCode", 1);

                cmd.Parameters.AddWithValue("@ServiceName", ServicesMod.ServiceName);
                cmd.Parameters.AddWithValue("@BillCharges", ServicesMod.BillCharges);
                cmd.Parameters.AddWithValue("@BillAutoOne", ServicesMod.BillChargesOne);

            cmd.Parameters.AddWithValue("@CheckedOneDay", ServicesMod.CheckedOneDay);
            cmd.Parameters.AddWithValue("@CheckedOutTime", ServicesMod.CheckedOutTime);
            cmd.Parameters.AddWithValue("@CheckOneDayTime", ServicesMod.CheckOneDayTime);

            cmd.Parameters.AddWithValue("@CheckOutTime", ServicesMod.CheckOutTime);
                if (ServicesMod.UnitID == null)
                    cmd.Parameters.AddWithValue("@UnitID", 0);
                else
                    cmd.Parameters.AddWithValue("@UnitID", ServicesMod.UnitID);

                cmd.Parameters.AddWithValue("@GeneralCharges", ServicesMod.GeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", ServicesMod.EmergencyCharges);
                cmd.Parameters.AddWithValue("@ServiceType", ServicesMod.ServiceType);

                if (ServicesMod.TPAHSNCode == "")
                {
                    cmd.Parameters.AddWithValue("@TPAHSNCode", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TPAHSNCode", ServicesMod.TPAHSNCode);
                }
                if (ServicesMod.HospitalHSNCode == null)
                {
                    cmd.Parameters.AddWithValue("@HospitalHSNCode", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HospitalHSNCode", ServicesMod.HospitalHSNCode);
                }
                if (ServicesMod.HSNCode == "")
                {
                    cmd.Parameters.AddWithValue("@HSNCode", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNCode", ServicesMod.HSNCode);
                }

                cmd.Parameters.AddWithValue("@RecommendedByDoctor", "");
                if (ServicesMod.HideInBilling == null || ServicesMod.HideInBilling == "No")
                {
                    cmd.Parameters.AddWithValue("@HideInBilling", "No");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HideInBilling", ServicesMod.HideInBilling);
                }
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                int RowNo = cmd.ExecuteNonQuery();
                #endregion

                #region -------------------Add ServiceCharges-------------------

                if ("Both" == ServicesMod.ServiceType || "IPD" == ServicesMod.ServiceType || "OPD" == ServicesMod.ServiceType)
                {
                    if (RowNo > 0)
                    {
                        int ServiceChargesID = 0;
                        ServicesMod.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                        ServiceID = ServicesMod.ServiceID;

                        DataSet dsServiceCharge = new DataSet();
                        dsServiceCharge = SelectServiceChargesbyID(ServiceID);
                        con.Open();
                        for (int i = 0; i < servicesCharges.WardID.Length; i++)
                        {
                            cmd = new SqlCommand("IUServiceCharges", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd.Parameters.AddWithValue("@LocationID", LocationID);

                            // if (ServicesMod.Mode == "Add")
                            // {
                            cmd.Parameters.AddWithValue("@ServiceChargesID", 0);
                            cmd.Parameters["@ServiceChargesID"].Direction = ParameterDirection.Output;
                            //  }
                            //else
                            //{
                            //    cmd.Parameters.AddWithValue("@ServiceChargesID", dsServiceCharge.Tables[0].Rows[i]["ServiceChargesID"].ToString());
                            //}

                            cmd.Parameters.AddWithValue("@ServiceID", ServicesMod.ServiceID);
                            cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(servicesCharges.WardID[i].ToString()));

                            if (ServicesMod.GeneralCharges == null)
                                cmd.Parameters.AddWithValue("@GeneralCharges", 0.00);
                            else
                                cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(servicesCharges.GeneralCharges1[i].ToString()));

                            if (ServicesMod.EmergencyCharges == null)
                                cmd.Parameters.AddWithValue("@EmergencyCharges", 0.00);
                            else
                                cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(servicesCharges.EmergencyCharges1[i].ToString()));

                            cmd.Parameters.AddWithValue("@CreationID", UserID);
                            cmd.Parameters.AddWithValue("@Mode", "Add");
                            ServiceChargesID = cmd.ExecuteNonQuery();
                        }
                    #endregion --------------------------------------

                    #region Add ServiceTPAWise
                    if (ServicesMod.HideInBilling == "Yes")
                    {


                        String Mode = "";
                        Mode = cmd.Parameters["@Mode"].Value.ToString();

                        if (Mode == "Add")
                        {
                            DataSet dsTPAWise = new DataSet();
                            dsTPAWise = GetServicesTPAWiseForOrganizationID();

                            for (int i = 0; i < dsTPAWise.Tables[0].Rows.Count; i++)
                            {
                                cmd = new SqlCommand("IUServicesTPAWise", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd.Parameters.AddWithValue("@ServicesTPAWiseID", 0);
                                cmd.Parameters["@ServicesTPAWiseID"].Direction = ParameterDirection.Output;

                                // cmd.Parameters.AddWithValue("@HideInBilling", ServicesMod.RecommendedByDoctor);
                                cmd.Parameters.AddWithValue("@ServiceID", ServicesMod.ServiceID);

                                cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                                cmd.Parameters.AddWithValue("@HideInBilling", ServicesMod.HideInBilling);
                                if (ServicesMod.UnitID == null)
                                    cmd.Parameters.AddWithValue("@UnitID", 0);
                                else
                                    cmd.Parameters.AddWithValue("@UnitID", ServicesMod.UnitID);

                                cmd.Parameters.AddWithValue("@OrganizationID", dsTPAWise.Tables[0].Rows[i]["OrganizationID"].ToString());
                                cmd.Parameters.AddWithValue("@GeneralCharges", ServicesMod.GeneralCharges);
                                cmd.Parameters.AddWithValue("@EmergencyCharges", ServicesMod.EmergencyCharges);
                                cmd.Parameters.AddWithValue("@ServiceType", ServicesMod.ServiceType);
                                cmd.Parameters.AddWithValue("@RecommendedByDoctor", "");
                                cmd.Parameters.AddWithValue("@CreationID", UserID);
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                                int ServicesTPAWiseID = cmd.ExecuteNonQuery();

                                //if (ServicesTPAWiseID > 0)
                                //{
                                //    flag = true;
                                //}
                                //else
                                //{
                                //    flag = false;
                                //}
                            }
                            #endregion

                            #region Add ServicesTPAWiseDetailForOrganization

                            ServicesMod.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                            ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());

                            DataSet dsWardDetailsTPAWise = new DataSet();
                            dsWardDetailsTPAWise = GetServicesTPAWiseDetailForOrganizationID();


                            for (int i = 0; i < dsWardDetailsTPAWise.Tables[0].Rows.Count; i++)
                            {

                                cmd = new SqlCommand("IUServicesDetailsTPAWise", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd.Parameters.AddWithValue("@ServicesDetailsTPAWiseID", 0);
                                cmd.Parameters["@ServicesDetailsTPAWiseID"].Direction = ParameterDirection.Output;

                                cmd.Parameters.AddWithValue("@ServicesTPAWiseID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["ServicesTPAWiseID"].ToString()));

                                if (servicesCharges.WardID == null)
                                    cmd.Parameters.AddWithValue("@WardID", 0);
                                else
                                    cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardID"].ToString()));

                                cmd.Parameters.AddWithValue("@WardName", Convert.ToString(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardName"].ToString()));

                                cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                                cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                                cmd.Parameters.AddWithValue("@CreationID", UserID);
                                cmd.Parameters.AddWithValue("@Mode", "Add");
                                int Row2 = cmd.ExecuteNonQuery();

                                if (Row2 > 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }
                    }

                    }
                }
                            #endregion

                if (flag == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
           

            
            con.Close();
            return flag;
        }

        #endregion



        public void OrganizationSaveTPA(Services ServicesMod)
        {
            #region IUOrganization

            SqlCommand cmd = new SqlCommand("IUOrganization", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@OrganizationID", 0);
            cmd.Parameters["@OrganizationID"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@OrganizationName", ServicesMod.OrganizationName);
            cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
            cmd.Parameters.AddWithValue("@OrganizationType", ServicesMod.ServiceType);
            cmd.Parameters.AddWithValue("@EnrollmentStatus", DBNull.Value);
            cmd.Parameters.AddWithValue("@EnrollmentStatus", "Yes");
            cmd.Parameters.AddWithValue("@ContactFrom", DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactUpTo", DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", DBNull.Value);
            cmd.Parameters.AddWithValue("@CityID", "");
            cmd.Parameters.AddWithValue("@CutForTPA", "");
            cmd.Parameters.AddWithValue("@StateID", "");
            cmd.Parameters.AddWithValue("@CountryID", "");
            cmd.Parameters.AddWithValue("@GSTNO", DBNull.Value);
            cmd.Parameters.AddWithValue("@FaxNo", DBNull.Value);
            cmd.Parameters.AddWithValue("@PinCode", DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNo1", DBNull.Value);
            cmd.Parameters.AddWithValue("@MobileNo", DBNull.Value);
            cmd.Parameters.AddWithValue("@EmailID", DBNull.Value);
            cmd.Parameters.AddWithValue("@URL", DBNull.Value);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "Add");
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            #endregion
        }
        string HSNCode = "";
        public string ReffrenceCode()
        {
            HSNCode += DateTime.Now.ToString("DD");
            HSNCode += DateTime.Now.ToString("MM");
            HSNCode += DateTime.Now.ToString("hh");
            HSNCode += DateTime.Now.ToString("mm");
            HSNCode += DateTime.Now.ToString("ss");
            return HSNCode;
        }
        public string ReffrenceExcel()
        {
            HSNCode = "";
            string Day, Month, year, hour, Min, sec;
            Day = DateTime.Now.ToString("dd");
            Month = DateTime.Now.ToString("MM");
            year = DateTime.Now.ToString("yyyy");
            hour = DateTime.Now.ToString("hh");
            Min = DateTime.Now.ToString("mm");
            sec = DateTime.Now.ToString("ss");
            HSNCode = Day + "_" + Month + "_" + year + "_" + hour + "_" + Min + "_" + sec;
            return HSNCode;
        }






        #region ShowAll
        public DataSet SelectAllData()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServices", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        #endregion


        #region ShowAll
        public DataSet GetAllServicesExcelData()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServicesExcelData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        #endregion

        #region ALl Ward
        public DataSet AllWardCharges()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllWardName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        #endregion

        #region Get TPA For IU
        public DataSet GetServicesTPAWiseForOrganizationID()
        {
            Connect();
            DataSet dsTPA = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetServicesTPAWiseForOrganizationID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsTPA = new DataSet();
                da.Fill(dsTPA);
                //con.Close();
                return dsTPA;
            }
            catch (Exception)
            {
                return dsTPA;
            }
        }

        public DataSet GetServicesTPAWiseDetailForOrganizationID()
        {
            //Services_Model ServicesMod = new Services_Model();

            Connect();
            DataSet dsTPA = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetServicesTPAWiseDetailForOrganizationID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsTPA = new DataSet();
                da.Fill(dsTPA);
                //con.Close();
                return dsTPA;
            }
            catch (Exception)
            {
                return dsTPA;
            }
        }

        #endregion

        #region Select Services By ID
        public DataSet SelectAllServicesbyID(int ServiceID)
        {
            SqlConnection con = null;

            DataSet ds = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                Connect();
                SqlCommand cmd = new SqlCommand("GetServices", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ServiceID", ServiceID); // i will pass zero to MobileID beacause its Primary .

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);

                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                con.Open();

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }
        }

        public DataSet SelectServiceChargesbyID(int ServiceID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetServiceCharges", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@ServiceID", ServiceID); // i will pass zero to MobileID beacause its Primary .


            DataSet ds = new DataSet();
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            //  ds = new DataSet();

            da.Fill(ds);
            con.Close();
            return ds;

        }

        #endregion

        #region Delete Services
        public int DeleteServices(int ServiceID, string ServiceType)
        {
            string Table = string.Empty;
           
                DataSet dsService = new DataSet();
                dsService = SelectAllServicesbyID(ServiceID);
                ServiceType = dsService.Tables[0].Rows[0]["ServiceType"].ToString();

                Connect();
                SqlCommand cmd = new SqlCommand("DeleteServices", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                cmd.Parameters.AddWithValue("@ServiceType", ServiceType);
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@TableName"].Value;
                con.Close();
                return i;
               
            }

           
        #endregion

        public DataSet GetServiceGroupID(string ServiceGroupName, string ServiceGroupID)
        {
            Connect();
            //int HospitalID = 1;
            //int LocationID = 1;
            if (ServiceGroupID == null)
            {
                ServiceGroupID = "%";
            }
            if (ServiceGroupID == "")
            {
                ServiceGroupID = "%";
            }
            //  SqlCommand cmd = new SqlCommand("select  ServiceGroupID,ServiceGroupName from ServiceGroup where ServiceGroupName like ''+@ServiceGroupName+'%' and ServiceGroupID like'" + ServiceGroupID + "%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            SqlCommand cmd = new SqlCommand("select  ServiceGroupID,ServiceGroupName,HSNCode from ServiceGroup where  ServiceGroupID like'" + ServiceGroupID + "' and (ServiceGroupName like '" + @ServiceGroupName + "%' OR HSNCode like'" + @ServiceGroupName + "%')  and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);

            cmd.Parameters.AddWithValue("@ServiceGroupName", ServiceGroupName);
            cmd.Parameters.AddWithValue("@HSNCode", ServiceGroupName);
            cmd.Parameters.AddWithValue("@ServiceGroupID", ServiceGroupID);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetUnitID(string UnitName)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("select UnitID, UnitName from Unit where UnitName like ''+@UnitName+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            cmd.Parameters.AddWithValue("@UnitName", UnitName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetUnit_id(string UnitID)
        {
            Connect();

            //SqlCommand cmd = new SqlCommand("select UnitID, UnitName from Unit where UnitID =@UnitID and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            SqlCommand cmd = new SqlCommand("select UnitID, UnitName from Unit where UnitID  like '" + UnitID + "' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);

            cmd.Parameters.AddWithValue("@UnitID", UnitID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetWardDetailsPivot()
        {
            List<string> add = new List<string>();

            DataSet ds = new DataSet();
            try
            {
                Connect();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWardDetailsPivot", param);
               
              
            }
            catch (DBConcurrencyException exp)
            {
            
                throw exp;
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return ds;
        }
        public DataSet GetAllWardName()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllWard", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }
        //-------------------------Save Excel Data



        #region Save & Edit
        public bool SaveExcel(DataSet ds)
        {
            int ServiceID = 0;
            //  ServicesCharges servicesCharges = new ServicesCharges();
            // ds= btnImport_Click("");

            bool flag = true;
            Connect();
            //  con.Open();

            SqlCommand cmd;

            try
            {
                #region ----------------------Add Services--------------------------------

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ServiceID"].ToString() == "")
                    {
                        ServiceID = 0;
                    }
                    else
                    {
                        ServiceID = Convert.ToInt32(dr["ServiceID"]);
                    }
                    string UntID = "";// GetUnitID(dr["UnitName"].ToString()).Tables[0].Rows[0]["UnitID"].ToString();
                    if (UntID == "")
                    {
                        UntID = "1";
                    }
                    if (dr["ServiceName"].ToString() != "")
                    {
                        cmd = new SqlCommand("IUServices", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);

                        if (ServiceID > 0)
                        {
                            cmd.Parameters.AddWithValue("@ServiceID", ServiceID);

                            cmd.Parameters.AddWithValue("@Mode", "Edit");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                            cmd.Parameters.AddWithValue("@Mode", "Add");
                            cmd.Parameters["@ServiceID"].Direction = ParameterDirection.Output;
                        }
                        cmd.Parameters.AddWithValue("@ServiceGroupID", dr["ServiceGroupID"]);

                        cmd.Parameters.AddWithValue("@ReferenceCode", 1);

                        cmd.Parameters.AddWithValue("@ServiceName", dr["ServiceName"]);


                        cmd.Parameters.AddWithValue("@UnitID", UntID);

                        cmd.Parameters.AddWithValue("@GeneralCharges", dr["OPD Charges"]);
                        cmd.Parameters.AddWithValue("@EmergencyCharges", dr["OPD Charges"]);
                        cmd.Parameters.AddWithValue("@ServiceType", dr["ServiceType"]);


                        cmd.Parameters.AddWithValue("@TPAHSNCode", ReffrenceCode());


                        cmd.Parameters.AddWithValue("@HospitalHSNCode", ReffrenceCode());

                        cmd.Parameters.AddWithValue("@RecommendedByDoctor", "");
                        cmd.Parameters.AddWithValue("@HideInBilling", "");
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        con.Open();
                        int RowNo = cmd.ExecuteNonQuery();
                        if (RowNo > 0)
                        {
                            ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                        }
                #endregion

                        #region -------------------Add ServiceCharges-------------------


                        if (ServiceID > 0)
                        {

                            int ServiceChargesID = 0;
                            if (RowNo > 0)
                            {
                                ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                            }
                            DataSet dsServiceCharge = new DataSet();
                            dsServiceCharge = SelectServiceChargesbyID(ServiceID);
                            dsServiceCharge.Reset();
                            dsServiceCharge = GetAllWardName();
                            con.Open();
                            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                            {
                                if (i > 5)
                                {

                                    string WordName = ds.Tables[0].Columns[i].ColumnName;
                                    string charg = dr[WordName].ToString();
                                    if (charg == "")
                                    {
                                        dr[WordName] = 0;
                                    }
                                    DataView dvService = new DataView(dsServiceCharge.Tables[0], " WardName = '" + WordName.ToString() + "' ", "", DataViewRowState.CurrentRows);
                                    if (dvService.ToTable().Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("IUServiceCharges", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                        cmd.Parameters.AddWithValue("@LocationID", LocationID);


                                        cmd.Parameters.AddWithValue("@ServiceChargesID", 0);
                                        cmd.Parameters["@ServiceChargesID"].Direction = ParameterDirection.Output;

                                        // cmd.Parameters.AddWithValue("@ServiceChargesID", dsServiceCharge.Tables[0].Rows[i]["ServiceChargesID"].ToString());

                                        cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                                        cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(dvService.ToTable().Rows[0]["WardID"]));
                                        cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(dr[WordName]));
                                        cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(dr[WordName]));
                                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                                        cmd.Parameters.AddWithValue("@Mode", "Add");
                                        ServiceChargesID = cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            con.Close();
                        }
                        #endregion --------------------------------------

                        #region Add ServiceTPAWise

                        String Mode = "";
                        Mode = cmd.Parameters["@Mode"].Value.ToString();
                    }
                    //if (Mode == "Add")
                    //{
                    //    DataSet dsTPAWise = new DataSet();
                    //    dsTPAWise = GetServicesTPAWiseForOrganizationID();

                    //    for (int i = 0; i < dsTPAWise.Tables[0].Rows.Count; i++)
                    //    {
                    //        cmd = new SqlCommand("IUServicesTPAWise", con);
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    //        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", 0);
                    //        cmd.Parameters["@ServicesTPAWiseID"].Direction = ParameterDirection.Output;

                    //        cmd.Parameters.AddWithValue("@HideInBilling", dsTPAWise.Tables[0].Rows[i]["HideInBilling"].ToString());
                    //        cmd.Parameters.AddWithValue("@ServiceID", ServicesMod.ServiceID);

                    //        cmd.Parameters.AddWithValue("@ReferenceCode", 1);


                    //        if (ServicesMod.UnitID == null)
                    //            cmd.Parameters.AddWithValue("@UnitID", 0);
                    //        else
                    //            cmd.Parameters.AddWithValue("@UnitID", ServicesMod.UnitID);

                    //        cmd.Parameters.AddWithValue("@OrganizationID", dsTPAWise.Tables[0].Rows[i]["OrganizationID"].ToString());
                    //        cmd.Parameters.AddWithValue("@GeneralCharges", ServicesMod.GeneralCharges);
                    //        cmd.Parameters.AddWithValue("@EmergencyCharges", ServicesMod.EmergencyCharges);
                    //        cmd.Parameters.AddWithValue("@ServiceType", ServicesMod.ServiceType);
                    //        cmd.Parameters.AddWithValue("@RecommendedByDoctor", ServicesMod.RecommendedByDoctor);
                    //        cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //        cmd.Parameters.AddWithValue("@Mode", "Add");
                    //        int ServicesTPAWiseID = cmd.ExecuteNonQuery();

                    //        //if (ServicesTPAWiseID > 0)
                    //        //{
                    //        //    flag = true;
                    //        //}
                    //        //else
                    //        //{
                    //        //    flag = false;
                    //        //}
                    //    }
                    //#endregion
                    //#region Add ServicesTPAWiseDetailForOrganization

                    //    ServicesMod.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                    //    ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());

                    //    DataSet dsWardDetailsTPAWise = new DataSet();
                    //    dsWardDetailsTPAWise = GetServicesTPAWiseDetailForOrganizationID();


                    //    for (int i = 0; i < dsWardDetailsTPAWise.Tables[0].Rows.Count; i++)
                    //    {

                    //        cmd = new SqlCommand("IUServicesDetailsTPAWise", con);
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    //        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    //        cmd.Parameters.AddWithValue("@ServicesDetailsTPAWiseID", 0);
                    //        cmd.Parameters["@ServicesDetailsTPAWiseID"].Direction = ParameterDirection.Output;

                    //        cmd.Parameters.AddWithValue("@ServicesTPAWiseID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["ServicesTPAWiseID"].ToString()));

                    //        if (servicesCharges.WardID == null)
                    //            cmd.Parameters.AddWithValue("@WardID", 0);
                    //        else
                    //            cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardID"].ToString()));

                    //        cmd.Parameters.AddWithValue("@WardName", Convert.ToString(dsWardDetailsTPAWise.Tables[0].Rows[i]["WardName"].ToString()));

                    //        cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                    //        cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(dsWardDetailsTPAWise.Tables[0].Rows[i]["GeneralCharges"].ToString()));
                    //        cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //        cmd.Parameters.AddWithValue("@Mode", "Add");
                    //        int Row2 = cmd.ExecuteNonQuery();

                    //        if (Row2 > 0)
                    //        {
                    //            flag = true;
                    //        }
                    //        else
                    //        {
                    //            flag = false;
                    //        }
                    //    }
                    //}





                    //-------------
                }

                        #endregion

                if (flag == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                flag = false;
            }
            con.Close();
            return flag;
        }

        #endregion

        public DataSet btnImport_Click(string path)
        {
            //SqlConnection conn = new SqlConnection(File.ReadAllText(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\srv.dll"));

            DataSet myDataSet = new DataSet();
            DataTable dataTable = new DataTable();
            string aa = path;
            //  string aa1 = @"D:\20-7-2019\KeystoneProject\KeystoneProject\Service_Excel/Service_Excel_DataDD_09_2019_05_11_55.xlsx";

            String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"", aa);
            //Create Connection to Excel work book 
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
            {

                // con.Open();
                //Create OleDbCommand to fetch data from Excel 
                // SqlCommand cmd = new SqlCommand("Select * from ProductDetails", con);
                //cmd.ExecuteNonQuery();

                //OleDbCommand cmd1 = new OleDbCommand("Select * from [Sheet1$]", excelConnection);
                OleDbCommand cmd1 = new OleDbCommand("Select * from [Exported from Keystone]", excelConnection);
                {
                    excelConnection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM  [Sheet1$]", excelConnString);

                    dataAdapter.Fill(myDataSet, "ProductDetails");
                    dataTable = myDataSet.Tables["ProductDetails"];



                    SaveExcel(myDataSet);

                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con))
                    {
                        excelConnection.Close();
                        con.Close();
                        //    sqlBulk.DestinationTableName = "ProductDetails";
                        //  sqlBulk.WriteToServer(dataTable);
                    }
                    // dReader.Close();

                    // MessageBox.Show("Record Imported Successfully");
                }
                //}
                // }
            }
            return myDataSet;
        }



    }
}