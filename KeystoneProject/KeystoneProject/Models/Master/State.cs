using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Keystone
{
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string ReferenceCode { get; set; }
        public string Mode { get; set; }

        public DataSet StoreAllState { get; set; }

    }
}