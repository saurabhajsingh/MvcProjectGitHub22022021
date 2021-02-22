using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class SampleType 
    {
       
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int SampleTypeID { get; set; }      
        public string SampleTypeName { get; set; }
        public string ReferenceCode { get; set; }
        public DataSet StoreAllSampleType { get; set; }
    }
}
