using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Models.Keystone;
using System.Data;
using System.IO;
namespace KeystoneProject.Controllers.Hospital
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/

       
        public ActionResult Country()
        {
            return View();
        }
     
        public JsonResult ShowData()
        {
            BL_Country count = new BL_Country();

            return new JsonResult { Data = count.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        BL_Country Obj_Country = new BL_Country();
        [HttpPost]

        public ActionResult Country(Country obj)
        {
            try
            {
                if (Obj_Country.CheckCountry(obj.CountryID, obj.CountryName))
                {
                    if (Obj_Country.Save(obj))
                    {
                        if (obj.CountryID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Country Updated Successfully !";
                            return RedirectToAction("Country", "Country");

                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Country Saved Successfully !";
                            return RedirectToAction("Country", "Country");
                        }
                       

                    }
                }
                else
                {
                    TempData["Msg"] = "Country Already Exists !";
                    return RedirectToAction("Country", "Country");
                }

            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("Country", "Country");
            }
            return RedirectToAction("Country", "Country");
        }

        public JsonResult DatatableBind(string prefix)
        {
            Country Obj_Model = new Models.Keystone.Country();
            Obj_Model.StoreAllCountry = Obj_Country.SelectAllData();
            List<Country> serch = new List<Country>();
            foreach (DataRow dr in Obj_Model.StoreAllCountry.Tables[0].Rows)
            {
                serch.Add(new Country
                {
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                    CountryName = dr["CountryName"].ToString(),
                    ReferenceCode = dr["ReferenceCode"].ToString(),
                    ISDCode = dr["ISDCode"].ToString(),


                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteCountry(int CountryID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Hospital.BL_Country bL_Country = new Buisness_Logic.Hospital.BL_Country();
                int a = bL_Country.DeleteCountry(CountryID);
                if (a == 1)
                {
                    data = "Country Deleted Successfully !";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Fill(int CountryID)
        {
            DataSet ds = new DataSet();
            List<Country> Search = new List<Country>();
            ds = Obj_Country.GetCountry(CountryID);
            Country AddCountry = new Country();
            AddCountry.CountryID = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryID"].ToString());
            AddCountry.CountryName = ds.Tables[0].Rows[0]["CountryName"].ToString();
            //AddCountry.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();
            AddCountry.ISDCode = ds.Tables[0].Rows[0]["ISDCode"].ToString();
            AddCountry.Mode = "Edit";
            Search.Add(AddCountry);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

	}
}