using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MasterFinacialAccounts
{
    public class BL_Schedule
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

        public bool Save(Schedule obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            if (obj.ScheduleID == 0)
            {
                cmd.Parameters.AddWithValue("@ScheduleID", 0);
                cmd.Parameters["@ScheduleID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScheduleID", obj.ScheduleID);
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

            if (obj.ScheduleName == null)
            {
                cmd.Parameters.AddWithValue("@ScheduleName", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScheduleName", obj.ScheduleName);
            }

            if (obj.Nature == null)
            {
                cmd.Parameters.AddWithValue("@Nature", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Nature", obj.Nature);
            }

            if (obj.BPT == null)
            {
                cmd.Parameters.AddWithValue("@BPT", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BPT", obj.BPT);
            }

            if (obj.MasterScheduleID == null)
            {
                cmd.Parameters.AddWithValue("@MasterScheduleID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MasterScheduleID", obj.MasterScheduleID);
            }

            if (obj.ScheduleType == null)
            {
                cmd.Parameters.AddWithValue("@ScheduleType", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScheduleType", obj.ScheduleType);
            }

            if (obj.GeneralLedgerPosting == null)
            {
                cmd.Parameters.AddWithValue("@GeneralLedgerPosting", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@GeneralLedgerPosting", obj.GeneralLedgerPosting);
            }

            if (obj.ShowDetailsInReports == null)
            {
                cmd.Parameters.AddWithValue("@ShowDetailsInReports", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShowDetailsInReports", obj.ShowDetailsInReports);
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

        public List<Schedule> SelectAllSchedule()
        {
            Connect();
            List<Schedule> schedule = new List<Schedule>();
            SqlCommand cmd = new SqlCommand("GetAllSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Close();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                schedule.Add(
                    new Schedule
                    {
                        ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                        ScheduleName = Convert.ToString(dr["ScheduleName"]),
                        Nature = Convert.ToString(dr["Nature"]),
                        BPT = Convert.ToString(dr["BPT"]),
                        MasterScheduleName = Convert.ToString(dr["MasterScheduleName"]),
                        MasterScheduleID = Convert.ToString(dr["MasterScheduleID"]),
                        GeneralLedgerPosting = Convert.ToString(dr["GeneralLedgerPosting"]),
                        ShowDetailsInReports = Convert.ToString(dr["ShowDetailsInReports"]),
                        ScheduleType = Convert.ToString(dr["ScheduleType"]),
                    });
            }
            return schedule;
        }
        public DataSet BindMasterSchedule(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ScheduleID , ScheduleName  FROM Schedule  where  HospitalID = '" + HospitalID + "' and LocationID = '" + LocationID + "' and RowStatus = 0 and ScheduleName like '" + prefix + "%' order by  ScheduleName  asc", con);
                //cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                //cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
               
            }
            catch (Exception )
            {
            }
            return ds;
        }

        public int DeleteSchedule(int ScheduleID)
        {
            Connect();
            int delete = 0;
            VoucherType voucher = new VoucherType();
            SqlCommand cmd = new SqlCommand("DeleteSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }
    }
}