using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrixSchedule.Model
{
	/// <summary>
	/// Базовый класс модели данных
	/// </summary>
	public class BaseEntity
	{
		/// <summary>
		/// Ключ
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Имя создателя записи
		/// </summary>
		public string EditUser { get; set; } = Environment.UserName;

		/// <summary>
		/// Дата создания записи
		/// </summary>
		public DateTime EditDate { get; set; } = DateTime.Now;

		/// <summary>
		/// Статус сущности
		/// </summary>
		[NotMapped]
		public States State { get; set; }

		/// <summary>
		/// Сделать статус актуальным
		/// </summary>
		internal void Unchanged()
		{
			State = States.Unchanged;
		}

		/// <summary>
		/// Сделать статус измененным
		/// </summary>
		internal void Changed()
		{
			if (State == States.Unchanged)
				State = States.Modified;
		}

		/// <summary>
		/// Клонировать
		/// </summary>
		/// <returns></returns>
		public virtual BaseEntity Clone()
		{
			return (BaseEntity)MemberwiseClone();
		}

		/// <summary>
		/// Заполнить свойства сущности, свойствами источника
		/// </summary>
		/// <param name="source"></param>
		public virtual void Update(BaseEntity source)
		{
			var curState = State;
			var tip = GetType();
			if (source.GetType() == tip)
			{
				var props = tip.GetProperties().ToList();
				foreach (var prop in props)
				{
					if (prop.CanWrite)
						prop.SetValue(this, prop.GetValue(source));
				}
			}
			State = curState;
		}
	}
}
