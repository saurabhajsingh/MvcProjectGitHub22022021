using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KeystoneProject.Buisness_Logic.Master
{
    public class BL_Prefix
    {
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        List<Prefix> PrefixList = new List<Prefix>();

        private SqlConnection con;
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public bool Save(Prefix obj)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("IUPrefixMasterSetting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);

            cmd.Parameters.AddWithValue("@PrefixMasterSettingID", obj.PrefixMasterSettingID);

            if (obj.PrefixMasterSettingID == 0)
            {
                cmd.Parameters.AddWithValue("@Mode", "ADD");
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mode", "Edit");
            }

            if (obj.ChkUHIDNo == null)
                cmd.Parameters.AddWithValue("@ChkUHIDNo", false);
            else
                cmd.Parameters.AddWithValue("@ChkUHIDNo", true);

            if (obj.ChkPatientOPD == null)
                cmd.Parameters.AddWithValue("@ChkPatientOPD", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientOPD", true);

            if (obj.ChkPatientOPDBillPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientOPDBillPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientOPDBillPrefix", true);

            if (obj.ChkPatientIPDPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientIPDPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientIPDPrefix", true);

            if (obj.ChkPatientIPDBillPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientIPDBillPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientIPDBillPrefix", true);

            if (obj.ChkPatientProvisionalPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientProvisionalPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientProvisionalPrefix", true);

            if (obj.ChkPatientFinalBillPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientFinalBillPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientFinalBillPrefix", true);


            if (obj.ChkPatientLabPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientLabPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientLabPrefix", true);

            if (obj.ChkPatientLabBillPrefix == null)
                cmd.Parameters.AddWithValue("@ChkPatientLabBillPrefix", false);
            else
                cmd.Parameters.AddWithValue("@ChkPatientLabBillPrefix", true);

            cmd.Parameters.AddWithValue("@PatientOpd", obj.PatientOpd);
            cmd.Parameters.AddWithValue("@PatientOPDBill", obj.PatientOPDBill);
            cmd.Parameters.AddWithValue("@PatientIpd", obj.PatientIpd);
            cmd.Parameters.AddWithValue("@PatientIPDBill", obj.PatientIPDBill);
            cmd.Parameters.AddWithValue("@PatientProvisionalBill", obj.PatientProvisionalBill);
            cmd.Parameters.AddWithValue("@PatientFinalBill", obj.PatientFinalBill);
            cmd.Parameters.AddWithValue("@PatientLab", obj.PatientLab);
            cmd.Parameters.AddWithValue("@PatientLabBill", obj.PatientLabBill);

            cmd.Parameters.AddWithValue("@CompanyName", obj.HospitalName);

            cmd.Parameters.AddWithValue("@StartingUHIDNo", obj.StartingUHIDNo);

            cmd.Parameters.AddWithValue("@StartingOpdNo", obj.StartingOpdNo);
            cmd.Parameters.AddWithValue("@StartingPatientOPDBillNo", obj.StartingPatientOPDBillNo);
            cmd.Parameters.AddWithValue("@StartingPatientIPD", obj.StartingPatientIPD);
            cmd.Parameters.AddWithValue("@StartingIPDBillNo", obj.StartingIPDBillNo);
            cmd.Parameters.AddWithValue("@StartingPatientProvisionalBillNo", obj.StartingPatientProvisionalBillNo);
            cmd.Parameters.AddWithValue("@StartingFinalBillNo", obj.StartingFinalBillNo);

            cmd.Parameters.AddWithValue("@StartingLabNo", obj.StartingLabNo);
            cmd.Parameters.AddWithValue("@StartingLabBillNo", obj.StartingLabBillNo);

            cmd.Parameters.AddWithValue("@FinancialYearID", obj.FinancialYearID);

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

        public List<Prefix> SelectAllData()
        {
            Connect();


            SqlCommand cmd = new SqlCommand("GetPrefixMasterSetting", con);
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
                if (dr["StartingLabNo"].ToString() == "")
                {
                    dr["StartingLabNo"] = 0;
                }
                PrefixList.Add(
                    new Prefix
                    {

                        PrefixMasterSettingID = Convert.ToInt32(dr["PrefixMasterSettingID"]),
                        ChkPatientOPD = dr["ChkPatientOPD"].ToString(),
                        ChkPatientOPDBillPrefix = dr["ChkPatientOPDBillPrefix"].ToString(),
                        ChkPatientIPDPrefix = dr["ChkPatientIPDPrefix"].ToString(),
                        ChkPatientIPDBillPrefix = dr["ChkPatientIPDBillPrefix"].ToString(),
                        ChkPatientProvisionalPrefix = dr["ChkPatientProvisionalPrefix"].ToString(),
                        ChkPatientFinalBillPrefix = dr["ChkPatientFinalBillPrefix"].ToString(),
                        ChkPatientLabPrefix = dr["ChkPatientLabPrefix"].ToString(),
                        ChkPatientLabBillPrefix = dr["ChkPatientLabBillPrefix"].ToString(),
                        PatientOpd = dr["PatientOpd"].ToString(),
                        PatientOPDBill = dr["PatientOPDBill"].ToString(),
                        PatientIpd = dr["PatientIpd"].ToString(),
                        PatientIPDBill = dr["PatientIPDBill"].ToString(),
                        PatientProvisionalBill = dr["PatientProvisionalBill"].ToString(),
                        PatientFinalBill = dr["PatientFinalBill"].ToString(),
                        PatientLab = dr["PatientLab"].ToString(),
                        PatientLabBill = dr["PatientLabBill"].ToString(),
                        StartingUHIDNo = Convert.ToInt32(dr["StartingUHIDNo"]),
                        StartingOpdNo = Convert.ToInt32(dr["StartingOpdNo"]),                       
                        StartingPatientOPDBillNo = Convert.ToInt32(dr["StartingPatientOPDBillNo"]),
                        StartingPatientIPD = Convert.ToInt32(dr["StartingPatientIPD"]),
                        StartingIPDBillNo = Convert.ToInt32(dr["StartingIPDBillNo"]),
                        StartingFinalBillNo = Convert.ToInt32(dr["StartingFinalBillNo"]),
                        StartingPatientProvisionalBillNo = Convert.ToInt32(dr["StartingPatientProvisionalBillNo"]),
                        StartingLabNo = Convert.ToInt32(dr["StartingLabNo"]),
                        StartingLabBillNo = Convert.ToInt32(dr["StartingLabBillNo"]),
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                        HospitalName = dr["CompanyName"].ToString(),


                    });
            }
            return PrefixList;
        }

        public bool CheckPrefixMasterSetting(int PrintRegNO, int FinancialYearID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("CheckPrefixMasterSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintRegNO", PrintRegNO);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
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

        public bool ChkStartingOpdNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingOpdNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintRegNO", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingPatientOPDBillNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingPatientOPDBillNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintBillNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingPatientIPD(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingPatientIPD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintIPDNO", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingIPDBillNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingIPDBillNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintBillNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingPatientProvisionalBillNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingPatientProvisionalBillNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintProvisionalBillNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingFinalBillNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingFinalBillNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintBillNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingLabNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingLabNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintLabNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public bool ChkStartingLabBillNo(int StartingOPD, int FinancialID)
        {
            Connect();
            string Table;
            bool flag;
            try
            {
                SqlCommand cmd = new SqlCommand("ChkStartingLabBillNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrintBillNo", StartingOPD);
                cmd.Parameters.AddWithValue("@FinancialYearID", FinancialID);
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

        public DataSet ChkUHIDNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintRegNO) as 'MaxPrintRegNO', min(PrintRegNO) as 'MinPrintRegNO' from Patient where HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkOPDNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintOPDNo) as 'MaxPrintRegNO', min(PrintOPDNo) as 'MinPrintRegNO' from PatientOPDDetails where HospitalID = " + HospitalID + " and FinancialYearID ="+ FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkOPDBill(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintBillNo) as 'MaxPrintRegNO', min(PrintBillNo) as 'MinPrintRegNO' from PatientBills where BillType = 'OPDBill' and FinancialYearID =" + FinancialID + " and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkIPDNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintIPDNO) as 'MaxPrintRegNO', min(PrintIPDNO) as 'MinPrintRegNO' from PatientIPDDetails where HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkIPDBillNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintBillNo) as 'MaxPrintRegNO', min(PrintBillNo) as 'MinPrintRegNO' from PatientBills where BillType = 'IPDBill' and HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkProvisionalBillNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(ProvisionalBillNo) as 'MaxPrintRegNO', min(ProvisionalBillNo) as 'MinPrintRegNO' from ProvisionalBills where  HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkFinalBillNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintBillNo) as 'MaxPrintRegNO', min(PrintBillNo) as 'MinPrintRegNO' from PatientBills where  BillType = 'IPDFinalBill'  and HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkLabNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintLabNo) as 'MaxPrintRegNO', min(PrintLabNo) as 'MinPrintRegNO' from PatientLab where  HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet ChkLabBillNo(int FinancialID)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("select MAX(PrintBillNo) as 'MaxPrintRegNO', min(PrintBillNo) as 'MinPrintRegNO' from PatientBills where BillType = 'OPDLabBills' and  HospitalID = " + HospitalID + " and FinancialYearID =" + FinancialID + " and LocationID = " + LocationID + "", con);
            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
    }
}