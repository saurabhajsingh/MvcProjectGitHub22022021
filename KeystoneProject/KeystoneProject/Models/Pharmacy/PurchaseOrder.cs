using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class PurchaseOrder
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string supplierName { get; set; }
        public string address { get; set; }        
        public string oldBillNumber { get; set; }
        public string orderDate { get; set; }
        public string referenceNumber { get; set; }
        public string referenceDate { get; set; }
        public string valueBy { get; set; }
        public string createEntry { get; set; }
        public string level { get; set; }
        public string payMode { get; set; }
        public string creditDays { get; set; }
        public string grossTotal { get; set; }
        public string addAmount { get; set; }
        public string lessAmount { get; set; }
        public string netAmount { get; set; }        
        public string productName1 { get; set; }        
        public string manufacturerName1 { get; set; }       
        public string packing1 { get; set; }        
        public string currentQuantity1 { get; set; }        
        public string maxLevel1 { get; set; }        
        public string minLevel1 { get; set; }        
        public string quantity1 { get; set; }       
        public string freeQuantity1 { get; set; }        
        public string rate1 { get; set; }
        public string totalAmount1 { get; set; }        
        public string supplierNameID { get; set; }
        public string productDetailsID1 { get; set; }
        public int PoId { get; set; }
        public string productNameID1 { get; set; }        
        public string manufacturerNameID1 { get; set; }
        public string purchaseType { get; set; }
        public string purchaseTypeMode { get; set; }
        public string addProduct { get; set; }

    }
}