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
    public class BL_TreatmentAdvice
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<TreatmentAdvice> TreatmentAdviceList = new List<TreatmentAdvice>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }


        public List<TreatmentAdvice> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetAllTreatmentAdvice", con);
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
                TreatmentAdviceList.Add(
                    new TreatmentAdvice
                    {
                        TreatmentAdviceID = Convert.ToInt32(dr["TreatmentAdviceID"]),
                        TreatmentAdviceName = Convert.ToString(dr["TreatmentAdviceName"]),
                        TreatmentAdviceDescription = Convert.ToString(dr["TreatmentAdviceDescription"]),
                    });
            }
            return TreatmentAdviceList;
        }


        public List<TreatmentAdvice> GetTreatmentAdvice(int TreatmentAdviceID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetTreatmentAdvice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TreatmentAdviceID", TreatmentAdviceID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                TreatmentAdviceList.Add(
                    new TreatmentAdvice
                    {
                        TreatmentAdviceID = Convert.ToInt32(dr["TreatmentAdviceID"]),
                        TreatmentAdviceName = Convert.ToString(dr["TreatmentAdviceName"]),
                        TreatmentAdviceDescription = Convert.ToString(dr["TreatmentAdviceDescription"]),
                    });
            }
            return TreatmentAdviceList;
        }



        public bool Save(TreatmentAdvice obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUTreatmentAdvice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.TreatmentAdviceID == 0)
            {
                cmd.Parameters.AddWithValue("@TreatmentAdviceID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@TreatmentAdviceID", obj.TreatmentAdviceID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@TreatmentAdviceName", obj.TreatmentAdviceName);
            if (obj.ReferenceCode == null)
                cmd.Parameters.AddWithValue("@ReferenceCode", string.Empty);
            else
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.ReferenceCode);
            if (obj.TreatmentAdviceDescription == null)
                cmd.Parameters.AddWithValue("@TreatmentAdviceDescription", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@TreatmentAdviceDescription", obj.TreatmentAdviceDescription);
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


        public bool CheckTreatmentAdvice(int TreatmentAdviceID, string TreatmentAdviceName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckTreatmentAdvice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@TreatmentAdviceID", TreatmentAdviceID);
                cmd.Parameters.AddWithValue("@TreatmentAdviceName", TreatmentAdviceName.ToUpper());
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


        public string Delete(int TreatmentAdviceID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeleteTreatmentAdvice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TreatmentAdviceID", TreatmentAdviceID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                con.Open();
                int i = cmd.ExecuteNonQuery();

                con.Close();
                if (i > 0)
                {
                    return Table;
                }
                else
                {
                    return Table;
                }
            }
            catch (Exception)
            {
                return Table;
            }
        }

    }
}