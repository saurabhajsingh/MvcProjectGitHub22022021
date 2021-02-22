using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class DoctorAppointmentSchedule
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int DoctorAppoinmentScheduleID { get; set; }
        public string DoctorID { get; set; }

      //  [Required(ErrorMessage = "Appointment Type is Required")]
        public string AppointmentType { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FromDate { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }


        public string FromDate1 { get; set; }

        public string FromTime1 { get; set; }

        public string ToTime1 { get; set; }

        public int NoOfAppointment { get; set; }

        public int PerAppointmentTimeinMinute { get; set; }

        public bool EndDurationStatus { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //public DateTime EndDurationDate { get; set; }

        public string DoctorPrintName { get; set; }

        public string DayDate { get; set; }
        public string monthweek { get; set; }

        public DataSet StoreAllDoctorAppointment { get; set; }

        public DataSet dgvDoctorSchedule { get; set; }

        public DataSet DoctorSchedule { get; set; }

        public List<string> dayss = new List<string>();

        public ArrayMonthDay[] contact { get; set; } 

    }
    public class ArrayMonthDay
    {

        public string monthweek { get; set; }

    }
}
