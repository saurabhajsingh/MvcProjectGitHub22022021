using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Buisness_Logic.MasterLaboratory;
using KeystoneProject.Models.Master;
using System.Net;
//using System.IO.TextWriter;

namespace KeystoneProject.Controllers.Master
{
    public class ParameterController : Controller
    {
        //
        // GET: /Parameter/
        private SqlConnection con;
        int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
        int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
        int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        BL_Parameter _Parameter = new BL_Parameter();
        Parameter objParameter = new Parameter();

        public JsonResult ParameterFormulaID(string prefix)
        {

            return new JsonResult { Data = _Parameter.ParameterFormulaID(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUnitID(string prefix)
        {

            return new JsonResult { Data = _Parameter.GetUnitID(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        
        
        }

        public JsonResult GetTestMasterForBindSampleType()
        {
            
            return new JsonResult { Data = _Parameter.GetTestMasterForBindSampleType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetNormalRange()
        {
            return new JsonResult { Data = _Parameter.GetParameterNormalRangeType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        
        }

        public JsonResult GetAllParameter()
        {
            return new JsonResult { Data = _Parameter.GetAllParameter(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        
        }

        public JsonResult GetParameter(int ParameterID)
        {

            return new JsonResult { Data = _Parameter.GetParameterData(ParameterID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        
        }


        public ActionResult Parameter()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {

            string val = "";

            //BL_OutSourceLab _OutSourceLab = new BL_OutSourceLab();
            if (_Parameter.DeleteParameter(id))
            {
                val = "Parameter Deleted Successfully !";
            }
            //    Response.Redirect("TestMasterAdd.cshtml");
            return Json(val);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Parameter(Parameter objpara, FormCollection fc)
        {     
           try
               
           {
               objpara.ParameterID = Request.Form["idParameterName"].ToString();
               if (_Parameter.CheckParameter(objpara.ParameterID, objpara.ParameterName))
               {
               
               objpara.ParameterName = Request.Form["ParameterName"].ToString();
               objpara.PrintAs = Request.Form["PrintAs"].ToString();
               objpara.Alias = Request.Form["AliasName"].ToString();
               objpara.Formula = Request.Form["Formula"].ToString();
               objpara.Method = Request.Form["MethodName"].ToString();
               //objpara.Description = Request.Form["Description"].ToString();
               objpara.Unit = Request.Form["UnitName"].ToString();
               objpara.SampleType = Request.Form["SampleType"].ToString();
               objpara.FormulaWithShortName = Request.Form["txtShortName"].ToString();

               if (Request.Form["txtHelpValue"] != null)
               {
                 //  objpara.HelpValueID = Request.Form["HelpValueID"];
                   objpara.HelpValue = Request.Form["txtHelpValue"];
               }
              
               if (Request.Form["DaysFrom"] != null)
               {
                   //objpara.NormalRangeID = Request.Form["NormalRangeID"];
                   objpara.DaysFrom = Request.Form["DaysFrom"];
                   objpara.DaysTo = Request.Form["DaysTo"];
                   objpara.ConvLow = Request.Form["ConvLow"];
                   objpara.ConvHigh = Request.Form["ConvHigh"];
                   objpara.ConvNormal = Request.Form["ConvNormal"];
                   objpara.Default = Request.Form["Default"];
                   objpara.ParameterID = Request.Form["idParameterName"];
                   objpara.Sex = Request.Form["Gender"];

               }

               if (Request.Form["ParameterFormulaID"] != null)
               {
               objpara.FormulaID = Request.Form["ParameterFormulaID"];
               objpara.Formulareference = Request.Form["txtReferenceName"];
          
               
               }

               if (Request.Form["txtPrecisionValue"] != null)
               {
                  // objpara.PrecisionID = Request.Form["PrecisionID"];
                   objpara.Precision = Request.Form["txtPrecisionValue"];
                
               
               
               }
                    if (objpara.ParameterID=="")
                    {
                        objpara.ParameterID = "0";
                    }
                int parameterid = Convert.ToInt32(objpara.ParameterID);
               if (_Parameter.save(objpara))
               {
                        if (parameterid > 0)
                        {
                            ModelState.Clear();
                           
                            TempData["msg"] = "Parameter Updated Successfully !";
                            return RedirectToAction("Parameter", "Parameter");
                        }
                        else
                        {
                            ModelState.Clear();

                            TempData["msg"] = "Parameter Saved Successfully !";
                            return RedirectToAction("Parameter", "Parameter");
                        }                  
               }
               
          }
           else
           {
               TempData["msg"] = "Parameter Already Exists !";
           }
               return RedirectToAction("Parameter", "Parameter");
        
           }
            catch(Exception ex)
           {

            }
            return View();
        }
	}
}