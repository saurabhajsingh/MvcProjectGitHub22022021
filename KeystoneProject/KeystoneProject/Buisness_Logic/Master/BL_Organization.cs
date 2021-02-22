using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;

using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_Organization
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        DataSet dsServices = new DataSet();
        DataSet dsWardTPAWise = new DataSet();
        DataSet dsTestMaster = new DataSet();
        DataSet dsTestWardTPAWise = new DataSet();
        int varOrganizationID;

        public void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public DataSet ShowAllOrganization()
        {
            Connect();
            DataSet ds = null;
            try
            {

                SqlCommand cmd = new SqlCommand("GetAllOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public bool CheckOrganization(int OrganizationID, string OrganizationName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", OrganizationID);
                cmd.Parameters.AddWithValue("@OrganizationName", OrganizationName);
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
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Save(Organization obj)
        {
            string[] date = obj.ContactFrom.Split('-');
            Connect();
            try
            {
                #region IUOrganization

                SqlCommand cmd = new SqlCommand("IUOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", 0);
                cmd.Parameters["@OrganizationID"].Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("@OrganizationName", obj.OrganizationName);
                cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
                if (obj.OrganizationType == null)
                    cmd.Parameters.AddWithValue("@OrganizationType", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@OrganizationType", obj.OrganizationType);
                if (obj.EnrollmentStatus == null)
                    cmd.Parameters.AddWithValue("@EnrollmentStatus", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EnrollmentStatus", obj.EnrollmentStatus);
                if (obj.ContactFrom == null)
                    cmd.Parameters.AddWithValue("@ContactFrom", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ContactFrom", date[0]);
                if (obj.ContactFrom == null)
                    cmd.Parameters.AddWithValue("@ContactUpTo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ContactUpTo", date[1]);
                if (obj.Address == null)
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                cmd.Parameters.AddWithValue("@CutForTPA", obj.CutForTPA);
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                if (obj.GSTNO == null)
                    cmd.Parameters.AddWithValue("@GSTNO", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GSTNO", obj.GSTNO);
                if (obj.FaxNo == null)
                    cmd.Parameters.AddWithValue("@FaxNo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
                if (obj.PinCode == null)
                    cmd.Parameters.AddWithValue("@PinCode", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                if (obj.PhoneNo1 == null)
                    cmd.Parameters.AddWithValue("@PhoneNo1", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@PhoneNo1", obj.PhoneNo1);
                if (obj.MobileNo == null)
                    cmd.Parameters.AddWithValue("@MobileNo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                if (obj.EmailID == null)
                    cmd.Parameters.AddWithValue("@EmailID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                if (obj.URL == null)
                    cmd.Parameters.AddWithValue("@URL", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@URL", obj.URL);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Add");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                #endregion

                varOrganizationID = Convert.ToInt32(cmd.Parameters["@OrganizationID"].Value.ToString());


                if (varOrganizationID > 0)
                {
                    #region Add ContactPersons

                    // ContactPerson_Class[] cnew123 = obj.contact;
                    //if(cnew123.Length.ToString()!="" && cnew123.Length!=null) 
                    //{
                    if (obj.ContactPersonName1 != null || obj.ContactPersonName2 != null)
                    {
                        

                       
                                SqlCommand cmd1 = new SqlCommand("IUContactPersons", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@ContactPersonID", 0);
                                cmd1.Parameters.AddWithValue("@OrganizationID", varOrganizationID);
                        if (obj.ContactPersonName1 == null || obj.ContactPersonName2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@ContactPersonName","");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@ContactPersonName", obj.ContactPersonName1 + "," + obj.ContactPersonName2);
                        }
                                
                        cmd1.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
                        if(obj.Designation1 == null || obj.Designation2== null)
                        {
                            cmd1.Parameters.AddWithValue("@Designation","" );
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@Designation",obj.Designation1 +","+ obj.Designation2 );
                        }

                        if (obj.ContactPersonEmail1 == null || obj.ContactPersonEmail2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@EmailID", "");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@EmailID", obj.ContactPersonEmail1 + "," + obj.ContactPersonEmail2);
                        }
                        if (obj.ContactPersonPhoneNO1 == null || obj.ContactPersonPhoneNO2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@PhoneNO","");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@PhoneNO",obj.ContactPersonPhoneNO1 + "," + obj.ContactPersonPhoneNO2);
                        }

                        
                                
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();

                          
                    }

                    #endregion

                    SqlCommand cmdIUGetServicesForOrganization = new SqlCommand("IUGetServicesForOrganization", con);
                    cmdIUGetServicesForOrganization.CommandType = CommandType.StoredProcedure;
                    cmdIUGetServicesForOrganization.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdIUGetServicesForOrganization.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdIUGetServicesForOrganization.Parameters.AddWithValue("@OrganizationID", varOrganizationID);
                    cmdIUGetServicesForOrganization.Parameters.AddWithValue("@CreationID", UserID);
                    con.Open();
                    int count = cmdIUGetServicesForOrganization.ExecuteNonQuery();
                    con.Close();
                   
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataSet SelectOrganizationByID(int OrganizationID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrganizationID", OrganizationID);
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

        public DataSet SelectContactPersonByID(int OrganizationID)
        {
            Connect();
            DataSet ds1 = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetContactPerson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", OrganizationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds1 = new DataSet();
                ad.Fill(ds1);
                con.Close();
                return ds1;
            }
            catch (Exception ex)
            {
                return ds1;
            }
        }



        public DataSet GetCity(string CityName)
        {
            DataSet ds = new DataSet();


            string query = "select City.CityID,City.CityName ,State.StateID ,State.StateName,Country.CountryID,Country.CountryName from City left join State on State.StateID=City.StateID left join Country on Country.CountryID=State.CountryID where  CityName like ''+@CityName+'%'";
            Connect();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CityName", CityName);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();



            return ds;
        }



        public bool Edit(Organization obj)
        {
            Connect();
            string[] date = obj.ContactFrom.Split('-');
            try
            {
                SqlCommand cmd = new SqlCommand("IUOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", obj.OrganizationID);
                cmd.Parameters.AddWithValue("@OrganizationName", obj.OrganizationName);
                cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
                if (obj.OrganizationType == null)
                    cmd.Parameters.AddWithValue("@OrganizationType", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@OrganizationType", obj.OrganizationType);
                if (obj.EnrollmentStatus == null)
                    cmd.Parameters.AddWithValue("@EnrollmentStatus", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EnrollmentStatus", obj.EnrollmentStatus);
                if (obj.ContactFrom == null)
                    cmd.Parameters.AddWithValue("@ContactFrom", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ContactFrom", Convert.ToDateTime(date[0]).ToString("yyyy-MM-dd"));
                if (obj.ContactFrom == null)
                    cmd.Parameters.AddWithValue("@ContactUpTo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ContactUpTo", Convert.ToDateTime(date[1]).ToString("yyyy-MM-dd"));
                if (obj.Address == null)
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                cmd.Parameters.AddWithValue("@CutForTPA", obj.CutForTPA);
                cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                if (obj.GSTNO == null)
                    cmd.Parameters.AddWithValue("@GSTNO", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GSTNO", obj.GSTNO);
                if (obj.FaxNo == null)
                    cmd.Parameters.AddWithValue("@FaxNo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
                if (obj.PinCode == null)
                    cmd.Parameters.AddWithValue("@PinCode", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                if (obj.PhoneNo1 == null)
                    cmd.Parameters.AddWithValue("@PhoneNo1", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@PhoneNo1", obj.PhoneNo1);
                if (obj.MobileNo == null)
                    cmd.Parameters.AddWithValue("@MobileNo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                if (obj.EmailID == null)
                    cmd.Parameters.AddWithValue("@EmailID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                if (obj.URL == null)
                    cmd.Parameters.AddWithValue("@URL", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@URL", obj.URL);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    if (obj.ContactPersonName1 != null || obj.ContactPersonName2 != null)
                    {
                        
                        
                                SqlCommand cmd1 = new SqlCommand("IUContactPersons", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@ContactPersonID", obj.ContactPersonID);
                                cmd1.Parameters.AddWithValue("@OrganizationID", obj.OrganizationID);
                        if (obj.ContactPersonName1 == null || obj.ContactPersonName2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@ContactPersonName", "");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@ContactPersonName", obj.ContactPersonName1 + "," + obj.ContactPersonName2);
                        }

                        cmd1.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
                        if (obj.Designation1 == null || obj.Designation2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@Designation", "");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@Designation", obj.Designation1 + "," + obj.Designation2);
                        }

                        if (obj.ContactPersonEmail1 == null || obj.ContactPersonEmail2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@EmailID", "");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@EmailID", obj.ContactPersonEmail1 + "," + obj.ContactPersonEmail2);
                        }
                        if (obj.ContactPersonPhoneNO1 == null || obj.ContactPersonPhoneNO2 == null)
                        {
                            cmd1.Parameters.AddWithValue("@PhoneNO", "");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@PhoneNO", obj.ContactPersonPhoneNO1 + "," + obj.ContactPersonPhoneNO2);
                        }

                        cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();
                          
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public DataSet GetServicesForOrganization()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetServicesForOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                con.Close();
                ad.Fill(ds);
                return ds;

            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetOrganizationForServiceTpaDetail(int varOrganizationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetOrganizationForServiceTpaDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", varOrganizationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;

                con.Close();
                ad.Fill(ds);
                return ds;

            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetTestMasterForOrganization()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTestMasterForOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                con.Close();
                ad.Fill(ds);
                return ds;

            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetOrganizationForTestTpaDetail(int varOrganizationID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetOrganizationForTestTpaDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", varOrganizationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                con.Close();
                ad.Fill(ds);
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }
        public int DeleteOrganization(int OrganizationID)
        {
            int row = 0;
            Connect();

            try
            {
                SqlCommand cmd = new SqlCommand("DeleteOrganization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OrganizationID", OrganizationID);
                con.Open();
                row = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return row;
            }
            return row;
        }
    }
}