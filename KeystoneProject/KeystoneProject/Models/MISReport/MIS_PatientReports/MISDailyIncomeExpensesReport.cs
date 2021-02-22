using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Controllers.MISReport.MIS_PatientReports;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISDailyIncomeExpensesReport
    {
        public string LocationID { get; set; }
        public string HospitalID { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }

        public string Date { get; set; }
        public string Income { get; set; }
        public string Expences { get; set; }
        public string Balance { get; set; }
        public string bal1 { get; set; }
        public string ExpAmt { get; set; }
        public string IncAmt { get; set; }
    }
}