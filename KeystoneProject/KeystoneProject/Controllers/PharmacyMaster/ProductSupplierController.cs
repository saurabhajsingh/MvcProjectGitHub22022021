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
    public class ProductSupplierController : Controller
    {
        BL_ProductSupplier objbl = new BL_ProductSupplier();
        ProductSupplier objmodel = new ProductSupplier();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
        public ActionResult ProductSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductSupplier(ProductSupplier objmodels, FormCollection fc)
        {

            if (fc["ProductCompanyNameID1"] != null)
            {
                objmodels.ProductCompanyNameID1 = fc["ProductCompanyNameID1"].ToString();
                objmodels.ProductCompanyName1 = fc["ProductCompanyName1"].ToString();
                objmodels.cashDiscount1 = fc["cashDiscount1"].ToString();
                objmodels.creditDiscount1 = fc["creditDiscount1"].ToString();
                objmodels.lessBy1 = fc["lessBy1"].ToString();
            }
           
            try
            {
                if (objbl.CheckProductSupplier(objmodels.ProductSupplierID, objmodels.ProductSupplierName))
                {
                    if (objbl.Save(objmodels))
                    {
                        if (objmodels.ProductSupplierID == "0" || objmodels.ProductSupplierID == "" || objmodels.ProductSupplierID == null)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "ProductSupplier Saved Successfully";
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "ProductSupplier Saved Successfully";
                        }

                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "ProductSupplier Name is Already Exists";
                    return RedirectToAction("ProductSupplier", "ProductSupplier");
                }
                return RedirectToAction("ProductSupplier", "ProductSupplier");

            }
            catch (Exception)
            {

                return RedirectToAction("ProductSupplier", "ProductSupplier");
            }


        }
        public JsonResult GetCityBind(string prefix)
        {

            DataSet ds = objbl.GetCity(prefix, "%");
            List<ProductSupplier> searchList = new List<ProductSupplier>();
            City(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ProductSupplier
                {
                    CityID = dr["CityID"].ToString(),
                    City = dr["CityName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public void City(string prefix)
        {
            DataSet ds = objbl.GetCity(prefix, "%");
            List<ProductSupplier> searchList = new List<ProductSupplier>();

            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string City)
        {
            KeystoneProject.Buisness_Logic.PharmacyMaster.BL_ProductSupplier BL_obj = new BL_ProductSupplier();
            KeystoneProject.Models.PharmacyMaster.ProductSupplier obj = new ProductSupplier();
            List<string> searchList = new List<string>();

            DataTable td = new DataTable();
            DataSet ds = objbl.GetCity(City, "%");
            td = objbl.GetCountryStateID(Convert.ToInt16(ds.Tables[0].Rows[0]["CityID"].ToString()));

            obj.State = td.Rows[0]["StateName"].ToString();
            obj.Country = td.Rows[0]["CountryName"].ToString();
            obj.StateID = td.Rows[0]["StateID"].ToString();
            obj.CountryID = td.Rows[0]["CountryID"].ToString();


            searchList.Add(obj.State);
            searchList.Add(obj.Country);
            searchList.Add(obj.StateID);
            searchList.Add(obj.CountryID);

            return Json(searchList);
        }
        public JsonResult BindPostindAccount(string prefix)
        {
            DataSet ds = objbl.BindPostindAccount(prefix);
            List<ProductSupplier> searchList = new List<ProductSupplier>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ProductSupplier  
                {
                    AccountsID = dr["AccountsID"].ToString(),
                    AccountName = dr["AccountName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ShowAllProductSupplier()
        {
            BL_ProductSupplier db = new BL_ProductSupplier();

            return new JsonResult { Data = db.ShowAllProductSupplier(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult BindCompanyName(string prefix)
        {
            DataSet ds = objbl.BindCompanyName(prefix);
            List<ProductSupplier> searchList = new List<ProductSupplier>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new ProductSupplier
                {
                    ProductCompanyNameID = dr["ProductCompanyNameID"].ToString(),
                    ProductCompanyName = dr["ProductCompanyName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult EditProductSupplier(int id)
        {
            BL_ProductSupplier db = new BL_ProductSupplier();
            ModelState.Clear();
            return new JsonResult { Data = db.GetProductSupplier(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult DeleteProductSupplier(int ProductSupplierID)
        {
            BL_ProductSupplier db = new BL_ProductSupplier();
            string _Del = null;
            try
            {
                string DependaincyName = db.Delete(Convert.ToInt32(ProductSupplierID));
                _Del = "ProductSupplier Deleted Successfully";

            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}