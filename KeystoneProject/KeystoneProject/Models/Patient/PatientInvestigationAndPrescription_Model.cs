using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Patient
{
    public class PatientInvestigationAndPrescription_Model
    {
        public string PrecriptionID { get; set; }
        public string Dischargetime { get; set; }
        public string DischargeDate { get; set; }
        public string pasthistory { get; set; }
        public string ChiefHistoryWithClinicalFindings { get; set; }
        public string BirthDetails { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public string EndResultID { get; set; }
        public string Age { get; set; }
        public string ChiefHistory { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
        public HttpPostedFileWrapper ImageFile1 { get; set; }
        public string patientipdno { get; set; }
            public int HospitalID { get; set; }
            public int LocationID { get; set; }
            public string OldCheckupAndPrecription { get; set; }
            public string PatientName { get; set; }
            public string GuardianName { get; set; }
            public string PatientIPDNO { get; set; }
            public string AddmissionDate { get; set; }
            public string ConsultantDrID { get; set; }
            public string ConsultantDoctor { get; set; }
            public string DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public string ReferredByDoctorID { get; set; }
            public string ReferredByDoctorName { get; set; }
            public string CheckServiceID { get; set; }
            public string CheckupAndPrecriptionID { get; set; }
            public int OPDIPDID { get; set; }
            public string ReferenceCode { get; set; }
            public int PatientRegNo { get; set; }
            public string PatientType { get; set; }
            public string InvestDate { get; set; }
            public string TreatmentGiven { get; set; }
            public string PreciseHistory { get; set; }
            public string HistoryOfDialysis { get; set; }
            public string Advices { get; set; }
            public string FollowUp { get; set; }

            public string OtherInformation1 { get; set; }
            public string OtherInformation2 { get; set; }
            public string InvestigationAdvice { get; set; } 
          public string CheckupID { get; set; }
            public string Investigation { get; set; }
            public string OtherInformation { get; set; }
            public string ConditionOnDischarge { get; set; }
            public string Urin { get; set; }
            public string BP { get; set; }
            public string Weight { get; set; }
            public string CVS { get; set; }
            public string HGT { get; set; }
            public string CNS { get; set; }
            public string ECG { get; set; }
            public string Allergy { get; set; }
            public string Pulse { get; set; }
            public string RS { get; set; }
            public string Temperature { get; set; }
            public string OtherFinding { get; set; }
            public string Jaundice { get; set; }
            public string PersonalHistory { get; set; }
            public string MedicalHistory { get; set; }
            public string OperativeNotes { get; set; }
            public string NameOfSurgary { get; set; }
            public string ChiefComplaint { get; set; }
            public string HistoryOfChiefComplaint { get; set; }
            public string MentrualHistory { get; set; }
            public string CourseDuringHospitalization { get; set; }
            public int CreationID { get; set; }
            public DateTime CreationDate { get; set; }
            public string InvestigationID { get; set; }
            public string InvestigationName { get; set; }
           
            public string DrugID { get; set; }
            public string DrugName { get; set; }
            public string DrugDescription { get; set; }
            public string DrugTiming { get; set; }
            public string Doses { get; set; }
            public int PatientIPDDischargeICDCodeID { get; set; }
            public string ICDCodeID { get; set; }
            public string ICDCode { get; set; }
            public string ICDName { get; set; }
            public string Mode { get; set; }
            public DataSet TestDS { get; set; }
            public DataSet ProductDS { get; set; }
            public string AdviceID { get; set; }
            public string AdviceName { get; set; }

            public string WardID { get; set; }
            public string WardName { get; set; }
            public string RoomID { get; set; }
            public string RoomNo { get; set; }
            public string BedID { get; set; }
            public string BedNo { get; set; }
            public string BedStatus { get; set; }
            public string EndResult { get; set; }
            public string FinalDiagnosis { get; set; }
            public string Gender { get; set; }
            public int FinancialYearID { get; set; }
            public string MobileNo { get; set; }

            public int PatientLabDetailID { get; set; }
            public string ResultValue { get; set; }
            public string BillDate { get; set; }
            public string BillDateExtra { get; set; }
            public string TestName { get; set; }
          



        // Discharge chk

            public string ConditionOnDischarge1 { get; set; }
            public string Urin1 { get; set; }
            public string BP1 { get; set; }
          
            public string CVS1 { get; set; }
            public string HGT1 { get; set; }
            public string CNS1 { get; set; }
            public string ECG1 { get; set; }
            public string Allergy1 { get; set; }
            public string Pulse1 { get; set; }
            public string RS1 { get; set; }
            public string OtherFinding1 { get; set; }
            public string Temperature1 { get; set; }
            public string Weight1 { get; set; }
            public string Jaundice1 { get; set; }
        //--------------OprativeNote

          
          
           
    }
    public class OPrativeNote
    {
        public string InvestDate { get; set; }
        public int OperativeID { get; set; }
        //public string Image1 { get; set; }
        //public string Image2 { get; set; }

        public string Treatment { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Surgery { get; set; }
        public string Anaesthesia { get; set; }
        public string Surgeons { get; set; }
        public string OperationdDate { get; set; }
        public string Time { get; set; }
        public string Pre_Operative { get; set; }
        public string Side { get; set; }
        public string Cautery { get; set; }
       
        public string Pacho { get; set; }
        public string IOL { get; set; }
        public string Operation { get; set; }
        public string AC_Maintainer { get; set; }
        public string Posterior { get; set; }
        public string Size { get; set; }
        public string Emergency { get; set; }

        public string Anaesthelist { get; set; }
        public string Associate { get; set; }
        public string Conjunctival { get; set; }
        public string Incision { get; set; }
        public string Capsulotomy { get; set; }
        public string Nucleus { get; set; }
        public string Wound { get; set; }

        public string Hydro { get; set; }
        public string Hydro_Dilineation { get; set; }
    }
        public class InvestigationForPrescription
        {
           
            public string[] ServicesID { get; set; }
            public string[] Services { get; set; }
            public string[] serviceRemarks { get; set; }
          
            public string[] TestIDs { get; set; }
            public string[] InvestigationNames { get; set; }
            public string[] Remarks { get; set; }
            public string[] LabNo { get; set; }
            public string[] TestDates { get; set; }
            public string[] DrugIDs { get; set; }
            public string[] AdvicesNames { get; set; }
            public string[] Days { get; set; }
            public string[] Doses { get; set; }
            public string[] FastFoods { get; set; }
            public string[] DoseIDs { get; set; }
            public string[] oldDoseIDs { get; set; }

          
    }


        public class InvestigationForPrescription1
        {
        public string Checkupin { get; set; }
        public string precriptionid { get; set; }
        public string CheckServiceID1 { get; set; }
            public string serviceRemarks { get; set; }
            public string ServicesID { get; set; }
            public string Services { get; set; }
            public string TestIDs { get; set; }
            public string InvestigationNames { get; set; }
            public string Remarks { get; set; }
            public string LabNo { get; set; }
            public string TestDates { get; set; }
            public string DrugIDs { get; set; }
            public string AdvicesNames { get; set; }
            public string Days { get; set; }
            public string Doses { get; set; }
            public string FastFoods { get; set; }
            public string DoseIDs { get; set; }
            public string oldDoseIDs { get; set; }


        }

    public class Discharge
    {

        public string chkDeathSummary { get; set; }
        public int DeathID { get; set; }
        public string EndResult { get; set; }
        public string FinalDiagnosis { get; set; }
        public string Dischargetime { get; set; }
        public string DischargeDate { get; set; }
        public string chkFinalDischarge { get; set; }
        public int TransferID { get; set; }
        public string ExpierdDate { get; set; }
        public string ExpierdTime { get; set; }
        public string Reason { get; set; }
        public string Causer { get; set; }
        public string Medico { get; set; }
        public string TreatmentDuring { get; set; }
        public string Line { get; set; }
        public string OtherInformation { get; set; }
        public string ChiefHistory { get; set; }
        public string Diagnosis { get; set; }
        public string ChiefComplaint { get; set; }
        public string Namesurgery { get; set; }
        public int PatientIPDDischargeID { get; set; }
        public string ICDCodeID { get; set; }
        public string ICDCode { get; set; }
        public string ICDName { get; set; }
        public string ChiefHistoryWithClinicalFindings { get; set; }
       
    }
}