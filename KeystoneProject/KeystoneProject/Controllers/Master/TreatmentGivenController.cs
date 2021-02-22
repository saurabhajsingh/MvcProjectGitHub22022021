using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class TreatmentGivenController : Controller
    {
        //
        // GET: /TreatmentGiven/
        public ActionResult TreatmentGiven()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TreatmentGiven(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            TreatmentGiven location = new TreatmentGiven();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;



            return View();
        }
	}
}