using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Pharmacy
{
    public class PurchaseReturn
    {

        public string postingAccountID { get; set; }
        public int ProductSupplierID { get; set; }
        public string ProductSupplierName { get; set; }
        public string BillNo { get; set; }
        public string BillNoAndDate { get; set; }
        public string ProductID { get; set; }


        public string ProductName { get; set; }

        public int ProductDetailsID { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }


        public string PurchaseRate { get; set; }

        public decimal MRP { get; set; }
        public decimal SalesRate { get; set; }

        public string remark { get; set; }
        public string CurrentStock { get; set; }
        public string TaxTypeInformationID { get; set; }
        public string Code { get; set; }
        public string SchemeDiscountInPer { get; set; }
        public string Barcode { get; set; }

        public string AccountsID { get; set; }
        public string AccountName { get; set; }
        public string PrintName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string EmailID { get; set; }

        public string taxType { get; set; }
        public string grossTotal { get; set; }

        public string otherAdj { get; set; }
        public string netAmount { get; set; }
        public string returnDate { get; set; }
        public string refBillDate { get; set; }
        public string refBillNumber { get; set; }

        //public string taxType { get; set; }
        public string oldBillNumber { get; set; }



        public string UserRate { get; set; }
        public string Category { get; set; }
        // public string ProductName { get; set; }
        public string Batch { get; set; }
        //  public string ExpiryDate { get; set; }
        public string Qty { get; set; }
        public string Scheme { get; set; }
        public string DisCountPer { get; set; }
        public string LessByPer { get; set; }
        public string Rate { get; set; }
        public string TotalAmount { get; set; }


    }
}