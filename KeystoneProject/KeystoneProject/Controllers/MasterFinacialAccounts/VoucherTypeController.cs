using KeystoneProject.Buisness_Logic.MasterFinacialAccounts;
using KeystoneProject.Models.MasterFinacialAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.MasterFinacialAccounts
{
    public class VoucherTypeController : Controller
    {
        //
        // GET: /VoucherType/

        BL_VoucherType obj_vouchertype = new BL_VoucherType();
        public ActionResult VoucherType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VoucherType(VoucherType obj)
        {
            try
            {
                if (obj_vouchertype.Save(obj))
                {
                    if(obj.VoucherTypeID>0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = " VoucherType Updated Successfully";
                        return RedirectToAction("VoucherType", "VoucherType");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = " VoucherType Saved Successfully";
                        return RedirectToAction("VoucherType", "VoucherType");
                    }
                    
                }
                else
                {
                    TempData["msg"] = "VoucherType Not Save";
                }
                return RedirectToAction("VoucherType", "VoucherType");

            }
            catch (Exception)
            {
                return RedirectToAction("VoucherType", "VoucherType");
            }
            
        }

        public JsonResult ShowAllVoucherType()
        {
            BL_VoucherType db = new BL_VoucherType();

            return new JsonResult { Data = db.SelectAllVoucherType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAccountID(string prefix)
        {
            KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_VoucherType Bl_obj = new BL_VoucherType();
            VoucherType AddServiceMod = new VoucherType();
            DataSet ds = Bl_obj.GetAccountID(prefix, "%");
            List<VoucherType> SearchList = new List<VoucherType>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new VoucherType
                {
                    AccountName = dr["AccountName"].ToString(),
                    MasterAcID = dr["AccountsID"].ToString(),
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DeleteVoucherType(int VoucherTypeID)
        {

            string val = "";

            BL_VoucherType db = new BL_VoucherType();
            if (db.DeleteVoucherType(VoucherTypeID))
            {
                val = "DeliveryType Deleted Successfully";
            }

            return Json(val);
          
        }
	}
}


