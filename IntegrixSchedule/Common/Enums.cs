using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule
{
	/// <summary>
	/// Статус сущности. Сигнализирует о ее изменениях.
	/// </summary>
	public enum States
	{
		/// <summary>
		/// Неизменялась.
		/// </summary>
		Unchanged = 0,

		/// <summary>
		/// Только что добавлена.
		/// </summary>
		Added = 1,

		/// <summary>
		/// Изменена.
		/// </summary>
		Modified = 2,

		/// <summary>
		/// Удалена.
		/// </summary>
		Deleted = 3
	}
}
