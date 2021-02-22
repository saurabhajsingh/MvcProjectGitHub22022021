using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class SMSSettingController : Controller
    {
        //
        // GET: /SMSSetting/

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }
        private SqlConnection con;


        private void Connect()
        {
            string Constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(Constring);
        }
        

        [HttpGet]
        public ActionResult SMSSetting(KeystoneProject.Models.Master.SMSSetting SMSSetting)
        {

            ViewData["smssave"] = "0";
            
            Buisness_Logic.Master.BL_SMSSetting setting = new Buisness_Logic.Master.BL_SMSSetting();
            Buisness_Logic.Master.BL_SMSSetting setting1 = new Buisness_Logic.Master.BL_SMSSetting();
            SMSSetting.dsparamiter = setting.GetMasterSmsDetails();

            SMSSetting.dsparamiter1 = setting1.GetSMSSetting();

            if (SMSSetting.dsparamiter.Tables[1].Rows.Count != 0)
            {
                SMSSetting.MobileSMSID = Convert.ToInt32(SMSSetting.dsparamiter.Tables[1].Rows[0]["MobileSMSID"].ToString());

                SMSSetting.IPDAddimisionDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDAddimisionDrSMS"].ToString();
                SMSSetting.IPDDischargeDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDDischargeDrSMS"].ToString();
                SMSSetting.IPDAddimisionPSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDAddimisionPSMS"].ToString();
                SMSSetting.OPDRegDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["OPDRegDrSMS"].ToString();
                SMSSetting.IPDDischargePSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDDischargePSMS"].ToString();
                SMSSetting.IPDAddimisionCDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDAddimisionCDrSMS"].ToString();
                SMSSetting.IPDDischargeCDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDDischargeCDrSMS"].ToString();
                SMSSetting.AppointmentPSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["AppointmentPSMS"].ToString();
                SMSSetting.AppointmentCDrSMS = SMSSetting.dsparamiter.Tables[1].Rows[0]["AppointmentCDrSMS"].ToString();
            //    SMSSetting.IPDDischargeP = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDDischargeP"].ToString();
            //    SMSSetting.IPDAdmissionCDr = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDAddimisionCDr"].ToString();
            //    SMSSetting.IPDDischargeCDr = SMSSetting.dsparamiter.Tables[1].Rows[0]["IPDDischargeCDr"].ToString();
            //    SMSSetting.AppointmentP = SMSSetting.dsparamiter.Tables[1].Rows[0]["AppointmentP"].ToString();
            //    SMSSetting.AppointmentCDr = SMSSetting.dsparamiter.Tables[1].Rows[0]["AppointmentCDr"].ToString();
            }

            if (SMSSetting.dsparamiter1.Tables[0].Rows.Count != 0)
            {
                SMSSetting.MasterSettingID = Convert.ToInt32(SMSSetting.dsparamiter1.Tables[0].Rows[0]["MasterSettingID"].ToString());
                SMSSetting.chkIPDAddimisionDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDAddimisionDrSMS"].ToString());
                SMSSetting.chkIPDDischargeDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDDischargeDrSMS"].ToString());
                SMSSetting.chkIPDAddimisionPSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDAddimisionPSMS"].ToString());
                SMSSetting.chkOPDRegDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["OPDRegDrSMS"].ToString());
                SMSSetting.chkIPDDischargePSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDDischargePSMS"].ToString());
                SMSSetting.chkIPDAddimisionCDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDAddimisionCDrSMS"].ToString());
                SMSSetting.chkIPDDischargeCDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["IPDDischargeCDrSMS"].ToString());
                SMSSetting.chkAppointmentPSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["AppointmentPSMS"].ToString());
                SMSSetting.chkAppointmentCDrSMS = Convert.ToBoolean(SMSSetting.dsparamiter1.Tables[0].Rows[0]["AppointmentCDrSMS"].ToString());
            }

            return View(SMSSetting);
        }

        [HttpPost]
        public ActionResult SMSSetting(FormCollection fc)
        {
            KeystoneProject.Models.Master.SMSSetting obj = new Models.Master.SMSSetting();
            

            if (fc["chkIPDAddimisionDrSMS"] == null)
            {
                obj.chkIPDAddimisionDrSMS = false;
            }
            else
            {
                obj.chkIPDAddimisionDrSMS = true;
            }

            if (fc["chkIPDDischargeDrSMS"] == null)
            {
                obj.chkIPDDischargeDrSMS = false;
            }
            else
            {
                obj.chkIPDDischargeDrSMS = true;
            }

            if (fc["chkIPDAddimisionPSMS"] == null)
            {
                obj.chkIPDAddimisionPSMS = false;
            }
            else
            {
                obj.chkIPDAddimisionPSMS = true;
            }

            if (fc["chkOPDRegDrSMS"] == null)
            {
                obj.chkOPDRegDrSMS = false;
            }
            else
            {
                obj.chkOPDRegDrSMS = true;
            }

            if (fc["chkIPDDischargePSMS"] == null)
            {
                obj.chkIPDDischargePSMS = false;
            }
            else
            {
                obj.chkIPDDischargePSMS = true;
            }

            if (fc["chkIPDDischargePSMS"] == null)
            {
                obj.chkIPDDischargePSMS = false;
            }
            else
            {
                obj.chkIPDDischargePSMS = true;
            }

            if (fc["chkIPDAddimisionCDrSMS"] == null)
            {
                obj.chkIPDAddimisionCDrSMS = false;
            }
            else
            {
                obj.chkIPDAddimisionCDrSMS = true;
            }

            if (fc["chkIPDDischargeCDrSMS"] == null)
            {
                obj.chkIPDDischargeCDrSMS = false;
            }
            else
            {
                obj.chkIPDDischargeCDrSMS = true;
            }
            if (fc["chkAppointmentPSMS"] == null)
            {
                obj.chkAppointmentPSMS = false;
            }
            else
            {
                obj.chkAppointmentPSMS = true;
            }

            if (fc["chkAppointmentCDrSMS"] == null)
            {
                obj.chkAppointmentCDrSMS = false;
            }
            else
            {
                obj.chkAppointmentCDrSMS = true;
            }

            obj.IPDAddimisionDrSMS = fc["IPDAddimisionDrSMS"];
            obj.IPDDischargeDrSMS = fc["IPDDischargeDrSMS"];
            obj.IPDAddimisionPSMS = fc["IPDAddimisionPSMS"];
            obj.OPDRegDrSMS = fc["OPDRegDrSMS"];
            obj.IPDDischargePSMS = fc["IPDDischargePSMS"];
            obj.IPDAddimisionCDrSMS = fc["IPDAddimisionCDrSMS"];
            obj.IPDDischargeCDrSMS = fc["IPDDischargeCDrSMS"];
            obj.AppointmentPSMS = fc["AppointmentPSMS"];
            obj.AppointmentCDrSMS = fc["AppointmentCDrSMS"];


            HospitalLocationID();
            Connect();
            DataSet dsMasterSetting = new DataSet();
            SqlCommand cmd1 = new SqlCommand("GetMasterSetting", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd1.Parameters.AddWithValue("@LocationID", LocationID);
            con.Open();
            SqlDataAdapter ad1 = new SqlDataAdapter();
            ad1.SelectCommand = cmd1;
            ad1.Fill(dsMasterSetting);

            con.Close();
            dsMasterSetting.Tables[0].Rows[0]["IPDAddimisionDrSMS"] = obj.chkIPDAddimisionDrSMS;
            dsMasterSetting.Tables[0].Rows[0]["MasterSettingID"] = obj.MasterSettingID;
            
            dsMasterSetting.Tables[0].Rows[0]["IPDDischargeDrSMS"] = obj.chkIPDDischargeDrSMS;
            dsMasterSetting.Tables[0].Rows[0]["IPDAddimisionPSMS"] = obj.chkIPDAddimisionPSMS;
            dsMasterSetting.Tables[0].Rows[0]["OPDRegDrSMS"] = obj.chkOPDRegDrSMS;
            dsMasterSetting.Tables[0].Rows[0]["IPDDischargePSMS"] = obj.chkIPDDischargePSMS;
            dsMasterSetting.Tables[0].Rows[0]["IPDAddimisionCDrSMS"] = obj.chkIPDAddimisionCDrSMS;
            dsMasterSetting.Tables[0].Rows[0]["IPDDischargeCDrSMS"] = obj.chkIPDDischargeCDrSMS;
            dsMasterSetting.Tables[0].Rows[0]["AppointmentPSMS"] = obj.chkAppointmentPSMS;
            dsMasterSetting.Tables[0].Rows[0]["AppointmentCDrSMS"] = obj.chkAppointmentCDrSMS;
            
       
            DataSet dschkmastersetting = new DataSet();
            SqlCommand cmdchk = new SqlCommand("update MasterSetting set  IPDAddimisionCDrSMS='" + obj.chkIPDAddimisionCDrSMS + "', OPDRegDrSMS='" + obj.chkOPDRegDrSMS + "' ,IPDAddimisionDrSMS='" + obj.chkIPDAddimisionDrSMS + "' ,IPDAddimisionPSMS='" + obj.chkIPDAddimisionPSMS + "' ,IPDDischargeDrSMS='" + obj.chkIPDDischargeDrSMS + "' ,IPDDischargePSMS='" + obj.chkIPDDischargePSMS + "',IPDDischargeCDrSMS='" + obj.chkIPDDischargeCDrSMS + "',AppointmentCDrSMS='" + obj.chkAppointmentCDrSMS + "' ,AppointmentPSMS='" + obj.chkAppointmentPSMS + "' where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and RowStatus=0", con);
            con.Open();
            int a = cmdchk.ExecuteNonQuery();
            con.Close();


            KeystoneProject.Buisness_Logic.Master.BL_SMSSetting Bl_obj = new Buisness_Logic.Master.BL_SMSSetting();
            int rows = Bl_obj.GetMasterSmsDetails().Tables[1].Rows.Count;
            if (rows == 0)
            {
                obj.Mode = "Add";
            }
            else
            {
                obj.Mode = "Edit";

                obj.MobileSMSID = Convert.ToInt32(fc["MobileSMSID"]);

            }

            bool chk = Bl_obj.Save(obj);

            if (chk == true)
            {
                ModelState.Clear();

                TempData["msg"] = "SMSSetting Updated Successfully";
 
            }
            else
            {
                TempData["msg"] = "SMSSetting Not Save";
            }


            return RedirectToAction("SMSSetting", "SMSSetting");
        }

        public JsonResult GetMasterSmsDetails()
        {
            KeystoneProject.Buisness_Logic.Master.BL_SMSSetting Bl_obj = new Buisness_Logic.Master.BL_SMSSetting();
            KeystoneProject.Models.Master.SMSSetting AddServiceMod = new Models.Master.SMSSetting();

            AddServiceMod.dsparamiter = Bl_obj.GetMasterSmsDetails();


            List<KeystoneProject.Models.Master.SMSSetting> search = new List<KeystoneProject.Models.Master.SMSSetting>();
            foreach (DataRow dr in AddServiceMod.dsparamiter.Tables[0].Rows)
            {
                search.Add(new KeystoneProject.Models.Master.SMSSetting
                {
                    PARAMETER = dr["PARAMETER"].ToString(),



                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}