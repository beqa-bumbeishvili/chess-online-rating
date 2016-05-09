using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Chess_Server.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
          
            if (username == "admin" && password == "vashli")
            {
                Session["UserRole"] = "admin";
            }
           return View("~/Views/Home/Index.cshtml");
            }
        }
    }
