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

                using (PowerShell ps = PowerShell.Create())
                {
                    ps.AddScript("Set-ExecutionPolicy -ExecutionPolicy Unrestricted");
                    ps.Invoke();

                    // Add command to laod modules so that the CS cmdlets will work
                    //PowerShellInstance.AddScript(@"Import-Module VMware.VimAutomation.Core;");
                    //PowerShellInstance.Invoke();
                    ps.Commands.AddCommand("Import-Module").AddArgument(@"VMware.VimAutomation.Core");
                    ps.Invoke();

                    // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                    // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                    ps.AddScript(string.Format(
                            "Connect-VIServer 192.168.46.10;  " +
                            "Get-VM pc_{0} | Restart-VM -confirm:$false; ", _vmnum) );

                    // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                    //PowerShellInstance.AddParameter("param1", "parameter 1 value!");
                    // invoke execution on the pipeline (collecting output)

                    Collection<PSObject> PSOutput = ps.Invoke();

                    // message for diagnostics
                    _message = "diag NULL";

                    // check the other output streams (for example, the error stream)
                    if (ps.Streams.Error.Count > 0)
                    {
                        // error records were written to the error stream.
                        //_message = PowerShellInstance.Streams.Error.ToString();
                        _message = "";
                        foreach (var i in ps.Streams.Error)
                        {
                            //display each product to console by using Display method in Farm Shop class
                            _message = _message + ' ' + i.ToString();
                        }
                    }
                    
                    // loop through each output object item
                    
                    foreach (PSObject outputItem in PSOutput)
                    {
                        // if null object was dumped to the pipeline during the script then a null
                        // object may be present here. check for null to prevent potential NRE.
                        if (outputItem != null)
                        {
                            //TODO: do something with the output item 
                            // outputItem.BaseOBject
                            // _message = outputItem.ToString();
                            _message = "Перезагрузка ВМ pc-" + Request.Form["vmnum"] + " прошла успешно" + " diag " + outputItem.ToString();
                        }
                        else {
                            _message = "Перезагрузка ВМ pc-" + Request.Form["vmnum"] + " прошла успешно";
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

            

            ViewBag.message = _message;
            return View();
        }
    }
}

static public class Powershelling {
    //public PowerShell Main() {

    //}
}
