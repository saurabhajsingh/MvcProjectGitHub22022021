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
    public class BL_Question
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Question> headerlist = new List<Question>();
        private SqlConnection con;

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public DataSet GetQuestionHead(string GetQuestionHead)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select QuestionHeadID,QuestionHeadName from QUESTIONHEAD where QuestionHeadName like ''+@GetQuestionHead+'%' and HospitalID = " + HospitalID + " and LocationID = " + LocationID +  "", con);
            cmd.Parameters.AddWithValue("@GetQuestionHead", GetQuestionHead);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public bool Question(Question obj)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("IUQuestion", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.QuestionID == 0)
            {
                cmd.Parameters.AddWithValue("@QuestionID", 0);
                cmd.Parameters["@QuestionID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@QuestionID", obj.QuestionID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");

            }
            if (obj.ReferenceCode == null)
            {
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            }
            cmd.Parameters.AddWithValue("@QuestionName", obj.QuestionName);

            if (obj.QuestionHeadID == null)
            {
                cmd.Parameters.AddWithValue("@QuestionHeadID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@QuestionHeadID", obj.QuestionHeadID);
                //cmd.Parameters.AddWithValue("@Mode", "Edit");
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

        public bool CheckQuestion(int QuestionID, string QuestionName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckQuestion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                cmd.Parameters.AddWithValue("@QuestionName", QuestionName.ToUpper());
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

        public List<Question> SelectAllQuestionHead()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllQuestion", con);
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
                headerlist.Add(
                    new Question
                    {
                        QuestionID = Convert.ToInt32(dr["QuestionID"]),
                        QuestionName = Convert.ToString(dr["QuestionName"])

                    });
            }
            return headerlist;


        }

        public List<Question> GetQuestion(int QuestionID)
        {
            Connect();

            SqlCommand cmd = new SqlCommand("GetQuestion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionID", QuestionID));
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                headerlist.Add(
                    new Question
                    {
                        QuestionID = Convert.ToInt32(dr["QuestionID"]),
                        QuestionHeadID = dr["QuestionHeadID"].ToString(),
                        QuestionName = Convert.ToString(dr["QuestionName"]),
                        QuestionHeadName = Convert.ToString(dr["QuestionHeadName"])

                    });
            }
            return headerlist;

        }

        public bool DeleteQuestion(int QuestionID)
        {

            Connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@QuestionID", SqlDbType.Int);
            apram[0].Value = QuestionID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteQuestion", apram);
            con.Close();

            return true;
        }
    }
}