using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Patient
{
    public class QuestionController : Controller
    {
        BL_Question Bl_Question = new BL_Question();
       
        [HttpGet]
        public ActionResult Question()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Question(Question obj)
        {
            try
            {
                BL_Question question2 = new BL_Question();
                if (question2.CheckQuestion(obj.QuestionID, obj.QuestionName))
                {
                    if (question2.Question(obj))
                    {
                        ModelState.Clear();
                        ViewData["flag"] = "Done";
                    }
                }
                else
                {
                    ViewData["flag"] = "Error";
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }

        }
        public JsonResult GetQuestionHeadRecord(string prefix)
        {

            DataSet dsQuestionhead = new DataSet();
            dsQuestionhead = Bl_Question.GetQuestionHead(prefix);

            List<Question> serch = new List<Question>();

            foreach (DataRow dr in dsQuestionhead.Tables[0].Rows)
            {
                serch.Add(new Question
                {

                    QuestionHeadName = dr["QuestionHeadName"].ToString(),
                    QuestionHeadID = dr["QuestionHeadID"].ToString()

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ShowQuestion()
        {

            BL_Question question1 = new BL_Question();

            return new JsonResult { Data = question1.SelectAllQuestionHead(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult EditQuestion(int id)
        {
            BL_Question question1 = new BL_Question();
            ModelState.Clear();

            return new JsonResult { Data = question1.GetQuestion(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult DeleteQuestion(int QuestionID)
        {

            string del = "";
            QuestionID = Convert.ToInt32(Request.Form["QuestionID"]);
            BL_Question questionhead1 = new BL_Question();

            if (questionhead1.DeleteQuestion(QuestionID))
            {
                del = "Delete";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        } 
	}
}