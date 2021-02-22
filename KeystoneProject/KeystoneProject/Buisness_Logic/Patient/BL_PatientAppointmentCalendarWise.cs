using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using KeystoneProject.Controllers.Patient;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientAppointmentCalendarWise
    {


        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<PatientAppointmentCalenderWise> PatientAppointmentList = new List<PatientAppointmentCalenderWise>();
        PatientAppointmentCalenderWise objpatientAppointment = new PatientAppointmentCalenderWise();

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public bool AddDoctorReschedule(PatientAppointmentCalenderWise obj)
        {
            Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("IUDoctorReschedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            if (obj.RescheduleID != null)
            {
                cmd.Parameters.AddWithValue("@RescheduleID", 0);
                cmd.Parameters["@RescheduleID"].Direction = ParameterDirection.Output;
            }
            else
            {
                cmd.Parameters.AddWithValue("@RescheduleID", obj.RescheduleID);
            }

            cmd.Parameters.AddWithValue("@Reason", obj.Reason);
            cmd.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
            cmd.Parameters.AddWithValue("@DoctorName", obj.DoctorName);

            cmd.Parameters.AddWithValue("@DoctorAppointmentScheduleID", 0);


            cmd.Parameters.AddWithValue("@PatientAppointmentID", 0);
            //  if(obj.)
            cmd.Parameters.AddWithValue("@CancelAppointment", obj.CancelAppointment);
            cmd.Parameters.AddWithValue("@FromTime", obj.FromTime);
            cmd.Parameters.AddWithValue("@ToTime", obj.ToTime);
            cmd.Parameters.AddWithValue("@AppointmentType", obj.AppointmentType);
            cmd.Parameters.AddWithValue("@Date", obj.Date);

            cmd.Parameters.AddWithValue("@CreationID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "Add");
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public bool Save(PatientAppointmentCalenderWise obj, DataSet ds)
        {
            Connect();

            con.Open();

            SqlCommand cmd = new SqlCommand("IUPatientAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@PFPatientName", obj.PFPatientName);
            if (obj.PatientID == null || obj.PatientID == "")
            {
                cmd.Parameters.AddWithValue("@PatientID", 0);
                cmd.Parameters["@PatientID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Mode", "Add");

            }
            else
            {
                cmd.Parameters.AddWithValue("@PatientID", obj.PatientID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");

            }

            cmd.Parameters.AddWithValue("@PatientRegNo", 0);
            cmd.Parameters.AddWithValue("@PatientName", (obj.FirstName + " " + obj.PMiddleName + " " + obj.PLastName).ToUpper());
            if (obj.FirstName == null)
            {
                cmd.Parameters.AddWithValue("@PFirstName", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("@PFirstName", obj.FirstName.ToUpper());

            }
            if (obj.PMiddleName != null)
            {
                cmd.Parameters.AddWithValue("@PMiddleName", obj.PMiddleName.ToUpper());
            }
            else
            {
                cmd.Parameters.AddWithValue("@PMiddleName", "");
            }
            if (obj.PLastName != null)
            {
                cmd.Parameters.AddWithValue("@PLastName", obj.PLastName.ToUpper());
            }
            else
            {
                cmd.Parameters.AddWithValue("@PLastName", "");
            }



            if (obj.Age != null)
            {
                cmd.Parameters.AddWithValue("@Age", obj.Age);

            }
            else
            {
                cmd.Parameters.AddWithValue("@Age", 0);
            }
            cmd.Parameters.AddWithValue("@AgeType", "Years");
            cmd.Parameters.AddWithValue("@DateOfBirth", obj.DateOFBirth);

            cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
            cmd.Parameters.AddWithValue("@PatientStatus", 0);

            cmd.Parameters.AddWithValue("@CreationID", UserID);
            int PatientID = cmd.ExecuteNonQuery();
            obj.PatientID = cmd.Parameters["@PatientID"].Value.ToString();

            // int i = cmd.ExecuteNonQuery();
            var delimiters = new char[] { 'T', 'o' };
            var results = obj.FromTime.Split(delimiters);
            // string[] datefo=obj.FromTime.Split(p);

            SqlCommand cmd1 = new SqlCommand("[IUPatientAppointmentDetails]", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
            cmd1.Parameters.AddWithValue("@PatientRegNo", 0);
            if (obj.PatientAppointmentScheduleID == null || obj.PatientAppointmentScheduleID == "")
            {

                cmd1.Parameters.AddWithValue("@PatientAppointmentScheduleID", 0);
                cmd1.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd1.Parameters.AddWithValue("@PatientAppointmentScheduleID", obj.PatientID);
                cmd1.Parameters.AddWithValue("@Mode", "Edit");
            }



            //   cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obj.DoctorAppoinmentScheduleID);      
            cmd1.Parameters.AddWithValue("@DoctorID", obj.DoctorID);
            if (obj.AppointmentDate == null)
            {
                cmd1.Parameters.AddWithValue("@AppointmentDate", System.DateTime.Now);
            }
            else
            {
                var date = obj.AppointmentDate.Split(',');
                var date1 = Convert.ToDateTime(date[0]).ToString("dd-MM-yyyy");
                cmd1.Parameters.AddWithValue("@AppointmentDate", Convert.ToDateTime(date1));
            }

            if (obj.AppointmentType != "OnCalls")
            {
                DataView dvDetail = new DataView(ds.Tables[0], "NoOfAppointment = '" + obj.AppointmentNoID + "' ", "", DataViewRowState.CurrentRows);
                if (dvDetail.Count > 0)
                {
                    cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", dvDetail[0]["DoctorAppoinmentScheduleID"].ToString());
                    cmd1.Parameters.AddWithValue("@FromTime", dvDetail[0]["FromTime"].ToString());
                    cmd1.Parameters.AddWithValue("@ToTime", dvDetail[0]["ToTime"].ToString());
                    cmd1.Parameters.AddWithValue("@NoOfAppointment", dvDetail[0]["NoOfAppointment"].ToString());
                }
                else
                {

                    if (ds.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString() == "")
                    {
                        cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", ds.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString());
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", obj.DoctorAppoinmentScheduleID);

                    }
                    if (obj.FromTime == "")
                    {

                        cmd1.Parameters.AddWithValue("@FromTime", "");
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FromTime", obj.FromTime);


                    }
                    if (obj.ToTime == "")
                    {
                        cmd1.Parameters.AddWithValue("@ToTime", "");


                    }
                    else
                    {

                        cmd1.Parameters.AddWithValue("@ToTime", obj.ToTime);
                    }
                    cmd1.Parameters.AddWithValue("@NoOfAppointment", obj.AppointmentNoID);
                }
            }
            else
            {
                if (ds.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString() == "")
                {
                    cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", ds.Tables[0].Rows[0]["DoctorAppoinmentScheduleID"].ToString());
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", "");

                }
                if (obj.FromTime == "")
                {
                    cmd1.Parameters.AddWithValue("@FromTime", obj.FromTime);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@FromTime", "");

                }
                if (obj.ToTime == "")
                {
                    cmd1.Parameters.AddWithValue("@ToTime", obj.ToTime);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@ToTime", "");
                }
                cmd1.Parameters.AddWithValue("@NoOfAppointment", 0);
            }
            cmd1.Parameters.AddWithValue("@CreationID", UserID);


            //cmd1.Parameters.AddWithValue("@AppointmentType", obj.AppointmentType);

            //cmd1.Parameters.AddWithValue("@FromTime", results[0]);
            //cmd1.Parameters.AddWithValue("@ToTime", results[2]);
            //cmd1.Parameters.AddWithValue("@NoOfAppointment", obj.AppointmentNoID);

            //cmd1.Parameters.AddWithValue("@CreationID", UserID);

            int Vouchar = cmd1.ExecuteNonQuery();



            con.Close();
            return true;

        }

        public List<PatientAppointmentCalenderWise> GetPatientAppointmentDetails(int PatientAppointmentScheduleID)
        {
            Connect();
            // List<Department> dept = new List<Department>();

            SqlCommand cmd = new SqlCommand("GetPatientAppointmentDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
            cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            cmd.Parameters.Add(new SqlParameter("@PatientAppointmentScheduleID", PatientAppointmentScheduleID));
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            sd.Fill(ds);
            con.Close();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        Table1 = "Table1",
                        DoctorAppoinmentScheduleID = Convert.ToInt32(dr["DoctorAppoinmentScheduleID"]).ToString(),
                        DoctorID = Convert.ToString(dr["DoctorID"]),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                        AppointmentDate = Convert.ToDateTime(dr["AppointmentDate"]).ToString("yyyy-MM-dd"),
                        NoOfAppointment = Convert.ToString(dr["NoOfAppointment"]),
                        AppointmentTime = Convert.ToString(dr["FromTime"]) + " To " + Convert.ToString(dr["ToTime"]),
                        ToTime = Convert.ToString(dr["ToTime"]),
                        // DateOFBirth = Convert.ToString(dr["DateOFBirth"]),
                        //  MobileNo = Convert.ToString(dr["MobileNo"]),
                        AppointmentType = Convert.ToString(dr["AppointmentType"])



                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {

                        Table2 = "Table2",
                        PFPatientName = Convert.ToString(dr["PFPatientName"]),
                        PatientID = Convert.ToString(dr["PatientID"]),
                        PFirstName = Convert.ToString(dr["PFirstName"]),
                        PMiddleName = Convert.ToString(dr["PMiddleName"]),
                        PLastName = Convert.ToString(dr["PLastName"]),
                        Age = Convert.ToString(dr["Age"]),
                        DateOFBirth = Convert.ToDateTime(dr["DateOFBirth"]).ToString("yyyy-MM-dd"),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        //DoctorPrintName = dr["Doctor Name"].ToString(),
                        //FromTime = dr["FromTime"].ToString(),
                        //ToTime = dr["ToTime"].ToString(),

                    });
            }




            return PatientAppointmentList;
        }

        # region Delete PatientAppointmentDetails Details

        public string DeletePatientAppointmentDetail(int PatientAppointmentScheduleID)
        {
            string Table = string.Empty;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("DeletePatientAppointmentDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@PatientAppointmentScheduleID", PatientAppointmentScheduleID);
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

        # endregion

        public List<PatientAppointmentCalenderWise> BindPrefixPatient()
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
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]).ToString(),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender = dr["Gender"].ToString(),

                    });
            }
            return PatientAppointmentList;

        }


        public List<PatientAppointmentCalenderWise> BindAppointmentDoctor()
        {
            Connect();
            string SpecializationID = "%";
            string Department = "%";
            SqlCommand cmd = new SqlCommand("select Distinct  Doctor.DoctorID , Doctor.DoctorPrintName,DoctorAppointmentSchedule.DoctorAppoinmentScheduleID  from DoctorAppointmentSchedule left join Doctor on Doctor.DoctorID = DoctorAppointmentSchedule.DoctorID where DoctorAppointmentSchedule.HospitalID = '" + HospitalID + "' and DoctorAppointmentSchedule.LocationID = '" + LocationID + "'  and  Doctor.DoctorPrintName like '%' + DoctorPrintName + '%' and  Doctor.DepartmentID like '" + Department + "' and Doctor.SpecializationID like '" + SpecializationID + "' and DoctorAppointmentSchedule.RowStatus = 0 order by Doctor.DoctorPrintName  asc ", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"].ToString())
                        ,

                       DoctorAppointmentScheduleID = dr["DoctorAppoinmentScheduleID"].ToString(),

                    });
            }
            return PatientAppointmentList;

        }

        public List<PatientAppointmentCalenderWise> BindDoctorAppointmenttype()
        {
            Connect();
            string SpecializationID = "%";
            string Department = "%";
            SqlCommand cmd = new SqlCommand("select Distinct  Doctor.DoctorID , Doctor.DoctorPrintName from DoctorAppointmentSchedule left join Doctor on Doctor.DoctorID = DoctorAppointmentSchedule.DoctorID where DoctorAppointmentSchedule.HospitalID = '" + HospitalID + "' and DoctorAppointmentSchedule.LocationID = '" + LocationID + "'  and  Doctor.DoctorPrintName like '%' + DoctorPrintName + '%' and  Doctor.DepartmentID like '" + Department + "' and Doctor.SpecializationID like '" + SpecializationID + "' and DoctorAppointmentSchedule.RowStatus = 0 order by Doctor.DoctorPrintName  asc ", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        DoctorID = Convert.ToInt32(dr["DoctorID"]).ToString(),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"].ToString())
                        //,

                        //DoctorAppointmentScheduleID = dr["DoctorAppoinmentScheduleID"].ToString(),

                    });
            }
            return PatientAppointmentList;

        }

        public DataSet GetDoctorRescheduleAppointment(int DoctorID, DateTime AppointmentDate, string AppointmentType)
        {
            Connect();
            {
                DataSet ds = new DataSet();
                try
                {
                    Connect();
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                    param[2].Value = DoctorID;
                    param[3] = new SqlParameter("@Date", SqlDbType.DateTime);
                    param[3].Value = AppointmentDate;
                    param[4] = new SqlParameter("@AppointmentType", SqlDbType.NVarChar);
                    param[4].Value = AppointmentType;
                    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetDoctorRescheduleAppointment", param);
                    PatientAppointmentCalenderWiseController obj = new PatientAppointmentCalenderWiseController();
                    List<string[]> fillFromTO = new List<string[]>();
                    obj.BindFromToTime(AppointmentType, DoctorID.ToString(), AppointmentDate.ToString());

                }




                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    throw ex;
                }
                return ds;
            }
        }

        public DataSet GetAllFinancialYear()
        {
            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllFinancialYear", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }


        public List<PatientAppointmentCalenderWise> GetPatientName(string PatientName)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select PatientName,PFirstName,PMiddleName,PLastName,PatientRegNO,DateOfBirth,MobileNo from Patient where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and PatientName like '" + PatientName + '%' + "' and RowStatus=0", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        PatientRegNo = dr["PatientRegNO"].ToString(),
                        PFirstName = Convert.ToString(dr["PFirstName"]),
                        PMiddleName = Convert.ToString(dr["PMiddleName"]),
                        PLastName = Convert.ToString(dr["PLastName"]),
                        DateOFBirth = Convert.ToDateTime(dr["DateOFBirth"]).ToString("yyyy-MM-dd"),
                        MobileNo = Convert.ToString(dr["MobileNo"])

                    });
            }

            return PatientAppointmentList;

        }

        public DataSet GetPatientAppointmentsAppointID(int HospitalID, int LocationID, int PatientID)
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
                param[2] = new SqlParameter("@PatientID", SqlDbType.Int);
                param[2].Value = PatientID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientAppointmentsAppointID", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetAllPatientAppointmentData(string Date)
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
                param[2] = new SqlParameter("@Date", SqlDbType.DateTime);
                param[2].Value = Convert.ToDateTime(Date);
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllPatientAppointmentData", param);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        public List<PatientAppointmentCalenderWise> GetDoctorAppointmentSchedule(int DoctorAppoinmentScheduleID, int DoctorID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetDoctorAppointmentSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", DoctorAppoinmentScheduleID);
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {

                        Table1 = "Table1",
                        DoctorAppointmentScheduleID = dr["DoctorAppoinmentScheduleID"].ToString(),
                        AppointmentType = dr["AppointmentType"].ToString(),
                        DoctorPrintName = dr["Doctor Name"].ToString(),
                        FromTime = dr["FromTime"].ToString(),
                        ToTime = dr["ToTime"].ToString(),

                    });
            }


            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        Table2 = "Table2",
                        DoctorPrintName = dr["Doctor Name"].ToString(),
                        DayDate = dr["DayDate"].ToString(),
                        // DoctorAppointmentScheduleID = dr["DoctorAppoinmentScheduleID"].ToString(),
                        AppointmentType = dr["AppointmentType"].ToString(),

                    });
            }


            return PatientAppointmentList;
        }


        public List<PatientAppointmentCalenderWise> GetPatientAppointmentDetailForBindAppointmentType(int DoctorID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetPatientAppointmentDetailForBindAppointmentType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {
                        AppointmentType = dr["AppointmentType"].ToString(),
                        DoctorAppoinmentID = dr["DoctorAppoinmentScheduleID"].ToString(),
                        AppointmentTypeID = "0",
                    });
            }


            return PatientAppointmentList;
        }

        public string GetNextOutsidePatientID()
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
                //param[2] = new SqlParameter("@PatientID", SqlDbType.Int);
                //param[2].Value = PatientID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetNextOutsidePatientID", param);


            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds.Tables[0].Rows[0]["PatientID"].ToString();


        }

        public List<PatientAppointmentCalenderWise> GetDoctorAppointmentSchedule1(int DoctorAppoinmentScheduleID, int DoctorID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("GetDoctorAppointmentSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@DoctorAppoinmentScheduleID", DoctorAppoinmentScheduleID);
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                PatientAppointmentList.Add(
                    new PatientAppointmentCalenderWise
                    {

                        Table1 = "Table1",
                        DoctorAppoinmentID = dr["DoctorAppoinmentScheduleID"].ToString(),
                        //DoctorNameID=dr["DoctorID"].ToString(),
                        DoctorName = dr["Doctor Name"].ToString(),
                        FromTime = dr["FromTime"].ToString(),
                        ToTime = dr["ToTime"].ToString(),
                        AppointmentType = dr["AppointmentType"].ToString(),
                    });
            }

            return PatientAppointmentList;

        }


        //public List<PatientAppointmentCalenderWise> GetAllPatientAppointmentDetail()
        //{
        //    Connect();
        //    SqlCommand cmd = new SqlCommand("GetAllPatientAppointmentDetail", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
        //    cmd.Parameters.AddWithValue("@LocationID", LocationID);
        //    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
        //    cmd.Parameters.AddWithValue("@DoctorID", 0);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();


        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        PatientAppointmentList.Add(
        //           new PatientAppointmentCalenderWise
        //           {
        //               PatientID = dr["PatientID"].ToString(),
        //               DateOFBirth = dr["DateOfBirth"].ToString(),
        //               PFirstName = dr["PatientName"].ToString(),
        //               MobileNo = dr["MobileNo"].ToString(),
        //               Status = Convert.ToBoolean(dr["Status"].ToString()),
        //               PatientRegNo = dr["PatientRegNo"].ToString(),
        //               PatientAppointmentScheduleID = dr["PatientAppointmentScheduleID"].ToString(),
        //               DoctorID = dr["DoctorID"].ToString(),
        //               DoctorPrintName = dr["Doctor Name"].ToString(),
        //               AppointmentDate = Convert.ToDateTime(dr["AppointmentDate"]).ToString("yyyy-MM-dd"),
        //               FromTime = dr["FromTime"].ToString(),
        //               ToTime = dr["ToTime"].ToString(),
        //               PatientAppointmentID = dr["PatientAppointmentID"].ToString(),
        //               DoctorAppoinmentScheduleID = dr["DoctorAppoinmentScheduleID"].ToString()
        //           });
        //    }


        //    return PatientAppointmentList;
        //}


        public DataSet GetPatientAppointmentDetailForDoctorDetail(int HospitalID, int LocationID, int DoctorID, string AppointmentType, string DayDate, DateTime AppointmentDate)
        {
            DataSet ds = new DataSet();
            DataSet dscopy = new DataSet();
            try
            {
                Connect();

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@DoctorID", SqlDbType.Int);
                param[2].Value = DoctorID;
                param[3] = new SqlParameter("@AppointmentType", SqlDbType.NVarChar, 50);
                param[3].Value = AppointmentType;
                param[4] = new SqlParameter("@DayDate", SqlDbType.NVarChar, 50);
                param[4].Value = DayDate;
                param[5] = new SqlParameter("@AppointmentDate", SqlDbType.Date);
                param[5].Value = AppointmentDate;
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientAppointmentDetailForDoctorDetail", param);


                DataView dvDetail = new DataView(ds.Tables[0], "AppointmentType = '" + AppointmentType + "' ", "", DataViewRowState.CurrentRows);




            }

            catch (DBConcurrencyException exp)
            {

                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");

                throw ex;
            }
            return ds;
        }







    }
}