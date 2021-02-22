using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
namespace KeystoneProject.Controllers.Patient
{
    public class PaymentAndDepositController : Controller
    {
        //
        // GET: /RefundAmount/
        public ActionResult PaymentAndDeposit(string IPDNo)
        {
            Deposit objmodl = new Deposit();

            BL_Deposit obj1 = new BL_Deposit();

            if (IPDNo != null)
            {
                objmodl.ds1 = obj1.GetOtherRefundAmount(IPDNo);


                if (objmodl.ds1.Tables[1].Rows.Count > 0)
                {
                    objmodl.PatientRegNoPrint= objmodl.ds1.Tables[1].Rows[0]["P_RegNo"].ToString();
                    objmodl.PatientRegNo = objmodl.ds1.Tables[1].Rows[0]["PatientRegNO"].ToString();
                    //ucOtherRefundAmount1.txtPrintRegNo.Text = dsRefoundAmount.Tables[0].Rows[0]["PrintRegNO"].ToString();
                    //    objmodl. = objmodl.ds1.Tables[0].Rows[0]["FinancialYearID"].ToString();
                    objmodl.OPDIPDID = Convert.ToInt32( objmodl.ds1.Tables[1].Rows[0]["OPD/IPDID"]);
                    string[] Type = IPDNo.Split(',');
                    objmodl.PatientName = objmodl.ds1.Tables[1].Rows[0]["PatientName"].ToString();
                    // ucRefoundAmount2.txtRefundableAmount .Text =  dsRefoundAmount.Tables[1].Rows[0]["Amount"].ToString();
                    if (Type[1] != "OPD")
                    {

                        if (objmodl.ds1.Tables[3].Rows.Count > 0)
                        {

                            if (Convert.ToDecimal(objmodl.ds1.Tables[3].Rows[0]["CrAmount"]) > Convert.ToDecimal(objmodl.ds1.Tables[0].Rows[0]["PreBalance"]))
                            {
                                objmodl.Refundsecurity = objmodl.ds1.Tables[0].Rows[0]["PreBalance"].ToString().Replace("-", "");
                            }
                        }
                        else
                        {
                            objmodl.PreBalance = objmodl.ds1.Tables[0].Rows[0]["PreBalance"].ToString().Replace("-", "");
                        }



                        //if(objmodl.ds1.Tables[2].Rows[0]["SecurityDeposit"].ToString()!="")
                        //{
                        //    objmodl.PaidAmount = Convert.ToDecimal(objmodl.ds1.Tables[2].Rows[0]["SecurityDeposit"].ToString().Replace("-", ""));

                        //}


                        //objmodl.PaidAmount=Convert.ToDecimal(objmodl.ds1.Tables[2].Rows[0]["SecurityDeposit"].ToString().Replace("-", ""));



                        if (objmodl.ds1.Tables[2].Rows[0]["SecurityDeposit"].ToString() != "")
                        {
                            objmodl.PaidAmount = Convert.ToDecimal(objmodl.ds1.Tables[2].Rows[0]["SecurityDeposit"].ToString().Replace("-", ""));
                        }
                    }
                    else
                    {
                        objmodl.PreBalance = objmodl.ds1.Tables[0].Rows[0]["PreBalance"].ToString().Replace("-", "");
                    }
                   

                }
            }

            return View(objmodl);
        }
        [HttpPost]
        public ActionResult PaymentAndDeposit(Deposit obj,FormCollection fc)
        {
            BL_Deposit obj1 = new BL_Deposit();
            string chk = "";
            if (fc["securitydeposit"]== null)
            {
                chk = "";
            }
            else
            {
                chk = (fc["securitydeposit"].ToString());
            }
           
            if(chk=="on")
            {
                obj.BillType = "SecurityDeposit RefundBills";
            }
            else
            {
                obj.BillType = "RefundBills";
            }
            int RowID = obj1.IURefoundAmount(obj);

            if (RowID > 0)
            {
                Session["OtherAccountRowID"] = RowID;
                return RedirectToAction("RptRefundAmount", "PatientReport");
            }
            else
            {
                return View();
            }
        }

        public ActionResult FillData(int Reg)
        {
            return new JsonResult { Data = Reg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Calculate(string PaidAmount, string PreBalance)
        {
            Deposit objPreBalanceModel = new Deposit();
            List<string> search1 = new List<string>();
            if (PaidAmount != "")
            {

                //Decimal TotalAmount = Convert.ToDecimal(objPreBalanceModel.PreBalance) - Convert.ToDecimal(objPreBalanceModel.PaidAmount);
                Decimal TotalAmount = Convert.ToDecimal(PreBalance) - Convert.ToDecimal(PaidAmount);
                objPreBalanceModel.BalanceAmount = TotalAmount.ToString();

                search1.Add(objPreBalanceModel.BalanceAmount);
            }
            return Json(search1);
        }

      

        public ActionResult RptRefundAmount1(int PatientAccountRowID, string BillType, string RegNo, string OPDIPDNO)
        {
            Session["OtherAccountRowID"] = PatientAccountRowID;
            Session["BillTypePaymentAndDeposit"] = BillType;
            Session["RegNo1"] = RegNo;
            Session["OPDIPDNO1"] = OPDIPDNO;

            return RedirectToAction("RptRefundAmount", "PatientReport");
        }
        public ActionResult RptRefundAmount()
        {

            return View();
        }
	}
}