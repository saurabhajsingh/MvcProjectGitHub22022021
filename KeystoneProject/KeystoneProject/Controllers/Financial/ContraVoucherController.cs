using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Financial;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Financial
{
    public class ContraVoucherController : Controller
    {
        //
        // GET: /ContraVoucher/
        public ActionResult ContraVoucher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContraVoucher(FormCollection collection)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
            ContraVoucher location = new ContraVoucher();
            location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            location.LocationId = 0;




            return View();
        }
	}
}