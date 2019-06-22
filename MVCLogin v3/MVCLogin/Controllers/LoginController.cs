using MVCLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVCLogin.Controllers
{
    public class LoginController : Controller
    {
        public static int trial = 0;
        public static string trialtemp;

        // GET: Login
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["User"];
            if(cookie!=null)
            {
                ViewBag.UserName = cookie["UserName"].ToString();
                ViewBag.Password = cookie["Password"].ToString();

            }
            return View();
        }
        [HttpPost]
        public ActionResult Autherize(MVCLogin.Models.User userModel)
        {
             HttpCookie cookie = new HttpCookie("User");
            if (userModel.Remember == true)
            {
                cookie["UserName"] = userModel.UserName;
                cookie["Password"] = userModel.Password;
                cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Response.Cookies.Add(cookie);

            }
            else
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Response.Cookies.Add(cookie);

            }
            using (Models.LoginDataBaseEntities db = new Models.LoginDataBaseEntities())
            {
                if (Session["trial1"] != null && string.Equals(trialtemp, userModel.UserName))
                {
                    
                    ViewBag.Report = "Your Login is temporary Blocked for 5 min";
                    return View("Index", userModel);

                }
                else if (Session["trial2"] != null && string.Equals(trialtemp, userModel.UserName))
                {
                    ViewBag.Report = "Your Login is temporary Blocked for 15 min";
                    return View("Index", userModel);
                }
                else
                {
                    var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                    if (userDetails == null)
                    {
                        if (trial == 0)
                        {
                            trialtemp = userModel.UserName;
                            trial = trial + 1;
                        }
                        else if (string.Equals(trialtemp, userModel.UserName))
                        {
                            trial = trial + 1;
                            if (trial == 3)
                            {
                                Session["trial1"] = userModel.UserName;
                                Session.Timeout = 1;
                            }
                            else if (trial == 6)
                            {
                                Session["trial2"] = userModel.UserName;
                                Session.Timeout = 1;
                            }
                            else if (trial == 9)
                            {
                               


                            }


                        }
                        else
                        {
                            trialtemp = userModel.UserName;
                            trial = 1;
                        }
                        userModel.LoginErrorMessage = "Wrong User Name Or Password";
                        return View("Index", userModel);
                    }
                    else
                    {
                        trial = 0;
                        trialtemp = null;

                        Session["userID"] = userDetails.UserID;
                        Session["userName"] = userDetails.UserName;
                        return RedirectToAction("Index", "Home");
                    }

                }

                                
            }
                
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}