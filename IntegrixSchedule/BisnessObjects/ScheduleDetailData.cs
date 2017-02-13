using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.Model;

namespace IntegrixSchedule.BisnessObjects
{
	public class ScheduleDetailData : ScheduleDetail
	{
		public ScheduleDetailData() { }

		public ScheduleDetailData(ScheduleDetail dat)
		{
			var curstate = State;
			var tip = GetType();
			var props = tip.GetProperties().ToList();
			foreach (var prop in props)
			{
				if (prop.CanWrite)
					prop.SetValue(this, prop.GetValue(dat));
			}
			State = curstate;
		}

		public string DisplayText
		{
			get
			{
				if (IsBreak) return "Перерыв";
				return "c " + TimeStart.ToShortTimeString() + " по " + TimeEnd.ToShortTimeString();
			}
		}
	}
}
