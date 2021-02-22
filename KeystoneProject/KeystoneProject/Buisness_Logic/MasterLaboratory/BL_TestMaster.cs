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
namespace KeystoneProject.Buisness_Logic.MasterLaboratory
{
    public class BL_TestMaster
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
            
        public List<TestMaster> GetTestMasterForBindCategory()
        {
            List<KeystoneProject.Models.MasterLaboratory.TestMaster> serachlist = new List<Models.MasterLaboratory.TestMaster>();
    
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
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new Models.MasterLaboratory.TestMaster
                    {
                    CategoryID = dr["TestGroupID"].ToString(),
                    Category = dr["TestGroupName"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }

        public List<TestMaster> GetTestMasterForBindSampleType()
        {
            List<KeystoneProject.Models.MasterLaboratory.TestMaster> serachlist = new List<Models.MasterLaboratory.TestMaster>();

            Connect();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("GetTestMasterForBindSampleType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                con.Open();
                ad.Fill(ds);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    serachlist.Add(new Models.MasterLaboratory.TestMaster
                    {
                        SampleTypeID =dr["SampleTypeID"].ToString(),
                        SampleType = dr["SampleType"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }

        public DataSet GetAllWard()
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllWard", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetAllHeader()
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllHeader", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetAllParameter()
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

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllParameter", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public bool CheckHeader(string HeaderName, int HeadID)
        {

            Connect();
            bool Table;
            bool flag;
            try
            {
                SqlCommand checkHearder = new SqlCommand("CheckHeader", con);
                checkHearder.CommandType = CommandType.StoredProcedure;
                checkHearder.Parameters.AddWithValue("@HospitalID", HospitalID);
                checkHearder.Parameters.AddWithValue("@LocationID", LocationID);
                checkHearder.Parameters.AddWithValue("@HeaderID", 0);
                checkHearder.Parameters.AddWithValue("@HeaderName", HeaderName);
                checkHearder.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
                checkHearder.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = checkHearder.ExecuteNonQuery();
                Table = (bool)checkHearder.Parameters["@NameExists"].Value;
                con.Close();
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
        public bool Save(TestMaster obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUHeader", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            // cmd.Parameters.AddWithValue("@QualifictionID", 0);
            if (obj.HeaderID == null)
            {
                cmd.Parameters.AddWithValue("@HeaderID", 0);
                cmd.Parameters.AddWithValue("@Mode", "Add");
            }
            else
            {
                cmd.Parameters.AddWithValue("@HeaderID", obj.HeaderID);
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }
            cmd.Parameters.AddWithValue("@ReferenceCode", 1);
            cmd.Parameters.AddWithValue("@HeaderName", obj.HeaderName);
            cmd.Parameters.AddWithValue("@CreationID", UserID);
            //  cmd.Parameters.AddWithValue("@Mode", "Add");
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
        public bool EditTestMaster(string TestID)
        {
            Connect();


            return true;
        }


        public bool SaveTest(TestMaster obj)
        {
            Connect();
            con.Open();
            if (obj.TestID == "" || obj.TestID == null)
            {


                SqlCommand cmd = new SqlCommand("IUTestMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.TestID == "0" || obj.TestID == "")
                {
                    cmd.Parameters.AddWithValue("@TestID", 0);
                    cmd.Parameters["@TestID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TestID", obj.TestID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");

                }

                cmd.Parameters.AddWithValue("@TestName", obj.TestName);

                if (obj.HSNCode =="" || obj.HSNCode ==null)
                {
                    cmd.Parameters.AddWithValue("@HSNCode", 0); 
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode); 
                }
                cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                cmd.Parameters.AddWithValue("@GeneralCharges", obj.OPDGeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", obj.OPDEmergencyCharges);
                cmd.Parameters.AddWithValue("@MyCost", 0);
                cmd.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                cmd.Parameters.AddWithValue("@ReffCommissionRs", obj.ReffCommissionRs);
                cmd.Parameters.AddWithValue("@Category", obj.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", obj.Category);
                cmd.Parameters.AddWithValue("@Alias", obj.Alias);
                cmd.Parameters.AddWithValue("@TestType", obj.TestType);
                cmd.Parameters.AddWithValue("@Commission", obj.Commission);
                cmd.Parameters.AddWithValue("@CommissionRs", obj.CommissionRs);
                cmd.Parameters.AddWithValue("@Client", obj.Client);
                cmd.Parameters.AddWithValue("@ForGender", obj.ForGender);
                cmd.Parameters.AddWithValue("@Format", obj.Format);
                cmd.Parameters.AddWithValue("@ProcessDay", obj.ProcessBy);
                cmd.Parameters.AddWithValue("@PrintOrder", obj.PrintOrder);
                cmd.Parameters.AddWithValue("@Methord", obj.Methord);
                cmd.Parameters.AddWithValue("@OutsideLab", obj.OutsideLab);
                cmd.Parameters.AddWithValue("@DisposalDay", obj.DisposalDay);
                cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                cmd.Parameters.AddWithValue("@SampleType", obj.SampleType);
                cmd.Parameters.AddWithValue("@PrintName", obj.PrintName);
                cmd.Parameters.AddWithValue("@PrintTogether", obj.PrintTogether);
                cmd.Parameters.AddWithValue("@WardView", obj.WardView);
                cmd.Parameters.AddWithValue("@Interpretation", obj.Interpretation);
                cmd.Parameters.AddWithValue("@HMSCode", obj.HMSCode);
                cmd.Parameters.AddWithValue("@PatientInstr", obj.PatientInstr);
                cmd.Parameters.AddWithValue("@Footer", obj.Footer);
                cmd.Parameters.AddWithValue("@TechnicianIntr", obj.TechnicianIntr);
                cmd.Parameters.AddWithValue("@BothFooter", obj.BothFooter);
                cmd.Parameters.AddWithValue("@MachinName", obj.MachinName);
                cmd.Parameters.AddWithValue("@CreationID", UserID);




                int row = cmd.ExecuteNonQuery();

                obj.TestID = Convert.ToUInt32(cmd.Parameters["@TestID"].Value).ToString();
                con.Close();
                //      var delimiters = new char[] { ','};

                #region IUTestMasterDetails
                if (row > 0)
                {
                    if (obj.HeaderParameterID != null)
                    {


                        int TestMasterDetails = 0;
                        //string[] ParameterName = obj.ParameterName.Split(',');
                        //string[] HeaderParmater = obj.ParameterID.Split(',');
                        //string[] Type = obj.Type.Split(',');
                        //string[] Space = obj.Space.Split(',');
                        // string[] HederID =obj.HeaderID.Split(',') ;
                        string HederID = "1";
                        for (int count = 0; count < obj.ParameterHeaderName.Length; count++)
                        {
                            SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand("IUTestMasterDetails", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                            if (obj.TestDetailsID == null || obj.TestDetailsID == "")
                            {
                                cmd1.Parameters.AddWithValue("@TestDetailsID", 0);
                                //  cmd1.Parameters["@TestDetailsID"].Direction = ParameterDirection.Output;
                                cmd1.Parameters.AddWithValue("@Mode", "Add");
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@TestDetailsID", obj.TestDetailsID);
                                cmd1.Parameters.AddWithValue("@Mode", "Edit");
                            }
                            //  cmd1.Parameters.AddWithValue("@TestDetailsID", 0);
                            cmd1.Parameters.AddWithValue("@TestID", obj.TestID);

                            // ParamiterID_HederID[count].ToString()
                            if (obj.Type[count].ToString() == "H")
                            {
                                HederID = obj.HeaderParameterID[count];
                            }
                            if (obj.Type[count].ToString() == "P")
                            {
                                cmd1.Parameters.AddWithValue("@ParameterName", obj.ParameterHeaderName[count]);

                                cmd1.Parameters.AddWithValue("@HeaderParmater", obj.HeaderParameterID[count]);
                                cmd1.Parameters.AddWithValue("@HeaderID", obj.HeaderParameterID[count]);
                            }
                            else
                            {


                                cmd1.Parameters.AddWithValue("@HeaderID", HederID);
                                cmd1.Parameters.AddWithValue("@ParameterName", obj.ParameterHeaderName[count]);

                                cmd1.Parameters.AddWithValue("@HeaderParmater", obj.HeaderParameterID[count]);
                            }
                            cmd1.Parameters.AddWithValue("@Type", obj.Type[count]);
                            cmd1.Parameters.AddWithValue("@Space", obj.Space[count]);
                            cmd1.Parameters.AddWithValue("@Order", "");
                            cmd1.Parameters.AddWithValue("@CreationID", UserID);


                            con.Open();
                            TestMasterDetails = cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                #endregion

                    #region IUTestMasterDetailIPDCharge
                    int IUTestMasterDetailIPDCharge = 0;

                    //if (TestMasterDetails > 0)
                    //{
                        if (obj.WardID != "0")
                        {


                            string[] wardID = obj.WardID.Split(',');
                            string[] wordname = obj.WardName.Split(',');
                            string[] gencharge = obj.GenralWardIDPCharges.Split(',');
                            string[] emergencycharge = obj.EmergencyIPDCharges.Split(',');
                            for (int count = 0; count < wordname.Length; count++)
                            {
                                SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand("IUTestMasterDetailIPDCharge", con);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd2.Parameters.AddWithValue("@WardID", wardID[count]);
                                cmd2.Parameters.AddWithValue("@WardName", wordname[count]);
                                cmd2.Parameters.AddWithValue("@GeneralCharges", gencharge[count]);
                                cmd2.Parameters.AddWithValue("@EmergencyCharges", emergencycharge[count]);
                                cmd2.Parameters.AddWithValue("@TestMasterDetailIPDChargeID", 0);
                                cmd2.Parameters.AddWithValue("@TestID", obj.TestID);
                                cmd2.Parameters.AddWithValue("@CreationID", UserID);
                                cmd2.Parameters.AddWithValue("@Mode", "Add");
                                con.Open();
                                IUTestMasterDetailIPDCharge = cmd2.ExecuteNonQuery();
                                con.Close();
                            }
                    #endregion


                            #region IUTestMasterTPAWise
                            DataSet ds = GetTestTPAWiseForOrganizationID(HospitalID, LocationID);

                            int IUTestMasterTPAWise = 0;
                            if (IUTestMasterDetailIPDCharge > 0)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    SqlCommand cmd3 = new System.Data.SqlClient.SqlCommand("IUTestMasterTPAWise", con);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd3.Parameters.AddWithValue("@LocationID", LocationID);
                                    if (obj.TestTPAWiseID == null || obj.TestTPAWiseID == "")
                                    {
                                        cmd3.Parameters.AddWithValue("@TestTPAWiseID", 0);
                                        cmd3.Parameters.AddWithValue("@Mode", "Add");
                                    }
                                    else
                                    {
                                        cmd3.Parameters.AddWithValue("@TestTPAWiseID", obj.TestTPAWiseID);
                                        cmd3.Parameters.AddWithValue("@Mode", "Edit");
                                    }

                                    cmd3.Parameters.AddWithValue("@OrganizationID", dr["OrganizationID"]);
                                    cmd3.Parameters.AddWithValue("@TestID", obj.TestID);
                                    cmd3.Parameters.AddWithValue("@RecommendedByDoctor", 0);
                                    cmd3.Parameters.AddWithValue("@GeneralCharges", obj.OPDGeneralCharges);
                                    if (obj.EmergencyCharges == null)
                                    {
                                        cmd3.Parameters.AddWithValue("@EmergencyCharges", "");

                                    }
                                    else
                                    {
                                        cmd3.Parameters.AddWithValue("@EmergencyCharges", obj.OPDEmergencyCharges);

                                    }
                                    cmd3.Parameters.AddWithValue("@CreationID", UserID);


                                    con.Open();

                                    IUTestMasterTPAWise = cmd3.ExecuteNonQuery();

                                    con.Close();
                                }
                            }
                            #endregion

                            DataSet dsTestTPAWiseDetail = GetTestTPAWiseDetailForOrganizationID(HospitalID, Convert.ToInt32(LocationID), Convert.ToInt32(obj.TestID));
                            //string[] Ward = obj.WardID.Split(',');
                            if (IUTestMasterTPAWise > 0)
                            {
                                foreach (DataRow dr in dsTestTPAWiseDetail.Tables[0].Rows)
                                {
                                    SqlCommand cmd4 = new System.Data.SqlClient.SqlCommand("IUTestMasterDetailsTPAWise", con);
                                    cmd4.CommandType = CommandType.StoredProcedure;
                                    cmd4.Parameters.AddWithValue("@HospitalID", HospitalID);
                                    cmd4.Parameters.AddWithValue("@LocationID", LocationID);
                                    cmd4.Parameters.AddWithValue("@TestDetailsTPAWiseID", dr["TestDetailsTPAWiseID"]);
                                    cmd4.Parameters.AddWithValue("@TestTPAWiseID", dr["TestTPAWiseID"]);
                                    cmd4.Parameters.AddWithValue("@WardID", dr["WardID"]);
                                    cmd4.Parameters.AddWithValue("@GeneralCharges", dr["GeneralCharges"]);
                                    cmd4.Parameters.AddWithValue("@EmergencyCharges", dr["EmergencyCharges"]);
                                    cmd4.Parameters.AddWithValue("@CreationID", UserID);
                                    cmd4.Parameters.AddWithValue("@Mode", "Add");

                                    con.Open();
                                    int IUTestMasterDetailsTPAWise = cmd4.ExecuteNonQuery();
                                    con.Close();

                                }
                            }
                        }
                    }
                    obj.TestID = Convert.ToUInt32(cmd.Parameters["@TestID"].Value).ToString();
                    DataSet dsDoctorToTestCharge = GetDoctorToTestCharges(HospitalID, LocationID);
                    foreach (DataRow dr in dsDoctorToTestCharge.Tables[0].Rows)
                    {

                        SqlCommand cmdDoctor = new System.Data.SqlClient.SqlCommand("IUTestChargesByDoctor", con);
                        cmdDoctor.CommandType = CommandType.StoredProcedure;
                        cmdDoctor.Parameters.AddWithValue("@HospitalID", dr["HospitalID"]);
                        cmdDoctor.Parameters.AddWithValue("@LocationID", dr["LocationID"]);
                        cmdDoctor.Parameters.AddWithValue("@DoctorID", dr["DoctorID"]);
                        cmdDoctor.Parameters.AddWithValue("@TestChargesByDoctorID", dr["TestChargesByDoctorID"] = 0);
                        cmdDoctor.Parameters.AddWithValue("@TestID", dr["TestID"] = obj.TestID);
                        cmdDoctor.Parameters.AddWithValue("@GeneralCharges", dr["GeneralCharges"]);
                        cmdDoctor.Parameters.AddWithValue("@EmergencyCharges", dr["EmergencyCharges"]);
                        cmdDoctor.Parameters.AddWithValue("@CreationID", dr["CreationID"]);
                        cmdDoctor.Parameters.AddWithValue("@Mode", dr["Mode"] = "Add");

                        con.Open();
                        int IUTestChargesByDoctor = cmdDoctor.ExecuteNonQuery();
                        con.Close();
                    }







                //}
            }
            else
            { 
                try
                {
                    SqlCommand cmd = new SqlCommand("IUTestMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);


                    cmd.Parameters.AddWithValue("@TestID", obj.TestID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");



                    cmd.Parameters.AddWithValue("@TestName", obj.TestName);
                    if (obj.HSNCode == "" || obj.HSNCode == null)
                    {
                        cmd.Parameters.AddWithValue("@HSNCode", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode);
                    }
                    cmd.Parameters.AddWithValue("@ReferenceCode", 0);
                    cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                    cmd.Parameters.AddWithValue("@GeneralCharges", obj.OPDGeneralCharges);
                    cmd.Parameters.AddWithValue("@EmergencyCharges", obj.OPDEmergencyCharges);
                    cmd.Parameters.AddWithValue("@MyCost", 0);
                    cmd.Parameters.AddWithValue("@ReffCommission", obj.ReffCommission);
                    cmd.Parameters.AddWithValue("@ReffCommissionRs", obj.ReffCommissionRs);
                    cmd.Parameters.AddWithValue("@Category", obj.CategoryID);
                    cmd.Parameters.AddWithValue("@CategoryName", obj.Category);
                    cmd.Parameters.AddWithValue("@Alias", obj.Alias);
                    cmd.Parameters.AddWithValue("@TestType", obj.TestType);
                    cmd.Parameters.AddWithValue("@Commission", obj.Commission);
                    cmd.Parameters.AddWithValue("@CommissionRs", obj.CommissionRs);
                    cmd.Parameters.AddWithValue("@Client", obj.Client);
                    cmd.Parameters.AddWithValue("@ForGender", obj.ForGender);
                    cmd.Parameters.AddWithValue("@Format", obj.Format);
                    cmd.Parameters.AddWithValue("@ProcessDay", obj.ProcessBy);
                    cmd.Parameters.AddWithValue("@PrintOrder", obj.PrintOrder);
                    cmd.Parameters.AddWithValue("@Methord", obj.Methord);
                    cmd.Parameters.AddWithValue("@OutsideLab", obj.OutsideLab);
                    cmd.Parameters.AddWithValue("@DisposalDay", obj.DisposalDay);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@SampleType", obj.SampleType);
                    cmd.Parameters.AddWithValue("@PrintName", obj.PrintName);
                    cmd.Parameters.AddWithValue("@PrintTogether", obj.PrintTogether);
                    cmd.Parameters.AddWithValue("@WardView", obj.WardView);
                    cmd.Parameters.AddWithValue("@Interpretation", obj.Interpretation);
                    cmd.Parameters.AddWithValue("@HMSCode", obj.HMSCode);
                    cmd.Parameters.AddWithValue("@PatientInstr", obj.PatientInstr);
                    cmd.Parameters.AddWithValue("@Footer", obj.Footer);
                    cmd.Parameters.AddWithValue("@TechnicianIntr", obj.TechnicianIntr);
                    cmd.Parameters.AddWithValue("@BothFooter", obj.BothFooter);
                    cmd.Parameters.AddWithValue("@MachinName", obj.MachinName);
                    cmd.Parameters.AddWithValue("@CreationID", UserID);




                    int row = cmd.ExecuteNonQuery();

                    obj.TestID = Convert.ToUInt32(cmd.Parameters["@TestID"].Value).ToString();
                    con.Close();

                    #region IUTestMasterDetails
                    if (row > 0)
                    {
                        if (obj.HeaderParameterID != null)
                        {


                            int TestMasterDetails = 0;
                            //string[] ParameterName = obj.ParameterName.Split(',');
                            //string[] HeaderParmater = obj.ParameterID.Split(',');
                            //string[] Type = obj.Type.Split(',');
                            //string[] Space = obj.Space.Split(',');
                            // string[] HederID =obj.HeaderID.Split(',') ;
                            string HederID = "1";
                            for (int count = 0; count < obj.ParameterHeaderName.Length; count++)
                            {
                                SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand("IUTestMasterDetails", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                                if (obj.TestDetailsID == null || obj.TestDetailsID == "")
                                {
                                    cmd1.Parameters.AddWithValue("@TestDetailsID", 0);
                                    //  cmd1.Parameters["@TestDetailsID"].Direction = ParameterDirection.Output;
                                    cmd1.Parameters.AddWithValue("@Mode", "Add");
                                }
                                else
                                {
                                    cmd1.Parameters.AddWithValue("@TestDetailsID", obj.TestDetailsID);
                                    cmd1.Parameters.AddWithValue("@Mode", "Edit");
                                }
                                //  cmd1.Parameters.AddWithValue("@TestDetailsID", 0);
                                cmd1.Parameters.AddWithValue("@TestID", obj.TestID);

                                // ParamiterID_HederID[count].ToString()
                                if (obj.Type[count].ToString() == "H")
                                {
                                    HederID = obj.HeaderParameterID[count];
                                }
                                if (obj.Type[count].ToString() == "P")
                                {
                                    cmd1.Parameters.AddWithValue("@ParameterName", obj.ParameterHeaderName[count]);

                                    cmd1.Parameters.AddWithValue("@HeaderParmater", obj.HeaderParameterID[count]);
                                    cmd1.Parameters.AddWithValue("@HeaderID", obj.HeaderParameterID[count]);
                                }
                                else
                                {


                                    cmd1.Parameters.AddWithValue("@HeaderID", HederID);
                                    cmd1.Parameters.AddWithValue("@ParameterName", obj.ParameterHeaderName[count]);

                                    cmd1.Parameters.AddWithValue("@HeaderParmater", obj.HeaderParameterID[count]);
                                }
                                cmd1.Parameters.AddWithValue("@Type", obj.Type[count]);
                                cmd1.Parameters.AddWithValue("@Space", obj.Space[count]);
                                cmd1.Parameters.AddWithValue("@Order", "");
                                cmd1.Parameters.AddWithValue("@CreationID", UserID);


                                con.Open();
                                TestMasterDetails = cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        #endregion

                        #region IUTestMasterDetailIPDCharge
                        int IUTestMasterDetailIPDCharge;

                        //if (TestMasterDetails > 0)
                        //{
                        if (obj.WardID != "0")
                        {


                            string[] wardID = obj.WardID.Split(',');
                            string[] wordname = obj.WardName.Split(',');
                            string[] gencharge = obj.GenralWardIDPCharges.Split(',');
                            string[] emergencycharge = obj.EmergencyIPDCharges.Split(',');
                            string[] TestMasterDetailIPDChargesID = obj.TestMasterDetailIPDChargeID.Split(',');
                            for (int count = 0; count < wordname.Length; count++)
                            {
                                SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand("IUTestMasterDetailIPDCharge", con);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                                cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                                cmd2.Parameters.AddWithValue("@WardID", wardID[count]);
                                cmd2.Parameters.AddWithValue("@WardName", wordname[count]);
                                cmd2.Parameters.AddWithValue("@GeneralCharges", gencharge[count]);
                                cmd2.Parameters.AddWithValue("@EmergencyCharges", emergencycharge[count]);
                                cmd2.Parameters.AddWithValue("@TestMasterDetailIPDChargeID", TestMasterDetailIPDChargesID[count]);
                                cmd2.Parameters.AddWithValue("@TestID", obj.TestID);
                                cmd2.Parameters.AddWithValue("@CreationID", UserID);
                                cmd2.Parameters.AddWithValue("@Mode", "Edit");
                                con.Open();
                                IUTestMasterDetailIPDCharge = cmd2.ExecuteNonQuery();
                                con.Close();
                            }
                            #endregion
                            //}

                        }

                    }
                }
                catch(Exception ex)
                {
                    return false;
                }
           
            }
           
                return true;
           
        }

        #region Delete
        public bool DeleteTestMaster(int HospitalID, int TestID, int LocationID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[3];
                aParams[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                aParams[0].Value = HospitalID;
                aParams[1] = new SqlParameter("@TestID", SqlDbType.Int);
                aParams[1].Value = TestID;
                aParams[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                aParams[2].Value = LocationID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteTestMaster", aParams);
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


        public DataSet GetDoctorToTestCharges(int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
             
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetDoctorToTestCharges", param);
                //ds = SqlHelper.ExecuteDataset(_sDbConnection, CommandType.StoredProcedure, "GetDoctorToTestCharges", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
           
        }
             public DataSet GetTestTPAWiseDetailForOrganizationID(int HospitalID, int LocationID, int TestID)
        {
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@TestID", SqlDbType.Int);
                param[2].Value = TestID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestTPAWiseDetailForOrganizationID", param);
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
        }
       public DataSet GetTestTPAWiseForOrganizationID(int HospitalID, int LocationID)
        {
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestTPAWiseForOrganizationID", param);
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
        }
        public DataSet GetParameterForTestMaster(int HospitalID, int LocationID, String SearchName)
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
                param[2] = new SqlParameter("@SearchName", SqlDbType.NVarChar, 50);
                param[2].Value = SearchName;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetParameterForTestMaster", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }

        public DataSet GetTestMasterDetailIPDCharge(int TestID)
        {
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                param[2] = new SqlParameter("@TestID", SqlDbType.Int);
                param[2].Value = TestID;
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestMasterDetailIPDCharge", param);
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
        }
        public DataSet GetAllTestMaster(int HospitalID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllTestMaster", param);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetTestMaster(int TestID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@TestID", SqlDbType.Int);
                param[1].Value = TestID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestMaster", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
            return ds;
        }
        public bool CheckTestMaster(string TestID, string TestName)
        {
             string t="";
            if(TestID==null || TestID=="")
            {
                 t = "0";
            }
            else
            {
                t = TestID;
            }
            Connect();
            bool Table;
            bool flag;
            try
            {
                SqlCommand checkHearder = new SqlCommand("CheckTestMaster", con);
                checkHearder.CommandType = CommandType.StoredProcedure;
                checkHearder.Parameters.AddWithValue("@HospitalID", HospitalID);
                checkHearder.Parameters.AddWithValue("@LocationID", LocationID);
                checkHearder.Parameters.AddWithValue("@TestID", t);
                checkHearder.Parameters.AddWithValue("@TestName", TestName);
                checkHearder.Parameters.Add("@NameExists", SqlDbType.Bit, 50);
                checkHearder.Parameters["@NameExists"].Direction = ParameterDirection.Output;
                con.Open();
                int i = checkHearder.ExecuteNonQuery();
                Table = (bool)checkHearder.Parameters["@NameExists"].Value;
                con.Close();
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
        public DataSet GetTestMasterDetail(int HospitalID, int TestID, int LocationID)
        {
            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@TestID", SqlDbType.Int);
                param[1].Value = TestID;
                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTestMasterDetails", param);
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