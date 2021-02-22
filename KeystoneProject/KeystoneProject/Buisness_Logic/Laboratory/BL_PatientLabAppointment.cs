using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Controllers.Laboratory;
using KeystoneProject.Models.Laboratory;

namespace KeystoneProject.Buisness_Logic.Laboratory
{
    public class BL_PatientLabAppointment
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        List<PatientLabAppointment> PatientLabAppointmentList = new List<PatientLabAppointment>();
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

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
        public List<PatientLabAppointment> BindPrefixPatient()
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
                PatientLabAppointmentList.Add(
                    new PatientLabAppointment
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]).ToString(),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender = dr["Gender"].ToString(),

                    });
            }
            return PatientLabAppointmentList;

        }

        public bool SaveTest(PatientLabAppointment obj)
        {
            Connect();
            con.Open();
            //string[] Add1 = obj.Mode.Split(',');
            //string[] TestID = obj.TestID.Split(',');
            //string[] Testname = obj.TestName.Split(',');
            //string[] Rate = obj.Rate.Split(',');
            //string[] Quantity = obj.Quantity.Split(',');
            //string[] Total = obj.TotalAmount.Split(',');
            //string[] TestType = obj.TestType.Split(',');
            //string[] PatientLAbDetailID = obj.PatientLabAppointmentDetailID.Split(',');
            //for (int row = 0; row < Add1.Length; row++)
            //{
            //    if (Add1[row] == "Add")
            //    {
            if(obj.PatientLabAppointmentID=="")
            {

            
                    SqlCommand cmd = new SqlCommand("IUPatientLabAppointment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    //if (obj.PatientLabAppointmentID == null)
                    //{


                        cmd.Parameters.AddWithValue("@PatientLabAppointmentID", 0);
                        cmd.Parameters["@PatientLabAppointmentID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@PatientLabAppointmentID", obj.PatientLabAppointmentID);
                    //    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    //}
                    cmd.Parameters.AddWithValue("@PatientPrefixName", obj.PrefixName);
                    cmd.Parameters.AddWithValue("@PatientName", obj.PatientName);
                    cmd.Parameters.AddWithValue("@GuardianName", obj.GuardianName);
                    cmd.Parameters.AddWithValue("@Age", obj.Year);
                    cmd.Parameters.AddWithValue("@AgeType", "Year");
                    cmd.Parameters.AddWithValue("@DateOfBirth", obj.DOB);
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                    cmd.Parameters.AddWithValue("@ConsultantDr", obj.ConsultantDr);
                    cmd.Parameters.AddWithValue("@ConsultantDrID", obj.ConsultantDrID);
                    cmd.Parameters.AddWithValue("@RefferredDr", obj.RefferedDr);
                    cmd.Parameters.AddWithValue("@RefferredDrID", obj.RefferedDrID);
                    cmd.Parameters.AddWithValue("@SampleCollectedBy", obj.SampleCollectionBy);

                    cmd.Parameters.AddWithValue("@CollectionCentre", obj.CollectionCentreName);
                    cmd.Parameters.AddWithValue("@PatientLabAppointmentDate", obj.AppointmentDate);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //con.Open();
                    int i = cmd.ExecuteNonQuery();
                    obj.PatientLabAppointmentID = (cmd.Parameters["@PatientLabAppointmentID"].Value).ToString();
                    con.Close();
                    if (i > 0)
                    {
                        string[] TestID = obj.TestID.Split(',');
                        string[] Testname = obj.TestName.Split(',');
                        string[] Rate = obj.Rate.Split(',');
                        string[] Quantity = obj.Quantity.Split(',');
                        string[] Total = obj.TotalAmount.Split(',');
                        string[] TestType = obj.TestType.Split(',');
                        for (int row2 = 0; row2 < TestID.Length; row2++)
                        {
                            SqlCommand cmd1 = new SqlCommand("IUPatientLabAppointmentDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@PatientLabAppointmentDetailID", 0);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                            
                            cmd1.Parameters.AddWithValue("@PatientLabAppointmentID", obj.PatientLabAppointmentID);
                            cmd1.Parameters.AddWithValue("@TestID", TestID[row2]);
                            cmd1.Parameters.AddWithValue("@TestName", Testname[row2]);
                            cmd1.Parameters.AddWithValue("@Rate", Rate[row2]);
                            cmd1.Parameters.AddWithValue("@Quantity", Quantity[row2]);
                            cmd1.Parameters.AddWithValue("@TotalAmount", Total[row2]);
                            cmd1.Parameters.AddWithValue("@TestType", TestType[row2]);
                            cmd1.Parameters.AddWithValue("@TPAWiseCollect", obj.OrganizationName);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            con.Open();
                            int j = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("IUPatientLabAppointment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);

                    cmd.Parameters.AddWithValue("@PatientLabAppointmentID", obj.PatientLabAppointmentID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@PatientPrefixName", obj.PrefixName);
                    cmd.Parameters.AddWithValue("@PatientName", obj.PatientName);
                    cmd.Parameters.AddWithValue("@GuardianName", obj.GuardianName);
                    cmd.Parameters.AddWithValue("@Age", obj.Year);
                    cmd.Parameters.AddWithValue("@AgeType", "Year");
                    cmd.Parameters.AddWithValue("@DateOfBirth", obj.DOB);
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                    cmd.Parameters.AddWithValue("@ConsultantDr", obj.ConsultantDr);
                    cmd.Parameters.AddWithValue("@ConsultantDrID", obj.ConsultantDrID);
                    cmd.Parameters.AddWithValue("@RefferredDr", obj.RefferedDr);
                    cmd.Parameters.AddWithValue("@RefferredDrID", obj.RefferedDrID);
                    cmd.Parameters.AddWithValue("@SampleCollectedBy", obj.SampleCollectionBy);
                    cmd.Parameters.AddWithValue("@CollectionCentre", obj.CollectionCentreName);
                    cmd.Parameters.AddWithValue("@PatientLabAppointmentDate", obj.AppointmentDate);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                   
                    int i = cmd.ExecuteNonQuery();
                    obj.PatientLabAppointmentID = (cmd.Parameters["@PatientLabAppointmentID"].Value).ToString();
                    con.Close();

                    string[] TestID = obj.TestID.Split(',');
                    string[] Testname = obj.TestName.Split(',');
                    string[] Rate = obj.Rate.Split(',');
                    string[] Quantity = obj.Quantity.Split(',');
                    string[] Total = obj.TotalAmount.Split(',');
                    string[] TestType = obj.TestType.Split(',');
                    string[] PatientLAbDetailID = obj.PatientLabAppointmentDetailID.Split(',');
             
                    for (int row1 = 0; row1 < TestID.Length; row1++)
                    {
                        SqlCommand cmd1 = new SqlCommand("IUPatientLabAppointmentDetails", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                        if (PatientLAbDetailID[row1] == "")
                        {
                            cmd1.Parameters.AddWithValue("@PatientLabAppointmentDetailID", 0);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@PatientLabAppointmentDetailID", PatientLAbDetailID[row1]);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");

                        }
                        cmd1.Parameters.AddWithValue("@PatientLabAppointmentID", obj.PatientLabAppointmentID);
                        cmd1.Parameters.AddWithValue("@TestID", TestID[row1]);
                        cmd1.Parameters.AddWithValue("@TestName", Testname[row1]);
                        cmd1.Parameters.AddWithValue("@Rate", Rate[row1]);
                        cmd1.Parameters.AddWithValue("@Quantity", Quantity[row1]);
                        cmd1.Parameters.AddWithValue("@TotalAmount", Total[row1]);
                        cmd1.Parameters.AddWithValue("@TPAWiseCollect", obj.OrganizationName);
                        cmd1.Parameters.AddWithValue("@TestType", TestType[row1]);
                        cmd1.Parameters.AddWithValue("@CreationID", UserID);
                        con.Open();
                        int j = cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
           // }
            return true;
        }

        public DataSet GetNextAppointentID(PatientLabAppointment obj)
        {
            Connect();
            DataSet ds = new DataSet();
         
            try
            {
                SqlCommand cmd = new SqlCommand("GetNextAppointentID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
           
                cmd.Parameters.AddWithValue("@PatientLabAppointmentID", 0);
               
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception )
            {
                return ds;
            }
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

        public DataSet GetPatient(string GetPatient)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("select PatientLabAppointmentID,PatientName,PatientLabAppointmentID as AppointmentID , MobileNo,DateOfBirth,Address,Gender  from PatientLabAppointment  where  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  PatientName asc , PatientLabAppointmentID desc", con);//Your data query goes here for searching the data
        
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

        public DataSet GetTest(string filter)
        {
            Connect();
            //SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationName  like  '%'+@filter+'%' and RowStatus=0 and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
            SqlCommand cmd = new SqlCommand("select  TestID,TestName,GeneralCharges,EmergencyCharges from  TestMaster  where TestName like ''+@filter+'%' and  RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  order by  TestName asc", con);
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

        public DataSet GetAllPatientLabAppointment()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllPatientLabAppointment", con);

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

        public DataSet GetPatientLabAppointmentDetails(string id)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientLabAppointmentDetails", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@PatientLabAppointmentID", id);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }
        public DataSet GetPatientLabAppointment(string id)
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientLabAppointment", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@PatientLabAppointmentID", id);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet DeletePatientLabAppointment(string PatientLabAppointmentID)
        {

            DataSet ds = new DataSet();
            try
            {

                Connect();
                SqlCommand cmd = new SqlCommand("DeletePatientLabAppointment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientLabAppointmentID", PatientLabAppointmentID);
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
    }
}