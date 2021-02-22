using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;


namespace KeystoneProject.Models.Patient
{
    public class PreBalanceAmount 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        /// 
        public decimal SecurityDeposityAmt { get; set; }
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int PatientAccountRowID { get; set; }
        public string Bill { get; set; }
        public string SecurityDeposityID { get; set; }

        //public int PatientAccountRowID12 { get; set; }

        [Required(ErrorMessage = "PatientRegNo is Required")]
        public string PatientRegNo { get; set; }
        public string OPDIPDID { get; set; }
        public string PrintOPDIPDNo { get; set; }
        public string PatinetType { get; set; }
        public string PaidAmount { get; set; }
        public string BillDate { get; set; }
        public string PaymentType { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Narrection { get; set; }
        public string Remarks { get; set; }
        public string Mode { get; set; }
        public DataSet StoreAllPreBalance { get; set; }
        public string PatientName { get; set; }
        public string PreBalance { get; set; }
        public string BalanceAmount { get; set; }
        public string OPDIPDNO { get; set; }
        public string BillNo { get; set; }
        public string P_BillNo { get; set; }
        public string P_RegNo { get; set; }
        public string BillType { get; set;}

        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public string PrintRegNo { get; set; }
        public string PrintPaymentTypeCount { get; set; }
        public string chkTPA { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal TPAOtherDeduction { get; set; }
        public string TpaParticular { get; set; }
        public string TPAStatus { get; set; }
        //public DataSet ds1 { get; set; }
    }
}
