using KeystoneProject.Buisness_Logic.FinancialAccount;
using KeystoneProject.Models.Financial;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Financial
{
    public class UserPaymentAccountController : Controller
    {

        BL_UserPaymentAccount objbl = new BL_UserPaymentAccount();
        UserPaymentAccount objmodel = new UserPaymentAccount();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }

        [HttpGet]
        public ActionResult UserPaymentAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserPaymentAccount(UserPaymentAccount objmodels)
        {
            if (Request.Form["AccountsID"] != null || Request.Form["AccountsID"] != "")
            {
                objmodels.AccountsID = Request.Form["AccountsID"];
                objmodels.AccountName = Request.Form["AccountName"];
                objmodels.UserID = Request.Form["UserID"];
                objmodels.FullName = Request.Form["FullName"];
                objmodels.CurrentDate = Request.Form["CurrentDate"];
                objmodels.Narration = Request.Form["Narration"];
                objmodels.PaidAmount = Request.Form["PaidAmount"];
                //objmodels.BalanceAmount = Request.Form["BalanceAmount"];
                objmodels.ReferenceCode = Request.Form["ReferenceCode"];
                objmodels.DrAmount = Request.Form["DrAmount"];
                objmodels.CrAmount = Request.Form["CrAmount"];
                objmodels.Balance = Request.Form["Balance"];
            }
            else
            {
                objmodels.AccountsID = Request.Form["AccountsID1"];
                objmodels.AccountName = Request.Form["AccountName1"];
                objmodels.UserID = Request.Form["UserID1"];
                objmodels.FullName = Request.Form["FullName1"];
                objmodels.CurrentDate = "";
                objmodels.Narration = "";
                objmodels.PaidAmount = Request.Form["PaidAmount1"];
                //objmodels.BalanceAmount = Request.Form["BalanceAmount1"];
                objmodels.ReferenceCode = "";
                objmodels.DrAmount = Request.Form["DrAmount1"];
                objmodels.CrAmount = Request.Form["CrAmount1"];
                objmodels.Balance = Request.Form["Balance1"];
            }

            try
            {
                if (objbl.Save(objmodels))
                {
                    ModelState.Clear();
                    TempData["msg"] = "UserPaymentAccount Save Successfully";
                }
                else
                {
                    TempData["msg"] = "UserPaymentAccount Not Save";
                }
                return RedirectToAction("UserPaymentAccount", "UserPaymentAccount");

            }
            catch (Exception ex)
            {

                return RedirectToAction("UserPaymentAccount", "UserPaymentAccount");
            }
        }

        public JsonResult BindAccountMasterName(string prefix)
        {
            DataSet ds = objbl.BindMasterAccountName(prefix);
            List<UserPaymentAccount> searchList = new List<UserPaymentAccount>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new UserPaymentAccount
                {
                    AccountsID = dr["AccountsID"].ToString(),
                    AccountName = dr["AccountName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BindUserName(string prefix)
        {
            DataSet ds = objbl.BindUserName(prefix);
            List<UserPaymentAccount> searchList = new List<UserPaymentAccount>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new UserPaymentAccount
                {
                    UserID = dr["UserID"].ToString(),
                    FullName = dr["FullName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ShowAllUserAccount()
        {
            BL_UserPaymentAccount db = new BL_UserPaymentAccount();

            return new JsonResult { Data = db.SelectAllUserPaymentAccount(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBalance(int UserID)
        {
            string Types = "BALANCE";
            string CurrentDate = DateTime.Now.ToString("yyyy/MM/dd");
            DataSet dsBalance = new DataSet();
            dsBalance = objbl.GetBalanceAmount(UserID, CurrentDate, Types);
            List<UserPaymentAccount> searchlist = new List<UserPaymentAccount>();

            if (dsBalance.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBalance.Tables[0].Rows)
                {
                    searchlist.Add(new UserPaymentAccount
                    {
                        BalanceAmount = dr["Total"].ToString(),

                    });
                }
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUserStatement()
        {
            string Type = "USER";
           
            DataSet dsBalance = new DataSet();
            dsBalance = objbl.GetUserStatus(Type);
            List<UserPaymentAccount> searchlist = new List<UserPaymentAccount>();

            foreach (DataRow dr in dsBalance.Tables[0].Rows)
            {
                searchlist.Add(new UserPaymentAccount
                {
                    UserBalance = dr["Balance"].ToString(),

                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllUserStatus()
        {
            string Type = "USER";

            DataSet dsBalance = new DataSet();
            dsBalance = objbl.GetUserStatus(Type);
            List<UserPaymentAccount> searchlist = new List<UserPaymentAccount>();

            foreach (DataRow dr in dsBalance.Tables[0].Rows)
            {
                searchlist.Add(new UserPaymentAccount
                {
                    UserID = dr["UserID"].ToString(),
                    FullName = dr["FullName"].ToString(),
                    DrAmount = dr["DrAmount"].ToString(),
                    CrAmount = dr["CrAmount"].ToString(),
                    Balance = dr["Balance"].ToString(),

                });
            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllUserStatement(int UserID)
        {
            string Types = "USER STATEMENT";
            string CurrentDate = DateTime.Now.ToString("yyyy/MM/dd");
            DataSet dsBalance = new DataSet();
            dsBalance = objbl.GetBalanceAmount(UserID, CurrentDate, Types);
            List<UserPaymentAccount> searchlist = new List<UserPaymentAccount>();

            if (dsBalance.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBalance.Tables[0].Rows)
                {
                    searchlist.Add(new UserPaymentAccount
                    {
                        UserID = dr["UserID"].ToString(),
                        FullName = dr["FullName"].ToString(),
                        DrAmount = dr["DrAmount"].ToString(),
                        CrAmount = dr["CrAmount"].ToString(),
                        Balance = dr["Balance"].ToString(),
                       

                    });
                }
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllUserPayment()
        {
            string Type = "USER";

            DataSet dsBalance = new DataSet();
            DataSet ds = new DataSet();
            dsBalance = objbl.GetUserStatus(Type);
            ds = objbl.GetUserStatus(Type);
            List<UserPaymentAccount> searchlist = new List<UserPaymentAccount>();


            foreach (DataRow dr in dsBalance.Tables[0].Rows)
            {
                searchlist.Add(new UserPaymentAccount
                {
                    UserID = dr["UserID"].ToString(),
                    FullName = dr["FullName"].ToString(),
                    DrAmount = dr["DrAmount"].ToString(),
                    CrAmount = dr["CrAmount"].ToString(),
                    Balance = dr["Balance"].ToString(),

                    AccountsID = dsBalance.Tables[1].Rows[0]["AccountsID"].ToString(),
                    AccountName = dsBalance.Tables[1].Rows[0]["AccountName"].ToString(),


                });
                
            }
           

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}

