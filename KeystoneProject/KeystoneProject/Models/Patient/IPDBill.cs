using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Patient
{
    public class IPDBill
    {
        
        public string DisCountResion { get; set; }
        public string Doctor { get; set; }
        public string txtinput { get; set; }
        public string ForAuthorization { get; set; }
        public string NonMedicalExp { get; set; }
        public string Message { get; set; }
        public string DoctorCharges { get; set; }
        public string flagfinal { get; set; }
        public string HospitalID { get; set; }
        public string LocationID { get; set; }
        public int BillNo { get; set; }
        public string PrintBillNo { get; set; }
        public string BillDate { get; set; }
        public string PatientRegNO { get; set; }
        public string OPDIPDID { get; set; }
        public string BillType { get; set; }
        public string GrossAmount { get; set; }
        public string TaxPercent { get; set; }
        public string TaxAmount { get; set; }
        public string ReffCommission { get; set; }
        public string Commisson { get; set; }
        public string TotalAmount { get; set; }
        public string DiscountPercent { get; set; }
        public string DiscountAmount { get; set; }
        public string DiscountReason { get; set; }
        public string DiscountReasonID { get; set; }
        public string NetPayableAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string PreBalanceAmount { get; set; }
        public string IsPaid { get; set; }
        public string PaidAmount { get; set; }
        public string DipositAmount { get; set; }
        public string IsBillMade { get; set; }
        public string PaymentType { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string BillNoDate { get; set; }
        public string Remarks { get; set; }
        public string SancationAmount { get; set; }
        public string FinancialYearID { get; set; }
        public string CreationID { get; set; }
        public string LastModificationID { get; set; }
        public string CreationDate { get; set; }
        public string BillDateStr { get; set; }
        public string BillTimeStr { get; set; }

        public ServiceName[] Services { get; set; }

    }
    public class PatientSerchDetails
    {
        public string PatientRegNO { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string GuardianName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
      
       
        
        
        
    }
        public class Patient1
    {
       
        public string GrossAmount { get; set; }
        public string NameOfPackege { get; set; }
        public string  PrivalageCardName { get; set; }
        public bool PrivalageCard { get; set; }
        public string TPAName { get; set; }
        public string Address { get; set; }
        public string flagfinal { get; set; }
        public DataTable td { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientRegNO { get; set; }
        public string PatientIPDNO { get; set; }
        public string PatientType { get; set; }
        public string GuardianName { get; set; }
        public string TPA_ID { get; set; }
        public string MobileNo { get; set; }
        public string ConsultantDrID { get; set; }
        public string DoctorPrintName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string ReferredByDoctorID { get; set; }

     
        
        public string DoctorPrintName1 { get; set; }
        public string AdvanceAmount { get; set; }
        public string AddmissionDate { get; set; }
        public string AddmDate { get; set; }
        public string AddmTime { get; set; }
        public string AddmissionType { get; set; }
        public string WardID { get; set; }
        public string WardName { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string BedID { get; set; }
        public string BedName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string PrintRegNO { get; set; }
        public string FinancialYearID { get; set; }
        public string FinancialYear { get; set; }
        public string PrintIPDNO { get; set; }
        public Double PreBalance { get; set; }
        public string PFirstName { get; set; }
        public string PMiddleName { get; set; }
        public string PLastName { get; set; }
        public string PaidAmountProvisnal
        {
            get;
            set;
        }
        public decimal TotalAmount { get; set; }
        public string TaxAmount { get; set; }
        public decimal NetPayableAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string DiscountAmount { get; set; }
        public IPDBill[] Bill { get; set; }

        public ServiceName[] Services { get; set; }


    }
}
