using KeystoneProject.Buisness_Logic.FinancialAccountReport;
using KeystoneProject.Models.FinancialAccountReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeystoneProject.Controllers.FinancialAccountReport
{
    public class MISBalanceSheetController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;
        
        int ScheduleID = 0;
        string ScheduleName;
        string ReportTypeName = "";

        Decimal TotalAmount = 0;

        DataSet dsLIABILITIES = new DataSet();
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        BL_MISBalanceSheet objbl = new BL_MISBalanceSheet();
        MISBalanceSheet objmodels = new MISBalanceSheet();

        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

        [HttpGet]
        public ActionResult MISBalanceSheet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MISBalanceSheet(FormCollection collection)
        {

            objmodels.DateFrom = Convert.ToDateTime(collection["DateFrom"]);
            objmodels.DateTo = Convert.ToDateTime(collection["DateTo"]);


            objmodels.ScheduleID = collection["ScheduleID"];
            objmodels.ScheduleName = collection["ScheduleName"];
            objmodels.Type = collection["Type"];

            Decimal ExpensesTotal = 0;
            Decimal IncomeTotal = 0;
            Decimal IndirectIncomeTotal = 0;
            Decimal IndirectExpensestotal = 0;
            Decimal DiffGross = 0;
            Decimal DiffNet = 0;


            if (objmodels.ScheduleID == "" || objmodels.ScheduleID == null || objmodels.ScheduleID == "All")
            {
                objmodels.ScheduleID = "";

            }
            if (objmodels.ScheduleName == "" || objmodels.ScheduleName == null || objmodels.ScheduleName == "All")
            {
                objmodels.ScheduleName = "%";

            }

            
            try
            {
                Decimal TotalLiabilities = 0;
                string Type = "LIABILITIES";
               
                dsLIABILITIES = objbl.GetLiabilitiesDetails(objmodels);
                string Sname = "";

                ScheduleID = 0;
                Type = "EXPENSES";
                ScheduleName = "";
                DataSet dsExp = new DataSet();
                objmodels.dsPatientReport = objbl.GetExpensesDetails(objmodels);
                if (dsExp.Tables.Count != 0)
                {
                    foreach (DataRow drINCOME in dsExp.Tables[0].Rows)
                    {

                        TotalAmount = 0;
                        drINCOME["Total"] = Math.Round(GetAccountTotalExepence(Convert.ToInt32(drINCOME["ScheduleID"].ToString()), "EXPENSES"), 2);
                        ExpensesTotal = ExpensesTotal + Convert.ToDecimal(drINCOME["Total"]);
                    }
                }
                Type = "INCOME";
                ScheduleName = "";
                DataSet dsInc = new DataSet();

                dsInc = objbl.GetIncomeDetails(objmodels);
                if (dsInc.Tables.Count != 0)
                {
                    foreach (DataRow drINCOME in dsInc.Tables[0].Rows)
                    {
                        TotalAmount = 0;
                        drINCOME["Total"] = Math.Round(GetAccountTotalExepence(Convert.ToInt32(drINCOME["ScheduleID"].ToString()), "INCOME"), 2);

                    }
                }
                string SCHname = "";
                Decimal totalHospitalPurch = 0;
                DataSet dsBill = new DataSet();
                ScheduleID = 0;
                Type = "HOSPITAL SALES TOTAL";
                ScheduleName = "";
                dsBill = objbl.GetIncomeDetails(objmodels);
                if (dsBill.Tables.Count != 0)
                {
                    foreach (DataRow drB in dsBill.Tables[0].Rows)
                    {
                        Sname = (drB["ScheduleName"].ToString());
                        totalHospitalPurch = Convert.ToDecimal(drB["Total"].ToString());
                    }
                }
                DataRow drNew = dsInc.Tables[0].NewRow();
                drNew["ScheduleName"] = Sname.ToString();
                drNew["Total"] = totalHospitalPurch;
                dsInc.Tables[0].Rows.Add(drNew);

                foreach (DataRow drCount in dsInc.Tables[0].Rows)
                {
                    IncomeTotal += Convert.ToDecimal(drCount["Total"].ToString());
                }

                DiffGross = IncomeTotal - ExpensesTotal;
                ScheduleID = 0;
                Type = "INDIRECT INCOME";
                ScheduleName = "";
                DataSet dsIndrIn = new DataSet();

                dsIndrIn = objbl.GetIncomeDetails(objmodels);
                foreach (DataRow drINCOME in dsIndrIn.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drINCOME["Total"] = GetAccountTotalExepence(Convert.ToInt32(drINCOME["ScheduleID"].ToString()), "INDIRECT INCOME");
                    IndirectIncomeTotal = IndirectIncomeTotal + Convert.ToDecimal(drINCOME["Total"]);

                }
              
                Decimal IncomeTally = DiffGross + IndirectIncomeTotal;

                //Get Indirect Expenses
                ScheduleID = 0;
                Type = "INDIRECT EXPENSES";
                ScheduleName = "";
                DataSet dsInExp = new DataSet();

                dsInExp = objbl.GetExpensesDetails(objmodels);
                foreach (DataRow drINCOME in dsInExp.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drINCOME["Total"] = GetAccountTotalExepence(Convert.ToInt32(drINCOME["ScheduleID"].ToString()), "INDIRECT EXPENSES");
                    IndirectExpensestotal = IndirectExpensestotal + Convert.ToDecimal(drINCOME["Total"]);
                }



                DateFrom = objmodels.DateFrom;
                DateTo = objmodels.DateTo;

               

                return View(objmodels);

            }
            catch (Exception ex)
            {
                
                throw;
            }

           
        }


        public decimal GetAccountTotalExepence(int ScheduleTotalID, string Nature)
        {


            string Type = "DETAILS";
            DataSet ds1 = new DataSet();
            if (Nature == "EXPENSES" || Nature == "INDIRECT EXPENSES")
            {
                ds1 = objbl.GetExpensesDetails(objmodels);
            }
            if (Nature == "INCOME" || Nature == "INDIRECT INCOME")
            {
                ds1 = objbl.GetIncomeDetails(objmodels);
            }
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    GetAccountTotalExepence(Convert.ToInt32(dr["ScheduleID"]), Nature);
                }
            }
            else
            {
                foreach (DataRow dr in ds1.Tables[1].Rows)
                {
                    TotalAmount = TotalAmount + (Convert.ToDecimal(dr["Total"].ToString()));
                }
            }

            return TotalAmount;
        }


	}
}