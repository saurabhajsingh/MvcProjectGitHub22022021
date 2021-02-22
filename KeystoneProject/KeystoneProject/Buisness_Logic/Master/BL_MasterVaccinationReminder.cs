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
    public class BL_MasterVaccinationReminder
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;
        int varMasterVaccinationReminderID;


        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        
        public DataSet ShowAllMasterVaccinationReminder()
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllMasterVaccinationReminder", con);
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
            catch (Exception ex)
            {
                return ds;
            }

           
        }

        public bool CheckMasterVaccinationReminder(int MasterVaccinationReminderID, string MasterVaccinationReminderName)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckMasterVaccinationReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderID", MasterVaccinationReminderID);
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderName", MasterVaccinationReminderName);
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Save(MasterVaccinationReminder obj)
        {
            Connect();
            try
            {
                #region IUMasterVaccinationReminder

                SqlCommand cmd = new SqlCommand("IUMasterVaccinationReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderID", 0);
                cmd.Parameters["@MasterVaccinationReminderID"].Direction = ParameterDirection.Output; 
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderName", obj.MasterVaccinationReminderName);
                cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value); 
                cmd.Parameters.AddWithValue("@Advise", obj.Advice); 
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Add");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                #endregion

                varMasterVaccinationReminderID = Convert.ToInt32(cmd.Parameters["@MasterVaccinationReminderID"].Value.ToString());


                if (varMasterVaccinationReminderID > 0)
                {
                    if(obj.VaccinesName!=null)
                    {
                        string[] vaccinesname = obj.VaccinesName.Split(',');
                        for(int k=0; k<vaccinesname.Length;k++)
                        {
                            if(vaccinesname[k].ToString()!="")
                            {
                                #region IUMasterVaccinationReminderDetail

                                SqlCommand cmd1 = new SqlCommand("IUMasterVaccinationReminderDetail", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@MasterVaccinationReminderID", varMasterVaccinationReminderID);
                                cmd1.Parameters.AddWithValue("@MasterVaccinationReminderDetailID", 0);
                                cmd1.Parameters["@MasterVaccinationReminderDetailID"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.AddWithValue("@VaccinesName", vaccinesname[k]);
                                //cmd1.Parameters.AddWithValue("@DueDate",null);
                                //cmd1.Parameters.AddWithValue("@GivenDate",null);
                                //cmd1.Parameters.AddWithValue("@HealthCheckupDate", null);
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();

                                #endregion
                            }
                        }
                    }

                    return true;
                    
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public DataSet SelectMasterVaccinationReminderByID(int MasterVaccinationReminderID)
        {
            Connect();
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetMasterVaccinationReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderID", MasterVaccinationReminderID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ds = new DataSet();
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

         

        public bool Edit(MasterVaccinationReminder obj)
        {
            Connect();
            try
            {
                #region IUMasterVaccinationReminder

                SqlCommand cmd = new SqlCommand("IUMasterVaccinationReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderID", obj.MasterVaccinationReminderID);
                
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderName", obj.MasterVaccinationReminderName);
                cmd.Parameters.AddWithValue("@ReferenceCode", DBNull.Value);
                cmd.Parameters.AddWithValue("@Advise", obj.Advice);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                #endregion
                 

                if (i > 0)
                {
                    if (obj.VaccinesName != null)
                    {
                        string[] vaccinesname = obj.VaccinesName.Split(',');
                        for (int k = 0; k < vaccinesname.Length; k++)
                        {
                            if (vaccinesname[k].ToString() != "")
                            {
                                #region IUMasterVaccinationReminderDetail

                                SqlCommand cmd1 = new SqlCommand("IUMasterVaccinationReminderDetail", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd1.Parameters.AddWithValue("@MasterVaccinationReminderID", obj.MasterVaccinationReminderID);
                                cmd1.Parameters.AddWithValue("@MasterVaccinationReminderDetailID", 0);
                                
                                cmd1.Parameters.AddWithValue("@VaccinesName", vaccinesname[k]);
                                //cmd1.Parameters.AddWithValue("@DueDate",null);
                                //cmd1.Parameters.AddWithValue("@GivenDate",null);
                                //cmd1.Parameters.AddWithValue("@HealthCheckupDate", null);
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                int j = cmd1.ExecuteNonQuery();
                                con.Close();

                                #endregion
                            }
                        }
                    }

                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int DeleteMasterVaccinationReminder(int MasterVaccinationReminderID)
        {
            int row = 0;
            Connect();

            try
            {
                SqlCommand cmd = new SqlCommand("DeleteMasterVaccinationReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@MasterVaccinationReminderID", MasterVaccinationReminderID);
                con.Open();
                row = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return row;
            }
            return row;
        }
    }
}