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
    public class HospitalCreationController : Controller
    {
        //
        // GET: /HospitalCreation/

        BL_HospitalCreation creation = new BL_HospitalCreation();
        [HttpGet]
        public ActionResult HospitalCreation()
        {
            //HospitalCreation hoscre = new Models.Keystone.HospitalCreation();
            //hoscre.StoreAllHospital = creation.SelectAllData();
            return View();
        }

        public JsonResult ShowHospital()
        {
            BL_HospitalCreation db = new BL_HospitalCreation();

            return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult HospitalCreation(HospitalCreation obj)
         {
            try 
            {
                //if (Request.IsAjaxRequest())
                //{
                //    return new EmptyResult();
                //}
            string A=    Request.Form["HospitalName"];
                TempData["Msg"]="";
                if (Request.Form["HospitalName"] != null)
                {
                    if (obj.HospitalID > 0)
                    {
                       
                            if (creation.Save(obj))
                            {
                                obj.HospitalName = "";
                                TempData["Msg"] = "Update Successfully";
                                ModelState.Clear();
                                RedirectToAction("HospitalCreation", "HospitalCreation");
                            }
                        
                       
                        RedirectToAction("HospitalCreation", "HospitalCreation");
                    }

                    else
                    {
                        try
                        {
                            if (creation.Save(obj))
                            {
                                if (obj.HospitalID > 0)
                                {
                                    obj.HospitalName = "";
                                    TempData["Msg"] = "Update Successfully";
                                    ModelState.Clear();
                                    RedirectToAction("HospitalCreation", "HospitalCreation");
                                }
                                TempData["Msg"] = "Save Successfully";
                                RedirectToAction("HospitalCreation", "HospitalCreation");
                            }
                        }

                        catch (Exception ex)
                        {
                            TempData["Msg"] = ex.Message;
                            RedirectToAction("HospitalCreation", "HospitalCreation");
                        }
                    }
                }
        }
          catch(Exception ex)
            {
            TempData["Msg"]= ex.Message;
            return RedirectToAction("HospitalCreation", "HospitalCreation");
            }
            return RedirectToAction("HospitalCreation", "HospitalCreation");
        }

        public JsonResult GetCityRecord(string Prefix)
                {
                BL_HospitalCreation blorg = new BL_HospitalCreation();
                HospitalCreation hoscre = new Models.Keystone.HospitalCreation();
                List<HospitalCreation> Search = new List<HospitalCreation>();
                DataSet dsCity = blorg.GetCity(Prefix); 
                //dsCity = creation.GetCity(Prefix);            
            foreach (DataRow dr in dsCity.Tables[0].Rows)
            {
                Search.Add(new HospitalCreation
                    {
                        CityName = dr["CityName"].ToString(),
                        CityID = dr["CityID"].ToString(),
                        StateID = dr["StateID"].ToString(),
                        StateName = dr["StateName"].ToString(),
                        CountryID = dr["CountryID"].ToString(),
                        CountryName = dr["CountryName"].ToString(),

                    });
            }
            return new JsonResult { Data =Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ImageUpload(HospitalCreation model)
        {
            string path = "";
            var File = model.ImageFile;
          if(File != null)
          {
              var FileName = Path.GetFileName(File.FileName);
              var extention = Path.GetExtension(File.FileName);
              var withoutextention = Path.GetFileNameWithoutExtension(File.FileName);

              path = Server.MapPath("~/")+"MRDFile/"+File.FileName;
              File.SaveAs(path);
              ViewData["ImagePath"] = path;
          }
          return new JsonResult { Data = path , JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        public JsonResult DatatableBind()
        {
            HospitalCreation Obj_Model = new Models.Keystone.HospitalCreation();
            Obj_Model.StoreAllHospital = creation.SelectAllData();
            List<HospitalCreation> serch = new List<HospitalCreation>();

            foreach(DataRow dr in Obj_Model.StoreAllHospital.Tables[0].Rows)
            {
                serch.Add(new HospitalCreation
                    {
                        HospitalID = Convert.ToInt32(dr["HospitalID"]),
                        HospitalName = dr["HospitalName"].ToString(),
                        //Logo = MimeMapping.GetMimeMapping(dr["Logo"].ToString()),
                    });
            }
            return new JsonResult {Data = serch , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Fill(int HospitalID)
        {
            DataSet ds = new DataSet();
            List<HospitalCreation> Search = new List<HospitalCreation>();
            ds = creation.GetHospital(HospitalID);
            HospitalCreation AddHospital = new HospitalCreation();
            AddHospital.HospitalID = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalID"].ToString());
            AddHospital.HospitalName = ds.Tables[0].Rows[0]["HospitalName"].ToString();
            AddHospital.GroupName = ds.Tables[0].Rows[0]["GroupName"].ToString();

            if (ds.Tables[0].Rows[0]["CityID"].ToString() == "0" || ds.Tables[0].Rows[0]["CityName"].ToString() == "")
            {
                AddHospital.CityName = "";
            }
            else
            {
                int cityID = Convert.ToInt32(ds.Tables[0].Rows[0]["CityID"].ToString());
                AddHospital.CityID = cityID.ToString();
                AddHospital.CityName = ds.Tables[0].Rows[0]["CityName"].ToString();
            }
            if (ds.Tables[0].Rows[0]["StateID"].ToString() == "0" || ds.Tables[0].Rows[0]["StateName"].ToString() =="")
            {
                AddHospital.StateName = "";
            }
            else
            {
                int stateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"].ToString());
                AddHospital.StateID = stateID.ToString();
                AddHospital.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
            }
            if (ds.Tables[0].Rows[0]["CountryID"].ToString() == "0" || ds.Tables[0].Rows[0]["CountryName"].ToString() == "")
            {
                AddHospital.CountryName = "";
            }
            else
            {
                int counID = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryID"].ToString());
                AddHospital.CountryID = counID.ToString();
                AddHospital.CountryName = ds.Tables[0].Rows[0]["CountryName"].ToString();
            }
            AddHospital.FaxNo = ds.Tables[0].Rows[0]["FaxNo"].ToString();
            AddHospital.ManagingBody = ds.Tables[0].Rows[0]["ManagingBody"].ToString();
            AddHospital.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
        
            AddHospital.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
            AddHospital.PhoneNo1 = ds.Tables[0].Rows[0]["PhoneNo1"].ToString();
            AddHospital.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();
            AddHospital.Address = ds.Tables[0].Rows[0]["Address"].ToString();
            AddHospital.Adminstrator = ds.Tables[0].Rows[0]["Adminstrator"].ToString();
            AddHospital.PinCode =ds.Tables[0].Rows[0]["PinCode"].ToString();
            AddHospital.URL = ds.Tables[0].Rows[0]["URL"].ToString();
            AddHospital.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
            AddHospital.Mode = "Edit";
            Search.Add(AddHospital);

            return new JsonResult {Data = Search , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteHospital(int HospitalID)
        {
            string _Del = "";
            try
            {
                KeystoneProject.Buisness_Logic.Hospital.BL_HospitalCreation bL_hospital = new Buisness_Logic.Hospital.BL_HospitalCreation();
                int a = bL_hospital.DeleteHospital(HospitalID);
                if (a == 1)
                {
                    _Del = "Country Deleted Successfully";
                }
                
            }
            catch(Exception ex)
            {
                _Del = ex.Message;
            }
            return new JsonResult { Data = _Del , JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
    }
	
}