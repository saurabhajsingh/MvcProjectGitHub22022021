using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;

namespace KeystoneProject.Models.Master
{
    public class Parameter
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        /// 
        [AllowHtml]
         public string FormulaParameterID { get; set; }
         public string Table1 { get; set; }
         public string Table2 { get; set; }
         public string PrecisionParameterID { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string ReferenceCode { get; set; }
        public string PrintAs { get; set; }
        public string Alias { get; set; }
        public string Footer { get; set; }
        public string Method { get; set; }
        public string Unit { get; set; }
        public string DiffCount { get; set; }
        public string SampleType { get; set; }
        public string Precision { get; set; }
        public string Formula { get; set; }
        public string FormulaOLD { get; set; }
        public string FormulaWithShortName { get; set; }
        public string Suffix { get; set; }
        public string SendSMS { get; set; }
        public string GTTGraph { get; set; }
        public string Mode { get; set; }


        public string DaysFrom { get; set; }

        public string DaysTo { get; set; }

        public string Sex { get; set; }

        public string Gender { get; set; }

        public string ConvLow { get; set; }

        public string ConvHigh { get; set; }

        public string ConvNormal { get; set; }

        public string Default { get; set; }

        public string HelpValueID { get; set; }

        public string HelpValue { get; set; }


        public string NormalRangeID { get; set; }

        public string FormulaID { get; set; }

        public string Formulareference { get; set; }

        public string PrecisionID { get; set; }
        public string parametereditor { get; set; }
        public string footerName { get; set; }
     
        public string Description { get; set; }


    }
}
