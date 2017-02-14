using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule.Model
{
	/// <summary>
	/// Шаблон расписаний
	/// </summary>
	public class ScheduleTemplate : BaseEntity
	{
		/// <summary>
		/// Ключ организации
		/// </summary>
		//[Index("IX_IsActualIdOrganozation", 1, IsUnique = true)]
		[Index]
		public Guid IdOrganization { get; set; }

		/// <summary>
		/// Описание шаблона
		/// </summary>
		public string Name { get; set; }

		private bool _isActual;
		/// <summary>
		/// Признак актуальности
		/// </summary>
		//[Index("IX_IsActualIdOrganozation", 2, IsUnique = true)]
		[Index]
		public bool IsActual
		{
			get { return _isActual; }
			set
			{
				_isActual = value;
				if (State == States.Unchanged)
					State = States.Modified;
			}
		}

		/// <summary>
		/// Время начала работы
		/// </summary>
		public DateTime TimeStart { get; set; }

		/// <summary>
		/// Время окончания работы
		/// </summary>
		public DateTime TimeEnd { get; set; }

		/// <summary>
		/// Интервал приема
		/// </summary>
		public int RecipietTime { get; set; }

		/// <summary>
		/// Время начала перерыва
		/// </summary>
		public DateTime? BreakeStartTime { get; set; }

		/// <summary>
		/// Время окончания перерыва
		/// </summary>
		public DateTime? BreakeEndTime { get; set; }

		//т.к. дней в неделе 7, то решил сделать плоскую структуру для шаблона

		/// <summary>
		/// Признак работы в понедельник
		/// </summary>
		public bool IsWork1 { get; set; }
		/// <summary>
		/// Признак работы во вторник
		/// </summary>
		public bool IsWork2 { get; set; }
		/// <summary>
		/// Признак работы в среду
		/// </summary>
		public bool IsWork3 { get; set; }
		/// <summary>
		/// Признак работы в четверг
		/// </summary>
		public bool IsWork4 { get; set; }
		/// <summary>
		/// Признак работы в пятницу
		/// </summary>
		public bool IsWork5 { get; set; }
		/// <summary>
		/// Признак работы в субботу
		/// </summary>
		public bool IsWork6 { get; set; }
		/// <summary>
		/// Признак работы в воскресенье
		/// </summary>
		public bool IsWork7 { get; set; }
	}
}
