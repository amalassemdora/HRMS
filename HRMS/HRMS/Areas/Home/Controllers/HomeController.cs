using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Classes;
using BL.Employee;

namespace HRMS.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Home

        //object of BL Employee
        MyEmployee emp = new MyEmployee();
        public ActionResult Index()
        {
            ViewBag.total = emp.GetEmployeeNo();
            ViewBag.Attend = emp.GetAttendNo();
            ViewBag.absence = emp.GetAbsenceNo();
            var data = emp.GetAllEmployee();
            return View(data);
        }
    }
}