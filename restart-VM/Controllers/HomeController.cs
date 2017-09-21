using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace restart_VM.Controllers
{
    public class HomeController : Controller
    {
        // GET: 
        public ActionResult Index(int? vmnum)
        {
            ViewBag.vmnum = vmnum;
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Index()
        {
            string _message;
            _message = "ds"  + " " + Request.Form["vmnum"];
            ViewBag.message = _message;
            return View();
        }
    }
}