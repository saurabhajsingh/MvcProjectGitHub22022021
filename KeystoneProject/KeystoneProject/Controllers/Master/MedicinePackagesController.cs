using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class MedicinePackagesController : Controller
    {
        BL_MedicinePackage db = new BL_MedicinePackage();
        //
        // GET: /MedicinePackages/
        public ActionResult MedicinePackage()
        {
            return View();
        }

        public JsonResult ShowAllMedicines()
        {

            BL_MedicinePackage db = new BL_MedicinePackage(); 

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [HttpGet]
        public JsonResult GetMedicinePackage(int id)
        {
            BL_MedicinePackage db = new BL_MedicinePackage(); 
            ModelState.Clear();

            return new JsonResult { Data = db.GetMedicinePackage(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult ShowAllPackages()
        {
            MedicinePackage location = new Models.Master.MedicinePackage();
            BL_MedicinePackage BLMedPac = new BL_MedicinePackage();
            List<MedicinePackage> serch = new List<MedicinePackage>();
            DataSet ds = BLMedPac.ShowAllPackages();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new MedicinePackage
                {
                    Package = dr["PackagesName"].ToString(),

                    PackagesID = Convert.ToInt32(dr["PackagesID"].ToString())

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult MedicinePackage(MedicinePackage location, FormCollection collection)
        {
            try
            {

                location.Medicines = Request.Form["MedicineName"].ToString();
                location.MedicinesID = Request.Form["MedicineID"].ToString();

                string pack = Request.Form["PackagesID"].ToString();

                if (pack.EndsWith(","))
                {
                    pack = pack.Substring(0, pack.Length - 1);
                }

                if (pack == "")
                    location.PackagesID = 0;
                else
                location.PackagesID = Convert.ToInt32(pack);

                BL_MedicinePackage BLMedPackages = new BL_MedicinePackage();
                //HospitalLocation Location = new HospitalLocation();
                //collection
                //return View();


               
                if (BLMedPackages.CheckMedicinePackages(location.PackagesID, location.Package))
                {
                    if (location.PackagesID > 0)
                    {
                        if (BLMedPackages.Edit(location))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Package Updated Successfully !";
                        }
                    }
                    else
                    {
                        if (BLMedPackages.Save(location))
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Package Saved Successfully !";
                        }
                    }
                }
                else
                {
                    TempData["msg"] = "Medicine Package Already Exists !";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return RedirectToAction("MedicinePackage", "MedicinePackages");
            }

            return RedirectToAction("MedicinePackage", "MedicinePackages");
        }

        public JsonResult DeleteMedicinePackage(int PackagesID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = db.Delete(Convert.ToInt32(PackagesID));
                _Del = "Medicine Package Deleted Successfully !";

            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}