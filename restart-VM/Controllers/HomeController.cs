using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
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
            // сообщение пользователю о результатах работы скрипта
            string _message;
            
            // парсим введеное пользователем число 
            try
            {
                int _vmnum = Int32.Parse(Request.Form["vmnum"]);

                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                    // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                    PowerShellInstance.AddScript("connect-viserver 192.168.46.10;  " +
                            "get-vm pc-TEST | Restart-VM -confirm:$false; ");

                    // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                    //PowerShellInstance.AddParameter("param1", "parameter 1 value!");
                    // invoke execution on the pipeline (collecting output)

                    Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                    // loop through each output object item
                    foreach (PSObject outputItem in PSOutput)
                    {
                        // if null object was dumped to the pipeline during the script then a null
                        // object may be present here. check for null to prevent potential NRE.
                        if (outputItem != null)
                        {
                            //TODO: do something with the output item 
                            // outputItem.BaseOBject
                            _message = outputItem.ToString();
                        }
                    }
                }

            }
            catch (FormatException e)
            {
                //Console.WriteLine(e.Message);
                _message = e.Message + " " + "введите число";
                ViewBag.message = _message;
                return View();
            }

            _message = "Перезагрузка ВМ pc-" + Request.Form["vmnum"] + " прошла успешно";

            ViewBag.message = _message;
            return View();
        }
    }
}

static public class Powershelling {
    //public PowerShell Main() {

    //}
}
