using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
namespace KeystoneProject.Controllers.Patient
{
    public class BedStatusController : Controller
    {
        //
        // GET: /BedStatus/
        int HospitalID;
        int LocationID;

        BL_BedStatus bl_status = new BL_BedStatus();
        BedStatus bedstatus = new BedStatus();
        [HttpGet]
        public ActionResult BedStatus(FormCollection collection)
        {
            bedstatus.dsView = bl_status.GetAllBedStatus();
            return View(bedstatus);
        }

        [HttpPost]
        public ActionResult BedStatus(BedStatus bed)
        {
            //BL_BedStatus db = new BL_BedStatus();
            //ModelState.Clear();
            //return View(db.GetAllBedStatus());

            bed.dsView = bl_status.GetAllBedStatus();
            return View(bed);
        }

        public ActionResult GetPatientIPDBedStatus(string prefix)
        {
            DataSet ds = bl_status.GetBedStatus(prefix);
            return View(prefix);
        }

    }
}