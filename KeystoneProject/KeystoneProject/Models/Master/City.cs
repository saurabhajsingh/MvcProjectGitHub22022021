using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Keystone
{
    public class City
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string ReferenceCode { get; set; }
        public string Mode { get; set; }
        public DataSet StoreAllCity { get; set; }
    }
}