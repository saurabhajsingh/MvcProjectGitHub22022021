using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Pharmacy
{
    public class PatientMedicalReturn
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }

        public int patientRegNo { get; set; }
        public string patientName { get; set; }
        public string PatientType { get; set; }
        public string doctorName { get; set; }
        public int doctorID { get; set; }
        public string salesPerson { get; set; }
        public int salesPersonID { get; set; }
        public string oldBillNumber { get; set; }
        public int referenceNumber { get; set; }
        public string address { get; set; }
        public string customerName { get; set; }
        public int customerID { get; set; }
        public string referenceDate { get; set; }
        public string remark { get; set; }


        public int grossTotal { get; set; }
        public int discountPercentage { get; set; }
        public int netAmount { get; set; }
        public int totalAmount { get; set; }
        public int vatAmount { get; set; }
        public string postingAccount { get; set; }
        public int discountAmount { get; set; }
        public int otherLess { get; set; }


        public string barCode { get; set; }
        public string productName { get; set; }
        public int productID { get; set; }
        public string batchNumber { get; set; }
        public string ProductDetailsID { get; set; }
        public string expiry { get; set; }
        public int mrp { get; set; }
        public int netRate { get; set; }
        public int prod_discPercentage { get; set; }
        public string QtyU { get; set; }
        public string QtyL { get; set; }
        public int freeQuantity { get; set; }


        public int medicalReturnID { get; set; }
        public int medicalReturnDetailID { get; set; }


        public string tblProductName { get; set; }
        public string tblProductID { get; set; }
        public string tblbatchNumber { get; set; }
        public string tblexpiryDate { get; set; }
        public string tblmrp { get; set; }
        public string tblnetRate { get; set; }
        public string tblQtyU { get; set; }
        public string tblQtyL { get; set; }
        public string tblfreeQuantity { get; set; }
        public string tbldiscountPercentage { get; set; }
        public string tbltotalAmount { get; set; }
    }
}