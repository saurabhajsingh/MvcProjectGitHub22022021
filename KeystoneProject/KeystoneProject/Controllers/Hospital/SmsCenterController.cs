using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Models.Keystone;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Hospital
{
    public class SmsCenterController : Controller
    {
        // GET: SmsCenter

            [HttpGet]
        public ActionResult SmsCenter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SmsCenter(SmsCenter obj , FormCollection fc)
        {
            try
            {
                

                if (obj.custonnumber == null)
                {
                    obj.custonnumber = "";
                }
                else
                {
                    obj.custonnumber = fc["custonnumber"].ToString();
                }

                if (obj.message == null)
                {
                    obj.message = "";
                }
                else
                {
                    obj.message = fc["message"].ToString();
                }

                if (obj.smstype == null)
                {
                    obj.smstype = "";
                }
                else
                {
                    obj.smstype = fc["smstype"].ToString();
                }

                if (obj.checkall == null)
                {
                    obj.checkall = "";
                }
                else
                {
                    obj.checkall = fc["checkall"].ToString();
                }

                if (obj.checkall == "on")
                {
                    obj.checkall = "1";
                }
                else
                {
                    obj.checkall = "0";
                }

                switch (obj.smstype)
                {
                    case "Custom":

                        SendSMS(obj.custonnumber, obj.message);

                        break;
                    case "Patient":
                        
                        break;
                    case "Employee":
                       
                        break;

                    case "Doctors":

                        break;
                    case "SelectAll":
                        DataSet dsSmsCenter =new DataSet();
                        BL_SmsCenter BLds = new BL_SmsCenter();
                        dsSmsCenter =   BLds.GetAllSmsSetting();

                        for (int i = 0; i < dsSmsCenter.Tables[0].Rows.Count; i++)
                        {
                            if (dsSmsCenter.Tables[0].Rows[i]["Select"].ToString() == "1")
                            {
                                if (dsSmsCenter.Tables[0].Rows[i]["MobileNo"].ToString() != "" || dsSmsCenter.Tables[0].Rows[i]["MobileNo"].ToString() != null)
                                {

                                    SendSMS1(dsSmsCenter.Tables[0].Rows[i]["MobileNo"].ToString(), obj.message);
                                }
                            }
                        }
                        break;

                }


                

            }
            catch (Exception ex)
            {
                return View();
            }
            return RedirectToAction("SmsCenter", "SmsCenter");
        }

        public void SendSMS(string custonnumber,string message)
        {
            string MSG="0";
            string MoNo="0";
            DataSet dsSMS = new DataSet();
            BL_SmsCenter smsCenter = new BL_SmsCenter();
            dsSMS = smsCenter.GetMasterSettingData();
            if (dsSMS.Tables.Count > 0)
            {
                MSG = message;
                MoNo = custonnumber;
                string URL = dsSMS.Tables[0].Rows[0]["SMSurl"].ToString();


                string URL1 = URL.Replace("*", MoNo);
                string URL2 = URL1.Replace("$", MSG);
                try
                {
                    HttpWebRequest request = WebRequest.Create(URL2) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                   
                }
                catch { }
            }
        }

        public bool SendSMS1(string MobNo,string message)
        {
            string MSG = "0";
            string MoNo = "0";
            DataSet dsSMS = new DataSet();
            BL_SmsCenter smsCenter = new BL_SmsCenter();
            dsSMS = smsCenter.GetMasterSettingData();
            if (dsSMS.Tables.Count > 0)
            {
                 MSG = message;
                 MoNo = MobNo;

                string URL = dsSMS.Tables[0].Rows[0]["SMSurl"].ToString();


                string URL1 = URL.Replace("*", MoNo);
                string URL2 = URL1.Replace("$", MSG);
                try
                {
                    HttpWebRequest request = WebRequest.Create(URL2) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    //MessageBox.Show("Message Send Successfully.");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public JsonResult ShowSmsCenterForAllData()
        {

            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.SelectAllSmsCenter(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult FillReferredDoctor(string ReferredDoctor)
        {

            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.FillReferredDoctor(ReferredDoctor), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult FillEmployee()
        {

            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.FillEmployee(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult FillPatientType()
        {

            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.FillPatientType(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        public JsonResult FillEmployeeDesignation(string Designation)
        {
            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.FillEmployeeDesignation(Designation), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult FillPatientOPDandIPD(string PatientType)
        {
            BL_SmsCenter smsCenter = new BL_SmsCenter();

            return new JsonResult { Data = smsCenter.FillPatientOPDandIPD(PatientType), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
    }
}