using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class Product
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string productCompany { get; set; }
        public string productName { get; set; }
        public string packing { get; set; }
        public string contains { get; set; }
        public string containsQuantity { get; set; }
        public string sellLoose { get; set; }
        public string generic { get; set; }
        public string scheduled { get; set; }
        public string minimumLevel { get; set; }
        public string maximumLevel { get; set; }
        public string manufacturerName { get; set; }
        
        public string category { get; set; }
        public string SKUCode { get; set; }
        public string HSNSACCode { get; set; }
        public string chapterCode { get; set; }
        public string barCode { get; set; }
        public string extrafield { get; set; }
        public string detailextrafield { get; set; }
        public string discontinueDate { get; set; }
                
        public string gst { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string utgst { get; set; }
        public string cess { get; set; }
        

        public string productCompanyID { get; set; }
        public string productNameID { get; set; }
        public string packingID { get; set; }
        public string genericID { get; set; }
        public string categoryID { get; set; }
        public string HSNSACCodeID { get; set; }
        public string manufacturerNameID { get; set; }
    }
}