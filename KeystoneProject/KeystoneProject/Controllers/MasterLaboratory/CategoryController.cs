using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Buisness_Logic.Master;
using System.Data;
using KeystoneProject.Models.MasterLaboratory;

namespace KeystoneProject.Controllers.Master
{
    public class CategoryController : Controller
    {


        BL_Category category = new BL_Category();

        public JsonResult ShowCategory()
        {

            BL_Category category1 = new BL_Category();

            return new JsonResult { Data = category1.SelectAllCategory(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        [HttpGet]
        public ActionResult Category()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Category(Category obj)
        {
            try
            {
                BL_Category category2 = new BL_Category();
                if (category2.CheckCategory(obj.CategoryID, obj.CategoryName))
                {
                    if (category2.Category(obj))
                    {
                        if (obj.CategoryID > 0)
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Category Update Successfully";
                            return RedirectToAction("Category", "Category");
                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["msg"] = "Category Saved Successfully";
                            return RedirectToAction("Category", "Category");
                        }
                       
                    }
                }
                else
                {
                    TempData["msg"] = "Category Name Already Exist's";
                    return RedirectToAction("Category", "Category");
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }

        }                                                                                                                                                                                                                                                                                                                                                                                                                              



        [HttpGet]

        public JsonResult EditCategory1(int id)
        {
            BL_Category category1 = new BL_Category();
            ModelState.Clear();

            return new JsonResult { Data = category1.GetCategory(id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return View(db.GetDepartment(id));
        }

        public ActionResult DeleteCategory(int CategoryId)
        {
           
            string del = "";
            CategoryId = Convert.ToInt32(Request.Form["CategoryId"]);
            BL_Category category1 = new BL_Category();
           
            if (category1.DeleteCategory(CategoryId))
            {
                del = "Category Deleted Succussfully ";
            }

            return Json(del, JsonRequestBehavior.AllowGet);
        }      


    }
}
