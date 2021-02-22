using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Models.Keystone;
using Microsoft.ApplicationBlocks.Data;
using System.IO;


namespace KeystoneProject.Buisness_Logic.Hospital
{
    public class BL_HospitalLocation
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

        public DataSet GetCity(string CityName, string City)
        {
            DataSet ds = new DataSet();
            try
            {
                KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Models.MasterFinacialAccounts.Accounts();
                Connect();
                SqlCommand cmd = new SqlCommand("select  CityID,CityName from City where  CityName like '" + CityName + "%" + "' and   CityID  like  '" + City + "%" + "' and RowStatus=0 order by  CityName asc", con);
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

        public DataTable GetCountryStateID(int CityID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("getCountryStateID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", @LocationID);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public List<HospitalLocation> SelectAllHospitalLocation()
        {
            Connect();
            List<HospitalLocation> accounts = new List<HospitalLocation>();
            SqlCommand cmd = new SqlCommand("GetAllHospitalLocation", con);
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
                string ReportsSignatureImg = "";

                string ReportSignature1Img = "";
                string CardBackendImg = "";
                string CardFrontendImg = "";
                string PriCardBackendImg ="";
                string PriCardFrontendImg ="";
                string OtherHeaderImg = "";
                string BillsHeaderImg = "";
                string LabReportsHeaderImg ="";
                string   ReportsHeaderImg ="";

                  if (dr["BillsHeaderImg"].ToString() != "")
                  {
                      BillsHeaderImg = Convert.ToBase64String((byte[])dr["BillsHeaderImg"]);
                  }
                  if (dr["LabReportsHeaderImg"].ToString() != "")
                  {
                      LabReportsHeaderImg = Convert.ToBase64String((byte[])dr["LabReportsHeaderImg"]);
                  }
                  if (dr["ReportsHeaderImg"].ToString() != "")
                  {
                      ReportsHeaderImg = Convert.ToBase64String((byte[])dr["ReportsHeaderImg"]);
                  }
                  if (dr["ReportsSignatureImg"].ToString() != "")
                {
                    ReportsSignatureImg = Convert.ToBase64String((byte[])dr["ReportsSignatureImg"]);
                }
                  if (dr["ReportSignature1Img"].ToString() != "")
                {
                    ReportSignature1Img = Convert.ToBase64String((byte[])dr["ReportSignature1Img"]);
                }
                  if (dr["CardBackendImg"].ToString() != "")
                {
                    CardBackendImg = Convert.ToBase64String((byte[])dr["CardBackendImg"]);
                }
                  if (dr["CardFrontendImg"].ToString() != "")
                {
                    CardFrontendImg = Convert.ToBase64String((byte[])dr["CardFrontendImg"]);
                }
                  if (dr["PriCardBackendImg"].ToString() != "")
                {
                    PriCardBackendImg = Convert.ToBase64String((byte[])dr["PriCardBackendImg"]);
                }
                  if (dr["PriCardFrontendImg"].ToString() != "")
                {
                    PriCardFrontendImg = Convert.ToBase64String((byte[])dr["PriCardFrontendImg"]);
                }
                  if (dr["OtherHeaderImg"].ToString() != "")
                {
                    OtherHeaderImg = Convert.ToBase64String((byte[])dr["OtherHeaderImg"]);
                }
                
                accounts.Add(
                    new HospitalLocation
                    {
                        LocationID = Convert.ToInt32(dr["LocationID"]),
                        LocationName = dr["LocationName"].ToString(),
                        HospitalName = dr["HospitalName"].ToString(),
                        GroupName = dr["GroupName"].ToString(),
                        Logo = dr["Logo"].ToString(),
                        ManagingBody = dr["ManagingBody"].ToString(),
                        Adminstrator = dr["Adminstrator"].ToString(),
                        Address = dr["Address"].ToString(),
                        Pincode = dr["PinCode"].ToString(),
                        PhoneNumber = dr["PhoneNo"].ToString(),
                        PhoneNumber1 = dr["PhoneNo1"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        EmailPassword = dr["EmailPassword"].ToString(),
                        Fax = Convert.ToString(dr["FaxNo"]),
                        EmailID = Convert.ToString(dr["EmailID"]),
                        CityID = Convert.ToString(dr["CityID"]),
                        StateID = Convert.ToString(dr["StateID"]),
                        CountryID = Convert.ToString(dr["CountryID"]),
                        City = Convert.ToString(dr["CityName"]),
                        State = Convert.ToString(dr["StateName"]),
                        Country = Convert.ToString(dr["CountryName"]),
                        URL = Convert.ToString(dr["URL"]),
                        BillHeader = Convert.ToString(dr["BillsHeader"]),
                        LabReportHeader = Convert.ToString(dr["LabReportsHeader"]),
                        ReportHeader = Convert.ToString(dr["ReportsHeader"]),
                        ReportSignature = Convert.ToString(dr["ReportSignature"]),
                        ReportSignature1 = Convert.ToString(dr["ReportSignature1"]),
                        AdmissionCardBack = Convert.ToString(dr["CardBackend"]),
                        AdmissionCardFront = Convert.ToString(dr["CardFrontend"]),
                        PrivilegeCardBack = Convert.ToString(dr["PriCardBackend"]),
                        PrivilegeCardFront = Convert.ToString(dr["PriCardFrontend"]),
                        OtherHeader = Convert.ToString(dr["OtherHeader"]),
                        BillHeader1 = Convert.ToString(dr["BillsHeader"]),
                        BillsHeaderImg = BillsHeaderImg,
                        LabReportsHeaderImg = LabReportsHeaderImg,
                        ReportsHeaderImg = ReportsHeaderImg,
                        ReportsSignatureImg = ReportsSignatureImg,
                        ReportSignature1Img = ReportSignature1Img,
                        CardBackendImg = CardBackendImg,
                        CardFrontendImg = CardFrontendImg,
                        PriCardBackendImg = PriCardBackendImg,
                        PriCardFrontendImg = PriCardFrontendImg,
                        OtherHeaderImg = OtherHeaderImg,
                        RegistrationNo = Convert.ToString(dr["RegistrationNo"]),
                        ServiceTaxNo = Convert.ToString(dr["ServiceTaxNo"]),
                        PANNo = Convert.ToString(dr["PANNo"]),
                        TANo = Convert.ToString(dr["TANo"]),
                        TDSCircle = Convert.ToString(dr["TDSCircle"]),
                        TDSProfessional = Convert.ToString(dr["TDSProfessional"]),
                        TDSContrator = Convert.ToString(dr["TDSContrator"]),
                        ServiceCharge = Convert.ToString(dr["ServiceCharge"]),
                        RegistrationCharge = Convert.ToString(dr["RegistrationCharge"]),
                        RegistrationRenwalCharges = Convert.ToString(dr["RegistrationRenwalCharges"]),
                    });
            }
            return accounts;
        }


        public bool CheckHospitalLocation(int LocationID, string LocationName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckHospitalLocation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);


                cmd.Parameters.AddWithValue("@LocationName", LocationName.ToUpper());

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
            catch (Exception)
            {
                return false;
            }
        }

        public bool Save(HospitalLocation obj)
        {
            Connect();

            try
            {

                SqlCommand cmd = new SqlCommand("IUHospitalLocation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);

                if (obj.ReferenceCode == null)
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                }

                if (obj.LocationID == 0 || obj.LocationID == null)
                {
                    cmd.Parameters.AddWithValue("@LocationID", 0);
                    cmd.Parameters["@LocationID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LocationID", obj.LocationID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                }
                if (obj.LocationName == null || obj.LocationName == "")
                {
                    cmd.Parameters.AddWithValue("@LocationName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LocationName", obj.LocationName);
                }
                if (obj.HospitalName == null || obj.HospitalName == "")
                {
                    cmd.Parameters.AddWithValue("@HospitalName", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HospitalName", obj.HospitalName);
                }

                if (obj.GroupName == null || obj.GroupName == "")
                {
                    cmd.Parameters.AddWithValue("@GroupName", obj.GroupName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GroupName", obj.GroupName);
                }

                if (obj.Logo == null || obj.Logo == "")
                {
                    cmd.Parameters.AddWithValue("@Logo",  DBNull.Value);
                }


                else
                {
                    //FileStream file1 = new FileStream(obj.Logo, FileMode.OpenOrCreate, FileAccess.Read);
                    //byte[] stphoto1 = new byte[file1.Length];
                    //file1.Read(stphoto1, 0, System.Convert.ToInt32(file1.Length));
                    //file1.Close();

                    cmd.Parameters.AddWithValue("@Logo", obj.Logo);
                }

                if (obj.ManagingBody == null || obj.ManagingBody == "")
                {
                    cmd.Parameters.AddWithValue("@ManagingBody", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ManagingBody", obj.ManagingBody);
                }

                if (obj.Adminstrator == null || obj.Adminstrator == "")
                {
                    cmd.Parameters.AddWithValue("@Adminstrator", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Adminstrator", obj.Adminstrator);
                }

                if (obj.Address == null || obj.Address == "")
                {
                    cmd.Parameters.AddWithValue("@Address", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                }
                if (obj.CityID == null || obj.CityID == "")
                {
                    cmd.Parameters.AddWithValue("@CityID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                }
                if (obj.Pincode == null || obj.Pincode == "")
                {
                    cmd.Parameters.AddWithValue("@PinCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PinCode", obj.Pincode);
                }

                if (obj.StateID == null || obj.StateID == "")
                {
                    cmd.Parameters.AddWithValue("@StateID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                }
                if (obj.CountryID == null || obj.CountryID == "")
                {
                    cmd.Parameters.AddWithValue("@CountryID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                }
                if (obj.PhoneNumber == null || obj.PhoneNumber == "")
                {
                    cmd.Parameters.AddWithValue("@PhoneNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNumber);
                }
                if (obj.Fax == null || obj.Fax == "")
                {
                    cmd.Parameters.AddWithValue("@FaxNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FaxNo", obj.Fax);
                }
                if (obj.PhoneNumber1 == null || obj.PhoneNumber1 == "")
                {
                    cmd.Parameters.AddWithValue("@PhoneNo1", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PhoneNo1", obj.PhoneNumber1);
                }
                if (obj.MobileNo == null || obj.MobileNo == "")
                {
                    cmd.Parameters.AddWithValue("@MobileNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                }

                if (obj.EmailPassword == null || obj.EmailPassword == "")
                {
                    cmd.Parameters.AddWithValue("@EmailPassword", 0);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@EmailPassword", obj.EmailPassword);
                }

                if (obj.EmailID == null || obj.EmailID == "")
                {
                    cmd.Parameters.AddWithValue("@EmailID", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                }

                if (obj.URL == null || obj.URL == "")
                {
                    cmd.Parameters.AddWithValue("@URL", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@URL", obj.URL);
                }
                if (obj.RegistrationNo == null || obj.RegistrationNo == "")
                {
                    cmd.Parameters.AddWithValue("@RegistrationNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RegistrationNo", obj.RegistrationNo);
                }

                if (obj.ServiceTaxNo == null || obj.ServiceTaxNo == "")
                {
                    cmd.Parameters.AddWithValue("@ServiceTaxNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ServiceTaxNo", obj.ServiceTaxNo);
                }

                if (obj.PANNo == null || obj.PANNo == "")
                {
                    cmd.Parameters.AddWithValue("@PANNo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PANNo", obj.PANNo);
                }

                if (obj.TANo == null || obj.TANo == "")
                {
                    cmd.Parameters.AddWithValue("@TANo", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TANo", obj.TANo);
                }

                if (obj.TDSCircle == null || obj.TDSCircle == "")
                {
                    cmd.Parameters.AddWithValue("@TDSCircle", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TDSCircle", obj.TDSCircle);
                }

                if (obj.RegistrationCharge == null || obj.RegistrationCharge == "")
                {
                    cmd.Parameters.AddWithValue("@RegistrationCharge", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RegistrationCharge", obj.RegistrationCharge);
                }

                if (obj.RegistrationRenwalCharges == null | obj.RegistrationRenwalCharges == "")
                {
                    cmd.Parameters.AddWithValue("@RegistrationRenwalCharges", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RegistrationRenwalCharges", obj.RegistrationRenwalCharges);
                }

                if (obj.TDSProfessional == null || obj.TDSProfessional == "")
                {
                    cmd.Parameters.AddWithValue("@TDSProfessional", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TDSProfessional", obj.TDSProfessional);
                }
                if (obj.TDSContrator == null || obj.TDSContrator == "")
                {
                    cmd.Parameters.AddWithValue("@TDSContrator", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TDSContrator", obj.TDSContrator);
                }
                if (obj.ServiceCharge == null || obj.ServiceCharge == "")
                {
                    cmd.Parameters.AddWithValue("@ServiceCharge", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ServiceCharge", obj.ServiceCharge);
                }


                if (obj.ReportSignature12 == "" || obj.ReportSignature12 == null)
                {

                    cmd.Parameters.AddWithValue("@ReportsSignatureImg",  null);
                }

                else
                {
                    

                    //FileStream file2 = new FileStream(obj.ReportSignature12, FileMode.OpenOrCreate, FileAccess.Read);
                    //byte[] stphoto2 = new byte[file2.Length];
                    //file2.Read(stphoto2, 0, System.Convert.ToInt32(file2.Length));
                    //file2.Close();
                    // E:\Govind\MVCProject23092020live\KeystoneProject\KeystoneProject\MRDFiles/images (7).jpg
                    string path = System.Web.HttpContext.Current.Server.MapPath("~")+ obj.ReportSignature12;
                    byte[] byteData = System.IO.File.ReadAllBytes(path);
                    cmd.Parameters.AddWithValue("@ReportsSignatureImg", byteData);
                }

                if (obj.ReportSignature12 == null || obj.ReportSignature12 == "")
                {
                    cmd.Parameters.AddWithValue("@ReportSignature", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReportSignature", obj.ReportSignature12);
                }

                if (obj.ReportSignature1 == "" || obj.ReportSignature1 == null)
                {
                    cmd.Parameters.AddWithValue("@ReportSignature1Img", null);
                }
                else
                {
                    FileStream file3 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~") + obj.ReportSignature1, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto3 = new byte[file3.Length];
                    file3.Read(stphoto3, 0, System.Convert.ToInt32(file3.Length));
                    file3.Close();
                    cmd.Parameters.AddWithValue("@ReportSignature1Img", stphoto3);
                }


                if (obj.ReportSignature1 == null || obj.ReportSignature1 == "")
                {
                    cmd.Parameters.AddWithValue("@ReportSignature1", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReportSignature1", obj.ReportSignature1);
                }

                if (obj.OtherSignature == null || obj.OtherSignature == "")
                {
                    cmd.Parameters.AddWithValue("@OtherHeaderImg", null);
                }

                else
                {
                    FileStream file4 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~") + obj.OtherSignature, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto4 = new byte[file4.Length];
                    file4.Read(stphoto4, 0, System.Convert.ToInt32(file4.Length));
                    file4.Close();

                    cmd.Parameters.AddWithValue("@OtherHeaderImg", stphoto4);
                }

                if (obj.OtherSignature == null || obj.OtherSignature == "")
                {
                    cmd.Parameters.AddWithValue("@OtherHeader", DBNull.Value);
                }
                else
                {
                 
                    cmd.Parameters.AddWithValue("@OtherHeader", obj.OtherSignature);
                }

                if (obj.BillHeader == null || obj.BillHeader == "")
                {
                    cmd.Parameters.AddWithValue("@BillsHeaderImg", null);
                }
                else
                {
                    FileStream file5 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~") + obj.BillHeader, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto5 = new byte[file5.Length];
                    file5.Read(stphoto5, 0, System.Convert.ToInt32(file5.Length));
                    file5.Close();
                    cmd.Parameters.AddWithValue("@BillsHeaderImg", stphoto5);
                }


                if (obj.BillHeader == null || obj.BillHeader == "")
                {
                    cmd.Parameters.AddWithValue("@BillsHeader", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillsHeader", obj.BillHeader);
                }

                if (obj.LabReportHeader == null || obj.LabReportHeader == "")
                {
                    cmd.Parameters.AddWithValue("@LabReportsHeaderImg", null);
                }

                else
                {
                    FileStream file6 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~")  + obj.LabReportHeader, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto6 = new byte[file6.Length];
                    file6.Read(stphoto6, 0, System.Convert.ToInt32(file6.Length));
                    file6.Close();

                    cmd.Parameters.AddWithValue("@LabReportsHeaderImg", stphoto6);
                }

                if (obj.LabReportHeader == null || obj.LabReportHeader == "")
                {
                    cmd.Parameters.AddWithValue("@LabReportsHeader", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LabReportsHeader", obj.LabReportHeader);
                }

                if (obj.ReportHeader == null || obj.ReportHeader == "")
                {
                    cmd.Parameters.AddWithValue("@ReportsHeaderImg", null);
                }
                else
                {
                    FileStream file7 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~")  + obj.ReportHeader, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto7 = new byte[file7.Length];
                    file7.Read(stphoto7, 0, System.Convert.ToInt32(file7.Length));
                    file7.Close();

                    cmd.Parameters.AddWithValue("@ReportsHeaderImg", stphoto7);
                }
                if (obj.ReportHeader == null || obj.ReportHeader == "")
                {
                    cmd.Parameters.AddWithValue("@ReportsHeader", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReportsHeader", obj.ReportHeader);
                }

                if (obj.AdmissionCardFront == null || obj.AdmissionCardFront == "")
                {
                    cmd.Parameters.AddWithValue("@CardFrontendImg", null);
                }
                else
                {
                    FileStream file8 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~")  + obj.AdmissionCardFront, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto8 = new byte[file8.Length];
                    file8.Read(stphoto8, 0, System.Convert.ToInt32(file8.Length));
                    file8.Close();

                    cmd.Parameters.AddWithValue("@CardFrontendImg", stphoto8);
                }
                if (obj.AdmissionCardFront == null || obj.AdmissionCardFront == "")
                {
                    cmd.Parameters.AddWithValue("@CardFrontend", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CardFrontend", obj.AdmissionCardFront);
                }

                if (obj.AdmissionCardBack == null || obj.AdmissionCardBack == "")
                {
                    cmd.Parameters.AddWithValue("@CardBackendImg", null);
                }
                else
                {
                    FileStream file9 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~")  + obj.AdmissionCardBack, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto9 = new byte[file9.Length];
                    file9.Read(stphoto9, 0, System.Convert.ToInt32(file9.Length));
                    file9.Close();

                    cmd.Parameters.AddWithValue("@CardBackendImg", stphoto9);
                }
                if (obj.AdmissionCardBack == null || obj.AdmissionCardBack == "")
                {
                    cmd.Parameters.AddWithValue("@CardBackend", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CardBackend", obj.AdmissionCardBack);
                }
                if (obj.PrivilegeCardBack == null || obj.PrivilegeCardBack == "")
                {
                    cmd.Parameters.AddWithValue("@PriCardBackendImg", null);
                }


                else
                {
                    FileStream file10 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~")  + obj.PrivilegeCardBack, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto10 = new byte[file10.Length];
                    file10.Read(stphoto10, 0, System.Convert.ToInt32(file10.Length));
                    file10.Close();
                    cmd.Parameters.AddWithValue("@PriCardBackendImg", stphoto10);
                }

                if (obj.PrivilegeCardBack == null || obj.PrivilegeCardBack == "")
                {
                    cmd.Parameters.AddWithValue("@PriCardBackend", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PriCardBackend", obj.PrivilegeCardBack);
                }

                if (obj.PrivilegeCardFront == null || obj.PrivilegeCardFront == "")
                {
                    cmd.Parameters.AddWithValue("@PriCardFrontendImg", null);
                }

                else
                {
                    FileStream file11 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~") + obj.PrivilegeCardFront, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto11 = new byte[file11.Length];
                    file11.Read(stphoto11, 0, System.Convert.ToInt32(file11.Length));
                    file11.Close();

                    cmd.Parameters.AddWithValue("@PriCardFrontendImg", stphoto11);
                }

                if (obj.PrivilegeCardFront == null || obj.PrivilegeCardFront == "")
                {
                    cmd.Parameters.AddWithValue("@PriCardFrontend", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PriCardFrontend", obj.PrivilegeCardFront);
                }


                if (obj.Pharmacy == null || obj.Pharmacy == "")
                {
                    cmd.Parameters.AddWithValue("@PharmacyImg", null);
                }

                else
                {
                    FileStream file15 = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~") + obj.PrivilegeCardFront, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] stphoto15 = new byte[file15.Length];
                    file15.Read(stphoto15, 0, System.Convert.ToInt32(file15.Length));
                    file15.Close();

                    cmd.Parameters.AddWithValue("@PharmacyImg", stphoto15);
                }

                if (obj.Pharmacy == null || obj.Pharmacy == "")
                {
                    cmd.Parameters.AddWithValue("@Pharmacy", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Pharmacy", obj.Pharmacy);
                }


                cmd.Parameters.AddWithValue("@CreationID", UserID);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int Delete(int LocationID)
        {
            int i = 0;

            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteHospitalLocation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                i = cmd.ExecuteNonQuery();

                con.Close();

            }
            return i;
        }
    }
}