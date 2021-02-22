using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Patient
{
    public class QuestionHeadController : Controller
    {
        BL_QuestionHead questionhead = new BL_QuestionHead();

        public JsonResult ShowQuestionHead()
        {

            BL_QuestionHead questionhead1 = new BL_QuestionHead();

            return new JsonResult { Data = questionhead1.SelectAllQuestionHead(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
         
        [HttpGet]
        public ActionResult QuestionHead()
        {
            return View();
        }

        [HttpPost]

        public ActionResult QuestionHead(QuestionHead obj)
        {
            try
            {
                BL_QuestionHead questionhead2 = new BL_QuestionHead();
                if (questionhead2.CheckQuestionHead(obj.QuestionHeadID, obj.QuestionHeadName))
                {
                    if (questionhead2.QuestionHead(obj))
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

        [HttpGet]

        public JsonResult EditQuestionHead(int id)
        {
            BL_QuestionHead questionhead1 = new BL_QuestionHead();
            ModelState.Clear();

            return new JsonResult { Data = questionhead1.GetQuestionHead(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         
        }

        public ActionResult DeleteQuestionHead(int QuestionHeadID)
        {

            string del = "";
            QuestionHeadID = Convert.ToInt32(Request.Form["QuestionHeadID"]);
            BL_QuestionHead questionhead1 = new BL_QuestionHead();

            if (questionhead1.DeleteQuestionHead(QuestionHeadID))
            {
                del = "Delete";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        } 
	}
}