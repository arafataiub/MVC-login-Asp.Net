using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLogin.Models;

namespace MVCLogin.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult AddOrEdit(int id=0)
        {
            
            User userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(User userModel) 
        {
           
            
            using (LoginDataBaseEntities l1= new LoginDataBaseEntities())
            {
                if(l1.Users.Any(x=>x.UserName==userModel.UserName))
                    {
                    ViewBag.DuplicateMessage = "User Name already exist";
                    return View("AddOrEdit", userModel);
                }

                l1.Users.Add(userModel);
                l1.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful";
            return View("AddOrEdit",new User());
        }
    }
}