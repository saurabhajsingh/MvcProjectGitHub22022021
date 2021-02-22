using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;
using System.Net;

namespace KeystoneProject.Controllers.Master
{
    public class TemplateController : Controller
    {
        //
        // GET: /Template/
        int HospitalID;
        int LocationID;
        int CreationID;

        BL_Template blobj = new BL_Template();
        PatientPrescriptionNew objmodel = new PatientPrescriptionNew();

        [HttpGet]
        public ActionResult Template()
        {
            HospitlLocationID();
            return View();
        }

        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["UserID"]);
        }
        public ActionResult Template(Template obj)
        {
            try
            {
                if (Request.Form["OldTemplate"].ToString() == null)
                {
                    obj.OldTemplateID = "";
                }
                else
                {
                    obj.OldTemplateID = Request.Form["OldTemplate"].ToString();
                }
               
                obj.MedicineLibraryID = Request.Form["MedicineLibraryID"].ToString();
                obj.Medicines = Request.Form["Medicines"].ToString();
                obj.Strength = Request.Form["Strength"].ToString();
                obj.Frequency = Request.Form["Frequency"].ToString();
                obj.Days = Request.Form["Days"].ToString();
                obj.Instruction = Request.Form["Instruction"].ToString();

                if (blobj.Save(obj))
                {
                    ModelState.Clear();
                    ViewData["flag"] = "Done";
                }

 
                return RedirectToAction("Template", "Template");

            }
            catch (Exception Ex)
            {
                return RedirectToAction("Template", "Template");
            }
        }

        public JsonResult GetAllOldTemplate()
        {
            HospitlLocationID();
            DataSet ds1 = blobj.GetAllOldTemplate(HospitalID, LocationID);
            // string OldBill = ds1.Tables[0].Rows[0]["BillNo&Date"].ToString();
            // objmodel.OldBillNo = ds1.Tables[0].Rows[0]["BillNo&Date"].ToString();

            List<Template> searchlist = new List<Template>();
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                searchlist.Add(new Template
                {
                    OldTemplateID = dr["TemplateID"].ToString(),
                    OldTemplate = dr["TemplateName"].ToString()
                });
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetTemplateOld(string OldTemplateID)
        {
            HospitlLocationID();


            DataSet ds1 = blobj.GetOLDTemplate(OldTemplateID, HospitalID, LocationID);
            DataSet ds = blobj.GetTemplateMedicine(OldTemplateID, HospitalID, LocationID);
            List<Template> searchlistMed = new List<Template>();
            List<Template> SearchlistOld = new List<Template>();
            Template obj = new Template();


            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                 
                obj.TemplateID = ds1.Tables[0].Rows[0]["TemplateID"].ToString();
                obj.TemplateName = ds1.Tables[0].Rows[0]["TemplateName"].ToString();  
                obj.ChiefComplaint = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ChiefComplaint"].ToString());
                obj.Allergies = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Allergies"].ToString());
                obj.ClinicalFinding = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ClinicalFinding"].ToString());
                obj.ProvitionalDiagnosis = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["ProvitionalDiagnosis"].ToString());
                obj.Investigation = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["Investigation"].ToString());
                obj.TreatmentAdvice = WebUtility.HtmlDecode(ds1.Tables[0].Rows[0]["TreatmentAdvice"].ToString());
                 
                SearchlistOld.Add(obj);
            }

            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                searchlistMed.Add(new Template
                {

                    // PatientPrescriptionNewMedicineID = ds.Tables[0].Rows[0]["PatientPrescriptionNewMedicineID"].ToString(),
                    MedicineLibraryID = dr1["MedicineLibraryID"].ToString(),
                    Medicines = dr1["Medicines"].ToString(),
                    Frequency = dr1["Frequency"].ToString(),
                    Days = dr1["Days"].ToString(),
                    Instruction = dr1["Instruction"].ToString(),
                    Strength = dr1["Strength"].ToString()


                });
            }

            return Json(new { SearchlistOld = SearchlistOld, searchlistMed = searchlistMed }, JsonRequestBehavior.AllowGet);

        }
	}
}