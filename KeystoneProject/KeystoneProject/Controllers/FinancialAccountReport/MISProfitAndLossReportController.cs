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
    public class MISProfitAndLossReportController : Controller
    {
        int HospitalID;
        int LocationID;
        int UserID;
        int ScheduleID = 0;
        string ScheduleName;
        string Type;
        Decimal TotalAmount = 0;

        BL_MISProfitAndLossReport objbl = new BL_MISProfitAndLossReport();
        MISProfitAndLossReport objmodels = new MISProfitAndLossReport();
        public void HospitlLocationID()
        {
            HospitalID = Convert.ToInt32(HttpContext.Session["HospitalID"]);
            LocationID = Convert.ToInt32(HttpContext.Session["LocationID"]);
            UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
        }
        //
        // GET: /MISProfitAndLossReport/

        [HttpGet]
        public ActionResult MISProfitAndLossReport()
        {
            return View();
        }
        public static Nullable<DateTime> DateFrom { get; set; }
        public static Nullable<DateTime> DateTo { get; set; }

        public JsonResult GetExpensesDetails(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {

            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();
            try
            {
                //Get Direct Expenses  
                ScheduleID = 0;
                Type = "EXPENSES";
                ScheduleName = "";
                DataSet ds = new DataSet();

                ScheduleID = 0;
                Type = "INDIRECT EXPENSES";
                ScheduleName = "";
                DataSet ds1 = new DataSet();
                string[] Typearrsy = { "EXPENSES", "INDIRECT EXPENSES" };

                for (int a = 0; a < Typearrsy.Length;a++ )
                {
                    ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Typearrsy[a], DateFrom, DateTo);

                    foreach (DataRow drExpenses in ds.Tables[0].Rows)
                    {
                        
                        TotalAmount = 0;
                        drExpenses["Total"] = Math.Round(GetAccountTotalExepence(Convert.ToInt32(drExpenses["ScheduleID"].ToString()), "EXPENSES", DateFrom, DateTo), 2);
                       
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lists.Add(new MISProfitAndLossReport
                            {
                                Type=Typearrsy[a],
                                ScheduleID = dr["ScheduleID"].ToString(),
                                ScheduleName = dr["ScheduleName"].ToString(),
                                Total = dr["Total"].ToString()


                            });
                        }
                    }

                }
                ScheduleID = 0;
                Type = "INCOME";
                ScheduleName = "";
                DataSet ds2 = new DataSet();
                string[] Typearrsy1 = { "INCOME", "HOSPITAL SALES TOTAL", "INDIRECT INCOME" };
                for (int i = 0; i < Typearrsy1.Length; i++)
                {
                    ds2 = objbl.GetIncomeDetails(ScheduleID, ScheduleName, Typearrsy1[i], DateFrom, DateTo);
                    foreach (DataRow drINCOME in ds.Tables[0].Rows)
                    {
                        TotalAmount = 0;
                        drINCOME["Total"] = Math.Round(GetAccountTotalExepence(Convert.ToInt32(drINCOME["ScheduleID"].ToString()), "INCOME", DateFrom, DateTo), 2);
                        
                    }
                    if (ds2.Tables[0].Rows.Count >0)
                    {
                        foreach (DataRow dr in ds2.Tables[0].Rows)
                        {
                            lists.Add(new MISProfitAndLossReport
                            {
                                Type = Typearrsy1[i],
                                ScheduleID = dr["ScheduleID"].ToString(),
                                ScheduleName = dr["ScheduleName"].ToString(),
                                Total = dr["Total"].ToString()
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public decimal GetAccountTotalExepence(int ScheduleTotalID, string Nature, DateTime DateFrom, DateTime DateTo)
        {
            Type = "DETAILS";
            ScheduleName = "";
            DataSet ds1 = new DataSet();

            if (Nature == "EXPENSES" || Nature == "INDIRECT EXPENSES")
            {
                ds1 = objbl.GetExpensesDetails(ScheduleTotalID, ScheduleName, Type, DateFrom, DateTo);
            }

            if (Nature == "INCOME" || Nature == "INDIRECT INCOME")
            {

                ds1 = objbl.GetIncomeDetails(ScheduleTotalID, ScheduleName, Type, DateFrom, DateTo);
            }


            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    GetAccountTotalExepence(Convert.ToInt32(dr["ScheduleID"]), Nature,DateFrom,DateTo);
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

        public JsonResult DirectExpensesDataModal(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  
               
                Type = "DETAILS";
                ScheduleName = "";
                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            lists.Add(new MISProfitAndLossReport
                            {
                                
                                ScheduleID = dr["ScheduleID"].ToString(),
                                AccountName = dr["AccountName"].ToString(),
                                Total = dr["Total"].ToString(),
                                AccountsID = dr["AccountsID"].ToString(),
                            });
                        }
                    }
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


           
        }

        public JsonResult PurchaseAccountsDataModal(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";
                ScheduleName = "";
                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            ScheduleID = dr["ScheduleID"].ToString(),
                           
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult PurchaseAccountsModalDataModal(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";
                ScheduleName = "";
                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            AccountsID = dr["AccountsID"].ToString(),
                            AccountName = dr["AccountName"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult GetTrialBalanceDetails(DateTime FromDate, DateTime ToDate, string AccountsID, string Type, int ScheduleID)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "Acount";
                
                DataSet ds = new DataSet();

                ds = objbl.GetTrialBalance(FromDate, ToDate, AccountsID, Type, ScheduleID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            AccountName = dr["AccountName"].ToString(),
                            DrAmount = dr["DrAmount"].ToString(),
                            CrAmount = dr["CrAmount"].ToString(),
                            Particular = dr["Particular"].ToString(),
                            Narration = dr["Narration"].ToString(),
                            TransactionType = dr["TransactionType"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult IndirectExpencesClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";
              
                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                foreach (DataRow drExpenses in ds.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drExpenses["Total"] = GetAccountTotalExepence(Convert.ToInt32(drExpenses["ScheduleID"].ToString()), "INDIRECT EXPENSES", DateFrom, DateTo);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            ScheduleID = dr["ScheduleID"].ToString(),
                           
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult ConsumableClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";

                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                foreach (DataRow drExpenses in ds.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drExpenses["Total"] = GetAccountTotalExepence(Convert.ToInt32(drExpenses["ScheduleID"].ToString()), "INDIRECT EXPENSES", DateFrom, DateTo);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            AccountName = dr["AccountName"].ToString(),
                            AccountsID = dr["AccountsID"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult DailyExpencesClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";

                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                foreach (DataRow drExpenses in ds.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drExpenses["Total"] = GetAccountTotalExepence(Convert.ToInt32(drExpenses["ScheduleID"].ToString()), "INDIRECT EXPENSES", DateFrom, DateTo);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            
                            ScheduleID = dr["ScheduleID"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult SalaryExpencesClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";

                DataSet ds = new DataSet();

                ds = objbl.GetExpensesDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                foreach (DataRow drExpenses in ds.Tables[0].Rows)
                {
                    TotalAmount = 0;
                    drExpenses["Total"] = GetAccountTotalExepence(Convert.ToInt32(drExpenses["ScheduleID"].ToString()), "INDIRECT EXPENSES", DateFrom, DateTo);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            AccountsID = dr["AccountsID"].ToString(),
                            AccountName = dr["AccountName"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult SalesAccountClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "HOSPITAL SALES";

                DataSet ds = new DataSet();

                ds = objbl.GetIncomeDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            DiscountAmount = dr["DiscountAmount"].ToString(),
                            AccountName = dr["AccountName"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleName = dr["ScheduleName"].ToString(),
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult DirectIncomeClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  

                Type = "DETAILS";

                DataSet ds = new DataSet();

                ds = objbl.GetIncomeDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {                           
                          
                            ScheduleName = dr["ScheduleName"].ToString(),
                            Total = dr["Total"].ToString(),
                            ScheduleID = dr["ScheduleID"].ToString(),
                            
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult IPDClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  
                int a = 0;
                Type = "HOSPITAL SALES Detail";
              
                DataSet ds = new DataSet();
               
                ds = objbl.GetIncomeDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                for (int i = 0; i < ds.Tables.Count; i++)
                {


                    string[] Type1 = { "OPD", "IPD", "LAB" };
                    if (ds.Tables[i].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[i].Rows)
                        {
                            lists.Add(new MISProfitAndLossReport
                            {
                                Type = Type1[i].ToString(),
                                ServiceGroupName = dr["ServiceGroupName"].ToString(),
                                TotalAmount = dr["Total Amount"].ToString(),


                            });
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public JsonResult IndirectIncomeClick(int ScheduleID, string ScheduleName, string Type, DateTime DateFrom, DateTime DateTo)
        {
            List<MISProfitAndLossReport> lists = new List<MISProfitAndLossReport>();

            try
            {
                //Get Direct Expenses  
                int a = 0;
                Type = "DETAILS";

                DataSet ds = new DataSet();

                ds = objbl.GetIncomeDetails(ScheduleID, ScheduleName, Type, DateFrom, DateTo);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lists.Add(new MISProfitAndLossReport
                        {

                            ScheduleName = dr["ScheduleName"].ToString(),
                            AccountName = dr["AccountName"].ToString(),
                            Total = dr["Total"].ToString(),
                            AccountsID = dr["AccountsID"].ToString(),

                        });
                    }
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = lists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }

        public ActionResult RptProfitAndLossReport()
        {
          
            return View();
        }
    }
}