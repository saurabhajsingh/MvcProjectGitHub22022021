using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KeystoneProject.Models.Models_Application
{
    public class Models_Application
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get;
            set;
        }
    }
}