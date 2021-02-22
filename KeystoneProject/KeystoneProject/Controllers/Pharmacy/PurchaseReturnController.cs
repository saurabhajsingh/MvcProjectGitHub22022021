using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Pharmacy;
using KeystoneProject.Buisness_Logic.Pharmacy;
using System.Data.SqlClient;
using System.Data;
namespace KeystoneProject.Controllers.Pharmacy
{
    public class PurchaseReturnController : Controller
    {
        //
        // GET: /PurchaseReturn/
        BL_PurchaseReturn BL_obj = new BL_PurchaseReturn();
        public ActionResult PurchaseReturn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PurchaseReturn(PurchaseReturn obj, FormCollection fc)
        {
            obj.grossTotal = fc["grossTotal"];
            obj.ProductSupplierID = Convert.ToInt32(fc["SupplierID"]);
            obj.ProductSupplierName = fc["supplierName"].ToString();

            obj.ProductID = fc["ProductID"].ToString();
            obj.UserRate = fc["UserRate"].ToString();
            //obj.UserRate
            obj.Category = fc["Category"].ToString();
            obj.Barcode = fc["Barcode"].ToString();
            obj.ProductName = fc["ProductName"].ToString();
            obj.BatchNo = fc["BatchNo"].ToString();
            obj.ExpiryDate = fc["ExpiryDate"].ToString();
            obj.Qty = fc["Qty"].ToString();
            obj.Scheme = fc["Scheme"].ToString();
            obj.DisCountPer = fc["DisCountPer"].ToString();
            obj.LessByPer = fc["LessByPer"].ToString();

            obj.SchemeDiscountInPer = fc["SchemeDiscountInPer"].ToString();
            obj.Rate = fc["Rate"].ToString();
            // obj.TaxTypeID = fc["TaxTypeID"].ToString();
            obj.TotalAmount = fc["TotalAmount"].ToString();
            obj.remark = fc["remark"].ToString();
            if (BL_obj.Save(obj))
            {
            }
            return RedirectToAction("PurchaseReturn", "PurchaseReturn");
        }
        PurchaseReturn obj = new Models.Pharmacy.PurchaseReturn();
        public ActionResult BindSuplName(string Name)
        {
            DataSet ds = BL_obj.BindProductSupl(Name);
            List<PurchaseReturn> obj = new List<Models.Pharmacy.PurchaseReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PurchaseReturn
                {
                    ProductSupplierID = Convert.ToInt32(dr["ProductSupplierID"]),
                    ProductSupplierName = dr["ProductSupplierName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult BindProductName(string Name)
        {
            DataSet ds = BL_obj.BindProductName(Name);
            List<PurchaseReturn> obj = new List<Models.Pharmacy.PurchaseReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PurchaseReturn
                {
                    ProductID = dr["ProductID"].ToString(),
                    ProductName = dr["ProductName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }


        public ActionResult BindBatchNoOfProduct(int ProductID)
        {
            DataSet ds = BL_obj.BindBatchNoOfProduct(ProductID);
            List<PurchaseReturn> obj = new List<Models.Pharmacy.PurchaseReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PurchaseReturn
                {
                    ProductDetailsID = Convert.ToInt32(dr["ProductDetailsID"]),
                    BatchNo = dr["BatchNo"].ToString(),
                    ExpiryDate = dr["ExpiryDate"].ToString(),
                    PurchaseRate = dr["PurchaseRate"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult BindPostingAccount(int ProductID)
        {
            DataSet ds = BL_obj.BindPostingAccount(ProductID);
            List<PurchaseReturn> obj = new List<Models.Pharmacy.PurchaseReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PurchaseReturn
                {
                    AccountsID = dr["AccountsID"].ToString(),
                    AccountName = dr["AccountName"].ToString(),

                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult FillData(int SupplierID)
        {
            DataSet dsSupplier = new DataSet();
            List<PurchaseReturn> obj1 = new List<Models.Pharmacy.PurchaseReturn>();

            dsSupplier = BL_obj.GetProductPurchase(SupplierID);

            if (dsSupplier.Tables[0].Rows.Count > 0)
            {
                //ucProductPurchaseReturn1.txtSupplierName.Text = dsSupplier.Tables[0].Rows[0]["ProductSupplierName"].ToString();


                DataSet dsPatientBillNo = BL_obj.GetProductPurchaseReturnOLDBillsNO(SupplierID);
                obj1.Add(new Models.Pharmacy.PurchaseReturn
                {
                    Address = dsSupplier.Tables[0].Rows[0]["Address"].ToString(),

                });
                if (dsPatientBillNo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                    {
                        obj1.Add(new Models.Pharmacy.PurchaseReturn
                        {
                            BillNoAndDate = dr["BillNo&Date"].ToString(),
                            BillNo = dr["BillNo"].ToString()
                        });
                    }
                }

            }
            return new JsonResult { Data = obj1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult FillOldBillDetails(int BillNo)
        {
            List<PurchaseReturn> obj1 = new List<Models.Pharmacy.PurchaseReturn>();
            DataSet dsProductPurchaseReturn = new DataSet();
            DataSet dsProductPurchaseReturnDetails = new DataSet();

            int ProductPurchaseReturnID = Convert.ToInt32(BillNo);
            // dsProductPurchaseBills = obj_ProductPurchase_R.GetPatientBills(BillNo, HospitalID, LocationID);
            dsProductPurchaseReturn = BL_obj.GetProductPurchaseReturn(BillNo);
            dsProductPurchaseReturnDetails = BL_obj.GetProductPurchaseReturnDetails(BillNo);

            foreach (DataRow dr in dsProductPurchaseReturnDetails.Tables[0].Rows)
            {
                obj1.Add(new Models.Pharmacy.PurchaseReturn
                {
                    UserRate = dr["UseRate"].ToString(),
                    Category = dr["Category"].ToString(),
                    ProductName = dr["ProductName"].ToString(),
                    BatchNo = dr["Batch"].ToString(),
                    ExpiryDate = dr["ExpiryDate"].ToString(),
                    Qty = dr["Quantity"].ToString(),
                    Scheme = dr["Scheme"].ToString(),
                    DisCountPer = dr["DiscountInPer"].ToString(),
                    LessByPer = dr["LessByPer"].ToString(),
                    Rate = dr["Rate"].ToString(),
                    TotalAmount = dr["TotalAmount"].ToString(),
                });
            }
            //  ucProductPurchaseReturn1.dgvBillsDetails.DataSource = dsProductPurchaseReturnDetails.Tables[0];
            if (dsProductPurchaseReturn.Tables[0].Rows.Count > 0)
            {
                // ucProductPurchase1.txtGrossTotal.Text = dsProductPurchaseBills.Tables[0].Rows[0]["GrossAmount"].ToString();
                // ucProductPurchase1.txtTaxPrecent.Text = dsProductPurchaseBills.Tables[0].Rows[0]["TaxPercent"].ToString();
                // ucProductPurchase1.txtTaxAmount.Text = dsProductPurchaseBills.Tables[0].Rows[0]["TaxAmount"].ToString();
                // ucProductPurchase1.txtTotalAmount.Text = dsProductPurchaseBills.Tables[0].Rows[0]["TotalAmount"].ToString();
                // ucProductPurchase1.txtDiscount.Text = dsProductPurchaseBills.Tables[0].Rows[0]["DiscountPercent"].ToString();
                // ucProductPurchase1.txtDiscountAmount.Text = dsProductPurchaseBills.Tables[0].Rows[0]["DiscountAmount"].ToString();
                obj.remark = dsProductPurchaseReturn.Tables[0].Rows[0]["Remark"].ToString();
                obj.Address = dsProductPurchaseReturn.Tables[0].Rows[0]["Address"].ToString();
                obj.BillNo = dsProductPurchaseReturn.Tables[0].Rows[0]["ReffNo"].ToString();
                obj.grossTotal = dsProductPurchaseReturn.Tables[0].Rows[0]["GrossAmount"].ToString();
                obj.otherAdj = dsProductPurchaseReturn.Tables[0].Rows[0]["OtherLess"].ToString();
                //ucProductPurchase1.cbRatesInclusiveTax.Checked = Convert.ToBoolean(dsProductPurchaseBillsDetails.Tables[0].Rows[0]["RatesInclusiveTax"]);
                obj.returnDate = dsProductPurchaseReturn.Tables[0].Rows[0]["ReturnDate"].ToString();
                obj.returnDate = dsProductPurchaseReturn.Tables[0].Rows[0]["ReffBillDate"].ToString();
                obj.netAmount = dsProductPurchaseReturn.Tables[0].Rows[0]["NetAmount"].ToString();
                obj.taxType = "false";
                if (Convert.ToBoolean(dsProductPurchaseReturn.Tables[0].Rows[0]["IsPostingAccount"].ToString()) == true)
                {
                    obj.taxType = "IsPostingAccount";
                }
                if (Convert.ToBoolean(dsProductPurchaseReturn.Tables[0].Rows[0]["AdjustmentInInvoice"].ToString()) == true)
                {
                    obj.taxType = "AdjustmentInInvoice";
                }
                if (Convert.ToBoolean(dsProductPurchaseReturn.Tables[0].Rows[0]["RateInclusiveTax"].ToString()) == true)
                {
                    obj.taxType = "RateInclusiveTax";
                }

                // ucProductPurchase1.cmbTax.Text = dsProductPurchaseBillsDetails.Tables[0].Rows[0]["BillTaxType"].ToString();
                // ucProductPurchase1.txtAdvance.Text = dsPatientBills.Tables[0].Rows[0]["PreBalanceAmount"].ToString();
                // ucProductPurchase1.txtOtherAdg.Text = dsProductPurchaseBillsDetails.Tables[0].Rows[0]["OtherAdg"].ToString();
                // ucProductPurchase1.txtBillAmount.Text = dsProductPurchaseBills.Tables[0].Rows[0]["PaidAmount"].ToString();
            }
            return Json(new { obj1 = obj1, obj = obj }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FillDataByProduct(int ProductDetailsID)
        {
            DataSet dsReturnDetail = new DataSet();
            dsReturnDetail = BL_obj.GetProductPurchaseForProductDeatil(ProductDetailsID);
            if (dsReturnDetail.Tables[0].Rows.Count > 0)
            {
                //ucProductPurchaseReturn1.txtMRP.Text = dsProductDetail.Tables[0].Rows[0]["MRP"].ToString();
                //ucProductPurchaseReturn1.txtSaleRate.Text = dsProductDetail.Tables[0].Rows[0]["SalesRate"].ToString();
                obj.ExpiryDate = GetShortDatetForAdtp(Convert.ToDateTime(dsReturnDetail.Tables[0].Rows[0]["ExpiryDate"].ToString()));
                //ucProductPurchaseReturn1.txtPurchTax.Text = dsReturnDetail.Tables[0].Rows[0]["PurcTaxID"].ToString();
                obj.Rate = dsReturnDetail.Tables[0].Rows[0]["PurchaseRate"].ToString();



            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public string GetShortDatetForAdtp(DateTime TextDate)
        {
            string ShortDate = "";
            string Month = "";
            string Year = "";

            if (TextDate.Date.Month.ToString().Length == 1)
            {
                ShortDate = "0" + TextDate.Date.Month + "-" + TextDate.Date.Year.ToString().Substring(2, 2);
            }
            else
            {
                ShortDate = TextDate.Date.Month + "" + TextDate.Date.Year.ToString().Substring(2, 2);
            }
            return ShortDate;
        }

    }
}