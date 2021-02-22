using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_ServicesPackage
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
        #region Save & Edit

        public bool Save(ServicesPackage objModel, ServicesPackage_ServicesCharges objServicepkg, ServicePackage_OPDCharges objserPkgOPD)
        {
            Services ServicesMod = new Services();
            ServicesCharges servicesCharges = new ServicesCharges();
         //   ServicesPackage_ServicesCharges servicepkg_servicecharge = new ServicesPackage_ServicesCharges();
            //  ServicesCharges servicesCharges = new ServicesCharges();


            bool flag = true;
            Connect();
            con.Open();

            SqlCommand cmd;
            if (objModel.serviceID1 > 0)
            {
                ServicesMod.Mode = "Edit";
            }
            else
            {
                ServicesMod.Mode = "Add";
            }
            try
            {
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
                    cmd.Parameters.AddWithValue("@ServiceID", objModel.serviceID1);
                }

                if (objModel.ServiceGroupID == null)
                    cmd.Parameters.AddWithValue("@ServiceGroupID", 0);
                else
                    cmd.Parameters.AddWithValue("@ServiceGroupID",objModel.ServiceGroupID);
                cmd.Parameters.AddWithValue("@ReferenceCode", 1);

                cmd.Parameters.AddWithValue("@ServiceName", objModel.ServiceName);

                if (objModel.UnitID == null)
                    cmd.Parameters.AddWithValue("@UnitID", 0);
                else
                    cmd.Parameters.AddWithValue("@UnitID", objModel.UnitID);

                cmd.Parameters.AddWithValue("@GeneralCharges", objModel.GeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", objModel.EmergencyCharges);
                cmd.Parameters.AddWithValue("@ServiceType", objModel.ServiceType);

                if (ServicesMod.TPAHSNCode == "")
                {
                    cmd.Parameters.AddWithValue("@TPAHSNCode", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TPAHSNCode", ServicesMod.TPAHSNCode);
                }
                if (ServicesMod.HospitalHSNCode != "")
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

                cmd.Parameters.AddWithValue("@RecommendedByDoctor", ServicesMod.RecommendedByDoctor);
                cmd.Parameters.AddWithValue("@HideInBilling", ServicesMod.RecommendedByDoctor);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                int RowNo = cmd.ExecuteNonQuery();
                #endregion


                if ("Both Package" == objModel.ServiceType || "IPD Package" == objModel.ServiceType)
                {
                    #region IPD Package



                    #region -------------------Add ServiceCharges-------------------

                    if ("Both Package" == objModel.ServiceType || "IPD Package" == objModel.ServiceType)
                    {
                        if (RowNo > 0)
                        {
                            ServicesMod.Mode = "Add";
                            int ServiceChargesID = 0;
                            objModel.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                            ServiceID = objModel.ServiceID;

                            //     DataSet dsServiceCharge = new DataSet();
                            //    dsServiceCharge = SelectServiceChargesbyID(ServiceID);

                            for (int i = 0; i < objServicepkg.WardID.Length; i++)
                            {
                                cmd = new SqlCommand("IUServiceCharges", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                                if (ServicesMod.Mode == "Add")
                                {
                                    cmd.Parameters.AddWithValue("@ServiceChargesID", 0);
                                    cmd.Parameters["@ServiceChargesID"].Direction = ParameterDirection.Output;
                                }
                                else
                                {
                                       cmd.Parameters.AddWithValue("@ServiceChargesID", 0);
                                }

                                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                                cmd.Parameters.AddWithValue("@WardID", Convert.ToInt32(objServicepkg.WardID[i].ToString()));

                                if (objModel.GeneralCharges == null)
                                    cmd.Parameters.AddWithValue("@GeneralCharges", 0.00);
                                else
                                    cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(objServicepkg.GeneralCharges1[i].ToString()));

                                if (objModel.EmergencyCharges == null)
                                    cmd.Parameters.AddWithValue("@EmergencyCharges", 0.00);
                                else
                                    cmd.Parameters.AddWithValue("@EmergencyCharges", Convert.ToDecimal(objServicepkg.EmergencyCharges1[i].ToString()));

                                cmd.Parameters.AddWithValue("@CreationID", UserID);
                                cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                                ServiceChargesID = cmd.ExecuteNonQuery();
                            }
                    #endregion --------------------------------------

                            #region ServiceDetailsPackage
                            objModel.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                            ServiceID = objModel.ServiceID;

                            for (int i = 0; i < objServicepkg.ServiceOrTestID.Length; i++)
                            {
                                int servicedetailpkgID = 0;
                                cmd = new SqlCommand("IUServiceDetailsPackage", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                                if (ServicesMod.Mode == "Add")
                                {
                                    cmd.Parameters.AddWithValue("@ServiceDetailsPackagelID", 0);
                                    cmd.Parameters["@ServiceDetailsPackagelID"].Direction = ParameterDirection.Output;

                                }
                                else
                                {
                                    //      cmd.Parameters.AddWithValue("@ServiceChargesID", dsServiceCharge.Tables[0].Rows[i]["ServiceChargesID"].ToString());
                                }

                                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                                cmd.Parameters.AddWithValue("@ServiceOrTestID", objServicepkg.ServiceOrTestID[i].ToString());
                                int servicetestid = Convert.ToInt32(objServicepkg.ServiceOrTestID[i].ToString());
                                cmd.Parameters.AddWithValue("@ServiceOrTestName", objServicepkg.ServiceNTestName[i].ToString());

                                cmd.Parameters.AddWithValue("@ServiceType", "Service");
                                cmd.Parameters.AddWithValue("@Quantity", objServicepkg.Quantity[i].ToString());
                                int quantity = Convert.ToInt32(objServicepkg.Quantity[i].ToString());
                                cmd.Parameters.AddWithValue("@Rechange", objModel.chkrechange);


                                cmd.Parameters.AddWithValue("@CreationID", UserID);
                                cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                                servicedetailpkgID = cmd.ExecuteNonQuery();


                            }


                            #endregion

                            #region  IUServicePackageForWardCharges

                            DataSet dsword = new DataSet();
                            dsword = GetWardName(ServiceID);
                            for (int j = 0; j < dsword.Tables[0].Rows.Count; j++)
                            {
                                int ServicePackageForWardCharges = 0;
                                cmd = new SqlCommand("IUServicePackageForWardCharges", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                                if (ServicesMod.Mode == "Add")
                                {
                                    cmd.Parameters.AddWithValue("@SPWchargeID", 0);
                                    cmd.Parameters["@SPWchargeID"].Direction = ParameterDirection.Output;
                                }
                                else
                                {
                                    //      cmd.Parameters.AddWithValue("@ServiceChargesID", dsServiceCharge.Tables[0].Rows[i]["ServiceChargesID"].ToString());
                                }

                                cmd.Parameters.AddWithValue("@ServicePackageID", dsword.Tables[0].Rows[j]["ServiceDetailsPackagelID"].ToString());

                                cmd.Parameters.AddWithValue("@ServiceOrTestID", dsword.Tables[0].Rows[j]["ServiceOrTestID"].ToString());

                                cmd.Parameters.AddWithValue("@ServiceType", "Service");

                                cmd.Parameters.AddWithValue("@WardID", dsword.Tables[0].Rows[j]["WardID"].ToString());
                                cmd.Parameters.AddWithValue("@WardName", dsword.Tables[0].Rows[j]["WardName"].ToString());

                                //    cmd.Parameters.AddWithValue("@Quantity", quantity);

                                if (objModel.GeneralCharges == null)
                                    cmd.Parameters.AddWithValue("@GeneralCharges", 0.00);
                                else
                                    cmd.Parameters.AddWithValue("@GeneralCharges", Convert.ToDecimal(objServicepkg.GenralCharges[j].ToString()));

                                cmd.Parameters.AddWithValue("@CreationID", UserID);
                                cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                                ServicePackageForWardCharges = cmd.ExecuteNonQuery();


                            }


                            #endregion


                            #region Add ServiceTPAWise

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

                                    cmd.Parameters.AddWithValue("@ServiceID", ServiceID);

                                    cmd.Parameters.AddWithValue("@ReferenceCode", 1);


                                    if (objModel.UnitID == null)
                                        cmd.Parameters.AddWithValue("@UnitID", 0);
                                    else
                                        cmd.Parameters.AddWithValue("@UnitID", objModel.UnitID);

                                    cmd.Parameters.AddWithValue("@OrganizationID", Convert.ToInt32(dsTPAWise.Tables[0].Rows[i]["OrganizationID"].ToString()));
                                    cmd.Parameters.AddWithValue("@GeneralCharges", objModel.GeneralCharges);
                                    cmd.Parameters.AddWithValue("@EmergencyCharges", objModel.EmergencyCharges);
                                    cmd.Parameters.AddWithValue("@ServiceType", objModel.ServiceType);
                                    cmd.Parameters.AddWithValue("@RecommendedByDoctor", ServicesMod.RecommendedByDoctor);
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

                                objModel.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
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

                                    //  cmd.Parameters.AddWithValue("@WardName", Convert.ToString(ServicesMod.dsServicesCharges.Tables[0].Rows[0]["WardName"].ToString()));

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
                                #endregion

                    #endregion
                }

                else if("OPD Package"== objModel.ServiceType)
                {
                    #region ServiceDetailsPackage for opd
                    ServicesMod.Mode = "Add";
                  
                    objModel.ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value.ToString());
                    ServiceID = objModel.ServiceID;

                    for (int i = 0; i < objserPkgOPD.ServiceOrTestIDOPD.Length; i++)
                    {
                        int servicedetailpkgOPDID = 0;
                        cmd = new SqlCommand("IUServiceDetailsPackageOPD", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);

                        if (ServicesMod.Mode == "Add")
                        {
                            cmd.Parameters.AddWithValue("@ServiceDetailsPackageOPDID", 0);
                            cmd.Parameters["@ServiceDetailsPackageOPDID"].Direction = ParameterDirection.Output;

                        }
                        else
                        {
                            //cmd.Parameters.AddWithValue("@ServiceDetailsPackageOPDID", objserPkgOPD.ServiceOrTestIDOPD[i].ToString());
                        }

                        cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                        cmd.Parameters.AddWithValue("@ServiceOrTestID", objserPkgOPD.ServiceOrTestIDOPD[i].ToString());
                       // int servicetestid = Convert.ToInt32(objServicepkg.ServiceOrTestID[i].ToString());
                        cmd.Parameters.AddWithValue("@ServiceOrTestName", objserPkgOPD.ServiceNTestNameOPD[i].ToString());

                        cmd.Parameters.AddWithValue("@ServiceType", "Service");
                        cmd.Parameters.AddWithValue("@Quantity", objserPkgOPD.QuantityOPD[i].ToString());
                        cmd.Parameters.AddWithValue("@GeneralCharges", objserPkgOPD.OPDGenralCharges[i].ToString());
                        cmd.Parameters.AddWithValue("@EmergencyCharges", objserPkgOPD.OPDEmergencyCharges[i].ToString());
                        cmd.Parameters.AddWithValue("@Rechange", objModel.chkrechange);


                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", ServicesMod.Mode);
                        servicedetailpkgOPDID = cmd.ExecuteNonQuery();


                    }


                    #endregion
                }

             


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


        public DataSet GetWardName(int ServiceID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetWardForPackages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                //con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetAllServiceDetailsPackage()
        {
            Connect();
            DataSet dsTPA = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllServiceDetailsPackage", con);
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

        public DataSet GetServicesCharges(int ServiceID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetServiceCharges", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet GetServicesChargesOPD(int ServiceID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetServiceChargesOPD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

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
        public DataSet SelectServiceChargesbyID(int ServiceID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetServiceCharges", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ServiceID", ServiceID); // i will pass zero to MobileID beacause its Primary .


            DataSet ds = new DataSet();
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            //  ds = new DataSet();

            da.Fill(ds);

            return ds;

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

        public DataSet GetAllWardName1()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllWard1", con);

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

        public DataSet GetAllWardNameWithPackageDetails(int ServiceID)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllWardWithServicePackages", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }

        #region GetServicesPackageforServiceNTest
        public DataSet GetServicesPackageforServiceNTest()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetServicesPackageforServiceNTest", con);

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
        #endregion

        public DataSet GetServiceDetailsPackage(int ServiceID)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetServiceDetailsPackage", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); 
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ServiceID", ServiceID);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }
    }
}