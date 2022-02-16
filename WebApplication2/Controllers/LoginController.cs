using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {

        public static List<UserSubjetctsAverageModel> userSubjects;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel userModel)
        {

            SecurityService securityService = new SecurityService();

            userSubjects = securityService.IsValid(userModel);

            if (userSubjects.Count > 0)
            {
                return View("LoginSuccess", userSubjects);
            }
            else
            {
                return View("LoginFailure", userModel);
            }
        }

        public IActionResult MyInfo()
        {
            return View("LoginSuccess", userSubjects);
        }
    }
}
