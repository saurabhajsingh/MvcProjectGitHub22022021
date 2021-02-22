using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Models.Keystone;
using System.Data;
using System.IO;

namespace KeystoneProject.Controllers.Hospital
{
    public class CityController : Controller
    {
        //
        // GET: /City/



        public ActionResult City()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ShowCity()
        {
            BL_City city = new BL_City();

            return new JsonResult { Data = city.SelectAllCity(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        BL_City Obj_City = new BL_City();
        [HttpPost]

        public ActionResult City(City obj)
        {

            try
            {
                if (Obj_City.CheckCity(obj.CityID, obj.CityName))
                {

                    string A = Request.Form["CityName"];
                    TempData["Msg"] = "";
                    if (Request.Form["CityName"] != null)
                    {
                        if (obj.CityID > 0)
                        {

                            if (Obj_City.Save(obj))
                            {
                                obj.StateName = "";
                                TempData["Msg"] = "City Updated Successfully !";
                                ModelState.Clear();
                                RedirectToAction("City", "City");
                            }
                            
                            RedirectToAction("City", "City");
                        }

                        else
                        {
                            try
                            {
                                if (Obj_City.Save(obj))
                                {

                                    obj.StateName = "";
                                    TempData["Msg"] = "City Saved Successfully !";
                                    ModelState.Clear();
                                    RedirectToAction("City", "City");                                    
                                }
                            }

                            catch (Exception ex)
                            {
                                TempData["Msg"] = ex.Message;
                                RedirectToAction("City", "City");
                            }
                        }
                    }
                }
                else
                {
                    //TempData["Msg"] = "Error";
                    TempData["Msg"] = "City Already Exists !";
                    return RedirectToAction("City", "City");

                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("City", "City");
            }
            return RedirectToAction("City", "City");
        }

        //public JsonResult GetCountryRecord(string prefix)
        //{
        //    DataSet dsCountry = new DataSet();
        //    dsCountry = Obj_City.GetCountry(prefix);
        //    List<City> search = new List<City>();
        //    foreach (DataRow dr in dsCountry.Tables[0].Rows)
        //    {
        //        search.Add(new City
        //        {
        //            CountryID = Convert.ToInt32(dr["CountryID"]),
        //            CountryName = dr["CountryName"].ToString(),

        //        });
        //    }
        //    return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        City city = new Models.Keystone.City();
        BL_City bl_city = new BL_City();
        public JsonResult GetStateRecord(string prefix)
        {


            List<City> search = new List<City>();
            DataSet dsState = new DataSet();
            dsState = bl_city.GetState(prefix);
            foreach (DataRow dr in dsState.Tables[0].Rows)
            {
                search.Add(new City
                {
                    StateID = Convert.ToInt32(dr["StateID"]),
                    StateName = dr["StateName"].ToString(),
                    CountryID = Convert.ToInt32(dr["CountryID"].ToString()),
                    CountryName = dr["CountryName"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        City Obj_Model = new Models.Keystone.City();
        public JsonResult DatatableBind(string prefix)
        {

            Obj_Model.StoreAllCity = new DataSet();

            Obj_Model.StoreAllCity = Obj_City.SelectAllCity();
            List<City> serch = new List<City>();
            if (Obj_Model.StoreAllCity.Tables.Count != null)
            {
                foreach (DataRow dr in Obj_Model.StoreAllCity.Tables[0].Rows)
                {
                    serch.Add(new City
                    {
                        CityID = Convert.ToInt32(dr["CityID"]),
                        CityName = dr["CityName"].ToString(),
                        ReferenceCode = dr["ReferenceCode"].ToString(),
                    });
                }
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Fill(int CityID)
        {
            DataSet ds = new DataSet();
            List<City> Search = new List<City>();
            ds = Obj_City.GetCity(CityID);
            City AddCity = new City();
            AddCity.CityID = Convert.ToInt32(ds.Tables[0].Rows[0]["CityID"].ToString());
            AddCity.CityName = ds.Tables[0].Rows[0]["CityName"].ToString();
            AddCity.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();


            if (ds.Tables[0].Rows[0]["CountryID"].ToString() == "0" || ds.Tables[0].Rows[0]["CountryName"].ToString() == "")
            {
                AddCity.CountryName = "";
            }
            else
            {
                AddCity.CountryID = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryID"].ToString());
                //AddState.CountryID = counID.ToString();
                AddCity.CountryName = ds.Tables[0].Rows[0]["CountryName"].ToString();
            }
            if (ds.Tables[0].Rows[0]["StateID"].ToString() == "" || ds.Tables[0].Rows[0]["StateName"].ToString() == "")
            {
                AddCity.StateName = "";
            }
            else
            {
                AddCity.StateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"].ToString());
                //AddState.CountryID = counID.ToString();
                AddCity.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
            }

            AddCity.Mode = "Edit";
            Search.Add(AddCity);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteCity(int CityID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Hospital.BL_City bl_city = new Buisness_Logic.Hospital.BL_City();
                int a = bl_city.DeleteCity(CityID);
                if (a == 1)
                {
                    data = "City Deleted Successfully !";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}