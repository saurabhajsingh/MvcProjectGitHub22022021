using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientImageManager
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string CasePaperID { get; set; }
        public int PatientRegNo { get; set; }
        public int OPDIPDID { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public string PatientType { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Paper { get; set; }
        public string PaperName { get; set; }
        public string PaperType { get; set; }
        public string PaperPath { get; set; }

        public HttpPostedFileWrapper ImageFile { get; set; }
        public string CreationID { get; set; }
        public string CreationDate { get; set; }
        public string LasteModificationID { get; set; }
        public string LasteModificationDate { get; set; }
        public string ConsultantDrID { get; set; }
        public string ConsultantDr { get; set; }

        public string ReferredByDoctorID { get; set; }
        public string ReferredByDoctor { get; set; }
        public string Years { get; set; }
        public string GetHospitalLocationData { get; set; }
        public string GetHospital { get; set; }

        public string EmailText { get; set; }
        public string a { get; set; }
        public string Mode { get; set; }
        public string Scan1 { get; set; }
        public string PathNew { get; set; }
        public string EmailID { get; set; }
        public string EmailPassword { get; set; }




    }
}
