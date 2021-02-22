using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KeystoneProject.Models.Master;
using System.Data;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_RefferToDoctor
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

        public List<RefferToDoctor> SelectAllData()
        {
            Connect();
            List<RefferToDoctor> doctorlist = new List<RefferToDoctor>();
            SqlCommand cmd = new SqlCommand("GetAllRefferToDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                doctorlist.Add(
                    new RefferToDoctor
                    {
                        RefferDoctorID = Convert.ToInt32(dr["RefferDoctorID"]),
                        InstituteName = Convert.ToString(dr["InstituteName"]),
                        PrintName = Convert.ToString(dr["PrintName"]),
                        ReferenceCode = Convert.ToString(dr["ReferenceCode"]),
                        PermanentAddress = Convert.ToString(dr["PermanentAddress"]),
                        TelephoneNo = Convert.ToString(dr["TelephoneNo"]),
                        FaxNo = Convert.ToString(dr["FaxNo"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                    });
            }
            return doctorlist;
        }

        public RefferToDoctor GetRefferToDoctor(int RefferDoctorID)
        {
            Connect();
            RefferToDoctor doctor = new RefferToDoctor();

            SqlCommand cmd = new SqlCommand("GetRefferToDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@RefferDoctorID", RefferDoctorID));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                doctor.RefferDoctorID = Convert.ToInt32(dr["RefferDoctorID"]);
                doctor.InstituteName = Convert.ToString(dr["InstituteName"]);
                doctor.PrintName = Convert.ToString(dr["PrintName"]);
                doctor.ReferenceCode = Convert.ToString(dr["ReferenceCode"]);
                doctor.PermanentAddress = Convert.ToString(dr["PermanentAddress"]);
                doctor.TelephoneNo = Convert.ToString(dr["TelephoneNo"]);
                doctor.FaxNo = Convert.ToString(dr["FaxNo"]);
                doctor.MobileNo = Convert.ToString(dr["MobileNo"]);
                doctor.EmailId = Convert.ToString(dr["EmailId"]);
            }
            return doctor;
        }

        public bool Save(RefferToDoctor obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IURefferToDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);

            if (obj.RefferDoctorID == 0)
            {
                cmd.Parameters.AddWithValue("@RefferDoctorID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@RefferDoctorID", obj.RefferDoctorID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            cmd.Parameters.AddWithValue("@InstituteName", obj.InstituteName);
            cmd.Parameters.AddWithValue("@PrintName", obj.PrintName);

            if (obj.PermanentAddress!=null)
            {
                cmd.Parameters.AddWithValue("@PermanentAddress", obj.PermanentAddress);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PermanentAddress", "");
            }
            if (obj.MobileNo!=null)
            {
                cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MobileNo", "");
            }
          
      
            if (obj.TelephoneNo == null)
            {
                cmd.Parameters.AddWithValue("@TelephoneNo","");
            }
            else
            {
                cmd.Parameters.AddWithValue("@TelephoneNo", obj.TelephoneNo);
            }
            if (obj.FaxNo != null)
            {
                cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FaxNo", "");
            }


            if (obj.EmailId != null)
            {
                cmd.Parameters.AddWithValue("@EmailId", obj.EmailId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EmailId","" );
            }


           
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@ReferenceCode", "");
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

        public int DeleteRefferDoctor(int RefferDoctorID)
        {
            Connect();
            int delete = 0;
            RefferToDoctor doctor = new RefferToDoctor();
            SqlCommand cmd = new SqlCommand("DeleteRefferToDoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@RefferDoctorID", RefferDoctorID);
            con.Open();
            delete = cmd.ExecuteNonQuery();
            return delete;
        }

        public bool CheckRefferToDoctor(int RefferDoctorID, string InstituteName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckRefferToDoctor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@RefferDoctorID", RefferDoctorID);
                cmd.Parameters.AddWithValue("@PrintName", InstituteName);
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
    }
}