using KeystoneProject.Buisness_Logic.FinancialAccount;
using KeystoneProject.Models.FinancialAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.FinancialAccount
{
    public class AccountStatementController : Controller
    {
       
        // GET: /AccountStatement/
        BL_AccountStatement objbl = new BL_AccountStatement();
        AccountStatement objmodel = new AccountStatement();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLoactionID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
         [HttpGet]
        public ActionResult AccountStatement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountStatement(FormCollection obj)
         {
             objmodel.FromDate = Convert.ToDateTime(obj["Fromdate"]);
             objmodel.ToDate = Convert.ToDateTime(obj["ToDate"]);
            objmodel.AccountsID = obj["AccountsID"];
            objmodel.AccountName = obj["account_name"];

            Session["FromDate"] = objmodel.FromDate;
            Session["ToDate"] = objmodel.ToDate;

            if (objmodel.AccountsID =="" || objmodel.AccountsID == null || objmodel.AccountsID =="All")
            {
                objmodel.AccountsID = "%";
            }

            objmodel.dsAccount = objbl.GetAccountStatementDetails(objmodel);
            DataSet ds1 = new DataSet();
            ds1 = objmodel.dsAccount;

            if (ds1.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    if (Convert.ToDecimal(dr["DrBalance"].ToString()) > 0)
                    {
                        dr["CrBalance"] = 0;
                    }
                    else if (Convert.ToDecimal(dr["DrBalance"].ToString()) < 0)
                    {
                         dr["DrBalance"] = 0;
                    }
                    else
                    {
                        dr["CrBalance"] = 0;
                        dr["DrBalance"] = 0;
                    }
                }
            }

             return View(objmodel);
         }
        public ActionResult DoubleClick(string AccountsID)
        {
            List<AccountStatement> searchlist1 = new List<AccountStatement>();
            objmodel.AccountsID = AccountsID;
            objmodel.FromDate =Convert.ToDateTime(Session["FromDate"]);
            objmodel.ToDate = Convert.ToDateTime(Session["ToDate"]);
            DataSet ds = objbl.GetAccountStatementForAccountDetail(objmodel);

            foreach (DataRow dr in ds.Tables[0].Rows)
           
            {
                searchlist1.Add(new AccountStatement
                {
                    AccountName = dr["AccountName"].ToString(),
                    FromEntryTypeID = dr["FromEntryTypeID"].ToString(),
                    TransactionType = dr["TransactionType"].ToString(),
                    Particular = dr["Particular"].ToString(),
                    Narration = dr["Narration"].ToString(),
                    DrAmount = Convert.ToDecimal(dr["DrAmount"].ToString()),
                    CrAmount = Convert.ToDecimal(dr["CrAmount"].ToString()),
                });
            }

            return new JsonResult { Data = searchlist1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult AccountName(string prefix)
        {
            BL_AccountStatement objAcc = new BL_AccountStatement();
            List<AccountStatement> searchlist = new List<AccountStatement>();
            DataSet ds = objAcc.BindAccountStatement(prefix);
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AccountStatement AccStat = new AccountStatement();
                AccStat.AccountsID = dr["AccountsID"].ToString();
                AccStat.AccountName = dr["AccountName"].ToString();
                searchlist.Add(AccStat);
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}