using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class DiagnosisController : Controller
    {
        //
        // GET: /Diagnosis/
        [HttpGet]
        public ActionResult Diagnosis()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Diagnosis(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            Diagnosis location = new Diagnosis();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}