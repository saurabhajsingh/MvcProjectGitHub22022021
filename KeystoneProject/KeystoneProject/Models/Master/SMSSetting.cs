using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class SMSSetting
    {
        public DataSet dsparamiter
        {
            get;
            set;
        }
        public DataSet dsparamiter1
        {
            get;
            set;
        }
        public string Mode 
        {
            get; 
            set; 
        }

        public int MobileSMSID
        { 
            get;
            set; 
        }

        public int MasterSettingID
        {
            get;
            set;
        }
        public string IPDAddimisionDrSMS
        {
            get;
            set;
        }
        public string IPDDischargeDrSMS
        {
            get;
            set;
        }
        public string IPDAddimisionPSMS
        {
            get;
            set;
        }
        public string OPDRegDrSMS
        {
            get;
            set;
        }
        public string IPDDischargePSMS
        {
            get;
            set;
        }
        public string IPDAddimisionCDrSMS
        {
            get;
            set;
        }
        public string IPDDischargeCDrSMS
        {
            get;
            set;
        }
        public string AppointmentPSMS
        {
            get;
            set;
        }
        public string AppointmentCDrSMS
        {
            get;
            set;
        }
        public string IPDDischargeP
        {
            get;
            set;
        }
        public string IPDAdmissionCDr
        {
            get;
            set;
        }
        public string IPDDischargeCDr
        {
            get;
            set;
        }
        public string AppointmentP
        {
            get;
            set;
        }
        public string AppointmentCDr
        {
            get;
            set;
        }

        public string PARAMETER
        {
            get;
            set;
        }
        //----------------------------------chk

        public bool chkIPDAddimisionDrSMS
        {
            get;
            set;
        }
        public bool chkIPDDischargeDrSMS
        {
            get;
            set;
        }
        public bool chkIPDAddimisionPSMS
        {
            get;
            set;
        }
        public bool chkOPDRegDrSMS
        {
            get;
            set;
        }
        public bool chkIPDDischargePSMS
        {
            get;
            set;
        }
        public bool chkIPDAddimisionCDrSMS
        {
            get;
            set;

        }
        public bool chkIPDDischargeCDrSMS
        {
            get;
            set;
        }
        public bool chkAppointmentPSMS
        {
            get;
            set;
        }
        public bool chkAppointmentCDrSMS
        {
            get;
            set;
        }
        public bool chkIPDAdmissionCDr
        {
            get;
            set;
        }
        public string chkIPDBillP
        {
            get;
            set;
        }
        public string chkIPDDischargeCDr
        {
            get;
            set;
        }
        public string chkAppoimentCDr
        {
            get;
            set;
        }
        public string chkAppoimentP
        {
            get;
            set;
        }
       
    }
}