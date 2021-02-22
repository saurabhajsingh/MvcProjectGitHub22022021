using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientFeedback
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PatientFeedback> headerlist = new List<PatientFeedback>();
        private SqlConnection con;


        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<Patient1> GetPatientName(string search)
        {
            Connect();

            List<Patient1> patientnamelist = new List<Patient1>();
            SqlCommand cmd = new SqlCommand("select PatientName,PatientRegNO,MobileNo,EmailID,Address from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientName like '" + search + '%' + "' and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new Patient1
                    {
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNO = Convert.ToString(dr["PatientRegNO"])
                    });

            }
            return patientnamelist;
        }

        public List<PatientFeedback> GetUserName(string search)
        {
            Connect();

            List<PatientFeedback> patientnamelist = new List<PatientFeedback>();
            SqlCommand cmd = new SqlCommand("select LoginName,UserID from users where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and LoginName like '" + search + '%' + "'  and RowStatus=0", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                patientnamelist.Add(
                    new PatientFeedback
                    {
                        Username = Convert.ToString(dr["LoginName"]),
                        UserID = Convert.ToString(dr["UserID"])
                    });

            }
            return patientnamelist;
        }

        public DataSet GetPatient(int PatientRegNO)
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
                param[2] = new SqlParameter("@PatientRegNO", SqlDbType.Int);
                param[2].Value = PatientRegNO;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientFeedbackForm", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetAllFeedbackForm()
        {

            Connect();
            SqlCommand cmd = new SqlCommand("GetAllFeedbackForm", con);

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

        public bool PatientFeedback(PatientFeedback obj)
        {
            Connect();
            int feedback = 0;

            string[] QuesName = obj.QuestionName.Split(',');
            QuesName = QuesName.Where(name => !string.IsNullOrEmpty(name)).ToArray();
            string[] headnameid = obj.QuestionHeadID.Split(',');
            headnameid = headnameid.Where(name => !string.IsNullOrEmpty(name)).ToArray();
            string[] review = obj.ReviewType.Split(',');

            string[] user = obj.Username.Split(',');
            string[] remark = obj.Comments.Split(',');
            string[] userquestionheadid = obj.userquestionhead.Split(',');

            for (int count = 0; count < QuesName.Length; count++)
            {
               
                SqlCommand cmd = new SqlCommand("IUFeedbackForm", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.FeedBackFormID == 0)
                {
                    cmd.Parameters.AddWithValue("@FeedBackFormID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                //if (obj.ReferenceCode == null)
                //    cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
                //else
                //    cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
                cmd.Parameters.AddWithValue("@QuestionHeadID", headnameid[count]);
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@QuestionName", QuesName[count]);
                cmd.Parameters.AddWithValue("@ReferredByDoctorID", obj.ReferredByDoctorID);
                cmd.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
                cmd.Parameters.AddWithValue("@Address", obj.Address);
                cmd.Parameters.AddWithValue("@MobileNo", obj.ContactNo);
                cmd.Parameters.AddWithValue("@EmailID", obj.EmailId);
               
                cmd.Parameters.AddWithValue("@UserName", user[count]);
                cmd.Parameters.AddWithValue("@Comments", remark[count]);
                cmd.Parameters.AddWithValue("@ReviewType", review[count]);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                con.Open();
                feedback = cmd.ExecuteNonQuery();
                con.Close();
     }
            if (feedback > 0)
            {
                return true;
      }
            else
            {
                return false;
            }

        }
    }
}