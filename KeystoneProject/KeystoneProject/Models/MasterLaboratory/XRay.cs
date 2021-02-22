using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class XRay
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }      
        public DataSet dsAllTestMaster { get; set; }
        public string WardID { get; set; }
        public string WardName { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public string TestID { get; set; }
        public string ReportName { get; set; }
        public string HSNCode { get; set; }
        public string PrintAs { get; set; }
        public string OPDGeneralCharges { get; set; }
        public string OPDEmergencyCharges { get; set; }
        public string ReffCommission { get; set; }
        public string ReffCommissionRs { get; set; }
        public string Alias { get; set; }
        public string TestType { get; set; }
        public string Commission { get; set; }
        public string Client { get; set; }
        public string ForGender { get; set; }
        public string Format { get; set; }
        public string ProcessDay { get; set; }
        public string PrintOrder { get; set; }
        public string Methord { get; set; }
        public string OutsideLab { get; set; }
        public string DisposalDay { get; set; }
        public string Discount { get; set; }
        public string SampleType { get; set; }
        public string PrintName { get; set; }
        public string PrintTogether { get; set; }
        public string WardView { get; set; }
        public string Interpretation { get; set; }
        public string HMSCode { get; set; }
        public string PatientInstr { get; set; }
        public string Footer { get; set; }
        public string TechnicianIntr { get; set; }
        public string BothFooter { get; set; }
        public string MachinName { get; set; }
        public int CreationID { get; set; }
        public string Mode { get; set; }
        public string ProcessBy { get; set; }
        public string CommissionRs { get; set; }
        public string TestMasterDetailIPDChargeID { get; set; }
        public string Description { get; set; }
        public string GenralWardIDPCharges { get; set; }
        public string EmergencyIPDCharges { get; set; }
        public string TestTPAWiseID { get; set; }
        public string unit { get; set; }
        public DataSet dsGetAllWardName { get; set; }
    }
}