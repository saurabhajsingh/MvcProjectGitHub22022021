using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KeystoneProject.Models.Master;
using System.Configuration;
namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_AddDoctorAppointment
    {

        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private SqlConnection con;

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        }

        public bool save(DoctorAppointmentSchedule obd)
        {
            bool flag;
            try
            {
                Connect();
                con.Open();

                SqlCommand cmd = new SqlCommand("IUDoctorAppointmentSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obd.DoctorAppoinmentScheduleID == 0)
                {
                    cmd.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                    cmd.Parameters["@DoctorAppoinmentScheduleID"].Direction = ParameterDirection.Output;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obd.DoctorAppoinmentScheduleID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }
             
                //cmd.Parameters.AddWithValue("@DoctorID", obd.DoctorID);
                if (obd.DoctorID == null)
                {
                    cmd.Parameters.AddWithValue("@DoctorID", 0);
                    
                }
                else
                {
                  
                    cmd.Parameters.AddWithValue("@DoctorID", obd.DoctorID);
                  
                }

                //cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                cmd.Parameters.AddWithValue("@AppointmentType ", obd.AppointmentType);
                cmd.Parameters.AddWithValue("@EndDurationDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@NoOfAppointment", obd.NoOfAppointment);
                cmd.Parameters.AddWithValue("@PerAppointmentTimeinMinute", obd.PerAppointmentTimeinMinute);
                //cmd.Parameters.AddWithValue("@FromDate", obd.FromDate);
                if (obd.FromDate == null)
                {
                    SqlParameter param1 = new SqlParameter
                    {
                        ParameterName = "@FromDate",
                        SqlDbType = SqlDbType.Date,
                        Value = obd.FromDate.ToString("yyyy-MM-dd")

                    };
                    cmd.Parameters.Add(param1);
                    //cmd.Parameters.AddWithValue("@FromDate", obd.FromDate);

                }
                else
                {
                    SqlParameter param1 = new SqlParameter
                    {
                        ParameterName = "@FromDate",
                        SqlDbType = SqlDbType.Date,
                        Value = DateTime.Now.ToString("yyyy-MM-dd")

                    };
                    cmd.Parameters.Add(param1);
                }
                //  cmd.Parameters.AddWithValue("@FromDate", obd.FromDate);

                cmd.Parameters.AddWithValue("@FromTime", obd.FromTime.ToString("HH:mm tt"));

                cmd.Parameters.AddWithValue("@ToTime", obd.ToTime.ToString("HH:mm tt"));


                //cmd.Parameters.AddWithValue("@EndDurationDate", obd.EndDurationDate);
                cmd.Parameters.AddWithValue("@EndDurationStatus", obd.EndDurationStatus);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
               

                int i = cmd.ExecuteNonQuery();

                if (i == 1)
                {
                    obd.DoctorAppoinmentScheduleID = Convert.ToInt32(cmd.Parameters["@DoctorAppoinmentScheduleID"].Value);
                }
                con.Close();
                if (obd.DoctorAppoinmentScheduleID > 0)
                {
                    //string[] day;
                    if (obd.monthweek == null)
                    {
                        obd.monthweek = "";
                         
                    }
                   
                        string[] day = obd.monthweek.Split(',');
                    
                   

                    for (int j = 0; j < day.Length; j++)
                    {
                        if (obd.AppointmentType == "Weekly")
                        {

                            {
                                con.Open();
                                SqlCommand cmd1 = new SqlCommand("IUDoctorAppointmentScheduleDetail", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                                cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleDetailID", 0);
                                cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obd.DoctorAppoinmentScheduleID);
                                cmd1.Parameters.AddWithValue("@AppointmentType", obd.AppointmentType);
                                cmd1.Parameters.AddWithValue("@DayDate", day[j]);
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");

                                int cmf = cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        else if (obd.AppointmentType == "Monthly")
                        {
                            con.Open();
                            SqlCommand cmd1 = new SqlCommand("IUDoctorAppointmentScheduleDetail", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                            cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleDetailID", 0);
                            cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obd.DoctorAppoinmentScheduleID);
                            cmd1.Parameters.AddWithValue("@AppointmentType", obd.AppointmentType);
                            cmd1.Parameters.AddWithValue("@DayDate", day[j]);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Mode", "Add");

                            int cmf = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    if (obd.monthweek == "")
                    {
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("IUDoctorAppointmentScheduleDetail", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                        cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                        cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleDetailID", 0);
                        cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obd.DoctorAppoinmentScheduleID);
                        cmd1.Parameters.AddWithValue("@AppointmentType", obd.AppointmentType);
                        cmd1.Parameters.AddWithValue("@DayDate", obd.AppointmentType);
                        cmd1.Parameters.AddWithValue("@CreationID", UserID);
                        cmd1.Parameters.AddWithValue("@Mode", "Add");

                        int cmf = cmd1.ExecuteNonQuery();
                        con.Close();
                    }


                    flag = true;
                }
                else
                {
                    flag = false;
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                flag = false;
            }
            return flag;
        }
        public DataSet GetDoctor(string GetDoctor)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select DoctorID,DoctorPrintName from Doctor where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and RowStatus =0 and DoctorPrintName  like '%'+@GetDoctor+'%'", con);
            cmd.Parameters.AddWithValue("@GetDoctor", GetDoctor);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet SelectAllDoctorAppointment()
        {
            Connect();
            DataSet dsDoctorAppointment = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllDoctorAppointmentSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsDoctorAppointment = new DataSet();
                da.Fill(dsDoctorAppointment);
                con.Close();

                return dsDoctorAppointment;
            }
            catch (Exception)
            {
                return dsDoctorAppointment;
            }


        }
        public DataSet DoctorName(string DoctorName)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlDataAdapter ad = new SqlDataAdapter("select DoctorID ,DoctorPrintName from Doctor where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and DoctorID='" + DoctorName + "' and RowStatus=0", con);

            con.Open();
            ad.Fill(ds);
            con.Close();

            return ds;
        }
        public DataSet GetDoctorAppointment(int DoctorAppoinmentScheduleID, int DoctorID)
        {
            SqlConnection con = null;


            DataSet ds = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());

                SqlCommand cmd = new SqlCommand("GetDoctorAppointmentSchedule", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID ", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID ", LocationID);
                cmd.Parameters.AddWithValue("@DoctorAppoinmentScheduleID ", DoctorAppoinmentScheduleID);
                cmd.Parameters.AddWithValue("@DoctorID", DoctorID);// i will pass zero to MobileID beacause its Primary .

                con.Open();

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }

        }

        public string DeleteDoctorAppointmentSchedule(int DoctorAppointmentScheduleID)
        {
            string Table = string.Empty;
            Connect();
            {
                SqlCommand cmd = new SqlCommand("DeleteDoctorAppointmentSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DoctorAppointmentScheduleID", DoctorAppointmentScheduleID);
           
                con.Open();
                int i = cmd.ExecuteNonQuery();
               

                //con.Open();
                //int i = cmd.ExecuteNonQuery();

                con.Close();
                if (i >= 1)
                {
                    return "Delete";
                }
                else
                {
                    return Table;
                }
            }
        }

    }
}