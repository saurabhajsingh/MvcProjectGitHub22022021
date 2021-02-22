using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class StockStatus
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string allProduct { get; set; }
        public string productName { get; set; }
        public string allProductBatch { get; set; }
        public string productBatch { get; set; }
        public int ProductID { get; set; }
        public int productBatchID { get; set; }
        public string PurchaseRate { get; set; }
        public string CurrentStock { get; set; }
        public string CurrentStock1 { get; set; }
        public string column { get; set; }

    }
}