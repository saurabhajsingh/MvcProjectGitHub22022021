using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Laboratory;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Buisness_Logic.Laboratory
{
    public class BL_PatientLabBills
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        List<PatientLabBills> PatientLabBillsList = new List<PatientLabBills>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public List<PatientLabBills> BindPrefixPatient()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PrefixID,PrefixName,Gender from   PatientPrefix where  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus= 0 order by PrefixID asc", con);//Your data query goes here for searching the data
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientLabBillsList.Add(
                    new PatientLabBills
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]).ToString(),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender = dr["Gender"].ToString(),

                    });
            }
            return PatientLabBillsList;

        }
        public DataSet GetPatientLabAppointment(string appID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabAppointment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientLabAppointmentID", appID);

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
        public DataSet GetPatientLabAppointmentDetails(string appID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabAppointmentDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientLabAppointmentID", appID);

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
        public DataSet GetPatientLabForPatientLabWithParameter(int HospitalID, int LocationID, int TestID)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabForPatientLabWithParameter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@TestID", TestID);

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
        #region GetPatientOldLabBills

        public DataSet GetPatientOldLabBills(int HospitalID, int LocationID, int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);

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

        #endregion

        public DataSet GetPatientLab(int HospitalID, int LocationID, int BillsNo)
        {
            SqlParameter[] aParams = new SqlParameter[3];
            try
            {
                Connect();
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand("GetPatientLab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillsNo", BillsNo);
                // cmd.Parameters["@PatientRegNO".Value=ParameterDirection.Output ;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                // ExceptionManager.Publish(ex);
                throw ex;
            }

        }
        public DataSet GetAllFinancialYear()
        {
            Connect();
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    con.Open();
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllFinancialYear", param);
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

        #region GetPatientLabDetails

        public DataSet GetPatientLabDetails(int HospitalID, int LocationID, int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);

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

        #endregion

        #region GetPatientLabOLdBillsDetails

        public DataSet GetPatientLabOLdBillsDetails(int HospitalID, int LocationID, int BillNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabOLdBillsDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);

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

        #endregion
        #region GetPatient

        public DataSet GetPatient(string PatientRegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select Patient.PatientRegNO ,upper(Patient.PatientName) as PatientName ,Patient.PatientRegNO as RegNO, Patient.PatientType as 'PatientType',Patient.FinancialYearID from Patient where Patient.PatientName like '" + PatientRegNO + "%' and Patient.HospitalID = " + HospitalID + " and Patient.LocationID = " + LocationID + " and Patient.RowStatus= 0 group by Patient.PatientRegNO,upper(Patient.PatientName)   ,Patient.PrintRegNO, Patient.PatientType,Patient.FinancialYearID ORDER BY Patient.PatientRegNO Desc", con);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        #endregion

        public DataSet GetPatientLabOPDIPDNo(int OPDIPDID)
        {
            Connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("GetPatientLabOPDIPDNo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@OPDIPDID", OPDIPDID);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        #region GetNxtNo

        public DataSet GetNextPatientRegNO(PatientLabBills obj)
        {
            Connect();
            DataSet ds = new DataSet();
            KeystoneProject.Buisness_Logic.Patient.BL_PatientOPDBill objFin = new Patient.BL_PatientOPDBill();
            try
            {
                SqlCommand cmd = new SqlCommand("GetNextPatientRegNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@FinancialYearID", objFin.GetAllFinancialYear().Tables[1].Rows[0]["FinancialYearID"].ToString());
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

        #endregion

        public DataSet GetPatientForLabBills(int HospitalID, int LocationID, int RegNO,int OPDIPDNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                //GetPatientForLabBillsOPDIPD
                SqlCommand cmd = new SqlCommand("GetPatientForLabBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", RegNO);
               


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




        public DataSet GetPatientForLabBillsOPDIPD( string PatientType, int OPDIPDNo)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                //GetPatientForLabBillsOPDIPD
                SqlCommand cmd = new SqlCommand("GetPatientForLabBillsOPDIPD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@PatientOPDIPDNO", OPDIPDNo);


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

        public DataSet GetPatientPrivilegeDiscount(int HospitalID, int LocationID, int PatientRegNO)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientPrivilegeDiscount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatietRegNo", PatientRegNO);

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

        public DataSet GetPatientLabBillsForTestMaster(PatientLabBills opft)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetPatientLabBillsForTestMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PatientRegNO", opft.DoctorID);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet GetPatientOLDLabBillsNO(int PatientRegNO, string BillType, int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("GetPatientOLDLabBillsNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@BillType", BillType);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return ds;
        }

        public DataSet GetPatientLabBillsForTestMaster(int DocID)
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
                param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                param[2].Value = DocID;
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
        public DataSet GetOutsource(string GetOutSourceLab)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select OutSourceLabID, LabName ,Address,MobileNo from  OutSourceLab  where LabName like '%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  LabName  asc", con);
            //cmd.Parameters.AddWithValue("@GetOutSourceLab", GetOutSourceLab);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet GetUser(string prefix)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select  UserID ,FullName  FROM Users where FullName like ''+@prefix+'%' and  RowStatus=0  order by  FullName asc", con);
            cmd.Parameters.AddWithValue("@prefix", @prefix);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetUser1(string id)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select  UserID ,FullName  FROM Users where UserID like ''+@id+'%' and  RowStatus=0  order by  FullName asc", con);
            cmd.Parameters.AddWithValue("@id", @id);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet CompanyNameBind(string GetOrganization)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select OrganizationID,OrganizationName from   Organization where OrganizationName like '%'  and OrganizationType = 'T.P.A' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + " and RowStatus= 0 order by OrganizationID asc", con);
            //cmd.Parameters.AddWithValue("@GetOrganization", GetOrganization);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet CollectionCentreBind(string GetCollectionCenter)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select CollectionID,CollectionName from CollectionCenter where CollectionName like '%' and RowStatus=0 and LocationID=" + LocationID + " and HospitalID=" + HospitalID + "", con);
            cmd.Parameters.AddWithValue("@GetCollectionCenter", GetCollectionCenter);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetDiscountReason(string GetReason)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select  DiscountReasonID,DiscountReason from  DiscountReason  where DiscountReason like ''+@GetReason+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  DiscountReason asc", con);
            cmd.Parameters.AddWithValue("@GetReason", GetReason);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet GetWardName(string GetWardName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select WardID,WardName from  Ward  where WardName like ''+@GetWardName+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  WardName asc", con);
            cmd.Parameters.AddWithValue("@GetWardName", GetWardName);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable GetRoomNo()
        {
            Connect();
            //SqlCommand cmd = new SqlCommand("select WardID,WardName from  Ward  where WardName like ''+@GetWardName+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  WardName asc", con);
            //cmd.Parameters.AddWithValue("@GetWardName", GetWardName);
            DataTable ds = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select  WardRooms.RoomID,WardRooms.RoomName from Ward left join WardRooms on Ward.WardID=WardRooms.WardID where  WardRooms.RowStatus=0 and WardRooms.HospitalID =" + HospitalID + " and WardRooms.LocationID =" + LocationID + "  order by  RoomName asc", con);
            //con.Close();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable GetBedNo()
        {
            Connect();
            //SqlCommand cmd = new SqlCommand("select WardID,WardName from  Ward  where WardName like ''+@GetWardName+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  WardName asc", con);
            //cmd.Parameters.AddWithValue("@GetWardName", GetWardName);
            DataTable ds = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select WardRoomsDetails.BedID,WardRoomsDetails.BedNO from Ward left join WardRoomsDetails on Ward.WardID=WardRoomsDetails.WardID where  WardRoomsDetails.RowStatus=0 and WardRoomsDetails.HospitalID =" + HospitalID + " and WardRoomsDetails.LocationID =" + LocationID + "  order by  BedNO asc", con);
            //con.Close();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetTest(string filter)
        {
            Connect();
            //SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationName  like  '%'+@filter+'%' and RowStatus=0 and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
            SqlCommand cmd = new SqlCommand("select  TestID,TestName,cast(GeneralCharges as decimal(18,2)) as 'GeneralCharges',CAST(EmergencyCharges as decimal(18,2)) as 'EmergencyCharges' from  TestMaster  where TestName like ''+@filter+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  TestName asc", con);
            cmd.Parameters.AddWithValue("@filter", filter);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetPackage(string filter)
        {
            Connect();
            //SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationName  like  '%'+@filter+'%' and RowStatus=0 and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
            SqlCommand cmd = new SqlCommand("select  distinct Profile.ProfileID as TestID  , Profile.Name as TestName ,GeneralCharges as Rate from Profile  where Profile.Name like ''+@filter+'%' and Profile.RowStatus = 0 and Profile.HospitalID =  " + HospitalID + "   and Profile.LocationID = " + LocationID + " order by  Profile.Name asc", con);
            cmd.Parameters.AddWithValue("@filter", filter);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet GetConsDoctor(string GetConsDoctor)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from  Doctor  where DoctorPrintName like ''+@GetConsDoctor+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and DoctorType = 'Consultant'", con);
            cmd.Parameters.AddWithValue("@GetConsDoctor", GetConsDoctor);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetRefferedDoctor(string GetRefferedDoctor)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select DoctorID,DoctorPrintName from  Doctor  where DoctorPrintName like ''+@GetRefferedDoctor+'%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " and DoctorType = 'Referred'", con);
            cmd.Parameters.AddWithValue("@GetRefferedDoctor", GetRefferedDoctor);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet Fill(string appID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientLabAppointmentID, PatientName from PatientLabAppointment where PatientName like  '%'   and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " ", con);
            //cmd.Parameters.AddWithValue("@appID", appID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public DataSet DeletePatientBills(string PatientRegNO, string BillNo)
        {

            DataSet ds = new DataSet();
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("DeletePatientBills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNO", PatientRegNO);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;


        }

        public List<PatientLabBills> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllPatientSearch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientLabBillsList.Add(
                    new PatientLabBills
                    {
                        PatientRegNo = Convert.ToInt32(dr["PatientRegNO"]).ToString(),
                        PatientName = Convert.ToString(dr["PatientName"]),
                        BillNo = Convert.ToString(dr["BillNo"]),
                        BillDate = Convert.ToDateTime(dr["BillDate"]).ToString("dd-MM-yyyy"),
                        Address = dr["Address"].ToString(),

                        LabNo = Convert.ToInt32(dr["LabNo"].ToString()),


                    });
            }
            return PatientLabBillsList;
        }

        public DataSet GetOutSourceLabTestRate(int OutSourceLabID)
        {

            DataSet ds = new DataSet();
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("GetOutSourceLabTestRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OutSourceLabID", OutSourceLabID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;


        }

        #region Save
        public PatientLabBills Save(Models.Laboratory.PatientLabBills obPatient)
        {
            Connect();
            string billtype="" ;
            try
            {
                int BillNo = 0;
                    List<string> PatientLabDetail = new List<string>();

                    #region Patient
                    string regNO = GetNextPatientRegNO(obPatient).Tables[0].Rows[0]["PatientRegNo"].ToString();
                    if (obPatient.PatientRegNo == regNO)
                    {
                        SqlCommand cmd = new SqlCommand("IUPatient", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd.Parameters.AddWithValue("@LocationID", LocationID);
                        cmd.Parameters.AddWithValue("@PatientRegNO", 0);
                        cmd.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;

                        if (obPatient.PatientType == "")
                        {
                            cmd.Parameters.AddWithValue("@PatientType", "OPD");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PatientType", obPatient.PatientType);
                        }
                        cmd.Parameters.AddWithValue("@PatientName", obPatient.PatientName);
                        cmd.Parameters.AddWithValue("@PFPatientName", obPatient.PrefixName);

                        cmd.Parameters.AddWithValue("@PFirstName", obPatient.PatientName);
                        cmd.Parameters.AddWithValue("@PMiddleName", "");
                        cmd.Parameters.AddWithValue("@PLastName", "");
                        cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                        cmd.Parameters.AddWithValue("@PFGuardianName", "");
                        cmd.Parameters.AddWithValue("@GuardianName", obPatient.GuardianName);

                        cmd.Parameters.AddWithValue("@Gender", obPatient.Gender);
                        cmd.Parameters.AddWithValue("@Age", obPatient.Age);
                        cmd.Parameters.AddWithValue("@AgeType", "Years");
                        cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(obPatient.DateOfBirth));
                        cmd.Parameters.AddWithValue("@Height", "");
                        cmd.Parameters.AddWithValue("@BloodGroup", "");
                        cmd.Parameters.AddWithValue("@BloodPressure", "");
                        cmd.Parameters.AddWithValue("@Source", "");
                        cmd.Parameters.AddWithValue("@Address", obPatient.Address);
                        cmd.Parameters.AddWithValue("@PanCardNo", "");
                        cmd.Parameters.AddWithValue("AadhaarNo", "");
                        cmd.Parameters.AddWithValue("@CityID", obPatient.CityID);
                        cmd.Parameters.AddWithValue("@PinCode", obPatient.PinCode);
                        cmd.Parameters.AddWithValue("@StateID", obPatient.StateID);
                        cmd.Parameters.AddWithValue("@Photo", "");
                        cmd.Parameters.AddWithValue("@CountryID", obPatient.CountryID);
                        cmd.Parameters.AddWithValue("@PhoneNo", obPatient.PhoneNo);
                        cmd.Parameters.AddWithValue("@MobileNo", obPatient.MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", "");
                        cmd.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                        cmd.Parameters.AddWithValue("@PatientCancel", "");
                        cmd.Parameters.AddWithValue("@PatientCancelReason", "");
                        cmd.Parameters.AddWithValue("@CreationID", UserID);
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                        con.Close();
                        con.Open();
                        cmd.ExecuteNonQuery().ToString();
                        obPatient.PatientRegNo = Convert.ToInt32(cmd.Parameters["@PatientRegNO"].Value).ToString();
                        con.Close();

                        #endregion

                        #region PatientOPD

                        SqlCommand cmdPatientOPDDetails = new SqlCommand("IUPatientOPDDetails", con);
                        cmdPatientOPDDetails.CommandType = CommandType.StoredProcedure;
                        cmdPatientOPDDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@LocationID", LocationID);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@PatientOPDNO", 0);
                        cmdPatientOPDDetails.Parameters["@PatientOPDNO"].Direction = ParameterDirection.Output;
                        cmdPatientOPDDetails.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);

                        cmdPatientOPDDetails.Parameters.AddWithValue("@PatientRegistrationDate", DateTime.Now);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@DepartmentID", 0);

                        cmdPatientOPDDetails.Parameters.AddWithValue("@PatientType", obPatient.PatientType);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@TPA_ID", obPatient.OrganizationID);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@DoctorID", obPatient.ConsultantDrID);

                        cmdPatientOPDDetails.Parameters.AddWithValue("@ReferredByDoctorID", obPatient.ReferredByDoctorID);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Weight", "");
                        cmdPatientOPDDetails.Parameters.AddWithValue("@BloodPressure", "");

                        cmdPatientOPDDetails.Parameters.AddWithValue("@Reason", "");
                        cmdPatientOPDDetails.Parameters.AddWithValue("@RegistrationCharges", "");
                        cmdPatientOPDDetails.Parameters.AddWithValue("@ConsultionCharges", "");
                        cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountInPer", 0.00);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountInRS", 0.00);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@TotalAmount", 0.00);

                        cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountReason", obPatient.DiscountReason);
                        //cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remarks);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@CreationID", UserID);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);

                        cmdPatientOPDDetails.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);

                        switch (obPatient.PaymentType)
                        {
                            case "Cheque":
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                                break;
                            case "Debit Card":
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                                break;
                            case "Credit Card":
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                                break;
                            case "E-Money":
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                                break;
                            case "EFT":
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                                break;
                            default:
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Number", "");
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Name", "");
                                cmdPatientOPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                                cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", "");
                                break;
                        }



                        cmdPatientOPDDetails.Parameters.AddWithValue("@Mode", "Add");
                        //con.Close();
                        con.Open();
                        int IUPatientOPDDetails = cmdPatientOPDDetails.ExecuteNonQuery();
                        obPatient.PatientOPDNO = Convert.ToInt32(cmdPatientOPDDetails.Parameters["@PatientOPDNO"].Value).ToString();

                        if (obPatient.OPDIPDID == "")
                        {
                            obPatient.OPDIPDID = obPatient.PatientOPDNO;

                        }
                        con.Close();
                    }
                    #endregion

                    #region PatientBill
                    if (obPatient.PatientRegNo != "")
                    {
                        SqlCommand CmdPatientBill = new SqlCommand("IUPatientBills", con);
                        CmdPatientBill.CommandType = CommandType.StoredProcedure;
                        CmdPatientBill.Parameters.AddWithValue("@HospitalID", HospitalID);
                        CmdPatientBill.Parameters.AddWithValue("@LocationID", LocationID);
                        CmdPatientBill.Parameters.AddWithValue("@BillNo", 0);
                        CmdPatientBill.Parameters["@BillNo"].Direction = ParameterDirection.Output;
                        CmdPatientBill.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);

                        string Dates = Convert.ToDateTime(obPatient.BillDate).ToString("dd-MM-yyyy hh:mm:ss");
                        //string Time=Convert.ToDateTime(obPatient.BillDatetime).ToString("hh:mm:ss");
                        CmdPatientBill.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(Dates));
                        //CmdPatientBill.Parameters.AddWithValue("@BillDate", obPatient.BillDate);
                        CmdPatientBill.Parameters.AddWithValue("@OPDIPDID ", obPatient.OPDIPDID);
                        if (obPatient.PatientType == "OPD")
                        {
                            CmdPatientBill.Parameters.AddWithValue("@BillType", "OPDLabBills");
                            obPatient.BillType = "OPDLabBills";

                        }
                        else
                        {
                            CmdPatientBill.Parameters.AddWithValue("@BillType", "IPDLabBills");
                            obPatient.BillType = "IPDLabBills";

                        }

                        CmdPatientBill.Parameters.AddWithValue("@GrossAmount  ", obPatient.GrossAmount);
                        CmdPatientBill.Parameters.AddWithValue("@TaxPercent ", obPatient.ServiceTax);
                        CmdPatientBill.Parameters.AddWithValue("@TaxAmount", obPatient.TaxAmount);
                        CmdPatientBill.Parameters.AddWithValue("@Commisson ", "");
                        CmdPatientBill.Parameters.AddWithValue("@ReffCommission ", "");
                        CmdPatientBill.Parameters.AddWithValue("@TotalAmount", obPatient.TotalAmount);
                        CmdPatientBill.Parameters.AddWithValue("@DiscountPercent", obPatient.Discount);
                        CmdPatientBill.Parameters.AddWithValue("@DiscountAmount ", obPatient.DiscountAmount);
                        CmdPatientBill.Parameters.AddWithValue("@DiscountReason", obPatient.DiscountReason);
                        CmdPatientBill.Parameters.AddWithValue("@NetPayableAmount", obPatient.NetPayableAmount);
                        CmdPatientBill.Parameters.AddWithValue("@BalanceAmount  ", obPatient.BalanceAmount);
                        CmdPatientBill.Parameters.AddWithValue("@PreBalanceAmount", obPatient.PreBalanceAmount);
                        CmdPatientBill.Parameters.AddWithValue("@IsPaid", obPatient.IsPaid);
                        CmdPatientBill.Parameters.AddWithValue("@PaidAmount ", obPatient.PaidAmount);
                        CmdPatientBill.Parameters.AddWithValue("@DipositAmount", obPatient.Deposit);


                        CmdPatientBill.Parameters.AddWithValue("@SancationAmount", "");
                        CmdPatientBill.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                        CmdPatientBill.Parameters.AddWithValue("@CreationID", UserID);
                        CmdPatientBill.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);

                    //CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                    //CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                    //CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.Date));
                    //CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remarks);
                    switch (obPatient.PaymentType)
                    {
                        case "Cheque":
                            CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                            CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                            CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                            break;
                        case "Debit Card":
                            CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                            CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                            CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                            break;
                        case "Credit Card":
                            CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                            CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                            CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                            break;
                        case "E-Money":
                            CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                            CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                            CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                            break;
                        case "EFT":
                            CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                            CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                            CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                            break;
                        default:
                            CmdPatientBill.Parameters.AddWithValue("@Number", "");
                            CmdPatientBill.Parameters.AddWithValue("@Name", "");
                            CmdPatientBill.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                            CmdPatientBill.Parameters.AddWithValue("@Remarks", "");
                            break;
                    }




                    CmdPatientBill.Parameters.AddWithValue("@Mode", "Add");
                        //con.Close();
                        con.Open();
                        int IUPatientBill = CmdPatientBill.ExecuteNonQuery();
                        obPatient.BillNo = Convert.ToInt32(CmdPatientBill.Parameters["@BillNo"].Value).ToString();
                        con.Close();
                    }
                    #endregion

                    #region PatienBillsDetails

                    if (Convert.ToInt32(obPatient.BillNo) > 0)
                    {
                        string[] TestID = obPatient.TestID.Split(',');
                        string[] Testname = obPatient.TestName.Split(',');
                        string[] Rate = obPatient.Rate.Split(',');
                        string[] Qty = obPatient.Qty.Split(',');
                        string[] DisSerType = obPatient.DiscountServiceType.Split(',');
                        string[] DisServAmt = obPatient.Discount_Service.Split(',');
                        string[] Servicetype = obPatient.ServiceType.Split(',');




                        for (int row = 0; row < TestID.Length; row++)
                        {
                            decimal Total = Convert.ToDecimal(Qty[row]) * Convert.ToDecimal(Rate[row]);
                            decimal totalAmt = 0;
                            if (DisSerType[row] == "%")
                            {
                                totalAmt = ((Total * Convert.ToDecimal(DisServAmt[row])) / 100);

                            }
                            if (DisSerType[0] == "Rs")
                            {
                                totalAmt = Total - Convert.ToDecimal(DisServAmt[row]);
                            }
                            SqlCommand CmdPatienBillsDetails = new SqlCommand("IUPatientBillsDetails", con);
                            CmdPatienBillsDetails.CommandType = CommandType.StoredProcedure;
                            CmdPatienBillsDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@LocationID", LocationID);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceID", TestID[row]);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceName", Testname[row]);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@UnitID", "");

                            CmdPatienBillsDetails.Parameters.AddWithValue("@Rate", Rate[row]);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@Quantity", Qty[row]);


                            CmdPatienBillsDetails.Parameters.AddWithValue("@TotalAmount", totalAmt);


                            CmdPatienBillsDetails.Parameters.AddWithValue("@Discount", obPatient.Discount);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@Commisson", "");
                            CmdPatienBillsDetails.Parameters.AddWithValue("@ReffCommission", "");
                            if (obPatient.PatientType == "OPD")
                            {
                                CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceType", Servicetype[row]);
                            }
                            else
                            {
                                CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceType", Servicetype[row]);

                            }
                            CmdPatienBillsDetails.Parameters.AddWithValue("@ChargesType", "General");
                            CmdPatienBillsDetails.Parameters.AddWithValue("@DoctorCharges", "");
                            CmdPatienBillsDetails.Parameters.AddWithValue("@Discount_Service", DisServAmt[row]);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@DiscountServiceType", DisSerType[row]);

                            CmdPatienBillsDetails.Parameters.AddWithValue("@HideInBilling", "");
                            CmdPatienBillsDetails.Parameters.AddWithValue("@DoctorID", obPatient.ConsultantDrID);

                            CmdPatienBillsDetails.Parameters.AddWithValue("@CreationID", UserID);
                            CmdPatienBillsDetails.Parameters.AddWithValue("@Mode", "Add");
                            con.Close();
                            con.Open();
                            int IUPatientBillsDetails = CmdPatienBillsDetails.ExecuteNonQuery();
                            //     objc.ProfileID = Convert.ToInt32(CmdPatienBillsDetails.Parameters["@ProfileID"].Value);
                            con.Close();
                        }
                    }
                    #endregion

                    #region PatientLab

                    SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                    cmdPatientLab.CommandType = CommandType.StoredProcedure;
                    cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                    cmdPatientLab.Parameters.AddWithValue("@LabNo", 0);
                    cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                    cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", obPatient.PatientRegNo);
                    cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                    cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", obPatient.OPDIPDID);

                    if (obPatient.PatientType == "OPD")
                    {
                        cmdPatientLab.Parameters.AddWithValue("@LabType", "OPDLab");

                    }
                    else
                    {
                        cmdPatientLab.Parameters.AddWithValue("@LabType", "IPDLab");

                    }
                    cmdPatientLab.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                    cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                    cmdPatientLab.Parameters.AddWithValue("@Mode", "Add");
                    //     con.Close();
                    con.Open();
                    int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                    obPatient.LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                    #endregion

                    #region PatientLabDetails
                    if (obPatient.LabNo > 0)
                    {
                        string[] TestID = obPatient.TestID.Split(',');
                        string[] Testname = obPatient.TestName.Split(',');
                        string[] Rate = obPatient.Rate.Split(',');
                        string[] Qty = obPatient.Qty.Split(',');
                        string[] Date = obPatient.ReportingDate.Split(',');
                        string[] Outid = obPatient.OutSourceID.Split(',');
                        string[] OutLabName = obPatient.LabName.Split(',');
                        string[] samplecoll = obPatient.samplecollection.Split(',');
                        for (int row = 0; row < TestID.Length; row++)
                        {
                            SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                            cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;
                            cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                            cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;
                            //string  BillNo = Convert.ToInt32(cmd.Parameters["@ProvisionalBillNo"].Value);
                            //  int PatientLabDetailID = (int)cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value;
                            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceID", Outid[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceLabName", OutLabName[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@ReportingDate", Convert.ToDateTime(Date[row]));
                            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", TestID[row]);

                            cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");




                            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", obPatient.SampleCollectionBoyID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CollectionCentre", obPatient.CollectionCentreName);
                            cmdPatientLabDetails.Parameters.AddWithValue("@TPAWiseName", obPatient.OrganizationName);
                            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Add");
                            con.Close();
                            con.Open();
                            //  int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                            // obPatient.LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
                            int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                            obPatient.PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value.ToString());
                            PatientLabDetail.Add(obPatient.PatientLabDetailID.ToString());
                            con.Close();
                        }
                    }
                    #endregion


                    #region Parameter

                    string[] TestID1 = obPatient.TestID.Split(',');
                    string[] Testname1 = obPatient.TestName.Split(',');
                    string[] Rate1 = obPatient.Rate.Split(',');
                    string[] Qty1 = obPatient.Qty.Split(',');
                    // string[] PatientLabDetailID1 = (obPatient.PatientLabDetailID);
                    DataSet ds = new DataSet();
                    int TestID2 = 0;
                    int PatientLabDetailID = 0;
                    for (int row = 0; row < TestID1.Length; row++)
                    {


                        TestID2 = Convert.ToInt32(TestID1[row].ToString());

                        BL_PatientLabBills obj = new BL_PatientLabBills();
                        ds = GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);

                        PatientLabDetailID = Convert.ToInt32(PatientLabDetail[row]);

                        // if ((Days > Convert.ToInt32(drTest["DaysFrom"].ToString()) && Days < Convert.ToInt32(drTest["DaysTo"].ToString())) ||( Days < Convert.ToInt32(drTest["DaysTo"].ToString())))
                        foreach (DataRow drParameter in ds.Tables[1].Rows)
                        {
                            //DataView dview = dsLlantas.Tables[0].DefaultView;
                            //dview.Sort = "Fecha desc";
                            string a = drParameter["ParameterID"].ToString();
                            int days = Decimal.ToInt32(Convert.ToDecimal(obPatient.TotalDay));
                            //  string R = obPatient.TotalDay;
                            if (Convert.ToInt32(drParameter["ParameterID"]) == 17)
                            {

                            }
                            DataView dvTest = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                            DataSet dsOnlyParameter = new DataSet();
                            DataTable dt = new DataTable();
                            dt = dvTest.ToTable();
                            dsOnlyParameter.Tables.Add(dt);

                            //DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + Days + " and DaysTo >=" + Days + " and Gender ='" + Gender + "')or  DaysTo <=" + Days + " and Gender ='" + Gender + "'and ParameterID = " + drParameter["ParameterID"].ToString() + "  ", "", DataViewRowState.CurrentRows);
                            // dsOnlyParameter.Tables[0].Rows[0]["DaysTo"] = "50000";
                            // dsOnlyParameter.Tables[0].Rows[1]["DaysTo"] = "50000";
                            DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + days + " and DaysTo >=" + days + " and Gender ='" + obPatient.Gender + "')", "", DataViewRowState.CurrentRows);

                            // DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + Convert.ToInt32( days) + " and DaysTo >=" +  Convert.ToInt32(days) + " and Gender ='" + obPatient.Gender + "')", "", DataViewRowState.CurrentRows);

                            SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                            cmdParamiter.CommandType = CommandType.StoredProcedure;
                            if (dv.Count != 0)
                            {

                                if ((obPatient.Gender.Trim() == dv[0]["Gender"].ToString().Trim()))
                                {
                                    //  DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();


                                    cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmdParamiter.Parameters.AddWithValue("@LabNo", obPatient.LabNo);

                                    cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                    // cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                    // PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
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
                                // string a = drParameter["ParameterID"].ToString();
                                // DataRow drW = dsPatientLabWithParameter.Tables[0].NewRow();
                                cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                //cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                // PatientLabDetailID = (int)cmdParamiter.Parameters["@PatientLabParameterID"].Value;
                                cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", PatientLabDetailID);
                                cmdParamiter.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
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

                            con.Open();

                            cmdParamiter.ExecuteNonQuery();
                            con.Close();
                        }
                    }


                    /*******************************/

                    //SqlCommand cmdpmt = new SqlCommand("RptPatientLabBills", con);
                    //cmdpmt.CommandType = CommandType.StoredProcedure;
                    //cmdpmt.Parameters.AddWithValue("@HospitalID", HospitalID);
                    //cmdpmt.Parameters.AddWithValue("@LocationID", LocationID);
                    //cmdpmt.Parameters.AddWithValue("@BillNO", obPatient.BillNo);
                    //cmdpmt.Parameters.AddWithValue("@BillType", obPatient.BillType);
                    //HttpContext.Current.Session["HospitalID"] = HospitalID;
                    //HttpContext.Current.Session["LocationID"] = LocationID;
                    //HttpContext.Current.Session["PatientLabBillNO"] = obPatient.BillNo;
                    //HttpContext.Current.Session["PatientLabBillType"] = obPatient.BillType;


                    //SqlDataAdapter da = new SqlDataAdapter(cmdpmt);
                    //DataSet dspmt = new DataSet();
                    ////  con.Open();
                    //da.Fill(dspmt);s

                    /********************************/

                    //  obPatient.RptPatientLabBill = dspmt;
                    obPatient.mybool = true;
              

            }
            catch (Exception ex)
            {
                //ex.Message();
                obPatient.mybool = false;
            }
            HttpContext.Current.Session["LabBillNo"] = obPatient.BillNo;
            HttpContext.Current.Session["LabBillType"] = obPatient.BillType;
            return obPatient;
            #endregion
             
        }

        #endregion

        #region Edit
        public bool Edit(Models.Laboratory.PatientLabBills obPatient)
        {
            Connect();
            List<string> patientLabDetail = new List<string>();
            List<string> listTestID = new List<string>();
            int BillNo = 0;
            int PatientLabDetailID = 0;
            string ServiceType = "";
            #region ForAuthorization
            int ForAuthorization = 0;
            // Aurthorise

            try
               {
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj4 = new Buisness_Logic.Master.BL_MasterSetting();
            DataSet dsMasterSetting = new DataSet();
            dsMasterSetting = obj4.GetMasterSetting();
            KeystoneProject.Buisness_Logic.Hospital.BL_Users user = new Buisness_Logic.Hospital.BL_Users();
            DataSet dsAuthorizationRights = new DataSet();
            // List<Users> Search = new List<Users>();
            dsAuthorizationRights = user.GetUsers(UserID);

                con.Open();

                if (dsAuthorizationRights.Tables[0].Rows.Count > 0)
               {
                string chkAurthoriseUserWise = dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();
                obPatient.ForAuthorization = "0";

                if (chkAurthoriseUserWise == "True")
                {
                    //  dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                    obPatient.ForAuthorization = "0";

                }
                if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
                {
                    obPatient.ForAuthorization = "1";
                   
                    SqlCommand cmdIUForAuthorization = new SqlCommand("IUForAuthorization", con);
                    cmdIUForAuthorization.CommandType = CommandType.StoredProcedure;
                    cmdIUForAuthorization.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@LocationID", LocationID);
                    // cmdIUForAuthorization.Parameters.AddWithValue("@AuthorizationID", 0);
                    cmdIUForAuthorization.Parameters.AddWithValue("@UserID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@AuthorationReason", obPatient.BillNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@BillType", "LabBill");
                    cmdIUForAuthorization.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmdIUForAuthorization.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);
                    cmdIUForAuthorization.Parameters.AddWithValue("@CreationID", UserID);
                    cmdIUForAuthorization.Parameters.AddWithValue("@Mode", "Add");
                   
                    cmdIUForAuthorization.ExecuteNonQuery();
                }

            }

            #endregion

            #region BillsModify

            BL_PatientLabBills billModify = new BL_PatientLabBills();
            DataSet dsPatientOldBills = new DataSet();
            dsPatientOldBills = billModify.GetPatientOldLabBills(Convert.ToInt32(HospitalID), Convert.ToInt32(LocationID), Convert.ToInt32(obPatient.BillNo));
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
                cmdBillModify.Parameters.AddWithValue("@ForAuthorization", obPatient.ForAuthorization);

                // cmdBillModify.Parameters.AddWithValue(" @DipositAmount", "0.00");
                cmdBillModify.Parameters.AddWithValue("@IsBillMade", "false");
                cmdBillModify.Parameters.AddWithValue("@CreationID", UserID);
                cmdBillModify.Parameters.AddWithValue("@Mode", "Add");
                //   con.Close();
              

                int IUPatientBillsModify = cmdBillModify.ExecuteNonQuery();
               
            }
            //con.Close();

            #endregion

            #region BillsDetailsModify

            BL_PatientLabBills BillModifyDetails = new BL_PatientLabBills();
            DataSet dsPatientBllDetails = new DataSet();
            dsPatientBllDetails = BillModifyDetails.GetPatientLabOLdBillsDetails(HospitalID, LocationID, Convert.ToInt32(obPatient.BillNo));
            SqlCommand updateauth = new SqlCommand("update PatientBillsDetailsModify set RowStatus = 2 where BillNo = '" + obPatient.BillNo + "' and RowStatus = 0", con);
            int cha = updateauth.ExecuteNonQuery();
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

            #region Patient Edit
            if (obPatient.PatientRegNo == "")
            {


                SqlCommand cmd1 = new SqlCommand("IUPatient", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                cmd1.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);
                //cmd1.Parameters["@PatientRegNO"].Direction = ParameterDirection.Output;

                if (obPatient.PatientType == "")
                {
                    cmd1.Parameters.AddWithValue("@PatientType", "OPD");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@PatientType", obPatient.PatientType);
                }
                cmd1.Parameters.AddWithValue("@PatientName", obPatient.PatientName);
                cmd1.Parameters.AddWithValue("@PFPatientName", obPatient.PrefixName);

                cmd1.Parameters.AddWithValue("@PFirstName", "");
                cmd1.Parameters.AddWithValue("@PMiddleName", "");
                cmd1.Parameters.AddWithValue("@PLastName", "");
                cmd1.Parameters.AddWithValue("@ReferenceCode", obPatient.OPDIPDID);
                cmd1.Parameters.AddWithValue("@PFGuardianName", "");
                cmd1.Parameters.AddWithValue("@GuardianName", obPatient.GuardianName);

                cmd1.Parameters.AddWithValue("@Gender", obPatient.Gender);
                cmd1.Parameters.AddWithValue("@Age", obPatient.Age);
                cmd1.Parameters.AddWithValue("@AgeType", "");
                cmd1.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(obPatient.DateOfBirth));
                cmd1.Parameters.AddWithValue("@Height", "");
                cmd1.Parameters.AddWithValue("@BloodGroup", "");
                cmd1.Parameters.AddWithValue("@Address", obPatient.Address);
                cmd1.Parameters.AddWithValue("@CityID", obPatient.CityID);
                cmd1.Parameters.AddWithValue("@PinCode", obPatient.PinCode);
                cmd1.Parameters.AddWithValue("@StateID", obPatient.StateID);
                cmd1.Parameters.AddWithValue("@Photo", "");
                cmd1.Parameters.AddWithValue("@CountryID", obPatient.CountryID);
                cmd1.Parameters.AddWithValue("@PhoneNo", obPatient.PhoneNo);
                cmd1.Parameters.AddWithValue("@MobileNo", obPatient.MobileNo);
                cmd1.Parameters.AddWithValue("@EmailID", "");
                cmd1.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                cmd1.Parameters.AddWithValue("@Mode", "Edit");

              
                cmd1.ExecuteNonQuery().ToString();
                obPatient.PatientRegNo = Convert.ToInt32(cmd1.Parameters["@PatientRegNO"].Value).ToString();
               
            #endregion

                #region PatientOPD

                SqlCommand cmdPatientOPDDetails = new SqlCommand("IUPatientOPDDetails", con);
                cmdPatientOPDDetails.CommandType = CommandType.StoredProcedure;
                cmdPatientOPDDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdPatientOPDDetails.Parameters.AddWithValue("@LocationID", LocationID);
                cmdPatientOPDDetails.Parameters.AddWithValue("@PatientOPDNO", 0);
                cmdPatientOPDDetails.Parameters["@PatientOPDNO"].Direction = ParameterDirection.Output;
                cmdPatientOPDDetails.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);

                cmdPatientOPDDetails.Parameters.AddWithValue("@PatientRegistrationDate", DateTime.Now);
                cmdPatientOPDDetails.Parameters.AddWithValue("@DepartmentID", 0);

                cmdPatientOPDDetails.Parameters.AddWithValue("@PatientType", obPatient.PatientType);
                cmdPatientOPDDetails.Parameters.AddWithValue("@TPA_ID", 0);
                cmdPatientOPDDetails.Parameters.AddWithValue("@DoctorID", obPatient.ConsultantDrID);

                cmdPatientOPDDetails.Parameters.AddWithValue("@ReferredByDoctorID", obPatient.ReferredByDoctorID);
                cmdPatientOPDDetails.Parameters.AddWithValue("@Weight", "");
                cmdPatientOPDDetails.Parameters.AddWithValue("@BloodPressure", "");

                cmdPatientOPDDetails.Parameters.AddWithValue("@Reason", "");
                cmdPatientOPDDetails.Parameters.AddWithValue("@RegistrationCharges", "");
                cmdPatientOPDDetails.Parameters.AddWithValue("@ConsultionCharges", "");
                cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountInPer", 0.00);
                cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountInRS", 0.00);
                cmdPatientOPDDetails.Parameters.AddWithValue("@TotalAmount", 0.00);
                //cmdPatientOPDDetails.Parameters.AddWithValue("@PaymentType", "");
                //cmdPatientOPDDetails.Parameters.AddWithValue("@Number", "");
                //cmdPatientOPDDetails.Parameters.AddWithValue("@Name", "");
                //cmdPatientOPDDetails.Parameters.AddWithValue("@Date", DateTime.Now);
                cmdPatientOPDDetails.Parameters.AddWithValue("@DiscountReason", obPatient.DiscountReason);
                // cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", "");
                cmdPatientOPDDetails.Parameters.AddWithValue("@CreationID", UserID);
                cmdPatientOPDDetails.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);

                cmdPatientOPDDetails.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);

                switch (obPatient.PaymentType)
                {
                    case "Cheque":
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                        break;
                    case "Debit Card":
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                        break;
                    case "Credit Card":
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                        break;
                    case "E-Money":
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                        break;
                    case "EFT":
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Date", obPatient.PaymentDate);
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", obPatient.Remark);
                        break;
                    default:
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Number", "");
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Name", "");
                        cmdPatientOPDDetails.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                        cmdPatientOPDDetails.Parameters.AddWithValue("@Remark", "");
                        break;
                }

               cmdPatientOPDDetails.Parameters.AddWithValue("@Mode", "Edit");
               
                int IUPatientOPDDetails = cmdPatientOPDDetails.ExecuteNonQuery();
                obPatient.PatientOPDNO = Convert.ToInt32(cmdPatientOPDDetails.Parameters["@PatientOPDNO"].Value).ToString();
                
            }
                #endregion

                #region PatientBill


               
                SqlCommand cmd = new SqlCommand("[IUPatientBillsOPD]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);
                cmd.Parameters.AddWithValue("@OPDIPDID", obPatient.OPDIPDID);

                if (obPatient.PatientType == "OPD")
                {
                    cmd.Parameters.AddWithValue("@BillType", "OPDLabBills");

                }
                else
                {
                    cmd.Parameters.AddWithValue("@BillType", "IPDLabBills");

                }

                cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obPatient.BillDate));
              
                cmd.Parameters.AddWithValue("@GrossAmount", obPatient.GrossAmount);
                cmd.Parameters.AddWithValue("@TaxAmount", obPatient.TaxAmount);
                cmd.Parameters.AddWithValue("@Commisson", "");
                cmd.Parameters.AddWithValue("@ReffCommission", "");
                cmd.Parameters.AddWithValue("@TaxPercent", obPatient.TaxPercent);
                cmd.Parameters.AddWithValue("@TotalAmount", obPatient.TotalAmount);
                cmd.Parameters.AddWithValue("@DiscountPercent", obPatient.Discount);
                cmd.Parameters.AddWithValue("@DiscountAmount", obPatient.DiscountAmount);
                cmd.Parameters.AddWithValue("@DiscountReason", obPatient.DiscountReason);
                cmd.Parameters.AddWithValue("@NetPayableAmount", obPatient.NetPayableAmount);
                cmd.Parameters.AddWithValue("@BalanceAmount", obPatient.BalanceAmount);
                cmd.Parameters.AddWithValue("@PreBalanceAmount", obPatient.PreBalanceAmount);
                decimal BAmt = Convert.ToDecimal(obPatient.BalanceAmount);
                if (BAmt <= 0) // ucPatientOPDBills1.txtBalAmount.Text.Trim()) <= 0)
                {
                    cmd.Parameters.AddWithValue("@IsPaid", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsPaid", 1);

                }
                cmd.Parameters.AddWithValue("@PaidAmount", obPatient.PaidAmount);
                cmd.Parameters.AddWithValue("@DipositAmount", "0.00");
                cmd.Parameters.AddWithValue("@IsBillMade", 0);
                cmd.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);
                switch (obPatient.PaymentType)
                {

                    case "Cheque":
                        cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        cmd.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "Debit Card":
                        cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        cmd.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "Credit Card":
                        cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        cmd.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "E-Money":
                        cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        cmd.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "EFT":
                        cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                        cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        cmd.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@Number", "");
                        cmd.Parameters.AddWithValue("@Name", "");
                        cmd.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                        cmd.Parameters.AddWithValue("@Remarks", "");
                        break;
                }


                //cmd.Parameters.AddWithValue("@Number", obPatient.Number);
                //cmd.Parameters.AddWithValue("@Name", obPatient.Name);
                //cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.Date));
                //cmd.Parameters.AddWithValue("@Remarks", obPatient.Remarks);
                cmd.Parameters.AddWithValue("@SancationAmount", "");
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                cmd.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                int i = cmd.ExecuteNonQuery();

                BillNo = Convert.ToInt32(cmd.Parameters["@BillNo"].Value);
                SqlCommand update = new SqlCommand("update PatientBillsDetails set RowStatus = 2 where BillNo = '" + BillNo + "' and RowStatus=0", con);
                update.ExecuteNonQuery();

                if (obPatient.PatientRegNo != "")
                {


                    SqlCommand CmdPatientBill = new SqlCommand("IUPatientBills", con);
                CmdPatientBill.CommandType = CommandType.StoredProcedure;
                CmdPatientBill.Parameters.AddWithValue("@HospitalID", HospitalID);
                CmdPatientBill.Parameters.AddWithValue("@LocationID", LocationID);
                CmdPatientBill.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                //   CmdPatientBill.Parameters["@BillNo"].Direction = ParameterDirection.Output;
                CmdPatientBill.Parameters.AddWithValue("@PatientRegNO", obPatient.PatientRegNo);
                CmdPatientBill.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(obPatient.BillDate));
                CmdPatientBill.Parameters.AddWithValue("@OPDIPDID ", obPatient.OPDIPDID);
                if (obPatient.PatientType == "OPD")
                {
                    CmdPatientBill.Parameters.AddWithValue("@BillType", "OPDLabBills");

                }
                else
                {
                    CmdPatientBill.Parameters.AddWithValue("@BillType", "IPDLabBills");

                }

                CmdPatientBill.Parameters.AddWithValue("@GrossAmount  ", obPatient.GrossAmount);
                CmdPatientBill.Parameters.AddWithValue("@TaxPercent ", obPatient.TaxPercent);
                CmdPatientBill.Parameters.AddWithValue("@TaxAmount", obPatient.TaxAmount);
                CmdPatientBill.Parameters.AddWithValue("@Commisson ", "");
                CmdPatientBill.Parameters.AddWithValue("@ReffCommission ", "");
                CmdPatientBill.Parameters.AddWithValue("@TotalAmount", obPatient.TotalAmount);
                CmdPatientBill.Parameters.AddWithValue("@DiscountPercent", obPatient.Discount);
                CmdPatientBill.Parameters.AddWithValue("@DiscountAmount ", obPatient.DiscountAmount);
                CmdPatientBill.Parameters.AddWithValue("@DiscountReason", obPatient.DiscountReason);
                CmdPatientBill.Parameters.AddWithValue("@NetPayableAmount", obPatient.NetPayableAmount);
                CmdPatientBill.Parameters.AddWithValue("@BalanceAmount  ", obPatient.BalanceAmount);
                CmdPatientBill.Parameters.AddWithValue("@PreBalanceAmount", obPatient.PreBalanceAmount);
                CmdPatientBill.Parameters.AddWithValue("@IsPaid", obPatient.IsPaid);
                CmdPatientBill.Parameters.AddWithValue("@PaidAmount ", obPatient.PaidAmount);
                CmdPatientBill.Parameters.AddWithValue("@DipositAmount", "");
                // CmdPatientBill.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);
                //CmdPatientBill.Parameters.AddWithValue("@Number", "");
                //CmdPatientBill.Parameters.AddWithValue("@Name", "");
                //CmdPatientBill.Parameters.AddWithValue("@Date", "");
                //CmdPatientBill.Parameters.AddWithValue("@Remarks", "");
                CmdPatientBill.Parameters.AddWithValue("@PaymentType", obPatient.PaymentType);
                // obPatient.PaymentDate = obPatient.PaymentDate.ToString();

                switch (obPatient.PaymentType)
                {

                    case "Cheque":
                        CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                        CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                        CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "Debit Card":
                        CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                        CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                        CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "Credit Card":
                        CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                        CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                        CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "E-Money":
                        CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                        CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                        CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    case "EFT":
                        CmdPatientBill.Parameters.AddWithValue("@Number", obPatient.Number);
                        CmdPatientBill.Parameters.AddWithValue("@Name", obPatient.Name);
                        CmdPatientBill.Parameters.AddWithValue("@Date", Convert.ToDateTime(obPatient.PaymentDate));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", obPatient.Remark);
                        break;
                    default:
                        CmdPatientBill.Parameters.AddWithValue("@Number", "");
                        CmdPatientBill.Parameters.AddWithValue("@Name", "");
                        CmdPatientBill.Parameters.Add(new SqlParameter("@Date", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
                        CmdPatientBill.Parameters.AddWithValue("@Remarks", "");
                        break;
                }


                CmdPatientBill.Parameters.AddWithValue("@SancationAmount", "");
                CmdPatientBill.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                    CmdPatientBill.Parameters.AddWithValue("@ForAuthorization", obPatient.ForAuthorization);
                    CmdPatientBill.Parameters.AddWithValue("@CreationID", UserID);
                CmdPatientBill.Parameters.AddWithValue("@Mode", "Edit");
                
                int IUPatientBill = CmdPatientBill.ExecuteNonQuery();
                obPatient.BillNo = Convert.ToInt32(CmdPatientBill.Parameters["@BillNo"].Value).ToString();
               
            }
            #endregion

            #region PatienBillsDetails

            if (Convert.ToInt32(obPatient.BillNo) > 0)
            {
                string[] TestID = obPatient.TestID.Split(',');
                string[] Testname = obPatient.TestName.Split(',');
                string[] Rate = obPatient.Rate.Split(',');
                string[] Qty = obPatient.Qty.Split(',');
                string[] DisSerType = obPatient.DiscountServiceType.Split(',');
                string[] DisServAmt = obPatient.Discount_Service.Split(',');
                string[] Servicetype = obPatient.ServiceType.Split(',');

                for (int row = 0; row < TestID.Length; row++)
                {
                    decimal Total = Convert.ToDecimal(Qty[row]) * Convert.ToDecimal(Rate[row]);
                    decimal totalAmt = 0;
                    if (DisSerType[row] == "%")
                    {
                        totalAmt = ((Total * Convert.ToDecimal(DisServAmt[row])) / 100);

                    }
                    if (DisSerType[0] == "Rs")
                    {
                        totalAmt = Total - Convert.ToDecimal(DisServAmt[row]);
                    }


                    // svc_array[i].Total = totalAmt.ToString();//total_a[i].ToString();



                    SqlCommand CmdPatienBillsDetails = new SqlCommand("IUPatientBillsDetails", con);
                    CmdPatienBillsDetails.CommandType = CommandType.StoredProcedure;
                    CmdPatienBillsDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@LocationID", LocationID);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@BillNo", obPatient.BillNo);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceID", TestID[row]);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceName", Testname[row]);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@UnitID", "");
                    CmdPatienBillsDetails.Parameters.AddWithValue("@ChargesType", "");
                    CmdPatienBillsDetails.Parameters.AddWithValue("@Rate", Rate[row]);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@Quantity", Qty[row]);


                    CmdPatienBillsDetails.Parameters.AddWithValue("@TotalAmount", totalAmt);


                    CmdPatienBillsDetails.Parameters.AddWithValue("@Discount", obPatient.Discount);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@Discount_Service", DisServAmt[row]);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@DiscountServiceType", DisSerType[row]);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@Commisson", "");
                    CmdPatienBillsDetails.Parameters.AddWithValue("@ReffCommission", "");
                    if (obPatient.PatientType == "OPD")
                    {
                        CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceType", Servicetype[row]);
                    }
                    else
                    {
                        CmdPatienBillsDetails.Parameters.AddWithValue("@ServiceType", Servicetype[row]);

                    }
                    CmdPatienBillsDetails.Parameters.AddWithValue("@HideInBilling", "");
                    CmdPatienBillsDetails.Parameters.AddWithValue("@DoctorID", obPatient.ConsultantDrID);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@ForAuthorization", obPatient.ForAuthorization[row]);

                        CmdPatienBillsDetails.Parameters.AddWithValue("@CreationID", UserID);
                    CmdPatienBillsDetails.Parameters.AddWithValue("@Mode", "Add");
                  
                    int IUPatientBillsDetails = CmdPatienBillsDetails.ExecuteNonQuery();
                    //     objc.ProfileID = Convert.ToInt32(CmdPatienBillsDetails.Parameters["@ProfileID"].Value);
                  
                }
            }

            DataSet dsPatientLab = GetPatientLab(HospitalID, LocationID, Convert.ToInt32(obPatient.BillNo));
            DataSet dsPatientLabforedit = new DataSet();
            dsPatientLabforedit = GetPatientLabDetails(HospitalID, LocationID, Convert.ToInt32(obPatient.BillNo));
                #endregion
                con.Open();
                #region PatientLab
                foreach (DataRow dr3 in dsPatientLab.Tables[0].Rows)
            {
                    
                SqlCommand cmdPatientLab = new SqlCommand("IUPatientLab", con);
                cmdPatientLab.CommandType = CommandType.StoredProcedure;
                cmdPatientLab.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmdPatientLab.Parameters.AddWithValue("@LocationID", LocationID);
                cmdPatientLab.Parameters.AddWithValue("@LabNo", dr3["LabNo"]);
                //  cmdPatientLab.Parameters["@LabNo"].Direction = ParameterDirection.Output;
                cmdPatientLab.Parameters.AddWithValue("@FinancialYearID", obPatient.FinancialYearID);
                cmdPatientLab.Parameters.AddWithValue("@PatientRegNo", dr3["PatientRegNo"]);
                cmdPatientLab.Parameters.AddWithValue("@OPDIPDID", dr3["OPDIPDID"]);
                cmdPatientLab.Parameters.AddWithValue("@LabType", dr3["LabType"]);
                cmdPatientLab.Parameters.AddWithValue("@BillNo", dr3["BillNo"]);
                cmdPatientLab.Parameters.AddWithValue("@CreationID", UserID);
                cmdPatientLab.Parameters.AddWithValue("@Mode", "Edit");
               
                int IUPatientLab = cmdPatientLab.ExecuteNonQuery();
                obPatient.LabNo = Convert.ToInt32(cmdPatientLab.Parameters["@LabNo"].Value);
            #endregion

                #region PatientLabDetails

                string[] TestID11 = obPatient.TestID.Split(',');
                string[] Testname11 = obPatient.TestName.Split(',');
                string[] Rate11 = obPatient.Rate.Split(',');
                string[] Qty11 = obPatient.Qty.Split(',');
                string[] Date = obPatient.ReportingDate.Split(',');
                string[] Outid = obPatient.OutSourceID.Split(',');
                string[] OutLabName = obPatient.LabName.Split(',');
                //    samplecoll11;
                List<string> samplecon = new List<string>();
                if (obPatient.samplecollection != null)
                {
                    string[] samplecoll11 = obPatient.samplecollection.Split(',');
                    for (int n = 0; n < samplecoll11.Length; n++)
                    {
                        samplecon.Add(samplecoll11[n]);
                    }
                }

                string[] Mode11 = obPatient.Mode.Split(',');
                int p = 0;
                if (dsPatientLabforedit.Tables[0].Rows.Count > 0)
                {
                    // string[] samplecoll = obPatient.samplecollection.Split(',');
                    for (int row = 0; row < TestID11.Length; row++)
                    {
                        SqlCommand cmdPatientLabDetails = new SqlCommand("IUPatientLabDetails", con);
                        cmdPatientLabDetails.CommandType = CommandType.StoredProcedure;

                        if (Mode11[row] == "Add")
                        {
                            cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", 0);
                            cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Direction = ParameterDirection.Output;

                            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceID", Outid[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceLabName", OutLabName[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@ReportingDate", Convert.ToDateTime(Date[row]));
                            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", TestID11[row]);


                            //else
                            //{
                            


                                    cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");
                              
                            
                            // }

                            //cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "BILLING");
                            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", obPatient.SampleCollectionBoyID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CollectionCentre", obPatient.CollectionCentreName);
                            cmdPatientLabDetails.Parameters.AddWithValue("@TPAWiseName", obPatient.OrganizationName);
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
                            cmdPatientLabDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@LocationID", LocationID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PatientLabDetailID", dsPatientLabforedit.Tables[0].Rows[p]["PatientLabDetailID"].ToString());
                            cmdPatientLabDetails.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceID", Outid[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@OutSourceLabName", OutLabName[row]);
                            cmdPatientLabDetails.Parameters.AddWithValue("@ReportingDate", Convert.ToDateTime(Date[row]));
                            cmdPatientLabDetails.Parameters.AddWithValue("@ProfileID", "");
                            cmdPatientLabDetails.Parameters.AddWithValue("@TestID", dsPatientLabforedit.Tables[0].Rows[p]["TestID"].ToString());

                          

                                    cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");
                              
                            
                            //if (samplecoll11[row] == "1")
                            // {
                            //cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "SAMPLECOLLECTION");
                            // }
                            // else
                            // {
                            // cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "BILLING");
                            //  }

                            //cmdPatientLabDetails.Parameters.AddWithValue("@TestStatus", "BILLING");
                            cmdPatientLabDetails.Parameters.AddWithValue("@SampleCollectionID", obPatient.SampleCollectionBoyID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@SamleCollectionDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CollectionCentre", obPatient.CollectionCentreName);
                            cmdPatientLabDetails.Parameters.AddWithValue("@TPAWiseName", obPatient.OrganizationName);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@AuthorizedDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteBy", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CompleteDate", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintID ", 0);
                            cmdPatientLabDetails.Parameters.AddWithValue("@PrintDate ", System.DateTime.Now);
                            cmdPatientLabDetails.Parameters.AddWithValue("@CreationID", UserID);
                            cmdPatientLabDetails.Parameters.AddWithValue("@Mode", "Edit");
                            p++;
                        }
                        string s = Mode11[row];
                        
                        int IUPatientLabDetails = cmdPatientLabDetails.ExecuteNonQuery();
                        obPatient.PatientLabDetailID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@PatientLabDetailID"].Value);
                        obPatient.TestID = Convert.ToInt32(cmdPatientLabDetails.Parameters["@TestID"].Value).ToString();

                        patientLabDetail.Add(obPatient.PatientLabDetailID.ToString());
                        listTestID.Add(obPatient.TestID);
                        
                        //  }
                    }
                }

                #endregion

                #region Parameter

                //string[] TestID1 = obPatient.TestID.Split(',');
                string[] Testname1 = obPatient.TestName.Split(',');
                string[] Rate1 = obPatient.Rate.Split(',');
                string[] Qty1 = obPatient.Qty.Split(',');
                string[] Mode = obPatient.Mode.Split(',');

                DataSet ds = new DataSet();
                int TestID2 = 0;
                for (int row = 0; row < TestID11.Length; row++)
                {
                    if (Mode[row] == "Add")
                    {
                        obPatient.PatientLabDetailID = Convert.ToInt32(patientLabDetail[row]);
                        TestID2 = Convert.ToInt32(TestID11[row].ToString());
                        BL_PatientLabBills obj = new BL_PatientLabBills();
                        ds = GetPatientLabForPatientLabWithParameter(HospitalID, LocationID, TestID2);
                        foreach (DataRow drParameter in ds.Tables[1].Rows)
                        {
                            DataView dvTest = new DataView(ds.Tables[0], "ParameterID = " + drParameter["ParameterID"].ToString() + " ", "", DataViewRowState.CurrentRows);

                            DataSet dsOnlyParameter = new DataSet();
                            DataTable dt = new DataTable();
                            dt = dvTest.ToTable();
                            dsOnlyParameter.Tables.Add(dt);

                            DataView dv = new DataView(dsOnlyParameter.Tables[0], " (DaysFrom <= " + obPatient.TotalDay + " and DaysTo >=" + obPatient.TotalDay + " and Gender ='" + obPatient.Gender + "')", "", DataViewRowState.CurrentRows);

                            SqlCommand cmdParamiter = new SqlCommand("IUPatientLabWithParameter", con);
                            cmdParamiter.CommandType = CommandType.StoredProcedure;
                            if (dv.Count != 0)
                            {

                                if ((obPatient.Gender.Trim() == dv[0]["Gender"].ToString().Trim()))
                                {

                                    cmdParamiter.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmdParamiter.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmdParamiter.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
                                    // cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", 0);
                                    cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                    cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                    cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", obPatient.PatientLabDetailID);
                                    // cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                    cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);

                                    cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                    cmdParamiter.Parameters.AddWithValue("@ParameterID", dv[0]["ParameterID"].ToString());
                                    cmdParamiter.Parameters.AddWithValue("@NormalRangeID", dv[0]["NormalRangeID"].ToString());
                                    cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                    cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                    cmdParamiter.Parameters.AddWithValue("@NLH", 0);
                                    cmdParamiter.Parameters.AddWithValue("@ConvLow", dv[0]["ConvLow"].ToString());
                                    cmdParamiter.Parameters.AddWithValue("@ConvHigh", dv[0]["ConvHigh"].ToString());
                                    cmdParamiter.Parameters.AddWithValue("@Remark", 0);
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
                                cmdParamiter.Parameters.AddWithValue("@LabNo", obPatient.LabNo);
                                cmdParamiter.Parameters.AddWithValue("@PatientLabParameterID", 0);
                                cmdParamiter.Parameters["@PatientLabParameterID"].Direction = ParameterDirection.Output;
                                cmdParamiter.Parameters.AddWithValue("@PatientLabDetailID", obPatient.PatientLabDetailID);

                                cmdParamiter.Parameters.AddWithValue("@CompleteBy", "");
                                cmdParamiter.Parameters.AddWithValue("@TestID", TestID2);
                                cmdParamiter.Parameters.AddWithValue("@ParameterID", drParameter["ParameterID"].ToString());
                                cmdParamiter.Parameters.AddWithValue("@NormalRangeID", 0);
                                cmdParamiter.Parameters.AddWithValue("@ResultValue", 0);
                                cmdParamiter.Parameters.AddWithValue("@Status", 0);
                                cmdParamiter.Parameters.AddWithValue("@NLH", "");
                                cmdParamiter.Parameters.AddWithValue("@ConvLow", "");
                                cmdParamiter.Parameters.AddWithValue("@ConvHigh", "");
                                cmdParamiter.Parameters.AddWithValue("@Remark", 0);
                                cmdParamiter.Parameters.AddWithValue("@CreationID", UserID);
                                cmdParamiter.Parameters.AddWithValue("@Mode", "Add");
                                // dsPatientLabWithParameter.Tables[0].Rows.Add(drW);
                            }
                          
                            cmdParamiter.ExecuteNonQuery();

                                con.Close();


                            }
                    }
                }

            }
            }
            catch (Exception exe)
            {
                throw;
            }
            HttpContext.Current.Session["HospitalID"] = HospitalID;
            HttpContext.Current.Session["LocationID"] = LocationID;
            HttpContext.Current.Session["PatientLabBillNO"] = obPatient.BillNo;
            if (obPatient.PatientType == "OPD")
            {
                HttpContext.Current.Session["PatientLabBillType"] = "OPDLabBills";
                HttpContext.Current.Session["LabBillNo"] = obPatient.BillNo;
                HttpContext.Current.Session["LabBillType"] = "OPDLabBills";
            }
            else
            {
                HttpContext.Current.Session["PatientLabBillType"] = "IPDLabBills";
                HttpContext.Current.Session["LabBillNo"] = obPatient.BillNo;
                HttpContext.Current.Session["LabBillType"] = "IPDLabBills";

            }
                #endregion


            return true;
        }

        #endregion

        public DataSet GetAllPatient(string GetAllPatient)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientRegNo, PatientName ,Address, MobileNo,PrintRegNO from patient where PatientName like ''+@GetAllPatient+'%'  and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "", con);
            cmd.Parameters.AddWithValue("@GetAllPatient", GetAllPatient);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }
    }
}