using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Buisness_Logic.MISReport.MIS_PatientReports;
using KeystoneProject.Models.MISReport.MIS_PatientReports;
using System.Data;
using System.IO;

namespace KeystoneProject.Controllers.MISReport.MIS_PatientReports
{
    public class MISDailyIncomeExpensesReportController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }

        MISDailyIncomeExpensesReport objMod = new MISDailyIncomeExpensesReport();
        //
        // GET: /MISDailyIncomeExpensesReport/

        BL_MISDailyIncomeExpensesReport obj = new BL_MISDailyIncomeExpensesReport();
        public ActionResult MISDailyIncomeExpensesReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MISDailyIncomeExpensesReport(MISDailyIncomeExpensesReport obj)
        {
            return View();
        }

        public JsonResult BindUserName(string prefix)
        {
            //DataSet ds = objPur.BindSupplierName(prefix);
            List<MISDailyIncomeExpensesReport> searchList = new List<MISDailyIncomeExpensesReport>();

            return new JsonResult { Data = obj.BindUserName(prefix), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RptReportDailyEncomeExpences(DateTime DateFrom, DateTime DateTo, string UserID, string Type)
        {
            var user = "";
            var inc = "";
            var exp="";
            decimal bal=0;
           
           // var supply = "";
            if (UserID == "0")
            {
                user = "%";
            }
            else
            {
                user = UserID;
            }
           

            DataSet ds = new DataSet();
            List<MISDailyIncomeExpensesReport> searchList = new List<MISDailyIncomeExpensesReport>();
            List<MISDailyIncomeExpensesReport> List = new List<MISDailyIncomeExpensesReport>();

            ds = obj.RptReportDailyEncomeExpences(DateFrom, DateTo, user);
            inc = Convert.ToString(ds.Tables[0].Compute("Sum(INCOME)", String.Empty));
            exp = Convert.ToString(ds.Tables[1].Compute("Sum(EXPENSES)", String.Empty));
            if (Type=="All")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataView dv = new DataView(ds.Tables[1], " Date = '" +dr["Date"].ToString() + "'", "", DataViewRowState.CurrentRows);
                    decimal EXPENSES = 0;
                    decimal Balance = 0;
                    if(dv.ToTable("Table1").Rows.Count>0)
                    {
                        foreach (DataRow dr1 in dv.ToTable("Table1").Rows)
                        {
                            EXPENSES = Convert.ToDecimal(dr1["EXPENSES"]);
                            dr["EXPENSES"] = EXPENSES;
                    }
                       // EXPENSES = Convert.ToDecimal(dv.ToTable("Table1").Columns["EXPENSES"].ToString());

                    }
                    else
                    {
                        EXPENSES = 0;
                        dr["EXPENSES"] = EXPENSES;
                    }

                    Balance = Convert.ToDecimal(dr["INCOME"]) - Convert.ToDecimal(EXPENSES);
                   
                  
                     if (exp != "" || exp!=null)
                    {
                        bal = Convert.ToDecimal(inc) - Convert.ToDecimal(exp);
                    }
                     
                     dr["Balance"] = Balance;

                    searchList.Add(new MISDailyIncomeExpensesReport
                    {
                        //SupplierName = dr["Date"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                        Income = dr["INCOME"].ToString(),
                        Expences = EXPENSES.ToString(),
                        Balance = Balance.ToString(),
                        bal1 = Convert.ToString(bal),
                        ExpAmt=exp,
                        IncAmt=inc,

                        //BillNoDate = dr["BillNo&Date"].ToString(),
                        //BillNo = Convert.ToInt32(dr["BillNo"])

                    });
                }
              
            }
           else if (Type=="Income")
            {
                if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    searchList.Add(new MISDailyIncomeExpensesReport
                    {
                        //SupplierName = dr["Date"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                        Income = dr["INCOME"].ToString(),
                        Expences = dr["EXPENSES"].ToString(),
                        Balance = dr["BALANCE"].ToString(),
                        IncAmt = inc,
                        //BillNoDate = dr["BillNo&Date"].ToString(),
                        //BillNo = Convert.ToInt32(dr["BillNo"])

                    });
                }
            }
            }
            else
            {
                if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    searchList.Add(new MISDailyIncomeExpensesReport
                    {
                        Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                       // Income = dr["INCOME"].ToString(),
                        Expences = dr["EXPENSES"].ToString(),
                        ExpAmt = exp,
                     // Balance = dr["BALANCE"].ToString(),
                        //ItemName = dr["ItemName"].ToString(),
                        //Rate = dr["Rate"].ToString(),
                        //Quantity = dr["Quantity"].ToString(),
                        //TotalAmount = dr["TotalAmount"].ToString()
                        //BillNo = Convert.ToInt32(dr["BillNo"])

                    });
                }
            }
            }

            if (user !=null)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        DataView dv1 = new DataView(ds.Tables[3], " Date = '" + dr["Date"].ToString() + "'", "", DataViewRowState.CurrentRows);
                        decimal EXPENSES = 0;
                        decimal Balance = 0;
                     
                        if (dv1.ToTable("Table3").Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in dv1.ToTable("Table3").Rows)
                            {
                                EXPENSES = Convert.ToDecimal(dr1["EXPENSES"]);
                                dr["EXPENSES"] = EXPENSES;
                            }
                            // EXPENSES = Convert.ToDecimal(dv.ToTable("Table1").Columns["EXPENSES"].ToString());

                        }
                        else
                        {
                            EXPENSES = 0;
                            dr["EXPENSES"] = EXPENSES;
                        }

                        Balance = Convert.ToDecimal(dr["INCOME"]) - Convert.ToDecimal(EXPENSES);

                        inc = Convert.ToString(ds.Tables[2].Compute("Sum(INCOME)", String.Empty));
                        exp = Convert.ToString(ds.Tables[3].Compute("Sum(EXPENSES)", String.Empty));
                        bal = Convert.ToDecimal(inc) - Convert.ToDecimal(exp);
                        dr["Balance"] = Balance;
                        List.Add(new MISDailyIncomeExpensesReport
                        {
                            Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy"),
                            UserID = dr["UserID"].ToString(),
                            UserName = dr["User Name"].ToString(),
                            Income = dr["INCOME"].ToString(),
                            Expences = EXPENSES.ToString(),
                            Balance = Balance.ToString(),
                            bal1 = Convert.ToString(bal),
                            ExpAmt = exp,
                            IncAmt = inc,


                        });
                    }
                }

                Session["RptReportDailyEncomeExpences"] = ds;
            }
            
            





            return Json(new { searchList = searchList, List = List, }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportDailyEncomeExpences(string Type)
        {
            Session["Type"] = Type;
            return View();
        }
	}
}