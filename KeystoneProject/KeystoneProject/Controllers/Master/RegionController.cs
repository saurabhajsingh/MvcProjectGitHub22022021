using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Controllers.Master
{
    public class RegionController : Controller
    {
        //
        // GET: /Region/
        public ActionResult Region()
        {
            KeystoneProject.Buisness_Logic.Master.Bl_Region Bl_obj = new Buisness_Logic.Master.Bl_Region();
            Region location = new Region();
            location.dsRegion = new System.Data.DataSet();
            location.dsRegion = Bl_obj.GetAllRegion();

            return View(location);
        }

        [HttpPost]
        public ActionResult Region(Region location)
        {
            //HospitalLocation Location = new HospitalLocation();
            //collection
            //return View();
           // Region location = new Region();
            //location.HospitalId = Convert.ToInt32(collection["hospital_name_hid_name"]);
            //location.LocationId = 0;


            KeystoneProject.Buisness_Logic.Master.Bl_Region Bl_obj = new Buisness_Logic.Master.Bl_Region();

            if (Bl_obj.Save(location))
            {

            }
            location.dsRegion = Bl_obj.GetAllRegion();

            return RedirectToAction("Region", "Region", location);
        }

        [HttpPost]
        public JsonResult DeleteRegion(int RegionID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Master.Bl_Region Bl_obj = new Buisness_Logic.Master.Bl_Region();

                int a = Bl_obj.DeleteRegion(RegionID);
            if (a == 1)
            {
                data = "Done";
            }

            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}