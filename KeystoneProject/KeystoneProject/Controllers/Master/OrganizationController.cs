using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using System.Data;
namespace KeystoneProject.Controllers.Master
{
    public class OrganizationController : Controller
    {
        //
        // GET: /Organization/
        public ActionResult Organization()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Organization( Organization location, FormCollection collection)
        {
            try
            {
                BL_Organization BLOrg = new BL_Organization();
                location.ContactFrom = collection["ContactFrom"];
                location.ContactPersonName1 = collection["personNameOne"];
                location.ContactPersonName2 = collection["personNameTwo"];
                location.Designation1 = collection["designationOne"];
                location.Designation2 = collection["designationTwo"];
                location.ContactPersonEmail1 = collection["emailIdOne"];
                location.ContactPersonEmail2 = collection["emailIdTwo"];
                location.ContactPersonPhoneNO1 = collection["phoneNumberOne"];
                location.ContactPersonPhoneNO2 = collection["phoneNumberTwo"];

                //String[] str1 = str.Split(new char[',']);

                if (BLOrg.CheckOrganization(location.OrganizationID, location.OrganizationName))
                {
                    if (location.OrganizationID > 0)
                    {
                        if (BLOrg.Edit(location))
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Organization Updated Successfully !";
                        }

                    }
                    else
                    {

                        if (BLOrg.Save(location))
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Organization Saved Successfully !";
                        }

                    }
                }
                else
                {
                    TempData["Msg"] = "Organization Already Exist's";
                }

            }
            catch(Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return View();
            }

            return RedirectToAction("Organization", "Organization");
        }




        public JsonResult Edit_Organization(int OrganizationID)
        {
            BL_Organization BLOrg = new BL_Organization(); 
           
            List<Organization> serch = new List<Organization>();

            DataSet ds = BLOrg.SelectOrganizationByID(OrganizationID);
            DataSet ds1 = BLOrg.SelectContactPersonByID(OrganizationID);
            Organization obj = new Organization();

            DataSet dss = BLOrg.GetCity("%");


            DataView dv=new DataView(dss.Tables[0],"CityID='"+ds.Tables[0].Rows[0]["CityID"].ToString()+"'","",DataViewRowState.CurrentRows);
              
            if(dv.ToTable().Rows.Count>0)
            {
                obj.CityName = dv.ToTable().Rows[0]["CityName"].ToString();
                obj.StateName = dv.ToTable().Rows[0]["StateName"].ToString();
                obj.CountryName = dv.ToTable().Rows[0]["CountryName"].ToString();
            }
            obj.OrganizationID = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganizationID"].ToString());
            obj.OrganizationName = ds.Tables[0].Rows[0]["OrganizationName"].ToString();
            obj.OrganizationType = ds.Tables[0].Rows[0]["OrganizationType"].ToString();
            obj.EnrollmentStatus = ds.Tables[0].Rows[0]["EnrollmentStatus"].ToString();
            if (ds.Tables[0].Rows[0]["ContactFrom"].ToString() == "")
            {
                obj.ContactFrom = null;
            }                                                                          
            else
            {
               // obj.ContactFrom = Convert.ToDateTime(ds.Tables[0].Rows[0]["ContactFrom"]).ToString("MM/dd/yyyy");
                obj.ContactFrom1 = ds.Tables[0].Rows[0]["ContactFrom"].ToString();
            }
            if (ds.Tables[0].Rows[0]["ContactUpTo"].ToString() == "")
            {
                obj.ContactUpTo = null;
            }
            else
            {
                //obj.ContactUpTo = Convert.ToDateTime(Convert.ToDateTime(ds.Tables[0].Rows[0]["ContactUpTo"]).ToString("MM/dd/yyyy"));

                obj.ContactUpTo1 = ds.Tables[0].Rows[0]["ContactUpTo"].ToString();
               // obj.ContactUpTo.ToString("yyyy-MM-dd");
            }
            obj.Address = ds.Tables[0].Rows[0]["Address"].ToString();
            obj.CityID = Convert.ToInt32(ds.Tables[0].Rows[0]["CityID"].ToString());
            obj.PinCode = ds.Tables[0].Rows[0]["PinCode"].ToString();
            obj.StateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"].ToString());
            obj.CountryID = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryID"].ToString());
            obj.GSTNO = ds.Tables[0].Rows[0]["GSTNO"].ToString();
            obj.FaxNo = ds.Tables[0].Rows[0]["FaxNo"].ToString();
            obj.PhoneNo1 = ds.Tables[0].Rows[0]["PhoneNo1"].ToString();
            obj.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
            obj.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
            obj.URL = ds.Tables[0].Rows[0]["URL"].ToString();
            obj.CutForTPA = Convert.ToDecimal(ds.Tables[0].Rows[0]["CutForTPA"].ToString());
            DataSet dscon = new DataSet();

            dscon = BLOrg.SelectContactPersonByID(OrganizationID);
           serch.Add(obj);
           foreach (DataRow dr in dscon.Tables[0].Rows)
           {
        
                 serch.Add(new Organization
                  {

                     ContactPersonID = Convert.ToInt32(dr["ContactPersonID"].ToString()),
                     ContactPersonName1 = dr["ContactPersonName"].ToString(),
                     Designation1 = dr["Designation"].ToString(),
                     ContactPersonPhoneNO1 = dr["PhoneNO"].ToString(),
                     ContactPersonEmail1 = dr["EmailID"].ToString(),
                     ReferenceCode = dr["ReferenceCode"].ToString(),



                 });
          }
           
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BindCity(string prefix)
        {
            Organization location = new Models.Master.Organization();
            BL_Organization BLOrg = new BL_Organization();

            List<Organization> serch = new List<Organization>();
            DataSet ds = BLOrg.GetCity(prefix);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new Organization
                {

                    CityID = Convert.ToInt32(dr["CityID"]),
                    CityName = dr["CityName"].ToString(),
                    StateID = Convert.ToInt32(dr["StateID"].ToString()),
                    StateName = dr["StateName"].ToString(),
                    CountryID = Convert.ToInt32(dr["CountryID"].ToString()),
                    CountryName = dr["CountryName"].ToString(),

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ShowAllOrganization()
        {
            Organization location = new Models.Master.Organization();
            BL_Organization BLOrg = new BL_Organization();
            List<Organization> serch = new List<Organization>();
            DataSet ds = BLOrg.ShowAllOrganization();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new Organization
                {
                    OrganizationName = dr["OrganizationName"].ToString(),
                    ReferenceCode = dr["ReferenceCode"].ToString(),
                    OrganizationID =Convert.ToInt32( dr["OrganizationID"].ToString())

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult DeleteOrganization(int OrganizationID)
        { string _Del = "";
            try
            {
               
                Organization location = new Models.Master.Organization();
                BL_Organization BLOrg = new BL_Organization();

                List<Organization> serch = new List<Organization>();
                int DependaincyName = BLOrg.DeleteOrganization(OrganizationID);
                if (DependaincyName == 1)
                {
                    _Del = "Doctor Deleted Successfully";
                }
                else
                {
                    _Del = "You Delete First" + DependaincyName;
                }
            }
            catch(Exception ex)
            {
                _Del = ex.Message;
            }

            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}