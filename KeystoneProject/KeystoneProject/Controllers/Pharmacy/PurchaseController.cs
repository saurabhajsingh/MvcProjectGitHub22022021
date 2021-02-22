using KeystoneProject.Buisness_Logic.Pharmacy;
using KeystoneProject.Models.Pharmacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using KeystoneProject.Models.Master;

namespace KeystoneProject.Controllers.Pharmacy
{
    public class PurchaseController : Controller
    {
        BL_Purchase objbl = new BL_Purchase();
        Purchase objmodel = new Purchase();

        List<Purchase> purchase = new List<Purchase>();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
        //
        // GET: /Purchase/
        [HttpGet]
        public ActionResult Purchase()
        {
            return View();
        }
        public JsonResult BindSupplier(string prefix)
        {
            DataSet ds = objbl.BindSupplier(prefix);
            List<Purchase> searchList = new List<Purchase>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Purchase
                {
                    ProductSupplierID = dr["ProductSupplierID"].ToString(),
                    ProductSupplierName = dr["ProductSupplierName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Get_Product(string prefix)
        {
            DataSet ds = objbl.Bind_productdetail(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                purchase.Add(
                    new Purchase
                    {
                        productName1 = Convert.ToString(dr["ProductName"]),
                        ProductID1 = Convert.ToString(dr["ProductID"]),
                    });
            }
            return new JsonResult { Data = purchase, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult GetProductSupplier(string ProductSupplierID)
        {
            BL_Purchase db = new BL_Purchase();

            DataSet dsBalance = new DataSet();
            DataSet ds = new DataSet();
            dsBalance = objbl.GetProductSupplier(ProductSupplierID);
            ds = objbl.GetProductSupplier(ProductSupplierID);
            List<Purchase> searchlist = new List<Purchase>();


            foreach (DataRow dr in dsBalance.Tables[0].Rows)
            {
                searchlist.Add(new Purchase
                {
                    ProductSupplierID = Convert.ToString(dr["ProductSupplierID"]),
                    ProductSupplierName = Convert.ToString(dr["ProductSupplierName"]),
                    Address = Convert.ToString(dr["Address"]),

                    Balance = dsBalance.Tables[1].Rows[0]["Balance"].ToString(),

                });

            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetProductPurchaseOLDBillsNO(string ProductSupplierID)
        {
            BL_Purchase db = new BL_Purchase();

            DataSet dsBalance = new DataSet();
            DataSet ds = new DataSet();
            dsBalance = objbl.GetProductPurchaseOLDBillsNO(ProductSupplierID);
            ds = objbl.GetProductPurchaseOLDBillsNO(ProductSupplierID);
            List<Purchase> searchlist = new List<Purchase>();


            foreach (DataRow dr in dsBalance.Tables[0].Rows)
            {
                searchlist.Add(new Purchase
                {
                    ProductSupplierID = Convert.ToString(dr["ProductSupplierID"]),
                    BillNo = Convert.ToString(dr["BillNo"]),
                    BillNoDate = Convert.ToString(dr["BillNo&Date"]),


                });

            }


            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult GetBatchNo(string ProductID)
        //{
        //    BL_Purchase db = new BL_Purchase();

        //    DataSet dsBalance = new DataSet();
        //    DataSet ds = new DataSet();
        //    dsBalance = objbl.BindBatchNoOfProduct(ProductID);

        //    List<Purchase> searchlist = new List<Purchase>();


        //    foreach (DataRow dr in dsBalance.Tables[0].Rows)
        //    {
        //        searchlist.Add(new Purchase
        //        {
        //            BatchNo = Convert.ToString(dr["BatchNo"]),
        //            ProductDetailsID = Convert.ToString(dr["ProductDetailsID"]),
        //        });

        //    }

        //    return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        //public ActionResult BindBatchDetails(string productName, string batchNumber)
        //{


        //    DataTable dt = new DataTable();
        //    DataSet ds = objbl.BindBatchDetails(productName, "%");
        //    dt = objbl.GetProductBatchNo(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductID"].ToString()), batchNumber);

        //    List<Purchase> searchlist = new List<Purchase>();


        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        searchlist.Add(new Purchase
        //        {
        //            MRP = Convert.ToString(dr["MRP"]),
        //            PurchaseRate = Convert.ToString(dr["PurchaseRate"]),
        //            SalesRate = Convert.ToString(dr["SalesRate"]),
        //            ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
        //            GST = Convert.ToString(dr["GST"]),
        //            CGST = Convert.ToString(dr["CGST"]),
        //            SGST = Convert.ToString(dr["SGST"]),
        //            UTGST = Convert.ToString(dr["UTGST"]),
        //        });

        //    }
        //    return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}

        [HttpPost]
        public ActionResult Purchase(Purchase obj, FormCollection fc)
        {
            try
            {
                obj.ProductPurchaseID = fc["ProductPurchaseID"].ToString();
                obj.ProductSupplierID = fc["ProductSupplierID"].ToString();
                obj.ProductSupplierName = fc["supplierName"].ToString();
                obj.Address = fc["address"].ToString();
                obj.SupplierRemark = fc["SupplierRemark"].ToString();
                obj.billDate = fc["billDate"].ToString();
                obj.BillDiscountPercent = fc["billDate"].ToString();
                obj.BillNo = fc["billNumber"].ToString();
                obj.grossTotal = fc["grossTotal"].ToString();
                obj.discountAmt = fc["discountAmt"].ToString();
                obj.taxAmount = fc["taxAmount"].ToString();
                obj.totalAmount = fc["totalAmount"].ToString();
                obj.otherAdj = fc["otherAdj"].ToString();
                obj.lessCreditDebit = fc["lessCreditDebit"].ToString();
                obj.netAmount = fc["netAmount"].ToString();
                obj.payment_type = fc["payment_type"].ToString();
                obj.billAmount = fc["billAmount"].ToString();
                obj.currentBalance = fc["currentBalance"].ToString();
                obj.cheque = fc["cheque"].ToString();
                obj.bankName = fc["bankName"].ToString();
                obj.chequeDate = fc["chequeDate"].ToString();
                obj.Remarks = fc["Remarks"].ToString();
                obj.ProductID1 = fc["ProductID1"].ToString();
                obj.batchNumber1 = fc["batchNumber1"].ToString();
                obj.HSNSACCode1 = fc["HSNSACCode1"].ToString();
                obj.expiry1 = fc["expiry1"].ToString();
                obj.mrp1 = fc["mrp1"].ToString();
                obj.salesRate1 = fc["salesRate1"].ToString();
                obj.purchaseRate1 = fc["purchaseRate1"].ToString();
                obj.quantity1 = fc["quantity1"].ToString();
                obj.free1 = fc["free1"].ToString();
                obj.discount1 = fc["discount1"].ToString();
                obj.discountSymbol1 = fc["discountSymbol1"].ToString();
                obj.lessBy1 = fc["lessBy1"].ToString();
                obj.lessBySymbol1 = fc["lessBySymbol1"].ToString();
                obj.totalamount1 = fc["totalamount1"].ToString();
                obj.productName1 = fc["productName1"].ToString();
                obj.ProductDetailsID1 = fc["ProductDetailsID1"].ToString();
                obj.gst1 = fc["gst1"].ToString();
                obj.sgst1 = fc["sgst1"].ToString();
                obj.cgst1 = fc["cgst1"].ToString();
                obj.utgst1 = fc["utgst1"].ToString();

                if (objbl.Save(obj))
                {
                    if (obj.ProductPurchaseID == "0" || obj.ProductPurchaseID == "" || obj.ProductPurchaseID == null)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Purchase Saved Successfully !";
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Purchase Updated Successfully !";
                    }

                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Purchase", "Purchase");
            }
            return RedirectToAction("Purchase", "Purchase");
        }

        public ActionResult FillOldBillDetails(int BillNo)
            {
            List<Purchase> obj1 = new List<Models.Pharmacy.Purchase>();
            DataSet dsProductPurchaseReturn = new DataSet();
            DataSet dsProductPurchaseReturnDetails = new DataSet();

            int ProductPurchaseID = Convert.ToInt32(BillNo);

            dsProductPurchaseReturn = objbl.GetProductPruchase1(BillNo);
            dsProductPurchaseReturnDetails = objbl.GetProductPruchaseDetails(BillNo);

            foreach (DataRow dr in dsProductPurchaseReturnDetails.Tables[0].Rows)
            {
                obj1.Add(new Models.Pharmacy.Purchase
                {
                    //ProductPurchaseDetailID = dr["ProductPurchaseDetailID"].ToString(),
                    ProductPurchaseID = dr["ProductPurchaseID"].ToString(),
                    ProductID1 = dr["ProductID"].ToString(),
                    productName1 = dr["ProductName"].ToString(),
                    batchNumber1 = dr["BatchNo"].ToString(),
                    ProductDetailsID1 = dr["ProductDetailID"].ToString(),
                    expiry1 = dr["Expiry"].ToString(),
                    HSNSACCode1 = dr["HSNCode"].ToString(),
                    purchaseRate1 = dr["PurchaseRate"].ToString(),
                    mrp1 = dr["MRPRate"].ToString(),
                    salesRate1 = dr["SaleRate"].ToString(),
                    quantity1 = dr["Quantity"].ToString(),
                    free1 = dr["FreeQuantity"].ToString(),
                    TaxRate = dr["TaxRate"].ToString(),
                    discount1 = dr["DiscBy"].ToString(),
                    discountSymbol1 = dr["DiscountType"].ToString(),
                    lessBy1 = dr["LessBy"].ToString(),
                    lessBySymbol1 = dr["LessByType"].ToString(),
                    totalamount1 = dr["TotalAmount"].ToString(),
                    gst1 = dr["GST"].ToString(),
                    sgst1 = dr["SGST"].ToString(),
                    cgst1 = dr["CGST"].ToString(),
                    utgst1 = dr["UTGST"].ToString(),

                });
            }

            if (dsProductPurchaseReturn.Tables[0].Rows.Count > 0)
            {
                objmodel.ProductPurchaseID = dsProductPurchaseReturn.Tables[0].Rows[0]["ProductPurchaseID"].ToString();
                objmodel.ProductSupplierID = dsProductPurchaseReturn.Tables[0].Rows[0]["ProductSupplierID"].ToString();
                objmodel.ProductSupplierName = dsProductPurchaseReturn.Tables[0].Rows[0]["SupplierName"].ToString();
                objmodel.Address = dsProductPurchaseReturn.Tables[0].Rows[0]["Address"].ToString();
                objmodel.SupplierRemark = dsProductPurchaseReturn.Tables[0].Rows[0]["SupplierRemark"].ToString();
                objmodel.billDate = dsProductPurchaseReturn.Tables[0].Rows[0]["BillDate"].ToString();
                objmodel.Date = dsProductPurchaseReturn.Tables[0].Rows[0]["Date"].ToString();
                objmodel.DueDate = dsProductPurchaseReturn.Tables[0].Rows[0]["DueDate"].ToString();
                objmodel.BillDiscountPercent = dsProductPurchaseReturn.Tables[0].Rows[0]["BillDiscountPercent"].ToString();
                objmodel.BillNo = dsProductPurchaseReturn.Tables[0].Rows[0]["BillNo"].ToString();
                objmodel.grossTotal = dsProductPurchaseReturn.Tables[0].Rows[0]["GrossAmount"].ToString();
                objmodel.discountAmt = dsProductPurchaseReturn.Tables[0].Rows[0]["DiscountAmount"].ToString();
                objmodel.taxAmount = dsProductPurchaseReturn.Tables[0].Rows[0]["TaxAmount"].ToString();
                objmodel.totalAmount = dsProductPurchaseReturn.Tables[0].Rows[0]["TotalAmount"].ToString();
                objmodel.lessCreditDebit = dsProductPurchaseReturn.Tables[0].Rows[0]["LessCrDr"].ToString();
                objmodel.netAmount = dsProductPurchaseReturn.Tables[0].Rows[0]["NetAmount"].ToString();
                objmodel.otherAdj = dsProductPurchaseReturn.Tables[0].Rows[0]["OtherAdg"].ToString();
                objmodel.billAmount = dsProductPurchaseReturn.Tables[0].Rows[0]["BillAmount"].ToString();
                objmodel.PurchaseTaxType = dsProductPurchaseReturn.Tables[0].Rows[0]["PurchaseTaxType"].ToString();
                objmodel.PurchaseTax = dsProductPurchaseReturn.Tables[0].Rows[0]["PurchaseTax"].ToString();
                objmodel.currentBalance = dsProductPurchaseReturn.Tables[0].Rows[0]["CurBalance"].ToString();
                objmodel.BillType = dsProductPurchaseReturn.Tables[0].Rows[0]["BillType"].ToString();
                objmodel.payment_type = dsProductPurchaseReturn.Tables[0].Rows[0]["PaymentType"].ToString();
                objmodel.cheque = dsProductPurchaseReturn.Tables[0].Rows[0]["Number"].ToString();
                objmodel.bankName = dsProductPurchaseReturn.Tables[0].Rows[0]["Name"].ToString();
                objmodel.chequeDate = dsProductPurchaseReturn.Tables[0].Rows[0]["ChequeDate"].ToString();
                objmodel.Remarks = dsProductPurchaseReturn.Tables[0].Rows[0]["Remarks"].ToString();
            }
            return Json(new { obj1 = obj1, objmodel = objmodel }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AjaxMethod_CheckProduct(string productNameID1, FormCollection fc)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_Purchase BL_obj = new BL_Purchase();
            KeystoneProject.Models.Pharmacy.Purchase obj = new Purchase();
            List<string> searchList = new List<string>();

            try
            {
                DataTable dt = new DataTable();
                DataSet ds = BL_obj.Bind_Newproductdetail("%", productNameID1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = BL_obj.Bind_PackQtyRate(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductID"].ToString()));
                    obj.HSNSACCode1 = dt.Rows[0]["HSNSACCode"].ToString();
                    obj.gst1 = dt.Rows[0]["GST"].ToString();
                    obj.sgst1 = dt.Rows[0]["SGST"].ToString();
                    obj.cgst1 = dt.Rows[0]["CGST"].ToString();
                    obj.utgst1 = dt.Rows[0]["UTGST"].ToString();

                    searchList.Add(obj.HSNSACCode1);
                    searchList.Add(obj.gst1);
                    searchList.Add(obj.sgst1);
                    searchList.Add(obj.cgst1);
                    searchList.Add(obj.utgst1);
                }
                return Json(searchList);
            }
            catch (Exception ex)
            {
                return Json(searchList);
            }
        }

        public JsonResult Bind_HSNCODE(string prefix)
        {

            DataSet ds = objbl.Bind_HSN(prefix);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                purchase.Add(new Purchase
                {
                    HSNSACCode1 = dr["HSNCode"].ToString(),
                });
            }
            return new JsonResult { Data = purchase, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Get_HSN_GST(string HSNSACCode1)
        {
            try
            {
                DataSet ds1 = new DataSet();
                DataSet ds = objbl.Bind_HSNID(HSNSACCode1, "%");
                ds1 = objbl.Bind_GSTPer(Convert.ToString(ds.Tables[0].Rows[0]["HSNCodeID"].ToString()));

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    purchase.Add(
                        new Purchase
                        {
                            gst1 = Convert.ToString(dr["GSTRate"]),
                        });
                }
            }
            catch (Exception ex)
            {

            }
            return new JsonResult { Data = purchase, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult imageuplod(Doctor model)
        {
            string path = "";
            var file = model.ImageFile;
            if (file != null)
            {
                KeystoneProject.Buisness_Logic.Pharmacy.BL_Purchase Bl_obj = new Buisness_Logic.Pharmacy.BL_Purchase();
                //var path = "";
                var filename = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                path = Server.MapPath("~/") + "MRDFiles/" + filename;
                DataSet ds = new DataSet();
                ds = Bl_obj.btnImport_Click(path);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        purchase.Add(new Models.Pharmacy.Purchase
                        {
                            productName1 = dr["ProductName"].ToString(),
                            batchNumber1 = dr["BatchNo"].ToString(),
                            expiry1 = Convert.ToDateTime(dr["Expiry"]).ToString("yyyy-MM-dd"),
                            mrp1 = dr["MRPRate"].ToString(),
                            salesRate1 = dr["SaleRate"].ToString(),
                            purchaseRate1 = dr["PurchaseRate"].ToString(),
                            quantity1 = dr["Quantity"].ToString(),
                            gst1 = dr["GST"].ToString(),
                            sgst1 = dr["SGST"].ToString(),
                            cgst1 = dr["CGST"].ToString(),
                            utgst1 = dr["UTGST"].ToString(),
                            free1 = dr["FreeQuantity"].ToString(),
                            discount1 = dr["DiscBy"].ToString(),
                            discountSymbol1 = dr["DiscountType"].ToString(),
                            lessBy1 = dr["LessBy"].ToString(),
                            lessBySymbol1 = dr["LessByType"].ToString(),
                            totalamount1 = dr["TotalAmount"].ToString(),

                            HSNSACCode1 = dr["HSNSACCode"].ToString(),
                            ProductDetailsID1 = dr["BatchNo"].ToString(),
                            ProductID1 = dr["BatchNo"].ToString(),


                        });
                    }
                }



            }
            return new JsonResult { Data = purchase, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Delete_Purchase(int ProductPurchaseID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Pharmacy.BL_Purchase objdb = new BL_Purchase();
                Purchase objSG = new Models.Pharmacy.Purchase();

                int DependaincyName = objdb.DeletePurchase(ProductPurchaseID);

                if (DependaincyName > 0)
                {
                    _Del = "Purchase Deleted Successfully !";
                }
                else
                {
                    _Del = "Purchase Can not be Deleted !";
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