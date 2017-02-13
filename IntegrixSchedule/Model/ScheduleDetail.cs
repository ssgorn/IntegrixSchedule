using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule.Model
{
	public class ScheduleDetail : BaseEntity
	{
		/// <summary>
		/// Ключ заголовка расписания
		/// </summary>
		[Index]
		public Guid IdScheduleHeader { get; set; }

		/// <summary>
		/// Время начала приема
		/// </summary>
		public DateTime TimeStart { get; set; }

		/// <summary>
		/// Время окончания приема
		/// </summary>
		public DateTime TimeEnd { get; set; }

		/// <summary>
		/// Признак перерыва
		/// </summary>
		public bool IsBreak { get; set; }
	}
}
