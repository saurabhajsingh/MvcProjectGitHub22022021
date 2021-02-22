using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace KeystoneProject.Models.Patient
{
    public class OPDBill 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        ///  public string DoctorCharges { get; set; }
        ///  
        ///  
        ///  
        public int PatientIPDNo { get; set; }
        public string PrintRegNO { get; set; }
        public string PrivalageCardName { get; set; }
        public bool PrivalageCard { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Years { get; set; }
        public string ForAuthorization { get; set; }
        public string txtinput { get; set; }
        public string ServiceHSNCode { get; set; }
        public string ServiceGroupCode { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string BillCharges { get; set; }
        public decimal BalencePre { get; set; }
        public string DoctorCharges { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int PatientRegNO { get; set; }
        public int PatientOPDNo { get; set; }
        public int PrintOPDNo { get; set; }
        public Nullable<DateTime> PatientRegistrationDate { get; set; }
        public string RegDate { get; set; }
        public string PatientType { get; set; }
        public int TPA_ID { get; set; }
        public string TPA_Name { get; set; }
        public int DoctorID { get; set; }
        public int Doctor { get; set; }
        public string Rate { get; set; }
        public string Rate1 { get; set; }
        public string ServiceType { get; set; }
        public decimal ReffCommission { get; set; }
        public decimal Commisson { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPrintName { get; set; }
        public string RDoctorPrintName { get; set; }
        public string RDoctorID { get; set; }
        public string DoctorPrintID { get; set; }
        public string DoctorPrintNameID { get; set; }
        public string ServiceGroupName { get; set; }
        public int ServiceGroupID { get; set; }
        public string servicename { get; set; }
        public string servicename1 { get; set; }
        public string ServiceID { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Time { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateOfBirth { get; set; }
        public string BillDate { get; set; }
        public int BillNo { get; set; }
        public string BillNoPrint { get; set; }
        public int BillNoId { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string RefferedDoctor { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
       // public int UnitID { get; set; }
        public int FinancialYearID { get; set; }
        public string FinancialYear { get; set; }
        public string Quantity { get; set; }
        public string Quantity1 { get; set; }
        public int ReferredByDoctorID { get; set; }
        public string Weight { get; set; }
        public string BloodPressure { get; set; }
        public string Reason { get; set; }
        public string RegistrationCharges { get; set; }
        public string ConsultionCharges { get; set; }
       // public string BillCharges { get; set; }
        public string ChargesType { get; set; }
        public string DiscountInPer { get; set; }
       
        public string DiscountInRS { get; set; }
        public string DiscountReasonID { get; set; }
        public decimal PreBalance { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmount1 { get; set; }
        public string HideInBilling { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetPayableAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal grosstotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SerTaxAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal SancationAmount { get; set; }
        public string PaymentType { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string DiscountReason { get; set; }
        public string RemarkCreationID { get; set; }
        public string Remarks { get; set; }
        public int CreationID { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
        public int LastModificationID { get; set; }
        public Nullable<DateTime> LastModificationDate { get; set; }
        public string Mode { get; set; }
        public string TotalDay { get; set; }
        public string TotalMonth { get; set; }
        public string Year { get; set; }
        public string BillNoDate { get; set; }
        public ServiceName[] Services { get; set; }

        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }

        public string MessageError { get; set; }

        //public int DepartmentID { get; set; }
        //public string DepartmentName { get; set; }
        //public int DoctorID { get; set; }
        //public string DoctorName { get; set; }
        public string Discount_Service { get; set; }
        public string DiscountServiceType { get; set; }
    }

    public class ServiceName
    {
        public string Authorization { get; set; }
        public string BillDate { get; set; }
        public string BillNoChangeHeder { get; set; }
        public string BillNo { get; set; }
        public string sevicedisAmt { get; set; }
        public string DiscountServiceType { get; set; }
        public string Discount_Service { get; set; }
        public string Doctorcharges { get; set; }
        public string Dr_charges { get; set; }
        public string ServiceGroupID { get; set; }
        public string SvcName { get; set; }
        public string SvcID { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }

        public string Quantity { get; set; }

        public string Rate { get; set; }
        public string ServiceType { get; set; }
        public string Total { get; set; }

        public string Mode { get; set; }



        public string PBillNo { get; set; }
    }

    public class PatientOPDBill
    {
        public string patientregNo { get; set; }
        public string patientname { get; set; }
        public string address { get; set; }
        public string contactno { get; set; }
        public string PrintRegNO { get; set; }



    }
}
