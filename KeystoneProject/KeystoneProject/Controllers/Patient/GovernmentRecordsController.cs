using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Patient
{
    public class GovernmentRecordsController : Controller
    {
        //
        // GET: /GovernmentRecords/
        public ActionResult GovernmentRecords()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GovernmentRecords(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            GovernmentRecords location = new GovernmentRecords();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}