using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IntegrixSchedule.View;
using IntegrixSchedule.ViewModel;

namespace IntegrixSchedule
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static MainViewViewModel MainViewViewModel { get; private set; }

		public App()
		{
			MainViewViewModel = new MainViewViewModel(new MainView());
			MainViewViewModel.Show();
		}
	}
}
