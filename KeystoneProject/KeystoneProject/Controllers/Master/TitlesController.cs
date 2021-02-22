using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class TitlesController : Controller
    {
        //
        // GET: /Titles/
        public ActionResult Titles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Titles(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            Titles location = new Titles();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;



            return View();
        }
	}
}