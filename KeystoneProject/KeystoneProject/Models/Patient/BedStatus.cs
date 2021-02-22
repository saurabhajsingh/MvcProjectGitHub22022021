using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Patient
{
    public class BedStatus
    {

        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int RoomID { get; set; }
        public string WardID { get; set; }
        public string BedID { get; set; }
        public string WardName { get; set; }
        public string RoomName { get; set; }
        public string BedNO { get; set; }
        public string AdmitDate { get; set; }
        public string RegNO { get; set; }
        public string IPDNO { get; set; }
        public string PatientName { get; set; }
        public string ConsDoctor { get; set; }
        public string ReffDoctor { get; set; }
        public string Deposit { get; set; }
        public string TPAName { get; set; }
        public DataSet dsView { get; set; }
        public string wardcolor { get; set;}
    }
}