using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.FinancialAccountReport
{
    public class MISHospitalProductPurchase
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierID { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }

        public string ProductName { get; set; }
        public string ProductID { get; set; }
        
        public DataSet dsPatientReport { get; set; }
        public string HospitalProductID { get; set; }
       
    }
}