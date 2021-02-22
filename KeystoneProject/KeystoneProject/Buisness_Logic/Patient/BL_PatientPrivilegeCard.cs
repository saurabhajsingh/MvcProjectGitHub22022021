using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Models.Patient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Buisness_Logic.Patient
{
    public class BL_PatientPrivilegeCard
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        int PrivilegeCardID=Convert.ToInt32(HttpContext.Current.Session["PrivilegeCardID"]);
       // DateTime FromDate = Convert.ToDateTime(HttpContext.Current.Session["ValidDate"]);
       // DateTime ToDate = Convert.ToDateTime(HttpContext.Current.Session["PrivilegeDate"]);
        List<PatientPrivilegeCard> PatientPrivilegeCardList = new List<PatientPrivilegeCard>();
        PatientPrivilegeCard obj = new PatientPrivilegeCard();

        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }


        public List<PatientPrivilegeCard> BindPrefixPatient()
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
                PatientPrivilegeCardList.Add(
                    new PatientPrivilegeCard
                    {
                        PrefixID = Convert.ToInt32(dr["PrefixID"]).ToString(),
                        PrefixName = Convert.ToString(dr["PrefixName"]),
                        Gender = dr["Gender"].ToString(),

                    });
            }
            return PatientPrivilegeCardList;

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
         public DataSet GetAllPrivilegePriceName(int HospitalID,int LocationID,int PrivilegeCardID)
         {
             Connect();
             {
                 DataSet ds = new DataSet();
                 try
                 {
                     SqlParameter[] param = new SqlParameter[3];
                     param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                     param[0].Value = HospitalID;
                     param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                     param[1].Value = LocationID;
                     param[2] = new SqlParameter("@PrivilegeCardID", SqlDbType.Int);
                     param[2].Value = PrivilegeCardID;
                     con.Open();
                     ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllPrivilegePriceName", param);
                     con.Close();
                 }
                 catch (Exception ex)
                 {
                     ExceptionManager.Publish(ex);
                     throw ex;
                 }
                 return ds;
             }
         }

         public List<PatientPrivilegeCard> FillPatient(string PatientRegNO)
         {
             Connect();
             SqlCommand cmd = new SqlCommand("select PatientRegNO as RegNO,PFPatientName,PFirstName,PMiddleName,PLastName,PFGuardianName,GuardianName,Gender,DateOFBirth,Weight,cast(Height as decimal(18,2)) as Height,BloodPressure,FinancialYearID from Patient where Patient.HospitalID = " + HospitalID + "  and Patient.LocationID =" + LocationID + " and Patient.RowStatus= 0 and  Patient.PatientRegNO like '" + PatientRegNO + "%' ORDER BY CONVERT(int, Patient.PatientRegNO) Desc ", con);
             DataSet ds = new DataSet();
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();
            
             foreach (DataRow dr in dt.Rows)
             {
                 if (dr["DateOfBirth"].ToString() != "")
                 {
                    PatientPrivilegeCardList.Add(
                        new PatientPrivilegeCard
                        {
                            PatientRegNo = Convert.ToInt32(dr["RegNO"]),
                            PatientName = (dr["PFirstName"]).ToString(),
                            PFPatientName = (dr["PFPatientName"]).ToString(),
                            PMiddleName = (dr["PMiddleName"]).ToString(),
                            PLastName = (dr["PLastName"]).ToString(),
                            PFGuardianName = (dr["PFGuardianName"]).ToString(),
                            GuardianName = (dr["GuardianName"]).ToString(),
                             //  Age = (dr["Age"]).ToString(),
                             Gender = (dr["Gender"]).ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                            Height = dr["Height"].ToString(),
                            Weight = dr["Weight"].ToString(),
                            BP = (dr["BloodPressure"]).ToString(),
                            FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                            chk = "",

                             // PatientType = (dr["PatientType"]).ToString(),

                         });
                 }
             }


             return PatientPrivilegeCardList;

         }

        public DataSet CheckPatientReg(string RegNo)
        {
            Connect();
            SqlCommand cmd = new SqlCommand("Select PatientRegNo, PatientPrivilegeCardID,PatientName from PatientPrivilegeCard where PatientRegNo = '" + RegNo + "'    and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);

            DataSet ds = new DataSet();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        //public DataSet CheckPatientName(string PatientName)
        //{
        //    Connect();
        //    SqlCommand cmd = new SqlCommand("Select PatientRegNo, PatientPrivilegeCardID,PatientName from PatientPrivilegeCard where PatientName like '" + PatientName + "%'    and HospitalID = " + HospitalID + " and LocationID = " + LocationID + "", con);

        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    con.Close();
        //    return ds;
        //}

        public bool CheckPatientName( string PatientName)
        {
            Connect();
            string Table;
            bool flag;
            SqlCommand cmd = new SqlCommand("CheckPatientReg", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
           
            cmd.Parameters.AddWithValue("@PatientName", PatientName.ToUpper().ToString());
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


        public List<PatientPrivilegeCard> BindPatient(string PatientName)
        {
             Connect();
             SqlCommand cmd = new SqlCommand("select PatientRegNO as RegNO,PrintRegNO,PFPatientName,PFirstName,PMiddleName,PLastName,PFGuardianName,GuardianName,Gender,DateOFBirth,Weight,cast(Height as decimal(18,2)) as Height,BloodPressure,FinancialYearID from Patient where Patient.HospitalID = " + HospitalID + "  and Patient.LocationID =" + LocationID + " and Patient.RowStatus= 0 and  Patient.PatientName like '" + PatientName + "%' ORDER BY CONVERT(int, Patient.PatientRegNO) Desc ", con);
             DataSet ds = new DataSet();
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();
             string Weig = "";
             string Bp = "";
             foreach (DataRow dr in dt.Rows)
             {
                 if (dr["DateOfBirth"].ToString() != "")
                 {
                    PatientPrivilegeCardList.Add(
                        new PatientPrivilegeCard
                        {
                            PatientRegNo = Convert.ToInt32(dr["RegNO"]),
                            PatientName = (dr["PFirstName"]).ToString(),
                            PFPatientName = (dr["PFPatientName"]).ToString(),
                            PMiddleName = (dr["PMiddleName"]).ToString(),
                            PLastName = (dr["PLastName"]).ToString(),
                            PFGuardianName = (dr["PFGuardianName"]).ToString(),
                            GuardianName = (dr["GuardianName"]).ToString(),
                            PrintRegNO = (dr["PrintRegNO"]).ToString(),
                            //  Age = (dr["Age"]).ToString(),
                            Gender = (dr["Gender"]).ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd"),
                            Height = dr["Height"].ToString(),
                            Weight = dr["Weight"].ToString(),
                            BP = (dr["BloodPressure"]).ToString(),
                            FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                            chk = "",

                             // PatientType = (dr["PatientType"]).ToString(),

                         });
                 }
             }


             return PatientPrivilegeCardList;

         }
         public List<PatientPrivilegeCard> AccountName(string prefix)
         {
             Connect();
             SqlCommand cmd = new SqlCommand("select AccountsID, AccountName ,Schedule.ScheduleName,Schedule.ScheduleID,  OPBalance, DrAmount ,CrLimit,PrintName, CreditDays,Address,PhoneNo,MobileNo,EmailID,TinNo,Pan,TinDate   from Accounts left join Schedule on Schedule.ScheduleID = Accounts.ScheduleID where  Accounts.RowStatus = 0 and Accounts.HospitalID = " + HospitalID + "  and Accounts.LocationID = " + LocationID + " and Accounts.AccountName like '" + prefix + "%'   order by Accounts.AccountName asc", con);//Your data query goes here for searching the data
             DataSet ds = new DataSet();
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();
             foreach (DataRow dr in dt.Rows)
             {
                 PatientPrivilegeCardList.Add(
                     new PatientPrivilegeCard
                     {
                         AccountsID = Convert.ToInt32(dr["AccountsID"]),
                         AmountName = Convert.ToString(dr["AccountName"]),

                     });
             }
             return PatientPrivilegeCardList;
         }
         public List<PatientPrivilegeCard> PrivilegeCard(string CardName)
         {
             Connect();
             SqlCommand cmd = new SqlCommand("select PrivilegeCardID,CardName,Path,Remark from PrivilegeCard where   RowStatus=0   and HospitalID=" + HospitalID + "  and LocationID = " + LocationID + "  order by CardName asc", con);//Your data query goes here for searching the data
             DataSet ds = new DataSet();
             con.Open();
             SqlDataAdapter da = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();
             foreach (DataRow dr in dt.Rows)
             {
                 PatientPrivilegeCardList.Add(
                     new PatientPrivilegeCard
                     {
                         PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                         CardName = Convert.ToString(dr["CardName"]),

                     });
             }
             return PatientPrivilegeCardList;
         }

        public string GetPatientPrintNo_ToRegNo(string PrintRegNO)
        {
            DataSet ds = new DataSet();
            Connect();
            SqlCommand cmd = new SqlCommand("select*from Patient where  Patient.PrintRegNO = " + PrintRegNO + "  and  HospitalID=" + HospitalID + " and LocationID=" + LocationID + " and RowStatus=0", con);//Your data query goes here for searching the data
            // Note: @filter parameter must be there.
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            string RegNo = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                RegNo = ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            }
            return RegNo;
        }
        public bool Save(PatientPrivilegeCard obj)
         {
             
            
             Connect();
            con.Open();
            if (obj.CardName != null)
            {
                #region PatientPrivilegeCard
                SqlCommand cmd = new SqlCommand("IUPatientPrivilegeCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
             
                if (obj.PatientRegNo != null)
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", obj.PatientRegNo);
                   
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientRegNo", 0);
                  
                }

                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@PrivilegeCardID", obj.PrivilegeCardID);
                if (obj.PatientPrivilegeCardID == 0)
                {
                    cmd.Parameters.AddWithValue("@PatientPrivilegeCardID", 0);
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PatientPrivilegeCardID", obj.PatientPrivilegeCardID);
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                }

                if (obj.PrivilegePriceDetailID.ToString() != "0")
                {
                    cmd.Parameters.AddWithValue("@PrivilegePriceDetailID",obj.PrivilegePriceDetailID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PrivilegePriceDetailID", 0);
                }

                //  cmd.Parameters.AddWithValue("@PatientRegNo", Convert.ToInt32(obj.PatientRegNo));
                cmd.Parameters.AddWithValue("@PrivilegeDate", Convert.ToDateTime(obj.PrivilegeDate));
                cmd.Parameters.AddWithValue("@PatientName", obj.PFirstName +" "+obj.PMiddleName+" "+obj.PLastName);
                if (obj.CardName == null)
                {
                    cmd.Parameters.AddWithValue("@CardName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CardName", obj.CardName);
                    
                }
                if (obj.PriceName == null)
                {
                    cmd.Parameters.AddWithValue("@PriceName", "");
                }
                else
                {

                    cmd.Parameters.AddWithValue("@PriceName", obj.PriceName);
                }
                if (obj.PriceAmt == null)
                {
                    cmd.Parameters.AddWithValue("@PriceAmt", "");
                }
                else
                {

                    cmd.Parameters.AddWithValue("@PriceAmt", obj.PriceAmt);
                }
                //  cmd.Parameters.AddWithValue("@PriceAmt", obj.PriceAmt);

                if (obj.AmountName == null || obj.AmountName == "")
                {
                    cmd.Parameters.AddWithValue("@AccountName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AccountName", obj.AmountName);
                }

                if (obj.PaidAmt ==null)
                {
                    cmd.Parameters.AddWithValue("@PaidAmt", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaidAmt", obj.PaidAmt);
                }
               
                cmd.Parameters.AddWithValue("@ValidDate", Convert.ToDateTime(obj.ValidDate));
                if (obj.Remark == null)
                {
                    cmd.Parameters.AddWithValue("@Remark", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remark", obj.Remark);
                }
                // cmd.Parameters.AddWithValue("@Remark", obj.Remark);
                cmd.Parameters.AddWithValue("@CreationID", UserID);
                //   cmd.Parameters.AddWithValue("@Mode", "Add");

                int i = cmd.ExecuteNonQuery();


                #endregion


                #region IUVoucharEntry

                SqlCommand cmd1 = new SqlCommand("[IUVoucharEntry]", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd1.Parameters.AddWithValue("@LocationID", LocationID);
                cmd1.Parameters.AddWithValue("@ReferenceCode", 0);
                cmd1.Parameters.AddWithValue("@VoucharEntryID", obj.VoucharEntryID);
                cmd1.Parameters.AddWithValue("@AccountsID", 1);
                cmd1.Parameters.AddWithValue("@VoucharID", 5);
                cmd1.Parameters.AddWithValue("@VoucharName", "CASH RECEIPTS");
                cmd1.Parameters.AddWithValue("@RefVoucharNo", "");
                cmd1.Parameters.AddWithValue("@CurrentDate", Convert.ToDateTime(obj.PrivilegeDate));
                cmd1.Parameters.AddWithValue("@VoucharDate", Convert.ToDateTime(obj.PrivilegeDate));
                cmd1.Parameters.AddWithValue("@Narration", obj.PatientRegNo + "--" + obj.PFirstName + "--" + obj.CardName);
                cmd1.Parameters.AddWithValue("@VoucharAccountName", "HOSPITAL CASH");
                cmd1.Parameters.AddWithValue("@VoucharDrAmount", obj.PaidAmt);
                cmd1.Parameters.AddWithValue("@VoucharCrAmount", "0.00");
                cmd1.Parameters.AddWithValue("@CreationID", UserID);
                cmd1.Parameters.AddWithValue("@Mode", "Add");
                int Vouchar = cmd1.ExecuteNonQuery();


                #endregion
                #region IUVoucharEntryDetails


                SqlCommand cmd2 = new SqlCommand("IUVoucharEntryDetail", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd2.Parameters.AddWithValue("@LocationID", LocationID);
                //  cmd2.Parameters.AddWithValue("@ReferenceCode", 0);
                cmd2.Parameters.AddWithValue("@VoucharEntryID", obj.VoucharEntryID);
                cmd2.Parameters.AddWithValue("@VoucharID", obj.VoucharID);
                cmd2.Parameters.AddWithValue("@InvNo", "");
                cmd2.Parameters.AddWithValue("@VoucharEntryDetailID", obj.VoucharEntryDetailID);
                cmd2.Parameters.AddWithValue("@VoucharName", "CASH RECEIPTS");

                if (obj.AccountsID == null)
                {
                    cmd2.Parameters.AddWithValue("@AccountsID", 0);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@AccountsID", obj.AccountsID);
                }

                if (obj.AmountName == null || obj.AmountName == "")
                {
                    cmd2.Parameters.AddWithValue("@AccountName", "");
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@AccountName", obj.AmountName);
                }
                cmd2.Parameters.AddWithValue("@DrAmount", "0.00");

                if (obj.PaidAmt == null)
                {
                    cmd2.Parameters.AddWithValue("@CrAmount",0);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@CrAmount", obj.PaidAmt);
                }
             
                cmd2.Parameters.AddWithValue("@ChequeNo", "");
                cmd2.Parameters.AddWithValue("@Name", "");
                cmd2.Parameters.AddWithValue("@Date", Convert.ToDateTime(obj.PrivilegeDate));
                cmd2.Parameters.AddWithValue("@Narration", obj.PatientRegNo + "--" + obj.PFirstName + "--" + obj.CardName);
                cmd2.Parameters.AddWithValue("@CreationID", UserID);
                cmd2.Parameters.AddWithValue("@Mode", "Add");
                int VoucharDetails = cmd2.ExecuteNonQuery();
                #endregion

            }
           

                 con.Close();
                 return true;
       
         }
         public List<PatientPrivilegeCard> GetPatientPrivilegeCard(int PatientPrivilegeCardID)
         {
             Connect();
             // List<Department> dept = new List<Department>();

             SqlCommand cmd = new SqlCommand("GetPatientPrivilegeCard", con);
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.Parameters.Add(new SqlParameter("@PatientPrivilegeCardID", PatientPrivilegeCardID));
             SqlDataAdapter sd = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             con.Open();
             sd.Fill(dt);
             con.Close();
             foreach (DataRow dr in dt.Rows)
             {
                 PatientPrivilegeCardList.Add(
                     new PatientPrivilegeCard
                     {
                        // PatientPrivilegeCardID = Convert.ToInt32(dr["PatientPrivilegeCardID"]),
                        // PatientName = Convert.ToString(dr["PatientName"]),
                         //  AdviceDescription = Convert.ToString(dr["AdviceDescription"]),
                         CardName = Convert.ToString(dr["CardName"]),
                         PriceName = Convert.ToString(dr["PriceName"]),
                         PriceAmt = Convert.ToDecimal(dr["PriceAmt"]),
                         AmountName=Convert.ToString(dr["AmountName"]),
                         PaidAmt=Convert.ToDecimal(dr["PaidAmt"]),
                         FromDate = Convert.ToString(dr["FromDate"]),
                         ToDate = Convert.ToString(dr["ToDate"]),
                        



                     });
             }
             return PatientPrivilegeCardList;
         }

         public List<PatientPrivilegeCard> SelectAllData()
         {
             Connect();

             string[] FirstName = null;
             SqlCommand cmd = new SqlCommand("GetAllPatientPrivilegeCard", con);
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
                 //if (dr["PatientName"].ToString()!="")
                 //{
                 //    FirstName = Convert.ToString(dr["PatientName"]).Split(' ');
                 //}
                 PatientPrivilegeCardList.Add(
                     new PatientPrivilegeCard
                     {
                         PatientPrivilegeCardID = Convert.ToInt32(dr["PatientPrivilegeCardID"]),
                         PatientName = dr["PatientName"].ToString(),
                         //PMiddleName = FirstName[1],
                         //PLastName = FirstName[2],
                         PatientRegNo = Convert.ToInt32(dr["PatientRegNo"]),
                         CardName=Convert.ToString(dr["CardName"]),
                         PriceName=Convert.ToString(dr["PriceName"]),
                         PriceAmt=Convert.ToDecimal(dr["PriceAmt"]),
                         FromDate=Convert.ToDateTime(dr["FromDate"]).ToString("dd-MM-yyyy"),
                         ToDate = Convert.ToDateTime(dr["ToDate"]).ToString("dd-MM-yyyy"),
                         PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"]),
                         PrivilegePriceDetailID = dr["PrivilegePriceDetailID"].ToString(),
                         AmountName = Convert.ToString(dr["AccountName"]),
                         PaidAmt = Convert.ToDecimal(dr["PaidAmt"])

                 
                     });
             }
             return PatientPrivilegeCardList;
         }


         public bool DeletePatientPrivilegeCard(int PatientPrivilegeCardID)
         {
             Connect();
             SqlParameter[] apram = new SqlParameter[2];
             apram[0] = new SqlParameter("@PatientPrivilegeCardID", SqlDbType.Int);
             apram[0].Value = PatientPrivilegeCardID;
             con.Open();
             SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "DeletePatientPrivilegeCard", apram);
             con.Close();

             return true;

         }


         public DataSet SelectByEditMode(int PatientPrivilegeCardID)
         {
             SqlConnection con = null;
             DataSet ds = null;
             try
             {
                 con = new SqlConnection(ConfigurationManager.ConnectionStrings["Mycon"].ToString());
                 SqlCommand cmd = new SqlCommand("GetPatientPrivilegeCard", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.AddWithValue("@PatientPrivilegeCardID", PatientPrivilegeCardID); // i will pass zero to MobileID beacause its Primary .       
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
       
        

    }
}