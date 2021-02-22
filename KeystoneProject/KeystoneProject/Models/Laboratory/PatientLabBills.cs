using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Laboratory
{
    public class PatientLabBills
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        /// 
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OPDLabBills { get; set; }
        public string Approved { get; set; }
        public string Completed { get; set; }
        public string Incomplete { get; set; }
        public string PatientLabName { get; set; }
        public string PrefixID { get; set; }
        public string PrefixName { get; set; }
        public string PatientLabAppointmentID { get; set; }
        public string PatientLabAppointmentDetailID { get; set; }
        public string SampleCollectionBoyID { get; set; }
        public string ConsultantDr { get; set; }
        public string RefferedDr { get; set; }
        public string SampleCollectionBy { get; set; }
        public string AppointmentDate{get;set;}
        public string Discount_Service { get; set; }
        public string ForAuthorization { get; set; }
        public string DiscountServiceType { get; set; }
        public decimal ServiceTotal { get; set; }
        public string ReportingDate { get; set; }
        public string TpaID { get; set; }
        public string OutSourceID { get; set; }
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int CollectionCentreID { get; set; }
        public string CollectionCentreName { get; set; }
        public int OutSourceLabID { get;set;}
        public int ProfileID { get; set; }
        public string Date { get; set; }
        public string ProfileName { get; set; }
        public string LabName { get; set; }
        public string ReffDoctorName { get; set; }
        public string FullName { get; set; }
        public string UserID { get; set; }
        public string Number { get; set; }
        public string txtinput { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string PrintRegNO { get; set; }
        public  string PaymentDate { get; set; }
        public string Rate { get; set; }
        public string OLDBill { get; set; }

        //  public DataSet dsOLDBill { get; set; }

        public string Qty { get; set; }

        public string PatientRegNo { get; set; }
        public string Mode { get; set; }

        public int CityID { get; set; }

        public int StateID { get; set; }

        public int CountryID { get; set; }
        public int PinCode { get; set; }
       
        public string EmailID { get; set; }

        public int PhoneNo { get; set; }


        public string PatientName { get; set; }

        public string GuardianName { get; set; }
        public string PatientType { get; set; }
        public string Age { get; set; }
        public string AgeType { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public string Years { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }


        public string TotalDay { get; set; }





        public string MobileNo { get; set; }


        public string Address { get; set; }
        public string PatientIPDNO { get; set; }
        public string ConsultantDrID { get; set; }
        public string ReferredByDoctorID { get; set; }
        public string WardID { get; set; }

        public string WardName { get; set; }
        public string RoomID { get; set; }

        public string RoomName { get; set; }
        public string BedID { get; set; }
        public int BedNo { get; set; }
        public string AddmissionType { get; set; }
        public string PatientOPDNO { get; set; }

        public string UnitID { get; set; }
        public decimal ServiceTax { get; set; }
        public string Discount { get; set; }
        public string DiscountAmount { get; set; }
        public int DiscountReasonID { get; set; }
        public string DiscountReason { get; set; }
        public string ChargesType { get; set; }

        public string ServiceType { get; set; }

        public string HideInBilling { get; set; }
        public string BillDate { get; set; }
        public string BillDatetime { get; set; }

        public string BillType { get; set; }

        public decimal GrossAmount { get; set; }
        public decimal TaxPercent { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal NetPayableAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        public bool IsPaid { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal PreBalanceAmount { get; set; }
        public decimal Deposit { get; set; }

        public string Remarks { get; set;}
        public string PaymentType { get; set; }
        public int FinancialYearID { get; set; }

        public string FinancialYear { get; set; }
        ////////////////////////  PatientLab  //////////////////////
        public string samplecollection { get; set; }
        public int LabNo { get; set; }
        public string OPDIPDID { get; set; }

        public string LabType { get; set; }

        public string BillNo { get; set; }

        public int DoctorID { get; set; }

        public string DoctorPrintName { get; set; }

        ////////////////////////  PatientLabDetails  //////////////////////

        public int PatientLabDetailID { get; set; }
        public string TestID { get; set; }

        public string TestName { get; set; }

        public string TestStatus { get; set; }

        public DateTime SampleCollectionDate { get; set; }

        public int AuthorizedID { get; set; }

        public DateTime AuthorizedDate { get; set; }

        public bool CompleteBy { get; set; }

        public DateTime CompleteDate { get; set; }


        public DateTime PrintDate { get; set; }

        public string GeneralCharges { get; set; }


        public string EmergencyCharges { get; set; }

        public string PrintOPDIPDNo { get; set; }
        // public DataSet dsPatientLab { get; set; }

        // public DataSet RptPatientLabBill { get; set; }


        public bool mybool { get; set; }
        public string PrivilegeCard { get; set; }
        public string TesttableID { get; set; }
        public string CreationID { get; set; }
     
    }

}
