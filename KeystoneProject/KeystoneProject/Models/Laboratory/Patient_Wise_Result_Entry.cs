using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Laboratory
{
    public class Patient_Wise_Result_Entry
    {
        public string AuthorzerID { get; set; }
        public string AuthorzeName { get; set; }
        public string btnsaveEdit { get; set; }
        public string Message { get; set; }
        public string cmbStatus { get; set; }

        public string txtRemark { get; set; }
        public string footer { get; set; }
       
        public string Color { get; set; }
        public string PrintRegNO { get; set; }
        public string UserID { get; set; }
        public string AuthoriserName { get; set; }
        public string Remark { get; set; }

        public string TestID { get; set; }
        public string TestName1 { get; set; }

        public string Teststatus { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }


        public string ParamiterID { get; set; }
        public string ResultValue { get; set; }
        public string status { get; set; }

        public DataSet dsParameterSet { get; set; }
        public string Patientname { get; set; }
        public string RegNo { get; set; }

        public DataSet dsGetAllTestMaster { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
        public string Formula { get; set; }

        public string LabNo { get; set; }

        public string PatientType { get; set; }

        public string LabType { get; set; }

        public string LabDate { get; set; }
        public string ConsDr { get; set; }

        public string Testname { get; set; }
        public string ParamiterName { get; set; }
        public string NLH { get; set; }

        public string Low { get; set; }
        public string Value { get; set; }

        public string Hiegh { get; set; }
        public string FullName { get; set; }

        public string HelpValue { get; set; }
        public string HelpValueID { get; set; }



    }
    public class FooterData
    {
        public string HLN { get; set; }
        public string LNo { get; set; }
        public string PreID { get; set; }
        public string Formu { get; set; }
        public string[] footer { get; set; }
        public string Nyvalue { get; set; }
        public string TesID { get; set; }
    }
}
