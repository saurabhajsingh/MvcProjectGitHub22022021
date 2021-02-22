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
    public class MedicalAccountController : Controller
    {
        BL_MedicalAccount M_account = new BL_MedicalAccount();
        
        // GET: MedicalAccount
        public ActionResult MedicalAccount()
        {
            return View();
        }

        public JsonResult Bind_table()
        {
            BL_MedicalAccount account = new BL_MedicalAccount();
            return new JsonResult { Data = account.Getdata(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Rebind_Data(int id)
        {
            BL_MedicalAccount data_bind = new BL_MedicalAccount();
            return new JsonResult { Data = data_bind.Bind_from_table(id), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult BindScheduleName(string prefix)
        {
            DataSet ds = M_account.BindSchedule(prefix);
            List<MedicalAccount> searchList = new List<MedicalAccount>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MedicalAccount
                {
                    ScheduleID = dr["ScheduleID"].ToString(),
                    ScheduleName = dr["ScheduleName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCityBind(string prefix)
         {

            DataSet ds = M_account.GetCity(prefix, "%");
            List<MedicalAccount> searchList = new List<MedicalAccount>();
            City(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new MedicalAccount
                {
                    CityID = dr["CityID"].ToString(),
                    city = dr["CityName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void City(string prefix)
        {
            DataSet ds = M_account.GetCity(prefix, "%");
            List<MedicalAccount> searchList = new List<MedicalAccount>();

            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string City)
        {
            KeystoneProject.Buisness_Logic.PharmacyMaster.BL_MedicalAccount BL_obj = new BL_MedicalAccount();
            KeystoneProject.Models.PharmacyMaster.MedicalAccount obj = new MedicalAccount();
            List<string> searchList = new List<string>();

            DataTable td = new DataTable();
            DataSet ds = M_account.GetCity(City, "%");
            td = M_account.GetCountryStateID(Convert.ToInt16(ds.Tables[0].Rows[0]["CityID"].ToString()));

            obj.state = td.Rows[0]["StateName"].ToString();
            obj.country = td.Rows[0]["CountryName"].ToString();
            obj.StateID = td.Rows[0]["StateID"].ToString();
            obj.CountryID = td.Rows[0]["CountryID"].ToString();


            searchList.Add(obj.state);
            searchList.Add(obj.country);
            searchList.Add(obj.StateID);
            searchList.Add(obj.CountryID);

            return Json(searchList);
        }

        [HttpPost]
        public ActionResult MedicalAccount(MedicalAccount obj, FormCollection fc)
        {
            try
            {
                BL_MedicalAccount med_save = new BL_MedicalAccount();
                if (med_save.CheckAccounts(obj.AccountID, obj.AccountName))
                {
                    
                if (med_save.Save(obj))
                {
                    if (obj.AccountID > 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = " Medical Account Updated Successfully";
                        return RedirectToAction("MedicalAccount", "MedicalAccount");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = " Medical Account Saved Successfully";
                        return RedirectToAction("MedicalAccount", "MedicalAccount");
                    }
                }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Medical Accounts Already Exist ";
                    //TempData["msg"] = "Accounts Not Save";
                }
                return RedirectToAction("MedicalAccount", "MedicalAccount");
            }
            catch (Exception)
            {
                return RedirectToAction("MedicalAccount", "MedicalAccount");
            }

        }

        public JsonResult Delete_MedAccount(int AccountID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.PharmacyMaster.BL_MedicalAccount objdb = new BL_MedicalAccount();
                MedicalAccount objSG = new Models.PharmacyMaster.MedicalAccount();

                int DependaincyName = objdb.DeleteAccounts(AccountID);

                if (DependaincyName == 1)
                {
                    _Del = "Medical Account Deleted Successfully";
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
