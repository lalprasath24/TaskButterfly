using EmployeeDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EmployeeDetails.Controllers
{
    public class HomeController : Controller
    {
        EmployeeDbEntities db=new EmployeeDbEntities();
        public ActionResult Index()
        {
           

            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            string searchStr = Request.Form.GetValues("search[value]").FirstOrDefault();
            

            //Pagination parameters;
            string length=Request.Form.GetValues("length").FirstOrDefault();
            string start=Request.Form.GetValues("start").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skipRecords=start != null ? Convert.ToInt32(start) : 0;
            int totalRec;



            IEnumerable<Employee> employees = GetEmployee(searchStr,pageSize,skipRecords, out totalRec);


            return Json(new {data=employees, recordsFiltered = totalRec, recordsTotal = totalRec });
        }

        private IEnumerable<Employee> GetEmployee(string searchStr,int pageSize,int skipRecords,out int totalRec)
        {
            IEnumerable<Employee> employees = db.Employees.Where(c=>c.name.Contains(searchStr)|| c.Experience.Contains(searchStr)|| c.gender.StartsWith(searchStr)).AsEnumerable<Employee>();

            totalRec=employees.Count();

            employees=employees.Skip(skipRecords).Take(pageSize);


            return employees;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Employee entity)
        {
            db.Employees.Add(entity);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
       



























    }
}