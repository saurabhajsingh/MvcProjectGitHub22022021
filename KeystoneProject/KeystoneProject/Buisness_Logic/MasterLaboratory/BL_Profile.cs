using KeystoneProject.Models.Master;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_Profile
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;
        List<Profile> profilelist = new List<Profile>();
        private SqlConnection con;
        private  int TestID;

        public DataSet GetAllTestMaster()
        {

            connect();
            SqlCommand cmd = new SqlCommand("GetAllTestMaster", con);

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


        public DataSet GetAllProfile()
        {

            connect();
            SqlCommand cmd = new SqlCommand("GetAllProfile", con);

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

        private void connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
            HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            
        }
       
        public bool Save(Profile obj)
        {
            connect();
            con.Open();
            try
            {
                #region IUProfileID
                if (obj.ProfileID == "0" || obj.ProfileID == "")
                {
                    SqlCommand cmd = new SqlCommand("IUProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);

                    if (obj.ProfileID == "0" || obj.ProfileID == "")
                    {
                        cmd.Parameters.AddWithValue("@ProfileID", 0);
                        cmd.Parameters["@ProfileID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProfileID", obj.ProfileID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");

                    }

                    cmd.Parameters.AddWithValue("@Name", obj.Name);

                    cmd.Parameters.AddWithValue("@CreationID", UserID);

                    cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                    cmd.Parameters.AddWithValue("@Client", obj.Client);
                    cmd.Parameters.AddWithValue("@GeneralCharges", obj.GeneralCharges);
                    cmd.Parameters.AddWithValue("@EmergencyCharges", obj.EmergencyCharges);
                    cmd.Parameters.AddWithValue("@MyCost", obj.MyCost);
                    if (obj.Commission == "")
                    {
                        cmd.Parameters.AddWithValue("@Commission", string.Empty);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Commission", obj.Commission);
                    }
                    if (obj.CommissionRs == "")
                    {
                        obj.CommissionRs = "";
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommissionRs", obj.CommissionRs);
                    }

                    cmd.Parameters.AddWithValue("@HMSCode", obj.HMSCode);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@ForGender", obj.ForGender);

                    cmd.Parameters.AddWithValue("@Panel", obj.Panel);

                    int row = cmd.ExecuteNonQuery();

                #endregion

                    obj.ProfileID = Convert.ToUInt32(cmd.Parameters["@ProfileID"].Value).ToString();
                    con.Close();

                    if (row > 0)
                    {
                        if (obj.Name != null)
                        {
                            int ProfileTest = 0;
                            string[] name = obj.TestName.Split(',');
                            string[] TesID = obj.TestID.Split(',');
                            for (int k = 0; k < TesID.Length; k++)
                            {
                                if (name[k].ToString() != "")
                                {
                                    #region IUProfileTest

                                    SqlCommand cmd1 = new SqlCommand("IUProfileTest", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;

                                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);

                                    if (obj.ProfileTestID == null || obj.ProfileTestID == "")
                                    {
                                        cmd1.Parameters.AddWithValue("@ProfileTestID", 0);

                                        cmd1.Parameters.AddWithValue("@Mode", "Add");
                                    }
                                    else
                                    {
                                        cmd1.Parameters.AddWithValue("@ProfileTestID", obj.ProfileTestID);
                                        cmd1.Parameters.AddWithValue("@Mode", "Edit");
                                    }

                                    cmd1.Parameters.AddWithValue("@ProfileID", obj.ProfileID);


                                    cmd1.Parameters.AddWithValue("@TestID", TesID[k]);



                                    cmd1.Parameters.AddWithValue("@ReferenceCode", 0);

                                    cmd1.Parameters.AddWithValue("@TestName", name[k]);

                                    cmd1.Parameters.AddWithValue("@CreationID", UserID);

                                    con.Open();
                                    ProfileTest = cmd1.ExecuteNonQuery();
                                    con.Close();
                                    #endregion
                                }
                            }
                        }

                        DataSet dsPatientCWProfile = GetPatientCWProfile();
                        foreach (DataRow NR in dsPatientCWProfile.Tables[0].Rows)
                        {
                            SqlCommand cmd2 = new SqlCommand("IUPatientCWProfile", con);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd2.Parameters.AddWithValue("@PatientCWProfileID", NR["PatientCWProfileID"] = 0);

                            cmd2.Parameters.AddWithValue("@ProfileID", NR["ProfileID"] = obj.ProfileID);
                            cmd2.Parameters.AddWithValue("@GeneralCharges", NR["GeneralCharges"] = obj.GeneralCharges);
                            cmd2.Parameters.AddWithValue("@EmergencyCharges", NR["EmergencyCharges"] = obj.EmergencyCharges);
                            cmd2.Parameters.AddWithValue("@OrganizationID", NR["OrganizationID"]);
                            cmd2.Parameters.AddWithValue("@RecommendedByDoctor", NR["RecommendedByDoctor"]);
                            cmd2.Parameters.AddWithValue("@CreationID", NR["CreationID"] = UserID);
                            cmd2.Parameters.AddWithValue("@Mode", NR["Mode"] = "Add");
                            //NR["CreationID"] = ds.Tables[0].Rows[0]["CreationID"].ToString().Trim();
                            //NR["Mode"] = "Add";
                            con.Open();
                            int CWProfileTest = cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                }

                else
                {
                    SqlCommand cmd = new SqlCommand("IUProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@ProfileID", obj.ProfileID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                    cmd.Parameters.AddWithValue("@Name", obj.Name);

                    cmd.Parameters.AddWithValue("@CreationID", UserID);

                    cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                    cmd.Parameters.AddWithValue("@Client", obj.Client);
                    cmd.Parameters.AddWithValue("@GeneralCharges", obj.GeneralCharges);
                    cmd.Parameters.AddWithValue("@EmergencyCharges", obj.EmergencyCharges);
                    cmd.Parameters.AddWithValue("@MyCost", obj.MyCost);
                    if (obj.Commission == "")
                    {
                        cmd.Parameters.AddWithValue("@Commission", string.Empty);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Commission", obj.Commission);
                    }
                    if (obj.CommissionRs == "")
                    {
                        obj.CommissionRs = "";
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommissionRs", obj.CommissionRs);
                    }

                    cmd.Parameters.AddWithValue("@HMSCode", obj.HMSCode);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@ForGender", obj.ForGender);

                    cmd.Parameters.AddWithValue("@Panel", obj.Panel);

                    int row = cmd.ExecuteNonQuery();

                    obj.ProfileID = Convert.ToUInt32(cmd.Parameters["@ProfileID"].Value).ToString();
                    con.Close();

                    if (row > 0)
                    {
                        if (obj.Name != null)
                        {
                            int ProfileTest = 0;
                            string[] name = obj.TestName.Split(',');
                            string[] TesID = obj.TestID.Split(',');
                            for (int k = 0; k < TesID.Length; k++)
                            {
                                if (name[k].ToString() != "")
                                {
                                    #region IUProfileTest

                                    SqlCommand cmd1 = new SqlCommand("IUProfileTest", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;

                                    cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                    if (obj.ProfileTestID == null || obj.ProfileTestID == "")
                                    {
                                        cmd1.Parameters.AddWithValue("@ProfileTestID", 0);

                                        cmd1.Parameters.AddWithValue("@Mode", "Add");
                                    }
                                    else
                                    {
                                        cmd1.Parameters.AddWithValue("@ProfileTestID", obj.ProfileTestID);
                                        cmd1.Parameters.AddWithValue("@Mode", "Edit");
                                    }
                                    cmd1.Parameters.AddWithValue("@ProfileID", obj.ProfileID);

                                    cmd1.Parameters.AddWithValue("@TestID", TesID[k]);

                                    cmd1.Parameters.AddWithValue("@ReferenceCode", 0);

                                    cmd1.Parameters.AddWithValue("@TestName", name[k]);

                                    cmd1.Parameters.AddWithValue("@CreationID", UserID);

                                    con.Open();
                                    ProfileTest = cmd1.ExecuteNonQuery();
                                    con.Close();
                                    #endregion
                                }
                            }
                        }


                    }

                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckProfile(string ProfileID, string Name)
        {
            string t = "";
            if (ProfileID == null || ProfileID == "")
            {
                t = "0";
            }
            else
            {
                t = ProfileID;
            }
            connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@ProfileID", t);
                cmd.Parameters.AddWithValue("@Name", Name.ToUpper());
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


        public DataSet GetProfile(int ProfileID)
        {
            DataSet ds = new DataSet();
            try
            {
                connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProfileID", SqlDbType.Int);
                param[0].Value = ProfileID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProfile", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }


        public DataSet GetPatientCWProfile()
        {
            DataSet ds = new DataSet();
            try
            {
                connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetPatientCWProfile", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetProfileTest(int ProfileID)
        {
            
            DataSet ds = new DataSet();
            try
            {
                connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@ProfileID", SqlDbType.Int);
                param[1].Value = ProfileID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetProfileTest", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public bool DeleteProfile(int ProfileID)
        {

            connect();
            SqlParameter[] apram = new SqlParameter[4];
            apram[0] = new SqlParameter("@ProfileID", SqlDbType.Int);
            apram[0].Value = ProfileID;
            apram[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
            apram[1].Value = HospitalID;
            apram[2] = new SqlParameter("@LocationID", SqlDbType.Int);
            apram[2].Value = LocationID;
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeleteProfile", apram);
            con.Close();

            return true;
        }

        //public List<Profile> GetProfile(int ProfileID)
        //{
        //    connect();

        //    SqlCommand cmd = new SqlCommand("GetProfile", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@ProfileID", ProfileID));
        //    cmd.Parameters.Add(new SqlParameter("@HospitalID", HospitalID));
        //    cmd.Parameters.Add(new SqlParameter("@LocationID", LocationID));
        //    SqlDataAdapter sd = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    con.Open();
        //    sd.Fill(dt);
        //    con.Close();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        profilelist.Add(
        //            new Profile
        //            {
        //                ProfileID = Convert.ToString(dr["ProfileID"]),
        //                Name = Convert.ToString(dr["Name"]),
        //                PrintAs = Convert.ToString(dr["PrintAs"]),
        //                Client = Convert.ToString(dr["Client"]),
        //                GeneralCharges = Convert.ToString(dr["GeneralCharges"]),
        //                EmergencyCharges = Convert.ToString(dr["EmergencyCharges"]),
        //                Commission = Convert.ToString(dr["Commission"]),
        //                CommissionRs = Convert.ToString(dr["CommissionRs"]),
        //                HMSCode = Convert.ToString(dr["HMSCode"]),
        //                Discount = Convert.ToString(dr["Discount"]),
        //                ForGender = Convert.ToString(dr["ForGender"]),
        //                Panel = Convert.ToString(dr["Panel"]),
        //            });
        //    }
        //    return profilelist;

        //}

       
    }
}