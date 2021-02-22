using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class InvestigationAdviceController : Controller
    {
        //
        // GET: /InvestigationAdvice/
        public ActionResult InvestigationAdvice()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InvestigationAdvice(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            InvestigationAdvice location = new InvestigationAdvice();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}