using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class PatientMedicalBill
    {
        public string ExparyDateColor { get; set; }

        public Decimal Contain { get; set; }
        public string ProductDetailsID { get; set; }
        public string expiryDate { get; set; }
        public string mrp { get; set; }
        public string netRate { get; set; }
        public string grossTotal { get; set; }
        public string totalAmount { get; set; }
        public string discountAmount { get; set; }
        public string discountPercentage { get; set; }
        public string vatAmount { get; set; }
        public string otheLess { get; set; }
        public string salesReturn { get; set; }
        public string netAmount { get; set; }
        public string paymentType { get; set; }
        public string cashRecipt { get; set; }
        public string scanBarcode { get; set; }
        public string barcodeNumber { get; set; }        
        public decimal Gst { get; set; }
        public string DiscountValue { get; set; }
        public string BillNoDate { get; set; }
        public string BillNo { get; set; }
        public string message { get; set; }
        public string PatientType { get; set; }
        public string SalesPersonName { get; set; }
        public string SalesPersonID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public string Prod_discountPercentage { get; set; }
        public string freeQuantity { get; set; }
       
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPrintName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string DoctorID { get; set; }
        public string DoctorPrintName { get; set; }
        public string PatientRegNO { get; set; }
        public string PatientName { get; set; }
        public string batchNumber { get; set; }
        public string QtyU { get; set; }
        public string QtyL { get; set; }

        public int MedicalBillID { get; set; }
        public int PatientBillsDetailsMedicalID { get; set; }

        public string tblProductName { get; set; }
        public string tblProductID { get; set; }
        public string tblbatchNumber { get; set; }
        public string tblmrp { get; set; }
        public string tblexpiryDate { get; set; }
        public string tblnetRate { get; set; }
        public string tblQtyU { get; set; }
        public string tblQtyL { get; set; }
        public string tblGST { get; set; }
        public string tbldiscountPercentage { get; set; }
        public string tbltotalAmount { get; set; }
        public string tblDiscountValue { get; set; }
        public string tblfreeQuantity { get; set; }

        public string Number { get; set; }
        public string Name { get; set; }
        public string paymentDate { get; set; }
        public string Remarks { get; set; }


    }
}