using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace KeystoneProject.Controllers.Patient
{
    public class PreBalanceAmountController : Controller
    {
        int HospitalID;
        int LocationID;
        int CreationID;
        int PatientAccountRowID;

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
            Connect();
        }
        private SqlConnection con;
        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

        BL_PrebalanceAmount objblamt = new BL_PrebalanceAmount();
        PreBalanceAmount prebalamt = new PreBalanceAmount();

        //
        // GET: /PreBalanceAmount/
        [HttpGet]
        public ActionResult PreBalanceAmount(FormCollection collection)
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult PreBalanceAmount(PreBalanceAmount PrebalAmt, FormCollection fc)
        {
            //if (fc["Mode"].ToString() == "Add")
            //{
            //    PrebalAmt.Mode = "ADD";
            //}
            //else
            //{
            //    PrebalAmt.Mode = "Edit";
            //    PrebalAmt.PatientAccountRowID = Convert.ToInt32(fc["PatientaccRowID"]);
            //}
            if (PrebalAmt.Mode == null)
            {
                PrebalAmt.Mode = "Add";
            }
            else
            {
                PrebalAmt.Mode = "Edit";
                PrebalAmt.PatientAccountRowID = Convert.ToInt32(fc["PatientaccRowID"]);
                if(fc["TDSAmtedit"].ToString()=="")
                {
                    PrebalAmt.TDSAmount = 0;
                }
                else
                {
                    PrebalAmt.TDSAmount = Convert.ToInt32(fc["TDSAmtedit"].ToString());
                }
                if (fc["TDSOtherdecdit"].ToString() == "")
                {
                    PrebalAmt.TPAOtherDeduction = 0;
                }
                else
                {
                    PrebalAmt.TPAOtherDeduction = Convert.ToInt32(fc["TDSOtherdecdit"].ToString());
                }
               
                PrebalAmt.BillType = Request.Form["Bill"];
            }


            if (PrebalAmt.Mode != "Add")
            {
               

                if (Request.Form["tblPayEditType"] != null)
                {
                    PrebalAmt.PaymentType = Request.Form["tblPayEditType"].ToString();
                    PrebalAmt.Number = Request.Form["tblEditNumber"];
                    PrebalAmt.Name = Request.Form["tblEditName"].ToString();
                    PrebalAmt.Date = Request.Form["tblpayDateEdit"].ToString();
                    PrebalAmt.Remarks = Request.Form["tblEditRemarks"].ToString();
                    PrebalAmt.PaidAmount = Request.Form["tblPayEditTotal"].ToString();
                    PrebalAmt.Mode = "Edit";
                    //if(Request.Form["tblPayEditTotal"].ToString()==null || Request.Form["tblPayEditTotal"].ToString()=="")
                    //{

                    //}
                  //  PrebalAmt.PaidAmount = fc["PaidAmtedit"];
                    PrebalAmt.BillDate = fc["editModalDate"];
                }
                else
                {
                    PrebalAmt.BillDate = fc["editModalDate"];
                    switch (PrebalAmt.PaymentType)
                    {
                        case "Cheque":
                            PrebalAmt.Number = Request.Form["No"];
                            PrebalAmt.Name = Request.Form["Namepay"];
                            PrebalAmt.Date = Request.Form["payDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["payRemarks"];
                            break;

                        case "Debit Card":
                            PrebalAmt.Number = Request.Form["No"];
                            PrebalAmt.Name = Request.Form["Namepay"];
                            PrebalAmt.Date = Request.Form["payDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["payRemarks"];
                            break;

                        case "Credit Card":
                            PrebalAmt.Number = Request.Form["No"];
                            PrebalAmt.Name = Request.Form["Namepay"];
                            PrebalAmt.Date = Request.Form["payDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["payRemarks"];
                            break;

                        case "EFT":
                            PrebalAmt.Number = Request.Form["No"];
                            PrebalAmt.Name = Request.Form["Namepay"];
                            PrebalAmt.Date = Request.Form["payDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["payRemarks"];
                            break;

                        case "E-Money":
                            PrebalAmt.Number = Request.Form["No"];
                            PrebalAmt.Name = Request.Form["Namepay"];
                            PrebalAmt.Date = Request.Form["payDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["payRemarks"];
                            break;

                        default:
                            PrebalAmt.Number = "Cash";
                            PrebalAmt.Name = "Cash";
                            PrebalAmt.Date = System.DateTime.Now.ToString();
                            PrebalAmt.Remarks = "Cash";
                            break;
                    }
                    PrebalAmt.Mode = "Edit";
                    PrebalAmt.PaidAmount = fc["PaidAmtedit"];
                }
            }
            if (PrebalAmt.Mode== "Add")
            {

                if (Request.Form["tblPaymentType"] != null)
                {
                    PrebalAmt.PaymentType = Request.Form["tblPaymentType"].ToString();
                    PrebalAmt.Number = Request.Form["tblNumber"];
                    PrebalAmt.Name = Request.Form["tblName"].ToString();
                    PrebalAmt.Date = Request.Form["tblpaymentDate"].ToString();
                    PrebalAmt.Remarks = Request.Form["tblRemarks"].ToString();
                    PrebalAmt.PaidAmount = Request.Form["tblPaymentTotal"].ToString();
                    PrebalAmt.Mode = "Add";
                }
                else
                {
                    #region PaymentType
                    switch (PrebalAmt.PaymentType)
                    {
                        case "Cheque":
                            PrebalAmt.Number = Request.Form["Number"];
                            PrebalAmt.Name = Request.Form["Name"];
                            PrebalAmt.Date = Request.Form["paymentDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["Remarks"];
                            break;

                        case "Debit Card":
                            PrebalAmt.Number = Request.Form["Number"];
                            PrebalAmt.Name = Request.Form["Name"];
                            PrebalAmt.Date = Request.Form["paymentDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["Remarks"];
                            break;

                        case "Credit Card":
                            PrebalAmt.Number = Request.Form["Number"];
                            PrebalAmt.Name = Request.Form["Name"];
                            PrebalAmt.Date = Request.Form["paymentDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["Remarks"];
                            break;

                        case "EFT":
                            PrebalAmt.Number = Request.Form["Number"];
                            PrebalAmt.Name = Request.Form["Name"];
                            PrebalAmt.Date = Request.Form["Date"].ToString();
                            PrebalAmt.Remarks = Request.Form["Remarks"];
                            break;

                        case "E-Money":
                            PrebalAmt.Number = Request.Form["Number"];
                            PrebalAmt.Name = Request.Form["Name"];
                            PrebalAmt.Date = Request.Form["paymentDate"].ToString();
                            PrebalAmt.Remarks = Request.Form["Remarks"];
                            break;

                        default:
                            PrebalAmt.Number = "Cash";
                            PrebalAmt.Name = "Cash";
                            PrebalAmt.Date = System.DateTime.Now.ToString();
                            PrebalAmt.Remarks = "Cash";
                            break;
                    }
                }
            }
            #endregion

            
            PreBalanceAmount PreBal = new Models.Patient.PreBalanceAmount();

            if (Request.Form["SecurityDepositeTPA"] != null)

                PrebalAmt.TPAStatus = Request.Form["SecurityDepositeTPA"].ToString();
            if (Request.Form["chkTPA"] == "true")
            {
                PrebalAmt.chkTPA = "true";
            }
            else
            {
                PrebalAmt.chkTPA = "false";
            }
            if (fc["chkAllpaidAmt"] != null)
            {
                string chkAllpaidAmt = fc["chkAllpaidAmt"].ToString();

                AllPaidAmtOPDIPDNo(fc, PrebalAmt);

            }
            else
            {
                 PreBal = objblamt.SavePrebalAmt(PrebalAmt);
            }
            if(PreBal.PatientAccountRowID>0)
            {
                PatientAccountRowID = PreBal.PatientAccountRowID;
                TempData["Msg"] = "Saved Successfully";
                Session["PrintPaymentTypeCount"] = PreBal.PatientAccountRowID;
                return RedirectToAction("RptPreBalanceAmount");
               
               // return RedirectToAction("PreBalanceAmount", "PreBalanceAmount");
            }
          //  PreBal.PatientAccountRowID = PrebalAmt.PatientAccountRowID;
            Connect();
            
            return RedirectToAction("PreBalanceAmount", "PreBalanceAmount");
        }
           


        public bool AllPaidAmtOPDIPDNo(FormCollection fc, PreBalanceAmount PreBal)
        {
            HospitlLocationID();
            DataSet ds = new System.Data.DataSet();
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@PatientRegNo", SqlDbType.Int);
            param[0].Value = fc["PatientRegNO"];
            param[1] = new SqlParameter("@LocationID", SqlDbType.Int);
            param[1].Value = LocationID;
            param[2] = new SqlParameter("@HospitalID", SqlDbType.Int);
            param[2].Value = HospitalID;

            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetRefoundIPDOPDID", param);
            string paidAmt = fc["PaidAmount"].ToString();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                SqlCommand cmd = new SqlCommand("GetRefoundAmountOPDIPD", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OPDIPDNo", dr["OPDIPDNO"]);
                cmd.Parameters.AddWithValue("@PatientType", dr["PatientType"]);
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                con.Open();
                da.Fill(ds1);
                con.Close();
                if(Convert.ToDecimal( ds1.Tables[0].Rows[0][0])<0)
                {
                    BL_Deposit obj1 = new BL_Deposit();
                    Deposit objDeposit = new Models.Patient.Deposit();
                    objDeposit.PatientRegNo = fc["PatientRegNO"];
                    objDeposit.BillDate = Convert.ToDateTime( DateTime.Now);
                    objDeposit.PaymentType = PreBal.PaymentType;
                        objDeposit.PatinetType= dr["PatientType"].ToString();
                    objDeposit.OPDIPDNO = dr["OPDIPDNO"].ToString();
                    objDeposit.PaidAmount = Convert.ToDecimal(ds1.Tables[0].Rows[0][0].ToString().Replace("-",""));
                    int RowID = obj1.IURefoundAmount(objDeposit);

                    //Deposit
                }
                else
                {
                   
                    //pribalence
                    PreBal.PatientRegNo = fc["PatientRegNO"];
                    PreBal.BillDate = DateTime.Now.ToString();
                    PreBal.PaymentType = "Cash";
                    PreBal.OPDIPDNO = dr["OPDIPDNO"].ToString() + "-" + dr["PatientType"].ToString();
                    PreBal.PaidAmount = ds1.Tables[0].Rows[0][0].ToString();
                    if (Convert.ToDecimal(PreBal.PaidAmount) > 0)
                    {
                        decimal TotalPaid = Convert.ToDecimal(paidAmt) - Convert.ToDecimal(PreBal.PaidAmount);
                        paidAmt = TotalPaid.ToString();
                        PreBal = objblamt.SavePrebalAmt(PreBal);
                    }
                }
            }
            return true;
        }
        public ActionResult RptPreBalanceAmount()
        {
           // Session["PrintPaymentTypeCount"] = PatientAccountRowID;
            return View();
        }

        public ActionResult RptbtnPreBalanceAmount(int PatientAccountRowID)
        {
            Session["PrintPaymentTypeCount"] = PatientAccountRowID;
          //  objblamt.RptPatientIPDAdvanceAmount(PatientRegNO, PatientIPDNo);
            return RedirectToAction("RptPreBalanceAmount");
        }
        public ActionResult RptbtnReufndBalanceAmount(int PatientAccountRowID)
        {
            Session["OtherAccountRowID"] = PatientAccountRowID;
            //  objblamt.RptPatientIPDAdvanceAmount(PatientRegNO, PatientIPDNo);
            return RedirectToAction("RptRefundAmount");
        }
        public ActionResult RptRefundAmount()
        {

            return View();
        }
        public ActionResult RptPatientIPDAdvanceAmount(int RegNo,int OPDIPDNO,int PatientAccountRowID,string BillType)
        {
          Session["OtherAccountRowID"] = PatientAccountRowID;
            Session["BillType"] = BillType;
          
            objblamt.RptPatientIPDAdvanceAmount(RegNo, OPDIPDNO);
            return RedirectToAction("RptRefundAmount");
        }
        public JsonResult GetPatientName(string prefix)
        {
            DataSet ds = objblamt.GetPatientName(prefix);
            List<PreBalanceAmount> SearchList = new List<PreBalanceAmount>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new PreBalanceAmount
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNO"].ToString(),
                    PrintRegNo=dr["PrintRegNO"].ToString()
                });
            }

            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult MasterSetting(string Password)
        {
           
            BL_PrebalanceAmount BL_Reg = new BL_PrebalanceAmount();
            string confirm = BL_Reg.GetPassword(Password);
           
          
                return new JsonResult { Data = confirm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }

        public JsonResult GetPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_PatientOPD BL_Reg = new BL_PatientOPD();
            string RegNo = BL_Reg.GetPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetIPDPatient(string prefix, bool outside)
        {
            BL_Patient_IPDBill _IPDBill = new BL_Patient_IPDBill();
            DataSet ds = objblamt.GetIPDPatient(prefix);
            List<PatientOPDBill> searchList = new List<PatientOPDBill>();
            // if (outside == false)
            //  {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new PatientOPDBill
                {
                    patientregNo = (dr["PatientRegNO"].ToString()),
                    patientname = dr["PatientName"].ToString(),
                    address = (dr["Address"].ToString()),
                    contactno = dr["MobileNo"].ToString(),
                    PrintRegNO = dr["PrintRegNO"].ToString()
                });
            }
            //}
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetPatientRegNo(int PatientRegNo)
        {
            DataSet ds = objblamt.GetPatientRegNo(PatientRegNo);
            List<string> SearchList = new List<string>();
            if(ds.Tables[0].Rows.Count>0)
            {

                prebalamt.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                SearchList.Add(prebalamt.PatientName);

            }
            else
            {
                SearchList.Add("Record Not Found");
            }
           // TempData["Msg"] = "Record Not Found";
            return Json(SearchList);
        }

        public ActionResult AjaxMethodDgv(string PatientRegNO)
            {
            Connect();
              
            #region ForAuthorization
            int ForAuthorization = 0;
            // Aurthorise
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting obj4 = new Buisness_Logic.Master.BL_MasterSetting();
            DataSet dsMasterSetting = new DataSet();
            dsMasterSetting = obj4.GetMasterSetting();
            KeystoneProject.Buisness_Logic.Hospital.BL_Users user = new Buisness_Logic.Hospital.BL_Users();
            DataSet dsAuthorizationRights = new DataSet();
            // List<Users> Search = new List<Users>();
            Connect();

            HospitlLocationID();
            dsAuthorizationRights = user.GetUsers(CreationID);
            string chkAurthoriseUserWise = "";
            if (dsAuthorizationRights.Tables[0].Rows.Count > 0)
            {
                 chkAurthoriseUserWise = dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();

            }
                if (chkAurthoriseUserWise == "True")

                {
                     chkAurthoriseUserWise = dsAuthorizationRights.Tables[0].Rows[0]["AuthorizationRights"].ToString();

                    if (chkAurthoriseUserWise == "True")
                    {
                        dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"] = false;
                        ForAuthorization = 0;
                    }
                }
            

            if (Convert.ToBoolean(dsMasterSetting.Tables[0].Rows[0]["ForAuthorization"]) == true)
            {
                ForAuthorization = 1;
            }


                #endregion







                List<PreBalanceAmount> search = new List<PreBalanceAmount>();

            DataSet ds1 = new DataSet();

            DataSet ds2 = new DataSet();

            DataSet ds = objblamt.GetPatientName(PatientRegNO);
            if (ForAuthorization == 0)
            {
                ds1 = objblamt.GetRefoundAmount(Convert.ToInt32(PatientRegNO));
            }
            if (ForAuthorization == 1)
            {
                ds1 = objblamt.GetRefoundAmountPatientAccountModyFyModyFy(Convert.ToInt32(PatientRegNO));
            }
            prebalamt.PreBalance = ds1.Tables[1].Rows[0]["PreBalance"].ToString();
            //Session["OPDIPDNO1"] = ds1.Tables[2].Rows[0]["OPD/IPDID"].ToString();

            if (ds1.Tables[2].Rows.Count > 0)
            {

                foreach (DataRow dr in ds1.Tables[2].Rows)
                {
                    if (dr["TDSAmount"].ToString() == "" || dr["TPAOtherDeduction"].ToString() == "")

                    {
                        dr["TDSAmount"] = 0;
                        dr["TPAOtherDeduction"] = 0;
                    }
                    search.Add(new PreBalanceAmount {
                        OPDIPDNO = dr["OPDIPDNO"].ToString(),
                        BillType = dr["BillType"].ToString(),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"].ToString()),
                        P_BillNo = dr["P_BillNo"].ToString(),
                        TDSAmount = Convert.ToDecimal(dr["TDSAmount"].ToString()),
                        TPAOtherDeduction = Convert.ToDecimal(dr["TPAOtherDeduction"].ToString()),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"].ToString()),
                        BillDate = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                        BillNo = dr["BillNo"].ToString(),
                        P_RegNo = dr["PatientRegNO"].ToString(),
                       PrintRegNo=dr["P_RegNo"].ToString(),
                        PaymentType = dr["PaymentType"].ToString(),
                        OPDIPDID = dr["OPD/IPDID"].ToString(),
                        PreBalance = ds1.Tables[1].Rows[0]["PreBalance"].ToString(),
                        PatientAccountRowID = Convert.ToInt32(dr["PatientAccountRowID"].ToString()),
                        PatinetType = dr["PatientType"].ToString(),
                    });
                }
            }
            else
            {
                search.Add(new PreBalanceAmount
                {

                    PreBalance = "0",
                    
                    
                    
                    
                    
                    
                    
            });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        public ActionResult AjaxMethod(string PatientRegNO)
            {
            string type="";
            List<PreBalanceAmount> searchList = new List<PreBalanceAmount>();
            int a;
            DataSet ds1 = objblamt.GetPatientName1(PatientRegNO);
            //List<string> searchList = new List<string>();
            HospitlLocationID();
          // DataSet ds2 =objblamt.GetIPDOPDIDForPrintIPDNo(PatientRegNO,a) ;
            DataSet dsPatientPrintIPDOpdID = new DataSet();

            DataSet ds = objblamt.GetPatientName1(PatientRegNO);
            ds1 = objblamt.GetRefoundAmount(Convert.ToInt32(PatientRegNO));
            if(ds.Tables[0].Rows.Count>0)
            {
                type = ds.Tables[0].Rows[0]["PatientType"].ToString();
            }
          
            string opdipd = "0";
            if (ds1.Tables[2].Rows.Count > 0)
            {
                opdipd = ds1.Tables[2].Rows[0]["OPDIPDNO"].ToString();
                if (opdipd == "")
                {
                    opdipd = "0";
                }
            }
            else if(ds1.Tables[4].Rows.Count > 0)
            {
                if (ds1.Tables[4].Rows.Count > 0)
                {
                    opdipd = ds1.Tables[4].Rows[0]["OPDIPDNO"].ToString();
                    if (opdipd == "")
                    {
                        opdipd = "0";
                    }
                }
            }
            else
            {
                if (ds1.Tables[5].Rows.Count > 0)
                {
                    opdipd = ds1.Tables[5].Rows[0]["OPDIPDNO"].ToString();
                    if (opdipd == "")
                    {
                        opdipd = "0";
                    }
                }
            }
            
            //int opdipd= Convert.ToInt32(ds1.Tables[2].Rows[0]["OPDIPDNO"].ToString());
            DataSet ds2 = new DataSet();
           ds2 = objblamt.GetIPDOPDIDForPrintIPDNo(Convert.ToInt32(PatientRegNO), Convert.ToInt32(opdipd), type);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                dsPatientPrintIPDOpdID = objblamt.GetRefoundIPDOPDID(Convert.ToInt32(PatientRegNO));
                prebalamt.SecurityDeposityAmt = Convert.ToDecimal( ds1.Tables[1].Rows[0]["SecurityDeposityAmt"]);
                if (ds1.Tables[3].Rows.Count > 0)
                {
                    prebalamt.SecurityDeposityID = ds1.Tables[3].Rows[0]["SecurityDeposityID"].ToString();
                }
                else
                {
                    prebalamt.SecurityDeposityID = "0";
                }
                prebalamt.PreBalance = ds1.Tables[1].Rows[0]["PreBalance"].ToString();
                if (dsPatientPrintIPDOpdID.Tables[1].Rows.Count>0)
                {
                    prebalamt.OPDIPDNO = dsPatientPrintIPDOpdID.Tables[1].Rows[0]["OPDIPDNO"].ToString();
                }
                if (ds2.Tables.Count>0)
                {


                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        if (type == "OPD")
                        {
                            if (ds2.Tables[0].Rows[0]["TPA_ID"].ToString()!="0")
                            {
                                prebalamt.chkTPA = "True";
                                prebalamt.PatinetType = type;
                           }
                        else
                          {
                                prebalamt.chkTPA = "False";
                                prebalamt.PatinetType = type;
                          }
                        }
                        if (type == "IPD")
                        {
                            if (ds2.Tables[0].Rows[0]["TPAStatus"].ToString() != "False")
                            {
                                prebalamt.chkTPA = "True";
                                prebalamt.PatinetType = type;
                            }
                            else
                            {
                                prebalamt.chkTPA = "False";
                                prebalamt.PatinetType = type;
                            }
                        }

                    }
                }
 
            }
            searchList.Add(prebalamt);
            //searchList.Add(prebalamt);
            return Json(searchList);
        }


        public ActionResult Calculate(string PaidAmount, string PreBalance)
        {
            string bal = "0";
            if(PreBalance=="")
            {
                bal = "0";
            }
            List<string> search1 = new List<string>();
            if (PaidAmount != "")
            {
                Decimal TotalAmount = Convert.ToDecimal(PreBalance) - Convert.ToDecimal(PaidAmount);
                prebalamt.BalanceAmount = TotalAmount.ToString();
                //.Replace("-","")
                search1.Add(prebalamt.BalanceAmount);
            }
            return Json(search1);
        }

        [HttpPost]
        public JsonResult DeletePreBalance(int PatientAccountRowID,string DeleteReason, string BillType,int Reg,string SecurityDeposityID)
        {
            //  int PatientAccountRowID = Convert.ToInt32(Request.Form["PatientAccountRowID"]);

            string _Del = null;
            try
            {
                string DependaincyName = objblamt.DeletePreBalanceAmount(Convert.ToInt32(PatientAccountRowID), DeleteReason, BillType,Reg,SecurityDeposityID);

                _Del = "Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetRefoundIPDOPDID1(string PatientRegNo)
        {

            BL_PrebalanceAmount db = new BL_PrebalanceAmount();
             return new JsonResult { Data = db.GetRefoundIPDOPDID(PatientRegNo), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        [HttpPost]
        public JsonResult GetRefoundAmountOPDIPD(string OPDIPDNo)
        {
           // OPDIPDNo = Request.Form["OPDIPDNO"].ToString();
            BL_PrebalanceAmount db = new BL_PrebalanceAmount();

            return new JsonResult { Data = db.GetRefoundAmountOPDIPD(OPDIPDNo.Split('-')[0], OPDIPDNo.Split('-')[1]), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}