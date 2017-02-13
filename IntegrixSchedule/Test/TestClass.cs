using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.BisnessObjects;
using IntegrixSchedule.Model;
using IntegrixSchedule.ViewModel;

namespace IntegrixSchedule.Test
{
	public class TestClass
	{
		public static TestClass Instance = new TestClass();

		private MainViewViewModel _mmmv;

		public void CreateTestData(MainViewViewModel mmmv, bool clear = false)
		{
			_mmmv = mmmv;
			var con = new MainDBContext();

			DBManager.Instance.SetNewDBContext(con);
			if (clear)
			{
				var te = con.SceduleTemplates.ToList();
				con.SceduleTemplates.RemoveRange(te);
				con.SaveChanges();

				var dd = con.ScheduleDetails.ToList();
				con.ScheduleDetails.RemoveRange(dd);
				con.SaveChanges();

				var dh = con.ScheduleHeaders.ToList();
				con.ScheduleHeaders.RemoveRange(dh);
				con.SaveChanges();

				var or = con.Organizations.ToList();
				con.Organizations.RemoveRange(or);
				con.SaveChanges();
			}

			con.Organizations.Add(new Organization() { Id = Guid.NewGuid(), Name = "Организация 1" });
			con.Organizations.Add(new Organization() { Id = Guid.NewGuid(), Name = "Организация 2" });
			con.Organizations.Add(new Organization() { Id = Guid.NewGuid(), Name = "Организация 3" });
			con.SaveChanges();

			var orgs = con.Organizations.ToList();
			foreach (var org in orgs)
			{
				con.SceduleTemplates.Add(new ScheduleTemplate()
				{
					Id = Guid.NewGuid(),
					TimeStart = new DateTime(2000, 1, 1, 8, 0, 0),
					TimeEnd = new DateTime(2000, 1, 1, 18, 0, 0),
					IdOrganization = org.Id,
					RecipietTime = 15,
					BreakeStartTime = new DateTime(2000, 1, 1, 13, 0, 0),
					BreakeEndTime = new DateTime(2000, 1, 1, 14, 45, 0),
					IsActual = true,
					IsWork1 = true,
					IsWork2 = true,
					IsWork3 = true,
					IsWork4 = true,
					IsWork5 = true,
					IsWork6 = true,
					IsWork7 = true
				});
				con.SaveChanges();
			}


			var heads = new List<ScheduleHeader>();
			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[0].Id,
				DateStart = new DateTime(2017, 2, 1),
				DateEnd = new DateTime(2017, 2, 10)
			});
			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[0].Id,
				DateStart = new DateTime(2017, 2, 11),
				DateEnd = new DateTime(2017, 2, 20)
			});
			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[0].Id,
				DateStart = new DateTime(2017, 2, 21),
				DateEnd = new DateTime(2017, 2, 28)
			});

			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[1].Id,
				DateStart = new DateTime(2017, 1, 1),
				DateEnd = new DateTime(2017, 1, 5)
			});
			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[1].Id,
				DateStart = new DateTime(2017, 1, 8),
				DateEnd = new DateTime(2017, 1, 11)
			});


			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[2].Id,
				DateStart = new DateTime(2017, 1, 14),
				DateEnd = new DateTime(2017, 1, 15)
			});
			heads.Add(new ScheduleHeader()
			{
				Id = Guid.NewGuid(),
				IdOrganization = orgs[2].Id,
				DateStart = new DateTime(2017, 1, 1),
				DateEnd = new DateTime(2017, 1, 11)
			});

			foreach (var head in heads)
			{
				var res = ((IMainViewModelCalc)_mmmv).CreateSchedule(head.DateStart, head.DateEnd, head.IdOrganization);
				con.ScheduleHeaders.Add(res.Key);
				foreach (var det in res.Value)
				{
					con.ScheduleDetails.Add(det);
				}
				con.SaveChanges();
			}
		}
	}
}
