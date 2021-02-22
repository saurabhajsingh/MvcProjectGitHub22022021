using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;
using KeystoneProject.Buisness_Logic.Patient;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KeystoneProject.Controllers.Patient
{
    public class PatientSearchController : Controller
    {
        //
        // GET: /PatientSearch/

        BL_PatientSearch bl_search = new BL_PatientSearch();

        public ActionResult PatientSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientSearch( FormCollection fc)
        {

            return View();
        }


        public JsonResult GetPatientRecord(string prefix)
        {

            string FirstName = "%";
            string Mdlename = "%";
            string LAstname = "%";
            FirstName = prefix;
            DataSet dsPatient = new DataSet();
            dsPatient = bl_search.GetPatient(FirstName, Mdlename, LAstname);
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsPatient.Tables[0].Rows)
            {
                if (dr["PFirstName"].ToString() != "")
                {
                search.Add(new PatientSearch
                     {
                         PatientName = dr["PFirstName"].ToString(),
                         PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                     });
                    }
                }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPMiddleNameRecord(string prefix)
        {
            string FirstName = "%";
            string Mdlename = "%";
            string LAstname = "%";
            Mdlename = prefix;
            DataSet dsPatient = new DataSet();
            dsPatient = bl_search.GetPatient(FirstName, Mdlename, LAstname);
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsPatient.Tables[0].Rows)
            {
                if (dr["PMiddleName"].ToString() != "")
                {
                    search.Add(new PatientSearch
                    {
                        PMiddleName = dr["PMiddleName"].ToString(),
                        PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                    });
                }
              
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPLastNameRecord(string prefix)
        {
               string FirstName= "%";
                string Mdlename = "%";
              string   LAstname = "%";
            LAstname = prefix;
            DataSet dsPatient = new DataSet();
            dsPatient = bl_search.GetPatient(FirstName, Mdlename, LAstname);
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsPatient.Tables[0].Rows)
            {
                if (dr["PLastName"].ToString() != "")
                {
                    search.Add(new PatientSearch
                    {
                        PLastName = dr["PLastName"].ToString(),
                        PatientRegNO = Convert.ToInt32(dr["PatientRegNO"].ToString()),
                    });
                }
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllConsultantDoctorRecord(string prefix)
        {
            DataSet dsdoctor = new DataSet();
            dsdoctor = bl_search.GetAllConsultantDoctor(prefix);
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsdoctor.Tables[0].Rows)
            {

                search.Add(new PatientSearch
                {
                    DoctorPrintName = dr["DoctorPrintName"].ToString(),
                    DoctorID = dr["DoctorID"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetAllRefferedDoctorRecord(string prefix)
        {
            DataSet dsRefdoctor = new DataSet();
            dsRefdoctor = bl_search.GetAllRefferedDoctorRecord(prefix);
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsRefdoctor.Tables[0].Rows)
            {

                search.Add(new PatientSearch
                {
                    RefferedDoctorPrintName = dr["DoctorPrintName"].ToString(),
                    ReffDoctor = dr["DoctorId"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetFinancialYear()
        {
            DataSet dsFinYear = new DataSet();
            dsFinYear = bl_search.getFinancialYear();
            List<PatientSearch> search = new List<PatientSearch>();
            foreach (DataRow dr in dsFinYear.Tables[0].Rows)
            {

                search.Add(new PatientSearch
                {
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"].ToString()),
                    financialYear = dr["financialYear"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetallPatientSearch(string  finacialyear, string FName, string Lastname,string MidLename, string PatientType, string Doctor )
        {
            if (finacialyear=="")
            {
                finacialyear = "%";
            }
            if(Lastname=="")
            {
                Lastname = "%";
            }
            if (FName == "")
            {
                FName = "%";
            }
            if (MidLename == "")
            {
                MidLename = "%";
            }
            if (PatientType == "")
            {
                PatientType = "%";
            }
            if (Doctor == "")
            {
                Doctor = "%";
            }
            DataSet dsPatient = new DataSet();
            dsPatient = bl_search.SelectAllData( finacialyear,FName, Lastname, MidLename, PatientType, Doctor);
            List<PatientSearch> search = new List<PatientSearch>();
           
           foreach(DataRow dr in dsPatient.Tables[0].Rows)
           {
             
                   string nm = dr["Dischargedate"].ToString();
               

               search.Add(new PatientSearch
               {
                   PMiddleName = dr["PMiddleName"].ToString(),
                   PatientRegNO = Convert.ToInt32(dr["PrintRegNO"].ToString()),
                   PFirstName = dr["PFirstName"].ToString(),
                   PLastName = dr["PLastName"].ToString(),
                   PatientType = dr["PatientType"].ToString(),
                   PatientName = dr["PatientName"].ToString(),
                   PrintOPDIPDNO = dr["PrintOPDIPDNO"].ToString(),
                   OPDIPDID = dr["OPDIPDID"].ToString(),                              
                   RegDate =dr["RegDate"].ToString(),                
                   GuardianName = dr["GuardianName"].ToString(),
                   Weight =dr["Weight"].ToString(),
                   PhoneNo = dr["PhoneNo"].ToString(),
                   Address = dr["Address"].ToString(),
                   Age = dr["Age"].ToString(),
                   Gender = dr["Gender"].ToString(),
                   ReffDoctor = dr["ReffDoctor"].ToString(),
                   ConsDoctor = dr["ConsDoctor"].ToString(),     
                   TPAName = dr["TPAName"].ToString(),
                   WardName = dr["WardName"].ToString(),
                   RoomName = dr["RoomName"].ToString(),
                   BedNO = dr["BedNO"].ToString(),
                   PatientStatus = dr["PatientStatus"].ToString(),
                   Dischargedate =dr["Dischargedate"].ToString(),
               });
           }

            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //public JsonResult GetAllFinancialYear(string prifix)
        //{
        //    string financialYear = "";
        //    List<PatientSearch> search = new List<PatientSearch>();
        //  //  KeystoneProject.Buisness_Logic.pa.BL_MasterSetting Bl_obj = new Buisness_Logic.Master.BL_MasterSetting();
        //     DataSet DS = new DataSet();


        //     foreach (DataRow dr in DS.Tables[0].Rows)
        //    {
        //        search.Add(
        //            new PatientSearch
        //            {
        //                FinancialYearID = Convert.ToInt32(dr["FinancialYearID"]),
        //                financialYear = Convert.ToString(dr["FinancialYear"]),

        //            });
        //    }

        //    return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
      
    }
}