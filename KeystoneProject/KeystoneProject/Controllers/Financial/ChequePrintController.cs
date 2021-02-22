using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.Financial;
using KeystoneProject.Models.Financial;

namespace KeystoneProject.Controllers.Financial
{
    public class ChequePrintController : Controller
    {
        //
        // GET: /ChequePrint/
        [HttpGet]
        public ActionResult ChequePrint()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChequePrint(ChequePrint obj, FormCollection fm)
        {
            obj.BankID = fm["Bankname"];
            obj.ChequeAmount = fm["ChequeAmt"];
            obj.ChequeDate = fm["Cheque Date"];
            obj.ChequeBookID = fm["BookName"];
            obj.VoucherEntryID = Convert.ToInt32( fm["VoucherEntryID"]);
            obj.ChequePrintID = Convert.ToInt32(fm["ChequePrintID"]);
            obj.Narretion = fm["Narration"];
            obj.ChequeNo = fm["Cheque"];
            obj.BankName = fm["Bank"];
            KeystoneProject.Buisness_Logic.Financial.BL_ChequePrint BL_obj = new BL_ChequePrint();
            BL_obj.SaveChequePrint(obj);
            return View();
        }
        public ActionResult Log()
        {
            return View();
        }

        public ActionResult GetAllBankName()
        {
            BL_ChequePrint BL_obj = new BL_ChequePrint();
          
            return new JsonResult { Data = BL_obj.GetAllBankName(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetAllPayeeName()
        {
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.GetAllPayeeName(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult DeleteChequePayeeName(int BankID)
        {
            //KeystoneProject.Report.ChequePrintView o = new Report.ChequePrintView();
           
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.DeleteChequePayeeName(BankID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetChequeBookByBankID(int BankID, string BookName)
        {
            //KeystoneProject.Report.ChequePrintView o = new Report.ChequePrintView();
            Session["chqueBookNameID"] = BookName;
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.GetChequeBookByBankID(BankID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetChequeLayoutForCheckPrint(int ChequeBookID)
        {
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.GetChequeLayoutForCheckPrint(ChequeBookID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetAllChequePrint()
        {
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.GetAllChequePrint(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetWardDetailsPivot()
        {
            BL_ChequePrint BL_obj = new BL_ChequePrint();

            return new JsonResult { Data = BL_obj.GetWardDetailsPivot(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}