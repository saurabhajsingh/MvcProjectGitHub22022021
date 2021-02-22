using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Pharmacy;
using KeystoneProject.Buisness_Logic.Pharmacy;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KeystoneProject.Controllers.Pharmacy
{
    public class StockStatusController : Controller
    {
        BL_StockStatus bl_stock = new BL_StockStatus();
        public ActionResult StockStatus()
        {
            return View();
        }

        public JsonResult BindProductName(string prefix)
        {
            DataSet ds = bl_stock.BindProduct(prefix);
            List<StockStatus> searchList = new List<StockStatus>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(
                    new StockStatus
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        productName = dr["ProductName"].ToString()
                    });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };
        }

        public ActionResult AjaxMethod_BatchName(string productNM)
        {
            List<StockStatus> searchList = new List<StockStatus>();

            DataSet ds = bl_stock.BindProduct(productNM);
            DataTable dt = new DataTable();

            dt = bl_stock.Bindbatch(Convert.ToInt32(ds.Tables[0].Rows[0]["ProductID"]));

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new StockStatus
                    {
                        productBatch = Convert.ToString(dr["BatchNo"]),
                    });
            }
            return new JsonResult
            {
                Data = searchList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult View_stock(string chk_allProduct, string chk_allBatch, string product, string batch, string productID)
        {
            try
            {
                if (chk_allProduct == "True")
                {
                    return new JsonResult { Data = bl_stock.Getdata(), JsonRequestBehavior = new JsonRequestBehavior() };
                }
                else if (product != "")
                {
                    if (chk_allBatch == "True")
                    {
                        return new JsonResult { Data = bl_stock.GetStockStatusData(productID, "%"), JsonRequestBehavior = new JsonRequestBehavior() };
                    }
                    else
                    {
                        if (batch != "")
                        {
                            return new JsonResult { Data = bl_stock.GetStockStatusData(productID, batch), JsonRequestBehavior = new JsonRequestBehavior() };
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Enter Batch No. / Model No. First !";                                             
                        }
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "Enter Product Name !";
                }
            }
            catch (Exception ex)
            {

            }
            return new JsonResult { Data = bl_stock.Getdata(), JsonRequestBehavior = new JsonRequestBehavior() };
        }
    }
}