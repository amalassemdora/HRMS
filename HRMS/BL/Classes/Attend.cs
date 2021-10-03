using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Classes
{
	public class Attend
	{
        public int Emp_Id { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public bool Attended { get; set; }
        public Nullable<System.TimeSpan> Attendance { get; set; }
        public Nullable<System.TimeSpan> Leave { get; set; }

    }
}
