using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.FinancialAccountReport
{
    public class MISBalanceSheet
    {
        public string HospitalID { get; set; }
        public string LoactionID { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }

        public string ScheduleID { get; set; }
        public string ScheduleName { get; set; }

        public DataSet dsPatientReport { get; set; }
        public string Type { get; set; }
    }
}