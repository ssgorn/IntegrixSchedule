using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule.Model
{
	/// <summary>
	/// Заголовок готового расписания
	/// </summary>
	public class ScheduleHeader : BaseEntity
	{
		/// <summary>
		/// Ключ организации
		/// </summary>
		[Index]
		public Guid IdOrganization { get; set; }

		/// <summary>
		/// Ключ шаблона, на основании которого было сформировано расписание
		/// для информации
		/// </summary>
		[Index]
		public Guid? IdScheduleTemplate { get; set; }

		/// <summary>
		/// Дата начала расписания
		/// </summary>
		public DateTime DateStart { get; set; }

		/// <summary>
		/// Дата окончания расписания
		/// </summary>
		public DateTime DateEnd { get; set; }

		public string Name => "c " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
	}
}
