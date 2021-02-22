using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_QuestionHead
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<QuestionHead> questionheadlist = new List<QuestionHead>();
        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<QuestionHead> SelectAllQuestionHead()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllQuestionHead", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                questionheadlist.Add(
                    new QuestionHead
                    {
                        QuestionHeadID = Convert.ToInt32(dr["QuestionHeadID"]),
                        QuestionHeadName = Convert.ToString(dr["QuestionHeadName"])

                    });
            }
            return questionheadlist;


        }

        public List<QuestionHead> GetQuestionHead(int QuestionHeadID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetQuestionHead", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionHeadID", QuestionHeadID));
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                questionheadlist.Add(
                    new QuestionHead
                    {
                        QuestionHeadID = Convert.ToInt32(dr["QuestionHeadID"]),
                        QuestionHeadName = Convert.ToString(dr["QuestionHeadName"])

                    });
            }
            return questionheadlist;

        }

        public bool QuestionHead(QuestionHead obj)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUQuestionHead", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.QuestionHeadID == 0)
            {
                cmd.Parameters.AddWithValue("@QuestionHeadID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@QuestionHeadID", obj.QuestionHeadID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }


            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);

            cmd.Parameters.AddWithValue("@QuestionHeadName", obj.QuestionHeadName);

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

        public bool CheckQuestionHead(int QuestionHeadID, string QuestionHeadName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckQuestionHead", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@QuestionHeadID", QuestionHeadID);
                cmd.Parameters.AddWithValue("@QuestionHeadName", QuestionHeadName.ToUpper());
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
        public bool DeleteQuestionHead(int QuestionHeadID)
        {

            Connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@QuestionHeadID", SqlDbType.Int);
            apram[0].Value = QuestionHeadID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteQuestionHead", apram);
            con.Close();

            return true;
        }

    }
}