using KeystoneProject.Buisness_Logic;
using KeystoneProject.Buisness_Logic.Pharmacy;
using KeystoneProject.Models.Pharmacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Pharmacy
{
    public class PurchaseOrderController : Controller
    {
        PurchaseOrder mod_PO = new PurchaseOrder();
        BL_PurchaseOrder bl_PO = new BL_PurchaseOrder();
        List<PurchaseOrder> list_po = new List<PurchaseOrder>();

        // GET: /PurchaseOrder/
        public ActionResult PurchaseOrder()
        {
            return View();
        }

        public JsonResult Get_supplier(string prefix)
        {
            DataSet ds = bl_PO.Bind_supplier(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_po.Add(
                    new PurchaseOrder
                    {
                        supplierName = Convert.ToString(dr["ProductSupplierName"]),
                        supplierNameID = Convert.ToString(dr["ProductSupplierID"]),

                    });
            }
            return new JsonResult { Data = list_po, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Get_Product(string prefix)
        {
            DataSet ds = bl_PO.Bind_productdetail(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_po.Add(
                    new PurchaseOrder
                    {
                        productName1 = Convert.ToString(dr["ProductName"]),
                        productNameID1 = Convert.ToString(dr["ProductID"]),
                    });
            }
            return new JsonResult { Data = list_po, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Get_Manufacturer(string prefix)
        {
            DataSet ds = bl_PO.Bind_Manufacturer(prefix);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list_po.Add(
                    new PurchaseOrder
                    {
                        manufacturerName1 = Convert.ToString(dr["ManufactureName"]),
                        //productNameID1 = Convert.ToString(dr["ProductID"]),
                    });
            }
            return new JsonResult { Data = list_po, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public ActionResult AjaxMethod_SUPP(string supplierName)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<PurchaseOrder> searchList = new List<PurchaseOrder>();

            DataTable td = new DataTable();
            DataSet ds = BL_obj.Bind_supplier(supplierName, "%");
            td = BL_obj.Getaddress(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductSupplierID"].ToString()));

            foreach (DataRow dr in td.Rows)
            {
                searchList.Add(
                    new PurchaseOrder
                    {
                        address = Convert.ToString(dr["Address"]),
                    });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult AjaxMethod_PackQtyRate(string productNameID1)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<string> searchList = new List<string>();

            try
            {
                DataTable dt = new DataTable();
                DataSet ds = BL_obj.Bind_Newproductdetail("%", productNameID1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = BL_obj.Bind_PackQtyRate(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductID"].ToString()));
                    obj.manufacturerName1 = dt.Rows[0]["MfrName"].ToString();
                    obj.packing1 = dt.Rows[0]["Packing"].ToString();
                    obj.maxLevel1 = dt.Rows[0]["MaxQtyLevel"].ToString();
                    obj.minLevel1 = dt.Rows[0]["MinQtyLevel"].ToString();
                    obj.productNameID1 = dt.Rows[0]["ProductID"].ToString();

                    searchList.Add(obj.manufacturerName1);
                    searchList.Add(obj.packing1);
                    searchList.Add(obj.maxLevel1);
                    searchList.Add(obj.minLevel1);
                    searchList.Add(obj.productNameID1);
                }
                return Json(searchList);
            }
            catch (Exception ex)
            {
                return Json(searchList);
            }
        }

        [HttpPost]
        public ActionResult PurchaseOrder(PurchaseOrder mod_PO, FormCollection fc)
        {
            try
            {
                //if (mod_PO.purchaseType != "Select")
                //{
                    if (fc["supplierNameID"] != null)
                    {
                        mod_PO.productNameID1 = fc["productNameID1"].ToString();
                        mod_PO.productName1 = fc["productName1"].ToString();
                        mod_PO.manufacturerName1 = fc["manufacturerName1"].ToString();
                        mod_PO.packing1 = fc["packing1"].ToString();
                        mod_PO.currentQuantity1 = fc["currentQuantity1"].ToString();
                        mod_PO.maxLevel1 = fc["maxLevel1"].ToString();
                        mod_PO.minLevel1 = fc["minLevel1"].ToString();
                        mod_PO.quantity1 = fc["quantity1"].ToString();
                        mod_PO.freeQuantity1 = fc["freeQuantity1"].ToString();
                        mod_PO.rate1 = fc["rate1"].ToString();
                        mod_PO.totalAmount1 = fc["totalAmount1"].ToString();

                    }
                    
                    int id = mod_PO.PoId;

                    if (bl_PO.SAVE(mod_PO))
                    {
                        if (id == 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Purchase Order Saved Successfully !";
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Purchase Order Updated Successfully !";
                        }

                    }
                    return RedirectToAction("PurchaseOrder", "PurchaseOrder");
            //}
            //    else
            //    {
            //    TempData["msg"] = "Kindly Select Purchase Type !";
            //        return View();
            //}
        }
            catch (Exception ex)
            {
                return RedirectToAction("PurchaseOrder", "PurchaseOrder");
            }

        }

        public ActionResult AjaxMethod_OldBill(string supplierName)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<PurchaseOrder> searchList = new List<PurchaseOrder>();

            DataTable dt = new DataTable();
            DataSet ds = BL_obj.Bind_supplier(supplierName, "%");
            dt = BL_obj.GetOldBill(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductSupplierID"].ToString()));

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PurchaseOrder
                    {
                        oldBillNumber = Convert.ToString(dr["BillNo&Date"]),
                    });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult Rebind_BillDetail(int id)
        {

            return new JsonResult { Data = bl_PO.Bind_from_BillNo(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Rebind_BillDetailProduct(int id)
        {

            return new JsonResult { Data = bl_PO.Bind_from_BillNoProduct(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public ActionResult AjaxMethod_CheckProduct(string productNameID1, FormCollection fc)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<string> searchList = new List<string>();

            try
            {
                DataTable dt = new DataTable();
                DataSet ds = BL_obj.Bind_Newproductdetail("%", productNameID1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = BL_obj.Bind_PackQtyRate(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductID"].ToString()));
                    obj.manufacturerName1 = dt.Rows[0]["MfrName"].ToString();
                    obj.packing1 = dt.Rows[0]["Packing"].ToString();
                    obj.minLevel1 = dt.Rows[0]["MinQtyLevel"].ToString();
                    obj.maxLevel1 = dt.Rows[0]["MaxQtyLevel"].ToString();
                    obj.productNameID1 = dt.Rows[0]["ProductID"].ToString();

                    searchList.Add(obj.manufacturerName1);
                    searchList.Add(obj.packing1);
                    searchList.Add(obj.minLevel1);
                    searchList.Add(obj.maxLevel1);
                    searchList.Add(obj.productNameID1);
                }                
                return Json(searchList);
            }
            catch (Exception ex)
            {
                return Json(searchList);
            }
        }

        public ActionResult AjaxMethod_AddProduct(string productName1, FormCollection fc)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<string> searchList = new List<string>();

            try
            {
                DataTable dt = new DataTable();
                DataSet ds = BL_obj.Check_newProduct(productName1, "%");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dt = BL_obj.Bind_PackQtyRate(Convert.ToInt16(ds.Tables[0].Rows[0]["ProductID"].ToString()));
                    //obj.manufacturerName1 = dt.Rows[0]["MfrName"].ToString();
                    //obj.packing1 = dt.Rows[0]["Packing"].ToString();
                    //obj.minLevel1 = dt.Rows[0]["MinQtyLevel"].ToString();
                    //obj.maxLevel1 = dt.Rows[0]["MaxQtyLevel"].ToString();
                    //obj.productNameID1 = dt.Rows[0]["ProductID"].ToString();

                    //searchList.Add(obj.manufacturerName1);
                    //searchList.Add(obj.packing1);
                    //searchList.Add(obj.minLevel1);
                    //searchList.Add(obj.maxLevel1);
                    //searchList.Add(obj.productNameID1);
                }
                else
                {
                    dt = BL_obj.Add_newProduct(productName1);
                    obj.productNameID1 = dt.Rows[0]["ProductID"].ToString();

                    searchList.Add(obj.productNameID1);

                }
                return Json(searchList);
            }
            catch (Exception ex)
            {
                return Json(searchList);
            }
        }

        public ActionResult AjaxMethod_PurType(string purchaseType)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder BL_obj = new BL_PurchaseOrder();
            KeystoneProject.Models.Pharmacy.PurchaseOrder obj = new PurchaseOrder();
            List<PurchaseOrder> searchList = new List<PurchaseOrder>();

            DataTable td = new DataTable();
            DataSet ds = BL_obj.Bind_PurTypeMode(purchaseType);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new PurchaseOrder
                    {
                        purchaseTypeMode = Convert.ToString(dr["AccountName"]),
                    });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult Delete_PO(int PoId)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Pharmacy.BL_PurchaseOrder objdb = new BL_PurchaseOrder();
                PurchaseOrder objSG = new Models.Pharmacy.PurchaseOrder();

                int DependaincyName = objdb.DeletePurchaseOrder(PoId);

                if (DependaincyName > 0)
                {
                    _Del = "Purchase Order Deleted Successfully !";
                }
                else
                {
                    _Del = "Purchase Order Can not be Deleted !";
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