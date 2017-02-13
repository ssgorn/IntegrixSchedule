using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IntegrixSchedule.Common;
using IntegrixSchedule.View;

namespace IntegrixSchedule.ViewModel
{
	public class SelectDatesViewModel : BaseViewModel
	{
		#region Ctr

		public SelectDatesViewModel(SelectDatesWindow selectDatesWindow, DateTime dateStart, DateTime dateEnd)
		{
			_selectDatesWindow = selectDatesWindow;
			DateStart = dateStart;
			DateEnd = dateEnd;
			ButtonClickCommand = new DelegateCommand(ButtonClickAction);
		}

		#endregion Ctr

		#region Команды

		public ICommand ButtonClickCommand { get; internal set; }

		#endregion Команды

		#region Свойства

		private readonly SelectDatesWindow _selectDatesWindow;

		private DateTime _dateStart;
		public DateTime DateStart
		{
			get { return _dateStart; }
			set
			{
				_dateStart = value;
				OnPropertyChanged("DateStart");
			}
		}

		private DateTime _dateEnd;
		public DateTime DateEnd
		{
			get { return _dateEnd; }
			set
			{
				_dateEnd = value;
				OnPropertyChanged("DateEnd");
			}
		}

		#endregion Свойства

		#region Методы

		public bool ShowDialog()
		{
			_selectDatesWindow.DataContext = this;
			return _selectDatesWindow.ShowDialog() ?? false;
		}

		protected void ButtonClickAction(object sender)
		{
			_selectDatesWindow.DialogResult = true;
		}

		#endregion Методы
	}
}
