using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;

namespace KeystoneProject.Controllers.Patient
{
    public class DeathCertificateController : Controller
    {
        //
        // GET: /BirthCertificate/
        BL_DeathCertificate bl_death = new BL_DeathCertificate();
        DeathCertificate certificate = new DeathCertificate();

        public ActionResult DeathCertificate(FormCollection fc)
        {
            return View();
        }
    [HttpPost]
        public ActionResult DeathCertificate(DeathCertificate cert)
        {
            try
            {

                string A = Request.Form["PatientRegNo"];
                TempData["Msg"] = "";
                if (Request.Form["PatientRegNo"] != null)
                {
                    if (cert.PatientDeathCertificateID > 0)
                    {

                        if (bl_death.Save(cert))
                        {
                            cert.PatientRegNo = "";
                            Session["PatientDeathCertificateID"] = cert.PatientDeathCertificateID;
                            return RedirectToAction("RptDeathCertificate", "DeathCertificate");
                            //TempData["Msg"] = "Update Successfully";
                            //ModelState.Clear();
                            //RedirectToAction("DeathCertificate", "DeathCertificate");
                        }


                        RedirectToAction("DeathCertificate", "DeathCertificate");
                    }

                    else
                    {
                        try
                        {
                            if (bl_death.Save(cert))
                            {
                                if (cert.PatientDeathCertificateID > 0)
                                {
                                    cert.PatientRegNo = "";
                                    Session["PatientDeathCertificateID"] = cert.PatientDeathCertificateID;
                                    return RedirectToAction("RptDeathCertificate", "DeathCertificate");
                                    //TempData["Msg"] = "Update Successfully";
                                    //ModelState.Clear();
                                    //RedirectToAction("DeathCertificate", "DeathCertificate");
                                }
                                //TempData["Msg"] = "Save Successfully";
                                Session["PatientDeathCertificateID"] = cert.PatientDeathCertificateID;
                               return RedirectToAction("RptDeathCertificate", "DeathCertificate");
                                //RedirectToAction("DeathCertificate", "DeathCertificate");
                            }
                        }

                        catch (Exception ex)
                        {
                            TempData["Msg"] = ex.Message;
                            RedirectToAction("DeathCertificate", "DeathCertificate");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("DeathCertificate", "DeathCertificate");
            }
            return RedirectToAction("DeathCertificate", "DeathCertificate");
        }

        public ActionResult RptDeathCertificate()
        {
            return View();
        }
        public ActionResult GetPatientRegNo(int PatientRegNo)
        {
            DataSet ds = bl_death.GetPatientRegNo(PatientRegNo);
            List<string> SearchList = new List<string>();

            certificate.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
            certificate.Address = ds.Tables[0].Rows[0]["Address"].ToString();
            certificate.GuardianName = ds.Tables[0].Rows[0]["GuardianName"].ToString();
            certificate.PatientRegNo = ds.Tables[0].Rows[0]["PatientRegNO"].ToString();
            SearchList.Add(certificate.PatientName);
            SearchList.Add(certificate.Address);
            SearchList.Add(certificate.GuardianName);
            SearchList.Add(certificate.PatientRegNo);
            return Json(SearchList);
        }
        public JsonResult GetPatientName(string prefix)
        {
            DataSet ds = bl_death.GetPatientName(prefix);
            List<DeathCertificate> SearchList = new List<DeathCertificate>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new DeathCertificate
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = dr["PatientRegNO"].ToString(),
                    Address = dr["Address"].ToString(),
                    GuardianName = dr["GuardianName"].ToString()
                });
            }

            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DatatableBind(string prefix)
        {
            DeathCertificate Obj_Model = new KeystoneProject.Models.Patient.DeathCertificate();
            Obj_Model.StoreAllCity = new DataSet();

            Obj_Model.StoreAllCity = bl_death.GetAllPatientDeathCertificate();
            List<DeathCertificate> serch = new List<DeathCertificate>();
            if (Obj_Model.StoreAllCity.Tables.Count != null)
            {
                foreach (DataRow dr in Obj_Model.StoreAllCity.Tables[0].Rows)
                {
                    serch.Add(new DeathCertificate
                    {
                      PatientDeathCertificateID =Convert.ToInt32( dr["PatientDeathCertificateID"].ToString()),
                      PatientRegNo = dr["PatientRegNo"].ToString(),
                      PatientName = dr["PatientName"].ToString()
                    });
                }
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Fill(int PatientRegNo)
        {
            DataSet ds = new DataSet();
            List<DeathCertificate> Search = new List<DeathCertificate>();
            ds = bl_death.GetPatientRegNo(PatientRegNo);
            DeathCertificate deathcert = new DeathCertificate();
            deathcert.PatientDeathCertificateID = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientDeathCertificateID"].ToString());
            deathcert.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
            deathcert.PatientRegNo = ds.Tables[0].Rows[0]["PatientRegNo"].ToString();
            deathcert.GuardianName = ds.Tables[0].Rows[0]["GuardianName"].ToString();
            deathcert.DateOfDeath = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateOfDeath"]).ToString("yyyy-MM-dd");
            deathcert.TimeOfDeath = Convert.ToDateTime(ds.Tables[0].Rows[0]["TimeOfDeath"]).ToString("hh:mm:ss tt");
            deathcert.DeathType = ds.Tables[0].Rows[0]["DeathType"].ToString();
            deathcert.ReasonOfDeath = ds.Tables[0].Rows[0]["ReasonOfDeath"].ToString();
            deathcert.Address = ds.Tables[0].Rows[0]["Address"].ToString();
            deathcert.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();
            deathcert.Discription = ds.Tables[0].Rows[0]["Discription"].ToString();
            deathcert.Mode = "Edit";
            Search.Add(deathcert);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       [HttpPost]
        public JsonResult DeletePatientDeathCertificate(int PatientDeathCertificateID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Patient.BL_DeathCertificate bl_deathcertificate = new Buisness_Logic.Patient.BL_DeathCertificate();
                int a = bl_deathcertificate.DeletePatientDeathCertificate(PatientDeathCertificateID);
                if (a == 1)
                {
                    data = "Death Certificate Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

       public JsonResult GetAllFinancialYear()
       {
           BL_DeathCertificate bl_death = new BL_DeathCertificate();
           DataSet ds = bl_death.GetAllFinancialYear();
           List<OPDBill> searchList = new List<OPDBill>();
           DataView dvTest = new DataView(ds.Tables[0], "FinancialYearID = " + ds.Tables[1].Rows[0]["FinancialYearID"].ToString() + " ", "", DataViewRowState.CurrentRows);
           dvTest.ToTable().Rows[0]["FinancialYear"].ToString();
           foreach (DataRow dr in dvTest.ToTable().Rows)
           {
               searchList.Add(new OPDBill
               {
                   FinancialYear = dr["FinancialYear"].ToString(),
                   FinancialYearID = Convert.ToInt32(dr["FinancialYearID"])
               });
           }
           return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
       }
	}
}