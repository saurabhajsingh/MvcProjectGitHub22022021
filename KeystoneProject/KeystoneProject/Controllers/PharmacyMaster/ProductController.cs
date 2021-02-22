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
    public class ProductController : Controller
    {
        BL_Product bl_prod = new BL_Product();
        Product Mod_prod = new Product();
        List<Product> List_product = new List<Product>();

        // GET: /Product/
        public ActionResult Product()
        {
            return View();
        }

        public JsonResult BindTable()
        {
            BL_Product fillTable = new BL_Product();
            return new JsonResult { Data = fillTable.GetData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

        }
        public JsonResult Rebind_Data(int id)
        {
            BL_Product fill_data = new BL_Product();
            return new JsonResult { Data = fill_data.Bind_From_Table(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

        }
        public JsonResult Bind_productComp(string prefix)
        {
            BL_Product bl_productComp = new BL_Product();
            List<Product> search_productComp = new List<Product>();

            DataSet ds = bl_productComp.Bind_Prod_Company(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_productComp.Add(new Product
                {
                    productCompany = dr["ProductCompanyName"].ToString(),
                    productCompanyID = dr["ProductCompanyNameID"].ToString(),
                });
            }
            return new JsonResult { Data = search_productComp, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }
        public JsonResult Bind_Manufacturer(string prefix)
        {
           
            DataSet ds = bl_prod.Bind_Manufctr_name(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                List_product.Add(new Product
                {
                    manufacturerName = dr["ManufactureName"].ToString(),
                    manufacturerNameID = dr["ManufactureID"].ToString(),
                });
            }
            return new JsonResult { Data = List_product, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }
        public JsonResult Bind_Pack(string prefix)
        {
            BL_Product bl_packp = new BL_Product();
            List<Product> search_pack = new List<Product>();

            DataSet ds = bl_packp.Bind_Packing(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_pack.Add(new Product
                {
                    packing = dr["ProductUnitName"].ToString(),
                    packingID = dr["ProductUnitID"].ToString(),
                });
            }
            return new JsonResult { Data = search_pack, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Bind_Gen(string prefix)
        {
            BL_Product bl_gen = new BL_Product();
            List<Product> search_gen = new List<Product>();

            DataSet ds = bl_gen.Bind_Generic(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_gen.Add(new Product
                {
                    generic = dr["GenericName"].ToString(),
                    genericID = dr["GenericID"].ToString(),
                });
            }
            return new JsonResult { Data = search_gen, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Bind_Cat(string prefix)
        {
            BL_Product bl_cat = new BL_Product();
            List<Product> search_cat = new List<Product>();

            DataSet ds = bl_cat.Bind_Category(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_cat.Add(new Product
                {
                    category = dr["ProductCategoryName"].ToString(),
                    categoryID = dr["ProductCategoryID"].ToString(),
                });
            }
            return new JsonResult { Data = search_cat, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public JsonResult Bind_HSNCODE(string prefix)
        {
            BL_Product bl_hsn = new BL_Product();
            List<Product> search_hsn = new List<Product>();

            DataSet ds = bl_hsn.Bind_HSN(prefix);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                search_hsn.Add(new Product
                {
                    HSNSACCode = dr["HSNCode"].ToString(),
                });
            }
            return new JsonResult { Data = search_hsn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        [HttpPost]
        public ActionResult Product(Product Mod_prod, FormCollection fc)
        {
            try
            {
                if (bl_prod.CheckProduct(Mod_prod.productNameID, Mod_prod.productName))
                {
                    if (bl_prod.SAVE(Mod_prod))
                    {
                        if (Mod_prod.productNameID == "0" || Mod_prod.productNameID == null)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Product Saved Successfully !";
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Product Updated Successfully !";
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Product Not Saved !";
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "Product Name is Already Exists";
                    return RedirectToAction("Product", "Product");
                }
                return RedirectToAction("Product", "Product");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Product", "Product");
            }            
        }

        public JsonResult DeleteProduct(int productNameID)
        {
            string data = "";
            try
            {
                int a = bl_prod.Delete(productNameID);
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

        public JsonResult Get_HSN_GST(string HSNSACCode)
        {
            try
            {
                DataSet ds1 = new DataSet();
                DataSet ds = bl_prod.Bind_HSNID(HSNSACCode, "%");
                ds1 = bl_prod.Bind_GSTPer(Convert.ToString(ds.Tables[0].Rows[0]["HSNCodeID"].ToString()));

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    List_product.Add(
                        new Product
                        {
                            gst = Convert.ToString(dr["GSTRate"]),
                        });
                }
            }
            catch(Exception ex)
            {

            }
            return new JsonResult { Data = List_product, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

    }
}