using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.FinancialAccountReport
{
    public class MISVoucherEntryWiseReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string ScheduleName { get; set; }
        public string ScheduleID { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }

        public string AccountName { get; set; }
        public string AccountID { get; set; }
        public string VoucherName { get; set; }
        public string VoucherID { get; set; }
        public string TransactionType { get; set; }
        public DataSet dsPatientReport { get; set; }
        public string Particular { get; set; }
    }
}