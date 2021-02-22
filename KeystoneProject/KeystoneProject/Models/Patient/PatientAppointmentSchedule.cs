using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientAppointmentSchedule
    {
    //    public string FromTime { get; set; }
        public string Table1 { get; set; }
        public string TypeID { get; set; }
        public string Table2 { get; set; }
        public int HospitalID { get; set; }
       public int LocationID { get; set; }

       public string PatientID { get; set; }

       public string PFPatientName { get; set; }

       public string PatientName { get; set; }
       public string FirstName { get; set; }
       public string PFirstName { get; set; }
       public string PMiddleName { get; set; }
       public string PLastName { get; set; }

       public string Age { get; set; }
       public string Year{get;set;}
       public string Day { get; set; }
       public string Month { get; set; }
       public string AgeType { get; set; }

       public string DateOFBirth { get; set; }

       public string MobileNo { get; set; }

       public string PatientStatus { get; set; }

       public bool Status { get; set; }

       public string OPNO { get; set; }

       public string PatientAppointmentID { get;set; }
       public string DoctorAppoinmentScheduleID { get; set; }
       public string PatientAppointmentScheduleID { get; set; }
       public string AppointDateID { get; set;}
       public string AppointmentNoID { get; set; }

       public string DoctorAppointmentScheduleID { get; set; }

       public string DoctorAppoinmentScheduleDetailID { get; set; }
        
       public string DoctorID { get; set; }
       public string AppointmentTime { get; set; }

       public string AppointmentDate { get; set; }

       public string FromTime { get; set; }

       public string ToTime { get; set; }

       public string NoOfAppointment { get; set; }
       public string DoctorAppoinmentID { get; set; }
       public string DoctorNameID { get; set; }
       public string DoctorName { get; set; }

       public string PatientAppointmentNO { get; set; }

       public string PatientRegNo { get; set; }

       public string OPDIPDID { get; set; }


       public string PrefixID { get; set; }

       public string PrefixName { get; set; }

       public string Gender { get; set; }

       public string FinancialYear { get; set; }

       public string FinancialYearID { get; set; }

       public string DoctorPrintName { get; set; }

       public string AppointmentType { get; set; }
       public string AppointmentTypeID { get; set; }
      // public string PatientAppointmentNO { get; set; }
       public DateTime Date { get; set; }
       public DateTime FromDate { get; set; }
        public string TotalReg { get; set; }

        public string PerAppointmentTimeinMinute { get; set; }

       public bool EndDurationStatus { get; set; }

       public DateTime EndDurationDate { get; set; }

       public string DayDate { get; set; }
       public string RescheduleID { get; set; }

       public string Reason { get; set; }
       public bool CancelAppointment { get; set; }

       public string TotalAppointment { get; set; }

       public string TodayDate { get; set; }


    }
}
