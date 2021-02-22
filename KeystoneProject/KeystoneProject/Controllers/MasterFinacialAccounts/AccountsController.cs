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
    public class AccountsController : Controller
    {
        BL_Accounts objbl = new BL_Accounts();
        Accounts objmodel = new Accounts();

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }

        [HttpGet]
        public ActionResult Accounts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Accounts(Accounts objmodels)
        {
            objmodels.AccountsID = Request.Form["AccountsID"];
            objmodels.AccountName = Request.Form["AccountName"];
            objmodels.PrintName = Request.Form["PrintName"];
            objmodels.Schedule = Request.Form["Schedule"];
            objmodels.CreditDays = Request.Form["CreditDays"];
            //objmodel.OpeningBalance = Request.Form["OpeningBalance"];
            objmodels.OpeningBalance = Request.Form["Nature"];
            objmodels.CreditLimit = Request.Form["CreditLimit"];
            objmodels.AccountType = Request.Form["AccountType"];
            objmodels.Address = Request.Form["Address"];
            objmodels.City = Request.Form["City"];
            objmodels.PinCode = Request.Form["PinCode"];
            objmodels.State = Request.Form["State"];
            objmodels.Country = Request.Form["Country"];
            objmodels.PhoneNo = Request.Form["PhoneNo"];
            objmodels.MobileNo = Request.Form["MobileNo"];
            objmodels.EmailID = Request.Form["EmailID"];
            objmodels.Fax = Request.Form["Fax"];
            objmodels.TIN = Request.Form["TIN"];
            objmodels.Remark = Request.Form["Remark"];
            objmodels.TinDate = Request.Form["TinDate"];
            objmodels.PanNo = Request.Form["PanNo"];
            objmodels.GstNo = Request.Form["GstNo"];
            objmodels.CrAmount = Request.Form["opening_bal"];


            try
            {
                if (objbl.CheckAccounts(objmodels.AccountsID, objmodels.AccountName))
                {

                    if (objbl.Save(objmodels))
                    {
                        if (objmodels.AccountsID == "0" || objmodels.AccountsID == "")
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Accounts Saved Successfully";
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Accounts Updated Successfully";
                        }

                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Accounts Already Exist's ";
                    //TempData["msg"] = "Accounts Not Save";
                }
                return RedirectToAction("Accounts", "Accounts");

            }
            catch (Exception)
            {

                return RedirectToAction("Accounts", "Accounts");
            }


        }
        public JsonResult BindScheduleName(string prefix)
        {
            DataSet ds = objbl.BindSchedule(prefix);
            List<Accounts> searchList = new List<Accounts>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Accounts
                {
                    ScheduleID = dr["ScheduleID"].ToString(),
                    Schedule = dr["ScheduleName"].ToString(),
                });
            }
            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCityBind(string prefix)
        {

            DataSet ds = objbl.GetCity(prefix, "%");
            List<Accounts> searchList = new List<Accounts>();
            City(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Accounts
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
            List<Accounts> searchList = new List<Accounts>();

            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string City)
        {
            KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_Accounts BL_obj = new BL_Accounts();
            KeystoneProject.Models.MasterFinacialAccounts.Accounts obj = new Accounts();
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
        public JsonResult ShowAllAccounts()
        {
            BL_Accounts db = new BL_Accounts();

            return new JsonResult { Data = db.SelectAllAccounts(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DeleteAccount(int AccountsID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_Accounts objdb = new BL_Accounts();
                Accounts objSG = new Models.MasterFinacialAccounts.Accounts();

                int DependaincyName = objdb.DeleteAccounts(AccountsID);

                if (DependaincyName == 1)
                {
                    _Del = "Account Deleted Successfully";
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