using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Other;

namespace KeystoneProject.Controllers.Other
{
    public class OtherRefundAmountController : Controller
    {
        // GET: OtherRefundAmount
    [HttpGet]
        public ActionResult OtherRefundAmount(int Reg)
        {
            Deposit objmodl = new Deposit();

            BL_OtherRefundAmount obj1 = new Buisness_Logic.Other.BL_OtherRefundAmount();

            if (Reg != null)
            {
                objmodl.ds1 = obj1.GetOtherRefundAmount(Reg);
                if (objmodl.ds1.Tables[2].Rows.Count > 0)
                {
                    objmodl.PatientRegNo = objmodl.ds1.Tables[2].Rows[0]["PatientRegNO"].ToString();
                    //ucOtherRefundAmount1.txtPrintRegNo.Text = dsRefoundAmount.Tables[0].Rows[0]["PrintRegNO"].ToString();
                    //objmodl. = objmodl.ds1.Tables[0].Rows[0]["FinancialYearID"].ToString();
                    objmodl.OPDIPDID = Convert.ToInt32(objmodl.ds1.Tables[2].Rows[0]["OPD/IPDID"]);
                    objmodl.PatientName = objmodl.ds1.Tables[0].Rows[0]["PatientName"].ToString();
                    // ucRefoundAmount2.txtRefundableAmount .Text =  dsRefoundAmount.Tables[1].Rows[0]["Amount"].ToString();
                    objmodl.PreBalance = objmodl.ds1.Tables[1].Rows[0]["PreBalance"].ToString().Replace("-", "");
                }
            }
            return View(objmodl);
        }
        [HttpPost]
        public ActionResult OtherRefundAmount(Deposit obj)
        {
            BL_OtherRefundAmount obj1 = new BL_OtherRefundAmount();

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
    }
}