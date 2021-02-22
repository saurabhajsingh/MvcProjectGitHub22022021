using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class HospitalController : Controller
    {
        //
        // GET: /DrugPackage/
        [HttpGet]
        public ActionResult Hospital()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Hospital(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            Hospital location = new Hospital();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
    }
}