using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientOPD
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientAppointID { get; set; }
        public int PatientRegNo { get; set; }

        public string PatientType { get; set; }

        public int printRegNo { get; set; }

        public string PFPatientName { get; set; }

        public string PatientName { get; set; }
        public string time { get; set; }

        public string PFirstName { get; set; }
        public string MediclaimCardID { get; set; }

        public string PMiddleName { get; set; }

        public string PLastName { get; set; }

        public string ReferenceCode { get; set; }

        public string PFGuardianName { get; set; }

        public string GuardianName { get; set; }


        public string Gender { get; set; }
        public string Age { get; set; }

        public string AadhaarNo { get; set; }
        public string AgeType { get; set; }

        public string DateOfBirth { get; set; }

        public string Height { get; set; }

        public string BloodGroup { get; set; }

        public string CityID { get; set; }

        public string CityName { get; set; }

        public string PinCode { get; set; }

        public string StateID { get; set; }

        public string CountryID { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }

        public string EmailID { get; set; }

        public string Photo { get; set; }

        public string Source { get; set; }

        public HttpPostedFileWrapper ImageFile { get; set; }

        public string FinancialYearID { get; set; }

        public string FinancialYear { get; set; }

        public string Address { get; set; }

        public string PanCardNo { get; set; }
        public string Message { get; set; }



        public string Elective_Surgery { get; set; }

        public string Medical_Management { get; set; }

        public string NameOfPackage { get; set; }
        public string NameOfPackageID { get; set; }
        public string ConsDr1 { get; set; }

        public string PatientCategory { get; set; }

      


        #region OPD Details

        public string PaymentDate
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }

        public string PrefixID
        {
            get;
            set;
        }


        public string PrefixName
        {
            get;
            set;
        }


        public string chkIPDOPD
        {
            get;
            set;
        }


        public string PatientOPDNO { get; set; }

        public string PrintOPDNo { get; set; }

        public string PatientRegistrationDate { get; set; }

        public string RegistrationTime { get; set; }

        public string TPA_ID { get; set; }

        public string DoctorID { get; set; }

        public string ConsultantDrID { get; set; }

        public string TemporaryDoctor { get; set; }

        public string RefferedTemporaryDoctor { get; set; }

        public string DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public string ReferredByDoctorID { get; set; }

        public string ReferredByDoctorName { get; set; }

        public string DoctorPrintName { get; set; }


        public string Weight { get; set; }

        public string BloodPressure { get; set; }

        public string Reason { get; set; }


        public string RegistrationCharge { get; set; }
        public string ConsultionCharges
        {
            get;
            set;
        }
        public string DiscountInPer
        {
            get;
            set;
        }
        public string DiscountInRS
        {
            get;
            set;
        }
        public string TotalAmount
        {
            get;
            set;
        }
        public string PaymentType
        {
            get;
            set;
        }
        public string Number
        {
            get;
            set;
        }


        public string NewOrRevisit
        {
            get;
            set;
        }

        public string day
        {
            get;
            set;
        }
        public string Month
        {
            get;
            set;
        }
        public string year
        {
            get;
            set;
        }

        public string Date
        {
            get;
            set;
        }

        public string DiscountReason
        {
            get;
            set;
        }




        public string Name
        {
            get;
            set;
        }

        #endregion


        #region IPDDetails


        // public string TPAID
        //{
        //    get;
        //    set;
        //}

        public string TPA_Name
        {
            get;
            set;
        }
        public string TPAStatus
        {
            get;
            set;
        }



        public string CCNClaimNo
        {
            get;
            set;
        }
        public string InsuranceCompny
        {
            get;
            set;
        }
        public string InsuranceCompnyID
        {
            get;
            set;
        }
        public string PolicyNo
        {
            get;
            set;
        }



        public string AddmissionType
        {
            get;
            set;
        }
        public DateTime AddmissionDate
        {
            get;
            set;
        }



        public DateTime EnterDateTime
        {
            get;
            set;
        }

        public bool IsCurrentBed
        {
            get;
            set;
        }


        public string OrganizationID
        {
            get;
            set;
        }

        public string OrganizationName
        {
            get;
            set;
        }


        public string OrganisationName
        {
            get;
            set;
        }





        public string WardName
        {
            get;
            set;
        }
        public string WardID
        {
            get;
            set;
        }
        public string RoomID
        {
            get;
            set;
        }
        public string RoomNo
        {
            get;
            set;
        }
        public string BedID
        {
            get;
            set;
        }
        public string BedNO
        {
            get;
            set;
        }

        public string BedCharges
        {
            get;
            set;
        }

        public string PatientIPDNO
        {
            get;
            set;
        }

        public string PrintIPDNO
        {
            get;
            set;
        }

        public string AdvanceAmt
        {
            get;
            set;
        }

        public string AdmissionCharges
        {
            get;
            set;
        }






        public string DiscountAmount
        {
            get;
            set;
        }



        public string PaidAmount
        {
            get;
            set;
        }

        public string BalanceAmount
        {
            get;
            set;
        }

        public string SecurityDeposityID
        {
            get;
            set;
        }

        public string CrAmount
        {
            get;
            set;
        }

        public string DrAmount
        {
            get;
            set;
        }

        public string BillType
        {
            get;
            set;
        }

        public string PrintSecurityDeposite
        {
            get;
            set;
        }

        public int OPDIPDID
        {
            get;
            set;
        }


        public string PatientCancel
        {
            get;
            set;
        }

        public string PatientCancelReason
        {
            get;
            set;
        }



        public string TemporaryDoctorStatus
        {
            get;
            set;
        }


        public string RefferedTemporaryDoctorStatus
        {
            get;
            set;
        }

        public string ApplyWardID
        {
            get;
            set;
        }


        public string ApplyWardName
        {
            get;
            set;
        }

        public string PatientTypeID { get; set; }

       
        #endregion


        public string TPARate { get; set; }
        public string GTotal  { get; set; }
        public double ServiceTax { get; set; }
        public double TaxAmount { get; set; }
        public double Discount { get; set; }
        public double NetPayableAmount { get; set; }
        public double BalAmount { get; set; }
        public double PreBalance { get; set; }

    }
}
