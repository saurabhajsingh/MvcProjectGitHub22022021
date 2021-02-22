using KeystoneProject.Buisness_Logic;
using KeystoneProject.Buisness_Logic.PharmacyMaster;
using KeystoneProject.Models.PharmacyMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.PharmacyMaster
{
    public class TaxInformationCompanyController : Controller
    {
        BL_TaxInformationCompany bl_tax = new BL_TaxInformationCompany();
        List<TaxInformationCompany> list_tax = new List<TaxInformationCompany>();
        public ActionResult TaxInformationCompany()
        {
            return View();
        }

        public JsonResult Bind_Table()
        {
            return new JsonResult { Data = bl_tax.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };
        }
        public JsonResult Bind_Posting_acc(string prefix)
        {
            DataSet ds = bl_tax.Bind_PostAccount(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_tax.Add(
                    new TaxInformationCompany
                    {
                        postingAccount = Convert.ToString(dr["AccountName"]),
                        postingAccountID = Convert.ToString(dr["AccountsID"]),
                        
                    });
            }
            return new JsonResult { Data = list_tax, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Bind_Tax_acc(string prefix)
        {
            DataSet ds = bl_tax.Bind_TaxAccount(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_tax.Add(
                    new TaxInformationCompany
                    {
                        taxInformationDetailTaxAccount = Convert.ToString(dr["AccountName"]),
                        taxInformationDetailTaxAccountID = Convert.ToString(dr["AccountsID"]),
                    });
            }
            return new JsonResult { Data = list_tax, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Bind_Surcharge_acc(string prefix)
        {
            DataSet ds = bl_tax.Bind_SrchrgAccount(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_tax.Add(
                    new TaxInformationCompany
                    {
                        surchargeAccount = Convert.ToString(dr["AccountName"]),
                        surchargeAccountID = Convert.ToString(dr["AccountsID"]),
                    });
            }
            return new JsonResult { Data = list_tax, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        [HttpPost]
        public ActionResult TaxInformationCompany(TaxInformationCompany obj, FormCollection fc)
        {
            if (bl_tax.Check_TaxCompany(obj.taxInformationTypeID, obj.code))
            {
                if (bl_tax.SAVE(obj))
                {
                    if (obj.taxInformationTypeID > 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "Tax Information Company Updated Successfully !";
                        return RedirectToAction("TaxInformationCompany", "TaxInformationCompany");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "Tax Information Company Saved Successfully !";
                        return RedirectToAction("TaxInformationCompany", "TaxInformationCompany");
                    }
                }
                else
                {
                    TempData["Msg"] = "Tax Information Company Not Saved !";
                }
            }
            else
            {
                ViewData["flag"] = "Error";
                TempData["Msg"] = "Tax Information Company Already Exist ";

            }
            return RedirectToAction("TaxInformationCompany", "TaxInformationCompany");
        }

        public JsonResult Rebind_Data(int taxInformationTypeID)
        {
            return new JsonResult { Data = bl_tax.Bind_From_Table(taxInformationTypeID), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Delete_Tax(int taxInformationTypeID)
        {
            string data="";
            try
            {
                int a = bl_tax.Delete_data(taxInformationTypeID);
                if (a == 1 )
                {
                    data = "Tax Information Company Deleted Successfully";
                }
            }
            catch(Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}