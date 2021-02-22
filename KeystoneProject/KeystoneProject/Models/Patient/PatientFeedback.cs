using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientFeedback 
    {

        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public string ReviewType { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string UserID { get; set; }
        public int FeedBackFormID { get; set; }
        public string Comments { get; set; }
        public string QuestionHeadID { get; set; }
        public string QuestionID { get; set; }
        public string QuestionHeadName { get; set; }
        public string QuestionName { get; set; }
        public int PatientRegNO { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string ConsultantDoctor { get; set; }
        public string DoctorID { get; set; }
        public string ReferredByDoctorID { get; set; }
        public string ReferredDr { get; set; }
        public string userquestionhead { get; set; }
        public string remark { get; set; }
        public DataSet dsgrid { get; set; }


        
    }
}
