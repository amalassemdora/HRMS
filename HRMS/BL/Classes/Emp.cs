using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Classes
{
	public class Emp
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Mobile { get; set; }
		public Nullable<System.DateTime> HiringDate { get; set; }
		public string Salary { get; set; }
		public Nullable<System.TimeSpan> AttendTime { get; set; }
		public Nullable<System.TimeSpan> LeaveTime { get; set; }
	}
}
