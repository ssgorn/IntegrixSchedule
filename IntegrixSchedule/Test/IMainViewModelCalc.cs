using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.Model;

namespace IntegrixSchedule.Test
{
	public interface IMainViewModelCalc
	{
		KeyValuePair<ScheduleHeader, List<ScheduleDetail>> CreateSchedule(DateTime dateStart, DateTime dateEnd,
			Guid idOrganization);
	}
}
