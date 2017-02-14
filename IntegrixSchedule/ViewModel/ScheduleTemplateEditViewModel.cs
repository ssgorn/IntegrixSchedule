using IntegrixSchedule.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class ScheduleTemplateEditViewModel : BaseViewModel
	{
		private readonly ScheduleTemplateWindow _scheduleTemplateWindow;

		private Guid _idOrganization;

		public ScheduleTemplateEditViewModel(ScheduleTemplateWindow mainView, Guid idOrganization)
		{
			_idOrganization = idOrganization;
			_scheduleTemplateWindow = mainView;
			AddScheduleCommad = new DelegateCommand(AddScheduleAction);
			DeleteScheduleCommad = new DelegateCommand(DeleteScheduleAction);
			SaveScheduleCommad = new DelegateCommand(SaveScheduleAction);
		}

		public bool ShowDialog()
		{
			_scheduleTemplateWindow.DataContext = this;
			return _scheduleTemplateWindow.ShowDialog() ?? false;
		}

		public ICommand AddScheduleCommad { get; internal set; }

		public ICommand DeleteScheduleCommad { get; internal set; }

		public ICommand SaveScheduleCommad { get; internal set; }

		public ICommand RowEditEndingCommand { get; internal set; }

		public ICommand CellEditEndingCommand { get; internal set; }

		private ObservableCollection<ScheduleTemplate> _templatesList;
		public ObservableCollection<ScheduleTemplate> TemplatesList
		{
			get
			{
				if (_templatesList == null)
				{
					_templatesList = DBManager.Instance.SceduleTemplatesGet(_idOrganization);
					if (_templatesList.Count > 0)
						SelectedTemplate = _templatesList[0];
				}

				return _templatesList;
			}
			set
			{
				_templatesList = value;
				OnPropertyChanged("TemplatesList");
			}

		}

		private ScheduleTemplate _selectedTemplate;

		public ScheduleTemplate SelectedTemplate
		{
			get { return _selectedTemplate; }
			set
			{
				if (SelectedTemplate != null
					&& SelectedTemplate.State != States.Unchanged)
				{
					SaveScheduleAction(null);
				}
				_selectedTemplate = value;
				OnPropertyChanged("SelectedTemplate");
				OnPropertyChanged("TemplatesList");
			}
		}

		private DataGridCellInfo _cellInfo;
		public DataGridCellInfo CellInfo
		{
			get { return _cellInfo; }
			set
			{
				_cellInfo = value;
				OnPropertyChanged("CellInfo");
			}
		}

		protected void AddScheduleAction(object sender)
		{
			var sched = new ScheduleTemplate()
			{
				Id = Guid.NewGuid(),
				IdOrganization = _idOrganization,
				TimeStart = new DateTime(2000, 1, 1),
				TimeEnd = new DateTime(2000, 1, 1),
				RecipietTime = 15,
				IsWork1 = true,
				IsWork2 = true,
				IsWork3 = true,
				IsWork4 = true,
				IsWork5 = true,
				IsWork6 = true,
				IsWork7 = true
			};
			_templatesList.Add(sched);
			SelectedTemplate = sched;
			OnPropertyChanged("SelectedTemplate");
			OnPropertyChanged("TemplatesList");
		}

		protected void DeleteScheduleAction(object sender)
		{
			if (SelectedTemplate != null)
			{
				DBManager.Instance.DeleteSelectedTemplate(SelectedTemplate);
				_templatesList.Remove(SelectedTemplate);
				OnPropertyChanged("TemplatesList");
			}
		}

		protected void SaveScheduleAction(object sender)
		{
			try
			{
				if (SelectedTemplate != null)
				{
					if (SelectedTemplate.IsActual)
					{
						foreach (var source in _templatesList.Where(x => x.Id != SelectedTemplate.Id && x.IsActual))
						{
							source.IsActual = false;
						}
					}

					foreach (var templ in _templatesList.OrderBy(x => x.IsActual))
					{
						DBManager.Instance.SaveSelectedTemplate(templ);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			_templatesList = null;
			OnPropertyChanged("SelectedTemplate");
			OnPropertyChanged("TemplatesList");
		}
	}
}
