using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Store;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Store
{
    public class HospitalProductMaintenanceController : Controller
    {
        //
        // GET: /HospitalProductMaintenance/
        public ActionResult HospitalProductMaintenance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HospitalProductMaintenance(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            HospitalProductMaintenance location = new HospitalProductMaintenance();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}