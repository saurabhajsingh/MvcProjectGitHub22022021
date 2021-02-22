using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class MasterVaccinationReminderController : Controller
    {
        //
        // GET: /MasterVaccinationReminder/
        public ActionResult MasterVaccinationReminder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MasterVaccinationReminder(MasterVaccinationReminder location , FormCollection collection)
        {
            try
            {


                BL_MasterVaccinationReminder BLmastervacc = new BL_MasterVaccinationReminder();
                //HospitalLocation Location = new HospitalLocation();
                //collection
                //return View();


                location.VaccinesName = collection["VaccinesName"];
                if (BLmastervacc.CheckMasterVaccinationReminder(location.MasterVaccinationReminderID, location.MasterVaccinationReminderName))
                {
                    if (location.MasterVaccinationReminderID > 0)
                    {
                        if (BLmastervacc.Edit(location))
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Record Updated Successfully";
                        }
                    }
                    else
                    {
                        if (BLmastervacc.Save(location))
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "Record Save Successfully";
                        }
                    }
                }
                else
                {
                    TempData["Msg"] = "Already Exist's";
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("MasterVaccinationReminder", "MasterVaccinationReminder");
            }

            return RedirectToAction("MasterVaccinationReminder", "MasterVaccinationReminder");
        }

       

        public JsonResult ShowAllMasterVaccinationReminder()
        {
            MasterVaccinationReminder location = new Models.Master.MasterVaccinationReminder();
            BL_MasterVaccinationReminder BLmstvac = new BL_MasterVaccinationReminder();
            List<MasterVaccinationReminder> serch = new List<MasterVaccinationReminder>();
            DataSet ds = BLmstvac.ShowAllMasterVaccinationReminder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                serch.Add(new MasterVaccinationReminder
                {
                    MasterVaccinationReminderName = dr["MasterVaccinationReminderName"].ToString(),
                   
                    MasterVaccinationReminderID = Convert.ToInt32(dr["MasterVaccinationReminderID"].ToString())

                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Edit_MasterVaccinationReminder(int MasterVaccinationReminderID)
        {
           
            BL_MasterVaccinationReminder BLMstVacc = new BL_MasterVaccinationReminder();
            List<MasterVaccinationReminder> serch = new List<MasterVaccinationReminder>();

            DataSet ds = BLMstVacc.SelectMasterVaccinationReminderByID(MasterVaccinationReminderID);

            MasterVaccinationReminder obj = new MasterVaccinationReminder();

            obj.MasterVaccinationReminderID = Convert.ToInt32(ds.Tables[0].Rows[0]["MasterVaccinationReminderID"].ToString());
            obj.MasterVaccinationReminderName = ds.Tables[0].Rows[0]["MasterVaccinationReminderName"].ToString();
            obj.Advice = ds.Tables[0].Rows[0]["Advise"].ToString();

           // serch.Add(obj);
            foreach (DataRow dr in ds.Tables[1].Rows)
            {

                serch.Add(new MasterVaccinationReminder
                {

                    VaccinesName = dr["VaccinesName"].ToString(),
                MasterVaccinationReminderName = ds.Tables[0].Rows[0]["MasterVaccinationReminderName"].ToString(),
           Advice = ds.Tables[0].Rows[0]["Advise"].ToString(),

                });
            }

            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult DeleteMasterVaccinationReminder(int MasterVaccinationReminderID)
        {
            string _Del = "";
            try
            {

                MasterVaccinationReminder location = new Models.Master.MasterVaccinationReminder();
                BL_MasterVaccinationReminder BLMstVacc = new BL_MasterVaccinationReminder();

                List<BL_MasterVaccinationReminder> serch = new List<BL_MasterVaccinationReminder>();
                int DependaincyName = BLMstVacc.DeleteMasterVaccinationReminder(MasterVaccinationReminderID);
                if (DependaincyName == 1)
                {
                    _Del = "Deleted Successfully";
                }
                else
                {
                    _Del = "You Delete First" + DependaincyName;
                }
            }
            catch (Exception ex)
            {
                _Del = ex.Message;
            }

            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}