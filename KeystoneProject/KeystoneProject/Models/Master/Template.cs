using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class Template
    {
        public string TemplateID { get; set; } 
        public string TemplateName { get; set; } 
        public string TemplateMedicineDetailsID { get; set; }
        public string ChiefComplaint { get; set; }
        public string Allergies { get; set; }
        public string ClinicalFinding { get; set; }
        public string ProvitionalDiagnosis { get; set; }
        public string Investigation { get; set; }
        public string TreatmentAdvice { get; set; }
        public string Medicines { get; set; } 
        public string MedicineLibraryID { get; set; } 
        public string Strength { get; set; }
        public string Frequency { get; set; }
        public string Instruction { get; set; } 
        public string Days { get; set; } 
        public string OldTemplateID { get; set; } 
        public string OldTemplate { get; set; }
    }
}