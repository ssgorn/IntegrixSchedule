using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IntegrixSchedule.ViewModel
{
	public abstract class BaseViewModel : IDisposable, INotifyPropertyChanged
	{
		public virtual void Dispose()
		{
			
		}

		#region OnPropertyCnahged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
