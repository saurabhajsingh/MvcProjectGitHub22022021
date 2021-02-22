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
    public class ProductCompanyController : Controller
    {

        BL_ProductCompany bl_pComp = new BL_ProductCompany();
        ProductCompany mod_pComp = new ProductCompany();
        List<ProductCompany> P_company = new List<ProductCompany>();

        // GET: /ProductCompany/
        public ActionResult ProductCompany()
        {
            return View();
        }

        public JsonResult Bind_Table()
        {
            return new JsonResult { Data = bl_pComp.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Bind_sales(string prefix)
        {
            DataSet ds = bl_pComp.Bind_SaleTax(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                P_company.Add(new ProductCompany
                {
                    salesTax = dr["Discription"].ToString(),
                    salesTaxID = dr["TaxTypeInformationID"].ToString(),
                });
            }
            return new JsonResult { Data = P_company, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Bind_Purchase(string prefix)
        {
            DataSet ds = bl_pComp.Bind_PurchaseTax(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                P_company.Add(new ProductCompany
                {
                    purchaseTax = dr["Discription"].ToString(),
                    purchaseTaxID = dr["TaxTypeInformationID"].ToString(),
                });
            }
            return new JsonResult { Data = P_company, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetCityBind(string prefix)
        {

            DataSet ds = bl_pComp.BindCity(prefix, "%");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                P_company.Add(new ProductCompany
                {
                    cityID = dr["CityID"].ToString(),
                    city = dr["CityName"].ToString(),
                });
            }
            return new JsonResult { Data = P_company, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void City(string prefix)
        {
            DataSet ds = bl_pComp.BindCity(prefix, "%");
            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string city)
        {
            KeystoneProject.Buisness_Logic.PharmacyMaster.BL_ProductCompany BL_obj = new BL_ProductCompany();
            KeystoneProject.Models.PharmacyMaster.ProductCompany obj = new ProductCompany();
            List<string> searchList = new List<string>();

            DataTable td = new DataTable();
            DataSet ds = bl_pComp.BindCity(city, "%");
            td = bl_pComp.GetCountryStateID(Convert.ToInt16(ds.Tables[0].Rows[0]["CityID"].ToString()));

            obj.state = td.Rows[0]["StateName"].ToString();
            obj.country = td.Rows[0]["CountryName"].ToString();
            obj.stateID = td.Rows[0]["StateID"].ToString();
            obj.countryID = td.Rows[0]["CountryID"].ToString();


            searchList.Add(obj.state);
            searchList.Add(obj.country);
            searchList.Add(obj.stateID);
            searchList.Add(obj.countryID);

            return Json(searchList);
        }

        public JsonResult Rebind_Data(int id)
        {
            BL_ProductCompany data_bind = new BL_ProductCompany();
            return new JsonResult { Data = data_bind.Bind_from_table(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        [HttpPost]
        public ActionResult ProductCompany(ProductCompany obj, FormCollection fc)
        {
            try
            {
                obj.radio = fc["radio"].ToString();
                obj.mobileNumber = fc["mobileNumber"].ToString();
                obj.phoneNumber = fc["phoneNumber"].ToString();
                BL_ProductCompany save_list = new BL_ProductCompany();
                if (save_list.Check_productCompany(obj.nameID, obj.name))
                {
                    if (save_list.SAVE(obj))
                    {
                        if (obj.nameID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Product Company Name Updated Successfully !";
                            return RedirectToAction("ProductCompany", "ProductCompany");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Product Company Name Saved Successfully !";
                            return RedirectToAction("ProductCompany", "ProductCompany");
                        }
                    }
                   
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "Product Company Name is Already Exists";
                    return RedirectToAction("ProductCompany", "ProductCompany");
                }
                return RedirectToAction("ProductCompany", "ProductCompany");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ProductCompany", "ProductCompany");
            }
        }

        public JsonResult Delete_Product(int nameID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.PharmacyMaster.BL_ProductCompany objdb = new BL_ProductCompany();
                int DependaincyName = objdb.Delete_pcomp(nameID);

                if (DependaincyName == 1)
                {
                    _Del = "Product Company Name Deleted Successfully";
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