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
    public class SalePurchaseModifyController : Controller
    {
        BL_SalePurchaseModify BL_obj = new BL_SalePurchaseModify();
        SalePurchaseModify obj = new SalePurchaseModify();

        public ActionResult SalePurchaseModify()
        {
            return View();
        }

        public ActionResult BindProductName(string Name)
        {
            DataSet ds = BL_obj.BindProductName(Name);
            List<SalePurchaseModify> obj = new List<Models.Pharmacy.SalePurchaseModify>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.SalePurchaseModify
                {
                    ProductID = Convert.ToInt32(dr["ProductID"]),
                    ProductName = dr["ProductName"].ToString(),
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

            //  return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult AjaxMethod_batchno(string prodnm)
        {
            List<SalePurchaseModify> obj1 = new List<Models.Pharmacy.SalePurchaseModify>();

            DataTable dt = new DataTable();
            DataSet ds1 = BL_obj.BindProductName(prodnm);

            int ProductID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ProductID"].ToString());

            DataSet ds = BL_obj.GetBatchNo(ProductID);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               
                obj1.Add(new Models.Pharmacy.SalePurchaseModify
                {
                   
                    batchNumber = dr["BatchNo"].ToString(),
                   
                });
            }
            return new JsonResult { Data = obj1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
                
        public JsonResult FillBillDetails(string batch, string prodID, string type, DateTime fromdt, DateTime todt)
        {            
            return new JsonResult { Data = BL_obj.FillBillDetails(batch, prodID, type, Convert.ToDateTime(fromdt), Convert.ToDateTime(todt)), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ModifyBillDetails(string prodID, string ProdNm, string batchno, string billno, string billDt, int purRate, int QTY, int totAmt, string type)
        {
            return new JsonResult { Data = BL_obj.ModifyBillDetails(prodID, ProdNm, batchno, billno, billDt, purRate, QTY, totAmt, type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteBillDetails(string prodID, string ProdNm, string batchno, string billno, string billDt, int purRate, int QTY, int totAmt, string type)
        {
            return new JsonResult { Data = BL_obj.DeleteBillDetails(prodID, ProdNm, batchno, billno, billDt, purRate, QTY, totAmt, type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteAllBillDetails(string batch, string prodID, string type, DateTime fromdt, DateTime todt)
        {
            return new JsonResult { Data = BL_obj.DeleteAllBillDetails(batch, prodID, type, Convert.ToDateTime(fromdt), Convert.ToDateTime(todt)), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}