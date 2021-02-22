using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;

namespace KeystoneProject.Controllers.Master
{
    public class MedicineLibraryController : Controller
    {
        //
        // GET: /MedicineLibrary/
        BL_MedicineLibrary obj = new BL_MedicineLibrary();

        public JsonResult ShowMedicineLibrary()
        {

            BL_MedicineLibrary db = new BL_MedicineLibrary();


           return new JsonResult { Data = db.SelectAllData(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        [HttpGet]
        public ActionResult MedicineLibrary()
        {
            return View();
        }

        public JsonResult GetMedicines(string prefix)
        {
            BL_MedicineLibrary BL_obj = new BL_MedicineLibrary();
            DataSet ds = BL_obj.GetMedicines(prefix);

            List<MedicineLibrary> searchlist = new List<MedicineLibrary>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MedicineLibrary
                {
                    DrugID = Convert.ToInt32(dr["DrugID"].ToString()),
                    DrugName = dr["DrugName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetMedicineType(string prefix)
        {
            BL_MedicineLibrary BL_obj = new BL_MedicineLibrary();
            DataSet ds = BL_obj.GetMedicineType(prefix);

            List<MedicineLibrary> searchlist = new List<MedicineLibrary>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MedicineLibrary
                {
                    MedicineLibraryID = Convert.ToInt32(dr["MedicineTypeID"].ToString()),
                    MedicineTypeName = dr["MedicineTypeName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUnit(string prefix)
        {
            BL_MedicineLibrary BL_obj = new BL_MedicineLibrary();
            DataSet ds = BL_obj.GetUnit(prefix);

            List<MedicineLibrary> searchlist = new List<MedicineLibrary>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new MedicineLibrary
                {
                    UnitID = Convert.ToInt32(dr["UnitID"].ToString()),
                    UnitName = dr["UnitName"].ToString()
                });
            }
            return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult MedicineLibrary(MedicineLibrary objMedicineLibrary)
        {
            objMedicineLibrary.txtdeteils = Request.Form["txtdeteils"].ToString();
            try
            {
                if (obj.CheckMedicineLibrary(objMedicineLibrary.MedicineLibraryID, objMedicineLibrary.Medicines))
                {
                    if (obj.Save(objMedicineLibrary))
                    {
                        if (objMedicineLibrary.MedicineLibraryID > 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Library Updated Successfully !";
                            return RedirectToAction("MedicineLibrary", "MedicineLibrary");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Medicine Library Saved Successfully !";
                            return RedirectToAction("MedicineLibrary", "MedicineLibrary");
                        }
                       
                    }
                }
                else
                {
                    TempData["msg"] = "Medicine Library Already Exists !";
                }
                return RedirectToAction("MedicineLibrary", "MedicineLibrary");

            }
            catch (Exception Ex)
            {
                TempData["msg"] = Ex.Message;
                return RedirectToAction("MedicineLibrary", "MedicineLibrary");
            }

        }





        [HttpGet]

        public JsonResult EditMedicineLibrary(int id)
        {

            ModelState.Clear();

            return new JsonResult { Data = obj.GetMedicineLibrary(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult DeleteMedicineLibrary(int MedicineLibraryID)
        {
            string _Del = null;
            try
            {
                string DependaincyName = obj.Delete(Convert.ToInt32(MedicineLibraryID));

                _Del = "Medicine Library Deleted Successfully !";


            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



    }
}