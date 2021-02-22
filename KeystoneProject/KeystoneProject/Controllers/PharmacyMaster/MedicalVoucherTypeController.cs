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
    public class MedicalVoucherTypeController : Controller
    {
        BL_MedicalVoucherType Med_vouchertype = new BL_MedicalVoucherType();
        // GET: /MedicalVoucherType/
        public ActionResult MedicalVoucherType()
        {
            return View();
        }
        public JsonResult Bind_Table()
        {
            BL_MedicalVoucherType Med_voucher = new BL_MedicalVoucherType();
            return new JsonResult { Data = Med_voucher.GetData(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Rebind_Data(int id)
        {
            BL_MedicalVoucherType data_bind = new BL_MedicalVoucherType();
            return new JsonResult { Data = data_bind.Bind_from_table(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        [HttpPost]
        
        public ActionResult MedicalVoucherType(MedicalVoucherType obj)
        {
            try
            {
                if (obj.debitMasterAccount == "true")
                {
                    obj.debitMasterAccount = Convert.ToString(true);
                }

                else if (obj.debitMasterAccount == "null")
                {
                    obj.debitMasterAccount = Convert.ToString(false);
                }
                else
                {
                    obj.debitMasterAccount = Convert.ToString(false);
                }

                if (obj.editMasterAccount == "true")
                {
                    obj.editMasterAccount = Convert.ToString(true);
                }

                else if (obj.editMasterAccount == null)
                {
                    obj.editMasterAccount = Convert.ToString(false);
                }
                else
                {
                    obj.editMasterAccount = Convert.ToString(false);
                }
                if (Med_vouchertype.CheckVoucher(obj.VoucherTypeID, obj.voucherTypeName))
                {
                    if (Med_vouchertype.SAVE(obj))
                {
                    if (obj.VoucherTypeID > 0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Voucher Type Updated Successfully";
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Voucher Type Saved Successfully";
                    }

                }
                else
                {
                    TempData["msg"] = "Medical Voucher Type Not Saved";
                }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Medical Voucher Type Already Exist ";
                    
                }
                
                return RedirectToAction("MedicalVoucherType", "MedicalVoucherType");

            }
            catch (Exception ex)
            {
                return RedirectToAction("MedicalVoucherType", "MedicalVoucherType");
            }

        }

        public JsonResult GetAccountID(string prefix)
        {
            KeystoneProject.Buisness_Logic.PharmacyMaster.BL_MedicalVoucherType Bl_obj = new BL_MedicalVoucherType();
            KeystoneProject.Models.PharmacyMaster.MedicalVoucherType AddServiceMod = new MedicalVoucherType();
            DataSet ds = Bl_obj.GetAccountID(prefix, "%");
            List<MedicalVoucherType> SearchList = new List<MedicalVoucherType>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new MedicalVoucherType
                {
                    masterAccount = dr["AccountName"].ToString(),
                    MasterAcID = Convert.ToInt32(dr["AccountsID"]),
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteVoucherType(int VoucherTypeID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.PharmacyMaster.BL_MedicalVoucherType bl_voucher = new BL_MedicalVoucherType();
                int a = bl_voucher.DeleteVoucher(VoucherTypeID);
                if (a == 1)
                {
                    data = "Voucher Type Deleted Successfully";
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