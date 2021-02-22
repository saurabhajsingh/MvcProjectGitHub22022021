using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace KeystoneProject.Models.Master
{
    public class Doctor
    {
        [Required(ErrorMessage = "Doctor Type is Required")]
        public string DoctorType { get; set; }
        public string JoinDate { get; set; }
        public string DateBirth { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }

        public int DoctorID { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        public string DoctorFName { get; set; }
        public string DoctorLName { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        public string DoctorPrintName { get; set; }
        public string RegNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string DepartmentID { get; set; }
        public string QualifictionID { get; set; }
        public string SpecializationID { get; set; }
        //public int CategoryID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<DateTime> DateOfJoining { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public string UploadDocuments { get; set; }
        public Decimal CounsultancyFees { get; set; }
        public int CounsultancyDuration { get; set; }
        public Decimal RenewalFee { get; set; }
        public Decimal RenewalDuration { get; set; }
        public Decimal Commission { get; set; }
        public string CommissionType { get; set; }
        public int ConsultancyLimit { get; set; }
        public string DoctorImage { get; set; }
        public string PermanentAddress { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        [StringLength(10, MinimumLength = 10)]
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public DataSet StoreAllDoctor { get; set; }
        public string TableName { get; set; }

        public string DepartmentName { get; set; }
        public string QualifictionName { get; set; }
        public string SpecializationName { get; set; }
        public string Mode { get; set; }


        public string LevingDate { get; set; }
        public string UploadSignature { get; set; }

    }
}