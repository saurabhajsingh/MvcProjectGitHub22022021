using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.PharmacyMaster
{
    public class BL_MedicalSchedule
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<MedicalSchedule> ScheduleList = new List<MedicalSchedule>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public List<MedicalSchedule> GetData()
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetAllMedicalSchedule", con);
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
                ScheduleList.Add(
                    new MedicalSchedule
                    {
                        ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                        scheduleName = Convert.ToString(dr["ScheduleName"]),
                        Nature = Convert.ToString(dr["Nature"]),
                        MasterScheduleName=Convert.ToString(dr["MasterScheduleName"]),
                        MasterScheduleID = dr["MasterScheduleID"].ToString(),
                        BPT = Convert.ToString(dr["BPT"]),
                        generalLedgerPosting = Convert.ToString(dr["GeneralLedgerPosting"]),
                        showDetailsInReports = Convert.ToString(dr["ShowDetailsInReports"]),
                       
                    });
            }
            return ScheduleList;
        }
        public DataSet BindMasterSchedule(string prefix)
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT ScheduleID , ScheduleName  FROM MedicalSchedule  where  HospitalID =" + HospitalID + " and LocationID =" + LocationID + "  and RowStatus = 0 AND ScheduleName <> '' order by  ScheduleName  asc ", con);
                //cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                //cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public bool Save(MedicalSchedule obj)
        {
            try
            { 
            Connect();
            SqlCommand cmd = new SqlCommand("IUMedicalSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.ScheduleID == 0)
            {
                cmd.Parameters.AddWithValue("@ScheduleID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScheduleID", obj.ScheduleID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            
            cmd.Parameters.AddWithValue("@MasterScheduleID", obj.MasterScheduleName);
            cmd.Parameters.AddWithValue("@ScheduleName", obj.scheduleName);
            cmd.Parameters.AddWithValue("@Nature", obj.Nature);

            if (obj.BPT == null)
            {
                cmd.Parameters.AddWithValue("@BPT", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BPT", obj.BPT);
            }

            if (obj.generalLedgerPosting == null)
            {
                cmd.Parameters.AddWithValue("@GeneralLedgerPosting", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@GeneralLedgerPosting", obj.generalLedgerPosting);
            }

            if (obj.showDetailsInReports == null)
            {
                cmd.Parameters.AddWithValue("@ShowDetailsInReports", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShowDetailsInReports", obj.showDetailsInReports);
            }
                cmd.Parameters.AddWithValue("@ReferenceCode", 0);
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
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        private bool RedirectToAction(string v, object p)
        {
            throw new NotImplementedException();
        }

        public int DeleteSchedule(int ScheduleID)
        {
            Connect();
            int delete = 0;
            
            SqlCommand cmd = new SqlCommand("DeleteMedicalSchedule", con);
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

