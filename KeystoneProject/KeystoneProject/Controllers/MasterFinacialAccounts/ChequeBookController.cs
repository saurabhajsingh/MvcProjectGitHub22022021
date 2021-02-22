using KeystoneProject.Buisness_Logic.MasterFinacialAccounts;
using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MasterFinacialAccounts
{
    public class ChequeBookController : Controller
    {

        BL_ChequeBook objbl = new BL_ChequeBook();
        ChequeBook objmodel = new ChequeBook();

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
        public ActionResult ChequeBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChequeBook(ChequeBook objmodels)
        {

            objmodels.ChequeBookID = Request.Form["ChequeBookID"];
            objmodels.ChequeBookName = Request.Form["ChequeBookName"];
            objmodels.BankID = Request.Form["BankID"];
            objmodels.BankName = Request.Form["BankName"];
            objmodels.BankAccountID = Request.Form["BankAccountID"];
            objmodels.AccountName = Request.Form["AccountName"];
            objmodels.ChequeLayoutID = Request.Form["ChequeLayoutID"];
            objmodels.LayoutName = Request.Form["LayoutName"];

            if (objbl.Save(objmodels))
            {

            }
            return View();


        }
        public JsonResult ShowAllChequeBook()
        {
            BL_ChequeBook db = new BL_ChequeBook();

            return new JsonResult { Data = db.SelectAllChequeBook(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllBank()
        {
            DataSet ds = objbl.GetAllBank();
            List<ChequeBook> searchlist = new List<ChequeBook>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new ChequeBook
                {
                    BankID = dr["BankID"].ToString(),
                    BankName = dr["BankName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAccountNameByBankID(string BankID)
        {
            DataSet ds = objbl.GetAccountNameByBankID(BankID);
            List<ChequeBook> searchlist = new List<ChequeBook>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new ChequeBook
                {
                    BankAccountID = dr["BankAccountID"].ToString(),
                    AccountName = dr["AccountName"].ToString(),
                    ChequeLayoutID = dr["ChequeLayoutID"].ToString(),
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Layout(string ChequeLayoutID)
        {
            DataSet ds = objbl.Layout(ChequeLayoutID);
            List<ChequeBook> searchlist = new List<ChequeBook>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new ChequeBook
                {
                    ChequeLayoutID = dr["ChequeLayoutID"].ToString(),
                    LayoutName = dr["LayoutName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult EditChequeBook(int ChequeBookID)
        {

            KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_ChequeBook objDBService = new BL_ChequeBook();
            ChequeBook ServiceModl = new ChequeBook();
            List<ChequeBook> SearchList = new List<ChequeBook>();


            DataSet ds = objDBService.FillData(ChequeBookID);

            ServiceModl.ChequeBookID = ds.Tables[0].Rows[0]["ChequeBookID"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new ChequeBook
                {
                    BankID = dr["BankID"].ToString(),
                    BankName = dr["BankName"].ToString(),
                    BankAccountID = dr["BankAccountID"].ToString(),
                    AccountName = dr["AccountName"].ToString(),
                    ChequeBookID = dr["ChequeBookID"].ToString(),
                    ChequeLayoutID = dr["ChequeLayoutID"].ToString(),
                    LayoutName = dr["LayoutName"].ToString(),
                    ChequeBookName = dr["BookName"].ToString(),
                    ChequeNoFrom = dr["ChequeSNo"].ToString(),
                    ChequeNoTo = dr["ChequeENo"].ToString(),

                });
            }

            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult DeleteCheckBook(int ChequeBookID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_ChequeBook objdb = new BL_ChequeBook();
                ChequeBook objSG = new Models.MasterFinacialAccounts.ChequeBook();

                int DependaincyName = objdb.DeleteCheckBook(ChequeBookID);

                if (DependaincyName == 1)
                {
                    _Del = "CheckBook Deleted Successfully";
                }
                else
                {
                    _Del = "Can not Delete";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
      

    }
}