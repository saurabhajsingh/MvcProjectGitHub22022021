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
using KeystoneProject.Buisness_Logic.Other;
using KeystoneProject.Models.Patient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace KeystoneProject.Controllers.Other
{
    public class OtherPreBalanceAmountController : Controller
    {
        // GET: OtherPreBalanceAmount
        int HospitalID;
        int LocationID;
        int CreationID;

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
        [HttpGet]
        public ActionResult OtherPreBalanceAmount()
        {
            return View();
        }
        BL_OtherPreBalanceAmount objblamt = new BL_OtherPreBalanceAmount();
        [HttpPost]
        public ActionResult OtherPreBalanceAmount(PreBalanceAmount PrebalAmt, FormCollection fc)
        {
            PrebalAmt.Mode = "ADD";
            if (Request.Form["tblPaymentType"] != null)
            {
                PrebalAmt.PaymentType = Request.Form["tblPaymentType"].ToString();
                PrebalAmt.Number = Request.Form["tblNumber"];
                PrebalAmt.Name = Request.Form["tblName"].ToString();
                PrebalAmt.Date = Request.Form["tblpaymentDate"].ToString();
                PrebalAmt.Remarks = Request.Form["tblRemarks"].ToString();
                PrebalAmt.PaidAmount = Request.Form["tblPaymentTotal"].ToString();
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
            #endregion

            //string number = fc["Number1"].ToString();
            //string name = fc["Name1"].ToString();
            //string date = fc["Date1"].ToString();
            //string remark = fc["Remarks"].ToString();
            //string mode = fc["Mode"].ToString();

            //String[] number_1 = number.Split(',');
            //String[] name_1 = name.Split(',');
            //String[] date_1 = date.Split(',');
            //String[] remark_1 = remark.Split(',');

            //PreBalanceAmount[] obj_array = new PreBalanceAmount[number_1.Length];

            //for (int i = 0; i < number_1.Length; i++)
            //{
            //    obj_array[i] = new PreBalanceAmount();
            //    obj_array[i].Number = number_1[i].ToString();
            //    obj_array[i].Name = name_1[i].ToString();
            //    obj_array[i].Date = Convert.ToDateTime(date_1[i].ToString());
            //    obj_array[i].Remarks = remark_1[i].ToString();
            //}

            // PrebalAmt.Date = Convert.ToDateTime(Request.Form["paymentDate"]);
            PreBalanceAmount PreBal = new Models.Patient.PreBalanceAmount();

            if (Request.Form["SecurityDepositeTPA"] != null)

                PrebalAmt.TPAStatus = Request.Form["SecurityDepositeTPA"].ToString();
            //if (fc["chkAllpaidAmt"] != null)
            //{
            //    string chkAllpaidAmt = fc["chkAllpaidAmt"].ToString();

            //    AllPaidAmtOPDIPDNo(fc, PrebalAmt);

            //}
            //else
            //{
              PreBal = objblamt.SavePrebalAmt(PrebalAmt);
            //}
            if (PreBal.PatientAccountRowID > 0)
            {

                Session["PrintPaymentTypeCount"] = PreBal.PrintPaymentTypeCount;
                return RedirectToAction("RptPreBalanceAmount");

            }
            //  PreBal.PatientAccountRowID = PrebalAmt.PatientAccountRowID;
            Connect();
            return RedirectToAction("OtherPreBalanceAmount", "OtherPreBalanceAmount");
        }
        PreBalanceAmount prebalamt = new PreBalanceAmount();
        public ActionResult AjaxMethod(string PatientRegNO)
        {
            Buisness_Logic.Other.BL_OtherPreBalanceAmount BL_obj = new Buisness_Logic.Other.BL_OtherPreBalanceAmount();
            List<PreBalanceAmount> searchList = new List<PreBalanceAmount>();
            List<PreBalanceAmount> searchListOPDIPD = new List<PreBalanceAmount>();
            List<PreBalanceAmount> searchListTable = new List<PreBalanceAmount>();
            int a;
          //  DataSet ds1 = objblamt.GetPatientName(PatientRegNO);
            //List<string> searchList = new List<string>();
            HospitlLocationID();
            // DataSet ds2 =objblamt.GetIPDOPDIDForPrintIPDNo(PatientRegNO,a) ;
            DataSet dsPatientPrintIPDOpdID = new DataSet();

          //  DataSet ds = objblamt.GetPatientName(PatientRegNO);
           // ds1 = objblamt.GetRefoundAmount(Convert.ToInt32(PatientRegNO));
            DataSet dsotherpre = BL_obj.GetPreBalanceAmountOther1(Convert.ToInt32(PatientRegNO), "%");
            //string opdipd = "0";
            //if (ds1.Tables[2].Rows.Count > 0)
            //{
            //    opdipd = ds1.Tables[2].Rows[0]["OPDIPDNO"].ToString();
            //    if (opdipd == "")
            //    {
            //        opdipd = "0";
            //    }
            //}

            //int opdipd= Convert.ToInt32(ds1.Tables[2].Rows[0]["OPDIPDNO"].ToString());
            DataSet ds2 = new DataSet();
           // ds2 = objblamt.GetIPDOPDIDForPrintIPDNo(Convert.ToInt32(PatientRegNO), Convert.ToInt32(opdipd), "IPD");
            if (dsotherpre.Tables[0].Rows.Count > 0)
            {
              //  dsPatientPrintIPDOpdID = objblamt.GetRefoundIPDOPDID(Convert.ToInt32(PatientRegNO));
                //prebalamt.SecurityDeposityAmt = Convert.ToDecimal(ds1.Tables[1].Rows[0]["SecurityDeposityAmt"]);
                //if (ds1.Tables[3].Rows.Count > 0)
                //{
                //    prebalamt.SecurityDeposityID = ds1.Tables[3].Rows[0]["SecurityDeposityID"].ToString();
                //}
                //else
                //{
                //    prebalamt.SecurityDeposityID = "0";
                //}
                

                foreach (DataRow dr in dsotherpre.Tables[3].Rows)
                {
                    searchListOPDIPD.Add(new PreBalanceAmount {

                        OPDIPDNO = dr["OPDIPDNO"].ToString(),
                        PrintOPDIPDNo = dr["OPDIPDNO"].ToString(),

                    });
                }
                foreach (DataRow dr in dsotherpre.Tables[2].Rows)
                {   if(dr["BillNo"].ToString()=="")
                    {
                        dr["BillDate"] = dr["Date"].ToString();
                    }
                    searchListTable.Add(new PreBalanceAmount
                    {
                         
                     OPDIPDNO=   dr["OPDIPDNO"].ToString(),
                     BillType=   dr["BillType"].ToString(),
                       DrAmount= Convert.ToDecimal( dr["DrAmount"].ToString()),
                       P_BillNo= dr["PrintBillNo"].ToString(),
                    // TDSAmount = Convert.ToDecimal(dr["TDSAmount"].ToString()),
                    // TPAOtherDeduction = Convert.ToDecimal(dr["TPAOtherDeduction"].ToString()),
                      CrAmount=   Convert.ToDecimal(dr["CrAmount"].ToString()),
                       BillDate= Convert.ToDateTime(dr["BillDate"]).ToString("dd-MMM-yyyy"),
                      BillNo=  dr["BillNo"].ToString(),
                     P_RegNo=   dr["PatientRegNO"].ToString(),
                      PaymentType=  dr["PaymentType"].ToString(),
                    OPDIPDID=   dr["OPDIPDNO"].ToString(),
                      PatientAccountRowID= Convert.ToInt32( dr["OtherAccountRowId"].ToString()),

                    });
                }
                prebalamt.PreBalance = dsotherpre.Tables[1].Rows[0]["PreBalance"].ToString();
                prebalamt.PatientRegNo = dsotherpre.Tables[0].Rows[0]["PatientRegNo"].ToString();
                //  prebalamt.f = dsotherpre.Tables[0].Rows[0]["FinancialYearID"].ToString();
                prebalamt.PrintOPDIPDNo = dsotherpre.Tables[2].Rows[0]["OPDIPDNO"].ToString();
                prebalamt.PatientName = dsotherpre.Tables[0].Rows[0]["PatientName"].ToString();
                //if (ds2.Tables[0].Rows.Count > 0)
                //{
                //    prebalamt.chkTPA = ds2.Tables[0].Rows[0]["TPAStatus"].ToString();
                //}
                searchList.Add(prebalamt);
            }
           
            //searchList.Add(prebalamt);
          
           return Json(new { prebalamt = prebalamt, searchListTable = searchListTable, searchListOPDIPD = searchListOPDIPD }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeletePreBalance(int PatientAccountRowID, string BillType, int Reg)
        {
            //  int PatientAccountRowID = Convert.ToInt32(Request.Form["PatientAccountRowID"]);

            string _Del = null;
            try
            {
                objblamt.DeleteOtherPreBalanceAmount(Convert.ToInt32(PatientAccountRowID));

                _Del = "Advice Deleted Successfully";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}