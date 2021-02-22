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
    public class ProductCategoryController : Controller
    {
        BL_ProductCategory Product_cat = new BL_ProductCategory();
        // GET: /ProductCategory/

            
        public ActionResult ProductCategory()
        {
            return View();
        }

        public JsonResult Bind_Table()
        {
            BL_ProductCategory fillTable = new BL_ProductCategory();
            return new JsonResult { Data = fillTable.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };

        }

        public JsonResult Rebind_Data(int id)
        {
            BL_ProductCategory data_bind = new BL_ProductCategory();
            return new JsonResult { Data = data_bind.Bind_from_table(id), JsonRequestBehavior = new JsonRequestBehavior() };

        }

        public JsonResult Bind_sales(string prefix)
        {
            BL_ProductCategory bl_sale = new BL_ProductCategory();
            ProductCategory mod_sale = new ProductCategory();
            List<ProductCategory> search_sale = new List<ProductCategory>();

            DataSet ds = bl_sale.Bind_SaleTax(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_sale.Add(new ProductCategory
                {
                    salesTax = dr["Discription"].ToString(),
                    saleTaxID = dr["TaxTypeInformationID"].ToString(),
                });
            }
            return new JsonResult { Data = search_sale, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Bind_Purchase(string prefix)
        {
            BL_ProductCategory bl_purchase = new BL_ProductCategory();
            ProductCategory mod_purchase = new ProductCategory();
            List<ProductCategory> search_purchase = new List<ProductCategory>();

            DataSet ds = bl_purchase.Bind_PurchaseTax(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_purchase.Add(new ProductCategory
                {
                    purchaseTax = dr["Discription"].ToString(),
                    purchaseTaxID = dr["TaxTypeInformationID"].ToString(),
                });
            }
            return new JsonResult { Data = search_purchase, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult ProductCategory(ProductCategory obj, FormCollection fc)
        {
            try
            {
                if (Product_cat.Check_productCat(obj.categoryID,obj.categoryName))
                {
                    if (Product_cat.SAVE(obj))
                    {
                        if (obj.categoryID > 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Product Category Updated Successfully";
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Product Category Saved Successfully";
                        }

                    }
                    else
                    {
                        TempData["msg"] = "Product Category Not Saved";
                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Product Category Already Exist ";

                }
                return RedirectToAction("ProductCategory", "ProductCategory");
            }
            catch (Exception ex)
            {

            }
        
            return RedirectToAction("ProductCategory", "ProductCategory");

        }

        public JsonResult Delete_ProductCat(int categoryID)
        {
            string data = "";
            try
            {
                
                int a = Product_cat.Delete_cat(categoryID);
                if (a == 1)
                {
                    data = "Product Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);


        }
    }


}
