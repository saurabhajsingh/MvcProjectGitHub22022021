using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class TPADocumentMaster
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int UserID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public int LetterID { get; set; }
    }
}