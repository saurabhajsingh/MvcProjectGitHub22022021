using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Store;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Store
{
    public class HospitalProductPurchaseController : Controller
    {
        //
        // GET: /HospitalProductPurchase/
        public ActionResult HospitalProductPurchase()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HospitalProductPurchase(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            HospitalProductPurchase location = new HospitalProductPurchase();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}