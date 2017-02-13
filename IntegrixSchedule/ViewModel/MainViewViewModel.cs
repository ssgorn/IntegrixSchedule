using IntegrixSchedule.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IntegrixSchedule.BisnessObjects;
using IntegrixSchedule.Common;
using IntegrixSchedule.Model;
using IntegrixSchedule.Test;

namespace IntegrixSchedule.ViewModel
{
	public class MainViewViewModel : BaseViewModel, IMainViewModelCalc
	{
		#region Ctr

		public MainViewViewModel(MainView mainView)
		{
			_mainView = mainView;
			EditScheduleCommad = new DelegateCommand(EditScheduleAction);
			CreateScheduleCommad = new DelegateCommand(CreateScheduleAction);
			TestCommad = new DelegateCommand(TestAction);
		}

		#endregion Ctr

		#region Команды

		public ICommand TestCommad { get; internal set; }

		public ICommand EditScheduleCommad { get; internal set; }

		public ICommand CreateScheduleCommad { get; internal set; }

		#endregion Команды

		#region Свойтсва

		private readonly MainView _mainView;

		private bool _isLoading = false;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				OnPropertyChanged("IsNoLoading");
				OnPropertyChanged("IsLoading");

			}
		}

		public bool IsNoLoading => !IsLoading;

		private ObservableCollection<Organization> _organizations;
		public ObservableCollection<Organization> Organizations
		{
			get
			{
				if (_organizations == null)
				{
					_organizations = DBManager.Instance.OrganizationsGet();
					if (_organizations.Count > 0)
						SelectedOrganization = _organizations[0];
				}

				return _organizations;
			}
			set
			{
				_organizations = value;
				OnPropertyChanged("Organizations");
			}

		}

		private Organization _selectedOrganization;

		public Organization SelectedOrganization
		{
			get { return _selectedOrganization; }
			set
			{
				_selectedOrganization = value;
				OnPropertyChanged("SelectedOrganization");
				OnPropertyChanged("ScheduleHeaders");
				OnPropertyChanged("ScheduleDetailHeaders");
			}
		}

		private IList<ScheduleHeader> _scheduleHeaders;

		public IList<ScheduleHeader> ScheduleHeaders
		{
			get
			{
				if (_scheduleHeaders == null || !_scheduleHeaders.Any(x => x.IdOrganization == SelectedOrganization?.Id))
				{
					_scheduleHeaders = DBManager.Instance.ScheduleHeadersGet(SelectedOrganization?.Id);
					if (_scheduleHeaders.Count > 0)
						SelectedScheduleHeader = _scheduleHeaders[0];
				}

				return _scheduleHeaders.OrderBy(x => x.DateStart).ToList();
			}
			set
			{
				_scheduleHeaders = value;
				OnPropertyChanged("ScheduleHeaders");
			}
		}

		private ScheduleHeader _selectedScheduleHeader;
		public ScheduleHeader SelectedScheduleHeader
		{
			get { return _selectedScheduleHeader; }
			set
			{
				_selectedScheduleHeader = value;
				OnPropertyChanged("SelectedScheduleHeader");
				OnPropertyChanged("ScheduleDetailHeaders");
			}
		}

		private IList<ScheduleDetailHeaderData> _scheduleDetailHeaders;
		public IList<ScheduleDetailHeaderData> ScheduleDetailHeaders
		{
			get
			{
				if (_scheduleDetailHeaders == null || !_scheduleDetailHeaders.Any(x => x.IdScheduleHeader == SelectedScheduleHeader?.Id))
					_scheduleDetailHeaders = DBManager.Instance.ScheduleDetailsGet(SelectedScheduleHeader?.Id);

				return _scheduleDetailHeaders.OrderBy(x => x.Date).ToList();
			}
			set
			{
				_scheduleDetailHeaders = value;
				OnPropertyChanged("ScheduleDetailHeaders");
			}
		}

		#endregion Свойтсва

		#region Методы

		public void Show()
		{
			_mainView.DataContext = this;
			_mainView.Show();
		}

		protected void EditScheduleAction(object sender)
		{
			using (var vm = new ScheduleTemplateEditViewModel(new ScheduleTemplateWindow(), SelectedOrganization.Id))
			{
				if (vm.ShowDialog())
				{

				}
			}
		}

		protected void CreateScheduleAction(object sender)
		{
			if (SelectedOrganization == null)
			{
				MessageBox.Show("Нельзя создать расписание пока не выбрана организация");
				return;
			}

			var templ = DBManager.Instance.GetActualScheduleTemplate(SelectedOrganization.Id);
			if (templ == null)
			{
				MessageBox.Show("Нельзя создать расписание, т.к. для данной организации не создано актуального шаблона расписаний");
				return;
			}

			using (var vm = new SelectDatesViewModel(new SelectDatesWindow(), DateTime.Now.Date, DateTime.Now.Date.AddDays(10)))
			{
				if (vm.ShowDialog())
				{
					using (var bw = new BackgroundWorker())
					{
						var ds = vm.DateStart;
						var de = vm.DateEnd;
						bw.DoWork += (o, args) =>
						{
							var res = ((IMainViewModelCalc)this).CreateSchedule(ds, de, SelectedOrganization.Id);
							DBManager.Instance.SaveScheduleHeader(res.Key);
							DBManager.Instance.SaveScheduleDetails(res.Value);
						};
						bw.RunWorkerCompleted += (o, args) =>
						{
							IsLoading = false;
							_scheduleHeaders = null;
							OnPropertyChanged("SelectedOrganization");
							OnPropertyChanged("ScheduleHeaders");
							OnPropertyChanged("ScheduleDetailHeaders");
						};
						IsLoading = true;
						bw.RunWorkerAsync();
					}
				}
			}
		}

		protected void TestAction(object sender)
		{
			using (var bw = new BackgroundWorker())
			{
				bw.DoWork += (o, args) =>
				{
					TestClass.Instance.CreateTestData(this, true);
				};
				bw.RunWorkerCompleted += (o, args) =>
				{
					IsLoading = false;
					_organizations = null;
					_scheduleHeaders = null;
					_scheduleDetailHeaders = null;
					_selectedOrganization = null;

					OnPropertyChanged("Organizations");
					if (Organizations.Count > 0)
						SelectedOrganization = Organizations[0];
				};
				IsLoading = true;
				bw.RunWorkerAsync();
			}
		}

		#endregion Методы

		#region IMainViewModelCalc

		KeyValuePair<ScheduleHeader, List<ScheduleDetail>> IMainViewModelCalc.CreateSchedule(DateTime dateStart, DateTime dateEnd, Guid idOrganization)
		{
			var res = new ScheduleHeader() { Id = Guid.NewGuid(), DateStart = dateStart.Date, DateEnd = dateEnd.Date, IdOrganization = idOrganization };
			var det = new List<ScheduleDetail>();

			var templ = DBManager.Instance.GetActualScheduleTemplate(idOrganization);
			if (templ == null)
				throw new Exception("Для данной организации не найден шаблон расписания");

			var days = (dateEnd - dateStart).Days;

			var dat = new DateTime(res.DateStart.Year, res.DateStart.Month, res.DateStart.Day, templ.TimeStart.Hour, templ.TimeStart.Minute, 0);
			for (int i = 0; i <= days; i++)
			{
				if ((dat.DayOfWeek == DayOfWeek.Monday && !templ.IsWork1)
				    || (dat.DayOfWeek == DayOfWeek.Thursday && !templ.IsWork2)
				    || (dat.DayOfWeek == DayOfWeek.Wednesday && !templ.IsWork3)
				    || (dat.DayOfWeek == DayOfWeek.Thursday && !templ.IsWork4)
				    || (dat.DayOfWeek == DayOfWeek.Friday && !templ.IsWork5)
				    || (dat.DayOfWeek == DayOfWeek.Saturday && !templ.IsWork6)
				    || (dat.DayOfWeek == DayOfWeek.Sunday && !templ.IsWork7))
				{
					dat = new DateTime(dat.AddDays(1).Year, dat.AddDays(1).Month, dat.AddDays(1).Day, templ.TimeStart.Hour, templ.TimeStart.Minute, 0);
					continue;
				}

				var datE = dat.AddMinutes(templ.RecipietTime);
				while (datE.TimeOfDay <= templ.TimeEnd.TimeOfDay)
				{
					bool isBreak = false;
					if (templ.BreakeStartTime != null && dat.TimeOfDay >= templ.BreakeStartTime?.TimeOfDay && dat.TimeOfDay < templ.BreakeEndTime?.TimeOfDay)
					{
						dat = new DateTime(dat.Year, dat.Month, dat.Day, templ.BreakeStartTime.Value.Hour, templ.BreakeStartTime.Value.Minute, 0);
						datE = new DateTime(dat.Year, dat.Month, dat.Day, templ.BreakeEndTime.Value.Hour, templ.BreakeEndTime.Value.Minute, 0);
						isBreak = true;
					}
					det.Add(new ScheduleDetail()
					{
						Id = Guid.NewGuid(),
						TimeStart = dat,
						TimeEnd = datE,
						IsBreak = isBreak,
						IdScheduleHeader = res.Id
					});

					dat = datE;
					datE = dat.AddMinutes(templ.RecipietTime);
				}
				dat = new DateTime(dat.AddDays(1).Year, dat.AddDays(1).Month, dat.AddDays(1).Day, templ.TimeStart.Hour, templ.TimeStart.Minute, 0);
			}

			return new KeyValuePair<ScheduleHeader, List<ScheduleDetail>>(res, det);
		}

		#endregion IMainViewModelCalc
	}
}
