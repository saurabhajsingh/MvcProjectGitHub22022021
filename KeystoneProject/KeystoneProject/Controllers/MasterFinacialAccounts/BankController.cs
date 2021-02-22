using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.MasterFinacialAccounts;
using KeystoneProject.Models.MasterFinacialAccounts;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class BankController : Controller
    {
        BL_Bank objbl = new BL_Bank();
        Bank objmodel = new Bank();

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
        public ActionResult Bank()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Bank(Bank bank,FormCollection fc)
        {



            if (bank.BankID == null)
            {
                bank.BankID = "";
            }
            else
            {
                bank.BankID = fc["BankID"].ToString();
            }

            if (bank.BankName != "" || bank.BankName != null)
            {
                bank.BankName = fc["BankName"].ToString();
            }
            else
            {
                bank.BankName = "";
            }

            if (bank.Address != "" || bank.Address != null)
            {
                bank.Address = fc["Address"].ToString();
            }
            else
            {
                bank.Address = "";
            }
            if (bank.CityID == null)
            {
                bank.CityID = "";
            }
            else
            {
                bank.CityID = fc["CityID"].ToString();
            }
            if (bank.StateID == null)
            {
                bank.StateID = "";
            }
            else
            {
                bank.StateID = fc["StateID"].ToString();
            }

            if (bank.CountryID == null)
            {
                bank.CountryID = "";
            }
            else
            {
                bank.CountryID = fc["CountryID"].ToString();
            }



            if (bank.PhoneNo != "" || bank.PhoneNo != null)
            {
                bank.PhoneNo = fc["PhoneNo"].ToString();
            }
            else
            {
                bank.PhoneNo = "";
            }
            if (bank.MobileNo != "" || bank.MobileNo != null)
            {
                bank.MobileNo = fc["MobileNo"].ToString();
            }
            else
            {
                bank.MobileNo = "";
            }
            if (bank.Fax != "" || bank.Fax != null)
            {
                bank.Fax = fc["Fax"].ToString();
            }
            else
            {
                bank.Fax = "";
            }
            if (bank.EmailID != "" || bank.EmailID != null)
            {
                bank.EmailID = fc["EmailID"].ToString();
            }
            else
            {
                bank.EmailID = "";
            }
            if (bank.Pincode != "" || bank.Pincode != null)
            {
                bank.Pincode = fc["Pincode"].ToString();
            }
            else
            {
                bank.Pincode = "";
            }

            bank.AccountName = fc["AccountName"].ToString();
            bank.AccountNumber = fc["AccountNumber"].ToString();
            try
            {

                //if (objbl.CheckBank(bank.BankID, bank.BankName))
                //{

                    if (objbl.Save(bank))
                 {
                    if (Convert.ToInt32(bank.BankID) > 0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Bank Updated Successfully";
                        return RedirectToAction("Bank", "Bank");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Bank Saved Successfully";
                        return RedirectToAction("Bank", "Bank");
                    }
                       
                  }
                    else
                    {
                        TempData["msg"] = "Bank Not Save";     
                    }
                    return RedirectToAction("Bank", "Bank");
                //}
                //else
                //{
                //    TempData["msg"] = "Bank Name is Already Exists";
                //    return RedirectToAction("Bank", "Bank");
                //}
                //return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
      
        public JsonResult GetCityBind(string prefix)
        {

            DataSet ds = objbl.GetCity(prefix, "%");
            List<Bank> searchList = new List<Bank>();
            City(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchList.Add(new Bank
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
            List<Bank> searchList = new List<Bank>();

            if (Convert.ToInt16(ds.Tables[0].Rows.Count) == 1)
            {

                Session["CityID"] = ds.Tables[0].Rows[0]["CityID"].ToString();
            }
        }
        public ActionResult AjaxMethod(string City)
        {
            KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_Bank BL_obj = new BL_Bank();
            KeystoneProject.Models.MasterFinacialAccounts.Bank obj = new Bank();
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
        public JsonResult ShowAllBank()
        {
            BL_Bank db = new BL_Bank();

            return new JsonResult { Data = db.SelectAllBank(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EditBank(int BankID)
        {

            KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_Bank objDBService = new BL_Bank();
            Bank ServiceModl = new Bank();
            List<Bank> SearchList = new List<Bank>();


             DataSet ds = objDBService.SelectAllServicesbyID(BankID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ServiceModl.BankID = ds.Tables[0].Rows[0]["BankID"].ToString();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SearchList.Add(new Bank
                    {
                        BankID = dr["BankID"].ToString(),
                        BankAccountID = dr["BankAccountID"].ToString(),
                        AccountName = dr["AccountName"].ToString(),
                        AccountNumber = dr["BankAccountNo"].ToString(),
                        Address = dr["Address"].ToString(),
                        CityID = dr["CityID"].ToString(),
                        StateID = dr["StateID"].ToString(),
                        CountryID = dr["CountryID"].ToString(),
                        PhoneNo = dr["PhoneNO"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        Fax = dr["Fax"].ToString(),
                        EmailID = dr["EmailID"].ToString(),
                        Pincode = dr["PinCode"].ToString(),
                        ReferenceCode = dr["ReferenceCode"].ToString(),
                        City = dr["CityName"].ToString(),
                        State = dr["StateName"].ToString(),
                        Country = dr["CountryName"].ToString(),
                        BankName = dr["BankName"].ToString(),



                    });
                }
            }
            
           
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        
        }
        public JsonResult DeleteBank(int BankID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.MasterFinacialAccounts.BL_Bank objdb = new BL_Bank();
                Accounts objSG = new Models.MasterFinacialAccounts.Accounts();

                int DependaincyName = objdb.DeleteBank(BankID);

                if (DependaincyName == 1)
                {
                    _Del = "Bank Deleted Successfully";
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