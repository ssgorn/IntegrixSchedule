using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IntegrixSchedule.BisnessObjects;
using IntegrixSchedule.Model;
using IntegrixSchedule.Test;
using IntegrixSchedule.View;
using IntegrixSchedule.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrixScheduleTests
{
	[TestClass]
	public class TestMainViewViewModel
	{
		private static Guid _idOrganization = Guid.NewGuid();

		[TestMethod]
		public void CreateScheduleTest()
		{
			var mmmv = new MainViewViewModel(new MainView());
			var ds = new DateTime(2017, 2, 1);
			var de = new DateTime(2017, 2, 10);

			DBManager.Instance = new MoqManager();

			var res = ((IMainViewModelCalc) mmmv).CreateSchedule(ds, de, _idOrganization);
			Assert.IsTrue(res.Value.Min(x => x.TimeStart).Date == res.Key.DateStart.Date);
		}

		private class MoqManager : IDBManager
		{
			public ScheduleTemplate GetActualScheduleTemplate(Guid idOrganization)
			{
				return new ScheduleTemplate()
				{
					Id = Guid.NewGuid(), IsActual = true, IdOrganization = _idOrganization, TimeStart = new DateTime(2000, 1, 1, 10, 0, 0),
					TimeEnd = new DateTime(2000, 1, 1, 14, 0, 0), RecipietTime = 30, BreakeStartTime = new DateTime(2000, 1, 1, 11, 0, 0),
					BreakeEndTime = new DateTime(2000, 1, 1, 11, 35, 0), IsWork1 = true, IsWork2 = true, IsWork3 = true, IsWork4 = true,
					IsWork5 = true, IsWork6 = true
				};
			}

			public void SetNewDBContext(IMainDBContext context)
			{
				throw new NotImplementedException();
			}

			public ObservableCollection<Organization> OrganizationsGet(bool force = false)
			{
				throw new NotImplementedException();
			}

			public ObservableCollection<ScheduleHeader> ScheduleHeadersGet(Guid? idOrganization)
			{
				throw new NotImplementedException();
			}

			public ScheduleDetailsCollection ScheduleDetailsGet(Guid? idHeader)
			{
				throw new NotImplementedException();
			}

			public void SaveScheduleHeader(ScheduleHeader hed)
			{
				throw new NotImplementedException();
			}

			public void SaveScheduleDetails(List<ScheduleDetail> dets)
			{
				throw new NotImplementedException();
			}

			public ObservableCollection<ScheduleTemplate> SceduleTemplatesGet(Guid idOrganization)
			{
				throw new NotImplementedException();
			}

			public ScheduleTemplate DeleteSelectedTemplate(ScheduleTemplate templ)
			{
				throw new NotImplementedException();
			}

			public void SaveSelectedTemplate(ScheduleTemplate templ)
			{
				throw new NotImplementedException();
			}
		}
	}
}
