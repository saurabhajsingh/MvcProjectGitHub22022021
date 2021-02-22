using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class Organization
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int OrganizationID { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationType { get; set; }

        public string EnrollmentStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string ContactFrom { get; set; }

        public Nullable<DateTime> ContactUpTo { get; set; }


        public string ContactFrom1 { get; set; }

        public string ContactUpTo1 { get; set; }

        public string Address { get; set; }

        public int CityID { get; set; }
        public string CityName { get; set; }
        public decimal CutForTPA { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public string GSTNO { get; set; }
        public string FaxNo { get; set; }

        public string PinCode { get; set; }

        public string PhoneNo1 { get; set; }

        public string MobileNo { get; set; }

        public string EmailID { get; set; }

        public string URL { get; set; }

       
        public DataSet StoreAllOrganization { get; set; }

        public DataSet BindContact { get; set; }
        public int ContactPersonID { get; set; }

        public int ContactPersonOrganizationID { get; set; }

        public string ContactPersonName1 { get; set; }
        public string ContactPersonName2 { get; set; }
        public string Designation1 { get; set; }
        public string Designation2 { get; set; }
        public string ContactPersonPhoneNO1 { get; set; }
        public string ContactPersonPhoneNO2 { get; set; }
        public string ReferenceCode { get; set; }
        public string ReferenceCode2 { get; set; }
        public string ContactPersonEmail1 { get; set; }
        public string ContactPersonEmail2 { get; set; }
    }
}
