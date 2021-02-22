using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class SalePurchaseModify
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }

        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string batchNumber { get; set; }
        public string billType { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

        public string tblProductID { get; set; }
        public string tblProductName { get; set; }
        public string tblbatchNumber { get; set; }    
        public string tblbillNo { get; set; }
        public string tblbillDate { get; set; }
        public string tblpurchaseRate { get; set; }
        public string tblquantity { get; set; }
        public string tblamount { get; set; }

    }
}