using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class GradesController : Controller
    {
        public IActionResult Index()
        {
            SecurityService securityService = new SecurityService();

            List<StudentsSubjectsAverageModel> courses = securityService.GetSubjects();


            return View(courses);        
        }
    }
}
