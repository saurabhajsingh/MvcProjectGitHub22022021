using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using KeystoneProject.Models.Master;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_Doctor
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);

        }

        public DataSet GetNextDoctorIDNO()
        {
            Connect();
            DataSet ds = new DataSet();

            try
            {

                SqlCommand cmd = new SqlCommand("GetNextDoctorIDNO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
               
                cmd.Parameters.AddWithValue("@DoctorID", 0);
             
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

        public bool Save(Doctor obj)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.DoctorID == 0)
            {
                cmd.Parameters.AddWithValue("@DoctorID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            cmd.Parameters.AddWithValue("@DoctorType", obj.DoctorType);
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@DoctorFName", obj.DoctorFName);
            if (obj.DoctorLName == null)
            {
                cmd.Parameters.AddWithValue("@DoctorLName","");
            }
            else
            {
                cmd.Parameters.AddWithValue("@DoctorLName", obj.DoctorLName);
            }
           
            cmd.Parameters.AddWithValue("@DoctorPrintName", obj.DoctorPrintName);

            if (Convert.ToString(obj.RegNo) == null)
                cmd.Parameters.AddWithValue("@RegNo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@RegNo", obj.RegNo);

            if (obj.DateOfBirth == null)
                // cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                cmd.Parameters.Add(new SqlParameter("@DateOfBirth", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));

            else
                cmd.Parameters.AddWithValue("@DateofBirth", obj.DateOfBirth);

            cmd.Parameters.AddWithValue("@Gender", obj.Gender.ToUpper());

            if (obj.DepartmentID == null)
            {
                cmd.Parameters.AddWithValue("@DepartmentID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DepartmentID", obj.DepartmentID);
            }
            if (obj.QualifictionID == null)
            {
                cmd.Parameters.AddWithValue("QualificationID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@QualificationID", obj.QualifictionID);
            }
            if (obj.SpecializationID == null)
            {
                cmd.Parameters.AddWithValue("@SpecializationID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SpecializationID", obj.SpecializationID);
            }
            cmd.Parameters.AddWithValue("@CategoryID", 1);

            if (obj.DateOfJoining == null)
                //  cmd.Parameters.AddWithValue("@DateOfJoining", DBNull.Value);
                cmd.Parameters.Add(new SqlParameter("@DateOfJoining", DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy hh:mm:ss", null)));
            else
                cmd.Parameters.AddWithValue("@DateOfJoining", obj.DateOfJoining);

            if (obj.UploadDocuments == null)
                // cmd.Parameters.AddWithValue("@DateOfLeaving", DBNull.Value);
                cmd.Parameters.AddWithValue("@UploadDocuments", "");
            else
                cmd.Parameters.AddWithValue("@UploadDocuments", obj.UploadDocuments);

            cmd.Parameters.AddWithValue("@CounsultancyFees", obj.CounsultancyFees);
            cmd.Parameters.AddWithValue("@CounsultancyDuration", obj.CounsultancyDuration);
            cmd.Parameters.AddWithValue("@RenewalFee", obj.RenewalFee);
            cmd.Parameters.AddWithValue("@RenewalDuration", obj.RenewalDuration);
            cmd.Parameters.AddWithValue("@Commission", obj.Commission);
            cmd.Parameters.AddWithValue("@CommissionType", "");
            cmd.Parameters.AddWithValue("@ConsultancyLimit", obj.ConsultancyLimit);

            if (obj.DoctorImage == null)
                cmd.Parameters.AddWithValue("@DoctorImage", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@DoctorImage", obj.DoctorImage);

            if (obj.PermanentAddress == null)
                cmd.Parameters.AddWithValue("@PermanentAddress", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PermanentAddress", obj.PermanentAddress);

            if (obj.TelephoneNo == null)
                cmd.Parameters.AddWithValue("@TelephoneNo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@TelephoneNo", obj.TelephoneNo);

            if (obj.FaxNo == null)
                cmd.Parameters.AddWithValue("@FaxNo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);

            if (obj.MobileNo == null)
                cmd.Parameters.AddWithValue("@MobileNo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);

            if (obj.EmailId == null)
                cmd.Parameters.AddWithValue("@EmailId", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@EmailId", obj.EmailId);

            if (obj.LevingDate == null)
                cmd.Parameters.AddWithValue("@LevingDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@LevingDate", obj.LevingDate);

            if (obj.UploadSignature == null)
                cmd.Parameters.AddWithValue("@UploadSignature", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@UploadSignature", obj.UploadSignature);

            cmd.Parameters.AddWithValue("@CreationID", UserID);

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

        public DataSet GetDepartment(string GetDepartment)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select DepartmentID,DepartmentName from Department where DepartmentName  like ''+@GetDepartment+'%' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            cmd.Parameters.AddWithValue("@GetDepartment", GetDepartment);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet GetQualification(string GetQualifiction)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select qualifictionID,QualifictionName from Qualifiction where QualifictionName like ''+@GetQualifiction+'%' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "" , con);
            cmd.Parameters.AddWithValue("@GetQualifiction", GetQualifiction);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet GetSpecialization(string GetSpecialization)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select specializationid, specializationname from Specialization where SpecializationName like ''+@Specialization+'%' and RowStatus = 0 and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            cmd.Parameters.AddWithValue("@Specialization", GetSpecialization);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public bool CheckDoctor(int DoctorID,string DoctorPrintName, string DoctorType)
        {
            string NameExists;
            Connect();
            SqlCommand checkdoctor = new SqlCommand("CheckDoctor", con);
            checkdoctor.CommandType = CommandType.StoredProcedure;
            checkdoctor.Parameters.AddWithValue("@HospitalID", HospitalID);
            checkdoctor.Parameters.AddWithValue("@LocationID", LocationID);
            checkdoctor.Parameters.AddWithValue("@DoctorID", DoctorID);
            checkdoctor.Parameters.AddWithValue("@DoctorPrintName", DoctorPrintName);
            checkdoctor.Parameters.AddWithValue("@DoctorType", DoctorType);
            checkdoctor.Parameters.Add("@NameExists", SqlDbType.NVarChar, 50);
            checkdoctor.Parameters["@NameExists"].Direction = ParameterDirection.Output;
            con.Open();
            int i = checkdoctor.ExecuteNonQuery();
           // Table = (string)cmd.Parameters["@NameExists"].Value;
            NameExists = (string)checkdoctor.Parameters["@NameExists"].Value;
            //  con.Close();
            if (Convert.ToInt32(NameExists) == 1)
               // if (i >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DataSet CheckDoctorPatientReg(string RegNo, string DoctorType)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select RegNo, DoctorID,DoctorFName,DoctorPrintName,DoctorType from Doctor where RegNo = '" + RegNo + "' and DoctorType = '"+ DoctorType + "' and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
           
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
     
        public DataSet SelectAllData()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllDoctor", con);
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


        public DataSet GetDoctor(int DoctorID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetDoctor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
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

                throw;
            }
        }

        public string DeleteDoctor(int DoctorID)
        {
            string Table = string.Empty;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteDoctore", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 50);
                cmd.Parameters["@TableName"].Direction = ParameterDirection.Output;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                Table = (string)cmd.Parameters["@TableName"].Value;
                con.Close();
                if (i >= 1)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
        }
    }
}