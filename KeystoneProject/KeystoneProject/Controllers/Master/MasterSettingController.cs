using KeystoneProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.Master
{
    public class MasterSettingController : Controller
    {
        //
        // GET: /MasterSetting/

        int HospitalID;
        int LocationID;
        int CreationID;

        public void HospitalLocationID()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            CreationID = Convert.ToInt32(Session["CreationID"]);
        }

        [HttpGet]
        public ActionResult MasterSetting(KeystoneProject.Models.Master.MasterSetting MasterSetting)
        {
            Buisness_Logic.Master.BL_MasterSetting setting = new Buisness_Logic.Master.BL_MasterSetting();
            MasterSetting.ShowAllMasterSetting = setting.GetMasterSetting();
            MasterSetting.dsFinancial = setting.GetAllFinancialYear();

            if (MasterSetting.ShowAllMasterSetting.Tables[0].Rows.Count != 0)
            {
                MasterSetting.OPDBillsOutSidePatient = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["OPDBillsOutSidePatient"].ToString());

                MasterSetting.ForAuthorization = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["ForAuthorization"].ToString());

                MasterSetting.OPDLabBillsOutSidePatient = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["OPDLabBillsOutSidePatient"].ToString());
                MasterSetting.MasterSettingID = Convert.ToInt32(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["MasterSettingID"].ToString());
                MasterSetting.DoctorNameInLabReport = MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["DoctorNameInLabReport"].ToString();
                MasterSetting.DoctorNameInLabReport1 = MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["DoctorNameInLabReport1"].ToString();
                MasterSetting.LogoSize = MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["LogoSize"].ToString();
                MasterSetting.LabReportFooter = MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["LabReportFooter"].ToString();

                MasterSetting.OPDRegistration = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["OPDRegistration"].ToString());

                MasterSetting.OpdBill = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["OpdBill"].ToString());
                MasterSetting.IPDAdmission = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["IPDAdmission"].ToString());
                MasterSetting.IpdBill = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["IpdBill"].ToString());
                MasterSetting.ProvisionalBill = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["ProvisionalBill"].ToString());
                MasterSetting.IpdFinalBill = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["IpdFinalBill"].ToString());
                MasterSetting.IPDDischargeSummary = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["IPDDischargeSummary"].ToString());
                MasterSetting.PatientPaymentAndDeposite = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["PatientPaymentAndDeposite"].ToString());
                MasterSetting.PatientPrescription = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["PatientPrescription"].ToString());
                MasterSetting.PatientPrescreptionNew = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["PatientPrescreptionNew"].ToString());
                MasterSetting.LabBills = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["LabBills"].ToString());
                MasterSetting.LabReport = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["LabReport"].ToString());
              
                MasterSetting.Authorized = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["Authorized"].ToString());

                MasterSetting.DateTimeSetting = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["DateTimeSetting"].ToString());

                MasterSetting.FinancialYearID = Convert.ToInt32(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["FinancialYearID"].ToString());
                MasterSetting.FinalBillWithHeader = Convert.ToBoolean(MasterSetting.ShowAllMasterSetting.Tables[0].Rows[0]["FinalBillWithHeader"].ToString());


            }
            return View(MasterSetting);

            
        }

        [HttpPost]

        public ActionResult MasterSetting(FormCollection form1)
        {
            Buisness_Logic.Master.BL_MasterSetting Setting = new Buisness_Logic.Master.BL_MasterSetting();
            KeystoneProject.Models.Master.MasterSetting MasterSetting = new MasterSetting();
            KeystoneProject.Models.Master.MasterSetting blankMasterSetting = new MasterSetting();

            if (form1["IPDAdmission"] == null)
            {
                 MasterSetting.IPDAdmission = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.IPDAdmission = Convert.ToBoolean(true);
            }
            if (form1["FinalBillWithHeader"] == null)
            {
                MasterSetting.FinalBillWithHeader = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.FinalBillWithHeader = Convert.ToBoolean(true);
            }

            if (form1["DateTimeSetting"] == null)
            {
                MasterSetting.DateTimeSetting = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.DateTimeSetting = Convert.ToBoolean(true);
            }

            if (form1["OPDBilling"] == null)
            {
                MasterSetting.OpdBill = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.OpdBill = Convert.ToBoolean(true);

            }

            if (form1["PatientPaymentAndDeposits"] == null)
            {
                MasterSetting.PatientPaymentAndDeposite = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.PatientPaymentAndDeposite = Convert.ToBoolean(true);
            }

            if (form1["IPDBills"] == null)
            {
                MasterSetting.IpdBill = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.IpdBill = Convert.ToBoolean(true);
            }
           

            if (form1["PatientPrescreptionNew"] == null)
            {
                MasterSetting.PatientPrescreptionNew = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.PatientPrescreptionNew = Convert.ToBoolean(true);
            }

            if (form1["LabBills"] == null)
            {
                MasterSetting.LabBills = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.LabBills = Convert.ToBoolean(true);
            }

            if (form1["LabReport"] == null)
            {
                MasterSetting.LabReport = Convert.ToBoolean(false);

            }
            else
            {
                MasterSetting.LabReport = Convert.ToBoolean(true);

            }

            if (form1["IPDFinalBills"] == null)
            {
                MasterSetting.IpdFinalBill = Convert.ToBoolean(false);

            }
            else
            {
                MasterSetting.IpdFinalBill = Convert.ToBoolean(true);

            }
            if (form1["ProvisionalBills"] == null)
            {
                MasterSetting.ProvisionalBill = Convert.ToBoolean(false);

            }
            else
            {
                MasterSetting.ProvisionalBill = Convert.ToBoolean(true);

            }

            if (form1["IPDDischarge"] == null)
            {
                MasterSetting.IPDDischargeSummary = Convert.ToBoolean(false);

            }
            else
            {
                MasterSetting.IPDDischargeSummary = Convert.ToBoolean(true);

            }

            if (form1["Authorized"] == null)
            {
                MasterSetting.Authorized = Convert.ToBoolean(false);
            }
            else
	       {
               MasterSetting.Authorized = Convert.ToBoolean(true);  
	       }
            if (form1["ForAuthorization"] == null)
            {
                MasterSetting.ForAuthorization = Convert.ToBoolean(false);
            }
            else
            {
                MasterSetting.ForAuthorization = Convert.ToBoolean(true);
            }
            MasterSetting.FinancialYearID = Convert.ToInt32(form1["FinancialYear"]);




            MasterSetting.DoctorNameInLabReport = form1["DoctorLabReport"];

            MasterSetting.DoctorNameInLabReport1 = form1["DoctorLabReport1"];



            MasterSetting.LogoSize = form1["LogoSize"];
            MasterSetting.LabReportFooter = form1["FooterSize"];


            MasterSetting.PrintAs = form1["PrintAs"];
            MasterSetting.Sequence = form1["Sequence"];


            MasterSetting.PatientDischargeSettingID = form1["PatientDischargeSettingID"];
            MasterSetting.Name = form1["Name"];
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting BL_obj = new Buisness_Logic.Master.BL_MasterSetting();

            int rows = BL_obj.GetMasterSetting().Tables[0].Rows.Count;
            if (rows == 0)
            {
                MasterSetting.Mode = "Add";
            }
            else
            {
                MasterSetting.Mode = "Edit";

                MasterSetting.MasterSettingID = Convert.ToInt32(form1["MasterSettingID"]);

            }

            string a = form1["txtSequence"];

            bool chk = BL_obj.Save(MasterSetting);

            if (chk == true)
            {
                ModelState.Clear();
                
                TempData["msg"] = "MasterSetting Updated Successfully";

                Buisness_Logic.Master.BL_MasterSetting setting = new Buisness_Logic.Master.BL_MasterSetting();
                blankMasterSetting.ShowAllMasterSetting = setting.GetMasterSetting();
                //blankMasterSetting.dsFinancial = setting.GetAllFinancialYear();
                
            }
            else
            {
                TempData["msg"] = "MasterSetting Not Save";
            }
            return RedirectToAction("MasterSetting", "MasterSetting");
        }
            
        public JsonResult GetDischargeSummarySetting()
        {
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting Bl_obj = new Buisness_Logic.Master.BL_MasterSetting();
            MasterSetting AddServiceMod = new MasterSetting();

            AddServiceMod.dsShowAllMasterSetting = Bl_obj.GetMasterSetting();


            List<MasterSetting> search = new List<MasterSetting>();
            foreach (DataRow dr in AddServiceMod.dsShowAllMasterSetting.Tables[3].Rows)
            {
                search.Add(new MasterSetting
                {
                    PatientDischargeSettingID = dr["PatientDischargeSettingID"].ToString(),
                    Name = dr["Name"].ToString(),
                    PrintAs = dr["PrintAs"].ToString(),
                    Sequence = dr["Sequence"].ToString(),



                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllFinancialYear()
        {
            List<MasterSetting> masterSetting=new List<Models.Master.MasterSetting>();
            KeystoneProject.Buisness_Logic.Master.BL_MasterSetting Bl_obj = new Buisness_Logic.Master.BL_MasterSetting();
            MasterSetting AddServiceMod = new MasterSetting();
          DataSet ds=  Bl_obj.GetAllFinancialYear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                masterSetting.Add(
                    new MasterSetting
                    {
                        FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
                        FinancialYears = Convert.ToString(dr["FinancialYear"]),

                    });
            }

            return new JsonResult { Data = masterSetting, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}