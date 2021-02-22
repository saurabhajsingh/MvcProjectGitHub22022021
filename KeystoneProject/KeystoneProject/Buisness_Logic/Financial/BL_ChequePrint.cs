using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Financial;
using System.IO;
namespace KeystoneProject.Buisness_Logic.Financial
{
    public class BL_ChequePrint
    {
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["MyCon"].ToString();
            con = new SqlConnection(constring);
        }

        public bool SaveChequePrint(ChequePrint obj)
        {
            Connect();
            SqlCommand CmdPatienBillsDetails = new SqlCommand("IUChequePrint", con);
            CmdPatienBillsDetails.CommandType = CommandType.StoredProcedure;
            CmdPatienBillsDetails.Parameters.AddWithValue("@HospitalID", HospitalID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@LocationID", LocationID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequePrintID", obj.ChequePrintID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@BankID", obj.BankID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@BankName", obj.BankName);
            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequeNo", obj.ChequeNo);

            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequeBookID", obj.ChequeBookID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@VoucherEntryID", obj.VoucherEntryID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@PayeeName", obj.PayeeName);
            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequeStatus", true);
            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequeAmount", obj.ChequeAmount);
            CmdPatienBillsDetails.Parameters.AddWithValue("@ChequeDate", Convert.ToDateTime( obj.ChequeDate));
            CmdPatienBillsDetails.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            CmdPatienBillsDetails.Parameters.AddWithValue("@TDS", obj.TDS);
            CmdPatienBillsDetails.Parameters.AddWithValue("@Amount", obj.TDSAmt);

            CmdPatienBillsDetails.Parameters.AddWithValue("@Narration", obj.Narretion);
            CmdPatienBillsDetails.Parameters.AddWithValue("@CreationID", UserID);
            CmdPatienBillsDetails.Parameters.AddWithValue("@Mode", "Edit");
            
            con.Open();
            CmdPatienBillsDetails.ExecuteNonQuery();
            con.Close();
            return true;
        }

        #region Get All Bank
        public List<KeystoneProject.Models.Financial.ChequePrint> GetAllBankName()
        {
            List<KeystoneProject.Models.Financial.ChequePrint> add = new List<Models.Financial.ChequePrint>();

            DataSet ds = new DataSet();
            Connect();
            try
            {

                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                     ds= SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllBank", param);
                
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    add.Add(new  Models.Financial.ChequePrint {
                    BankID=dr["BankID"].ToString(),
                    BankName=dr["BankName"].ToString()
                    });

                }



                    
            SqlConnection con1;
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con1 = new SqlConnection("server=GOVIND;uid=sa;pwd=pass@123;database=Live1516");
            //int HospitalID = Convert.ToInt32(Session["HospitalIDReport"].ToString());
            //int LocationID = Convert.ToInt32(Session["LocationIDReport"].ToString());
            //int PatientRegNo = Convert.ToInt32(Session["PatientRegNoReport"].ToString());
                DataSet ds1 = new DataSet();
                SqlParameter[] param1 = new SqlParameter[3];
                param1[0] = new SqlParameter("@ChequeBookID", SqlDbType.Int);
                param1[0].Value = 4;

                //param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                //param[1].Value = 1;

                //param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                //param[2].Value = 1;

                ds1 = SqlHelper.ExecuteDataset(con1, CommandType.StoredProcedure, "GetChequeLayoutForCheckPrint", param1);
                MemoryStream stream = new MemoryStream((byte[])ds1.Tables[0].Rows[0]["BackImage"]);
                System.Drawing.Image img = new System.Drawing.Bitmap(stream);

                string a = img.ToString();
     
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
            return add;
        }
        # endregion

