using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Employee;
using BL.Classes;

namespace BL.Employee
{
	public class MyEmployee
	{
		HrEntities db = new HrEntities();
		string Day = DateTime.Today.Day.ToString();
		string month =DateTime.Today.Month.ToString();
		string year = DateTime.Today.Year.ToString();
		//string trues = "true";
		//string falses = "false";
		public List<Emp> GetAllEmployee()
		{
			var data = db.Employees.Select(a => new Emp { Id = a.Id, Name = a.Name, Mobile = a.Mobile, HiringDate = a.HiringDate, Salary = a.Salary, AttendTime = a.AttendTime, LeaveTime = a.LeaveTime }).ToList();
			return data;
		}
		public int GetEmployeeNo()
		{
			var data = db.Employees.Count();
			return data;
		}
		public int GetAttendNo()
		{
			var data = db.DailyAttends.Where(a=>(a.Day.Equals(Day))&& (a.Month.Equals(month)) && (a.Year.Equals(year)) && (a.Attended)).Count();
			return data;
		}
		public int GetAbsenceNo()
		{
			int total = GetEmployeeNo();
			int attend = GetAttendNo();
			int data = total - attend;
			//var data = db.DailyAttends.Where(a => (a.Day.Equals(Day)) && (a.Month.Equals(month)) && (a.Year.Equals(year)) && (a.Attended =="false")).Count();
			return data;
		}
	}
}
