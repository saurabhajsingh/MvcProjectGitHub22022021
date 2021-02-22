using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Controllers.MasterLaboratory;

namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_OutSourceLab
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        public List<OutSourceLab> GetTestMasterForBindCategory()
        {
            List<KeystoneProject.Models.MasterLaboratory.OutSourceLab> serachlist = new List<Models.MasterLaboratory.OutSourceLab>();

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTestMasterForBindCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new Models.MasterLaboratory.OutSourceLab
                    {
                        TestGroupID = Convert.ToInt32(dr["TestGroupID"].ToString()),
                        TestGroupName = dr["TestGroupName"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }

        public DataSet GetCity(string GetCity)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select CityID,CityName,State.StateID,State.StateName,Country.CountryID,Country.CountryName from  City  left join Country on Country.CountryID = City.CountryID left join State on State.StateID = City.StateID where CityName like ''+@GetCity+'%'  and  City.HospitalID = '" + HospitalID + "' and City.LocationID = '" + LocationID + "' and City.RowStatus=0 and State.RowStatus=0 and Country.RowStatus=0 ", con);
            cmd.Parameters.AddWithValue("@GetCity", GetCity);
            //and HospitalID =" + HospitalID + " and LocationID =" + LocationID + "
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
       
        public bool Save(OutSourceLab obj)
        {
            
                Connect();
                con.Open();
                if (obj.OutSourceLabID == "0")
                {

                    SqlCommand cmd = new SqlCommand("IUOutSourceLabRegistration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);


                    if (obj.OutSourceLabID == "0")
                    {
                        cmd.Parameters.AddWithValue("@OutSourceLabID", 0);
                        cmd.Parameters["@OutSourceLabID"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Mode", "Add");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@OutSourceLabID", obj.OutSourceLabID);
                        cmd.Parameters.AddWithValue("@Mode", "Edit");
                    }
                    cmd.Parameters.AddWithValue("@labName", obj.LabName.ToUpper());
                    cmd.Parameters.AddWithValue("@ManagingBody", obj.ManagingBody);
                    cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                    cmd.Parameters.AddWithValue("@Adminstrator", obj.Adminstrator);
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                    cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                    cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                    cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                    cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                    cmd.Parameters.AddWithValue("@URL", obj.URL);
                    cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
                    cmd.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                    //  cmd.Parameters.AddWithValue("@Mode", "Add");

                    int row = cmd.ExecuteNonQuery();
                    obj.OutSourceLabID = Convert.ToString(cmd.Parameters["@OutSourceLabID"].Value);
                    // obj.LabName = cmd.Parameters["@labName"].ToString();
                    con.Close();

                    if (row > 0)
                    {
                        //String[] LabtestdetailsID = obj.OutSourceLabtestdetailsID.ToString().Split(',');
                        String[] GroupID = obj.GroupID.ToString().Split(',');
                        String[] GroupName = obj.TestGroupName.Split(',');
                        String[] TestID = obj.Test.ToString().Split(',');
                        String[] TestName = obj.TestName.Split(',');
                        String[] Rate = obj.Rate.Split(',');
                        String[] Percent = obj.Percentage.Split(',');

                        for (int count = 0; count < TestName.Length; count++)
                        {
                            SqlCommand cmd1 = new SqlCommand("IUOutSourceLabtestdetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);


                            if (obj.OutSourceLabtestdetailsID == "0")
                            {
                                cmd1.Parameters.AddWithValue("@OutSourceLabtestdetailsID", 0);
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                            }
                            else
                            {
                                //int LabtestdetailsID1 = Convert.ToInt32(LabtestdetailsID[count]);
                                cmd1.Parameters.AddWithValue("@OutSourceLabtestdetailsID", obj.OutSourceLabtestdetailsID);
                                cmd1.Parameters.AddWithValue("@Mode", "Edit");
                            }
                            cmd1.Parameters.AddWithValue("@OutSourceLabID", obj.OutSourceLabID);
                            cmd1.Parameters.AddWithValue("@labName", obj.LabName);
                            if (GroupName[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@TestGroupName", "");
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@TestGroupName", GroupName[count]);
                            }
                            int TestID1 = Convert.ToInt32(TestID[count]);

                            cmd1.Parameters.AddWithValue("@TestID", TestID1);
                            cmd1.Parameters.AddWithValue("@TestName", TestName[count]);
                            if (Percent[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@Percentage", 0);
                            }
                            else
                            {
                                int Percent1 = Convert.ToInt32(Percent[count]);
                                cmd1.Parameters.AddWithValue("@Percentage", Percent1);
                            }
                            if (Rate[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@Rate", 0.00);
                            }
                            else
                            {
                                decimal Rate1 = Convert.ToDecimal(Rate[count]);
                                cmd1.Parameters.AddWithValue("@Rate", Rate1);
                            }
                            int GroupID1 = Convert.ToInt32(GroupID[count]);
                            cmd1.Parameters.AddWithValue("@TestGroupID", Convert.ToInt32(GroupID1));

                            cmd1.Parameters.AddWithValue("@CreationID", UserID);

                            con.Open();
                            int i = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("IUOutSourceLabRegistration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);
                    cmd.Parameters.AddWithValue("@OutSourceLabID", obj.OutSourceLabID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@labName", obj.LabName.ToUpper());
                    cmd.Parameters.AddWithValue("@ManagingBody", obj.ManagingBody);
                    cmd.Parameters.AddWithValue("@ReferenceCode", 1);
                    cmd.Parameters.AddWithValue("@Adminstrator", obj.Adminstrator);
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                    cmd.Parameters.AddWithValue("@CityID", obj.CityID);
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                    cmd.Parameters.AddWithValue("@StateID", obj.StateID);
                    cmd.Parameters.AddWithValue("@CountryID", obj.CountryID);
                    cmd.Parameters.AddWithValue("@PhoneNo", obj.PhoneNo);
                    cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", obj.EmailID);
                    cmd.Parameters.AddWithValue("@URL", obj.URL);
                    cmd.Parameters.AddWithValue("@FaxNo", obj.FaxNo);
                    cmd.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);
                

                    int row = cmd.ExecuteNonQuery();
                    obj.OutSourceLabID = Convert.ToString(cmd.Parameters["@OutSourceLabID"].Value);
                  
                    con.Close();

                    if (row > 0)
                    {
                        String[] LabtestdetailsID = obj.OutSourceLabtestdetailsID.ToString().Split(',');
                        String[] GroupID = obj.GroupID.ToString().Split(',');
                        String[] GroupName = obj.TestGroupName.Split(',');
                        String[] TestID = obj.Test.ToString().Split(',');
                        String[] TestName = obj.TestName.Split(',');
                        String[] Rate = obj.Rate.Split(',');
                        String[] Percent = obj.Percentage.Split(',');

                        for (int count = 0; count < TestName.Length; count++)
                        {
                            SqlCommand cmd1 = new SqlCommand("IUOutSourceLabtestdetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@OutSourceLabID", obj.OutSourceLabID);
                            int LabtestdetailsID1 = Convert.ToInt32(LabtestdetailsID[count]);
                            cmd1.Parameters.AddWithValue("@OutSourceLabtestdetailsID", LabtestdetailsID1);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");
                            cmd1.Parameters.AddWithValue("@labName", obj.LabName);
                            if (GroupName[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@TestGroupName", "");
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@TestGroupName", GroupName[count]);
                            }
                            int TestID1 = Convert.ToInt32(TestID[count]);

                            cmd1.Parameters.AddWithValue("@TestID", TestID1);
                            cmd1.Parameters.AddWithValue("@TestName", TestName[count]);
                            if (Percent[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@Percentage", 0);
                            }
                            else
                            {
                                int Percent1 = Convert.ToInt32(Percent[count]);
                                cmd1.Parameters.AddWithValue("@Percentage", Percent1);
                            }
                            if (Rate[count] == "")
                            {
                                cmd1.Parameters.AddWithValue("@Rate", 0.00);
                            }
                            else
                            {
                                decimal Rate1 = Convert.ToDecimal(Rate[count]);
                                cmd1.Parameters.AddWithValue("@Rate", Rate1);
                            }
                            int GroupID1 = Convert.ToInt32(GroupID[count]);
                            cmd1.Parameters.AddWithValue("@TestGroupID", Convert.ToInt32(GroupID1));

                            cmd1.Parameters.AddWithValue("@CreationID", UserID);

                        
                            con.Open();
                            int i = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                }
                
               
          
            return true;
        }

        public DataSet GetTestMasterForBindTestNameGrid()
        {
            DataSet ds = new DataSet();
            try
            {  
                Connect();
                SqlCommand cmd = new SqlCommand("GetTestMasterForBindTestNameGrid", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
               
               
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;
             
           
           
            
        }

        public DataSet GetOutSourceLab(int OutSourceLabID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetOutSourceLab", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OutSourceLabID", OutSourceLabID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;




        }

        public DataSet GetTestMasterForBindTestNameGrid1(int OutSourceLabID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetTestMasterForBindTestNameGrid1", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@OutSourceLabID", OutSourceLabID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;




        }

        public bool CheckOutSourceLab(string OutSourceLabID, string LabName)
        {
            string t = "";
            if (OutSourceLabID == null || OutSourceLabID == "")
            {
                t = "0";
            }
            else
            {
                t = OutSourceLabID;
            }
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckOutSourceLab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                cmd.Parameters.AddWithValue("@OutSourceLabID", t);
                cmd.Parameters.AddWithValue("@LabName", LabName.ToUpper());
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
        public DataSet GetAllOutSourceLab()
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("GetAllOutSourceLab", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);


                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ds;

        }

        #region Delete
        public bool DeleteOutSourceLab(int OutSourceLabID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@OutSourceLabID", SqlDbType.Int);
                aParams[1].Value = OutSourceLabID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteOutSourceLab", aParams);
            }
            catch (DBConcurrencyException exp)
            {
                ExceptionManager.Publish(exp);
                exp.Data.Add("returnValue", "-1");
                throw exp;
            }
            catch (Exception ex)
            {
                ex.Data.Add("returnValue", "0");
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return true;

        }
        #endregion

    }
}