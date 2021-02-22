using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.FinancialAccountReport
{
    public class MISProfitAndLossReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string ScheduleID { get; set; }
        public string ScheduleName { get; set; }

        public DataSet dsPatientReport { get; set; }
        public string Type { get; set; }
        public string DiscountAmount { get; set; }
        public string Total { get; set; }
        public string AccountName { get; set; }
        public string AccountsID { get; set; }
        public string DrAmount { get; set; }
        public string CrAmount { get; set; }
        public string Particular { get; set; }
        public string Narration { get; set; }
        public string TransactionType { get; set; }
        public string ServiceGroupName { get; set; }
        public string TotalAmount{get;set;}
    }
}