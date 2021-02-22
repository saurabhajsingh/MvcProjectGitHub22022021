using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Buisness_Logic.MasterLaboratory;


namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class OutSourceLabController : Controller
    {
        //
        // GET: /OutSourceLab/
        private SqlConnection con;
       
        public ActionResult OutSourceLab()
        {
            return View();
        }
       


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }

    [HttpPost]
        public ActionResult OutSourceLab(OutSourceLab obj,FormCollection fc)
        
    {
            BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
            try
            {
                if (_OutSourceLab.CheckOutSourceLab(obj.OutSourceLabID, obj.LabName))
                {
                    if (obj.OutSourceLabID == null || obj.OutSourceLabID == "0")
                    {
                        obj.OutSourceLabID = "0";
                    }
                    else
                    {
                        obj.OutSourceLabID = (fc["OutSourceLabID"].ToString());
                    }
                    if (fc["LabtestdetailsID"] == null)
                    {
                        obj.OutSourceLabtestdetailsID = "0";
                    }
                    else
                    {
                        obj.OutSourceLabtestdetailsID = fc["LabtestdetailsID"].ToString();
                    }
                    obj.LabName = fc["LabName"].ToString();
                    if (obj.ManagingBody == "" || obj.ManagingBody == null)
                    {
                        obj.ManagingBody = "";
                    }
                    else
                    {
                        obj.ManagingBody = fc["ManagingBody"].ToString();
                    }

                    //if (obj.Percentage == "" || obj.Percentage == null)
                    //{
                    //    obj.Percentage = "";
                    //}
                    //else
                    //{
                    //    obj.Percentage = fc["Percentage"].ToString();
                    //}

                    if (obj.Adminstrator == "")
                    {
                        obj.Adminstrator = "";
                    }
                    else
                    {
                        obj.Adminstrator = fc["Adminstrative"].ToString();
                    }

                    if (obj.ContactPerson == "" || obj.ContactPerson == null)
                    {
                        obj.ContactPerson = "";
                    }
                    else
                    {
                        obj.ContactPerson = fc["ContactPerson"].ToString();
                    }
                    //   obj.OutSourceLabtestdetailsID = "";

                    obj.TestGroupName = fc["TestGroupName"].ToString();
                    obj.TestName = fc["TestName"].ToString();
                    obj.Rate = fc["Rate1"].ToString();
                    obj.Percentage = fc["Percent"].ToString();
                    obj.Test = fc["Test"].ToString();
                    obj.GroupID = fc["GroupID"].ToString();


                    if (obj.Address == "" || obj.Address == null)
                    {
                        obj.Address = "";
                    }
                    else
                    {
                        obj.Address = fc["Address"].ToString();
                    }


                    if (obj.CityID == 0 || obj.CityID == null)
                    {
                        obj.CityID = 0;
                    }
                    else
                    {
                        obj.CityID = Convert.ToInt32(fc["CityID"].ToString());
                    }

                    if (obj.CountryID == 0 || obj.CountryID == null)
                    {
                        obj.CountryID = 0;
                    }
                    else
                    {
                        obj.CountryID = Convert.ToInt32(fc["CountryID"].ToString());
                    }

                    if (obj.StateID == 0 || obj.StateID == null)
                    {
                        obj.StateID = 0;
                    }
                    else
                    {
                        obj.StateID = Convert.ToInt32(fc["StateID"].ToString());
                    }

                    if (obj.PinCode == "" || obj.PinCode == null)
                    {
                        obj.PinCode = "";
                    }
                    else
                    {
                        obj.PinCode = fc["PinCode"].ToString();
                    }

                    if (obj.PhoneNo == "" || obj.PhoneNo == null)
                    {
                        obj.PhoneNo = "";
                    }
                    else
                    {
                        obj.PhoneNo = fc["PhoneNo"].ToString();
                    }

                    if (obj.MobileNo == "" || obj.MobileNo == null)
                    {
                        obj.MobileNo = "";
                    }
                    else
                    {
                        obj.MobileNo = fc["MobileNo"].ToString();
                    }

                    if (obj.EmailID == "" || obj.EmailID == null)
                    {
                        obj.EmailID = "";
                    }
                    else
                    {
                        obj.EmailID = fc["EmailID"].ToString();
                    }

                    if (obj.FaxNo == "" || obj.FaxNo == null)
                    {
                        obj.FaxNo = "";
                    }
                    else
                    {
                        obj.FaxNo = fc["FaxNo"].ToString();
                    }

                    if (obj.URL == "" || obj.URL == null)
                    {
                        obj.URL = "";
                    }
                    else
                    {
                        obj.URL = fc["URL"].ToString();
                    }



                    if (_OutSourceLab.Save(obj))
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        TempData["Msg"] = "OutSourceLab Saved Succussfully ";
                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "OutSourceLab Already Exist's ";
                }
                return RedirectToAction("OutSourceLab", "OutSourceLab");
              
            }
            catch (Exception ex)
            {
                //ExceptionManager.Publish(ex);
                //ShowMessage(ex.Message, MessageTypes.ErrorMessage);
            }
            return View(obj);
        }

    public JsonResult Edit(int OutSourceLabID)
    {

        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
        OutSourceLab obj = new OutSourceLab();
        List<OutSourceLab> GetOutSourceLab = new List<OutSourceLab>();
        DataSet ds = _OutSourceLab.GetOutSourceLab(OutSourceLabID);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            GetOutSourceLab.Add(new OutSourceLab

            {
                HospitalID = Convert.ToInt32(dr["HospitalID"]),
                LocationID = Convert.ToInt32(dr["LocationID"]),
                OutSourceLabID = (dr["OutSourceLabID"]).ToString(),
                LabName = dr["LabName"].ToString(),
                ReferenceCode = dr["ReferenceCode"].ToString(),
                ManagingBody = dr["ManagingBody"].ToString(),
                Adminstrator = dr["Adminstrator"].ToString(),
                Address = dr["Address"].ToString(),
                CityID = Convert.ToInt32(dr["CityID"].ToString()),
                PinCode = dr["PinCode"].ToString(),
                StateID = Convert.ToInt32(dr["StateID"].ToString()),
                CountryID = Convert.ToInt32(dr["CountryID"].ToString()),
                PhoneNo = dr["PhoneNo"].ToString(),
                MobileNo = dr["MobileNo"].ToString(),
                EmailID = dr["EmailID"].ToString(),
                URL = dr["URL"].ToString(),
                FaxNo = dr["FaxNo"].ToString(),
                ContactPerson = dr["ContactPerson"].ToString(),
                Mode = dr["Mode"].ToString(),
                CreationID = Convert.ToInt32(dr["CreationID"]),

               
            });
        }



        return new JsonResult { Data = GetOutSourceLab, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }

    [HttpGet]
    public JsonResult GetTestMasterForBindCategory(string prefix)
    {
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
        return new JsonResult { Data = _OutSourceLab.GetTestMasterForBindCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }


    public ActionResult GetCity(string prefix)
    {
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
        DataSet ds = _OutSourceLab.GetCity(prefix);
          List<OutSourceLab> GetCityName = new List<OutSourceLab>();
      
       
    
        int i = 0;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            OutSourceLab obj = new OutSourceLab();
            obj.CityID  = Convert.ToInt32(dr["CityID"].ToString());

            obj.City = dr["CityName"].ToString();
            if (dr["StateID"] == "" && dr["StateID"]=="0")
            {
                obj.StateID = 0;
            }
            else
            {
                obj.StateID = Convert.ToInt32(dr["StateID"].ToString());
            }
            
            obj.State = dr["StateName"].ToString();
            if (dr["CountryID"] == null || dr["CountryID"] == "")
            {
                obj.CountryID = 0;
            }
            else
            {
                obj.CountryID = Convert.ToInt32(dr["CountryID"].ToString());
            }
            
            obj.Country = dr["CountryName"].ToString();
            GetCityName.Add(obj);
        }
        return new JsonResult { Data = GetCityName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }


    [HttpGet]
    public JsonResult GetTestMasterForBindTestNameGrid(string prefix)
    
    {
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
        //int i=0;
        List<OutSourceLab> GetTestMasterForBindTestNameGrid = new List<OutSourceLab>();
        DataSet ds = _OutSourceLab.GetTestMasterForBindTestNameGrid();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            //if (i < 3920)
            //{
                GetTestMasterForBindTestNameGrid.Add(new OutSourceLab
                {
                    TestID = Convert.ToInt32(dr["TestID"].ToString()),
                    TestName = dr["TestName"].ToString().Replace(",", ";"),
                    TestGroupID = Convert.ToInt32(dr["TestGroupID"].ToString()),
                    TestGroupName = dr["TestGroupName"].ToString(),
                    Rate = dr["Rate"].ToString(),
                    Percentage = "0",





                });
        //    }
        //i++;
        }
        return new JsonResult { Data = GetTestMasterForBindTestNameGrid, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }
    public JsonResult GetTestMasterForBindTestNameGrid1(int OutSourceLabID)
    {
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();

        List<OutSourceLab> GetTestMasterForBindTestNameGrid1 = new List<OutSourceLab>();
        DataSet ds = _OutSourceLab.GetTestMasterForBindTestNameGrid1(OutSourceLabID);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            GetTestMasterForBindTestNameGrid1.Add(new OutSourceLab
            {
                OutSourceLabtestdetailsID= dr["OutSourceLabTestDetailsID"].ToString(),
                TestID = Convert.ToInt32(dr["TestID"].ToString()),
                TestName = dr["TestName"].ToString().Replace(",", ";"),
                TestGroupID = Convert.ToInt32(dr["TestGroupID"].ToString()),
                TestGroupName = dr["TestGroupName"].ToString(),
                Rate = dr["Rate"].ToString(),
                Percentage = dr["Percentage"].ToString(),
                LabName=dr["LabName"].ToString(),




            });
        }
        return new JsonResult { Data = GetTestMasterForBindTestNameGrid1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }
    public JsonResult GetAllOutSourceLab(string prefix)
    {
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();

        List<OutSourceLab> GetAllOutSourceLab = new List<OutSourceLab>();
        DataSet ds = _OutSourceLab.GetAllOutSourceLab();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            GetAllOutSourceLab.Add(new OutSourceLab
            {
                OutSourceLabID = (dr["OutSourceLabID"].ToString()),
                LabName = dr["LabName"].ToString(),
                Address = dr["Address"].ToString(),
               

            });
        }
        return new JsonResult { Data = GetAllOutSourceLab, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }
    public ActionResult Delete(int OutSourceLabID)
    {

        string val = "";
     
        BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
        if (_OutSourceLab.DeleteOutSourceLab(OutSourceLabID))
        {
            val = "OutSourceLab Deleted Successfully";
        }
        //    Response.Redirect("TestMasterAdd.cshtml");
        return Json(val);
    }

	}
}