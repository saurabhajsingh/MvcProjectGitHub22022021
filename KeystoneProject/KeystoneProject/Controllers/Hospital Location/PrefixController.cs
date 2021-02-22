using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Hospital_Location
{
    public class PrefixController : Controller
    {
        // GET: Prefix

            [HttpGet]
        public ActionResult Prefix()
        {
            return View();
        }

        BL_PatientOPDBill _OPDBill = new BL_PatientOPDBill();
        BL_Prefix objBL_prefix = new BL_Prefix();
      
        public JsonResult ShowPrefixMasterSetting()
        {

            BL_Prefix db = new BL_Prefix();


            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [HttpPost]
        public ActionResult Prefix(Prefix objPrefix)
        {
            try
            {
                //if (objBL_prefix.CheckPrefixMasterSetting(objPrefix.StartingUHIDNo, objPrefix.FinancialYearID))
                //{
                    objPrefix.Mode = "Edit";
                    if (objBL_prefix.Save(objPrefix))
                    {
                       
                            ModelState.Clear();
                            TempData["msg"] = "Record Update Successfully";
                            return RedirectToAction("Prefix", "Prefix");
                       

                    }
                //}
                //else
                //{
                //    TempData["msg"] = "UHID Number Already Exist's";
                //    return RedirectToAction("Prefix", "Prefix");
                //}

              
                return RedirectToAction("Prefix", "Prefix");

            }
            catch (Exception Ex)
            {
                return RedirectToAction("Prefix", "Prefix");
            }

        }

        public JsonResult ChkUHIDNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkUHIDNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "UHID Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString()+" and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkOPDNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkOPDNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "OPD Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkOPDBill(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkOPDBill(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "OPD Bill Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkIPDNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkIPDNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "IPD Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkIPDBillNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkIPDBillNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "IPD Bill Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkProvisionalBillNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkProvisionalBillNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "Provisional Bill Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkFinalBillNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkFinalBillNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "Final Bill Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkLabNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkLabNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "Lab Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkLabBillNo(int FinancialID)
        {
            string chk = "";
            BL_Prefix db = new BL_Prefix();

            DataSet ds = db.ChkLabBillNo(FinancialID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                chk = "Lab Number Stared at " + ds.Tables[0].Rows[0]["MinPrintRegNO"].ToString() + " and Last Number is " + ds.Tables[0].Rows[0]["MaxPrintRegNO"].ToString();
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ChkStartingOpdNo(int StartingOPD, int FinancialID,string Type)
        {
            string chk = "";
            if (Type == "StartingOpdNo")
            {
              
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingOpdNo(StartingOPD, FinancialID))
                {
                    chk = "OPD number is Not Use";
                }
                else
                {
                    chk = "OPD number is Use";
                }               
            }

            else if (Type == "StartingPatientOPDBillNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingPatientOPDBillNo(StartingOPD, FinancialID))
                {
                    chk = "OPD Bill Number is Not Use";
                }
                else
                {
                    chk = "OPD Bill number is Use";
                }
            }

            else if (Type == "StartingPatientIPD")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingPatientIPD(StartingOPD, FinancialID))
                {
                    chk = "IPD number is Not Use";
                }
                else
                {
                    chk = "IPD number is Use";
                }
            }

            else if (Type == "StartingIPDBillNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingIPDBillNo(StartingOPD, FinancialID))
                {
                    chk = "IPD Bill number is Not Use";
                }
                else
                {
                    chk = "IPD Bill number is Use";
                }
            }

            else if (Type == "StartingPatientProvisionalBillNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingPatientProvisionalBillNo(StartingOPD, FinancialID))
                {
                    chk = "ProvisionalBill number is Not Use";
                }
                else
                {
                    chk = "ProvisionalBill number is Use";
                }
            }

            else if (Type == "StartingFinalBillNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingFinalBillNo(StartingOPD, FinancialID))
                {
                    chk = "FinalBill number is Not Use";
                }
                else
                {
                    chk = "FinalBill number is Use";
                }
            }

            else if (Type == "StartingLabNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingLabNo(StartingOPD, FinancialID))
                {
                    chk = "Lab number is Not Use";
                }
                else
                {
                    chk = "Lab number is Use";
                }
            }

            else if (Type == "StartingLabBillNo")
            {
                BL_Prefix db = new BL_Prefix();
                if (db.ChkStartingLabBillNo(StartingOPD, FinancialID))
                {
                    chk = "LabBill number is Not Use";
                }
                else
                {
                    chk = "LabBill number is Use";
                }
            }

            return new JsonResult { Data = chk, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        
    }
}