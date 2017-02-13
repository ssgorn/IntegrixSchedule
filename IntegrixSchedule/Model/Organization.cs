using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule.Model
{
	/// <summary>
	/// Организации
	/// </summary>
	public class Organization : BaseEntity
	{
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; }
	}
}
