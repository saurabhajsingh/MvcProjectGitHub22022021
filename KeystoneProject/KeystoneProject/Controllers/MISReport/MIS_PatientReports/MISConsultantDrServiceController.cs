using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.MISReport.MISPatientReport;
using KeystoneProject.Models.MISReport.PatientReport;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.IO;

namespace KeystoneProject.Controllers.PatientReport
{
    public class MISConsultantDrServiceController : Controller
    {
        private SqlConnection con;

        BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
        private void Connect()
        {   
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        //
        // GET: /MISConsultantDrService/
        public ActionResult MISConsultantDrService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MISConsultantDrService(MISConsultantDrService objCon,FormCollection fc)
        {

            try
            {
                BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
                MISConsultantDrService objModel = new MISConsultantDrService();
                objCon.PaidAmtID = fc["PaidID"];
                objCon.PaidDate = fc["PaidDate"];
                    objCon.PaidAmount = fc["ConAmt"];
                    objCon.ServiceID = fc["SerID"];
                    objCon.ServiceName = fc["SerName"];
                    objCon.ServiceAmount = fc["SerAmt"];
                    objCon.PatientID = fc["Regno"];
                    objCon.PatientName = fc["PatName"];
                    objCon.DrID = fc["DocrID"];
                    objCon.DrName = fc["DocName"];

                    objCon.BillAmt = fc["BillAmt"];
                    objCon.BillDt = fc["BillDt"];
                    objCon.BillNo = fc["Billno"];
               
                
               
                if (objCon.PaidAmount != "")
                {
                    
                       
                       

                        

                        // objCon.PaidDate=fc[""];
                        //objCon.PaidAmount = fc["PaidAmt"];

                       


                        if (obj.Save(objCon))
                        {
                            ModelState.Clear();
                            ViewData["flag"] = "Done";
                            TempData["msg"] = "Save Changes";
                        }
                    }

            
                
                    return RedirectToAction("MISConsultantDrService", "MISConsultantDrService");

            }
            catch (Exception Ex)
            {
                return RedirectToAction("MISConsultantDrService", "MISConsultantDrService");
            }

            return View();
        }

        public JsonResult GetAllDoctor(string prefix)
        {
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
            DataSet ds = obj.GetAllDoctor(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISConsultantDrService
                {
                    DrID = dr["DoctorID"].ToString(),
                    DrName = dr["DoctorName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllPatient(string prefix)
        {
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
            DataSet ds = obj.GetAllPatient(prefix);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISConsultantDrService
                {
                    PatientID = dr["PatientRegNO"].ToString(),
                    PatientName = dr["PatientName"].ToString()
                });
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPreBalanceAmount(string id)
        {
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
            DataSet ds = obj.GetPreBalanceAmount(id);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISConsultantDrService
                {
                    PatientID = dr["PatientRegNO"].ToString(),
                    PatientName = dr["PatientName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllServiceForDrList(string type)
        {
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
            DataSet ds = obj.GetAllServiceForDrList(type);
            if(ds.Tables.Count >=0)
            {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MISConsultantDrService
                {
                    ServiceID = dr["ServiceID"].ToString(),
                    ServiceName = dr["ServiceName"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                    UnitID = dr["UnitID"].ToString()
                });
            }
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        //public JsonResult Getservicename(string type)
        //{
        //    BL_Report obj = new BL_Report();
        //    return new JsonResult { Data = obj.Getservicename(type), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        public JsonResult Getservicename(string type)
        {
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
            DataSet ds = obj.Getservicename(type);
            if (ds.Tables.Count >= 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchlist.Add(new MISConsultantDrService
                    {
                        ServiceID =(dr["ServiceID"]).ToString(),
                        ServiceName = Convert.ToString(dr["ServiceName"].ToString()),

                        PatientType = dr["ServiceType"].ToString(),
                        ServiceType = dr["ServiceType"].ToString(),

                    });
                }
            }

            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult Save(string servicename, string id, string type)
        {
            string message = "";
            try
            {
               BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
                MISConsultantDrService ConSer = new MISConsultantDrService();
                //objReschedule.HospitalID = HospitalID;
                //objReschedule.LocationID = LocationID;
                ConSer.HospitalID = Convert.ToInt32(Session["HospitalID"]);
                ConSer.LocationID = Convert.ToInt32(Session["LocationID"]);
                int UserID = Convert.ToInt32(Session["UserID"]);
                ConSer.ServiceID = id;
                ConSer.ServiceName = servicename;
                ConSer.ServiceType = type;
                ConSer.Mode = "Add";
                DataSet ds = obj.Getservicename(type);
                if(ds.Tables[0].Rows.Count > 0)
                {
                    if(ConSer.Mode=="Add")
                    {
                        string ServiceType = ds.Tables[0].Rows[0]["ServiceType"].ToString();
                        Connect();
                        SqlCommand cmd = new SqlCommand("Delete from Getservicename where ServiceType like  '" + ServiceType + '%' + "'", con);
                        con.Open();
                        int a = cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }

                if (obj.IUGetServices(ConSer))
                {
                    TempData["msg"] = "Successfully";
                }
                //return RedirectToAction("MISConsultantDrService", "MISConsultantDrService");
                return new JsonResult { Data = "Successfully", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            //return RedirectToAction("MISConsultantDrService", "MISConsultantDrService");
            return new JsonResult { Data = "Successfully", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetReportMISConsultantDoctor(DateTime FromDate, DateTime ToDate, string PatientType, string ServiceID, string PatientRegNo, string DoctorID)
        
        {
            DataSet dsPatientBillNo = new DataSet();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();
           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            var reg = "";
            var dr = "";
            var ser = "";
            if (PatientRegNo == "0")
            {
                reg = "%";
            }
            else
            {
                reg = Convert.ToInt32(PatientRegNo).ToString();
            }
            if (DoctorID == "0")
            {
                dr = "%";
            }
            else
            {
                dr = Convert.ToInt32(DoctorID).ToString();
            }
            if (ServiceID == null)
            {
                ser = "%";
            }
            else
            {
                ser = (ServiceID).ToString();

            }
            dsPatientBillNo = obj.GetReportMISConsultantDoctor(FromDate, ToDate, PatientType, ser, dr, reg);
           // var PaidAmt = Convert.ToString(dsPatientBillNo.Tables[0].Compute("Sum(PaidAmount)", String.Empty));
            var BillAmt = "";
            var AmtTotal = "";
        //    var BalAmt = "";
            
            DataTable dt1 = dsPatientBillNo.Tables[1].Copy();
            DataTable dt2 = dsPatientBillNo.Tables[2].Copy();

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt2.Rows[i]["DoctorID"].ToString() == dt1.Rows[j]["DoctorID"].ToString() && dt2.Rows[i]["ServiceID"].ToString() == dt1.Rows[j]["ServiceID"].ToString() && dt2.Rows[i]["PatientRegNO"].ToString() == dt1.Rows[j]["Pat.Reg"].ToString())
                    {
                        dt1.Rows[j].Delete();
                    }
                }
                dt1.AcceptChanges();
            }


            
            DataTable dt3 = dt1.Copy();
            dsPatientBillNo.Tables[1].Reset();
            if (dt3.Rows.Count > 0)
            {

                BillAmt = Convert.ToString(dt3.Compute("sum([Bill.Amt])", string.Empty).ToString());
                AmtTotal = Convert.ToString(dt3.Compute("Sum([TotalAmount])", String.Empty).ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                if (PatientType=="IPD")
                {
                    
                    foreach (DataRow dr1 in dt3.Rows)
                    {
                        searchlist.Add(new MISConsultantDrService
                        {
                            ServiceID = dr1["ServiceID"].ToString(),
                            BillNo = dr1["BillNo"].ToString(),
                            DrName = dr1["ConsultantName"].ToString(),
                            PatReg = dr1["Pat.Reg"].ToString(),
                            PatientName = dr1["Pat.Name"].ToString(),
                            BillDt = Convert.ToDateTime(dr1["BillDt"]).ToString("dd-MM-yyyy"),
                            BillAmt = dr1["Bill.Amt"].ToString(),
                            DrID = dr1["DoctorID"].ToString(),
                            OPDIPDNO = dr1["OPDIPDNO"].ToString(),
                            BillType = dr1["BillType"].ToString(),
                            ServiceName = dr1["ServiceName"].ToString(),
                            Rate = dr1["Rate"].ToString(),
                            Quantity = dr1["Quantity"].ToString(),
                            TotalAmount = dr1["TotalAmount"].ToString(),
                            TPACASH = dr1["TPA/CASH"].ToString(),
                            NR = dr1["NR"].ToString(),
                            PaidAmount = dr1["PaidAmount"].ToString(),

                            BalanceAmt = dr1["BalanceAmount"].ToString(),
                            totalamt = AmtTotal,
                            bill = BillAmt
                        });
                    }
                }
                else
                {
                    //if (dt3.Rows[0]["BalanceAmount"] == null)
                    //{
                    //    BalAmt = "0.00";
                    //}
                    foreach (DataRow dr1 in dt3.Rows)
                    {
                        searchlist.Add(new MISConsultantDrService
                        {
                            ServiceID = dr1["ServiceID"].ToString(),
                            BillNo = dr1["BillNo"].ToString(),
                            DrName = dr1["ConsultantName"].ToString(),
                            PatReg = dr1["Pat.Reg"].ToString(),
                            PatientName = dr1["Pat.Name"].ToString(),
                            BillDt = Convert.ToDateTime(dr1["BillDt"]).ToString("dd-MM-yyyy"),
                            BillAmt = dr1["Bill.Amt"].ToString(),
                            DrID = dr1["DoctorID"].ToString(),
                            OPDIPDNO = dr1["OPDIPDNO"].ToString(),
                            BillType = dr1["BillType"].ToString(),
                            ServiceName = dr1["ServiceName"].ToString(),
                            Rate = dr1["Rate"].ToString(),
                            Quantity = dr1["Quantity"].ToString(),
                            TotalAmount = dr1["TotalAmount"].ToString(),
                            //TPACASH = dr1["TPA/CASH"].ToString(),
                            NR = dr1["NR"].ToString(),
                            BalanceAmt =dr1["BalanceAmount"].ToString(),
                            PaidAmount = dr1["PaidAmount"].ToString(),
                            totalamt = AmtTotal,
                            bill = BillAmt
                        });
                    }
                }
               



            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPaidAmt(DateTime FromDate, DateTime ToDate, string PatientType, string PaidType, string ServiceID, string DoctorId,string PatientRegNo)
        {


            DataSet dsPatientBillNo = new DataSet();
            List<MISConsultantDrService> searchlist = new List<MISConsultantDrService>();

           BL_MISConsultantDrService obj = new BL_MISConsultantDrService();
            
            var dr = "";
            var ser = "";
            var PatNo = "";

            if (DoctorId == "")
            {
                dr = "%";
            }
            else
            {
                dr = Convert.ToInt32(DoctorId).ToString();
            }
            if (ServiceID == "0")
            {
                ser = "%";
            }
            else
            {
                ser =(ServiceID).ToString();
            }
            if (PatientRegNo == "")
            {
                PatNo = "%";
            }
            else
            {
                PatNo = (PatientRegNo).ToString();
            }

            dsPatientBillNo = obj.GetPaidAmt(FromDate, ToDate, PatientType, PaidType, ser, dr, PatNo);
            // var PaidAmt = Convert.ToString(dsPatientBillNo.Tables[0].Compute("Sum(PaidAmount)", String.Empty));
            var PaidAmt = "";
            var SerAmt = "";
            //var SerAmt1 = "";
            if (dsPatientBillNo.Tables[1].Rows.Count > 0)
            {
                PaidAmt = Convert.ToString(dsPatientBillNo.Tables[1].Compute("Sum(ConsultantAmount)", String.Empty).ToString());
                SerAmt = Convert.ToString(dsPatientBillNo.Tables[1].Compute("Sum(ServiceAmount)", String.Empty).ToString());
            }



            if (dsPatientBillNo.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr1 in dsPatientBillNo.Tables[1].Rows)
                {
                    searchlist.Add(new MISConsultantDrService
                    {
                        PaidAmtID=dr1["PaidAmtID"].ToString(),
                        BillDt = Convert.ToDateTime(dr1["BillDt"]).ToString("dd-MM-yyyy"),
                        DrName = dr1["ConsultantName"].ToString(),
                        DrID = dr1["DoctorID"].ToString(),
                        PatReg = dr1["Pat.Reg"].ToString(),
                        PatientName = dr1["Pat.Name"].ToString(),
                        ServiceName = dr1["ServiceName"].ToString(),
                        ServiceAmount = dr1["ServiceAmount"].ToString(),
                        PaidAmount = dr1["ConsultantAmount"].ToString(),
                        PaidDate = Convert.ToDateTime(dr1["PaidDate"]).ToString("dd-MM-yyyy"),
                        PaidAmt1 = PaidAmt,
                        SerAmt1 = SerAmt
                        
                       
                        
                    });
                }
                Session["ReportMISPaidUnpaidPatientWise"] = dsPatientBillNo;


            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult RptReportMISPaidUnpaidPatientWise()
        {
            return View();
        }
	}
}