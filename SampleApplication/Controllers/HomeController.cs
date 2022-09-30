using Microsoft.AspNetCore.Mvc;
using SampleApplication.Models;
using System.Data;
using System.Web.Mvc;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeInfo()
        {
            return View();
        }
        public ActionResult EmployeeList(DataTable model)
        {
            using (var context = new MyDbDataContext())
            {
                var employees = (from n in context.Employees select n);

                var count = employees.Count();

                //isortcol_0 paramater will be 0 for the first column (present in JqDataTable)
                //isortcol_0 paramater will be 1 for the second column
                //isortcol_0 paramater will be 2 for the third column
                //etc....
                //if you click on the column header ssortdir_0 paramater will change from 'asc' to 'desc'.
                if (model.iSortCol_0 == 0 && model.sSortDir_0 == "asc")
                {
                    employees = employees.OrderBy(x => x.Name);
                }
                else if (model.iSortCol_0 == 0 && model.sSortDir_0 == "desc")
                {
                    employees = employees.OrderByDescending(x => x.Name);
                }
                if (model.iSortCol_0 == 1 && model.sSortDir_0 == "asc")
                {
                    employees = employees.OrderBy(x => x.City);
                }
                else if (model.iSortCol_0 == 1 && model.sSortDir_0 == "desc")
                {
                    employees = employees.OrderByDescending(x => x.City);
                }
                if (model.iSortCol_0 == 2 && model.sSortDir_0 == "asc")
                {
                    employees = employees.OrderBy(x => x.State);
                }
                else if (model.iSortCol_0 == 2 && model.sSortDir_0 == "desc")
                {
                    employees = employees.OrderByDescending(x => x.State);
                }

                //Global Search functionality 

                if (!(string.IsNullOrEmpty(model.sSearch)) && !(string.IsNullOrWhiteSpace(model.sSearch)))
                {
                    employees = employees.Where(x => x.Name.Contains(model.sSearch) || x.City.Contains(model.sSearch) || x.State.Contains(model.sSearch));
                }

                //individual column filtering   

                // first column (Name)  search textbox value will bind to sSearch_0 parameter (JqDataTable.cs)
                // second column (City) search textbox value will bind to sSearch_1 parameter (JqDataTable.cs)
                // third column (State) search textbox value will bind to sSearch_2 parameter (JqDataTable.cs)
                //etc..
                //if you have n columns  then use have to use sSearch_(n-1) properties in your model (JqDataTable) class



                //Name Column Filtering
                if (!(string.IsNullOrEmpty(model.sSearch_0)) && !(string.IsNullOrWhiteSpace(model.sSearch_0)))
                {
                    employees = employees.Where(x => x.Name.Contains(model.sSearch_0));
                }
                //City Column Filtering
                if (!(string.IsNullOrEmpty(model.sSearch_1)) && !(string.IsNullOrWhiteSpace(model.sSearch_1)))
                {
                    employees = employees.Where(x => x.City.Contains(model.sSearch_1));
                }
                //State Column Filtering
                if (!(string.IsNullOrEmpty(model.sSearch_2)) && !(string.IsNullOrWhiteSpace(model.sSearch_2)))
                {
                    employees = employees.Where(x => x.State.Contains(model.sSearch_2));
                }

                //etc....

                var emplist = employees.Skip(model.iDisplayStart).Take(model.iDisplayLength).ToList();
                var result = new
                {
                    iTotalRecords = emplist.Count(),//records per page 
                    iTotalDisplayRecords = count, //total table count
                    aaData = emplist //employee list

                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
