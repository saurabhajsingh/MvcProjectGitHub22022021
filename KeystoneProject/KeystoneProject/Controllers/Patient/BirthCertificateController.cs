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
    public class BirthCertificateController : Controller
    {
        //
        // GET: /BirthCertificate/

        BL_BirthCertificate bl_birth = new BL_BirthCertificate();
        BirthCertificate certificate = new BirthCertificate();
        public ActionResult BirthCertificate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult BirthCertificate(BirthCertificate cert)
         {
            try
            {

                string A = Request.Form["PatientName"];
                TempData["Msg"] = "";
                if (Request.Form["PatientName"] != null)
                {
                    if (cert.CertificateNo > 0)
                    {
                        if (bl_birth.Save(cert))
                        {
                            cert.PatientName = "";
                            Session["CertificateNo"] = cert.CertificateNo;
                            return RedirectToAction("RptBirthCertificate", "BirthCertificate");
                            //TempData["Msg"] = "Update Successfully";
                            //ModelState.Clear();
                            //RedirectToAction("BirthCertificate", "BirthCertificate");
                        }
                        RedirectToAction("BirthCertificate", "BirthCertificate");
                    }
                    else
                    {
                        try
                        {
                            if (bl_birth.Save(cert))
                            {
                                if (cert.CertificateNo > 0)
                                {
                                    cert.PatientName = "";
                                    Session["CertificateNo"] = cert.CertificateNo;
                                    return RedirectToAction("RptBirthCertificate", "BirthCertificate");
                                    //TempData["Msg"] = "Update Successfully";
                                    //ModelState.Clear();
                                    //RedirectToAction("BirthCertificate", "BirthCertificate");
                                }

                                Session["CertificateNo"] = cert.CertificateNo;
                                return RedirectToAction("RptBirthCertificate", "BirthCertificate");
                                //TempData["Msg"] = "Save Successfully";
                                //RedirectToAction("BirthCertificate", "BirthCertificate");
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["Msg"] = ex.Message;
                            RedirectToAction("BirthCertificate", "BirthCertificate");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("BirthCertificate", "BirthCertificate");
            }
            return RedirectToAction("BirthCertificate", "BirthCertificate");
        }

        public ActionResult RptBirthCertificate()
        {
            return View();
        }
        public JsonResult GetAllBirthCert(string prefix)
        {
            BL_BirthCertificate db = new BL_BirthCertificate();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
        }

        public ActionResult GetPatientRegNo(int PatientRegNo)
        {
            DataSet ds = bl_birth.GetPatientRegNo(PatientRegNo);
            List<string> SearchList = new List<string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                certificate.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
                certificate.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                SearchList.Add(certificate.PatientName);
                SearchList.Add(certificate.Address);
            }
            
            return Json(SearchList);
        }
        public JsonResult GetPatientName(string prefix)
        {
            DataSet ds = bl_birth.GetPatientName(prefix);
            List<BirthCertificate> SearchList = new List<BirthCertificate>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchList.Add(new BirthCertificate
                {
                    PatientName = dr["PatientName"].ToString(),
                    PatientRegNo = Convert.ToInt32( dr["PatientRegNO"].ToString()), 
                    Address = dr["Address"].ToString(),
                    printReg = dr["PrintRegNO"].ToString()
                });
            }
            return new JsonResult { Data = SearchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        
        public JsonResult DataBind(int CertificateNo)
                {
            BirthCertificate Obj_Model = new KeystoneProject.Models.Patient.BirthCertificate();
            DataSet StoreAllCity = new DataSet();

            StoreAllCity = bl_birth.GetDataForPatientBirthCertificate(CertificateNo);
            List<BirthCertificate> serch = new List<BirthCertificate>();
            if (StoreAllCity.Tables.Count != null)
            {
                foreach (DataRow dr in StoreAllCity.Tables[0].Rows)
                {
                         Obj_Model.CertificateNo = Convert.ToInt32(dr["CertificateNo"].ToString());
                         Obj_Model. ReferenceCode = dr["ReferenceCode"].ToString();
                         Obj_Model.PatientRegNo = Convert.ToInt32(dr["PatientRegNo"].ToString());
                         Obj_Model.PatientName = dr["PatientName"].ToString();                        
                         Obj_Model.MotherName = dr["MotherName"].ToString();
                         Obj_Model. Cast = dr["Cast"].ToString();
                         Obj_Model. FatherName = dr["FatherName"].ToString();
                         Obj_Model. Address = dr["Address"].ToString();
                         Obj_Model. MAge = dr["MAge"].ToString();
                         Obj_Model. MNationality = dr["MNationality"].ToString();
                         Obj_Model. MQualification = dr["MQualification"].ToString();
                         Obj_Model.  MOccuption = dr["MOccuption"].ToString();
                         Obj_Model. MReligion = dr["MReligion"].ToString();
                         Obj_Model. FAge = dr["FAge"].ToString();
                         Obj_Model. FQualification = dr["FQualification"].ToString();
                         Obj_Model. FNationality = dr["FNationality"].ToString();
                         Obj_Model. FOccuption = dr["FOccuption"].ToString();
                         Obj_Model. FReligion = dr["FReligion"].ToString();
                         if (dr["Date"].ToString() != "")
                         {
                             Obj_Model.Date = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                         }
                           if (dr["DOB1"].ToString() != "")
                           {
                               Obj_Model.DOB1 = Convert.ToDateTime(dr["DOB1"]).ToString("yyyy-MM-dd");
                           }
                           if (dr["DOB2"].ToString() != "")
                           {
                               Obj_Model.DOB2 = Convert.ToDateTime(dr["DOB2"]).ToString("yyyy-MM-dd");
                           }
                           if (dr["DOB3"].ToString() != "")
                           {
                               Obj_Model.DOB3 = Convert.ToDateTime(dr["DOB3"]).ToString("yyyy-MM-dd");
                           }
                           if (dr["DOB4"].ToString() != "")
                           {
                               Obj_Model.DOB4 = Convert.ToDateTime(dr["DOB4"]).ToString("yyyy-MM-dd");
                           }

                           if (dr["TOB4"].ToString() != "")
                           {
                               Obj_Model.TOB4 = Convert.ToDateTime(dr["TOB4"]).ToString("hh:mm:ss tt");
                           }
                           if (dr["TOB1"].ToString() != "")
                           {
                               Obj_Model.TOB1 = Convert.ToDateTime(dr["TOB1"]).ToString("hh:mm:ss tt");
                           }
                           if (dr["TOB2"].ToString() != "")
                           {
                               Obj_Model.TOB2 = Convert.ToDateTime(dr["TOB2"]).ToString("hh:mm:ss tt");
                           }
                           if (dr["TOB3"].ToString() != "")
                           {
                               Obj_Model.TOB3 = Convert.ToDateTime(dr["TOB3"]).ToString("hh:mm:ss tt");
                           }

                        Obj_Model.BirthPlace = dr["BirthPlace"].ToString();
                        Obj_Model.Weight1 = dr["Weight1"].ToString();
                        Obj_Model.Weight2 = dr["Weight2"].ToString();
                        Obj_Model.Weight3 = dr["Weight3"].ToString();
                        Obj_Model.Weight4 = dr["Weight4"].ToString();
                        Obj_Model.Sex1 = dr["Sex1"].ToString();
                        Obj_Model.Sex2 = dr["Sex2"].ToString();
                        Obj_Model.Sex3 = dr["Sex3"].ToString();
                        Obj_Model.Sex4 = dr["Sex4"].ToString();
                        Obj_Model.DeliveryType = dr["DeliveryType"].ToString();
                        Obj_Model.NoOfChild = dr["NoOfChild"].ToString();
                        Obj_Model.HODone = dr["HODone"].ToString();
                        Obj_Model.Remarks = dr["Remarks"].ToString();
                        serch.Add(Obj_Model);
                }
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult Fill(int CertificateNo)
        //{
        //    DataSet ds = new DataSet();
        //    List<BirthCertificate> Search = new List<BirthCertificate>();
        //    ds = bl_birth.GetDataForPatientBirthCertificate(CertificateNo);
        //    BirthCertificate birthcert = new BirthCertificate();
        //    birthcert.CertificateNo = Convert.ToInt32(ds.Tables[0].Rows[0]["CertificateNo"].ToString());
        //    birthcert.PatientName = ds.Tables[0].Rows[0]["PatientName"].ToString();
        //    birthcert.PatientRegNo = Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNo"].ToString());
        //    birthcert.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();
        //    birthcert.Cast = ds.Tables[0].Rows[0]["Cast"].ToString();
        //    birthcert.Date = ds.Tables[0].Rows[0]["Date"].ToString();
        //    birthcert.MotherName = ds.Tables[0].Rows[0]["MotherName"].ToString();
        //    birthcert.FatherName = ds.Tables[0].Rows[0]["FatherName"].ToString();
        //    birthcert.Address = ds.Tables[0].Rows[0]["Address"].ToString();
        //    birthcert.MAge = ds.Tables[0].Rows[0]["MAge"].ToString();
        //    birthcert.MNationality = ds.Tables[0].Rows[0]["MNationality"].ToString();
        //    birthcert.MOccuption = ds.Tables[0].Rows[0]["MOccuption"].ToString();
        //    birthcert.MQualification = ds.Tables[0].Rows[0]["MQualification"].ToString();
        //    birthcert.MReligion = ds.Tables[0].Rows[0]["MReligion"].ToString();
        //    birthcert.FAge = ds.Tables[0].Rows[0]["FAge"].ToString();
        //    birthcert.FNationality = ds.Tables[0].Rows[0]["FNationality"].ToString();
        //    birthcert.FOccuption = ds.Tables[0].Rows[0]["FOccuption"].ToString();
        //    birthcert.FQualification = ds.Tables[0].Rows[0]["FQualification"].ToString();
        //    birthcert.FReligion = ds.Tables[0].Rows[0]["FReligion"].ToString();
        //    birthcert.DOB1 = ds.Tables[0].Rows[0]["DOB1"].ToString();
        //    birthcert.DOB2 = ds.Tables[0].Rows[0]["DOB2"].ToString();
        //    birthcert.DOB3 = ds.Tables[0].Rows[0]["DOB3"].ToString();
        //    birthcert.DOB4 = ds.Tables[0].Rows[0]["DOB4"].ToString();
        //    birthcert.TOB1 = ds.Tables[0].Rows[0]["TOB1"].ToString();
        //    birthcert.TOB2 = ds.Tables[0].Rows[0]["TOB2"].ToString();
        //    birthcert.TOB3 = ds.Tables[0].Rows[0]["TOB3"].ToString();
        //    birthcert.TOB4 = ds.Tables[0].Rows[0]["TOB4"].ToString();
        //    birthcert.Weight1 = ds.Tables[0].Rows[0]["Weight1"].ToString();
        //    birthcert.Weight2 = ds.Tables[0].Rows[0]["Weight2"].ToString();
        //    birthcert.Weight3 = ds.Tables[0].Rows[0]["Weight3"].ToString();
        //    birthcert.Weight4 = ds.Tables[0].Rows[0]["Weight4"].ToString();
        //    birthcert.Sex1 = ds.Tables[0].Rows[0]["Sex1"].ToString();
        //    birthcert.Sex2 = ds.Tables[0].Rows[0]["Sex2"].ToString();
        //    birthcert.Sex3 = ds.Tables[0].Rows[0]["Sex3"].ToString();
        //    birthcert.Sex4 = ds.Tables[0].Rows[0]["Sex4"].ToString();
        //    birthcert.BirthPlace = ds.Tables[0].Rows[0]["BirthPlace"].ToString();
        //    birthcert.DeliveryType = ds.Tables[0].Rows[0]["DeliveryType"].ToString();
        //    birthcert.HODone = ds.Tables[0].Rows[0]["HODone"].ToString();
        //    birthcert.Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
        //    birthcert.NoOfChild = ds.Tables[0].Rows[0]["NoOfChild"].ToString();
       
        //    birthcert.Mode = "Edit";
        //    Search.Add(birthcert);

        //    return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public JsonResult DeletePatientBirthCertificate(int CertificateNo)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Patient.BL_BirthCertificate bl_birthcert = new Buisness_Logic.Patient.BL_BirthCertificate();
                int a = bl_birthcert.DeletePatientBirthCertificate(CertificateNo);
                if (a == 1)
                {
                    data = "Birth Certificate Deleted Successfully";
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
            BL_BirthCertificate bl_death = new BL_BirthCertificate();
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