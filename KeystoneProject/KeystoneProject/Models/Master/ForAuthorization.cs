using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class ForAuthorization
    {
        public string CrAmount
        { get; set; }
        public string DrAmount
        { get; set; }


        public string BeforeData
        { get; set; }
        public string AfterData
        { get; set; }
        public string Rowstatus
        { get; set; }
        public decimal TotalAmount
        { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DataSet StoreAllForAuthorization { get; set; }
        public string AuthorizationID { get; set; }
        public string UserName { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string BillNo { get; set; }
        public string PatientAccountRowID { get; set; }
        public string BillType { get; set; }
        public string Date { get; set; }
        public string AuthorationReason { get; set; }
        public string Authorise { get; set; }


        public string SrNo { get; set; }
      
        public string ServiceName { get; set; }
        public string Rate { get; set; }
        public string Quantity { get; set; }
        public string TotalAmt { get; set; }
        public string ServiceType { get; set; }

        
    }
}