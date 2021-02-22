using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterFinacialAccounts
{
    public class Schedule
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public int ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public string Nature { get; set; }
        public string ScheduleType { get; set; }
        public string BalanceSheet { get; set; }
        public string BPT { get; set; }
        public string PL { get; set; }
        public string Trading { get; set; }
        public string ReferenceCode { get; set; }
        public string GeneralLedgerPosting { get; set; }
        public string ShowDetailsInReports { get; set; }
        public DataSet StoreAllSchedule { get; set; }
        public string MasterScheduleName {get;set;}
        public string MasterScheduleID { get; set; }


    }
}