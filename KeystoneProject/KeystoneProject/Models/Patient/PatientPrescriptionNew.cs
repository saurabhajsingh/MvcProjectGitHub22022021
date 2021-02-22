using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Patient;


namespace KeystoneProject.Models.Patient
{
    public class PatientPrescriptionNew
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        /// 
         
        public string Tabsection1 { get; set; }
        public string Tabsection2 { get; set; }
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int PatientRegNO { get; set; }
        public int PrintRegNO { get; set; }
        public string PatientPrescriptionNewID { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public int OPDIPDID { get; set; }
        public string Message { get; set; }

        public string Age { get; set; }
        public string AgeType { get; set; }
        public string Gender { get; set; }
        public string Date { get; set; }

        public int ChiefComplaintID { get; set; }

        [AllowHtml]
        public string txtChiefComplaint { get; set; }

        [AllowHtml]
        public string txtAllergies { get; set; }
        [AllowHtml]
        public string txtClinicalFinding { get; set; }
        [AllowHtml]
        public string txtProvitionalDiagnosis { get; set; }
        [AllowHtml]
        public string txtInvestigations { get; set; }
        [AllowHtml]
        public string txtTreatmentAdvice { get; set; }
        [AllowHtml]
        public string txtChiefComplaintName { get; set; }
        public string txtAllergiesName { get; set; }
        public string txtClinicalFindingName { get; set; }
        public string txtProvitionalDiagnosisName { get; set; }
        public string txtInvestigationsName { get; set; }
        public string txtTreatmentAdviceName { get; set; }
        public string ChiefComplaint { get; set; }
        public int AllergiesID { get; set; }
        public string Allergies { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Pulse { get; set; }
        public string BP { get; set; }
        public string BMI { get; set; }
        public string Temp { get; set; }

        public int ClinicalFindingsID { get; set; }
        public string ClinicalFindings { get; set; }

        public int ProvisionalDiagonosisID { get; set; }
        public string ProvisionalDiagonosis { get; set; }

        public int InvestigationsID { get; set; }
        public string Investigations { get; set; }

        public int TreatmentAdviceID { get; set; }
        public string TreatmentAdvice { get; set; }
        public string Remark { get; set; }
        public string FollowUpTime { get; set; }
        public string FollowUpDate { get; set; }
        //    public string Medicine { get; set; }
        public string Strength { get; set; }
        public string Frequency { get; set; }
        public string Instruction { get; set; }

        public string Days { get; set; }

        public int CreationID { get; set; }

        public string MedicineLibraryID { get; set; }
        public string Medicines { get; set; }

        public string PackagesID { get; set; }
        public string PackagesName { get; set; }

        public string AdviceID { get; set; }
        public string AdviceName { get; set; }


        public PatientInvestigationAndPrescription[] InvestigationArray { get; set; }

        public PatientPrescriptionMedicine[] MedicineArray { get; set; }
        public DataSet Showtest { get; set; }

        public DataSet ShowTreatmentAdvice { get; set; }
        public DataSet ShowProvisionalDiagonosis { get; set; }
        public DataSet ShowChiefHistory { get; set; }
        public DataSet ShowAllergies { get; set; }

        public DataSet ShowChiefComplaint { get; set; }
        public string PatientPrescriptionNewInvestigationID { get; set; }

        public string PatientPrescriptionNewMedicineID { get; set; }


        public string OldBillNo { get; set; }

        public string BillNo { get; set; }
        public string ICDCodeID { get; set; }
        public string ICDName { get; set; }
        public string ICDCode { get; set; } 
        public string icdcodename { get; set; } 
        public string ICDCodeIDhidden { get; set; }

        #region Consultant doctor history

        #endregion


        public int PatientRegNO1 { get; set; }
        public string PatientPrescriptionNewID1 { get; set; }
        public string PatientName1 { get; set; }
        public string GuardianName1 { get; set; }
        public int OPDIPDID1 { get; set; }
        public string Message1 { get; set; }
        public string Age1 { get; set; }
        public string AgeType1 { get; set; }
        public string Gender1 { get; set; }
        public string Date1 { get; set; }

        public string Height1 { get; set; }
        public string Weight1 { get; set; }
        public string Pulse1 { get; set; }
        public string BP1 { get; set; }
        public string BMI1 { get; set; }
        public string Temp1 { get; set; }

          [AllowHtml]
        public string ChiefComplaint1 { get; set; }
          [AllowHtml]
        public string Allergies1 { get; set; }
          [AllowHtml]
        public string ClinicalFinding1 { get; set; }
          [AllowHtml]
        public string ProvitionalDiagnosis1 { get; set; }
          [AllowHtml]
        public string Investigation1 { get; set; }
          [AllowHtml]
        public string TreatmentAdvice1 { get; set; }
        public string MedicineLibraryID1 { get; set; }
        public string Remark1 { get; set; }
        public string FollowUpTime1 { get; set; }
        public string FollowUpDate1 { get; set; }
        public string Medicines1 { get; set; }
        public string Strength1 { get; set; }
        public string Frequency1 { get; set; }
        public string Instruction1 { get; set; }
        public string Days1 { get; set; }

    }

    public class report
    {
        public DataSet dsReport { get; set; }
    }

    public class PatientPrescriptionNew1
    {
    

    }
}
