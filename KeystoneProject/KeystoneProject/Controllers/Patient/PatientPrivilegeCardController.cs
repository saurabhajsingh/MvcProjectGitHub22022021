using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientPrivilegeCardController : Controller
    {
        //
        // GET: /PrivilegeCard/
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        int PatientPrivilegeCardID = Convert.ToInt32(System.Web.HttpContext.Current.Session["PatientPrivilegeCardID"]);
        int PrivilegeCardID = Convert.ToInt32(System.Web.HttpContext.Current.Session["PrivilegeCardID"]);
        int PrivilegeCardID1;
       int PrivilegePriceDetailID = Convert.ToInt32(System.Web.HttpContext.Current.Session["PrivilegePriceDetailID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        public ActionResult PatientPrivilegeCard()
        {
            return View();
        }
       [HttpGet]
        public JsonResult EditPatientPrivilegeCard(int PatientRegNo)
        {

            ModelState.Clear();

            return new JsonResult { Data = _PatientPrivilegeCard.GetPatientPrivilegeCard(PatientPrivilegeCardID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

       [HttpPost]
       public ActionResult Delete(int PatientPrivilegeCardID)
       {
           string del = "";
           PatientPrivilegeCardID =Convert.ToInt32( Request.Form["PatientPrivilegeCardID"]);
           BL_PatientPrivilegeCard _PatientPrivilegeCard = new BL_PatientPrivilegeCard();
           if (_PatientPrivilegeCard.DeletePatientPrivilegeCard(PatientPrivilegeCardID))
           {
               del = "Delete";
           }

           return Json(del, JsonRequestBehavior.AllowGet);
       }

       public JsonResult BindPatientPrefix(string prefix)
       {
           return new JsonResult { Data = _PatientPrivilegeCard.BindPrefixPatient(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
       }

        public JsonResult ShowPatientPrivilegeCard()
        {

            BL_PatientPrivilegeCard _PatientPrivilegeCard = new BL_PatientPrivilegeCard();


            return new JsonResult { Data = _PatientPrivilegeCard.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        [HttpPost]
        public ActionResult PatientPrivilegeCard(PatientPrivilegeCard obj,FormCollection fc)
        {
            BL_PatientPrivilegeCard _PatientPrivilegeCard = new BL_PatientPrivilegeCard();
            obj.CardName = "";
            obj.CardName = fc["CardName1"].ToString();
            obj.PrivilegeCardID =Convert.ToInt32( fc["CardName"].ToString());
            obj.PriceAmt = Convert.ToInt32(fc["Price"].ToString());
            obj.PriceName = fc["PriceName1"].ToString();
            obj.PrivilegePriceDetailID = fc["PrivilegePriceDetailID"].ToString();

       
          //  obj.PatientPrivilegeCardID =Convert.ToInt32(fc["PatientPrivilegeCardID"].ToString());
            //obj.AmountName = fc["AccountName1"].ToString();
            


            if (_PatientPrivilegeCard.Save(obj))
            {
                ModelState.Clear();
                TempData["msg"] = "Record Saved Successfully";
            }

            else
            {
                ViewData["flag"] = "Error";
            }
            return RedirectToAction("PatientPrivilegeCard", "PatientPrivilegeCard");
              


           // return View();
        }
        BL_PatientPrivilegeCard _PatientPrivilegeCard = new BL_PatientPrivilegeCard();

        public JsonResult GetAllFinancialYear()
        {
            DataSet ds = _PatientPrivilegeCard.GetAllFinancialYear();
            List<PatientPrivilegeCard> searchList = new List<PatientPrivilegeCard>();
            DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                searchList.Add(new PatientPrivilegeCard
                {
                    FinancialYear = dr["FinancialYear"].ToString(),
                    FinancialYearID =Convert.ToInt32(dr["FinancialYearID"])
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillPatient(string PatientRegNO)
        {
            BL_PatientPrivilegeCard blPatientPrivilegeCard = new BL_PatientPrivilegeCard();
            DataSet ds = blPatientPrivilegeCard.CheckPatientReg(PatientRegNO);
            List<PatientPrivilegeCard> searchList = new List<PatientPrivilegeCard>();
            string chk = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                searchList.Add(new PatientPrivilegeCard
                {
                    chk = "Privilege Card Already Created",
                });
                return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                return new JsonResult { Data = _PatientPrivilegeCard.FillPatient(PatientRegNO), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetAllPrivilegePriceName(int PrivilegeCardID)

        {
  
            DataSet ds = _PatientPrivilegeCard.GetAllPrivilegePriceName( HospitalID, LocationID, Convert.ToInt32(PrivilegeCardID));
            List<PatientPrivilegeCard> searchList = new List<PatientPrivilegeCard>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dvTest = new DataView(ds.Tables[0], "PrivilegePriceDetailID = " + ds.Tables[0].Rows[0]["PrivilegePriceDetailID"].ToString() + " ", "", DataViewRowState.CurrentRows);
                dvTest.ToTable().Rows[0]["PriceName"].ToString();
                foreach (DataRow dr in dvTest.ToTable().Rows)
                {
                    searchList.Add(new PatientPrivilegeCard
                    {
                        //PaidAmt=Convert.ToInt32(dr["PaidAmt"]),
                        //AmountName=dr["AmountName"].ToString(),
                        PriceName = dr["PriceName"].ToString(),
                        PriceAmt = Convert.ToDecimal(dr["Price"].ToString()),
                        PrivilegePriceDetailID = dr["PrivilegePriceDetailID"].ToString(),

                        PrivilegeCardID = Convert.ToInt32(dr["PrivilegeCardID"])
                    });
                }
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllPrivilegePrice(int PrivilegePriceDetailID)
        {
            DataSet ds = _PatientPrivilegeCard.GetAllPrivilegePriceName(HospitalID, LocationID, Convert.ToInt32(PrivilegePriceDetailID));
            List<PatientPrivilegeCard> searchList = new List<PatientPrivilegeCard>();
            DataView dvTest = new DataView(ds.Tables[0], "PrivilegePriceDetailID = " + ds.Tables[0].Rows[0]["PrivilegePriceDetailID"].ToString() + " ", "", DataViewRowState.CurrentRows);
            dvTest.ToTable().Rows[0]["Price"].ToString();
            foreach (DataRow dr in dvTest.ToTable().Rows)
            {
                searchList.Add(new PatientPrivilegeCard
                {
                    PriceAmt = Convert.ToDecimal(dr["Price"].ToString()),

                    PrivilegePriceDetailID =dr["PrivilegePriceDetailID"].ToString()
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
     


        public JsonResult RegisteredPatient(string PatientName)
        {

            return new JsonResult { Data = _PatientPrivilegeCard.BindPatient(PatientName), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CheckPatientReg(string PatientName)
        {

            BL_PatientPrivilegeCard blPatientPrivilegeCard = new BL_PatientPrivilegeCard();
         //   blPatientPrivilegeCard.CheckPatientName( PatientName);
            List<PatientPrivilegeCard> searchList = new List<PatientPrivilegeCard>();
            string chk = "";
            if (blPatientPrivilegeCard.CheckPatientName(PatientName))
            {
                chk = "Privilege Card Already Created";
            }


            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult AccountsName(string prefix)
        {

            return new JsonResult { Data = _PatientPrivilegeCard.AccountName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Card(string CardName)
        {

            return new JsonResult { Data = _PatientPrivilegeCard.PrivilegeCard(CardName), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPatientPrintNo_ToRegNo(string PrintRegNo)
        {
            BL_PatientPrivilegeCard BL_Reg = new BL_PatientPrivilegeCard();
            string RegNo = BL_Reg.GetPatientPrintNo_ToRegNo(PrintRegNo);
            return new JsonResult { Data = RegNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

    }
}