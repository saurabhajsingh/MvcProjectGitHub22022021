﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class MailTypeController : Controller
    {
        //
        // GET: /MailType/
        public ActionResult MailType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MailType(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            MailType location = new MailType();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}