using KeystoneProject.Models.MasterLaboratory;
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
    public class BL_XRay
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
        public List<XRay> GetTestMasterForBindCategory()
        {
            List<KeystoneProject.Models.MasterLaboratory.XRay> serachlist = new List<Models.MasterLaboratory.XRay>();

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
                    serachlist.Add(new Models.MasterLaboratory.XRay
                    {
                        CategoryID = dr["TestGroupID"].ToString(),
                        CategoryName = dr["TestGroupName"].ToString()

                    });

                }
            }
            catch (Exception ex)
            {
                return serachlist;
            }
            return serachlist;
        }

        public List<string> GetUnitID(string Unit)
        {
            Connect();
            List<string> unitlist = new List<string>();

            SqlCommand cmd = new SqlCommand("select UnitName from Unit where UnitName like ''+@UnitName+'%' and RowStatus=0 and HospitalID =" + HospitalID + " and LocationID = " + LocationID + "", con);
            cmd.Parameters.AddWithValue("@UnitName", Unit);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                unitlist.Add(Convert.ToString(dr["UnitName"]));

            }
            return unitlist;
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

        public bool SaveXRay(XRay obj)
        {
            Connect();
            con.Open();
            if (obj.TestID == "" || obj.TestID == null)
            {
                SqlCommand cmd = new SqlCommand("IUTestMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (obj.TestID == "0" || obj.TestID == "" || obj.TestID == null)
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

                cmd.Parameters.AddWithValue("@TestName", obj.ReportName);

                if (obj.HSNCode == "" || obj.HSNCode == null)
                {
                    cmd.Parameters.AddWithValue("@HSNCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode);
                }
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.unit);
                cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                cmd.Parameters.AddWithValue("@GeneralCharges", obj.OPDGeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", obj.OPDEmergencyCharges);
                cmd.Parameters.AddWithValue("@MyCost", 0);
                cmd.Parameters.AddWithValue("@ReffCommission", 0);
                cmd.Parameters.AddWithValue("@ReffCommissionRs", 0);
                cmd.Parameters.AddWithValue("@Category", obj.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", obj.CategoryName);
                cmd.Parameters.AddWithValue("@Alias", 0);
                cmd.Parameters.AddWithValue("@TestType", 0);
                cmd.Parameters.AddWithValue("@Commission", 0);
                cmd.Parameters.AddWithValue("@CommissionRs", 0);
                cmd.Parameters.AddWithValue("@Client", 0);
                cmd.Parameters.AddWithValue("@ForGender", 0);
                cmd.Parameters.AddWithValue("@Format", 0);
                cmd.Parameters.AddWithValue("@ProcessDay", 0);
                cmd.Parameters.AddWithValue("@PrintOrder", 0);
                cmd.Parameters.AddWithValue("@Methord", 0);
                cmd.Parameters.AddWithValue("@OutsideLab", 0);
                cmd.Parameters.AddWithValue("@DisposalDay", 0);
                cmd.Parameters.AddWithValue("@Discount", 0);
                cmd.Parameters.AddWithValue("@SampleType", 0);
                cmd.Parameters.AddWithValue("@PrintName", 0);
                cmd.Parameters.AddWithValue("@PrintTogether", 0);
                cmd.Parameters.AddWithValue("@WardView", 0);
                cmd.Parameters.AddWithValue("@Interpretation", 0);
                cmd.Parameters.AddWithValue("@HMSCode", 0);
                cmd.Parameters.AddWithValue("@PatientInstr", 0);

                if (obj.Description == null)
                {
                    cmd.Parameters.AddWithValue("@Footer", "");
                }
                else
                {

                    if (obj.Description == "<p><br></p>" || obj.Description == "<h1><br></h1>" || obj.Description == "<h2><br></h2>" || obj.Description == "<h3><br></h3>")
                    {
                        cmd.Parameters.AddWithValue("@Footer", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Footer", (obj.Description));
                    }


                }
                //cmd.Parameters.AddWithValue("@Footer", obj.Footer);
                cmd.Parameters.AddWithValue("@TechnicianIntr", 0);
                cmd.Parameters.AddWithValue("@BothFooter", 0);
                cmd.Parameters.AddWithValue("@MachinName", 0);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                int row = cmd.ExecuteNonQuery();

                obj.TestID = Convert.ToUInt32(cmd.Parameters["@TestID"].Value).ToString();
                con.Close();


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


                cmd.Parameters.AddWithValue("@TestName", obj.ReportName);

                if (obj.HSNCode == "" || obj.HSNCode == null)
                {
                    cmd.Parameters.AddWithValue("@HSNCode", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HSNCode", obj.HSNCode);
                }
                cmd.Parameters.AddWithValue("@ReferenceCode", obj.unit);
                cmd.Parameters.AddWithValue("@PrintAs", obj.PrintAs);
                cmd.Parameters.AddWithValue("@GeneralCharges", obj.OPDGeneralCharges);
                cmd.Parameters.AddWithValue("@EmergencyCharges", obj.OPDEmergencyCharges);
                cmd.Parameters.AddWithValue("@MyCost", 0);
                cmd.Parameters.AddWithValue("@ReffCommission", 0);
                cmd.Parameters.AddWithValue("@ReffCommissionRs", 0);
                cmd.Parameters.AddWithValue("@Category", obj.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", obj.CategoryName);
                cmd.Parameters.AddWithValue("@Alias", 0);
                cmd.Parameters.AddWithValue("@TestType", 0);
                cmd.Parameters.AddWithValue("@Commission", 0);
                cmd.Parameters.AddWithValue("@CommissionRs", 0);
                cmd.Parameters.AddWithValue("@Client", 0);
                cmd.Parameters.AddWithValue("@ForGender", 0);
                cmd.Parameters.AddWithValue("@Format", 0);
                cmd.Parameters.AddWithValue("@ProcessDay", 0);
                cmd.Parameters.AddWithValue("@PrintOrder", 0);
                cmd.Parameters.AddWithValue("@Methord", 0);
                cmd.Parameters.AddWithValue("@OutsideLab", 0);
                cmd.Parameters.AddWithValue("@DisposalDay", 0);
                cmd.Parameters.AddWithValue("@Discount", 0);
                cmd.Parameters.AddWithValue("@SampleType", 0);
                cmd.Parameters.AddWithValue("@PrintName", 0);
                cmd.Parameters.AddWithValue("@PrintTogether", 0);
                cmd.Parameters.AddWithValue("@WardView", 0);
                cmd.Parameters.AddWithValue("@Interpretation", 0);
                cmd.Parameters.AddWithValue("@HMSCode", 0);
                cmd.Parameters.AddWithValue("@PatientInstr", 0);

                if (obj.Description == null)
                {
                    cmd.Parameters.AddWithValue("@Footer", "");
                }
                else
                {

                    if (obj.Description == "<p><br></p>" || obj.Description == "<h1><br></h1>" || obj.Description == "<h2><br></h2>" || obj.Description == "<h3><br></h3>")
                    {
                        cmd.Parameters.AddWithValue("@Footer", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Footer", (obj.Description));
                    }


                }

                cmd.Parameters.AddWithValue("@TechnicianIntr", 0);
                cmd.Parameters.AddWithValue("@BothFooter", 0);
                cmd.Parameters.AddWithValue("@MachinName", 0);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                int row = cmd.ExecuteNonQuery();

                obj.TestID = Convert.ToUInt32(cmd.Parameters["@TestID"].Value).ToString();
                con.Close();
               

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
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;

        }
        public bool CheckTestMaster(string TestID, string TestName)
        {
            string t = "";
            if (TestID == null || TestID == "")
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

        public List<XRay> GetAllParameter()
        {
            List<XRay> searchList = new List<XRay>();
            Connect();

            SqlCommand cmd = new SqlCommand("GetAllXrayMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new XRay
                    {
                        TestID = item["TestID"].ToString(),
                        ReportName = item["TestName"].ToString(),
                        PrintAs = item["PrintAs"].ToString(),                      

                    });


            }
            return searchList;

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

    }
   
}