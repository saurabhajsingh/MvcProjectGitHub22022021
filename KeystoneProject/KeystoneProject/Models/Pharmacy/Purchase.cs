using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class Purchase
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string ProductSupplierName { get; set; }
        public string ProductSupplierID { get; set; }
        public string Address { get; set; }
        public string Balance { get; set; }
        public string BillNoDate { get; set; }
        public string BillNo { get; set; }
        public string ProductDetailsID { get; set; }        
        public string grossTotal { get; set; }
        public string SupplierRemark { get; set; }
        public string billDate { get; set; }
        public string BillDiscountPercent { get; set; }
        public string discountAmt { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string otherAdj { get; set; }
        public string lessCreditDebit { get; set; }
        public string netAmount { get; set; }
        public string billAmount { get; set; }
        public string currentBalance { get; set; }
        public string ProductPurchaseID { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public string PurchaseTaxType { get; set; }
        public string PurchaseTax { get; set; }
        public string BillType { get; set; }
        public string payment_type { get; set; }        
        public string cheque { get; set; }
        public string bankName { get; set; }
        public string chequeDate { get; set; }
        public string Other { get; set; }
        public string Remarks { get; set; }        
        public string TaxRate { get; set; }       
        public string ProductLocationID { get; set; }
        public string ProductLocation { get; set; }
        public string productName1 { get; set; }
        public string ProductID1 { get; set; }
        public string batchNumber1 { get; set; }
        public string HSNSACCode1 { get; set; }
        public string HSNSACCodeID1 { get; set; }
        public string expiry1 { get; set; }
        public string mrp1 { get; set; }
        public string salesRate1 { get; set; }
        public string purchaseRate1 { get; set; }
        public string quantity1 { get; set; }
        public string gst1 { get; set; }
        public string sgst1 { get; set; }
        public string cgst1 { get; set; }
        public string utgst1 { get; set; }
        public string free1 { get; set; }
        public string discount1 { get; set; }
        public string discountSymbol1 { get; set; }
        public string lessBy1 { get; set; }
        public string lessBySymbol1 { get; set; }
        public string totalamount1 { get; set; }
        public string ProductDetailsID1 { get; set; }       


    }
}