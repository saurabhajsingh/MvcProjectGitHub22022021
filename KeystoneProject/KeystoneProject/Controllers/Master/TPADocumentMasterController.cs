using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class TPADocumentMasterController : Controller
    {
        BL_TPADocumentMaster blTPA = new BL_TPADocumentMaster();
        public ActionResult TPADocumentMaster()
        {
            return View();
        }
        public JsonResult Bind_parameter_table()
        {
            return new JsonResult { Data = blTPA.Getdata(), JsonRequestBehavior = new JsonRequestBehavior() };
        }
        public JsonResult Insert_Parameter(string parameter)
        {
            return new JsonResult { Data = blTPA.Add_Parameter(parameter), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        public JsonResult Bind_table()
        {
            return new JsonResult { Data = blTPA.GetTpaLetter(), JsonRequestBehavior = new JsonRequestBehavior() };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TPADocumentMaster(TPADocumentMaster obj, FormCollection fc)
        {
            try
            {
                if (blTPA.Save(obj))
                {
                    if (obj.LetterID > 0)
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TPA Covering Letter Updated Successfully !";
                        return RedirectToAction("TPADocumentMaster", "TPADocumentMaster");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "TPA Covering Letter Saved Successfully !";
                        return RedirectToAction("TPADocumentMaster", "TPADocumentMaster");
                    }
                }

                return RedirectToAction("TPADocumentMaster", "TPADocumentMaster");
            }
            catch (Exception ex)
            {
                return RedirectToAction("TPADocumentMaster", "TPADocumentMaster");
            }

        }

        public JsonResult Delete_letter(int LetterID)
        {
            string _Del = null;
            try
            {
                int DependaincyName = blTPA.Deletedata(LetterID);

                if (DependaincyName == 1)
                {
                    _Del = "TPA Covering Letter Deleted Successfully !";
                }
                else
                {
                    _Del = "TPA Covering Letter Can Not Be Deleted !";
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