using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.Model;

namespace IntegrixSchedule.BisnessObjects
{
	public class ScheduleDetailsCollection : ObservableCollection<ScheduleDetailHeaderData>
	{
		public ScheduleDetailsCollection() { }

		public ScheduleDetailsCollection(List<ScheduleDetail> allDetails, Guid idScheduleHeader)
		{
			var list = allDetails.GroupBy(x => x.TimeStart.Date).ToList();
			foreach (var day in list)
			{
				Add(new ScheduleDetailHeaderData()
				{
					Date = day.Key,
					IdScheduleHeader = idScheduleHeader,
					ScheduleDetails = new ObservableCollection<ScheduleDetailData>(
						allDetails.Where(x => x.TimeStart.Date == day.Key).Select(x => new ScheduleDetailData(x)).ToList())
				});
			}
			
		}
	}
}
