using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class EndResult
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int EndResultID { get; set; }


        public string EndResultName { get; set; }

        public DataSet StoreAllEndResult { get; set; }
        public string Mode { get; set; }  
    }
}