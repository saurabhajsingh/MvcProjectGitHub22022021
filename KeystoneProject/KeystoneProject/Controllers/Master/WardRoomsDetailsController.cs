using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace KeystoneProject.Controllers.Master
{
    public class WardRoomsDetailsController : Controller
    {
        //
        // GET: /WardRoomsDetails/

            private SqlConnection con;
            int HospitalID = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalID"]);
            int LocationID = Convert.ToInt32(System.Web.HttpContext.Current.Session["LocationID"]);
            int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
         private void Connect()
        {
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
            
       
        }

        BL_WardRoomDetails dbWard = new BL_WardRoomDetails();

        public JsonResult GetAllWard()
        {
           
            return new JsonResult {Data= dbWard.SelectGetAllWard(),JsonRequestBehavior=JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetWard(int WardID)
        {
            return new JsonResult { Data=dbWard.GetWardData(WardID),JsonRequestBehavior=JsonRequestBehavior.AllowGet};
        }

        public JsonResult GetwardRoom(int RoomID)
        {
            return new JsonResult { Data = dbWard.GetWardRooms(RoomID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region Delete Data
        public ActionResult DeleteWardRoomsDetails(int RoomID, string WardID)


        {
               string Message="";
                if (RoomID != 0)
                {
                    
                        string DependaincyName = dbWard.DeleteWardRoomsDetails(RoomID, Convert.ToInt32(WardID));
                        if (DependaincyName == "Delete")
                        {
                           
                            Message = "Record Delete successfully";
                        }
                        else
                        {
                         Message=  "You Delete First " + DependaincyName + "";

                        }

                      

                }
             return   new JsonResult { Data = Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        
    
        #endregion
         public JsonResult DeleteBedID(int BedID)
        
         {
            var returnValue = string.Empty;
            try
            {
                bool result = dbWard.CheckBedIDInIPDWardDetails(BedID);
                if (result)
                {
                    Connect();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Update WardRoomsDetails set RowStatus = 1 where  BedID=" + BedID + " and  RowStatus = 0", con);
                 //   cmd.CommandType = CommandType.Text;
                    int outputResult = cmd.ExecuteNonQuery();

                    if (outputResult > 0)
                    {
                        returnValue = "Delete";
                    }
                    else
                    {
                        returnValue = "Failure";
                    }
                }
                else
                {
                    returnValue = "Failure";
                }
            }
              catch (Exception ex)

            {
                returnValue = "Please Try Again Later.";
            }
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
         

        public JsonResult GetWardRoomDetails(int RoomID)
        {
            return new JsonResult {Data = dbWard.GetWardRoomsDetails(RoomID),JsonRequestBehavior=JsonRequestBehavior.AllowGet};
        }

        public JsonResult GetAllWardRoomDetail(string prefix)
        {

            List<WardRoomsDetails> search = new List<WardRoomsDetails>();
            BL_WardRoomDetails dbWard = new BL_WardRoomDetails();
            DataSet dsState = new DataSet();
            dsState = dbWard.GetAllWard(prefix);

            foreach (DataRow dr in dsState.Tables[0].Rows)
            {
                search.Add(new WardRoomsDetails
                {
                    WardID = Convert.ToInt32(dr["WardID"]),
                    WardName = dr["WardName"].ToString(),
                    GeneralCharges = dr["GeneralCharges"].ToString(),
                    EmergencyCharges = dr["EmergencyCharges"].ToString(),
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult WardRoomsDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WardRoomsDetails(WardRoomsDetails objWardRoom, FormCollection fc)
        {

            try
            {
                if (objWardRoom.BedNo != null)
                {
                    if (dbWard.CheckWard(objWardRoom.WardName, objWardRoom.WardID))
                    {
                        if (dbWard.CheckWardRooms(objWardRoom.WardID, Convert.ToInt32(objWardRoom.RoomID), objWardRoom.RoomName))
                        {
                            objWardRoom.BedNo = Request.Form["BedNo"].ToString();

                            objWardRoom.BedStatus1 = fc["Bedcount"].ToString();
                           
                           
                            int wardid = Convert.ToInt32(objWardRoom.WardID);
                           
                            if (dbWard.save(objWardRoom))
                            {
                                if (wardid > 0)
                                {
                                    ModelState.Clear();
                                    TempData["Msg"] = " Ward Updated Successfully";
                                    return RedirectToAction("WardRoomsDetails", "WardRoomsDetails");
                                }
                                else
                                {
                                    ModelState.Clear();
                                    TempData["Msg"] = " Ward Saved Successfully";
                                    return RedirectToAction("WardRoomsDetails", "WardRoomsDetails");
                                }
                            }
                        }
                        else
                        {
                         TempData["msg"]  = "Room Already Exist's";

                        }

                    }

                    else
                    {

                        TempData["msg"] = "Ward Already Exist's";
                    }
                }
                else
                {
                    TempData["msg"] = "Please Add BED No ";
                    RedirectToAction("WardRoomsDetails", "WardRoomsDetails");
                }

                return RedirectToAction("WardRoomsDetails", "WardRoomsDetails");
            }
            catch (Exception ex)
            {
               TempData["msg"]  = "Please Add BED No "; 
                return RedirectToAction("WardRoomsDetails", "WardRoomsDetails");
            }
           
        }
	}
}