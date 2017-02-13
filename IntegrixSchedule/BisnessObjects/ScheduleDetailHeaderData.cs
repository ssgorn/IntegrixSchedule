using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.Model;

namespace IntegrixSchedule.BisnessObjects
{
	public class ScheduleDetailHeaderData
	{
		public DateTime Date { get; set; }

		public Guid IdScheduleHeader { get; set; }

		public string DisplayDate => Date.ToShortDateString();

		private IList<ScheduleDetailData> _scheduleDetails;

		public IList<ScheduleDetailData> ScheduleDetails
		{
			get { return _scheduleDetails.OrderBy(x => x.TimeStart).ToList(); }
			set { _scheduleDetails = value; }
		}
	}
}
