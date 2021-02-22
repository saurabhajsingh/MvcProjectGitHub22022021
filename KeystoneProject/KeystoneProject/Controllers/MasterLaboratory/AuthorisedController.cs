using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.MasterLaboratory;
using KeystoneProject.Buisness_Logic.MasterLaboratory;

namespace KeystoneProject.Controllers.MasterLaboratory
{
    public class AuthorisedController : Controller
    {
        //
        // GET: /Authorised/
        public ActionResult Authorised()
        {
            return View();
        }

         [HttpPost]
        public ActionResult Authorised(Authorised obj, FormCollection fc)
        {
             BL_Authorised  _Authorised = new BL_Authorised();
              try
            {
                if (fc["AuthoriserID"].ToString() == null || fc["AuthoriserID"].ToString() == "")
                
                 {
                     obj.AuthoriserID = "0";
                 }
                else
                {
                    obj.AuthoriserID = fc["AuthoriserID"].ToString();
                }
                if (obj.UserId == null || obj.UserId == "")
                {
                    obj.UserId = "0";
                }
                else
                {
                    obj.UserId = fc["UserId"].ToString();
                }
                if (obj.AuthoriserName == null || obj.AuthoriserName == "")
                {
                    obj.AuthoriserName = "";
                }
                else
                {
                    obj.AuthoriserName = fc["AuthoriserName"].ToString();
                }
                if (obj.MobileNo == null || obj.MobileNo == "")
                {
                    obj.MobileNo = "";
                }
                else
                {
                    obj.MobileNo = fc["MobileNo"].ToString();
                }
                if (fc["Image1"].ToString() == null || fc["Image1"].ToString() == "")
                {
                    obj.Signature = "";
                }
                else
                {
                    obj.Signature = fc["Image1"].ToString();
                }
                  if (obj.Remark == null || obj.Remark == "")
                  {
                      obj.Remark = "";
                  }
                  else
                  {
                      obj.Remark = fc["Remark"].ToString();
                  }

                if (_Authorised.Save(obj))
                {
                    if (Convert.ToInt32(obj.AuthoriserID) > 0)
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        TempData["Msg"] = "Authoriser Updated Successfully !";
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                        TempData["Msg"] = "Authoriser Saved Successfully !";
                    }
                   
                }

                else
                {
                    ViewData["flag"] = "Error";
                    TempData["Msg"] = "Authoriser Doesn't Saved !";
                }
                return RedirectToAction("Authorised", "Authorised");

            }
              catch (Exception ex)
              {
           
              }
            return RedirectToAction("Authorised", "Authorised");
        }

         public ActionResult GetAuthoriser(string prefix)
         {
             BL_Authorised  _Authorised = new BL_Authorised();
             DataSet ds = _Authorised.GetAuthorised(prefix);
             List<Authorised> GetAuthorisedName = new List<Authorised>();



             int i = 0;
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 Authorised obj = new Authorised();
                obj.UserId = dr["UserID"].ToString();

                 obj.AuthoriserName = dr["FullName"].ToString();
                 GetAuthorisedName.Add(obj);
             }
             return new JsonResult { Data = GetAuthorisedName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
         public ActionResult Delete(int AuthoriserID)
         {

             string val = "";

             BL_Authorised _Authorised = new BL_Authorised();
             if (_Authorised.DeleteAuthoriser(AuthoriserID))
             {
                 val = "Authoriser Deleted Successfully !";
             }
             //    Response.Redirect("TestMasterAdd.cshtml");
             return Json(val);
         }
         public JsonResult imageuplod(Authorised model)
         {
             string path = "";
             
             var file = model.ImageFile;
             if (file != null)
             {
                 //var path = "";
                 var filename = Path.GetFileName(file.FileName);
                 var extention = Path.GetExtension(file.FileName);
                 var filenamethoutextenction = Path.GetFileNameWithoutExtension(file.FileName);
                 //  file.SaveAs(Server.MapPath("~/MRDFiles/" + file.FileName));
                 //  Session["Paper"] = "/MRDFiles/" + file.FileName;

                 path = Server.MapPath("~/") + "MRDFiles/" + file.FileName;

                 file.SaveAs(path);
                 path = "/MRDFiles/" + file.FileName;
                 ViewData["ImagePath"] = "/MRDFiles/" + file.FileName;
                 // Session["Paper"] = path;
                 // ViewData["ImagePath"] = path;
             }
             return new JsonResult { Data = path, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
         public JsonResult GetAllAuthoriser(string prefix)
         {
             BL_Authorised _Authorised = new BL_Authorised();

             List<Authorised> GetAllAuthorisedName = new List<Authorised>();
             DataSet ds = _Authorised.GetAllAuthoriser();
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 GetAllAuthorisedName.Add(new Authorised
                 {
                     AuthoriserID = dr["AuthoriserID"].ToString(),
                     UserId = dr["UserID"].ToString(),
                     AuthoriserName = dr["AuthoriserName"].ToString(),


                 });
             }
             return new JsonResult { Data = GetAllAuthorisedName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
       [HttpGet]
         public JsonResult GetAuthoriser(int id)
         {

             BL_Authorised _Authorised = new BL_Authorised();
             //Authorised obj = new Authorised();
             List<Authorised> GetAuthorised = new List<Authorised>();
             DataSet ds = _Authorised.GeteditAuthorised(id);
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 GetAuthorised.Add(new Authorised

                 {
                     HospitalId = Convert.ToInt32(dr["HospitalID"]),
                     LocationId = Convert.ToInt32(dr["LocationID"]),
                     UserId = dr["UserId"].ToString(),
                     AuthoriserID = dr["AuthoriserID"].ToString(),
                     AuthoriserName = dr["AuthoriserName"].ToString(),
                     MobileNo = dr["MobileNo"].ToString(),
                     Remark = dr["Remark"].ToString(),
                     Signature = dr["SignatureImg"].ToString(),
                     
                     Mode = dr["Mode"].ToString(),
                     CreationID = Convert.ToInt32(dr["CreationID"]),


                 });
             }



             return new JsonResult { Data = GetAuthorised, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         }
	}
}