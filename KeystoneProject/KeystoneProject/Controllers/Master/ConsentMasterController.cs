using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data.SqlClient;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class ConsentMasterController : Controller
    {

        BL_ConsentMaster consentMaster = new BL_ConsentMaster();


        private SqlConnection con;

        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);

        public void HospitalLocation()
        {
            HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
            LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
            UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        }
        [HttpGet]
        public ActionResult ConsentMaster()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConsentMaster(FormCollection fc, ConsentMaster obj)
        {
            try
            {
                if (consentMaster.CheckConsentMaster(obj.ConsentID, obj.ConsentName))
                {

                   if (obj.ConsentName != null)
                    {
                        obj.ConsentName = fc["ConsentName"].ToString();
                    }
                    else
                    {
                        obj.ConsentName = "";
                    }
                    if (obj.Language != null)
                    {
                        obj.Language = fc["Language"].ToString();
                    }
                    else
                    {
                        obj.Language = "";
                    }
                    if (obj.PathName != null)
                    {
                        obj.PathName = fc["PathName"].ToString();
                    }
                    else
                    {
                        obj.PathName = "";
                    }
                    if (obj.ReferenceCode != null)
                    {
                        obj.ReferenceCode = fc["ReferenceCode"].ToString();
                    }
                    else
                    {
                        obj.ReferenceCode = "";
                    }
                    if (obj.ConsentID == "")
                    {
                        obj.ConsentID = "0";
                    }

                    int consentID = Convert.ToInt32(obj.ConsentID);

                    if (consentMaster.Save(obj))
                    {

                        if (consentID > 0)
                        {
                            ModelState.Clear();

                            TempData["msg"] = "ConsentMaster Updated Successfully";
                            return RedirectToAction("ConsentMaster", "ConsentMaster");
                        }
                        else
                        {
                            ModelState.Clear();

                            TempData["msg"] = "ConsentMaster Saved Successfully";
                            return RedirectToAction("ConsentMaster", "ConsentMaster");
                        }

                    }
                }
                else
                {
                    TempData["Msg"] = "ConsentMaster Already Exist's";
                }
                return RedirectToAction("ConsentMaster", "ConsentMaster");
            }
            catch (Exception exe)
            {
                throw;
            }
            return RedirectToAction("ConsentMaster", "ConsentMaster");

        }

        public JsonResult GetAllConsentMaster()
        {
            BL_ConsentMaster BL_obj = new BL_ConsentMaster();
            return new JsonResult { Data = BL_obj.GetAllConsentMaster(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EditConsentMaster(int ConsentID)
        {
            BL_ConsentMaster cons = new BL_ConsentMaster();
            ConsentMaster obj = new ConsentMaster();
            List<ConsentMaster> GetList = new List<ConsentMaster>();
            DataSet ds = cons.GetConsentMaster(ConsentID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetList.Add(new ConsentMaster

                {

                    HospitalID = Convert.ToInt32(dr["HospitalID"]),
                    LocationID = Convert.ToInt32(dr["LocationID"]),
                    ConsentName = dr["ConsentName"].ToString(),
                    ConsentID = dr["ConsentID"].ToString(),
                    Path = dr["Path"].ToString(),
                   
                });
            }



            return new JsonResult { Data = GetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Delete(int ConsentID)
        {
            string val = "";
            ConsentMaster obj = new ConsentMaster();
            BL_ConsentMaster consentMaster = new BL_ConsentMaster();
            if (consentMaster.DeleteConsentMaster(ConsentID))
            {
                val = "ConsentMaster Deleted Successfully";
            }
            return Json(val);
        }
    }
}