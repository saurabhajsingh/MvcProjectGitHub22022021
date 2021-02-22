using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientFeedbackController : Controller
    {
        BL_PatientFeedback patientfeedback = new BL_PatientFeedback();

        [HttpGet]
        public ActionResult PatientFeedback()
        {
            BL_PatientFeedback patientfeedback1 = new BL_PatientFeedback();
            PatientFeedback AddServiceMod = new PatientFeedback();

            AddServiceMod.dsgrid = patientfeedback1.GetAllFeedbackForm();
            return View(AddServiceMod);
        }

        [HttpPost]
        public ActionResult PatientFeedback(PatientFeedback obj,FormCollection fm)
        {
           
            
            {
                string username = fm["Username"].ToString();


                string comment = fm["Comments"].ToString();
                string headnameid = fm["headid"].ToString();
                string name = fm["Qestion"].ToString();
                string ReviewType = fm["ReviewType"].ToString();
                obj.remark = fm["remark"].ToString();
                string remark = obj.remark;
                obj.userquestionhead = fm["userquestionhead"].ToString();
                string userquestionhead = obj.userquestionhead;

                string[] remark1 = comment.Split(',');
               
                string[] userquestionhead1 = userquestionhead.Split(',');
                string[]  username1 = username.Split(',');
                List<string> stringlist = new List<string>();
                List<string> remarklist = new List<string>();
            
              
                string[] headnameid1 = headnameid.Split(',');

                for (int j = 0; j < ( username1.Length ); j++)
                {
                      for (int i = 0; i < headnameid1.Length; i++)
                {
                    if (userquestionhead1[j] == headnameid1[i])
                   {
                        stringlist.Add(username1[j]);
                   }
                       
                     
                }
                }

                for (int j = 0; j < (remark1.Length); j++)
                {
                    for (int i = 0; i < headnameid1.Length; i++)
                    {
                        if (userquestionhead1[j] == headnameid1[i])
                        {
                            remarklist.Add(remark1[j]);                      
                        }
                    }
                }



                string emp = string.Join(",", stringlist);
                obj.Username = emp;

                string comm = string.Join(",", remarklist);
                obj.Comments = comm;
               
                    try
                    {
                       
                        //if (obj.Comments != "" || obj.Comments != null)
                        //{
                        //    obj.Comments = fm["comment"].ToString();
                        //}
                        //else
                        //{
                        //    obj.Comments = "";
                        //}

                        if (obj.ReviewType != "" || obj.ReviewType != null)
                        {
                            obj.ReviewType = fm["ReviewType"].ToString();
                        }
                        else
                        {
                            obj.ReviewType = "";
                        }
                        obj.QuestionName = "";
                        if (obj.QuestionName != "" || obj.QuestionName != null)
                        {
                            obj.QuestionName = fm["Qestion"].ToString();
                        }
                        else
                        {
                            obj.QuestionName = "";
                        }

                        if (obj.QuestionHeadID != "" || obj.QuestionHeadID != null)
                        {
                            obj.QuestionHeadID = fm["headid"].ToString();
                        }
                        else
                        {
                            obj.QuestionHeadID = "";
                        }

                        BL_PatientFeedback patientfeedback1 = new BL_PatientFeedback();
                        if (patientfeedback1.PatientFeedback(obj))
                        {
                            ModelState.Clear();
                            ViewData["flag"] = "Done";
                        }
                        else
                        {
                            ViewData["flag"] = "Error";
                        }
                        return PatientFeedback();
                    }
                    catch (Exception)
                    {
                        return View();
                    }

            }
        }

        public JsonResult GetPatientNameValue(string prefix)
        {
            BL_PatientFeedback patientfeedback = new BL_PatientFeedback();

            return new JsonResult { Data = patientfeedback.GetPatientName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUserValue(string prefix)
        {
            BL_PatientFeedback patientfeedback1 = new BL_PatientFeedback();

            return new JsonResult { Data = patientfeedback1.GetUserName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillData(string prefix)
                            {
            DataSet ds = new DataSet();
            List<PatientFeedback> Search = new List<Models.Patient.PatientFeedback>();
            try
            {
                ds = patientfeedback.GetPatient(Convert.ToInt32(prefix));
                PatientFeedback AddFeedback = new PatientFeedback();
                Search.Add(
                 new PatientFeedback
                    {
               ContactNo = ds.Tables[0].Rows[0]["MobileNo"].ToString(),
               EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString(),
               Address = ds.Tables[0].Rows[0]["Address"].ToString(),
                ConsultantDoctor = ds.Tables[0].Rows[0]["ConsultantDrName"].ToString(),
                ReferredDr = ds.Tables[0].Rows[0]["ReferredDrName"].ToString(),
               DoctorID = ds.Tables[0].Rows[0]["ConsultantDrID"].ToString(),
               ReferredByDoctorID = ds.Tables[0].Rows[0]["ReferredDrID"].ToString()
            });

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Search, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFeedbackForm()
        {
            BL_PatientFeedback patientfeedback1 = new BL_PatientFeedback();
            PatientFeedback AddServiceMod = new PatientFeedback();

            AddServiceMod.dsgrid = patientfeedback1.GetAllFeedbackForm();


            List<PatientFeedback> search = new List<PatientFeedback>();
            foreach (DataRow dr in AddServiceMod.dsgrid.Tables[0].Rows)
            {

                search.Add(new PatientFeedback
                {
                    QuestionHeadID = dr["QuestionHeadID"].ToString(),
                    QuestionHeadName = dr["QuestionHeadName"].ToString(),
                    QuestionName = dr["QuestionName"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
       
	}
}