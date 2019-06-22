using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLogin.Models;

namespace MVCLogin.Controllers
{
    public class ForgetPasswordController : Controller
    {
        // GET: ForgetPassword
        public ActionResult ForgetPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Autherize(MVCLogin.Models.User userModel)
        {
            

            using (Models.LoginDataBaseEntities db = new Models.LoginDataBaseEntities())
            {
                var userDetails = db.Users.Where(x => x.UserName == userModel.UserName).FirstOrDefault();
                if (userDetails == null)
                {
                    
                    userModel.LoginErrorMessage = "Wrong User Name Or Password";
                    return View("ForgetPassword", userModel);
                }
                else
                {

                    Session["userID"] = userDetails.UserID;
                    Session["userName"] = userDetails.UserName;
                   // return RedirectToAction("Index", "Home");
                    return View("Autherize", userModel);
                }
            }

               
        }
    }
}