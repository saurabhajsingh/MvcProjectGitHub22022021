using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Keystone
{
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string ReferenceCode { get; set; }
        public string ISDCode { get; set; }
        public string DateFormat { get; set; }
        public string DateTime { get; set; }
        public string DateSeparator { get; set; }
        public string Century { get; set; }
        public string TimeSeparator { get; set; }
        public string Seconds { get; set; }
        public string BDName { get; set; }
        public string BDAbbreviation { get; set; }
        public string ADName { get; set; }
        public string ADAbbreviation { get; set; }
        public string LacsMillion { get; set; }
        public string NumericSeparator { get; set; }
        public string DecimalSeparator { get; set; }
        public string DecimalDigits { get; set; }
        public string Mode { get; set; }
        public DataSet StoreAllCountry { get; set; }
    }
}