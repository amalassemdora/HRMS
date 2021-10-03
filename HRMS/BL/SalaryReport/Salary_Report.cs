using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Employee;
using BL.Classes;

namespace BL.SalaryReport
{
	public class Salary_Report
	{
		HrEntities db = new HrEntities();
		//Get Attended Days for each employee
		public int GetAttendedDays(int id, string month, string year)
		{
			var Attend = db.DailyAttends.Where(a => (a.Emp_Id == id) && (a.Month == month) && (a.Year == year) && (a.Attended)).Count();
			return Attend;
		}
		//Get Absence Days for each employee
		public int GetAbsenceDays(int id, string month, string year)
		{
			var total = db.DailyAttends.Where(a => (a.Emp_Id == id) && (a.Month == month) && (a.Year == year)).Count();
			int Attend = GetAttendedDays(id, month, year);
			return total - Attend;
		}
		//Get Attend time for each employee
		public TimeSpan GetAttendtime(int id, string month, string year)
		{
			//TimeSpan dateDifference = endTime.Subtract(beginTime);
			var attendtime = db.Employees.Where(a => (a.Id == id)).Select(a => a.AttendTime).SingleOrDefault();
			TimeSpan t = (TimeSpan)attendtime;
			return t;
		}
		//get leave time for each employee
		public TimeSpan GetLeavetime(int id, string month, string year)
		{
			var leavetime = db.Employees.Where(a => (a.Id == id)).Select(a => a.LeaveTime).SingleOrDefault();
			TimeSpan l = (TimeSpan)leavetime;
			return l;
		}
		//get list of attendance for each employee
		public List<DailyAttend> Worklist(int id, string month, string year)
		{
			var work_list = db.DailyAttends.Where(a => (a.Emp_Id == id) && (a.Month == month) && (a.Year == year) && (a.Attended)).ToList();
			return work_list;
		}
		//Get OverTime for each employee
		public float Getovertime(int id, string month, string year)
		{
			float overtime = 0;
			TimeSpan attendtime = GetAttendtime(id, month, year);
			TimeSpan leavetime = GetLeavetime(id, month, year);
			var work_list = Worklist(id, month, year);
			foreach (var item in work_list)
			{
				if (item.Attendance < attendtime)
				{
					TimeSpan Difference = (TimeSpan)( attendtime - item.Attendance);
					overtime += (Difference.Minutes + Difference.Hours);
				}
				if (leavetime < item.Leave)
				{
					TimeSpan dateDifference = (TimeSpan)(item.Leave - leavetime);
					overtime += (dateDifference.Minutes + dateDifference.Hours);
				}
			}
			return overtime / 60;
		}
	
		//Get deductionTime for each employee
		public float Getdeductiontime(int id, string month, string year)
		{
			float deductiontime = 0;
			TimeSpan attendtime = GetAttendtime(id, month, year);
			TimeSpan leavetime = GetLeavetime(id, month, year);
			var work_list = Worklist(id, month, year);
			foreach (var item in work_list)
			{
				if (item.Attendance > attendtime)
				{
					TimeSpan Difference = (TimeSpan)(item.Attendance - attendtime);

					deductiontime += (Difference.Hours + Difference.Minutes);
				}
				if ( leavetime > item.Leave)
				{
					TimeSpan dateDifference = (TimeSpan)(leavetime - item.Leave);
					deductiontime +=(dateDifference.Hours + dateDifference.Minutes );
				}
			}	
				return deductiontime/60;		
		}
		//Amount of OverTime for each employee
		public float GetAmountOfOvertime(int id, string month, string year, int amount = 2)
		{
			float Over_Time = Getovertime(id, month, year);
			float AmountOver = Over_Time * amount;
			return AmountOver;
		}

		//Amount of deduction for each employee
		public float GetAmountOfDeductiontime(int id, string month, string year, int amount = 2)
		{
			float Deduction_Time = Getdeductiontime(id, month, year);
			float AmountDeduction = Deduction_Time * amount;
			return AmountDeduction;
		}
		////get Base salary for each employee
		public int GetBaseSalary(int id)
		{
			var basesalary = db.Employees.Where(a => (a.Id == id)).Select(a => a.Salary).SingleOrDefault();
			return Int32.Parse(basesalary);
		}

		////Get Net salary for each employee
		public int GetNetSalary(int id, string month, string year)
		{
			int BaseSalary = GetBaseSalary(id);
			float AmountOver = GetAmountOfOvertime(id, month, year);
			float Amountdeduction = GetAmountOfDeductiontime(id, month, year);
			float NetSalary = (BaseSalary + AmountOver) - Amountdeduction;
			return (int) NetSalary;
		}

		//List<Emp> Search(DateTime Month, DateTime Year)
		//{
		//	var Employee = db.DailyAttends.Where(c => c.Day.Month == Month).ToList();
		//	return 10;
		//}
	}
}
