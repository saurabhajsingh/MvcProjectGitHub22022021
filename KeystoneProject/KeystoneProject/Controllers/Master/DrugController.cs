using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class DrugController : Controller
    {
        //
        // GET: /Drug/
        [HttpGet]
        public ActionResult Drug()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Drug(Drug location)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
           // Drug location = new Drug();
            Buisness_Logic.Master.BL_Drug obj = new Buisness_Logic.Master.BL_Drug();



            return View();
        }
	}
}