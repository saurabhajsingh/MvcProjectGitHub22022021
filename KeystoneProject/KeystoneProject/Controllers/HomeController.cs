using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using KeystoneProject.Models.Pharmacy;
using KeystoneProject.Buisness_Logic.Patient;
using KeystoneProject.Models.Patient;

namespace KeystoneProject.Controllers
{
    public class HomeController : Controller
    
    {
        int HospitalID =0;
        int LocationID =0;
        int UserID =0;

        private SqlConnection con;

        private void Connect()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


       
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your Dashboard page.";
            return View();
        }


        public ActionResult Dashboard1()
        {
            ViewBag.Message = "Your Dashboard page.";
            return View();
        }
        public ActionResult Master()
        {
            ViewBag.Message = "Your Dashboard page.";
            return View();
        }
        public DataSet ReportMISPatientWiseCollectionReport(DateTime Fromdate,DateTime ToDate, string PatientType)
        {
            DataSet ds = new DataSet();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("ReportMISPatientAccountStatusDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@DateFrom", Fromdate);
                cmd.Parameters.AddWithValue("@DateTo", ToDate);
                cmd.Parameters.AddWithValue("@PatientType", PatientType);
                cmd.Parameters.AddWithValue("@PatientRegNo", "%");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;

            }
        }
        public ActionResult FromDate_Todate(string datefrom ,string dateto)
        {
            DataSet ds = new DataSet();

            string Month = Convert.ToDateTime(dateto).ToString("MM");
            DateTime fromdate = new DateTime(Convert.ToDateTime(dateto).Year, 01, 01);
            int mon = Convert.ToInt32( Convert.ToInt32(0));
            DateTime startdate = new DateTime();
            string format = "";
            int mo = Convert.ToDateTime(datefrom).Month;
            KeystoneProject.Models.Keystone.DashbordChart obj = new Models.Keystone.DashbordChart();
            List<KeystoneProject.Models.Keystone.DashbordChart> AddList = new List<Models.Keystone.DashbordChart>();
            for (var m = Convert.ToDateTime(datefrom).Month; m <= Convert.ToInt32(Month); m++)
            {
                int ipdAddMonth = 0;
                format+= DateTime.Now.AddMonths(-(mon)).ToString("MMM") + ","; 
              //  string format1= DateTime.Now.AddMonths(-(mon)).ToString("dd/MM/yyyy");
                //DataView dv1 = new DataView(ds.Tables[5], " Date = " +Convert.ToDateTime(format) + " ", "", DataViewRowState.CurrentRows);
                DateTime now = new DateTime();

                var startDate = Convert.ToDateTime(datefrom);
                var endDate = Convert.ToDateTime(dateto);

                //var endDate = startDate.AddMonths(1).AddDays(-1);

                // OPD IPD Collection Month Wise

                DataSet ReportMISPatientWiseCollectionReportOPD = ReportMISPatientWiseCollectionReport(startDate, endDate, "OPD");
              
                DataSet ReportMISPatientWiseCollectionReportIPD = ReportMISPatientWiseCollectionReport(startDate, endDate, "IPD");
                //End
                //Income And Expence
                KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports.BL_MISDailyIncomeExpensesReport BL_IncomeExpence = new Buisness_Logic.MISReport.MIS_PatientReports.BL_MISDailyIncomeExpensesReport();

              DataSet dsIncome_Expence= BL_IncomeExpence.RptReportDailyEncomeExpences(startDate, endDate, "%");

                //End

               string Month1 = Convert.ToDateTime(datefrom).AddMonths((mon)).ToString("MMM");
                AddList.Add(new Models.Keystone.DashbordChart {
                     Month= Convert.ToDateTime(datefrom).AddMonths((mon)).ToString("MMM"),
                OPDCollection = ReportMISPatientWiseCollectionReportOPD.Tables[0].Compute("sum([PaidAmount])", string.Empty).ToString(),
                   IPDCollection = ReportMISPatientWiseCollectionReportIPD.Tables[0].Compute("sum([PaidAmount])", string.Empty).ToString(),
                    Income = dsIncome_Expence.Tables[0].Compute("sum([INCOME])", string.Empty).ToString(),
                    Expence = dsIncome_Expence.Tables[1].Compute("sum([EXPENSES])", string.Empty).ToString(),
                });
                mon++;
                //End
            }



                   

                
                //  ViewData["dataPoints1"] = dataPoints1;
                // ViewBag.DataPoints  = JsonConvert.SerializeObject(dataPoints);

            
            //ViewBag.DataPoints = dataPoints;
          
            return new JsonResult { Data = AddList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult DashBordPatientOPDNew()
        {
            BL_BedStatus bl_status = new BL_BedStatus();
            string o = DateTime.Now.ToString("dd-MM-yyyy");
            SqlDataAdapter ad = new SqlDataAdapter();
            KeystoneProject.Models.Keystone.DashbordChart obj = new Models.Keystone.DashbordChart();
            Connect();
            // BedStatus bedstatus = new BedStatus();
           DataSet dsstatus = bl_status.GetAllBedStatus();

            DataSet ReportMISPatientWiseCollectionReportOPD= ReportMISPatientWiseCollectionReport(DateTime.Now,DateTime.Now, "OPD");


            obj.OPDCollection = ReportMISPatientWiseCollectionReportOPD.Tables[0].Compute("sum([PaidAmount])", string.Empty).ToString();

            DataSet ReportMISPatientWiseCollectionReportIPD = ReportMISPatientWiseCollectionReport(DateTime.Now, DateTime.Now, "IPD");


           obj.IPDCollection= ReportMISPatientWiseCollectionReportIPD.Tables[0].Compute("sum([PaidAmount])", string.Empty).ToString();
           if (obj.IPDCollection=="")
            {
                obj.IPDCollection = "0";
            }
            if (obj.OPDCollection == "")
            {
                obj.OPDCollection = "0";
            }
            SqlCommand cmd = new SqlCommand("DashBordPatientOPDNew", con);
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HospitalID", HospitalID);
            cmd.Parameters.AddWithValue("@LocationID", HospitalID);
            cmd.Parameters.AddWithValue("@fromDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ToDate", DateTime.Now);
            ad.SelectCommand = cmd;
            con.Open();
            ad.Fill(ds);
            con.Close();
            obj.OPDRegNew = ds.Tables[0].Rows.Count.ToString() ;
            obj.IPDRegNew = ds.Tables[1].Rows.Count.ToString();
            obj.AppoimentNew = ds.Tables[2].Rows.Count.ToString();
            obj.IPDDischargeNew = ds.Tables[3].Rows.Count.ToString();
            obj.BedStatus = dsstatus.Tables[0].Rows.Count.ToString();
            DataView dvItem = new DataView(dsstatus.Tables[0], " RegNO > " + 0 + " and IPDNO > " + 0+ " ", "", DataViewRowState.CurrentRows);
            obj.BedStatusOccupied = dvItem.ToTable().Rows.Count.ToString();
            return new JsonResult { Data =obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DashboardWardDetails()
        {
            BL_BedStatus bl_status = new BL_BedStatus();
            DataSet dsstatus = bl_status.GetAllBedStatusDashboad();


           
            List<string> serchword = new List<string>();
            List<string> serchcolor = new List<string>();
            List<string> count = new List<string>();
            foreach (DataRow dr in dsstatus.Tables[0].Rows)
            {
                DataView dvBedCount = new DataView(dsstatus.Tables[1], "WardID = '" + dr["WardID"] + "'", "", DataViewRowState.CurrentRows);

                DataView dvBedOccupied = new DataView(dsstatus.Tables[2], "WardID = '" + dr["WardID"] + "'", "", DataViewRowState.CurrentRows);

                int BedOccuipaid = dvBedOccupied.ToTable().Rows.Count;
                int BedCount = dvBedCount.ToTable().Rows.Count;
                decimal Total =    Convert.ToDecimal( BedOccuipaid) / Convert.ToDecimal(BedCount) * (100);

                count.Add(Total.ToString());
                serchword.Add(dr["WardName"].ToString());

                serchcolor.Add(dr["wardcolor"].ToString());
            }
            return Json(new { serchword = serchword ,serchcolor= serchcolor, count= count }, JsonRequestBehavior.AllowGet);

        }
    }
}