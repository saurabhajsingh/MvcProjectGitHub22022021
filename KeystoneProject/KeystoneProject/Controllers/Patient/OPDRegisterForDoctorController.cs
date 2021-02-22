using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Patient
{
    public class OPDRegisterForDoctorController : Controller
    {
        //
        // GET: /OPDRegisterForDoctor/
        public ActionResult OPDRegisterForDoctor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OPDRegisterForDoctor(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            OPDRegisterForDoctor location = new OPDRegisterForDoctor();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}