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

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_TestMasterTPAWise
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        private SqlConnection con;
        public DataSet GetAllTestMasterTPAWise()
        {
            connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllTestMasterTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

            }
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
        //public DataSet GetTestMasterTPAWise(int TestTPAWiseID)
        //{
        //    connect();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("GetTestMasterTPAWise", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@TestTPAWiseID", TestTPAWiseID);
        //        con.Open();
        //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //        ad.Fill(ds);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return ds;
        //}
        public DataSet GetTestMasterDetailsTPAWise(int TestTPAWiseID)
        {
            connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTestMasterDetailsTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestTPAWiseID", TestTPAWiseID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetTpaName(string prefix)
        {
            connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select  OrganizationID,OrganizationName  from  Organization  where OrganizationName  like '" + prefix + "%' and RowStatus=0 and HospitalID =" + 1 + " and LocationID =" + 1 + "and OrganizationID !=0  and OrganizationType ='T.P.A' order by  OrganizationName asc", con);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetTestMasterDetailsTPAWise1(int TestTPAWiseID)
        {

            connect();
            SqlCommand cmd = new SqlCommand("GetTestMasterDetailsTPAWise", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@TestTPAWiseID", TestTPAWiseID);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public DataSet GetAllWardName()
        {

            connect();
            SqlCommand cmd = new SqlCommand("GetAllWard", con);

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

        public DataSet GetTestMasterTPAWiseDetails( string TPAID)
        {

            connect();
            SqlCommand cmd = new SqlCommand("GetTestMasterTPAWise", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@HospitalID", HospitalID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@LocationID", LocationID); // i will pass zero to MobileID beacause its Primary .
            cmd.Parameters.AddWithValue("@TestTPAWiseID", TPAID); 
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            return ds;

        }
        public DataSet SelectAllTestTPAWise()
        {
            connect();
            DataSet dsServiceTPAWise = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetAllTestMasterTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                cmd.CommandTimeout = 70000;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dsServiceTPAWise = new DataSet();
                da.Fill(dsServiceTPAWise);
                con.Close();

                return dsServiceTPAWise;
            }
            catch (Exception)
            {
                return dsServiceTPAWise;
            }

        }

        public DataSet GetTestName(string TPAID)
        {
            connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("select TestMasterTPAWise.TestTPAWiseID ,TestMaster.TestName,TestMaster.TestID  from TestMasterTPAWise left join TestMaster on TestMaster.TestID = TestMasterTPAWise.TestID where  TestMasterTPAWise.OrganizationID   = " + TPAID + " and  TestMasterTPAWise.HospitalID =" + 1 + " and TestMasterTPAWise.LocationID =" + 1 + " and TestMasterTPAWise.RowStatus = 0 order by  TestMaster.TestName asc", con);
                cmd.Parameters.AddWithValue("@TPAID", TPAID);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            { }
            return ds;

        }

        public bool Edit(TestMasterTPAWise objmodel)
        {
            connect();
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("IUTestMasterTPAWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@TestTPAWiseID", objmodel.TestTPAWiseID);
                cmd.Parameters.AddWithValue("@OrganizationID", "");
                cmd.Parameters.AddWithValue("@TestID", "");
                cmd.Parameters.AddWithValue("@GeneralCharges", objmodel.GeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", objmodel.EmergencyCharges);
                cmd.Parameters.AddWithValue("@RecommendedByDoctor", objmodel.RecommendedByDoctor);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
               
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    try
                    {
                        int k = 0;
                        WardDetail_class[] ward123 = objmodel.ward;
                        for (int j = 0; j < ward123.Length; j++)
                        
                        {

                            SqlCommand cmd1 = new SqlCommand("IUTestMasterDetailsTPAWise", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            cmd1.Parameters.AddWithValue("@TestDetailsTPAWiseID", ward123[j].TestDetailsTPAWiseID);
                           
                            //cmd1.Parameters["@TestDetailsTPAWiseID"].Direction = ParameterDirection.Output;


                            cmd1.Parameters.AddWithValue("@TestTPAWiseID", objmodel.TestTPAWiseID);
                            cmd1.Parameters.AddWithValue("@WardID", ward123[j].WardId);
                            cmd1.Parameters.AddWithValue("@GeneralCharges", ward123[j].GeneralCharges);
                            cmd1.Parameters.AddWithValue("@EmergencyCharges", ward123[j].EmergencyCharges);
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);
                            cmd1.Parameters.AddWithValue("@Mode", "Edit");
                            con.Open();
                             k = cmd1.ExecuteNonQuery();
                             con.Close();
                        }
                      
                    }
                        
                    catch (Exception ex)
                    {
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
        public DataSet GetTestDetailsTPAWiseID(int TestTPAWiseID)
        {
            DataSet ds = new DataSet();
            try
            {
                connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@TestTPAWiseID", SqlDbType.Int);
                param[0].Value = TestTPAWiseID;
                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestDetailsTPAWiseID", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetTPAWiseID(int TestTPAWiseID)
        {

            DataSet ds = new DataSet();
            try
            {
                connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@TestTPAWiseID", SqlDbType.Int);
                param[1].Value = TestTPAWiseID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestTPAWiseID", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
    }

}