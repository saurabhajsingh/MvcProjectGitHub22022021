using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Patient
{
    public class TPADocumentController : Controller
    {
        BL_TPADocument bl_tpa = new BL_TPADocument();
        public ActionResult TPADocument()
        {
            return View();
        }

        public ActionResult AjaxMethod(int regno)
        {
            KeystoneProject.Models.Patient.TPADocument obj = new TPADocument();
            List<string> searchList = new List<string>();

            DataTable dt = new DataTable();
            dt = bl_tpa.Bind_detail(regno);

            if (dt.Rows.Count > 0)
            {
                obj.patientName = dt.Rows[0]["PatientName"].ToString();
                searchList.Add(obj.patientName);
            }
            else
            {
                obj.patientName = "Not Exist";
                searchList.Add(obj.patientName);
            }
            return Json(searchList);
        }

        public JsonResult view_letter(int registrationNumber)
        {
            try
            {
                KeystoneProject.Models.Patient.TPADocument obj = new TPADocument();
                List<string> searchList1 = new List<string>();

                DataSet ds = new DataSet();
                DataSet dsparameter = new DataSet();
                dsparameter = bl_tpa.GetParaeter();
                ds = bl_tpa.Bind_letter(registrationNumber);

                string Narration = "";
                string Narration1 = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                    {
                        foreach (DataRow dr in dsparameter.Tables[0].Rows)
                        {

                            Narration = dr1["Naration"].ToString();
                            if (Narration.Contains(dr["PARAMETER"].ToString()))
                            {
                                string from = ds.Tables[0].Rows[0][dr["PARAMETER"].ToString().Replace("@", "")].ToString();
                                string to = dr["PARAMETER"].ToString();
                                Narration = dr1["Naration"].ToString().Replace(dr["PARAMETER"].ToString(), ds.Tables[0].Rows[0][dr["PARAMETER"].ToString().Replace("@", "")].ToString());

                            }
                            dr1["Naration"] = Narration.Replace("%", "\n");
                        }
                    }
                    Narration1 = ds.Tables[1].Rows[0]["Naration"].ToString();
                }
                else
                {
                    Narration1 = "Not Exist";
                }

                obj.patientName = Narration1;
                searchList1.Add(obj.patientName);

                return Json(searchList1);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TPADocument(TPADocument obj, FormCollection fc)
        {
            try
            {
                if (bl_tpa.Save(obj))
                {
                    if (obj.DocumentID > 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TPA Document Updated Successfully !";
                        return RedirectToAction("TPADocument", "TPADocument");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TPA Document Saved Successfully !";
                        return RedirectToAction("TPADocument", "TPADocument");
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["Msg"] = "TPA Document Not Saved !";
                    return RedirectToAction("TPADocument", "TPADocument");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("TPADocument", "TPADocument");
            }

        }

        public JsonResult Old_Doc(int regno)
        {
            return new JsonResult { Data = bl_tpa.Bind_OldDoc(regno), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public ActionResult AjaxMethod_Old_Doc(int regno)
        {
            KeystoneProject.Models.Patient.TPADocument obj = new TPADocument();
            List<string> searchList = new List<string>();

            DataTable dt = new DataTable();
            dt = bl_tpa.Bind_Narration(regno);

            if (dt.Rows.Count > 0)
            {
                obj.narration = dt.Rows[0]["Naration"].ToString();
                obj.patientName = dt.Rows[0]["TpaDocumentID"].ToString();
                searchList.Add(obj.narration);
                searchList.Add(obj.patientName);
            }
            return Json(searchList);
        }

        public JsonResult Print_Data(string registrationNumber)
        {
            try
            {
                KeystoneProject.Models.Patient.TPADocument obj = new TPADocument();
                List<string> searchList1 = new List<string>();

                string regno = registrationNumber;

                searchList1.Add(regno);
                Session["RegNO"] = regno;
                return Json(searchList1);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

    }
}