        #region Get All ChequePrint
        public DataSet GetAllChequePrint(int HospitalID, int LocationID)
        {
            Connect();
            try
            {
               
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllChequePrint", param);
                
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
        # endregion


        public List<KeystoneProject.Models.Financial.ChequePrint> GetAllPayeeName()
        {
            List<KeystoneProject.Models.Financial.ChequePrint> add = new List<Models.Financial.ChequePrint>();

            DataSet ds = new DataSet();
            Connect();
            try
            {

                SqlDataAdapter ad = new SqlDataAdapter("select PayeeNameID,PayeeName from chequePayeeName where chequePayeeName.RowStatus=0 and HospitalID =" + HospitalID + " and LocationID =" + LocationID + " order by  PayeeName asc",con);
                ad.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    add.Add(new Models.Financial.ChequePrint
                    {
                        PayeeNameID = dr["PayeeNameID"].ToString(),
                        PayeeName = dr["PayeeName"].ToString()
                    });

                }
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
            return add;
        }

        #region Get ChequeBook
        public List<KeystoneProject.Models.Financial.ChequePrint> GetChequeBookByBankID(int BankID)
        {
            List<KeystoneProject.Models.Financial.ChequePrint> add = new List<Models.Financial.ChequePrint>();

            DataSet ds = new DataSet();
            try
            {
                     Connect();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@BankID", SqlDbType.Int);
                    param[0].Value = BankID;

                    param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[1].Value = HospitalID;

                    param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[2].Value = LocationID;

                    ds= SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetChequeBookByBankID", param);
                
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        add.Add(new Models.Financial.ChequePrint
                        {
                            ChequeBookID = dr["ChequeBookID"].ToString(),
                            BookName = dr["BookName"].ToString()
                        });

                    }
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
            return add;
        }
        # endregion

        # region Delete Cheque PayeeName

        public bool DeleteChequePayeeName(int PayeeNameID)
        {
            try
            {
                Connect();
                SqlParameter[] aParams = new SqlParameter[1];
                aParams[0] = new SqlParameter("@PayeeNameID", SqlDbType.Int);
                aParams[0].Value = PayeeNameID;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "DeleteChequePayeeName", aParams);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                return false;
            }
            return true;
        }



        # endregion
        public List<KeystoneProject.Models.Financial.ChequePrint> GetChequeLayoutForCheckPrint(int BooKNameID)
        {
            List<KeystoneProject.Models.Financial.ChequePrint> add = new List<Models.Financial.ChequePrint>();

            DataSet ds = new DataSet();
            try
            {
                Connect();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ChequeBookID", SqlDbType.Int);
                param[0].Value = BooKNameID;

                param[1] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[1].Value = HospitalID;

                param[2] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[2].Value = LocationID;

                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetChequeLayoutForCheckPrint", param);
        
                HttpContext.Current.Session["GetChequeLayoutForCheckPrint"] = ds;
            int StartNo = 0;
            int EndNo = 0;
            StartNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeSNo"].ToString());
            EndNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeENo"].ToString());
            int Total = EndNo - StartNo;
           DataSet dsBindCheque=new DataSet();
            dsBindCheque.Tables.Add(new DataTable());
            dsBindCheque.Tables[0].Columns.Add("ID");
            dsBindCheque.Tables[0].Columns.Add("ChequeNo");

            int SchequeNo = 0;

            for (int i = 0; Total >= i; i++)
            {



                DataRow dr = dsBindCheque.Tables[0].NewRow();


                DataView dv1 = new DataView(ds.Tables[2], " ChequeNo = '" + StartNo + "' ", "", DataViewRowState.CurrentRows);
                if (dv1.Count > 0)
                {
                    if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Active")
                    {
                        dr["ChequeNo"] = StartNo.ToString() + " --- P";
                        dr["ID"] = StartNo;
                    }
                    else if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Cancel")
                    {
                        dr["ChequeNo"] = StartNo.ToString() + " --- C";
                        dr["ID"] = StartNo;
                    }
                    else
                    {
                        dr["ChequeNo"] = StartNo;
                        dr["ID"] = StartNo;
                    }
                }
                else
                {
                    if (SchequeNo == 0)
                    {
                        SchequeNo = StartNo;
                    }
                    dr["ChequeNo"] = StartNo;
                    dr["ID"] = StartNo;

                }
                dsBindCheque.Tables[0].Rows.Add(dr);

                StartNo++;
            }

            foreach (DataRow dr in dsBindCheque.Tables[0].Rows)
                {
                    add.Add(new Models.Financial.ChequePrint
                    {
                        ID = dr["ID"].ToString(),
                        ChequeNo = dr["ChequeNo"].ToString()
                    });

                }
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
            return add;
        }
        public List<KeystoneProject.Models.Financial.ChequePrint> GetAllChequePrint()
        {
            List<KeystoneProject.Models.Financial.ChequePrint> add = new List<Models.Financial.ChequePrint>();

            DataSet ds = new DataSet();
            try
            {
                Connect();
                
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                    param[0].Value = HospitalID;
                    param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                    param[1].Value = LocationID;
                    ds= SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAllChequePrint", param);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        add.Add(new Models.Financial.ChequePrint
                        {
                            BankID = dr["BankID"].ToString(),
                            BankName = dr["BankName"].ToString(),
                            PayeeName = dr["PayeeName"].ToString(),
                            ChequeAmount = dr["ChequeAmount"].ToString(),
                            ChequeNo = dr["ChequeNo"].ToString(),
                            ChequeBookID = dr["ChequeBookID"].ToString(),
                            ChequeDate = Convert.ToDateTime(dr["ChequeDate"]).ToString("yyyy-dd-MM"),
                            TDS = dr["TDS"].ToString(),
                            TDSAmt = dr["Amount"].ToString(),
                            Narretion = dr["Narration"].ToString(),
                            VoucherEntryID = Convert.ToInt32( dr["VoucherEntryID"]),
                            ChequePrintID = Convert.ToInt32(dr["ChequePrintID"]),
                        });

                    }
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
            return add;
        }


        public List<string> GetWardDetailsPivot()
        {
            List<string> add = new List<string>();

            DataSet ds = new DataSet();
            try
            {
                Connect();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@HospitalID", SqlDbType.Int);
                param[0].Value = HospitalID;
                param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
                param[1].Value = LocationID;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetWardDetailsPivot", param);
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    add.Add(dc.ColumnName);
                    //add.Add(string,{dc.ColumnName
                    //});
                   

                }
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
            return add;
        }
       
    }
}