using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class FinancialYearController : Controller
    {

        int HospitalID;
        int LocationID;
        int CreationID;

        BL_FinancialYear dbFinancialYear = new BL_FinancialYear();
        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
        //
        // GET: /FinancialYear/
        public ActionResult FinancialYear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FinancialYear(FinancialYear objfinancialyear)
        {
            try
            {
                if (dbFinancialYear.CheckFinancialYear(objfinancialyear.FinancialYearID, objfinancialyear.FinancialYears))
                {
                    if (dbFinancialYear.Save(objfinancialyear))
                    {
                        if (objfinancialyear.FinancialYearID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "FinancialYear Update Successfully";
                            return RedirectToAction("FinancialYear", "FinancialYear");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "FinancialYear Saved Successfully";
                            return RedirectToAction("FinancialYear", "FinancialYear");

                        }

                    }
                }
                else
                {
                    TempData["Msg"] = "FinancialYear Already Exist's";
                    return RedirectToAction("FinancialYear", "FinancialYear");
                }

            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("FinancialYear", "FinancialYear");
            }
            return RedirectToAction("FinancialYear", "FinancialYear");
        }
        public JsonResult GetAllFinancialYear()
        {
            FinancialYear year = new FinancialYear();
            BL_FinancialYear blyear = new BL_FinancialYear();

            return new JsonResult { Data = blyear.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EditFinancialYear(int id)
        {
            BL_FinancialYear blyear = new BL_FinancialYear();
            return new JsonResult { Data = blyear.GetFinancialYear(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}