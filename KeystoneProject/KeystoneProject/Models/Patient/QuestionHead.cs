using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class QuestionHead
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public int QuestionHeadID { get; set; }
        public string QuestionHeadName { get; set; }
        public string ReferenceCode { get; set; }
        public string Description { get; set; }
        public DataSet StoreAllQuestionHeadName { get; set; }
        public string Mode { get; set; }    
    }
}