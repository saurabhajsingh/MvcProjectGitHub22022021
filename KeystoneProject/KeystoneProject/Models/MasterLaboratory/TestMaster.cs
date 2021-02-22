using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class TestMaster
    {
        public int HospitalID { get; set; }
        //public int LocationID { get; set; }
        public DataSet dsParameterSet { get; set; }
        public string CategoryID { get; set; }
        public string UploadFile { get; set; }
        public DataSet dsGetAllTestMaster { get; set; }
        public string HSNCode { get; set; }
        public string Wordname { get; set; }

        public string GenralWardIDPCharges { get; set; }

        public string EmergencyIPDCharges { get; set; }
        public string ProcessBy { get; set; }
        public string[] HeaderParmater1 { get; set; }
        public string HeaderParmater { get; set; }
        public string OPDGeneralCharges { get; set; }
        public string OPDEmergencyCharges { get; set; }
        public string TestID { get; set; }
        public string TestMasterDetailIPDChargeID { get; set; }
        public string IncludeforTPA { get; set; }
        //  public string HeaderParmater { get; set; }
        public string[] HeaderParameterID { get; set; }
        public string MachineName { get; set; }
        public string Type1 { get; set; }
        public string[] Type { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string[] ParameterHeaderName { get; set; }
        public string[] Space { get; set; }
        public string Space1 { get; set; }
        //public int HospitalID { get; set; }
        public DataSet dsCatgary { get; set; }
        public DataSet dsSampleType { get; set; }
        public DataSet dsGetAllWardName { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
        public DataSet dsAllTestMaster { get; set; }
        public string HeaderName { get; set; }
        public string HeaderID { get; set; }
        public string SampleTypeID { get; set; }
        public int LocationID { get; set; }
        public string WardID { get; set; }
        public string WardName { get; set; }
        public string TestName { get; set; }
        public string ReferenceCode { get; set; }
        public string PrintAs { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public string MyCost { get; set; }
        public string ReffCommission { get; set; }
        public string TestDetailsTPAWiseID { get; set; }
        public string TestDetailsID { get; set; }
        public string TestTPAWiseID { get; set; }
        //  public string [ReffCommission%Rs] { get; set; }
        public string Category { get; set; }
        public string Alias { get; set; }
        public string TestType { get; set; }
        public string Commission { get; set; }

        //  public string Commission%Rs { get; set; }
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


      // public string HeaderName { get; set; }
            //public string HeaderID { get; set; }
        public string ParamiterName { get; set; }


        public string CommissionRs { get; set; }

        public string ReffCommissionRs { get; set; }
    }
}