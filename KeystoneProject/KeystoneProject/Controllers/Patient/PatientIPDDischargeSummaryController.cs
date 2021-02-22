using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientIPDDischargeSummaryController : Controller
    {
        //
        // GET: /PatientIPDDischargeSummary/
        public ActionResult PatientIPDDischargeSummary()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientIPDDischargeSummary(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            PatientIPDDischargeSummary location = new PatientIPDDischargeSummary();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}